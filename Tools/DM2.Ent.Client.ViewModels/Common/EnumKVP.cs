//// <copyright file="EnumKVP.cs" company="BancLogix">
//// Copyright (c) Banclogix. All rights reserved.
//// </copyright>
//// <author> zhanggq </author>
//// <date> 2016/05/12 10:49:24 </date>
//// <modify>
////   修改人：zhanggq
////   修改时间：2016/05/12 10:49:24
////   修改描述：新建 EnumKVP
////   版本：1.0
//// </modify>
//// <review>
////   review人：
////   review时间：
//// </review >

namespace DM2.Ent.Client.ViewModels.Common
{
    /// <summary>
    /// 存储枚举的键值对对象
    /// </summary>
    public class EnumKVP
    {
        public int Key { get; set; }
        public string Value { get; set; }

        public EnumKVP(int k, string v)
        {
            this.Key = k;
            this.Value = v;
        }
    }
}
