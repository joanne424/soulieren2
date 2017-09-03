// <copyright file="ExtendedDataGrid_Theme.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>董国君</author>
// <date> 2013/10/9 16:27:38 </date>
// <summary> 扩展DataGrid主题 </summary>
// <modify>
//      修改人：XXX
//      修改时间：XXXXXX
//      修改描述：XXXXX
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace BancLogix.ExtendedDataGrid
{
    /// <summary>
    /// 扩展DataGrid主题相关分布类
    /// </summary>
    public partial class ExtendedDataGrid
    {
        #region 字段

        /// <summary>
        /// 主题依赖属性
        /// </summary>
        public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register("Theme", typeof(Themes), typeof(ExtendedDataGrid), new PropertyMetadata(Themes.Default));
        
        /// <summary>
        /// 主题资源地址
        /// </summary>
        private const string GenericThemeUri = "/BancLogix.ExtendedDataGrid;component/Themes/{0}.xaml";

        /// <summary>
        /// 默认主题资源文件URI
        /// </summary>
        private static readonly Uri DefaultThemeURI = new Uri("/BancLogix.ExtendedDataGrid;component/Themes/Default.xaml", UriKind.Relative);

        /// <summary>
        /// 默认主题资源文件
        /// </summary>
        private readonly ResourceDictionary defaultTheme = new ResourceDictionary() { Source = DefaultThemeURI };

        /// <summary>
        /// 上一个主题资源文件
        /// </summary>
        private ResourceDictionary lastTheme;

        #endregion

        #region 枚举
        /// <summary>
        /// 主题枚举
        /// </summary>
        public enum Themes
        {
            /// <summary>
            /// 默认
            /// </summary>
            Default,

            /// <summary>
            /// 黑色
            /// </summary>
            Black
        }
        #endregion

        /// <summary>
        /// 属性 主题
        /// </summary>
        public Themes Theme
        {
            get
            {
                return (Themes)this.GetValue(ThemeProperty);
            }

            set
            {
                this.AddTheme(value);
                this.SetValue(ThemeProperty, value);
            }
        }

        /// <summary>
        /// 改变ExtendedDataGrid主题
        /// </summary>
        /// <param name="value">主题对象</param>
        private void AddTheme(Themes value)
        {
            if (!UriParser.IsKnownScheme("pack"))
            {
                UriParser.Register(new GenericUriParser(GenericUriParserOptions.GenericAuthority), "pack", -1);
            }

            // Remove last loaded themes
            if (Resources.MergedDictionaries.Contains(this.lastTheme))
            {
                Resources.MergedDictionaries.Remove(this.lastTheme);
            }

            // Add new Theme
            var newTheme = new ResourceDictionary { Source = new Uri(string.Format(GenericThemeUri, value), UriKind.Relative) };
            if (!Resources.MergedDictionaries.Contains(newTheme))
            {
                Resources.MergedDictionaries.Add(newTheme);
                this.lastTheme = newTheme;
            }
        }

        /// <summary>
        /// 初始加载相关资源
        /// </summary>
        private void AddResources()
        {
            if (!UriParser.IsKnownScheme("pack"))
            {
                UriParser.Register(new GenericUriParser(GenericUriParserOptions.GenericAuthority), "pack", -1);
            }

            // Default Theme
            if (!Resources.MergedDictionaries.Contains(this.defaultTheme))
            {
                Resources.MergedDictionaries.Add(this.defaultTheme);
            }

            this.lastTheme = this.defaultTheme;
        }
    }
}
