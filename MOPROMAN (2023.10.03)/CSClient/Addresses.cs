using Sharp7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
                    NAPATIE = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 4, OFFSET = 0, LENGTH = 4, AMOUNT_MAX = DB4_MAX, dlgPrepocet = (float pY) => pY };
                    PRUD = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 0, LENGTH = 4, AMOUNT_MAX = DB1_MAX , dlgPrepocet = (float pY) => pY };
                    SOBERT_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 8, LENGTH = 4, AMOUNT_MAX = DB1_MAX , dlgPrepocet = (float pY) => pY };
                    SOBERT_VYKON = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 12, LENGTH = 4, AMOUNT_MAX = DB1_MAX , dlgPrepocet = (float pY) => pY };
                        VYKON = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 0, LENGTH = 4, AMOUNT_MAX = DB1_MAX , dlgPrepocet = (float pY) => pY };
                        PRISPOSOBENIE = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 0, LENGTH = 4, AMOUNT_MAX = DB1_MAX , dlgPrepocet = (float pY) => pY };
                        TLAK_VODY = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 0, LENGTH = 4, AMOUNT_MAX = DB1_MAX , dlgPrepocet = (float pY) => pY };                    
                    TEPLOTA_VODY_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 0, LENGTH = 4, AMOUNT_MAX = DB1_MAX , dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 4, LENGTH = 4, AMOUNT_MAX = DB1_MAX, dlgPrepocet = (float pY) => pY }; break;
                case ("PEC_B"):
                    DB4_MAX = 4; DB1_MAX = 60; MK0_MAX = 168;
                    NAPATIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 140, LENGTH = 4, AMOUNT_MAX = MK0_MAX,dlgPrepocet = (float pY) => 1000 * (pY - 9) / 66 };
                    PRUD = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 144, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => 400 * pY / 160 };
                    SOBERT_VSTUP = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 100, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) =>pY };
                    SOBERT_VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 104, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => pY };
                    VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 148, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => 150*(pY-500)/4100};
                    PRISPOSOBENIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 164, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => 2.4f*(pY+78)/138-0.9f};
                    TLAK_VODY = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 56, LENGTH = 4, AMOUNT_MAX = DB1_MAX, dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 48, LENGTH = 4, AMOUNT_MAX = DB1_MAX , dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 52, LENGTH = 4, AMOUNT_MAX = DB1_MAX , dlgPrepocet = (float pY) => pY }; break;
                case ("PEC_C"):
                    DB1_MAX = 64; MK0_MAX = 168;
                    NAPATIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 140, LENGTH = 4, AMOUNT_MAX = MK0_MAX , dlgPrepocet = (float pY) => 600 * (pY - 8.1f) / 59.9f };
                    PRUD = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 144, LENGTH = 4, AMOUNT_MAX = MK0_MAX , dlgPrepocet = (float pY) => 400 * pY / 110 };
                    SOBERT_VSTUP = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 100, LENGTH = 4, AMOUNT_MAX = MK0_MAX , dlgPrepocet = (float pY) => pY };
                    SOBERT_VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 104, LENGTH = 4, AMOUNT_MAX = MK0_MAX , dlgPrepocet = (float pY) => pY };
                    VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 148, LENGTH = 4, AMOUNT_MAX = MK0_MAX , dlgPrepocet = (float pY) => 200*(pY-310)/5430};
                    PRISPOSOBENIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 164, LENGTH = 4, AMOUNT_MAX = MK0_MAX , dlgPrepocet = (float pY) => 2*(pY+64)/119};
                    TLAK_VODY = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 56, LENGTH = 4, AMOUNT_MAX = DB1_MAX , dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 60, LENGTH = 4, AMOUNT_MAX = DB1_MAX , dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 48, LENGTH = 4, AMOUNT_MAX = DB1_MAX , dlgPrepocet = (float pY) => pY }; break;

            }
        }
    }
}




