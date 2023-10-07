using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Sharp7;
using System.Threading;


namespace Keyence.IV.Sdk.Sample_CSharp.Parts
{
    enum EStrana {RH, LH, XX};
    public partial class PlcResultDisplayControl : UserControl
    {

        //neuspesne za sebou iduce pokusy o pripojenie k PLC
        int ErrorsConn = 0;
        int ErrorRead = 0;
        int ErrorReadAll = 0;
        
        public PlcResultDisplayControl()
        {
            InitializeComponent();
            this.nacitajConfig();
            tmr1.Tick += new EventHandler(this.read);

            Client = new S7Client();
            if (IntPtr.Size == 4)
                this.Text = this.Text + " - Running 32 bit Code";
            else
                this.Text = this.Text + " - Running 64 bit Code";

            //PripojKPLC();
        }

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


        void nacitajConfig() {
            this.Rack = 0;
            this.Slot = 0;
            this.DB_Number = 1;
            this.WordCount = 2; //Properties.Settings.Default.Words;
            this.IP =  /*"10.0.0.192";*/ Properties.Settings.Default.IP;
            tmr1.Interval = /*200;*/ Properties.Settings.Default.Interval;
        }


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
            else {
                this.ErrorRead++;
                this.ErrorReadAll++;
                if (ErrorRead == 3) {
                    ErrorRead = 0;
                    odpojiť();
                    Application.DoEvents();
                    Thread.Sleep(1000);
                    PripojKPLC();
                }
               
            }


        }


        private void ShowResult(int Result)
        {
            // This function returns a textual explaination of the error code
            TSSLabel.ForeColor = Color.Red;
            TSSLabel.Text = Client.ErrorText(Result);
            if (Result == 0) {
                TSSLabel.ForeColor = Color.Black;
                TSSLabel.Text = TSSLabel.Text + " (" + Client.ExecutionTime.ToString() + " ms)";
            }
        }


        private void read(Object myObject, EventArgs myEventArgs) {
            ReadArea();
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
                    DialogResult  vysledok = MessageBox.Show("Ani po troch pokusoch o pripojenie sa nepodarilo pripojiť k PLC! \n Źeláte si pokusiť o pripojenie opäť?", "Chyba", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
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
            tmr1.Enabled = false;
            TSSLabel.ForeColor = Color.Black;
            TSSLabel.Text = "Disconnected";
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



        private int dajDec(byte h1, byte h2) {
            //return h1 * 256 + h2;
            return BitConverter.ToInt16(new byte[] { h2, h1 }, 0);
            //string vys16 = string.Format("{0:x}", h1) + string.Format("{0:x}", h2);
            //return Convert.ToInt32(vys16, 16);
        }

        private void Vypis(byte[] bytes, int Size)
        {
            int akt,min,max = 0;
 
            akt = dajDec(bytes[0], bytes[1]);
            min = dajDec(bytes[2], bytes[3]);
            max = dajDec(bytes[4], bytes[5]);
            lbAkt1.Text = akt.ToString() + " mm";;
            lbMin1.Text = min.ToString() + " mm";
            lbMax1.Text = max.ToString() + " mm";

            akt = dajDec(bytes[8], bytes[9]);
            min = dajDec(bytes[10], bytes[11]);
            max = dajDec(bytes[12], bytes[13]);
            lbAkt2.Text = akt.ToString() + " mm";
            lbMin2.Text = min.ToString() + " mm";
            lbMax2.Text = max.ToString() + " mm";


            akt = dajDec(bytes[16], bytes[17]);
            min = dajDec(bytes[18], bytes[19]);
            max = dajDec(bytes[20], bytes[21]);
            lbAkt3.Text = akt.ToString() + " mm";
            lbMin3.Text = min.ToString() + " mm";
            lbMax3.Text = max.ToString() + " mm";
            
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
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            //new InfoForm(){ TopMost = true }.ShowDialog();
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

        private void btnPripojPlc_Click(object sender, EventArgs e)
        {
            PripojKPLC();
        }
    }
}
