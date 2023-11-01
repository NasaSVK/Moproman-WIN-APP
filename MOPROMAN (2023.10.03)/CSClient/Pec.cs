using Sharp7;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace nsAspur
{
    internal class Pec
    {

        public string PEC_ID = "PEC_C";
        public int Slot = 0;
        public int Rack = 0;
        public string IP_PLC;
        
        public Address NAPATIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 140, LENGTH = 4, AMOUNT_MAX = MaxBuffer.MK };
        public Address PRUD = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 144, LENGTH = 4, AMOUNT_MAX = MaxBuffer.MK };
        public Address SOBERT_VSTUP = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 100, LENGTH = 4, AMOUNT_MAX = MaxBuffer.MK };
        public Address SOBERT_VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 104, LENGTH = 4, AMOUNT_MAX = MaxBuffer.MK };
        public Address VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 148, LENGTH = 4, AMOUNT_MAX = MaxBuffer.MK };
        public Address PRISPOSOBENIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 164, LENGTH = 4, AMOUNT_MAX = MaxBuffer.MK };

        public Address TLAK_VODY = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 56, LENGTH = 4, AMOUNT_MAX = MaxBuffer.DB };
        public Address TEPLOTA_VODY_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 60, LENGTH = 4, AMOUNT_MAX = MaxBuffer.DB };
        public Address TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 48, LENGTH = 4, AMOUNT_MAX = MaxBuffer.DB };        

        public static byte[] BufferDB = new byte[MaxBuffer.DB];
        int AmountDB = MaxBuffer.DB;
        public static byte[] BufferMK = new byte[MaxBuffer.MK];
        int AmountMK = MaxBuffer.MK;

        private dlgLoguj Loguj;
        private dlgShowResultInfo ShowResultInfo;
        private DataGridView dgvRecords;

        private S7Client Client;
        private S7MultiVar Writer;
        private readonly int ErrInterval = 5;
        
        private System.Timers.Timer tmrRead;
        private System.Timers.Timer tmrError;


        public int ErrorRead { get; private set; }
        public bool SPRACOVAVAM { get; private set; }

        public string CUR_VALUE_BUF_DB { get { return BitConverter.ToString(BufferDB); } }
        public string CUR_VALUE_BUF_MK { get { return Helpers.OdrezAParsuj(System.Text.Encoding.UTF8.GetString(BufferMK)); } }


        public Pec(string pPecID, string pIP_PLC, Address pNapatie, Address pPrud, Address pSobertVstup, Address pSobertVykon, Address pVykon, Address pPrisposobenie, Address pTlakVody, Address pTeplotaVodyVstup, Address pTeplotaVodyVystup) {
            
            
            this.PEC_ID = pPecID;
            this.IP_PLC = pIP_PLC;
            this.NAPATIE = pNapatie;
            this.PRUD = pPrud;
            this.SOBERT_VSTUP = pSobertVstup;
            this.SOBERT_VYKON = pSobertVykon;
            this.VYKON = pVykon;
            this.PRISPOSOBENIE = pPrisposobenie;
            this.TLAK_VODY = pTlakVody;
            this.TEPLOTA_VODY_VSTUP = pTeplotaVodyVstup;
            this.TEPLOTA_VODY_VYSTUP = pTeplotaVodyVystup;
        }

        public Pec(string pPecID, string pIP_PLC, dlgLoguj pDlgLoguj, DataGridView dgvRecords, dlgShowResultInfo pDlgShowResultInfo)
        {
            this.Loguj = pDlgLoguj;
            this.dgvRecords = dgvRecords;
            this.ShowResultInfo = pDlgShowResultInfo;

            tmrRead.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            tmrError.Elapsed += new ElapsedEventHandler(PripojKPLC);            

        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                              e.SignalTime);
        }

        private void ReadArea()
        {
            int ResultMK, ResultDB = 0;
            int SizeReadDB =0 , SizeReadMK = 0;

            //#################################################################################################################
            try
            {
                ResultDB = Client.ReadArea(S7Consts.S7AreaDB, 001, 0, this.AmountDB, S7Consts.S7WLByte, BufferDB, ref SizeReadDB);
                ResultMK = Client.ReadArea(S7Consts.S7AreaMK, 000, 0, this.AmountMK, S7Consts.S7WLByte, BufferMK, ref SizeReadMK);
            }
            catch
            {
                ResultMK = -999;
                MessageBox.Show("-9999999999999999999999999999999999999999999999999");
            }
            //#################################################################################################################

            //ak je resut OK (0) vypise do logu OK, ak nie je OK vypisem nieco ine
            this.ShowResultInfo(ResultDB + ResultMK);

            if (ResultDB == 0 && ResultMK == 0)
            {
                ErrorRead = 0;
                //lbPrecHodnotaRaw.Text = "DB:" + this.CUR_VALUE_BUF_DB.Trim() + " /  " + "MK:" + this.CUR_VALUE_BUF_MK.Trim(); //vypisem orezany buffer                     
                //lbBytesRead.Text = "DB:" + SizeReadDB.ToString() + " / " + "MK:" + SizeReadMK.ToString();  //kolko bytov bolo precitanych              
                SPRACOVAVAM = true;
                SpracujZaznamDB();
                SPRACOVAVAM = false;
            }
            else
            {
                this.ErrorRead++;
                this.Loguj("Chyba pri čítaní hodnoty z PLC", MessageBoxIcon.Error);

                //Thread.Sleep(this.ErrInterval);
                //po troch chybach citania sa odpajam
                if (ErrorRead >= 5)
                {
                    Thread.Sleep(this.ErrInterval);
                    ErrorRead = 0;
                    odpojiť();
                    //kazdych 5 sekund sa snazi pripojit k plc
                    PripojKPLC(null, null);
                }
            }
        }


        private void PripojKPLC(Object myObject, EventArgs myEventArgs)
        {
            int Result = Client.ConnectTo(IP_PLC, Rack, Slot);

            //ShowResultInfo(Result); //NEPODARILO SA PRIPOJIT

            if (Result == 0)
            {
                this.Loguj("Connected", MessageBoxIcon.Information);
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

                this.Loguj("Nepodarilo sa pripojiť k PLC!", MessageBoxIcon.Error);
            }
        }

        private void odpojiť()
        {
            Client.Disconnect();
            tmrRead.Enabled = false;
            //TextError.ForeColor = Color.Black;
            //TextError.Text = "Disconnected";

            this.Loguj("Disconnected", MessageBoxIcon.Information);
        }


        void aktualizujDtg()
        {

            if (DB.Context.records.Count() > 0)
            {
                int id_last = DB.Context.records.Max(rec => rec.id);
                List<record> pom = DB.Context.records.Where(r => r.id > id_last - MainForm.POCET_ZAZNAMOV).ToList();
                pom.Reverse();
                dgvRecords.DataSource = pom; //DB.Context.reports.Where(r=>r.ID > id_last- 10).ToList().Reverse();
            }
        }

        private void SpracujZaznamDB()
        {

            DateTime DATE_TIME = DateTime.Now;            

            float NAPATIE = Bytes.NAPATIE.getVaue();
            float PRUD = Bytes.PRUD.getVaue();
            float SOBERT_VSTUP = Bytes.SOBERT_VSTUP.getVaue();
            float SOBERT_VYKON = Bytes.SOBERT_VYKON.getVaue();
            float VYKON = Bytes.VYKON.getVaue();
            float PRISPOSOBENIE = Bytes.PRISPOSOBENIE.getVaue();

            float TLAK_VODY = Bytes.TLAK_VODY.getVaue();
            float TEPLOTA_VODY_VSTUP = Bytes.TEPLOTA_VODY_VSTUP.getVaue();
            float TEPLOTA_VODY_VYSTUP = Bytes.TEPLOTA_VODY_VYSTUP.getVaue();

            


        DB.ulozDoDB(new record()
            {
                pec_id = this.PEC_ID,
                date_time = DATE_TIME,
                napatie = NAPATIE,
                prud = PRUD,
                sobert_vstup = SOBERT_VSTUP,
                sobert_vykon = SOBERT_VYKON,
                t_voda_vstup = TEPLOTA_VODY_VSTUP,
                t_voda_vystup = TEPLOTA_VODY_VYSTUP,
                tlak = TLAK_VODY,
                rz_pribenie = PRISPOSOBENIE,
                vykon = VYKON,
                zmena = Helpers.dajZmenu(DATE_TIME)
            });


            if (DB.UlozDB())
            {
                aktualizujDtg();
                this.Loguj("Zaznam ulozeny do DB!", MessageBoxIcon.Information);
            }
            else
            {
                this.Loguj("### ### ###", MessageBoxIcon.Error);
                this.Loguj("Zaznam sa nepodarilo ulozit do DB!", MessageBoxIcon.Error);
                this.Loguj("### ### ###", MessageBoxIcon.Error);
            }
        }


    }
}
