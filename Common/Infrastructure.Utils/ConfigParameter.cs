// <copyright file="ConfigParameter.cs" company="Banclogix ">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>donggj</author>
// <date> 2013-10-8 </date>
// <summary>Tick报价ICE服务配置信息</summary>
// <modify>
//      修改人：donggj
//      修改时间：2013-10-9
//      修改描述：
//      版本：2.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
namespace Infrastructure.Utils
{
    #region

    using System;
    using System.Configuration;

    #endregion

    /// <summary>
    /// 配置文件参数管理
    /// </summary>
    public static class ConfigParameter
    {
        #region Static Fields

        /// <summary>
        /// 默认的拥有者Id
        /// </summary>
        public static readonly string DefaultOwnerId;

        public static readonly string SqlConnectionStr;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="ConfigParameter"/> class.
        /// </summary>
        static ConfigParameter()
        {
            try
            {
                SqlConnectionStr = ConfigurationManager.AppSettings["sqlConnectionStr"];
            }
            catch (Exception exception)
            {
                Log.TraceManager.Error.Write("ConfigParmeter", exception, "Invalid item in config file.");
                throw;
            }
        }

        #endregion
    }
}