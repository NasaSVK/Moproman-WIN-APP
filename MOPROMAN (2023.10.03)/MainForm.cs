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

namespace PLC_Mapper
{

    public partial class MainForm : Form
    {
        bool a = false;
        private S7Client Client;
        private byte[] Buffer = new byte[65536];
        private byte[] DB_A = new byte[1024];
        private byte[] DB_B = new byte[1024];
        private byte[] DB_C = new byte[1024];

        string IP;
        int WordCount;
        int Rack;
        int Slot;
        int DB_Number;

        EStrana strana  = EStrana.XX;

        void aktivaciaKomp() {
            label18.Enabled = true;
        }


        void nacitajConfig() {
            this.Rack = 0;
            this.Slot = 0;
            this.DB_Number = 1;
            this.WordCount = Properties.Settings.Default.Words;
            this.IP = Properties.Settings.Default.IPadd;
            tmr1.Interval = Properties.Settings.Default.Refresh;
        }

        private void ShowResult(int Result)
        {
            // This function returns a textual explaination of the error code
            TextError.ForeColor = Color.Red;
            TextError.Text = Client.ErrorText(Result);
            if (Result == 0) {
                TextError.ForeColor = Color.Black;
                TextError.Text = TextError.Text + " (" + Client.ExecutionTime.ToString() + " ms)";
            }
        }

        public MainForm(string[] args)
        {
            /*
            for (int i = 0; i < args.Length; i++)
            {
                MessageBox.Show(String.Format("args[{0}] == {1}", i, args[i]));
            }*/

            InitializeComponent();
           
            this.nacitajConfig();
            this.nastavZobrazenie(args);
            this.WindowState = FormWindowState.Maximized;
            tmr1.Tick += new EventHandler(this.read);

            Client = new S7Client();
            if (IntPtr.Size == 4)
                this.Text = this.Text + " - Running 32 bit Code";
            else
                this.Text = this.Text + " - Running 64 bit Code";

            //PripojKPLC();
        }

        private void read(Object myObject, EventArgs myEventArgs) {
            ReadArea();
        }

