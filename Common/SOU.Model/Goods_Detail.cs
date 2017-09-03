namespace SOU.Model
{
    using System;

    using DM2.Ent.Presentation.Models.Base;

    public class Goods_Detail : BaseVm
    {
        /// <summary>
        /// auto_increment
        /// </summary>
        public int id { get; set; }

        public string goods_name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int gid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? y_price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal g_price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imgfirst { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string creation_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int spec_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sample { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mod_user { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime order_time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? first_buy_num { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? first_user_buy_num { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string weight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? postage_state { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int recommend { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime mod_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime create_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int scid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? mounting { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string send_goods { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int spec_goods { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int is_delete { get; set; }
    }
}
