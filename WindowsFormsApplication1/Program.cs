using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infrastructure.Log;

namespace SendMSM
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            TraceManager.CreateDefault();
            TraceManager.Debug.Write("AutoServer", "Initail log complete when start!");
            Application.Run(new Form1());
        }
    }
}