        void nastavZobrazenie(string [] pArg) {

            this.Location = new System.Drawing.Point(0, 0);
            if (pArg.Length > 0)
            {
                if (pArg[0] == "L")
                {
                    this.Location = new System.Drawing.Point(-1440, 0);
                    lbHand.Text = "LH";
                    //this.strana = EStrana.LH;
                    this.DB_Number = 1;
                }
                else
                    if (pArg[0] == "R")
                {
                    this.Location = new System.Drawing.Point(+1440, 0);
                    lbHand.Text = "RH";
                    //this.strana = EStrana.RH;
                    this.DB_Number = 2;
                }
                /*
                else
                    if (pArg[0] == "M")
                    this.Location = new System.Drawing.Point(0, 0);
                    lbHand.Text = "RH";*/
                else
                    MessageBox.Show("Neznámy parameter (" + pArg[0] + ") pri spustení programu!", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            /*
            if (Properties.Settings.Default.StartMon == "L")
                this.Location = new System.Drawing.Point(-1440, 0);
            else
            if (Properties.Settings.Default.StartMon == "R")
                this.Location = new System.Drawing.Point(1440, 0);
            else
                this.Location = new System.Drawing.Point(0, 0);
                */
        }

        private void PripojKPLC()
        {
            int Result;
            int Rack = 0;
            int Slot = 0;
            Result = Client.ConnectTo(IP, Rack, Slot);
            ShowResult(Result);
            if (Result == 0)
            {
                this.ErrorsConn = 0;
                TSSLabel.ForeColor = Color.Black;
                TSSLabel.Text = TSSLabel.Text + " PDU Negotiated : " + Client.PduSizeNegotiated.ToString();
                lbErrors.Text = ErrorReadAll.ToString();
                //##########################################
                //##########################################
                tmr1.Enabled = true;
                //##########################################
                //##########################################
                btnPripojPlc.Enabled = false;
            }
            else
            {
                //tmr1.Start();
                ErrorsConn++;
                if (ErrorsConn == 3)
                {
                    this.ErrorsConn = 3;
                    DialogResult vysledok = MessageBox.Show("Ani po troch pokusoch o pripojenie sa nepodarilo pripojiť k PLC! \n Źeláte si pokusiť o pripojenie opäť?", "Chyba", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (vysledok == DialogResult.Yes)
                    {
                        ErrorsConn = 0;
                        PripojKPLC();
                    }
                    else
                        Application.Exit();
                }
                else
                {
                    Thread.Sleep(1000);
                    PripojKPLC();
                }
            }
        }

        private void odpojiť()
        {
            Client.Disconnect();
            TextError.ForeColor = Color.Black;
            TextError.Text = "Disconnected";
        }


        //############################################################################
        //############################################################################
        private void HexDump(TextBox DumpBox, byte[] bytes, int Size)
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
            DumpBox.Text=result.ToString();
        }
        //############################################################################
        //############################################################################

        private void zapisHodnotu(int hodnota, Label ciel, int min, int max, PictureBox pPB) {
            if ((hodnota < min) || (hodnota > max))
            {
                ciel.BackColor = Color.Red;
                if (hodnota < min)
                    pPB.Image = imageList1.Images[2];
                else
                    if (hodnota > max)
                    pPB.Image = imageList1.Images[1];
            }
            else
            {
                ciel.BackColor = Color.LightGreen;
                pPB.Image = imageList1.Images[0];
            }

            ciel.Text = hodnota.ToString() + " mm";
        }

        private int dajDec(byte h1, byte h2) {
            //return h1 * 256 + h2;
            return BitConverter.ToInt32(new byte[]{h1,h2}, 0);
            //string vys16 = string.Format("{0:x}", h1) + string.Format("{0:x}", h2);
            //return Convert.ToInt32(vys16, 16);
        }

        private void Vypis(byte[] bytes, int Size)
        {
            int akt,min,max = 0;
            //akt = 6666;
            akt = dajDec(bytes[0], bytes[1]);
            //min = -4444;
            min = dajDec(bytes[2], bytes[3]);
            max = 9999;
            max = dajDec(bytes[4], bytes[5]);
            zapisHodnotu(akt, lbPAkt, min, max,pbxStatusP);
            lbHMin.Text = min.ToString() + " mm";
            lbHMax.Text = max.ToString() + " mm";

            akt = dajDec(bytes[8], bytes[9]);
            
            min = dajDec(bytes[10], bytes[11]);
            max = dajDec(bytes[12], bytes[13]);
            zapisHodnotu(akt, lbDAkt, min, max,pbxStatusD);
            lbPMin.Text = min.ToString() + " mm";
            lbPMax.Text = max.ToString() + " mm";


            akt = dajDec(bytes[16], bytes[17]);
            min = dajDec(bytes[18], bytes[19]);
            max = dajDec(bytes[20], bytes[21]);
            zapisHodnotu(akt, lbLAkt, min, max,pbxStatusL);
            lbDMin.Text = min.ToString() + " mm";
            lbDMax.Text = max.ToString() + " mm";


            
        }
        
        //############################################################################
        //############################################################################ 


        private void ReadArea()
        {
            // Declaration separated from the code for readability
            int Amount;
            int SizeRead = 0;
            int Result;

            lbBR.Text = "0";
            Amount = 12; //Properties.Settings.Default.Words
            //public int ReadArea(int Area, int DBNumber, int Start, int Amount, int WordLen, byte[] Buffer, ref int BytesRead)
            //#################################################################################################################
            Result = Client.ReadArea(S7Consts.S7AreaDB, this.DB_Number, 0, Amount, S7Consts.S7WLInt, Buffer, ref SizeRead);
            //#################################################################################################################

            ShowResult(Result);
            if (Result == 0)
            {
                Vypis(Buffer, SizeRead);
                lbBR.Text = SizeRead.ToString();
            }
            else
            {
                this.ErrorRead++;
                this.ErrorReadAll++;
                if (ErrorRead == 3)
                {
                    ErrorRead = 0;
                    odpojiť();
                    Application.DoEvents();
                    Thread.Sleep(1000);
                    PripojKPLC();
                }
            }

        }


        void DBMultiWrite()
        {
            for (int c = 0; c < 16; c++)
            {
                DB_A[c] = 0x01;
            }
            for (int c = 0; c < 16; c++)
            {
                DB_B[c] = 0x02;
            }

            // Multi Writer Instance
            S7MultiVar Writer = new S7MultiVar(Client);

            //TxtRes_A.Text = "";
            //TxtRes_B.Text = "";
            //TxtRes_C.Text = "";
            //TxtDump_A.Text = "";
            //TxtDump_B.Text = "";
            //TxtDump_C.Text = "";

            //int DBNumber_A = Convert.ToInt32(TxtDB_A.Text);
            //int DBNumber_B = Convert.ToInt32(TxtDB_B.Text);
            //int DBNumber_C = Convert.ToInt32(TxtDB_C.Text);

            // Add Items def.
            
            //Writer.Add(S7Consts.S7AreaDB, S7Consts.S7WLByte, DBNumber_A, 0, 16, ref DB_A);
            //Writer.Add(S7Consts.S7AreaDB, S7Consts.S7WLByte, DBNumber_B, 0, 16, ref DB_B);
            //Writer.Add(S7Consts.S7AreaDB, S7Consts.S7WLByte, DBNumber_C, 0, 16, ref DB_C);
            
            
            // Performs the Write
            int Result = Writer.Write();
            // Shows the results
            ShowResult(Result);

            //TxtRes_A.Text = Client.ErrorText(Writer.Results[0]);
            //TxtRes_B.Text = Client.ErrorText(Writer.Results[1]);
            //TxtRes_C.Text = Client.ErrorText(Writer.Results[2]);
        }

        void ReadSZL()
        {
            S7Client.S7SZL SZL = new S7Client.S7SZL();
            int Size = 4096; // Declare our Buffer Size
            SZL.Data = new byte[Size];

            //label29.Text = "0";
            //TxtSZL.Text = "";

            //int Result = Client.ReadSZL(Convert.ToInt32(TxtSZLID.Text), Convert.ToInt32(TxtSZLIndex.Text), ref SZL, ref Size);
            /*
            ShowResult(Result);
            if (Result == 0)
            {
                label29.Text = Size.ToString();
                HexDump(TxtSZL, SZL.Data, Size);
            }*/
        }

        void ReadCPUInfo()
        {
            /*
            S7Client.S7CpuInfo Info = new S7Client.S7CpuInfo();
            txtModuleTypeName.Text = "";
            txtSerialNumber.Text = "";
            txtCopyright.Text = "";
            txtAsName.Text = "";
            txtModuleName.Text = "";
            int Result = Client.GetCpuInfo(ref Info);
            ShowResult(Result);
            if (Result==0)
            {
                txtModuleTypeName.Text = Info.ModuleTypeName;
                txtSerialNumber.Text = Info.SerialNumber;
                txtCopyright.Text = Info.Copyright;
                txtAsName.Text = Info.ASName;
                txtModuleName.Text = Info.ModuleName;
            }*/
        }

        void ReadOrderCode()
        {
            /*
            S7Client.S7OrderCode Info = new S7Client.S7OrderCode();
            txtOrderCode.Text = "";
            txtVersion.Text = "";
            int Result = Client.GetOrderCode(ref Info);
            ShowResult(Result);
            if (Result == 0)
            {
                txtOrderCode.Text = Info.Code;
                txtVersion.Text = Info.V1.ToString() + "." + Info.V2.ToString() + "." + Info.V3.ToString();
            }*/
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

        private void btnMultiWrite_Click(object sender, EventArgs e)
        {
            DBMultiWrite();
        }


        private void ReadSZLBtn_Click(object sender, EventArgs e)
        {
            ReadSZL();
        }

        private void ReadCPUInfoBtn_Click(object sender, EventArgs e)
        {
            ReadCPUInfo();
        }

        private void ReadOrderCodeBtn_Click(object sender, EventArgs e)
        {
            ReadOrderCode();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void SetDateTimeBtn_Click(object sender, EventArgs e)
        {
            ShowResult(Client.SetPlcSystemDateTime());
        }

        private void SetPwdBtn_Click(object sender, EventArgs e)
        {
            //ShowResult(Client.SetSessionPassword(txtPwd.Text));
        }

        private void ClrPwdBtn_Click(object sender, EventArgs e)
        {
            ShowResult(Client.ClearSessionPassword());
        }

        void ShowPlcStatus()
        {
            /*
            int Status = 0;
            int Result = Client.PlcGetStatus(ref Status);
            ShowResult(Result);
            if (Result == 0)
            {
                switch (Status)
                {
                    case S7Consts.S7CpuStatusRun:
                        {
                            lblStatus.Text = "RUN";
                            lblStatus.ForeColor = System.Drawing.Color.LimeGreen;
                            break;
                        }
                    case S7Consts.S7CpuStatusStop:
                        {
                            lblStatus.Text = "STOP";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            break;
                        }
                    default:
                        {
                            lblStatus.Text = "Unknown";
                            lblStatus.ForeColor = System.Drawing.Color.Black;
                            break;
                        }
                }
            }
            else
            {
                lblStatus.Text = "Unknown";
                lblStatus.ForeColor = System.Drawing.Color.Black;
            }*/
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
            while(Result.Length<4)
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
            ShowResult(Result);
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

        void GetSelectedDB()
        {
            /*
            int Size = 65536; // 64 K (the maximum for a S7400)
            byte[] Buffer = new byte[Size];
            txtDBGet.Text = "";
            int Result = Client.DBGet(System.Convert.ToInt32(txtBlockNum.Text), Buffer, ref Size);
            ShowResult(Result);
            if (Result==0)
            {
                HexDump(txtDBGet, Buffer, Size);
            }*/
        }

        private void PlcStatusBtn_Click(object sender, EventArgs e)
        {
            ShowPlcStatus();
        }

        private void PlcStopBtn_Click(object sender, EventArgs e)
        {
            ShowResult(Client.PlcStop());
            ShowPlcStatus();
        }

        private void PlcHotBtn_Click(object sender, EventArgs e)
        {
            ShowResult(Client.PlcHotStart());
            ShowPlcStatus();
        }

        private void PlcColdBtn_Click(object sender, EventArgs e)
        {
            ShowResult(Client.PlcColdStart());
            ShowPlcStatus();
        }

        private void BlockInfoBtn_Click(object sender, EventArgs e)
        {
            GetBlockInfo();
        }

        private void DBGetBtn_Click(object sender, EventArgs e)
        {
            GetSelectedDB();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            new InfoForm(){ TopMost = true }.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            //Thread.Sleep(100);
            //System.Windows.Forms.DoEvent();
            Application.DoEvents();
            PripojKPLC();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.odpojiť();
        }

    }

    enum EStrana {RH, LH, XX};
}
