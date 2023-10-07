using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nsAspur
{
    class Bytes
    {
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

        public const int SIG = 54 + POSUN;
    }
}
