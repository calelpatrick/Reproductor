using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reproductor
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Inicio frm = new Inicio();
            //frm.FormClosed += MainForm_Closed();
            //frm.Show(); 
            Application.Run( new Inicio());
        }



        private static void MainForm_Closed(object sender, FormClosedEventArgs e)
        {
            ((Form)sender).FormClosed -= MainForm_Closed;

            if (Application.OpenForms.Count == 0)
            {
                Application.ExitThread();
            }
            else
            {
                Application.OpenForms[0].FormClosed += MainForm_Closed;
            }
        }
    }
}
