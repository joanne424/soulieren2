// <copyright file="Customer.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>zoukp</author>
// <date> 2017/07/23 11:28:07 </date>
// <summary>  </summary>
// <modify>
//      修改人：zoukp
//      修改时间：2017/07/23 11:28:07
//      修改描述：新建 Customer.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace SOU.Model
{
    using System;

    using DM2.Ent.Presentation.Models.Base;

    [Serializable]
    public class Customer : BaseVm
    {
        #region

        /// <summary>
        ///     auto_increment
        /// </summary>
        //public int uid { get; set; }

        /// <summary>
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// </summary>
        public string pwd { get; set; }

        /// <summary>
        /// </summary>
        public string rpwd { get; set; }

        /// <summary>
        /// </summary>
        public int state { get; set; }

        /// <summary>
        /// </summary>
        public string nickname { get; set; }

        /// <summary>
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// </summary>
        public int is_send_email { get; set; }

        /// <summary>
        /// </summary>
        public int Is_send_sms { get; set; }

        /// <summary>
        /// </summary>
        public int sex { get; set; }

        /// <summary>
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// </summary>
        public string county { get; set; }

        /// <summary>
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// </summary>
        public int role { get; set; }

        /// <summary>
        /// </summary>
        public string ur { get; set; }

        /// <summary>
        /// </summary>
        public string state1 { get; set; }

        /// <summary>
        /// </summary>
        public int reg_channel { get; set; }

        /// <summary>
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// </summary>
        public string actualName { get; set; }

        /// <summary>
        /// </summary>
        public string headerimg { get; set; }

        /// <summary>
        /// </summary>
        public string birthday { get; set; }

        /// <summary>
        /// </summary>
        public string last_login_time { get; set; }

        /// <summary>
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        /// </summary>
        public string create_time { get; set; }

        /// <summary>
        /// </summary>
        public int isvirtual { get; set; }

        #endregion
    }
}