using System;
using System.Windows.Forms;

namespace SimpleClientServer
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm mainForm = new MainForm();
            MainController mainController = new MainController(mainForm);

            Application.Run(mainForm);
        }
    }
}
