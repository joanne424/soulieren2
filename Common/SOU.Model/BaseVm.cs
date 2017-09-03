// <copyright file="BaseVM.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2014/2/20 13:49:49 </date>
// <summary> ViewModle的基类 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2014/2/20 13:49:49
//      修改描述：新建 BaseVM.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace DM2.Ent.Presentation.Models.Base
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Caliburn.Micro;

    /// <summary>
    /// VM基类
    /// </summary>
    public class BaseVm : Screen, IDataErrorInfo
    {
        /// <summary>
        /// 日期格式化模板
        /// </summary>
        public const string DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

        #region Flds

        /// <summary>
        /// 错误 字段
        /// </summary>
        private string error;

        /// <summary>
        /// 错误信息列表
        /// </summary>
        private HashSet<string> properties;

        /// <summary>
        /// 拥有者ID
        /// </summary>
        private string ownerId = "string";

        /// <summary>
        /// 仓储缓存字典
        /// </summary>
        //private Dictionary<Type, IRepositoryCommon> repositoryDic = new Dictionary<Type, IRepositoryCommon>();

        /// <summary>
        /// Service缓存字典
        /// </summary>
        private Dictionary<Type, IDisposable> serviceDic = new Dictionary<Type, IDisposable>();

        /// <summary>
        /// VM所属窗口是否已经关闭
        /// </summary>
        private bool isClosed = false;

        private string _name;
        #endregion

        #region Ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseVm"/> class.
        /// </summary>
        public BaseVm()
        {
            properties = new HashSet<string>();
            this.ValidationStep = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseVm"/> class.
        /// </summary>
        /// <param name="varOwnerId">拥有者Id</param>
        public BaseVm(string varOwnerId)
            : this()
        {
            if (string.IsNullOrEmpty(varOwnerId))
            {
                return;
            }

            this.ownerId = varOwnerId;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="BaseVm"/> class. 
        /// </summary>
        //~BaseVm()
        //{
        //    this.ClearRepository();
        //}

        #endregion

        #region Props

        public virtual string _Name
        {
            get
            {
                return this._name;
            }

            set
            {
                this._name = value;
                this.NotifyOfPropertyChange("_Name");
            }
        }

        /// <summary>
        /// 错误 属性
        /// </summary>
        public string Error
        {
            get
            {
                return this.error;
            }

            protected set
            {
                this.error = value;
                this.NotifyOfPropertyChange("Error");
            }
        }

        /// <summary>
        /// Gets or sets the ValidationStep property.
        /// </summary>
        public int ValidationStep { get; set; }

        /// <summary>
        /// 是否已经关闭
        /// </summary>
        public bool IsClosed
        {
            get { return this.isClosed; }
        }

        /// <summary>
        /// 拥有者Id
        /// </summary>
        protected string OwnerId
        {
            get
            {
                if (string.IsNullOrEmpty(this.ownerId))
                {
                    this.ownerId = "";
                }

                return this.ownerId;
            }
        }

        /// <summary>
        /// 是否处于测试中
        /// </summary>
        protected bool IsInTest
        {
            get { return this.ownerId != ""; }
        }

        /// <summary>
        /// 索引
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns>返回错误信息</returns>
        //string IDataErrorInfo.this[string propertyName]
        //{
        //    get
        //    {
        //        var message = this.Validate(propertyName);
        //        if (properties.Contains(propertyName))
        //        {
        //            return this.Error = message;
        //        }

        //        properties.Add(propertyName);
        //        return null;
        //    }
        //}

        #endregion

        #region OverideMeths

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void TryClose()
        {
            this.isClosed = true;
            try
            {
                base.TryClose();
            }
            catch
            {
                this.TryClose(false);
            }
        }

        /// <summary>
        /// 关闭窗口重载
        /// </summary>
        /// <param name="dialogResult">确认框结果值</param>
        public override void TryClose(bool? dialogResult)
        {
            this.isClosed = true;
            base.TryClose(dialogResult);
        }

        #endregion

        #region ProtectMeths

        public void SetDisplayName(string displayName)
        {
            //System.Windows.Application.Current.Dispatcher.Invoke(
            //    new System.Action(
            //        () =>
            //        {
            //            this.DisplayName = displayName;
            //        }));
        }

        /// <summary>
        /// 获取仓储实现
        /// 如果此实例已经获取过一次会自动缓存
        /// </summary>
        /// <typeparam name="T">仓储接口</typeparam>
        /// <returns>接口实现</returns>
        //public T GetRepository<T>() where T : IRepositoryCommon
        //{
        //    if (this.repositoryDic.ContainsKey(typeof(T)))
        //    {
        //        return (T)this.repositoryDic[typeof(T)];
        //    }
        //    else
        //    {
        //        var parameter = new ParameterOverrides { { "varOwnerId", this.ownerId } };
        //        var rep = IocContainer.Instance.Container.Resolve<T>(parameter);
        //        this.repositoryDic.Add(typeof(T), rep);
        //        return rep;
        //    }
        //}

        /// <summary>
        /// 销毁其中的仓储
        /// </summary>
        //protected void ClearRepository()
        //{
        //    foreach (var item in this.repositoryDic.Values)
        //    {
        //        var repositoryCommon = item as IRepositoryCommon;
        //        if (repositoryCommon == null)
        //        {
        //            return;
        //        }

        //        repositoryCommon.Dispose();
        //    }

        //    this.repositoryDic.Clear();
        //}

        /// <summary>
        /// 清空Service
        /// </summary>
        //protected void ClearService()
        //{
        //    this.serviceDic.Clear();
        //}

        /// <summary>
        /// 获取Sevice
        /// </summary>
        /// <typeparam name="TService">泛型Service</typeparam>
        /// <returns>获取到的Sevice实例</returns>
        //protected TService GetSevice<TService>() where TService : IDisposable
        //{
        //    try
        //    {
        //        if (this.serviceDic.ContainsKey(typeof(TService)))
        //        {
        //            return (TService)this.serviceDic[typeof(TService)];
        //        }
        //        else
        //        {
        //            var service = (TService)Activator.CreateInstance(typeof(TService), this.ownerId);
        //            this.serviceDic.Add(typeof(TService), service);
        //            return service;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Infrastructure.Log.TraceManager.Error.Write("QuoteManager", ex, "获取Service异常");
        //        return default(TService);
        //    }
        //}

        /// <summary>
        /// 重置所有的错误。
        /// </summary>
        //protected void ResetErrors()
        //{
        //    properties.Clear();
        //}

        /// <summary>
        /// 根据商品ID转换成商品名称
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="contractID">商品id</param>
        /// <returns>商品名称</returns>
        //protected string ConvertNameByID<T>(string contractID) where T : IRepositoryCommon
        //{
        //    var symbolRepo = this.GetRepository<T>();
        //    return symbolRepo.GetName(contractID);
        //}

        /// <summary>
        /// 提交时验证
        /// </summary>
        /// <returns>返回结果</returns>
        //protected virtual bool ValidateforSumbit()
        //{
        //    bool result = true;
        //    foreach (var i in properties)
        //    {
        //        this.NotifyOfPropertyChange(i);
        //        if (this.Error == null)
        //        {
        //            continue;
        //        }

        //        return false;
        //    }

        //    string logicError = this.OnValidated();
        //    if (string.IsNullOrEmpty(logicError) == false)
        //    {
        //        this.Error = logicError;
        //        result = false;
        //    }
        //    else
        //    {
        //        this.Error = null;
        //    }

        //    return result;
        //}

        /// <summary>
        /// 提交时验证
        /// </summary>
        /// <param name="step">The validating step.</param>
        /// <returns>返回结果</returns>
        //protected bool ValidateforSumbit(int step)
        //{
        //    this.ValidationStep = step;
        //    return this.ValidateforSumbit();
        //}

        /// <summary>
        /// 业务逻辑的验证(非属性的验证)
        /// </summary>
        /// <returns>错误信息</returns>
        protected virtual string OnValidated()
        {
            return null;
        }

        /// <summary>
        /// 验证时
        /// </summary>
        /// <param name="propertyName">属性</param>
        /// <param name="handle">是否处理该条验证</param>
        /// <returns>返回结果</returns>
        protected virtual string OnValidating(string propertyName, ref bool handle)
        {
            return null;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="propertyName">属性</param>
        /// <returns>返回结果</returns>
        protected virtual string OnValidate(string propertyName)
        {
            return null;
        }

        public virtual void Dispose()
        {
        }

        #endregion

        #region PriMeths

        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns>返回结果</returns>
        //private string Validate(string propertyName)
        //{
        //    var handle = false;
        //    var message = this.OnValidating(propertyName, ref handle);
        //    if (handle || message != null)
        //    {
        //        return message;
        //    }

        //    var result = ValidationContext.Instance.Validation(this, propertyName, this.ValidationStep);
        //    if (result.Success)
        //    {
        //        return this.OnValidate(propertyName);
        //    }

        //    if (result.MessageKey != null)
        //    {
        //        string msg = (string)Application.Current.TryFindResource(result.MessageKey);

        //        if (string.IsNullOrEmpty(msg) == true)
        //        {
        //            return "Can't find message key.";
        //        }
        //        else
        //        {
        //            return msg;
        //        }
        //    }

        //    return result.Message;
        //}

        #endregion


        public string this[string columnName]
        {
            get
            {
                return string.Empty;
            }
        }
    }
}