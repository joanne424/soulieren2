// <copyright file="QueryTermsKVP.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>muxf</author>
// <date> 2013-11-28 14:10:02 </date>
// <summary> Excel,Cev操作类 </summary>
// <modify>
//      修改人：muxf
//      修改时间：2013-11-28 14:09:56
//      修改描述：新建 CsvOperation.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace Infrastructure.Utils
{
    /// <summary>
    /// 报表导出查询条件实体
    /// </summary>
    public class QueryTermsKvp
    {
        /// <summary>
        /// 查询条件名称
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 查询条件内容
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="k">名称</param>
        /// <param name="v">内容</param>
        public QueryTermsKvp(string k, string v)
        {
            this.Key = k;
            this.Value = v;
        }
    }
}
