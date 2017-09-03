// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SmsEntity.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/01/22 01:41:41 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/01/22 01:41:41
//      修改描述：新建 SmsEntity.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace AutoService
{
    using System;

    /// <summary>
    /// The sms entity.
    /// </summary>
    public class SmsEntity
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the create_time.
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Gets or sets the last_update_time.
        /// </summary>
        public DateTime last_update_time { get; set; }

        /// <summary>
        /// Gets or sets the operate_id.
        /// </summary>
        public int operate_id { get; set; }

        /// <summary>
        /// Gets or sets the opt_type.
        /// </summary>
        public int opt_type { get; set; }

        /// <summary>
        /// Gets or sets the param_list.
        /// </summary>
        public string param_list { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public int priority { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether result.
        /// </summary>
        public bool result { get; set; }

        /// <summary>
        /// Gets or sets the result_body.
        /// </summary>
        public string result_body { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// Gets or sets the template_id.
        /// </summary>
        public string template_id { get; set; }

        /// <summary>
        /// Gets or sets the to_phone.
        /// </summary>
        public string to_phone { get; set; }

        /// <summary>
        /// Gets or sets the try_time.
        /// </summary>
        public int try_time { get; set; }

        #endregion
    }
}