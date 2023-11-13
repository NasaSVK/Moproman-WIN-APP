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
        public Address NAPATIE; 
        public Address PRUD; 
        public Address SOBERT_VSTUP; 
        public Address SOBERT_VYKON; 
        public Address VYKON; 
        public Address PRISPOSOBENIE; 

        public Address TLAK_VODY;
        public Address PRIETOK_VODY;

        public Address TEPLOTA_VODY_VSTUP; 
        public Address TEPLOTA_VODY_VYSTUP;
        public Address TEPLOTA_VODY_P1;
        public Address TEPLOTA_VODY_P2;

        public Address FREKVENCIA;
        public Address TEPLOTA_VODY_PRIMARNY_OKRUH;

        public Addresses(string pPec)
        {

            switch (pPec)
            {

                case ("PEC_A"):
                    int DB4_MAX = 4; int DB1_MAX = 8;
                    FREKVENCIA = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 4, OFFSET = 5, LENGTH = 4, AMOUNT_MAX = MaxBuffer.DB, dlgPrepocet = (float pY) => pY };                    
                    TEPLOTA_VODY_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 0, LENGTH = 4, AMOUNT_MAX = DB1_MAX , dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 4, LENGTH = 4, AMOUNT_MAX = DB1_MAX, dlgPrepocet = (float pY) => pY }; 
                    break;
                case ("PEC_B"):
                    DB1_MAX = 60; int MK0_MAX = 168;
                    NAPATIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 140, LENGTH = 4, AMOUNT_MAX = MK0_MAX,dlgPrepocet = (float pY) => 1000 * (pY - 9) / 66 };
                    PRUD = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 144, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => 400 * pY / 160 };
                    SOBERT_VSTUP = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 100, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) =>pY };
                    SOBERT_VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 104, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => pY };
                    VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 148, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => 150*(pY-500)/4100};
                    PRISPOSOBENIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 164, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => 2.4f*(pY+78)/138-0.9f};
                    TLAK_VODY = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 56, LENGTH = 4, AMOUNT_MAX = DB1_MAX, dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 48, LENGTH = 4, AMOUNT_MAX = DB1_MAX , dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 52, LENGTH = 4, AMOUNT_MAX = DB1_MAX , dlgPrepocet = (float pY) => pY }; 
                    break;
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
                    TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 48, LENGTH = 4, AMOUNT_MAX = DB1_MAX , dlgPrepocet = (float pY) => pY }; 
                    break;
                case ("PEC_D"):
                    DB1_MAX = 68; MK0_MAX = 168;
                    NAPATIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 140, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => 600*pY/68f };
                    PRUD = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 144, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => 400 * pY / 110 };
                    SOBERT_VSTUP = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 100, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => pY };
                    SOBERT_VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 104, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => pY };
                    VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 148, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => 200 * (pY - 100) / 6350 };
                    PRISPOSOBENIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 164, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => 1.9f*(pY + 78)/138};
                    TLAK_VODY = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 56, LENGTH = 4, AMOUNT_MAX = DB1_MAX, dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 60, LENGTH = 4, AMOUNT_MAX = DB1_MAX, dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 64, LENGTH = 4, AMOUNT_MAX = DB1_MAX, dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_P1 = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 48, LENGTH = 4, AMOUNT_MAX = DB1_MAX, dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_P2 = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 52, LENGTH = 4, AMOUNT_MAX = DB1_MAX, dlgPrepocet = (float pY) => pY };
                    break;       
                case ("PEC_G"):
                    DB1_MAX = 68; MK0_MAX = 168;
                    NAPATIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 140, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY)=> 600*(pY-8.55f)/30f};
                    PRUD = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 144, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => 400 * pY / 120 };
                    SOBERT_VSTUP = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 100, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => pY };
                    SOBERT_VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 104, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => pY };
                    VYKON = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 148, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => 200 * (pY - 300) / 3600};
                    PRISPOSOBENIE = new Address() { AREA = S7Consts.S7AreaMK, DB_NUMBER = 0, OFFSET = 164, LENGTH = 4, AMOUNT_MAX = MK0_MAX, dlgPrepocet = (float pY) => 2.08f * (pY + 84) / 142 };
                    TEPLOTA_VODY_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 60, LENGTH = 4, AMOUNT_MAX = DB1_MAX, dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 64, LENGTH = 4, AMOUNT_MAX = DB1_MAX, dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_P1 = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 48, LENGTH = 4, AMOUNT_MAX = DB1_MAX, dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_P2 = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 56, LENGTH = 4, AMOUNT_MAX = DB1_MAX, dlgPrepocet = (float pY) => pY };
                    PRIETOK_VODY = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 1, OFFSET = 52, LENGTH = 4, AMOUNT_MAX = DB1_MAX, dlgPrepocet = (float pY) => pY };
                    break;             
                case ("PEC_H"):
                    int DB29_MAX = 36; int DB39_MAX = 168;
                    FREKVENCIA = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 29, OFFSET = 32, LENGTH = 4, AMOUNT_MAX = DB29_MAX, dlgPrepocet = (float pY) => pY };
                    NAPATIE = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 29, OFFSET = 0, LENGTH = 4, AMOUNT_MAX = DB29_MAX, dlgPrepocet = (float pY) => 600 * pY / 5f };
                    PRUD = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 29, OFFSET = 4, LENGTH = 4, AMOUNT_MAX = DB29_MAX, dlgPrepocet = (float pY) => 400 * pY / 15 };                    
                    VYKON = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 29, OFFSET = 28, LENGTH = 4, AMOUNT_MAX = DB29_MAX, dlgPrepocet = (float pY) => pY };
                    PRISPOSOBENIE = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 29, OFFSET = 20, AMOUNT_MAX = DB29_MAX, dlgPrepocet = (float pY) => 2*(pY + 9.9f)/19.8f };
                    
                    PRIETOK_VODY = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 39, OFFSET = 4, LENGTH = 4, AMOUNT_MAX = DB39_MAX, dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_VSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 39, OFFSET = 16, LENGTH = 4, AMOUNT_MAX = DB39_MAX, dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_VYSTUP = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 39, OFFSET = 20, LENGTH = 4, AMOUNT_MAX = DB39_MAX, dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_P1 = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 39, OFFSET = 8, LENGTH = 4, AMOUNT_MAX = DB39_MAX, dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_P2 = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 39, OFFSET = 12, LENGTH = 4, AMOUNT_MAX = DB39_MAX, dlgPrepocet = (float pY) => pY };
                    TEPLOTA_VODY_PRIMARNY_OKRUH = new Address() { AREA = S7Consts.S7AreaDB, DB_NUMBER = 39, OFFSET = 24, LENGTH = 4, AMOUNT_MAX = DB39_MAX, dlgPrepocet = (float pY) => pY };
                    break;
            }
        }
    }
}




