// <copyright file="EnumBinder.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>tangl</author>
// <date>2013/10/31 7:49:33</date>
// <modify>
//   修改人：tangl
//   修改时间：2013/10/31 7:49:33
//   修改描述：新建 EnumBindingHelper.cs
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>

namespace DM2.Ent.Client.Views.ExtendClass
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;

    using DM2.Ent.Client.Views;

    using Infrastructure.Common;

    /// <summary>
    /// 枚举绑定帮助类
    /// </summary>
    public class EnumBinder
    {
        #region Path

        /// <summary>
        /// 注册路径依赖属性
        /// </summary>
        public static readonly DependencyProperty PathProperty = DependencyProperty.RegisterAttached(
            "Path", typeof(string), typeof(EnumBinder), new PropertyMetadata(OnPathPropertyValueChanged));

        /// <summary>
        /// 得到Path
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <returns>返回path</returns>
        public static string GetPath(DependencyObject obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置Path
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <param name="value">依赖属性value</param>
        public static void SetPath(DependencyObject obj, string value)
        {
            if (DesignerProperties.GetIsInDesignMode(obj))
            {
                return;
            }

            var comboBox = obj as ComboBox;
            if (comboBox == null)
            {
                // throw new Exception("无法对非 'ComboBox' 的派生对象进行 'Enum' 的绑定。");
                return;
            }

            if (string.IsNullOrEmpty(value))
            {
                // throw new Exception("参数 'Path' 无效。");
                return;
            }

            Assembly assembly = null;
            string typePath = null;
            var content = value.Split('/');
            if (content.Length == 1)
            {
                assembly = Assembly.GetExecutingAssembly();
                typePath = content[0];
            }
            else if (content.Length == 2)
            {
                try
                {
                    assembly = Assembly.LoadFrom(content[0].GetFullPath());
                }
                catch
                {
                    return;
                }

                typePath = content[1];
                if (assembly == null)
                {
                    // throw new Exception(string.Format("无法加载程序集 '{0}'。", content[0]));
                    return;
                }
            }
            else
            {
                // throw new Exception("参数 'Path' 应该为 'assembly.dll/namespace.enumname'");
                return;
            }

            var type = assembly.GetType(typePath);
            if (type == null)
            {
                // throw new Exception(string.Format("无法在程序集 '{0}' 中找到类型 '{1}' 的定义。", assembly.FullName, typePath));
                return;
            }

            if (!type.IsSubclassOf(typeof(Enum)))
            {
                // throw new Exception(string.Format("类型 '{0}' 不是一个有效的枚举类型。", typePath));
                return;
            }

            var names = Enum.GetNames(type);
            var list = new object[names.Length];
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = new
                {
                    Display = App.Current.TryFindResource(type.Name + "." + names[i]),
                    Value = Enum.Parse(type, names[i]),
                };
            }

            comboBox.ItemsSource = list;
            comboBox.DisplayMemberPath = "Display";
            comboBox.SelectedValuePath = "Value";
        }
        
        /// <summary>
        /// 属性变化事件
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <param name="e">属性变化事件对象</param>
        private static void OnPathPropertyValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            SetPath(obj, (string)e.NewValue);
        }

        #endregion

        #region Path with All

        /// <summary>
        /// 注册路径依赖属性
        /// </summary>
        public static readonly DependencyProperty PathWithAllProperty = DependencyProperty.RegisterAttached(
            "PathWithAll", typeof(string), typeof(EnumBinder), new PropertyMetadata(OnPathWithAllPropertyValueChanged));

        /// <summary>
        /// 得到PathWithAll
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <returns>返回PathWithAll</returns>
        public static string GetPathWithAll(DependencyObject obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置PathWithAll
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <param name="value">依赖属性value</param>
        public static void SetPathWithAll(DependencyObject obj, string value)
        {
            if (DesignerProperties.GetIsInDesignMode(obj))
            {
                return;
            }

            var comboBox = obj as ComboBox;
            if (comboBox == null)
            {
                // throw new Exception("无法对非 'ComboBox' 的派生对象进行 'Enum' 的绑定。");
                return;
            }

            if (string.IsNullOrEmpty(value))
            {
                // throw new Exception("参数 'Path' 无效。");
                return;
            }

            Assembly assembly = null;
            string typePath = null;
            var content = value.Split('/');
            if (content.Length == 1)
            {
                assembly = Assembly.GetExecutingAssembly();
                typePath = content[0];
            }
            else if (content.Length == 2)
            {
                try
                {
                    assembly = Assembly.LoadFrom(content[0].GetFullPath());
                }
                catch
                {
                    return;
                }

                typePath = content[1];
                if (assembly == null)
                {
                    // throw new Exception(string.Format("无法加载程序集 '{0}'。", content[0]));
                    return;
                }
            }
            else
            {
                // throw new Exception("参数 'Path' 应该为 'assembly.dll/namespace.enumname'");
                return;
            }

            var type = assembly.GetType(typePath);
            if (type == null)
            {
                // throw new Exception(string.Format("无法在程序集 '{0}' 中找到类型 '{1}' 的定义。", assembly.FullName, typePath));
                return;
            }

            if (!type.IsSubclassOf(typeof(Enum)))
            {
                // throw new Exception(string.Format("类型 '{0}' 不是一个有效的枚举类型。", typePath));
                return;
            }

            var names = Enum.GetNames(type);
            //var list = new object[names.Length];
            var list = new object[names.Length + 1];
            list[0] = new { Display = "", Value = -1 };
            for (int i = 1; i < list.Length; i++)
            {
                list[i] = new
                {
                    Display = App.Current.TryFindResource(type.Name + "." + names[i - 1]),
                    Value = Enum.Parse(type, names[i - 1]),
                };
            }

            comboBox.ItemsSource = list;
            comboBox.DisplayMemberPath = "Display";
            comboBox.SelectedValuePath = "Value";
        }

        /// <summary>
        /// 属性变化事件
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <param name="e">属性变化事件对象</param>
        private static void OnPathWithAllPropertyValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            SetPathWithAll(obj, (string)e.NewValue);
        }

        #endregion
    }
}