using Sharp7;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace nsAspur
{    
    internal class Pec
    {

        public string PEC_ID = "NEINICIALIZOVANE";
        public int Slot = 2;
        public int Rack = 0;
        public string IP_PLC = "NEINICIALIZOVANE";

        private Addresses ADDRESSES;

        private byte[] BufferDB = new byte[MaxBuffer.DB];
        int AmountDB = MaxBuffer.DB;
        private byte[] BufferMK = new byte[MaxBuffer.MK];
        int AmountMK = MaxBuffer.MK;

        private dlgLoguj Loguj;
        private dlgShowResultInfo ShowResultInfo;
        private dlgZoznamdoGUI dlgZoznamdoGUI;
        private dlgStrucnyVypisGUI dlgStrucnyVypisGUI;

        private S7Client Client;
        private S7MultiVar Writer;        
        
        private System.Timers.Timer tmrRead;
        private System.Timers.Timer tmrError;


        public int ErrorRead { get; private set; }
        public bool SPRACOVAVAM { get; private set; }

        public string CUR_VALUE_BUF_DB { get { return BitConverter.ToString(BufferDB); } }
        public string CUR_VALUE_BUF_MK { get { return Helpers.OdrezAParsuj(System.Text.Encoding.UTF8.GetString(BufferMK)); } }


        public Pec(string pPecID, string pIP_PLC, int pRack, int pSlot, dlgLoguj pDlgLoguj, dlgZoznamdoGUI pdlgZoznamdoGUI, dlgShowResultInfo pDlgShowResultInfo, dlgStrucnyVypisGUI pdlgStrucnyVypisGUI)
        {
            this.PEC_ID = pPecID;
            this.IP_PLC = pIP_PLC;
            this.Rack = pRack;
            this.Slot = pSlot;

            this.Client = new S7Client();
            this.Writer = new S7MultiVar(Client);

            this.Loguj = pDlgLoguj;
            this.dlgZoznamdoGUI = pdlgZoznamdoGUI;
            this.ShowResultInfo = pDlgShowResultInfo;
            this.dlgStrucnyVypisGUI = pdlgStrucnyVypisGUI;
            
            tmrError = new System.Timers.Timer(MainForm.ErrInterval);
            tmrRead = new System.Timers.Timer(MainForm.ReadInterval);
            tmrRead.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            tmrError.Elapsed += new ElapsedEventHandler(PripojKPLC);

            this.ADDRESSES = new Addresses(this.PEC_ID);
            this.AmountMK = this.ADDRESSES.NAPATIE.AMOUNT_MAX;
            this.AmountDB = this.ADDRESSES.TLAK_VODY.AMOUNT_MAX;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
            this.ReadArea();
        }



        private void ReadArea()
        {
            int ResultMK, ResultDB = 0;
            int SizeReadDB = 0, SizeReadMK = 0;

            //#################################################################################################################
                ResultDB = Client.ReadArea(S7Consts.S7AreaDB, 001, 0, this.AmountDB, S7Consts.S7WLByte, BufferDB, ref SizeReadDB);
                ResultMK = Client.ReadArea(S7Consts.S7AreaMK, 000, 0, this.AmountMK, S7Consts.S7WLByte, BufferMK, ref SizeReadMK);                   
            //#################################################################################################################

            //ak je resut OK (0) vypise do logu OK, ak nie je OK vypisem nieco ine
            this.ShowResultInfo(ResultDB + ResultMK,Client.ErrorText(ResultMK) + " / "+Client.ErrorText(ResultDB), Client.ExecutionTime);

            if (ResultDB == 0 && ResultMK == 0)
            {
                ErrorRead = 0;
                //lbPrecHodnotaRaw.Text = "DB:" + this.CUR_VALUE_BUF_DB.Trim() + " /  " + "MK:" + this.CUR_VALUE_BUF_MK.Trim(); //vypisem orezany buffer                     
                //lbBytesRead.Text = "DB:" + SizeReadDB.ToString() + " / " + "MK:" + SizeReadMK.ToString();  //kolko bytov bolo precitanych              
                dlgStrucnyVypisGUI(
                    this.PEC_ID + " DB:" + this.CUR_VALUE_BUF_DB.Trim() + " /  " + "MK:" + this.CUR_VALUE_BUF_MK.Trim(),
                    "DB:" + SizeReadDB.ToString() + " / " + "MK:" + SizeReadMK.ToString());

                SPRACOVAVAM = true;
                SpracujZaznamDB();
                SPRACOVAVAM = false;
            }
            else
            {
                this.ErrorRead++;
                this.Loguj(this.PEC_ID +": Chyba pri čítaní hodnoty z PLC", MessageBoxIcon.Error);

                //Thread.Sleep(this.ErrInterval);
                //po troch chybach citania sa odpajam
                if (ErrorRead > MainForm.MaxErrCount)
                {
                    
                    ErrorRead = 0;
                    odpojiť();
                    //kazdych 10 sekund sa snazi pripojit k plc
                    Thread.Sleep(MainForm.ErrInterval);
                    PripojKPLC(null, null);
                }
            }
        }


        public void PripojKPLC(Object myObject, EventArgs myEventArgs)
        {
            int Result = Client.ConnectTo(IP_PLC, this.Rack, this.Slot);

            if (Result == 0)
            {
                this.Loguj(this.PEC_ID + ": Connected", MessageBoxIcon.Information);
                //vypnem casovac snaziaci sa o spojenie kazdych 5 sekund
                if (tmrError.Enabled == true) tmrError.Enabled = false;
                //TextError.ForeColor = Color.Black;
                //TextError.Text = " PDU Negotiated : " + Client.PduSizeNegotiated.ToString();
                this.ReadArea();
                tmrRead.Enabled = true;
            }
            else
            {
                if (tmrRead.Enabled == true) tmrRead.Enabled = false;
                //v pripade chyby spustim druhy casovac nastaveny na periodu 5s a tri krat v intervale jednej sekundy sa pokusim pripojit
                if (tmrError.Enabled == false) tmrError.Enabled = true;

                this.Loguj(this.PEC_ID + ": Nepodarilo sa pripojiť k PLC!", MessageBoxIcon.Error);
            }
        }

        private void odpojiť()
        {
            Client.Disconnect();
            tmrRead.Enabled = false;
            //TextError.ForeColor = Color.Black;
            //TextError.Text = "Disconnected";

            this.Loguj(this.PEC_ID + ": Disconnected", MessageBoxIcon.Information);
        }


        void aktualizujDtg()
        {
            //if (!DB.UKLADA_SA)
           // {
             //   DB.UKLADA_SA = true;
                if (DB.Context.records.Count() > 0)
                {
                    int id_last = DB.Context.records.Max(rec => rec.id);
                    List<record> vypisovany_zoznam = DB.Context.records.Where(r => r.id > id_last - MainForm.POCET_ZAZNAMOV).ToList();
                    vypisovany_zoznam.Reverse();
                    //vlkno timera volajuce metody z MainFormu na vypis hodnot
                    dlgZoznamdoGUI(vypisovany_zoznam); //DB.Context.reports.Where(r=>r.ID > id_last- 10).ToList().Reverse();
                }
              //  DB.UKLADA_SA = false;
            //}
        }

        private void SpracujZaznamDB()
        {

            DateTime DATE_TIME = DateTime.Now;            

            float NAPATIE = this.ADDRESSES.NAPATIE.dlgPrepocet(this.ADDRESSES.NAPATIE.getVaue(this.BufferMK));
            float PRUD = this.ADDRESSES.PRUD.dlgPrepocet(this.ADDRESSES.PRUD.getVaue(this.BufferMK));
            float SOBERT_VSTUP = this.ADDRESSES.SOBERT_VSTUP.getVaue(this.BufferMK);
            float SOBERT_VYKON = this.ADDRESSES.SOBERT_VYKON.getVaue(this.BufferMK);
            float VYKON = this.ADDRESSES.VYKON.getVaue(this.BufferMK);
            float PRISPOSOBENIE = this.ADDRESSES.PRISPOSOBENIE.getVaue(this.BufferMK);

            float TLAK_VODY = this.ADDRESSES.TLAK_VODY.getVaue(this.BufferDB);
            float TEPLOTA_VODY_VSTUP = this.ADDRESSES.TEPLOTA_VODY_VSTUP.getVaue(this.BufferDB);
            float TEPLOTA_VODY_VYSTUP = this.ADDRESSES.TEPLOTA_VODY_VYSTUP.getVaue(this.BufferDB);

            DB.ulozDoDB(new record()
            {
                pec_id = this.PEC_ID,
                date_time = DATE_TIME,
                napatie = (float)Math.Round(NAPATIE, 2),
                prud = (float)Math.Round(PRUD, 2),
                sobert_vstup = (float)Math.Round(SOBERT_VSTUP, 2),
                sobert_vykon = (float)Math.Round(SOBERT_VYKON, 2),
                t_voda_vstup = (float)Math.Round(TEPLOTA_VODY_VSTUP, 2),
                t_voda_vystup = (float)Math.Round(TEPLOTA_VODY_VYSTUP, 2),
                tlak = (float)Math.Round(TLAK_VODY, 2),
                rz_pribenie = (float)Math.Round(PRISPOSOBENIE, 2),
                vykon = (float)Math.Round(VYKON, 2),
                zmena = Helpers.dajZmenu(DATE_TIME)
            }) ; 


            if (DB.UlozDB())
            {
                aktualizujDtg();
                this.Loguj(this.PEC_ID + ": Zaznam ulozeny do DB!", MessageBoxIcon.Information);
            }
            else
            {
                this.Loguj("### ### ### ### ### ###", MessageBoxIcon.Error);
                this.Loguj(this.PEC_ID + " Zaznam sa nepodarilo ulozit do DB!", MessageBoxIcon.Error);
                this.Loguj("### ### ### ### ### ###", MessageBoxIcon.Error);
                tmrRead.Enabled = false;
                int slp = (int)(Helpers.RDM.NextDouble()* 1000);
                Thread.Sleep(slp);
                tmrRead.Enabled = true;
            }
        }


    }
}
