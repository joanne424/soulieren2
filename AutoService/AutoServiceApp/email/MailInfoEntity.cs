// <copyright file="MailInfoEntity.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>zoukp</author>
// <date> 2016/12/26 05:44:11 </date>
// <summary>  </summary>
// <modify>
//      修改人：zoukp
//      修改时间：2016/12/26 05:44:11
//      修改描述：新建 MailInfoEntity.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace AutoServiceApp
{
    using System;

    /// <summary>
    /// The mail info entity.
    /// </summary>
    public class MailInfoEntity
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the body encoding.
        /// </summary>
        public string BodyEncoding { get; set; }

        /// <summary>
        /// Gets or sets the cc.
        /// </summary>
        public string CC { get; set; }

        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        //public string CreateTime { get; set; }

        /// <summary>
        /// Gets or sets the from address.
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// Gets or sets the from host.
        /// </summary>
        public string FromHost { get; set; }

        /// <summary>
        /// Gets or sets the from port.
        /// </summary>
        public int FromPort { get; set; }

        /// <summary>
        /// Gets or sets the from pwd.
        /// </summary>
        public string FromPwd { get; set; }

        /// <summary>
        /// Gets or sets the from user.
        /// </summary>
        public string FromUser { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is html.
        /// </summary>
        public bool IsHtml { get; set; }

        /// <summary>
        /// Gets or sets the last update time.
        /// </summary>
       // public string LastUpdateTime { get; set; }

        /// <summary>
        /// Gets or sets the operate id.
        /// </summary>
        public int OperateId { get; set; }

        /// <summary>
        /// Gets or sets the opt type.
        /// </summary>
        public int OptType { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the subject encoding.
        /// </summary>
        public string SubjectEncoding { get; set; }

        /// <summary>
        /// Gets or sets the to address.
        /// </summary>
        public string ToAddress { get; set; }

        /// <summary>
        /// Gets or sets the trytime.
        /// </summary>
        public int Trytime { get; set; }

        #endregion
    }
}