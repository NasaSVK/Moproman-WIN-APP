using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sharp7;
using System.Threading;
using System.IO;
using System.Net.NetworkInformation;



//\\10.0.0.6\technik\AktualProject\Tubex_FOTO\Photo Logging Camera System\ftpcam\

//Automatické skrutkovanie pomocou robotov Universal Robots

namespace nsAspur
{
    /*ANCHOR*/
    /*https://stackoverflow.com/questions/15131779/resize-controls-with-form-resize*/

    /*Could not create the Java virtual machine*/
    /*https://windowsreport.com/java-virtual-machine-fatal-error/*/

public partial class MainForm : Form
{
    private S7Client Client;
    private S7MultiVar Writer;

    //https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.datagridview.datasource?view=net-5.0
    //private DataGridView dgvRecords = new DataGridView();
    private BindingSource bindingSource1 = new BindingSource();


        //private MySqlDb mysqldb = new MySqlDb();

        //private byte[] SPR = new byte[1] {(byte)'F' };
        //public char SPR_VALUE { get { return (char)SPR[0]; } set { SPR[0] = (byte)value; } }

        //public string CUR_VALUE_BUF { get { return System.Text.Encoding.UTF8.GetString(Buffer); } }
        public string CUR_VALUE_BUF { get { return BitConverter.ToString(Buffer); } }
        public string CUR_VALUE { get { return Helpers.OdrezAParsuj(System.Text.Encoding.UTF8.GetString(Buffer)); } }

        private byte[] TRG = new byte[1] { 0 }; //(byte)'0'

        private byte SIG_BYTE { get { return Buffer[Bytes.SIG]; } set { Buffer[Bytes.SIG] = value; } }

    private static readonly int BYTE_LENGTH = 64;
    private readonly int POCET_ZAZNAMOV = 8; //pocet vypisovanych zaznamov do GUI
    
        private byte[] Buffer = new byte[BYTE_LENGTH];
    int Amount = BYTE_LENGTH;



    string IP_PLC;

    int Rack = 0;
    int Slot = 2;
    int DB_Number = 100;

    int ErrorRead = 0; //aktualny stav chyb; ak precita viac ako 3 chyby za sebou odpoji sa a nesledne sa snazi nasledne pripojit             

    public int ErrInterval { get; private set; }
    public bool SPRACOVAVAM { get; private set; }
    public int TimeOut { get; private set; }

        private List<Brush> ItemsColor = new List<Brush>();
    private int MaxErrCount;    
    

        void aktivaciaKomp() {

        lbBytesRead.Enabled = true;
    }


        public MainForm(string[] args)
        {

            InitializeComponent();

            //DB.TestujDB();

            //InitializeDataGridView();

            this.CreateHandle();
            this.lbxLogs.DrawMode = DrawMode.OwnerDrawFixed;

            this.nacitajConfig();
            //this.WindowState = FormWindowState.Maximized;

            tmrRead.Tick += new EventHandler(this.read);
            tmrErr.Tick += new EventHandler(this.PripojKPLC);

            TimeOut = Properties.Settings.Default.TimeOut;

            Client = new S7Client();
            Writer = new S7MultiVar(Client);

            if (IntPtr.Size == 4)
                this.Text = this.Text + " - Running 32 bit Code";
            else
                this.Text = this.Text + " - Running 64 bit Code";

            this.Loguj("*** START ***", MessageBoxIcon.Warning);

            //https://ourcodeworld.com/articles/read/506/winforms-cross-thread-operation-not-valid-control-controlname-accessed-from-a-thread-other-than-the-thread-it-was-created-on
            new Thread(() =>
            {
                Thread.Sleep(TimeOut);
                //this.Invoke(new Action(() => this.WindowState = FormWindowState.Minimized));                
                //MessageBox.Show("Vlakno spustene!");
            }).Start(); 
        }

        private void InitializeDataGridView()
        {
            // Set up the data source.
            //bindingSource1.DataSource = DB.Context.reports.ToList(); //GetData("Select * From Products");
            //dgvRecords.DataSource = bindingSource1;
            //dgvRecords.DataSource = DB.Context.reports.ToList(); //GetData("Select * From Products");
            //dgvRecords.DataSource = bindingSource1;
            aktualizujDtg();
        }


