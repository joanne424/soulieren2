// <copyright file="App.xaml.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>wangmy</author>
// <date> 2016/11/01 05:12:15 </date>
// <summary>  </summary>
// <modify>
//      修改人：wangmy
//      修改时间：2016/11/01 05:12:15
//      修改描述：新建 App.xaml.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace DM2.Ent.Client.Views
{
    #region

    using System;
    using System.Windows;
    using System.Windows.Threading;

    using Infrastructure.Log;

    #endregion

    /// <summary>
    ///     App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the App class.
        /// </summary>
        public App()
        {
            try
            {
                TraceManager.CreateDefault();
                AppDomain.CurrentDomain.UnhandledException += this.AppdomainUnhandledException;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 处理UI线程触发的异常
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unknown Exception:" + e.Exception.Message);
            TraceManager.Error.Write("Application.DispatcherUnhandledException", e.Exception);
            e.Handled = true;
        }

        /// <summary>
        /// 处理Appdomain内线程触发的异常
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AppdomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            TraceManager.Error.Write(
                "Appdomain.UnhandledException", 
                e.ExceptionObject as Exception, 
                "IsTerminating: {0}", 
                e.IsTerminating);
        }

        #endregion
    }
}