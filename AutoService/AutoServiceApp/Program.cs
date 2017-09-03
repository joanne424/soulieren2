using Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TraceManager.CreateDefault();
            TraceManager.Debug.Write("AutoServer", "Initail log complete when start!");
            AutoTaskCore.Instance.Run();
            Console.ReadKey();
        }
    }
}