        private bool citajSignal() {

            byte[] sig_byte = new byte[0];
            int  size_read = 0;
            int Result = Client.ReadArea(S7Consts.S7AreaDB, DB_Number, 0, 1, S7Consts.S7WLByte, sig_byte, ref size_read);            
            return true;
        }

        private void ReadArea()
        {

        int SizeRead = 0;
        int Result = 0;
        lbBytesRead.Text = "";


            //#################################################################################################################
            //Result = Client.ReadArea(S7Consts.S7AreaDB, DB_Number, 0, this.Amount, S7Consts.S7WLByte, Buffer, ref SizeRead);
            Result = Client.ReadArea(S7Consts.S7AreaDB, DB_Number, 0, this.Amount, S7Consts.S7WLByte, Buffer, ref SizeRead);
            //#################################################################################################################

            //ak je resut OK (0) vypise do logu OK, ak nie je OK vypisem nieco ine
            this.ShowResultInfo(Result);

        if (Result == 0) {
                ErrorRead = 0;
                lbPrecHodnotaRaw.Text = this.CUR_VALUE_BUF.Trim(); //this.CUR_VALUE_BUF; //vypisem celkovy buffer
                //lbPrecHodnotaText.Text = this.CUR_VALUE.Trim(); // vypisem orezany buffer                       
                lbBytesRead.Text = SizeRead.ToString();  //kolko bytov bolo precitanych              

                if (Buffer[Bytes.SIG] == 1) {
                    SPRACOVAVAM = true;
                    SpracujZaznamDB();
                    zapisSignal();
                    SPRACOVAVAM = false;
                }
                
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

        private void SpracujZaznamDB()
        {

            DateTime DATE_TIME = DateTime.Now;

            byte vIn = 70;
            char vOut = Convert.ToChar(vIn);

            char SHIFT = (char)Buffer[Bytes.SHIFT];
                        
            float CYCLE_TIME = S7.GetRealAt(Helpers.ParsujBytes(Buffer, Bytes.CYCLE_TIME, 4),0);
            //float CYCLE_TIME = Helpers.BytesToFloat(Helpers.ParsujBytes(Buffer, Bytes.CYCLE_TIME, 4));

            char UR1_B = (char)Buffer[Bytes.R1_B];
            char UR1_F = (char)Buffer[Bytes.R1_F];
            char UR1_S = (char)Buffer[Bytes.R1_S];

            char UR2_B = (char)Buffer[Bytes.R2_B];
            char UR2_F = (char)Buffer[Bytes.R2_F];
            char UR2_S = (char)Buffer[Bytes.R2_S];
            
            
            float OK_ = S7.GetRealAt(Helpers.ParsujBytes(Buffer, Bytes.OK_, 4),0);
            float NOK_ = S7.GetRealAt(Helpers.ParsujBytes(Buffer, Bytes.NOK_, 4), 0);
        

            int CNT_OK = S7.GetIntAt(Helpers.ParsujBytes(Buffer, Bytes.CNT_OK, 2),0);
            int CNT_NOK = S7.GetIntAt(Helpers.ParsujBytes(Buffer, Bytes.CNT_NOK, 2),0);
   
            float R1_ = S7.GetRealAt(Helpers.ParsujBytes(Buffer, Bytes.R1_, 4),0);
            float R2_ = S7.GetRealAt(Helpers.ParsujBytes(Buffer, Bytes.R2_, 4),0);
  

            int UR1 = S7.GetIntAt(Helpers.ParsujBytes(Buffer, Bytes.CNT_OKR1, 2), 0);       
            int UR2 = S7.GetIntAt(Helpers.ParsujBytes(Buffer, Bytes.CNT_OKR2, 2), 0);

            char ERROR = (char)Buffer[Bytes.ERROR];

            float ERROR_TIME = S7.GetRealAt(Helpers.ParsujBytes(Buffer, Bytes.ERROR_TIME, 4),0);
            float WAITING_TIME = S7.GetRealAt(Helpers.ParsujBytes(Buffer, Bytes.WAITING_TIME, 4),0);

            //string TestString1 = S7.GetStringAt(Helpers.ParsujBytes(Buffer, Bytes.TESTString1, 10), 0);
            //string TestString2 = S7.GetStringAt(Helpers.ParsujBytes(Buffer, Bytes.TESTString2, 10), 0);


            DB.ulozDoDB(new report2s() {
                DATE_TIME = DATE_TIME,
                SHIFT = SHIFT.ToString(),
                CYCLE_TIME = CYCLE_TIME,
                R1_B = UR1_B.ToString(),
                R1_S = UR1_S.ToString(),
                R1_F = UR1_F.ToString(),                                
                R2_B = UR2_B.ToString(),
                R2_S = UR2_S.ToString(),
                R2_F = UR2_F.ToString(),
                OK_ = OK_,
                NOK_ = NOK_,
                CNT_OK = CNT_OK,
                CNT_NOK = CNT_NOK,
                UR1 = UR1,
                UR2 = UR2,
                R1_ = R1_,
                R2_ = R2_,
                ERROR = ERROR.ToString(),
                ERROR_TIME = ERROR_TIME,
                WAITING_TIME = WAITING_TIME
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
        /// <summary>
        /// zapise 0 do signalizacneho bytu na PLC
        /// </summary>        
        private void zapisSignal()
        {


            //int Result = Client.WriteArea(S7Consts.S7AreaDB, DB_Number, Bytes.SIG, 2, 1, new byte[] { 1 });
            Writer.Add(S7Consts.S7AreaDB, S7Consts.S7WLByte, DB_Number, Bytes.SIG, 1, ref TRG);
            int Result = Writer.Write();

            //ak je resut OK (0) vypise do logu OK, ak nie je OK vypisem nieco ine
            this.ShowResultInfo(Result);

            if (Result == 0)
            {
                ErrorRead = 0;
                //lbBytesRead.Text = SizeRead.ToString();  //kolko bytov bolo precitanych              

                this.Loguj("NULA ZAPISANA DO SIGNALBYTU", MessageBoxIcon.Information);
                Buffer[Bytes.SIG] = 1;
            }
            else
            {
                this.ErrorRead++;
                this.Loguj("Chyba pri zapise hodnoty (SIGPBYTE) z PLC", MessageBoxIcon.Error);                
            }
        }

        void aktualizujDtg() {
            
            int id_last = DB.Context.reports.Max(rec=>rec.ID);
            List<report2s> pom = DB.Context.reports.Where(r => r.ID > id_last - POCET_ZAZNAMOV).ToList();
            pom.Reverse();
            dgvRecords.DataSource = pom; //DB.Context.reports.Where(r=>r.ID > id_last- 10).ToList().Reverse();
        }

        private void PripojKPLC(Object myObject, EventArgs myEventArgs)
        {
        int Result = Rack = Slot = 0;
            Slot = 0;

        Result = Client.ConnectTo(IP_PLC, Rack, Slot);

        //ShowResultInfo(Result); //NEPODARILO SA PRIPOJIT

        if (Result == 0)
        {
            this.Loguj("Connected", MessageBoxIcon.Information);
            //vypnem casovac snaziaci sa o spojenie kazdych 5 sekund
            if (tmrErr.Enabled == true) tmrErr.Enabled = false;            
            TextError.ForeColor = Color.Black;
            TextError.Text = " PDU Negotiated : " + Client.PduSizeNegotiated.ToString();                
            //lbErrors.Text = ErrorReadAll.ToString();
            //##########################################
            //##########################################
            this.ReadArea();
            tmrRead.Enabled = true;
            //##########################################
            //##########################################
            //btnPripojPlc.Enabled = false;
        }
        else
        {          
            if (tmrRead.Enabled == true) tmrRead.Enabled = false;
            //v pripade chyby spustim druhy casovac nastaveny na periodu 5s a tri krat v intervale jednej sekundy sa pokusim pripojit
            if (tmrErr.Enabled == false) tmrErr.Enabled = true;

            this.Loguj("Nepodarilo sa pripojiť k PLC!", MessageBoxIcon.Error);
        }
    }

    void nacitajConfig()
    {
        this.Rack = 0;
        this.Slot = 0;
        DB_Number = Properties.Settings.Default.DBNumber;                    
        this.IP_PLC = Properties.Settings.Default.IP_PLC;
        this.tmrRead.Interval = Properties.Settings.Default.Refresh;
        this.ErrInterval = Properties.Settings.Default.ErrInt;
        this.tmrErr.Interval = this.ErrInterval;
        this.MaxErrCount = Properties.Settings.Default.MaxErrCount;                                    
    }


    // This function returns a textual explaination of the error code            
    private void ShowResultInfo(int Result)
    {            
        //ak je vysledok 0, zobrazi sa OK a cas citania
        if (Result == 0)
        {
            TextError.ForeColor = Color.Black;
            TextError.Text = Client.ErrorText(Result) + " (" + Client.ExecutionTime.ToString() + " ms)";
            this.Loguj("PLC " + Client.ErrorText(Result) + " (" + Client.ExecutionTime.ToString() + " ms)", MessageBoxIcon.None);
        }
        else
        {
            TextError.ForeColor = Color.Red;
            TextError.Text = Client.ErrorText(Result);
            //this.Loguj(Client.ErrorText(Result), MessageBoxIcon.Error);
        }                           
    }


    private void read(Object myObject, EventArgs myEventArgs) {

        ReadArea();
    }

    private void odpojiť()
    {
        Client.Disconnect();
        tmrRead.Enabled = false;
        TextError.ForeColor = Color.Black;
        TextError.Text = "Disconnected";

        this.Loguj("Disconnected", MessageBoxIcon.Information);
    }


    //########################################################################################################################################
    //########################################################################################################################################

    //vypis logu ListBox-u
    private void Loguj(string pText, MessageBoxIcon pTyp)
    {
        Color farba;

        switch (pTyp) { 
           case MessageBoxIcon.Error: farba = Color.Red; break;
           case MessageBoxIcon.Information: farba = Color.Blue; break;
           case MessageBoxIcon.None: farba = Color.Black; break;
           case MessageBoxIcon.Warning: farba = Color.Orange;  break;
           default: farba = Color.Black; break;
        }

        if (MessageBoxIcon.Error != pTyp) pText = DateTime.Now + "   " + pText;

        //this.lbxLogs.Items.Insert(0, new Label() { ForeColor = Color.Black, Height = 30, Width = 200, Text = pText, Font = new Font(new FontFamily("Comic Sans MS"), 12)});

        this.ItemsColor.Insert(0, new SolidBrush(farba));

        if (!this.IsHandleCreated)
        {
            //this.CreateHandle();
        }
        this.Invoke(new Action(()=>lbxLogs.Items.Insert(0, pText)));
        //lbxLogs.Items.Insert(0, pText);
        if (lbxLogs.Items.Count >= 999999) {
            this.ItemsColor.Clear();
            this.lbxLogs.Items.Clear();

        }                        
    }

    //otvorenie okna na zobrazenie informaci a apke
    private void btnInfo_Click(object sender, EventArgs e)
    {
        new InfoForm(){ TopMost = true }.ShowDialog();
    }

    //pri loadnuti okna (PO KONSTRUKTORE FORMU) sa snazim pripojit 
    private void MainForm_Load(object sender, EventArgs e)
    {            
        //System.Windows.Forms.DoEvent();
        Application.DoEvents();
        PripojKPLC(null,null);
    }

    //zatvaranie apky
    private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        this.odpojiť();
    }

    //klikacie citanie
    private void btnCitaj_Click(object sender, EventArgs e)
    {
            //this.ReadArea();
            SpracujZaznamDB();
    }


    private void button1_Click(object sender, EventArgs e)
    {
        // These are tests done on my DB

        DateTime DT = DateTime.Now;
        S7.SetSIntAt(Buffer, 40, -125);
        S7.SetIntAt(Buffer, 42, 32501);
        S7.SetDIntAt(Buffer, 44, -332501);
        S7.SetLIntAt(Buffer, 48, -99832501);
        S7.SetRealAt(Buffer, 56, (float)98.778);
        S7.SetLRealAt(Buffer, 60, 123000000000.778);
        S7.SetUSIntAt(Buffer, 24, 125);
        S7.SetUIntAt(Buffer, 26, 32501);
        S7.SetUDIntAt(Buffer, 28, 332501);
        S7.SetULintAt(Buffer, 32, 99832501);
        S7.SetDateAt(Buffer, 80, DT);
        S7.SetTODAt(Buffer, 82, DT);
        S7.SetDTLAt(Buffer, 112, DT);
        S7.SetLTODAt(Buffer, 86, DT);
        S7.SetLDTAt(Buffer, 94, DT);
        //PlcDBWrite();
    }


    //ZAPIS FARABENEJ POLOZKY DO ListBoxu
    //http://www.thescarms.com/dotnet/CustomListBox.aspx
    private void lbxLogs_DrawItem(object sender, DrawItemEventArgs e)
    {
        e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(),
          e.Font, this.ItemsColor[e.Index], e.Bounds, StringFormat.GenericDefault);
        /*
        if (e.Index != 0) 
        e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(),
          e.Font, new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
        else
            e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(),
          e.Font, CurrentBrush, e.Bounds, StringFormat.GenericDefault);*/
}

//START/PAUSE skenovania
private void btnStop_Click(object sender, EventArgs e)
        {
            if (btnStop.Text == "PAUSE")
            {
                btnStop.Text = "START";
                tmrRead.Enabled = false;
                tmrErr.Enabled = false;
                this.Loguj("*** PAUSE ***", MessageBoxIcon.Warning);
            }
            else
            {
                btnStop.Text = "PAUSE";
                tmrRead.Enabled = true;
                this.Loguj("*** START ***", MessageBoxIcon.Warning);
            }
                
        }

        //####################################################################
        //SUSTAVY
        //####################################################################
        private int dajDec(byte h1, byte h2)
        {

            byte[] pole = { h2, h1 };
            int pom = BitConverter.ToInt16(pole, 0);
            return pom;
            //return h1 * 256 + h2;
            //return BitConverter.ToInt32(pole, 0);
            //string vys16 = string.Format("{0:x}", h1) + string.Format("{0:x}", h2);
            //return Convert.ToInt32(vys16, 16);
        }

        private void HexDump(Label DumpBox, byte[] bytes, int Size)
        {
            if (bytes == null)
                return;
            int bytesLength = Size;
            int bytesPerLine = 16;

            char[] HexChars = "0123456789ABCDEF".ToCharArray();

            int firstHexColumn =
                  8                   // 8 characters for the address
                + 3;                  // 3 spaces

            int firstCharColumn = firstHexColumn
                + bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
                + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                + 2;                  // 2 spaces 

            int lineLength = firstCharColumn
                + bytesPerLine           // - characters to show the ascii value
                + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

            char[] line = (new String(' ', lineLength - 2) + Environment.NewLine).ToCharArray();
            int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
            StringBuilder result = new StringBuilder(expectedLines * lineLength);

            for (int i = 0; i < bytesLength; i += bytesPerLine)
            {
                line[0] = HexChars[(i >> 28) & 0xF];
                line[1] = HexChars[(i >> 24) & 0xF];
                line[2] = HexChars[(i >> 20) & 0xF];
                line[3] = HexChars[(i >> 16) & 0xF];
                line[4] = HexChars[(i >> 12) & 0xF];
                line[5] = HexChars[(i >> 8) & 0xF];
                line[6] = HexChars[(i >> 4) & 0xF];
                line[7] = HexChars[(i >> 0) & 0xF];

                int hexColumn = firstHexColumn;
                int charColumn = firstCharColumn;

                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (j > 0 && (j & 7) == 0) hexColumn++;
                    if (i + j >= bytesLength)
                    {
                        line[hexColumn] = ' ';
                        line[hexColumn + 1] = ' ';
                        line[charColumn] = ' ';
                    }
                    else
                    {
                        byte b = bytes[i + j];
                        line[hexColumn] = HexChars[(b >> 4) & 0xF];
                        line[hexColumn + 1] = HexChars[b & 0xF];
                        line[charColumn] = (b < 32 ? '·' : (char)b);
                    }
                    hexColumn += 3;
                    charColumn++;
                }
                result.Append(line);
            }
            DumpBox.Text = result.ToString();
        }

        string HexByte(byte B)
        {
            string Result = Convert.ToString(B, 16);
            if (Result.Length < 2)
                Result = "0" + Result;
            return "0x" + Result;
        }

        string HexWord(ushort W)
        {
            string Result = Convert.ToString(W, 16);
            while (Result.Length < 4)
                Result = "0" + Result;
            return "0x" + Result;
        }


        void GetBlockInfo()
        {
            S7Client.S7BlockInfo BI = new S7Client.S7BlockInfo();
            int[] BlockType =
            {
                S7Client.Block_OB,
                S7Client.Block_DB,
                S7Client.Block_SDB,
                S7Client.Block_FC,
                S7Client.Block_SFC,
                S7Client.Block_FB,
                S7Client.Block_SFB
            };
            /*
            txtBI.Text = "";
            int Result = Client.GetAgBlockInfo(BlockType[CBBlock.SelectedIndex], System.Convert.ToInt32(txtBlockNum.Text), ref BI);
            ShowResultInfo(Result);
            if (Result==0)
            {
                // Here a more descriptive Block Type, Block lang and so on, are needed, 
                // but I'm too lazy, do it yourself.
                txtBI.Text = txtBI.Text + "Block Type    : " + HexByte((byte)BI.BlkType) + (char)13 + (char)10;
                txtBI.Text = txtBI.Text + "Block Number  : " + Convert.ToString(BI.BlkNumber) + (char)13 + (char)10;
                txtBI.Text = txtBI.Text + "Block Lang    : " + HexByte((byte)BI.BlkLang) + (char)13 + (char)10;
                txtBI.Text = txtBI.Text + "Block Flags   : " + HexByte((byte)BI.BlkFlags) + (char)13 + (char)10;
                txtBI.Text = txtBI.Text + "MC7 Size      : " + Convert.ToString(BI.MC7Size) + (char)13 + (char)10;
                txtBI.Text = txtBI.Text + "Load Size     : " + Convert.ToString(BI.LoadSize) + (char)13 + (char)10;
                txtBI.Text = txtBI.Text + "Local Data    : " + Convert.ToString(BI.LocalData) + (char)13 + (char)10;
                txtBI.Text = txtBI.Text + "SBB Length    : " + Convert.ToString(BI.SBBLength) + (char)13 + (char)10;
                txtBI.Text = txtBI.Text + "Checksum      : " + HexWord((ushort)BI.CheckSum) + (char)13 + (char)10;
                txtBI.Text = txtBI.Text + "Version       : 0." + Convert.ToString(BI.Version) + (char)13 + (char)10;
                txtBI.Text = txtBI.Text + "Code Date     : " + BI.CodeDate + (char)13 + (char)10;
                txtBI.Text = txtBI.Text + "Intf.Date     : " + BI.IntfDate + (char)13 + (char)10;
                txtBI.Text = txtBI.Text + "Author        : " + BI.Author +(char)13 + (char)10;               
                txtBI.Text = txtBI.Text + "Family        : " + BI.Family +(char)13 + (char)10;
                txtBI.Text = txtBI.Text + "Header        : " + BI.Header;
            }*/
        }


        private void BlockInfoBtn_Click(object sender, EventArgs e)
        {
            GetBlockInfo();
        }
        
        /*
        void SetTRG(char pZnak) { 
            this.TRG = new byte[] { (byte) pZnak };
        }*/

        /*
        void SetSPR(char pZnak)
        {
            this.SPR = new byte[] { (byte)pZnak };
        }
        
         pPB.Image = imageList1.Images[2];
         
         */
    }

}
