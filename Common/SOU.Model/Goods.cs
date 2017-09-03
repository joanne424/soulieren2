using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOU.Model
{
    public class Goods
    {
        #region
        /// <summary>
        /// auto_increment
        /// </summary>
        public int gid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string shop_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string brand { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int zid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gdes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string small_img { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imgs_details { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int keys { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sample { get; set; }
        /// <summary>
        /// on update CURRENT_TIMESTAMP
        /// </summary>
        public DateTime order_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double? sales { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string video { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? recommend { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime create_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int shop_create_id { get; set; }

        #endregion Model
    }
}
