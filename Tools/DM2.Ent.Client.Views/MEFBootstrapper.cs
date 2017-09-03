// <copyright file="MEFBootstrapper.cs" company="Banclogix ">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>董国君</author>
// <date> 2013-08-12 </date>
// <summary>启动类</summary>
// <modify>
//      修改人：董国君
//      修改时间：2013-08-12
//      修改描述：新建
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
    using System.Collections.Generic;
    using System.Windows;

    using Caliburn.Micro;

    #endregion

    /// <summary>
    /// 启动类
    /// </summary>
    public class MEFBootstrapper : BootstrapperBase
    {
        #region Fields

        /// <summary>
        /// 集成容器
        /// </summary>
        private SimpleContainer container;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MEFBootstrapper"/> class.
        /// </summary>
        public MEFBootstrapper()
        {
            this.Initialize();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 重载基类的BuildUp方法
        /// </summary>
        /// <param name="instance">
        /// 实例
        /// </param>
        protected override void BuildUp(object instance)
        {
            this.container.BuildUp(instance);
        }

        /// <summary>
        /// 重载基类的 Configure方法
        /// </summary>
        protected override void Configure()
        {
            this.container = new SimpleContainer();
            this.container.Singleton<IWindowManager, WindowManager>();
            this.container.PerRequest<IShell, ShellViewModel>();
        }

        /// <summary>
        /// 获取所有实例
        /// </summary>
        /// <param name="service">
        /// 服务
        /// </param>
        /// <returns>
        /// 返回实例集合
        /// </returns>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return this.container.GetAllInstances(service);
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="service">
        /// 服务
        /// </param>
        /// <param name="key">
        /// Key
        /// </param>
        /// <returns>
        /// 返回值
        /// </returns>
        protected override object GetInstance(Type service, string key)
        {
            var instance = this.container.GetInstance(service, key);
            if (instance != null)
            {
                return instance;
            }

            throw new Exception("Could not locate any instances.");
        }

        /// <summary>
        /// 启动方法
        /// </summary>
        /// <param name="sender">
        /// 事件对象
        /// </param>
        /// <param name="e">
        /// 事件参数
        /// </param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            this.DisplayRootViewFor<IShell>();
        }

        #endregion
    }
}