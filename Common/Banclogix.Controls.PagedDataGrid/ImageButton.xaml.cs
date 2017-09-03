// <copyright file="ImageButton.xaml.cs" company="Banclogix ">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>donggj</author>
// <date> 2013-12-26 </date>
// <summary>图标按钮</summary>
// <modify>
//      修改人：donggj
//      修改时间：2013-12-26
//      修改描述：
//      版本：2.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Banclogix.Controls.PagedDataGrid
{
    /// <summary>
    /// ImageButton.xaml 的交互逻辑
    /// </summary>
    public partial class ImageButton : Button
    {
        #region 字段
        /// <summary>
        /// Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));

        /// <summary>
        /// Using a DependencyProperty as the backing store for EntryImageSource.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty EntryImageSourceProperty =
            DependencyProperty.Register("EntryImageSource", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));

        /// <summary>
        /// Using a DependencyProperty as the backing store for GrayImageSource.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty GrayImageSourceProperty =
            DependencyProperty.Register("GrayImageSource", typeof(ImageSource), typeof(ImageButton), new UIPropertyMetadata(null));

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageButton"/> class.
        /// </summary>
        public ImageButton()
        {
            this.InitializeComponent();

            this.Style = this.FindResource("ImageButtonStyle") as Style;
            this.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.ImageButton_IsEnabledChanged);
            this.MouseEnter += this.ImageButton_MouseEnter;
            this.MouseLeave += this.ImageButton_MouseLeave;
        }

        #region 属性

        /// <summary>
        /// 按钮图片路径
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                return (ImageSource)this.GetValue(ImageSourceProperty);
            }

            set
            {
                this.SetValue(ImageSourceProperty, value);
            }
        }

        /// <summary>
        /// 鼠标进入图片路径
        /// </summary>
        public ImageSource EntryImageSource
        {
            get
            {
                return (ImageSource)this.GetValue(EntryImageSourceProperty);
            }

            set
            {
                this.SetValue(EntryImageSourceProperty, value);
            }
        }

        /// <summary>
        /// 灰色图片路径
        /// </summary>
        public ImageSource GrayImageSource
        {
            get
            {
                return (ImageSource)this.GetValue(GrayImageSourceProperty);
            }

            set
            {
                this.SetValue(GrayImageSourceProperty, value);
            }
        }
        #endregion

        #region 公开方法

        /// <summary>
        /// 加载Template函数处理
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.IsEnabled && this.ImageSource != null)
            {
                this.innerImage.Source = this.ImageSource;
            }
            else if (!this.IsEnabled && this.GrayImageSource != null)
            {
                this.innerImage.Source = this.GrayImageSource;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 按钮IsEnabled属性改变事件
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void ImageButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsEnabled && this.ImageSource != null)
            {
                this.innerImage.Source = this.ImageSource;
            }
            else if (!this.IsEnabled && this.GrayImageSource != null)
            {
                this.innerImage.Source = this.GrayImageSource;
            }
        }

        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void ImageButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.IsEnabled && this.ImageSource != null)
            {
                this.innerImage.Source = this.ImageSource;
            }
            else if (!this.IsEnabled && this.GrayImageSource != null)
            {
                this.innerImage.Source = this.GrayImageSource;
            }
        }

        /// <summary>
        /// 鼠标进入事件
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        private void ImageButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.EntryImageSource != null)
            {
                this.innerImage.Source = this.EntryImageSource;
            }
        }
        #endregion
    }
}
