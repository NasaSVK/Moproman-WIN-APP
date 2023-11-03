using Sharp7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsAspur
{
    internal class Addresses
    {

        public Address NAPATIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 140, LENGTH = 4, AMOUNT_MAX = MaxBuffer.MK };
        public Address PRUD = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 144, LENGTH = 4, AMOUNT_MAX = MaxBuffer.MK };
        public Address SOBERT_VSTUP = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 100, LENGTH = 4, AMOUNT_MAX = MaxBuffer.MK };
        public Address SOBERT_VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 104, LENGTH = 4, AMOUNT_MAX = MaxBuffer.MK };
        public Address VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 148, LENGTH = 4, AMOUNT_MAX = MaxBuffer.MK };
        public Address PRISPOSOBENIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 164, LENGTH = 4, AMOUNT_MAX = MaxBuffer.MK };

        public Address TLAK_VODY = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 56, LENGTH = 4, AMOUNT_MAX = MaxBuffer.DB };
        public Address TEPLOTA_VODY_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 60, LENGTH = 4, AMOUNT_MAX = MaxBuffer.DB };
        public Address TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 48, LENGTH = 4, AMOUNT_MAX = MaxBuffer.DB };

        public Addresses(string pPec)
        {

            switch (pPec)
            {

                case ("PEC_A"):
                    int DB4_MAX = 4; int DB1_MAX = 16; int MK0_MAX = 0;                    
                    NAPATIE = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 4, OFFSET = 0, LENGTH = 4, AMOUNT_MAX = DB4_MAX };
                        PRUD = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 0, LENGTH = 4, AMOUNT_MAX = DB1_MAX };
                    SOBERT_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 8, LENGTH = 4, AMOUNT_MAX = DB1_MAX };
                    SOBERT_VYKON = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 12, LENGTH = 4, AMOUNT_MAX = DB1_MAX };
                        VYKON = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 0, LENGTH = 4, AMOUNT_MAX = DB1_MAX };
                        PRISPOSOBENIE = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 0, LENGTH = 4, AMOUNT_MAX = DB1_MAX };
                        TLAK_VODY = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 0, LENGTH = 4, AMOUNT_MAX = DB1_MAX };                    
                    TEPLOTA_VODY_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 0, LENGTH = 4, AMOUNT_MAX = DB1_MAX };
                    TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 4, LENGTH = 4, AMOUNT_MAX = DB1_MAX }; break;
                case ("PEC_B"):
                    DB4_MAX = 4; DB1_MAX = 60; MK0_MAX = 168;
                    NAPATIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 140, LENGTH = 4, AMOUNT_MAX = MK0_MAX };
                    PRUD = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 144, LENGTH = 4, AMOUNT_MAX = MK0_MAX };
                    SOBERT_VSTUP = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 100, LENGTH = 4, AMOUNT_MAX = MK0_MAX };
                    SOBERT_VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 104, LENGTH = 4, AMOUNT_MAX = MK0_MAX };
                    VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 148, LENGTH = 4, AMOUNT_MAX = MK0_MAX };
                    PRISPOSOBENIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 164, LENGTH = 4, AMOUNT_MAX = MK0_MAX };
                    TLAK_VODY = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 56, LENGTH = 4, AMOUNT_MAX = DB1_MAX };
                    TEPLOTA_VODY_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 48, LENGTH = 4, AMOUNT_MAX = DB1_MAX };
                    TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 52, LENGTH = 4, AMOUNT_MAX = DB1_MAX }; break;
                case ("PEC_C"):
                    DB1_MAX = 64; MK0_MAX = 168;
                    NAPATIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 140, LENGTH = 4, AMOUNT_MAX = MK0_MAX };
                    PRUD = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 144, LENGTH = 4, AMOUNT_MAX = MK0_MAX };
                    SOBERT_VSTUP = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 100, LENGTH = 4, AMOUNT_MAX = MK0_MAX };
                    SOBERT_VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 104, LENGTH = 4, AMOUNT_MAX = MK0_MAX };
                    VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 148, LENGTH = 4, AMOUNT_MAX = MK0_MAX };
                    PRISPOSOBENIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 164, LENGTH = 4, AMOUNT_MAX = MK0_MAX };
                    TLAK_VODY = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 56, LENGTH = 4, AMOUNT_MAX = DB1_MAX };
                    TEPLOTA_VODY_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 60, LENGTH = 4, AMOUNT_MAX = DB1_MAX };
                    TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 48, LENGTH = 4, AMOUNT_MAX = DB1_MAX }; break;

            }
        }
    }
}




