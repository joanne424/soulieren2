// <copyright file="FocusExtension.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author> wangmy </author>
// <date> 2015/11/24 17:08:44 </date>
// <modify>
//   修改人：wangmy
//   修改时间：2015/11/24 17:08:44
//   修改描述：新建 FocusExtension
//   版本：1.0
// </modify>
// <review>
//   review人：
//   review时间：
// </review>

using System.Windows;

namespace Banclogix.Controls.WPF
{
    /// <summary>
    /// 焦点扩展类
    /// </summary>
    public static class FocusExtension
    {
        /// <summary>
        /// 是否焦点状态的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached("IsFocused", typeof(bool), typeof(FocusExtension), new PropertyMetadata(false, FocusedChangedCallback));

        /// <summary>
        /// 获取是否处于焦点
        /// </summary>
        /// <param name="obj">控件对象</param>
        /// <returns>焦点状态</returns>
        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }

        /// <summary>
        /// 设置是否处于焦点
        /// </summary>
        /// <param name="obj">控件对象</param>
        /// <param name="isFocused">焦点状态</param>
        public static void SetIsFocused(DependencyObject obj, bool isFocused)
        {
            obj.SetValue(IsFocusedProperty, isFocused);
        }

        /// <summary>
        /// 焦点状态变化事件回调函数
        /// </summary>
        /// <param name="dependencyObject">控件对象</param>
        /// <param name="dependencyPropertyChangedEventArgs">状态参数</param>
        private static void FocusedChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var element = dependencyObject as UIElement;
            if (element != null && (bool)dependencyPropertyChangedEventArgs.NewValue)
            {
                element.Focus();
            }
        }
    }
}
