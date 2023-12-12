using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace nsMOPROMAN
{

    //http://www.codescratcher.com/windows-forms/single-instance-windows-form-application-in-c/
    static class Program
    {

        private static Mutex mutex = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {        
            //Settings for single instance app start
            const string appName = "SingleInstanceApp";
            bool createdNew;
            mutex = new Mutex(true, appName, out createdNew);
            if (!createdNew)
            {
                //app is already running! Exiting the application  
                MessageBox.Show("Aplikácia je už spustená!","Chyba",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            //Settings for single instance app end

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(args));
        }
    }
}
