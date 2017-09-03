// <copyright file="PasswordBoxHelper.cs" company="Banclogix ">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>donggj</author>
// <date> 2013-10-8 </date>
// <summary>密码输入框自定义可用于密码文字绑定的依赖属性</summary>
// <modify>
//      修改人：donggj
//      修改时间：2013-10-9
//      修改描述：
//      版本：2.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>

namespace DM2.Ent.Client.Views.ExtendClass
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// 为PasswordBox控件的Password增加绑定功能
    /// </summary>
    public static class PasswordBoxHelper
    {
        #region 依赖属性
        /// <summary>
        /// 密码依赖属性
        /// </summary>
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached(
            "Password",
            typeof(string),
            typeof(PasswordBoxHelper),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        /// <summary>
        /// 附加依赖属性
        /// </summary>
        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached(
            "Attach",
            typeof(bool),
            typeof(PasswordBoxHelper),
            new PropertyMetadata(false, Attach));

        /// <summary>
        /// 是否更新依赖属性
        /// </summary>
        private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached(
            "IsUpdating",
            typeof(bool),
            typeof(PasswordBoxHelper));

        #endregion

        #region 公开静态方法
        /// <summary>
        /// Set附加方法
        /// </summary>
        /// <param name="dp">依赖对象</param>
        /// <param name="value">设置的值</param>
        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        /// <summary>
        /// Get附加方法
        /// </summary>
        /// <param name="dp">依赖对象</param>
        /// <returns>返回附加的值</returns>
        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        /// <summary>
        /// Set密码方法
        /// </summary>
        /// <param name="dp">依赖对象</param>
        /// <returns>返回密码</returns>
        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        /// <summary>
        /// Get密码方法
        /// </summary>
        /// <param name="dp">依赖对象</param>
        /// <param name="value">设置的值</param>
        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }
        #endregion

        #region 私有静态方法
        /// <summary>
        /// Get IsUpdating依赖属性的值
        /// </summary>
        /// <param name="dp">依赖对象</param>
        /// <returns>返回IsUpdating属性值</returns>
        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        /// <summary>
        /// Set IsUpdating依赖属性的值
        /// </summary>
        /// <param name="dp">依赖对象</param>
        /// <param name="value">需要设置的值</param>
        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        /// <summary>
        /// 密码依赖属性已改变事件
        /// </summary>
        /// <param name="sender">依赖对象</param>
        /// <param name="e">依赖属性改变事件参数</param>
        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;
            if (!(bool)GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }

            passwordBox.PasswordChanged += PasswordChanged;
        }

        /// <summary>
        /// 附加方法
        /// </summary>
        /// <param name="sender">依赖对象</param>
        /// <param name="e">依赖属性改变事件参数</param>
        private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox == null)
            {
                return;
            }

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        /// <summary>
        /// 密码已改变事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">调用参数</param>
        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
        #endregion
    }
}
