// <copyright file="NonExceptionCtrlAttribute.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2016/06/22 10:27:00 </date>
// <summary> 不进行异常控制特性 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2016/06/22 10:27:00
//      修改描述：新建 ExceptionCtrlAttribute.cs
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
    /// 不进行异常控制的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class NonExceptionCtrlAttribute : System.Attribute
    {
    }
}