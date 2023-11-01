using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Windows.Forms;

namespace nsAspur
{

    delegate void dlgLoguj(string text, MessageBoxIcon pTyp);
    delegate void dlgShowResultInfo(int pResultNumber);

    class Helpers
    {
        static Random random = new Random();


        public static string dajZmenu(DateTime pDatumCas) {
            
            int hodina = pDatumCas.Hour;
            
            if ((hodina >= 6) && (hodina < 14)) return "ZMENA-1";
            else
                if ((hodina >= 14) && (hodina < 22)) return "ZMENA-2";
            else
                return ("ZMENA-3");
        }
        
        static public int dajCislo() {
            
            return random.Next(100000000, 999999999);
        }

        static public string Parsuj(String pRetazec) {
            int index = pRetazec.IndexOf('\r');
            if (index > 0)
                return pRetazec.Substring(0,index);
            else
                return null;
        }

        static public string OdrezAParsuj(String pRetazec)
        {

            /*
            int index = pRetazec.IndexOf('\r');
            if (index > 0) return pRetazec.Substring(2, index-2);
            index = pRetazec.IndexOf('\0');
            if (index > 0) return pRetazec.Substring(2, index-2);*/
            return pRetazec.Trim();                  
        }


        static public string BytesToStrig(byte[] pPole) {

            return System.Text.Encoding.UTF8.GetString(pPole);
        }

        static public int BytesToInt(byte[] pPole)
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/types/how-to-convert-a-byte-array-to-an-int
            int i = BitConverter.ToInt16(pPole, 0);
            
            return i;
        }

        static public float BytesToFloat(byte[] pPole)
        {
            float i = (float)BitConverter.ToDouble(pPole, 0);
            return i;
        }

        static public byte[] ParsujBytes(byte[] pPole,int pIndex, int pDlzka) {

            byte[] pole = new byte[pDlzka];
            
            Array.Copy(pPole,pIndex,pole,0,pDlzka);
            char[] pom = System.Text.Encoding.UTF8.GetChars(pPole);
            //Array.Reverse(pole,0, pole.Length);
            return pole; 
        }

        static public bool BytesToBool(byte[] pPole)
        {
            bool i = BitConverter.ToBoolean(pPole, 0);
            return i;
        }

        static public bool ByteToBool(byte pByte)
        {
            bool i =  Convert.ToBoolean(pByte);
            return i;
        }

        //funcia vychadza z originalnej funkcie Pablo Agirre 
        //s tym, ze sa snazit previest pole bytov na string od jeho tretieho prvku
        //ocakava sa, ze trimnutim sa odstrania pripadne prazdne znaky v zavere
        /// <summary>
        // na druhom prvku pola, tak nemusi byt uvedena ocakavana dlzka stringu
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="Pos"></param>
        /// <returns></returns>
        public static string GetStringAtAlt(byte[] Buffer, int Pos)
        {
            //int size = (int)Buffer[Pos + 1];
            return Encoding.UTF8.GetString(Buffer, 2, Buffer.Length-2);
        }

    }
}
//Ako odhadujes pravdepodobnosti prieniku linii v TV grafoch???