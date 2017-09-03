//// <copyright file="InstrumentEnumKVP.cs" company="BancLogix">
//// Copyright (c) Banclogix. All rights reserved.
//// </copyright>
//// <author> zhanggq </author>
//// <date> 2016/05/23 09:44:24 </date>
//// <modify>
////   修改人：zhanggq
////   修改时间：2016/05/23 09:44:24
////   修改描述：新建 InstrumentEnumKVP
////   版本：1.0
//// </modify>
//// <review>
////   review人：
////   review时间：
//// </review >

namespace DM2.Ent.Client.ViewModels.Common
{
    /// <summary>
    /// Report 001 用Instrument实体
    /// </summary>
    public class InstrumentEnumKVP
    {
        public int Key { get; set; }
        public int Tpye { get; set; }
        public string Value { get; set; }

        public InstrumentEnumKVP(int k, int t, string v)
        {
            this.Key = k;
            this.Tpye = t;
            this.Value = v;
        }
    }
}
