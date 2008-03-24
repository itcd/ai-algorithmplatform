using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PositionSet_Connected_EditorDemo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new EditorDemoForm());
        }
    }
}