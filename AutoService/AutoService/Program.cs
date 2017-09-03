// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/01/22 01:40:38 </date>
// <summary>
//   The program.
// </summary>
// <modify>
//      修改人：zoukp
//      修改时间：2017/01/22 01:40:38
//      修改描述：新建 Program.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace AutoService
{
    using System.ServiceProcess;

    /// <summary>
    /// The program.
    /// </summary>
    internal static class Program
    {
        #region Methods

        /// <summary>
        ///     应用程序的主入口点。
        /// </summary>
        private static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new Service1() };
            ServiceBase.Run(ServicesToRun);
        }

        #endregion
    }
}