// <copyright file="ImageButtonChrome.cs" company="Banclogix ">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>donggj</author>
// <date> 2013-12-26 </date>
// <summary>ImageButtonChrome类</summary>
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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Runtime;
using System.Windows.Media.Animation;

namespace Banclogix.Controls.PagedDataGrid
{    
    /// <summary>
    /// ImageButtonChrome类
    /// </summary>
    public sealed class ImageButtonChrome : Decorator
    {
        #region 字段
        /// <summary>
        /// 背景
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty;

        /// <summary>
        /// 边框
        /// </summary>
        public static readonly DependencyProperty BorderBrushProperty;

        /// <summary>
        /// 是否有默认效果
        /// </summary>
        public static readonly DependencyProperty RenderDefaultedProperty;

        /// <summary>
        /// 是否鼠标移到上面有效果
        /// </summary>
        public static readonly DependencyProperty RenderMouseOverProperty;

        /// <summary>
        /// 是否有按下效果
        /// </summary>
        public static readonly DependencyProperty RenderPressedProperty;

        /// <summary>
        /// 是否有圆角效果
        /// </summary>
        public static readonly DependencyProperty RoundCornersProperty;

        /// <summary>
        /// 公用边框画笔
        /// </summary>
        private static Pen commonBorderPen;

        /// <summary>
        /// 公用内边框画笔
        /// </summary>
        private static Pen commonInnerBorderPen;

        /// <summary>
        /// 公用按钮不可用边框覆盖画笔
        /// </summary>
        private static Pen commonDisabledBorderOverlay;

        /// <summary>
        /// 公用按钮不可用背景颜色画刷
        /// </summary>
        private static SolidColorBrush commonDisabledBackgroundOverlay;

        /// <summary>
        /// 公用默认内边框画笔
        /// </summary>
        private static Pen commonDefaultedInnerBorderPen;

        /// <summary>
        /// 公用鼠标移到上面后的背景颜色画刷
        /// </summary>
        private static LinearGradientBrush commonHoverBackgroundOverlay;

        /// <summary>
        /// 公用鼠标移到上面的边框画笔
        /// </summary>
        private static Pen commonHoverBorderOverlay;

        /// <summary>
        /// 公用按下的背景颜色画刷
        /// </summary>
        private static LinearGradientBrush commonPressedBackgroundOverlay;

        /// <summary>
        /// 公用按下边框的画笔
        /// </summary>
        private static Pen commonPressedBorderOverlay;

        /// <summary>
        /// 公用按下时左侧阴影颜色画刷
        /// </summary>
        private static LinearGradientBrush commonPressedLeftDropShadowBrush;

        /// <summary>
        /// 公用按下时顶部阴影颜色画刷
        /// </summary>
        private static LinearGradientBrush commonPressedTopDropShadowBrush;

        /// <summary>
        /// 资源访问
        /// </summary>
        private static object resourceAccess;

        /// <summary>
        /// 本地资源
        /// </summary>
        private ImageButtonChrome.LocalResources localResources;
        #endregion

