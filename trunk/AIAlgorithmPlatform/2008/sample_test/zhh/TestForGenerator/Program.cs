using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using M2M.Position;
using M2M.Util;
using Position_Implement;
using PositionSet3D = System.Collections.Generic.List<M2M.Position.Position3D>;
using Position3DSet = System.Collections.Generic.List<M2M.Position.IPosition3D>;

namespace TestForGenerator
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
            Application.Run(new LaunchTest());
        }
    }
}
