using Sharp7;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;

namespace nsAspur
{


    class MaxBuffer { 
        public const int DB = 60;
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
    }

    class Bytes
    {
        public static Address NAPATIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 140, LENGTH = 4, AMOUNT_MAX =148};
        public static Address PRUD = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 144, LENGTH = 4, AMOUNT_MAX = 148 };
        public static Address SOBERT_VSTUP = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 100, LENGTH = 4, AMOUNT_MAX = 148 };
        public static Address SOBERT_VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 104, LENGTH = 4, AMOUNT_MAX = 148 };
        public static Address VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 148, LENGTH = 4, AMOUNT_MAX = 148 };
        public static Address PRISPOSOBENIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 164, LENGTH = 4, AMOUNT_MAX = 148 };

        public static Address TLAK_VODY = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 56, LENGTH = 4, AMOUNT_MAX = 60 };
        public static Address TEPLOTA_VODY_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 48, LENGTH = 4, AMOUNT_MAX = 60 };
        public static Address TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 52, LENGTH = 4, AMOUNT_MAX = 60 };

        /*
        private const int POSUN = 0;

        public const int SHIFT = 0 + POSUN;
        public const int CYCLE_TIME = 2 + POSUN;

        public const int R1_B = 6 + POSUN;
        public const int R1_S = 7 + POSUN;
        public const int R1_F = 8 + POSUN;

        public const int R2_B = 9 + POSUN;
        public const int R2_S = 10 + POSUN;
        public const int R2_F = 11 + POSUN;        
        
        public const int OK_ = 12 + POSUN;
        public const int NOK_ = 16 + POSUN;

        public const int CNT_OKR1 = 20 + POSUN;
        public const int CNT_OKR2 = 24 + POSUN;

        public const int CNT_OK = 28 + POSUN;
        public const int CNT_NOK = 32 + POSUN;

        public const int R1_ = 36 + POSUN;
        public const int R2_ = 40 + POSUN;

        public const int ERROR = 44 + POSUN;
        public const int ERROR_TIME = 46 + POSUN;
        public const int WAITING_TIME = 50 + POSUN;
               
        public const int TESTString1 = 64 + POSUN;
        public const int TESTString2 = 74 + POSUN;
        */
        public const int SIG = 54; /*+ POSUN;*/
    }
}
