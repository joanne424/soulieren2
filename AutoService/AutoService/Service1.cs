using Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AutoService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            TraceManager.CreateDefault();
            TraceManager.Debug.Write("AutoServer", "Initail log complete when start!");
            AutoTaskCore.Instance.Run();
        }

        protected override void OnStop()
        {
            AutoTaskCore.Instance.Dispose();
        }
    }
}