        /// <summary>
        /// Initializes static members of the <see cref="ImageButtonChrome" /> class.
        /// </summary>
        static ImageButtonChrome()
        {
            ImageButtonChrome.BackgroundProperty = Control.BackgroundProperty.AddOwner(typeof(ImageButtonChrome), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
            ImageButtonChrome.BorderBrushProperty = Border.BorderBrushProperty.AddOwner(typeof(ImageButtonChrome), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
            ImageButtonChrome.RenderDefaultedProperty = DependencyProperty.Register("RenderDefaulted", typeof(bool), typeof(ImageButtonChrome), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(ImageButtonChrome.OnRenderDefaultedChanged)));
            ImageButtonChrome.RenderMouseOverProperty = DependencyProperty.Register("RenderMouseOver", typeof(bool), typeof(ImageButtonChrome), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(ImageButtonChrome.OnRenderMouseOverChanged)));
            ImageButtonChrome.RenderPressedProperty = DependencyProperty.Register("RenderPressed", typeof(bool), typeof(ImageButtonChrome), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(ImageButtonChrome.OnRenderPressedChanged)));
            ImageButtonChrome.RoundCornersProperty = DependencyProperty.Register("RoundCorners", typeof(bool), typeof(ImageButtonChrome), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));
            ImageButtonChrome.resourceAccess = new object();
            UIElement.IsEnabledProperty.OverrideMetadata(typeof(ImageButtonChrome), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageButtonChrome"/> class.
        /// </summary>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public ImageButtonChrome()
        {
        }

        #region 属性

        /// <summary>
        /// 背景颜色画刷
        /// </summary>
        public Brush Background
        {
            get
            {
                return (Brush)this.GetValue(ImageButtonChrome.BackgroundProperty);
            }

            set
            {
                this.SetValue(ImageButtonChrome.BackgroundProperty, value);
            }
        }

        /// <summary>
        /// 边框颜色画刷
        /// </summary>
        public Brush BorderBrush
        {
            get
            {
                return (Brush)this.GetValue(ImageButtonChrome.BorderBrushProperty);
            }

            set
            {
                this.SetValue(ImageButtonChrome.BorderBrushProperty, value);
            }
        }

        /// <summary>
        /// 是否有按钮默认效果
        /// </summary>
        public bool RenderDefaulted
        {
            get
            {
                return (bool)this.GetValue(ImageButtonChrome.RenderDefaultedProperty);
            }

            set
            {
                this.SetValue(ImageButtonChrome.RenderDefaultedProperty, value);
            }
        }

        /// <summary>
        /// 是否有鼠标移到按钮上的效果
        /// </summary>
        public bool RenderMouseOver
        {
            get
            {
                return (bool)this.GetValue(ImageButtonChrome.RenderMouseOverProperty);
            }

            set
            {
                this.SetValue(ImageButtonChrome.RenderMouseOverProperty, value);
            }
        }

        /// <summary>
        /// 是否有按钮按下效果
        /// </summary>
        public bool RenderPressed
        {
            get
            {
                return (bool)this.GetValue(ImageButtonChrome.RenderPressedProperty);
            }

            set
            {
                this.SetValue(ImageButtonChrome.RenderPressedProperty, value);
            }
        }

        /// <summary>
        /// 是否有圆角
        /// </summary>
        public bool RoundCorners
        {
            get
            {
                return (bool)this.GetValue(ImageButtonChrome.RoundCornersProperty);
            }

            set
            {
                this.SetValue(ImageButtonChrome.RoundCornersProperty, value);
            }
        }

        /// <summary>
        /// 效果初始大小
        /// </summary>
        internal int EffectiveValuesInitialSize
        {
            get
            {
                return 9;
            }
        }

        /// <summary>
        /// 公用鼠标移到边框效果的覆盖画笔
        /// </summary>
        private static Pen CommonHoverBorderOverlay
        {
            get
            {
                if (ImageButtonChrome.commonHoverBorderOverlay == null)
                {
                    lock (ImageButtonChrome.resourceAccess)
                    {
                        if (ImageButtonChrome.commonHoverBorderOverlay == null)
                        {
                            Pen pen = new Pen();
                            pen.Thickness = 1.0;
                            pen.Brush = new SolidColorBrush(Color.FromRgb(60, 127, 177));
                            pen.Freeze();
                            ImageButtonChrome.commonHoverBorderOverlay = pen;
                        }
                    }
                }

                return ImageButtonChrome.commonHoverBorderOverlay;
            }
        }

        /// <summary>
        /// 公用按钮按下边框的覆盖画笔
        /// </summary>
        private static Pen CommonPressedBorderOverlay
        {
            get
            {
                if (ImageButtonChrome.commonPressedBorderOverlay == null)
                {
                    lock (ImageButtonChrome.resourceAccess)
                    {
                        if (ImageButtonChrome.commonPressedBorderOverlay == null)
                        {
                            Pen pen = new Pen();
                            pen.Thickness = 1.0;
                            pen.Brush = new SolidColorBrush(Color.FromRgb(44, 98, 139));
                            pen.Freeze();
                            ImageButtonChrome.commonPressedBorderOverlay = pen;
                        }
                    }
                }

                return ImageButtonChrome.commonPressedBorderOverlay;
            }
        }

        /// <summary>
        /// 公用按钮不可用的边框覆盖画笔
        /// </summary>
        private static Pen CommonDisabledBorderOverlay
        {
            get
            {
                if (ImageButtonChrome.commonDisabledBorderOverlay == null)
                {
                    lock (ImageButtonChrome.resourceAccess)
                    {
                        if (ImageButtonChrome.commonDisabledBorderOverlay == null)
                        {
                            Pen pen = new Pen();
                            pen.Thickness = 1.0;
                            pen.Brush = new SolidColorBrush(Color.FromRgb(173, 178, 181));
                            pen.Freeze();
                            ImageButtonChrome.commonDisabledBorderOverlay = pen;
                        }
                    }
                }

                return ImageButtonChrome.commonDisabledBorderOverlay;
            }
        }

        /// <summary>
        /// 公共鼠标移到按钮上的背景覆盖颜色画刷
        /// </summary>
        private static LinearGradientBrush CommonHoverBackgroundOverlay
        {
            get
            {
                if (ImageButtonChrome.commonHoverBackgroundOverlay == null)
                {
                    lock (ImageButtonChrome.resourceAccess)
                    {
                        if (ImageButtonChrome.commonHoverBackgroundOverlay == null)
                        {
                            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
                            linearGradientBrush.StartPoint = new Point(0.0, 0.0);
                            linearGradientBrush.EndPoint = new Point(0.0, 1.0);
                            linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 234, 246, 253), 0.0));
                            linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 217, 240, 252), 0.5));
                            linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 190, 230, 253), 0.5));
                            linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 167, 217, 245), 1.0));
                            linearGradientBrush.Freeze();
                            ImageButtonChrome.commonHoverBackgroundOverlay = linearGradientBrush;
                        }
                    }
                }

                return ImageButtonChrome.commonHoverBackgroundOverlay;
            }
        }

        /// <summary>
        /// 公用按钮按下效果的背景覆盖颜色画刷
        /// </summary>
        private static LinearGradientBrush CommonPressedBackgroundOverlay
        {
            get
            {
                if (ImageButtonChrome.commonPressedBackgroundOverlay == null)
                {
                    lock (ImageButtonChrome.resourceAccess)
                    {
                        if (ImageButtonChrome.commonPressedBackgroundOverlay == null)
                        {
                            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
                            linearGradientBrush.StartPoint = new Point(0.0, 0.0);
                            linearGradientBrush.EndPoint = new Point(0.0, 1.0);
                            linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 194, 228, 246), 0.5));
                            linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 171, 218, 243), 0.5));
                            linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(255, 144, 203, 235), 1.0));
                            linearGradientBrush.Freeze();
                            ImageButtonChrome.commonPressedBackgroundOverlay = linearGradientBrush;
                        }
                    }
                }

                return ImageButtonChrome.commonPressedBackgroundOverlay;
            }
        }

        /// <summary>
        /// 公用按钮不可用状态的背景颜色覆盖的颜色画刷
        /// </summary>
        private static SolidColorBrush CommonDisabledBackgroundOverlay
        {
            get
            {
                if (ImageButtonChrome.commonDisabledBackgroundOverlay == null)
                {
                    lock (ImageButtonChrome.resourceAccess)
                    {
                        if (ImageButtonChrome.commonDisabledBackgroundOverlay == null)
                        {
                            SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromRgb(244, 244, 244));
                            solidColorBrush.Freeze();
                            ImageButtonChrome.commonDisabledBackgroundOverlay = solidColorBrush;
                        }
                    }
                }

                return ImageButtonChrome.commonDisabledBackgroundOverlay;
            }
        }

        /// <summary>
        /// 公用内边框画笔
        /// </summary>
        private static Pen CommonInnerBorderPen
        {
            get
            {
                if (ImageButtonChrome.commonInnerBorderPen == null)
                {
                    lock (ImageButtonChrome.resourceAccess)
                    {
                        if (ImageButtonChrome.commonInnerBorderPen == null)
                        {
                            Pen pen = new Pen();
                            pen.Thickness = 1.0;
                            pen.Brush = new LinearGradientBrush
                            {
                                StartPoint = new Point(0.0, 0.0),
                                EndPoint = new Point(0.0, 1.0),
                                GradientStops = 
                                {
                                    new GradientStop(Color.FromArgb(250, 255, 255, 255), 0.0),
                                    new GradientStop(Color.FromArgb(133, 255, 255, 255), 1.0)
                                }
                            };
                            pen.Freeze();
                            ImageButtonChrome.commonInnerBorderPen = pen;
                        }
                    }
                }

                return ImageButtonChrome.commonInnerBorderPen;
            }
        }

        /// <summary>
        /// 公用默认效果内部边框画笔
        /// </summary>
        private static Pen CommonDefaultedInnerBorderPen
        {
            get
            {
                if (ImageButtonChrome.commonDefaultedInnerBorderPen == null)
                {
                    lock (ImageButtonChrome.resourceAccess)
                    {
                        if (ImageButtonChrome.commonDefaultedInnerBorderPen == null)
                        {
                            Pen pen = new Pen();
                            pen.Thickness = 1.0;
                            pen.Brush = new SolidColorBrush(Color.FromArgb(249, 0, 204, 255));
                            pen.Freeze();
                            ImageButtonChrome.commonDefaultedInnerBorderPen = pen;
                        }
                    }
                }

                return ImageButtonChrome.commonDefaultedInnerBorderPen;
            }
        }

        /// <summary>
        /// 按下状态左侧阴影颜色画刷
        /// </summary>
        private static LinearGradientBrush CommonPressedLeftDropShadowBrush
        {
            get
            {
                if (ImageButtonChrome.commonPressedLeftDropShadowBrush == null)
                {
                    lock (ImageButtonChrome.resourceAccess)
                    {
                        if (ImageButtonChrome.commonPressedLeftDropShadowBrush == null)
                        {
                            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
                            linearGradientBrush.StartPoint = new Point(0.0, 0.0);
                            linearGradientBrush.EndPoint = new Point(1.0, 0.0);
                            linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(128, 51, 51, 51), 0.0));
                            linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 51, 51, 51), 1.0));
                            linearGradientBrush.Freeze();
                            ImageButtonChrome.commonPressedLeftDropShadowBrush = linearGradientBrush;
                        }
                    }
                }

                return ImageButtonChrome.commonPressedLeftDropShadowBrush;
            }
        }

        /// <summary>
        /// 按下状态顶部阴影颜色画刷
        /// </summary>
        private static LinearGradientBrush CommonPressedTopDropShadowBrush
        {
            get
            {
                if (ImageButtonChrome.commonPressedTopDropShadowBrush == null)
                {
                    lock (ImageButtonChrome.resourceAccess)
                    {
                        if (ImageButtonChrome.commonPressedTopDropShadowBrush == null)
                        {
                            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
                            linearGradientBrush.StartPoint = new Point(0.0, 0.0);
                            linearGradientBrush.EndPoint = new Point(0.0, 1.0);
                            linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(128, 51, 51, 51), 0.0));
                            linearGradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 51, 51, 51), 1.0));
                            linearGradientBrush.Freeze();
                            ImageButtonChrome.commonPressedTopDropShadowBrush = linearGradientBrush;
                        }
                    }
                }

                return ImageButtonChrome.commonPressedTopDropShadowBrush;
            }
        }

        /// <summary>
        /// 是否有动画效果
        /// </summary>
        private bool Animates
        {
            get
            {
                return SystemParameters.PowerLineStatus == PowerLineStatus.Online && SystemParameters.ClientAreaAnimation && RenderCapability.Tier > 0 && this.IsEnabled;
            }
        }

        /// <summary>
        /// 背景覆盖颜色画刷
        /// </summary>
        private Brush BackgroundOverlay
        {
            get
            {
                if (!this.IsEnabled)
                {
                    return Brushes.Transparent;
                    ////return ImageButtonChrome.CommonDisabledBackgroundOverlay;
                }

                if (!this.Animates)
                {
                    if (this.RenderPressed)
                    {
                        return ImageButtonChrome.CommonPressedBackgroundOverlay;
                    }

                    if (this.RenderMouseOver)
                    {
                        return ImageButtonChrome.CommonHoverBackgroundOverlay;
                    }

                    return null;
                }
                else
                {
                    if (this.localResources != null)
                    {
                        if (this.localResources.BackgroundOverlay == null)
                        {
                            this.localResources.BackgroundOverlay = ImageButtonChrome.CommonHoverBackgroundOverlay.Clone();
                            this.localResources.BackgroundOverlay.Opacity = 0.0;
                        }

                        return this.localResources.BackgroundOverlay;
                    }

                    return null;
                }
            }
        }

        /// <summary>
        /// 边框覆盖画笔
        /// </summary>
        private Pen BorderOverlayPen
        {
            get
            {
                if (!this.IsEnabled)
                {
                    if (this.RoundCorners)
                    {
                        return ImageButtonChrome.CommonDisabledBorderOverlay;
                    }

                    return null;
                }
                else
                {
                    if (!this.Animates)
                    {
                        if (this.RenderPressed)
                        {
                            return ImageButtonChrome.CommonPressedBorderOverlay;
                        }

                        if (this.RenderMouseOver)
                        {
                            return ImageButtonChrome.CommonHoverBorderOverlay;
                        }

                        return null;
                    }
                    else
                    {
                        if (this.localResources != null)
                        {
                            if (this.localResources.BorderOverlayPen == null)
                            {
                                this.localResources.BorderOverlayPen = ImageButtonChrome.CommonHoverBorderOverlay.Clone();
                                this.localResources.BorderOverlayPen.Brush.Opacity = 0.0;
                            }

                            return this.localResources.BorderOverlayPen;
                        }

                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// 内部边框画笔
        /// </summary>
        private Pen InnerBorderPen
        {
            get
            {
                if (!this.IsEnabled)
                {
                    return ImageButtonChrome.CommonInnerBorderPen;
                }

                if (!this.Animates)
                {
                    if (this.RenderPressed)
                    {
                        return null;
                    }

                    if (this.RenderDefaulted)
                    {
                        return ImageButtonChrome.CommonDefaultedInnerBorderPen;
                    }

                    return ImageButtonChrome.CommonInnerBorderPen;
                }
                else
                {
                    if (this.localResources != null)
                    {
                        if (this.localResources.InnerBorderPen == null)
                        {
                            this.localResources.InnerBorderPen = ImageButtonChrome.CommonInnerBorderPen.Clone();
                        }

                        return this.localResources.InnerBorderPen;
                    }

                    return ImageButtonChrome.CommonInnerBorderPen;
                }
            }
        }

        /// <summary>
        /// 左侧阴影颜色画刷
        /// </summary>
        private LinearGradientBrush LeftDropShadowBrush
        {
            get
            {
                if (!this.IsEnabled)
                {
                    return null;
                }

                if (!this.Animates)
                {
                    if (this.RenderPressed)
                    {
                        return ImageButtonChrome.CommonPressedLeftDropShadowBrush;
                    }

                    return null;
                }
                else
                {
                    if (this.localResources != null)
                    {
                        if (this.localResources.LeftDropShadowBrush == null)
                        {
                            this.localResources.LeftDropShadowBrush = ImageButtonChrome.CommonPressedLeftDropShadowBrush.Clone();
                            this.localResources.LeftDropShadowBrush.Opacity = 0.0;
                        }

                        return this.localResources.LeftDropShadowBrush;
                    }

                    return null;
                }
            }
        }

        /// <summary>
        /// 顶部阴影颜色画刷
        /// </summary>
        private LinearGradientBrush TopDropShadowBrush
        {
            get
            {
                if (!this.IsEnabled)
                {
                    return null;
                }

                if (!this.Animates)
                {
                    if (this.RenderPressed)
                    {
                        return ImageButtonChrome.CommonPressedTopDropShadowBrush;
                    }

                    return null;
                }
                else
                {
                    if (this.localResources != null)
                    {
                        if (this.localResources.TopDropShadowBrush == null)
                        {
                            this.localResources.TopDropShadowBrush = ImageButtonChrome.CommonPressedTopDropShadowBrush.Clone();
                            this.localResources.TopDropShadowBrush.Opacity = 0.0;
                        }

                        return this.localResources.TopDropShadowBrush;
                    }

                    return null;
                }
            }
        }
        #endregion

        /// <summary>
        /// 重载的大小调整的处理函数
        /// </summary>
        /// <param name="availableSize">大小</param>
        /// <returns>调整后的大小</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            UIElement child = this.Child;
            Size desiredSize;
            if (child != null)
            {
                Size availableSize2 = default(Size);
                bool flag = availableSize.Width < 4.0;
                bool flag2 = availableSize.Height < 4.0;
                if (!flag)
                {
                    availableSize2.Width = availableSize.Width - 4.0;
                }

                if (!flag2)
                {
                    availableSize2.Height = availableSize.Height - 4.0;
                }

                child.Measure(availableSize2);
                desiredSize = child.DesiredSize;

                if (!flag)
                {
                    desiredSize.Width += 4.0;
                }

                if (!flag2)
                {
                    desiredSize.Height += 4.0;
                }
            }
            else
            {
                desiredSize = new Size(Math.Min(4.0, availableSize.Width), Math.Min(4.0, availableSize.Height));
            }

            return desiredSize;
        }

        /// <summary>
        /// 重载控件布局改变的处理函数
        /// </summary>
        /// <param name="finalSize">改变后的大小</param>
        /// <returns>调整后的大小</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect finalRect = default(Rect);
            finalRect.Width = Math.Max(0.0, finalSize.Width - 4.0);
            finalRect.Height = Math.Max(0.0, finalSize.Height - 4.0);
            finalRect.X = (finalSize.Width - finalRect.Width) * 0.5;
            finalRect.Y = (finalSize.Height - finalRect.Height) * 0.5;
            UIElement child = this.Child;
            if (child != null)
            {
                child.Arrange(finalRect);
            }

            return finalSize;
        }

        /// <summary>
        /// 重载的控件渲染函数
        /// </summary>
        /// <param name="drawingContext">绘图的上下文</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect rect = new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight);
            this.DrawBackground(drawingContext, ref rect);
            this.DrawDropShadows(drawingContext, ref rect);
            if (this.IsEnabled)
            {
                this.DrawBorder(drawingContext, ref rect);
            }

            this.DrawInnerBorder(drawingContext, ref rect);
        }

        #region 私有方法

        /// <summary>
        /// 按钮的默认效果改变的事件处理
        /// </summary>
        /// <param name="o">依赖对象</param>
        /// <param name="e">事件参数</param>
        private static void OnRenderDefaultedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ImageButtonChrome buttonChrome = (ImageButtonChrome)o;
            if (buttonChrome.Animates)
            {
                if (!buttonChrome.RenderPressed)
                {
                    if ((bool)e.NewValue)
                    {
                        if (buttonChrome.localResources == null)
                        {
                            buttonChrome.localResources = new ImageButtonChrome.LocalResources();
                            buttonChrome.InvalidateVisual();
                        }

                        Duration duration = new Duration(TimeSpan.FromSeconds(0.3));
                        ColorAnimation animation = new ColorAnimation(Color.FromArgb(249, 0, 204, 255), duration);
                        GradientStopCollection gradientStops = ((LinearGradientBrush)buttonChrome.InnerBorderPen.Brush).GradientStops;
                        gradientStops[0].BeginAnimation(GradientStop.ColorProperty, animation);
                        gradientStops[1].BeginAnimation(GradientStop.ColorProperty, animation);
                        DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
                        doubleAnimationUsingKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame(1.0, TimeSpan.FromSeconds(0.5)));
                        doubleAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame(1.0, TimeSpan.FromSeconds(0.75)));
                        doubleAnimationUsingKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame(0.0, TimeSpan.FromSeconds(2.0)));
                        doubleAnimationUsingKeyFrames.RepeatBehavior = RepeatBehavior.Forever;
                        Timeline.SetDesiredFrameRate(doubleAnimationUsingKeyFrames, new int?(10));
                        buttonChrome.BackgroundOverlay.BeginAnimation(Brush.OpacityProperty, doubleAnimationUsingKeyFrames);
                        buttonChrome.BorderOverlayPen.Brush.BeginAnimation(Brush.OpacityProperty, doubleAnimationUsingKeyFrames);
                        return;
                    }

                    if (buttonChrome.localResources == null)
                    {
                        buttonChrome.InvalidateVisual();
                        return;
                    }

                    Duration duration2 = new Duration(TimeSpan.FromSeconds(0.2));
                    DoubleAnimation doubleAnimation = new DoubleAnimation();
                    doubleAnimation.Duration = duration2;
                    buttonChrome.BorderOverlayPen.Brush.BeginAnimation(Brush.OpacityProperty, doubleAnimation);
                    buttonChrome.BackgroundOverlay.BeginAnimation(Brush.OpacityProperty, doubleAnimation);
                    ColorAnimation colorAnimation = new ColorAnimation();
                    colorAnimation.Duration = duration2;
                    GradientStopCollection gradientStops2 = ((LinearGradientBrush)buttonChrome.InnerBorderPen.Brush).GradientStops;
                    gradientStops2[0].BeginAnimation(GradientStop.ColorProperty, colorAnimation);
                    gradientStops2[1].BeginAnimation(GradientStop.ColorProperty, colorAnimation);
                    return;
                }
            }
            else
            {
                buttonChrome.localResources = null;
                buttonChrome.InvalidateVisual();
            }
        }

        /// <summary>
        /// 鼠标移到按钮上的事件处理
        /// </summary>
        /// <param name="o">依赖对象</param>
        /// <param name="e">事件参数</param>
        private static void OnRenderMouseOverChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ImageButtonChrome buttonChrome = (ImageButtonChrome)o;
            if (buttonChrome.Animates)
            {
                if (!buttonChrome.RenderPressed)
                {
                    if ((bool)e.NewValue)
                    {
                        if (buttonChrome.localResources == null)
                        {
                            buttonChrome.localResources = new ImageButtonChrome.LocalResources();
                            buttonChrome.InvalidateVisual();
                        }

                        Duration duration = new Duration(TimeSpan.FromSeconds(0.3));
                        DoubleAnimation animation = new DoubleAnimation(1.0, duration);
                        buttonChrome.BorderOverlayPen.Brush.BeginAnimation(Brush.OpacityProperty, animation);
                        buttonChrome.BackgroundOverlay.BeginAnimation(Brush.OpacityProperty, animation);
                        return;
                    }

                    if (buttonChrome.localResources == null)
                    {
                        buttonChrome.InvalidateVisual();
                        return;
                    }

                    if (buttonChrome.RenderDefaulted)
                    {
                        double opacity = buttonChrome.BackgroundOverlay.Opacity;
                        double num = (1.0 - opacity) * 0.5;
                        DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
                        doubleAnimationUsingKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame(1.0, TimeSpan.FromSeconds(num)));
                        doubleAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame(1.0, TimeSpan.FromSeconds(num + 0.25)));
                        doubleAnimationUsingKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame(0.0, TimeSpan.FromSeconds(num + 1.5)));
                        doubleAnimationUsingKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame(opacity, TimeSpan.FromSeconds(2.0)));
                        doubleAnimationUsingKeyFrames.RepeatBehavior = RepeatBehavior.Forever;
                        Timeline.SetDesiredFrameRate(doubleAnimationUsingKeyFrames, new int?(10));
                        buttonChrome.BackgroundOverlay.BeginAnimation(Brush.OpacityProperty, doubleAnimationUsingKeyFrames);
                        buttonChrome.BorderOverlayPen.Brush.BeginAnimation(Brush.OpacityProperty, doubleAnimationUsingKeyFrames);
                        return;
                    }

                    Duration duration2 = new Duration(TimeSpan.FromSeconds(0.2));
                    DoubleAnimation doubleAnimation = new DoubleAnimation();
                    doubleAnimation.Duration = duration2;
                    buttonChrome.BackgroundOverlay.BeginAnimation(Brush.OpacityProperty, doubleAnimation);
                    buttonChrome.BorderOverlayPen.Brush.BeginAnimation(Brush.OpacityProperty, doubleAnimation);
                    return;
                }
            }
            else
            {
                buttonChrome.localResources = null;
                buttonChrome.InvalidateVisual();
            }
        }

        /// <summary>
        /// 按钮按下的事件处理
        /// </summary>
        /// <param name="o">依赖对象</param>
        /// <param name="e">事件参数</param>
        private static void OnRenderPressedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ImageButtonChrome buttonChrome = (ImageButtonChrome)o;
            if (!buttonChrome.Animates)
            {
                buttonChrome.localResources = null;
                buttonChrome.InvalidateVisual();
                return;
            }

            if ((bool)e.NewValue)
            {
                if (buttonChrome.localResources == null)
                {
                    buttonChrome.localResources = new ImageButtonChrome.LocalResources();
                    buttonChrome.InvalidateVisual();
                }

                Duration duration = new Duration(TimeSpan.FromSeconds(0.1));
                DoubleAnimation animation = new DoubleAnimation(1.0, duration);
                buttonChrome.BackgroundOverlay.BeginAnimation(Brush.OpacityProperty, animation);
                buttonChrome.BorderOverlayPen.Brush.BeginAnimation(Brush.OpacityProperty, animation);
                buttonChrome.LeftDropShadowBrush.BeginAnimation(Brush.OpacityProperty, animation);
                buttonChrome.TopDropShadowBrush.BeginAnimation(Brush.OpacityProperty, animation);
                animation = new DoubleAnimation(0.0, duration);
                buttonChrome.InnerBorderPen.Brush.BeginAnimation(Brush.OpacityProperty, animation);
                ColorAnimation animation2 = new ColorAnimation(Color.FromRgb(194, 228, 246), duration);
                GradientStopCollection gradientStops = ((LinearGradientBrush)buttonChrome.BackgroundOverlay).GradientStops;
                gradientStops[0].BeginAnimation(GradientStop.ColorProperty, animation2);
                gradientStops[1].BeginAnimation(GradientStop.ColorProperty, animation2);
                animation2 = new ColorAnimation(Color.FromRgb(171, 218, 243), duration);
                gradientStops[2].BeginAnimation(GradientStop.ColorProperty, animation2);
                animation2 = new ColorAnimation(Color.FromRgb(144, 203, 235), duration);
                gradientStops[3].BeginAnimation(GradientStop.ColorProperty, animation2);
                animation2 = new ColorAnimation(Color.FromRgb(44, 98, 139), duration);
                buttonChrome.BorderOverlayPen.Brush.BeginAnimation(SolidColorBrush.ColorProperty, animation2);
                return;
            }

            if (buttonChrome.localResources == null)
            {
                buttonChrome.InvalidateVisual();
                return;
            }

            bool renderMouseOver = buttonChrome.RenderMouseOver;
            Duration duration2 = new Duration(TimeSpan.FromSeconds(0.1));
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = duration2;
            buttonChrome.LeftDropShadowBrush.BeginAnimation(Brush.OpacityProperty, doubleAnimation);
            buttonChrome.TopDropShadowBrush.BeginAnimation(Brush.OpacityProperty, doubleAnimation);
            buttonChrome.InnerBorderPen.Brush.BeginAnimation(Brush.OpacityProperty, doubleAnimation);
            if (!renderMouseOver)
            {
                buttonChrome.BorderOverlayPen.Brush.BeginAnimation(Brush.OpacityProperty, doubleAnimation);
                buttonChrome.BackgroundOverlay.BeginAnimation(Brush.OpacityProperty, doubleAnimation);
            }

            ColorAnimation colorAnimation = new ColorAnimation();
            colorAnimation.Duration = duration2;
            buttonChrome.BorderOverlayPen.Brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
            GradientStopCollection gradientStops2 = ((LinearGradientBrush)buttonChrome.BackgroundOverlay).GradientStops;
            gradientStops2[0].BeginAnimation(GradientStop.ColorProperty, colorAnimation);
            gradientStops2[1].BeginAnimation(GradientStop.ColorProperty, colorAnimation);
            gradientStops2[2].BeginAnimation(GradientStop.ColorProperty, colorAnimation);
            gradientStops2[3].BeginAnimation(GradientStop.ColorProperty, colorAnimation);
        }

        /// <summary>
        /// 画背景
        /// </summary>
        /// <param name="dc">绘图的上下文</param>
        /// <param name="bounds">矩形</param>
        private void DrawBackground(DrawingContext dc, ref Rect bounds)
        {
            if (!this.IsEnabled && !this.RoundCorners)
            {
                return;
            }

            Brush brush = this.Background;
            if (bounds.Width > 4.0 && bounds.Height > 4.0)
            {
                Rect rectangle = new Rect(bounds.Left + 1.0, bounds.Top + 1.0, bounds.Width - 2.0, bounds.Height - 2.0);
                if (brush != null)
                {
                    dc.DrawRectangle(brush, null, rectangle);
                }

                brush = this.BackgroundOverlay;

                if (brush != null)
                {
                    dc.DrawRectangle(brush, null, rectangle);
                }
            }
        }

        /// <summary>
        /// 画阴影
        /// </summary>
        /// <param name="dc">绘图的上下文</param>
        /// <param name="bounds">矩形</param>
        private void DrawDropShadows(DrawingContext dc, ref Rect bounds)
        {
            if (bounds.Width > 4.0 && bounds.Height > 4.0)
            {
                Brush leftDropShadowBrush = this.LeftDropShadowBrush;
                if (leftDropShadowBrush != null)
                {
                    dc.DrawRectangle(leftDropShadowBrush, null, new Rect(1.0, 1.0, 2.0, bounds.Bottom - 2.0));
                }

                Brush topDropShadowBrush = this.TopDropShadowBrush;

                if (topDropShadowBrush != null)
                {
                    dc.DrawRectangle(topDropShadowBrush, null, new Rect(1.0, 1.0, bounds.Right - 2.0, 2.0));
                }
            }
        }

        /// <summary>
        /// 画外边框
        /// </summary>
        /// <param name="dc">绘图的上下文</param>
        /// <param name="bounds">矩形</param>
        private void DrawBorder(DrawingContext dc, ref Rect bounds)
        {
            if (bounds.Width >= 5.0 && bounds.Height >= 5.0)
            {
                Brush brush = this.BorderBrush;
                Pen pen = null;
                if (brush != null)
                {
                    if (ImageButtonChrome.commonBorderPen == null)
                    {
                        lock (ImageButtonChrome.resourceAccess)
                        {
                            if (ImageButtonChrome.commonBorderPen == null)
                            {
                                if (!brush.IsFrozen && brush.CanFreeze)
                                {
                                    brush = brush.Clone();
                                    brush.Freeze();
                                }

                                Pen pen2 = new Pen(brush, 1.0);

                                if (pen2.CanFreeze)
                                {
                                    pen2.Freeze();
                                    ImageButtonChrome.commonBorderPen = pen2;
                                }
                            }
                        }
                    }

                    if (ImageButtonChrome.commonBorderPen != null && brush == ImageButtonChrome.commonBorderPen.Brush)
                    {
                        pen = ImageButtonChrome.commonBorderPen;
                    }
                    else
                    {
                        if (!brush.IsFrozen && brush.CanFreeze)
                        {
                            brush = brush.Clone();
                            brush.Freeze();
                        }

                        pen = new Pen(brush, 1.0);

                        if (pen.CanFreeze)
                        {
                            pen.Freeze();
                        }
                    }
                }

                Pen borderOverlayPen = this.BorderOverlayPen;

                if (pen != null || borderOverlayPen != null)
                {
                    if (this.RoundCorners)
                    {
                        Rect rectangle = new Rect(bounds.Left + 0.5, bounds.Top + 0.5, bounds.Width - 1.0, bounds.Height - 1.0);
                        if (this.IsEnabled && pen != null)
                        {
                            dc.DrawRoundedRectangle(null, pen, rectangle, 2.75, 2.75);
                        }

                        if (borderOverlayPen != null)
                        {
                            dc.DrawRoundedRectangle(null, borderOverlayPen, rectangle, 2.75, 2.75);
                            return;
                        }
                    }
                    else
                    {
                        PathFigure pathFigure = new PathFigure();
                        pathFigure.StartPoint = new Point(0.5, 0.5);
                        pathFigure.Segments.Add(new LineSegment(new Point(0.5, bounds.Bottom - 0.5), true));
                        pathFigure.Segments.Add(new LineSegment(new Point(bounds.Right - 2.5, bounds.Bottom - 0.5), true));
                        pathFigure.Segments.Add(new ArcSegment(new Point(bounds.Right - 0.5, bounds.Bottom - 2.5), new Size(2.0, 2.0), 0.0, false, SweepDirection.Counterclockwise, true));
                        pathFigure.Segments.Add(new LineSegment(new Point(bounds.Right - 0.5, bounds.Top + 2.5), true));
                        pathFigure.Segments.Add(new ArcSegment(new Point(bounds.Right - 2.5, bounds.Top + 0.5), new Size(2.0, 2.0), 0.0, false, SweepDirection.Counterclockwise, true));
                        pathFigure.IsClosed = true;
                        PathGeometry pathGeometry = new PathGeometry();
                        pathGeometry.Figures.Add(pathFigure);

                        if (this.IsEnabled && pen != null)
                        {
                            dc.DrawGeometry(null, pen, pathGeometry);
                        }

                        if (borderOverlayPen != null)
                        {
                            dc.DrawGeometry(null, borderOverlayPen, pathGeometry);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 画内边框
        /// </summary>
        /// <param name="dc">绘图的上下文</param>
        /// <param name="bounds">矩形</param>
        private void DrawInnerBorder(DrawingContext dc, ref Rect bounds)
        {
            if (!this.IsEnabled && !this.RoundCorners)
            {
                return;
            }

            if (bounds.Width >= 4.0 && bounds.Height >= 4.0)
            {
                Pen innerBorderPen = this.InnerBorderPen;
                if (innerBorderPen != null)
                {
                    dc.DrawRoundedRectangle(null, innerBorderPen, new Rect(bounds.Left + 1.5, bounds.Top + 1.5, bounds.Width - 3.0, bounds.Height - 3.0), 1.75, 1.75);
                }
            }
        }

        #endregion

        #region 私有类

        /// <summary>
        /// 本地资源类
        /// </summary>
        private class LocalResources
        {
            /// <summary>
            /// 边框画笔
            /// </summary>
            public Pen BorderOverlayPen { get; set; }

            /// <summary>
            /// 内边框画笔
            /// </summary>
            public Pen InnerBorderPen { get; set; }

            /// <summary>
            /// 背景颜色画刷
            /// </summary>
            public LinearGradientBrush BackgroundOverlay { get; set; }

            /// <summary>
            /// 左阴影颜色画刷
            /// </summary>
            public LinearGradientBrush LeftDropShadowBrush { get; set; }

            /// <summary>
            /// 顶部阴影颜色画刷
            /// </summary>
            public LinearGradientBrush TopDropShadowBrush { get; set; }
        }

        #endregion
    }
}
