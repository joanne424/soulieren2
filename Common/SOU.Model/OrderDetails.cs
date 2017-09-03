using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOU.Model
{
    public class OrderDetails
    {
        #region Prop
        /// <summary>
        /// auto_increment
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string order_sn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int user_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? best_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? shipping_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? shipping_num { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string shipping_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pay_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pay_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal goods_amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal shipping_fee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? balance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal paid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal money_paid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime create_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? confirm_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime shipping_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime pay_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime done_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int order_channel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int order_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? invoice_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string invoice_title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string depot_sn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime modify_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? modify_user { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cancel_text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? is_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string web_source { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? depot_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int isvirtual { get; set; }
        #endregion Model
    }
}
