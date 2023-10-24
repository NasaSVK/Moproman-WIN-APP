using Sharp7;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;

namespace nsAspur
{


    class MaxBuffer { 
        public const int DB = 64;
        public const int MK = 168;
    }

    class DBatributes { 
        
    }
    
    
    class Address {
   
        public byte AREA { get; set;}  
        public int DB_NUMBER;
        public int OFFSET;
        public int LENGTH;
        public int AMOUNT_MAX;

        public float getVaue(byte[] pBuffer) { 
            
            return S7.GetRealAt(pBuffer, OFFSET);
        }

        public float getVaue()
        {

            if (AREA == S7Consts.S7AreaDB)
                return S7.GetRealAt(MainForm.BufferDB, OFFSET);
            else
                return S7.GetRealAt(MainForm.BufferMK, OFFSET);
        }
    }

    class Bytes
    {

        public static Address NAPATIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 140, LENGTH = 4, AMOUNT_MAX = 148 };
        public static Address PRUD = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 144, LENGTH = 4, AMOUNT_MAX = 148 };
        public static Address SOBERT_VSTUP = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 100, LENGTH = 4, AMOUNT_MAX = 148 };
        public static Address SOBERT_VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 104, LENGTH = 4, AMOUNT_MAX = 148 };
        public static Address VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 148, LENGTH = 4, AMOUNT_MAX = 148 };
        public static Address PRISPOSOBENIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 164, LENGTH = 4, AMOUNT_MAX = 148 };

        public static Address TLAK_VODY = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 56, LENGTH = 4, AMOUNT_MAX = 60 };
        public static Address TEPLOTA_VODY_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 60, LENGTH = 4, AMOUNT_MAX = 60 };
        public static Address TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 48, LENGTH = 4, AMOUNT_MAX = 60 };
        /*
        public static Address NAPATIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 140, LENGTH = 4, AMOUNT_MAX =148};
        public static Address PRUD = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 144, LENGTH = 4, AMOUNT_MAX = 148 };
        public static Address SOBERT_VSTUP = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 100, LENGTH = 4, AMOUNT_MAX = 148 };
        public static Address SOBERT_VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 104, LENGTH = 4, AMOUNT_MAX = 148 };
        public static Address VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 148, LENGTH = 4, AMOUNT_MAX = 148 };
        public static Address PRISPOSOBENIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 164, LENGTH = 4, AMOUNT_MAX = 148 };

        public static Address TLAK_VODY = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 56, LENGTH = 4, AMOUNT_MAX = 60 };
        public static Address TEPLOTA_VODY_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 48, LENGTH = 4, AMOUNT_MAX = 60 };
        public static Address TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 52, LENGTH = 4, AMOUNT_MAX = 60 };
        */

        public const int SIG = 54; /*+ POSUN;*/
    }
}
