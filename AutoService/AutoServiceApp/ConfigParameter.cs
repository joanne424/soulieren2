﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigParameter.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace AutoServiceApp
{
    using System.Configuration;

    /// <summary>
    ///     The config parameter.
    /// </summary>
    public class ConfigParameter
    {
        #region Static Fields

        /// <summary>
        ///     The instance.
        /// </summary>
        private static readonly ConfigParameter instance = new ConfigParameter();

        #endregion

        #region Fields

        /// <summary>
        ///     The email interval.
        /// </summary>
        public readonly string emailInterval = ConfigurationManager.AppSettings["emailInterval"];

        /// <summary>
        ///     The my sql connection str.
        /// </summary>
        public readonly string mySqlConnectionStr = ConfigurationManager.AppSettings["mySqlConnectionStr"];

        /// <summary>
        ///     The order expired.
        /// </summary>
        public readonly string orderExpired = ConfigurationManager.AppSettings["orderExpired"];

        /// <summary>
        ///     The remo order interval.
        /// </summary>
        public readonly string removeOrderInterval = ConfigurationManager.AppSettings["removeOrderInterval"];

        /// <summary>
        ///     The sms internal.
        /// </summary>
        public readonly int smsInternal = int.Parse(ConfigurationManager.AppSettings["smsInternal"]);

        /// <summary>
        ///     The from host.
        /// </summary>
        public readonly string FromHost = ConfigurationManager.AppSettings["fromHost"];

        /// <summary>
        ///     The from port.
        /// </summary>
        public readonly string FromPort = ConfigurationManager.AppSettings["fromPort"];

        /// <summary>
        ///     The from pwd.
        /// </summary>
        public readonly string FromPwd = ConfigurationManager.AppSettings["fromPwd"];

        /// <summary>
        ///     The from user.
        /// </summary>
        public readonly string FromUser = ConfigurationManager.AppSettings["fromUser"];

        /// <summary>
        /// The sms cecret.
        /// </summary>
        public readonly string smsCecret = ConfigurationManager.AppSettings["smsCecret"];

        /// <summary>
        /// The sms key.
        /// </summary>
        public readonly string smsKey = ConfigurationManager.AppSettings["smsKey"];

        /// <summary>
        /// The sms url.
        /// </summary>
        public readonly string smsUrl = ConfigurationManager.AppSettings["smsUrl"];

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static ConfigParameter Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion
    }
}