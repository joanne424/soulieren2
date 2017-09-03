// <copyright file="NonPerformanceStatisticAttribute.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2016/06/06 04:08:58 </date>
// <summary> 不进行性能统计特性 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2016/06/06 04:08:58
//      修改描述：新建 NonPerformanceStatisticAttribute.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >
namespace PerformanceStatisticCore
{
    #region

    using System;

    #endregion

    /// <summary>
    /// 不进行性能统计特性
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class NonPerformanceStatisticAttribute : System.Attribute
    {
    }
}