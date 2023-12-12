using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nsMOPROMAN
{
    static class Config
    {
        public static int DISPLAYED_RECORDS;

        public static void LoadConfig() {

            DISPLAYED_RECORDS = Properties.Settings.Default.DISPLAYED_RECS;
        }
    }
}
