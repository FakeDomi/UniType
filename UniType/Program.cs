using System;
using System.Windows.Forms;

namespace domi1819.UniType
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ReSharper disable once UnusedVariable
            InputForm mainForm = new InputForm();

            Application.Run();
        }
    }
}
