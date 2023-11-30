using Sharp7;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
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
        private record newRecord = new record();


        private byte[] BufferDB = new byte[0];
        int AmountDB = MaxBuffer.DB;
        private byte[] BufferDB4 = new byte[0];
        int AmountDB4 = 0;        
        private byte[] BufferDB29 = new byte[0];
        int AmountDB29 = 0;        
        private byte[] BufferDB39 = new byte[0];
        int AmountDB39 = 0;
        private byte[] BufferMK = new byte[0];
        int AmountMK = 0;

        private dlgLoguj Loguj;
        private dlgShowResultInfo ShowResultInfo;
        private dlgZoznamdoGUI dlgZoznamdoGUI;
        private dlgZaznamDoGUI dlgZaznamDoGUI;
        private dlgStrucnyVypisGUI dlgStrucnyVypisGUI;

        private S7Client Client;
        private S7MultiVar Writer;        
        
        private System.Timers.Timer tmrRead;
        private System.Timers.Timer tmrError;


        public int ErrorRead { get; private set; }
        public bool SPRACOVAVAM { get; private set; }

        public string CUR_VALUE_BUF_DB { get { return BitConverter.ToString(BufferDB); } }
        public string CUR_VALUE_BUF_MK { get { return Helpers.OdrezAParsuj(System.Text.Encoding.UTF8.GetString(BufferMK)); } }


        public Pec(string pPecID, string pIP_PLC, int pRack, int pSlot, 
            int pAmountDB,int pAmountDb4, int pAmountDb29, int pAmountDb39, int pAmountMK, 
            dlgLoguj pDlgLoguj, dlgZoznamdoGUI pdlgZoznamdoGUI, dlgShowResultInfo pDlgShowResultInfo, dlgStrucnyVypisGUI pdlgStrucnyVypisGUI,dlgZaznamDoGUI pdlgZaznamDoGUI)
        {
            this.PEC_ID = pPecID;
            this.IP_PLC = pIP_PLC;
            this.Rack = pRack;
            this.Slot = pSlot;

            this.AmountDB = pAmountDB;
            this.AmountDB4 = pAmountDb4;
            this.AmountDB29 = pAmountDb29;
            this.AmountDB39 = pAmountDb39;
            this.AmountMK = pAmountMK;
            


            this.BufferDB = new byte[AmountDB];
            this.BufferDB4 = new byte[AmountDB4];
            this.BufferDB29 = new byte[AmountDB29];
            this.BufferDB39 = new byte[AmountDB39];
            this.BufferMK = new byte[AmountMK];

            this.Client = new S7Client();
            this.Writer = new S7MultiVar(Client);

            this.Loguj = pDlgLoguj;
            this.dlgZoznamdoGUI = pdlgZoznamdoGUI;
            this.ShowResultInfo = pDlgShowResultInfo;
            this.dlgStrucnyVypisGUI = pdlgStrucnyVypisGUI;
            this.dlgZaznamDoGUI = pdlgZaznamDoGUI;

            tmrError = new System.Timers.Timer(MainForm.ErrInterval);
            tmrRead = new System.Timers.Timer(MainForm.ReadInterval);
            tmrRead.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            tmrError.Elapsed += new ElapsedEventHandler(PripojKPLC);

            this.ADDRESSES = new Addresses(this.PEC_ID);
            //this.AmountMK = this.ADDRESSES.NAPATIE.AMOUNT_MAX;
            //this.AmountDB = this.ADDRESSES.TEPLOTA_VODY_VSTUP.AMOUNT_MAX;
            //this.AmountDB4 = this.ADDRESSES.FREKVENCIA.AMOUNT_MAX;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
            this.ReadArea();
        }

        private void ReadArea()
        {
            int ResultDB = 0, ResultDB4 = 0, ResultDB29 = 0, ResultDB39 = 0, ResultMK = 0;  
            int SizeReadDB = 0, SizeReadDB4 = 0, SizeReadDB29 = 0 , SizeReadDB39 = 0, SizeReadMK = 0;

            //#################################################################################################################            
            if (AmountDB != 0)
                ResultDB = Client.ReadArea(S7Consts.S7AreaDB, 001, 0, this.AmountDB, S7Consts.S7WLByte, BufferDB, ref SizeReadDB);
            if (AmountDB4 != 0)
                ResultDB4 = Client.ReadArea(S7Consts.S7AreaDB, 004, 0, this.AmountDB4, S7Consts.S7WLByte, BufferDB4, ref SizeReadDB4);
            if (AmountDB29 != 0)
                ResultDB29 = Client.ReadArea(S7Consts.S7AreaDB, 029, 0, this.AmountDB29, S7Consts.S7WLByte, BufferDB29, ref SizeReadDB29);
            if (AmountDB39 != 0)
                ResultDB39 = Client.ReadArea(S7Consts.S7AreaDB, 039, 0, this.AmountDB39, S7Consts.S7WLByte, BufferDB39, ref SizeReadDB39);
            if (AmountMK != 0)
                ResultMK = Client.ReadArea(S7Consts.S7AreaMK, 000, 0, this.AmountMK, S7Consts.S7WLByte, BufferMK, ref SizeReadMK);                   
            //#################################################################################################################

            //ak je resut OK (0) vypise do logu OK, ak nie je OK vypisem nieco ine
            this.ShowResultInfo(this.PEC_ID, ResultDB + ResultMK, Client.ErrorText(ResultDB) + " / "+Client.ErrorText(ResultMK), Client.ExecutionTime);

            if (ResultDB == 0 && ResultMK == 0 && ResultDB4 == 0 && ResultDB29 == 0 && ResultDB39 == 0)
            {
                ErrorRead = 0;
                        
                dlgStrucnyVypisGUI(
                    this.PEC_ID + " MK:" + this.CUR_VALUE_BUF_DB.Trim(),
                    "DB:" + SizeReadDB.ToString() + " / " + "DB4:" + SizeReadDB4.ToString() + " / " + "DB29:" + SizeReadDB29.ToString() + " / "+ "DB39:" + SizeReadDB39.ToString() + " / " + "MK:" + SizeReadMK.ToString());              
                
                this.SpracujZaznamDB();              
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
            
            
            if (!DB.UKLADA_SA)
            {
                DB.UKLADA_SA = true;
                //if (DB.Context.records.Count() > 0)
                //{
                    int id_last = DB.Context.records.Max(rec => rec.id);
                    List<record> vypisovany_zoznam = DB.Context.records.Where(r => r.id > id_last - MainForm.POCET_ZAZNAMOV).ToList();
                    vypisovany_zoznam.Reverse();
                    //vlkno timera volajuce metody z MainFormu na vypis hodnot
                    dlgZoznamdoGUI(vypisovany_zoznam); //DB.Context.reports.Where(r=>r.ID > id_last- 10).ToList().Reverse();                    
                //}
                DB.UKLADA_SA = false;
            }
        }

        private void SpracujZaznamDB()
        {
            //record newRecord  = new record();
            //newRecord.id = 0;
            //DateTime DATE_TIME = DateTime.Now;
            newRecord.date_time = DateTime.Now;

            //float? NAPATIE;
            if (this.PEC_ID == "PEC_H")
               newRecord.napatie = this.ADDRESSES.NAPATIE?.getVaue(this.BufferDB29);
            else
                newRecord.napatie = this.ADDRESSES.NAPATIE?.getVaue(this.BufferMK);

            //float? PRUD;
            if (this.PEC_ID == "PEC_H")
                 newRecord.prud = this.ADDRESSES.PRUD?.getVaue(this.BufferDB29);
            else
                newRecord.prud = this.ADDRESSES.PRUD?.getVaue(this.BufferMK);

            newRecord.sobert_vstup = this.ADDRESSES.SOBERT_VSTUP?.getVaue(this.BufferMK);
            newRecord.sobert_vykon = this.ADDRESSES.SOBERT_VYKON?.getVaue(this.BufferMK);

            //float? VYKON;
            if (this.PEC_ID == "PEC_H")
                newRecord.vykon = this.ADDRESSES.VYKON?.getVaue(this.BufferDB29);
            else
                newRecord.vykon = this.ADDRESSES.VYKON?.getVaue(this.BufferMK);

            //float? PRISPOSOBENIE;
            if (this.PEC_ID == "PEC_H")
                newRecord.rz_pribenie = this.ADDRESSES.PRISPOSOBENIE?.getVaue(this.BufferDB29);
            else
                newRecord.rz_pribenie = this.ADDRESSES.PRISPOSOBENIE?.getVaue(this.BufferMK);

            newRecord.tlak = this.ADDRESSES.TLAK_VODY?.getVaue(this.BufferDB);

            //float? TEPLOTA_VODY_VSTUP;
            if (this.PEC_ID == "PEC_H")
                newRecord.t_voda_vstup= this.ADDRESSES.TEPLOTA_VODY_VSTUP?.getVaue(this.BufferDB39);
            else
                newRecord.t_voda_vstup = this.ADDRESSES.TEPLOTA_VODY_VSTUP?.getVaue(this.BufferDB);


            //float? TEPLOTA_VODY_VYSTUP;
            if (this.PEC_ID == "PEC_H")
                newRecord.t_voda_vystup = this.ADDRESSES.TEPLOTA_VODY_VYSTUP?.getVaue(this.BufferDB39);
            else
                newRecord.t_voda_vystup = this.ADDRESSES.TEPLOTA_VODY_VYSTUP?.getVaue(this.BufferDB);

            //float? TEPLOTA_P1;
            if (this.PEC_ID == "PEC_H")
                newRecord.teplota_p1 = this.ADDRESSES.TEPLOTA_VODY_P1?.getVaue(this.BufferDB39);
            else
                newRecord.teplota_p1 = this.ADDRESSES.TEPLOTA_VODY_P1?.getVaue(this.BufferDB);

            //float? TEPLOTA_P2;
            if (this.PEC_ID == "PEC_H")
                newRecord.teplota_p2 = this.ADDRESSES.TEPLOTA_VODY_P2?.getVaue(this.BufferDB39);
            else
                newRecord.teplota_p2 = this.ADDRESSES.TEPLOTA_VODY_P2?.getVaue(this.BufferDB);

            //float? PRIETOK;
            if (this.PEC_ID == "PEC_H")
                newRecord.prietok_vody = this.ADDRESSES.PRIETOK_VODY?.getVaue(this.BufferDB39);
            else
                newRecord.prietok_vody = this.ADDRESSES.PRIETOK_VODY?.getVaue(this.BufferDB);

            //float? FREKVENCIA;
            if (this.PEC_ID == "PEC_H")
                newRecord.frekvencia = this.ADDRESSES.FREKVENCIA?.getVaue(this.BufferDB29);
            else
                newRecord.frekvencia = this.ADDRESSES.FREKVENCIA?.getVaue(this.BufferDB4);

            newRecord.teplota_okruh = this.ADDRESSES.TEPLOTA_PRIMARNY_OKRUH?.getVaue(this.BufferDB39);
            //float? teplota_p1 = TEPLOTA_P1.HasValue ? (float?)Math.Round((float)TEPLOTA_P1, 2) : null;
            //float? teplota_p2 = TEPLOTA_P2.HasValue ? (float?)Math.Round((float)TEPLOTA_P2, 2) : null;
            
            newRecord.pec_id = this.PEC_ID;
            newRecord.napatie = newRecord.napatie.HasValue ? (float?)Math.Round((float)newRecord.napatie, 2) : null;
            newRecord.prud = newRecord.prud.HasValue ? (float?)Math.Round((float)newRecord.prud, 2) : null;
            newRecord.sobert_vstup = newRecord.sobert_vstup.HasValue ? (float?)Math.Round((float)newRecord.sobert_vstup, 2) : null;
            newRecord.sobert_vykon = newRecord.sobert_vykon.HasValue ? (float?)Math.Round((float)newRecord.sobert_vykon, 2) : null;
            newRecord.t_voda_vstup = newRecord.t_voda_vstup.HasValue ? (float?)Math.Round((float)newRecord.t_voda_vstup, 2) : null;
            newRecord.t_voda_vystup = newRecord.t_voda_vystup.HasValue ? (float?)Math.Round((float)newRecord.t_voda_vystup, 2) : null;
            newRecord.tlak = newRecord.tlak.HasValue ? (float?)Math.Round((float)newRecord.tlak, 2) : null;
            newRecord.rz_pribenie = newRecord.rz_pribenie.HasValue ? (float?)Math.Round((float)newRecord.rz_pribenie, 2) : null;
            newRecord.vykon = newRecord.vykon.HasValue ? (float?)Math.Round((float)newRecord.vykon, 2) : null;

            newRecord.teplota_p1 = newRecord.teplota_p1.HasValue ? (float?)Math.Round((float)newRecord.teplota_p1, 2) : null;
            newRecord.teplota_p2 = newRecord.teplota_p2.HasValue ? (float?)Math.Round((float)newRecord.teplota_p2, 2) : null;
            newRecord.prietok_vody = newRecord.prietok_vody.HasValue ? (float?)Math.Round((float)newRecord.prietok_vody, 2) : null;
            newRecord.frekvencia = newRecord.frekvencia.HasValue ? (float?)Math.Round((float)newRecord.frekvencia, 2) : null;
            newRecord.teplota_okruh = newRecord.teplota_okruh.HasValue ? (float?)Math.Round((float)newRecord.teplota_okruh, 2) : null;

            newRecord.zmena = Helpers.dajZmenu(newRecord.date_time);



            /*
            record UkladanyZaznam = new record()
            {

                pec_id = this.PEC_ID,
                date_time = DATE_TIME,

                napatie = NAPATIE.HasValue ? (float?)Math.Round((float)NAPATIE, 2) : null,
                prud = PRUD.HasValue ? (float?)Math.Round((float)PRUD, 2) : null,
                sobert_vstup = SOBERT_VSTUP.HasValue ? (float?)Math.Round((float)SOBERT_VSTUP, 2) : null,
                sobert_vykon = SOBERT_VYKON.HasValue ? (float?)Math.Round((float)SOBERT_VYKON, 2) : null,
                t_voda_vstup = TEPLOTA_VODY_VSTUP.HasValue ? (float?)Math.Round((float)TEPLOTA_VODY_VSTUP, 2) : null,
                t_voda_vystup = TEPLOTA_VODY_VYSTUP.HasValue ? (float?)Math.Round((float)TEPLOTA_VODY_VYSTUP, 2) : null,
                tlak = TLAK_VODY.HasValue ? (float?)Math.Round((float)TLAK_VODY, 2) : null,
                rz_pribenie = PRISPOSOBENIE.HasValue ? (float?)Math.Round((float)PRISPOSOBENIE, 2) : null,
                vykon = VYKON.HasValue ? (float?)Math.Round((float)VYKON, 2) : null,

                teplota_p1 = TEPLOTA_P1.HasValue ? (float?)Math.Round((float)TEPLOTA_P1, 2) : null,
                teplota_p2 = TEPLOTA_P2.HasValue ? (float?)Math.Round((float)TEPLOTA_P2, 2) : null,
                prietok_vody = PRIETOK.HasValue ? (float?)Math.Round((float)PRIETOK, 2) : null,
                frekvencia = FREKVENCIA.HasValue ? (float?)Math.Round((float)FREKVENCIA, 2) : null,
                teplota_okruh = TEPLOTA_OKRUH.HasValue ? (float?)Math.Round((float)TEPLOTA_OKRUH, 2) : null,

                zmena = Helpers.dajZmenu(DATE_TIME)*/



                /*
                pec_id = this.PEC_ID,
                date_time = DATE_TIME,

                napatie = NAPATIE.HasValue ? (float?)Math.Round((float)NAPATIE, 2) : null,
                prud = PRUD.HasValue ? (float?)Math.Round((float)PRUD, 2) : null,
                sobert_vstup = SOBERT_VSTUP.HasValue ? (float?)Math.Round((float)SOBERT_VSTUP, 2) : null,
                sobert_vykon = SOBERT_VYKON.HasValue ? (float?)Math.Round((float)SOBERT_VYKON, 2) : null,
                t_voda_vstup = TEPLOTA_VODY_VSTUP.HasValue ? (float?)Math.Round((float)TEPLOTA_VODY_VSTUP, 2) : null,
                t_voda_vystup = TEPLOTA_VODY_VYSTUP.HasValue ? (float?)Math.Round((float)TEPLOTA_VODY_VYSTUP, 2) : null,
                tlak = TLAK_VODY.HasValue ? (float?)Math.Round((float)TLAK_VODY, 2) : null,
                rz_pribenie = PRISPOSOBENIE.HasValue ? (float?)Math.Round((float)PRISPOSOBENIE, 2) : null,
                vykon = VYKON.HasValue ? (float?)Math.Round((float)VYKON, 2) : null,

                teplota_p1 = TEPLOTA_P1.HasValue ? (float?)Math.Round((float)TEPLOTA_P1, 2) : null,
                teplota_p2 = TEPLOTA_P2.HasValue ? (float?)Math.Round((float)TEPLOTA_P2, 2) : null,
                prietok_vody = PRIETOK.HasValue ? (float?)Math.Round((float)PRIETOK, 2) : null,
                frekvencia = FREKVENCIA.HasValue ? (float?)Math.Round((float)FREKVENCIA, 2) : null,
                teplota_okruh = TEPLOTA_OKRUH.HasValue ? (float?)Math.Round((float)TEPLOTA_OKRUH, 2) : null,

                zmena = Helpers.dajZmenu(DATE_TIME)
            };*/


            try {                
                //DB.ulozDoDB(UkladanyZaznam);
                DB.ulozDoDB(newRecord);
            }            
            catch (System.InvalidOperationException)
            {                
                this.Loguj("########################################################", MessageBoxIcon.Warning);
                this.Loguj(this.PEC_ID + ": SNAHA OPATOVNE UKALDANIE ZAZNAMU DO DB PO CHYBE UKLADANIA!", MessageBoxIcon.Warning);
                this.Loguj("########################################################", MessageBoxIcon.Warning);
                //DB.ulozDoDB(UkladanyZaznam);
                tmrRead.Enabled = false;
                Thread.Sleep(300);
                tmrRead.Enabled = true;
            };


            try
            {
                if (DB.UlozDB())
                {
                    //aktualizujDtg();
                    this.dlgZaznamDoGUI(newRecord);
                    this.Loguj(this.PEC_ID + ": Zaznam ulozeny do DB!", MessageBoxIcon.Information);
                }
                else
                {
                    this.Loguj("### ### ### ### ### ###", MessageBoxIcon.Error);
                    this.Loguj(this.PEC_ID + " Zaznam sa nepodarilo ulozit do DB!", MessageBoxIcon.Error);
                    this.Loguj("### ### ### ### ### ###", MessageBoxIcon.Error);
                    tmrRead.Enabled = false;
                    int slp = (int)(Helpers.RDM.NextDouble() * 5000);
                    Thread.Sleep(slp);
                    tmrRead.Enabled = true;
                }
            }
            catch (UpdateException ex)
            {
                newRecord.id++;
                MessageBox.Show("DODATOCNE INKREMENTOVANE ID-ka","Warrning",MessageBoxButtons.OK);
            }
        }

        public void AktivujTimer() {
            tmrRead.Enabled = true;
        }

        public void DeaktivujTimer()
        {
            tmrRead.Enabled = false;
        }

    }
}
