// <copyright file="WindowsResourceDictionary.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2017/05/25 06:02:58 </date>
// <summary> 窗口资源定义文件类 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2017/05/25 06:02:58
//      修改描述：新建 WindowsResourceDictionary.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >
namespace DM2.Ent.Client.Views.Resources.Theme.DarkBlue
{
    #region

    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Shapes;

    #endregion

    /// <summary>
    /// 窗口资源定义文件类
    /// 之所以添加此资源类是为了在模板或者样式中使用代码
    /// 可以有两种解决方案一种是使用CaliburnMicro的Message绑定，但是页面显示处理代码使用此方式很不优雅而且麻烦
    /// 对于拖动代码还需要在使用位置重复添加
    /// </summary>
    public partial class WindowsResourceDictionary : ResourceDictionary
    {
        #region Fields

        /// <summary>
        /// 是否处于底部高度调制中
        /// </summary>
        private bool isBottomHightChanging;

        /// <summary>
        /// 是否处于左下对角调整中
        /// </summary>
        private bool isLeftBottomGrip;

        /// <summary>
        /// 是否处于左上对角调整中
        /// </summary>
        private bool isLeftTopGrip;

        /// <summary>
        /// 是否处于左宽度调制中
        /// </summary>
        private bool isLeftWinden;

        /// <summary>
        /// 是否处于右下对角调整中
        /// </summary>
        private bool isRightBottomGrip;

        /// <summary>
        /// 是否处于右上对角调整中
        /// </summary>
        private bool isRightTopGrip;

        /// <summary>
        /// 是否处于右宽度调制中
        /// </summary>
        private bool isRightWinden;

        /// <summary>
        /// 是否处于顶部高度调制中
        /// </summary>
        private bool isTopHightChanging;

        /// <summary>
        /// 上一次标题鼠标点击谈起的时间
        /// </summary>
        private DateTime lasttitleLeftDownUp = DateTime.MinValue;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsResourceDictionary"/> class.
        /// </summary>
        public WindowsResourceDictionary()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 左键按下拖动逻辑
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void MouseLeftDragMove(object sender, MouseButtonEventArgs e)
        {
            var win = (Window)((FrameworkElement)sender).TemplatedParent;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                win.DragMove();
            }
        }

        /// <summary>
        /// 窗口最大化事件处理
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnMaximizeWindow(object sender, RoutedEventArgs e)
        {
            var win = (Window)((FrameworkElement)sender).TemplatedParent;
            win.WindowState = win.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        /// <summary>
        /// 窗口最小化事件处理
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnMinimizeWindow(object sender, RoutedEventArgs e)
        {
            var win = (Window)((FrameworkElement)sender).TemplatedParent;
            win.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 在鼠标悬停此窗口后将Topmost置为false
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="mouseEventArgs">
        /// The mouse event args.
        /// </param>
        private void OnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            ////在鼠标悬停此窗口后将Topmost置为false,原来期望实现的效果是只要鼠标在窗口上划过，表明用户已经注意到窗口，即将topmost去掉，但是
            ////实际效果是，虽然Topmost已经置为为False，但是如果鼠标不点击一下窗口，窗口依然是一直保持在最前，原因未搞懂
            var win = (Window)((FrameworkElement)sender).TemplatedParent;
            win.Topmost = false;

            ////另外一种设想将窗口的拥有者设置为主窗口，则当前窗口始终浮动在父窗口之前，没有找到可以让鼠标点击主窗口时主窗口显示在最上
            ////this.Owner = Application.Current.MainWindow;
        }

        /// <summary>
        /// 响应底部高度改变
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsBottomHightChanging(object sender, MouseEventArgs e)
        {
            if (this.isBottomHightChanging)
            {
                var rect = (Rectangle)sender;
                rect.CaptureMouse();
                var win = (Window)rect.TemplatedParent;
                double newHeight = e.GetPosition(win).Y + 5;
                if (newHeight > 0)
                {
                    win.Height = newHeight;
                }
            }
        }

        /// <summary>
        /// 结束底部高度改变模式
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsEndBottomHight(object sender, MouseEventArgs e)
        {
            this.isBottomHightChanging = false;
            var rect = (Rectangle)sender;
            rect.ReleaseMouseCapture();
        }

        /// <summary>
        /// 结束左下对角改变模式
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsEndLeftBottomGrip(object sender, MouseEventArgs e)
        {
            this.isLeftBottomGrip = false;
            var rect = (Rectangle)sender;
            rect.ReleaseMouseCapture();
        }

        /// <summary>
        /// 结束左上对角改变模式
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsEndLeftTopGrip(object sender, MouseEventArgs e)
        {
            this.isLeftTopGrip = false;
            var rect = (Rectangle)sender;
            rect.ReleaseMouseCapture();
        }

        /// <summary>
        /// 结束左宽度改变模式
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsEndLeftWiden(object sender, MouseEventArgs e)
        {
            this.isLeftWinden = false;
            var rect = (Rectangle)sender;
            rect.ReleaseMouseCapture();
        }

        /// <summary>
        /// 结束左上对角改变模式
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsEndRightBottomGrip(object sender, MouseEventArgs e)
        {
            this.isRightBottomGrip = false;
            var rect = (Rectangle)sender;
            rect.ReleaseMouseCapture();
        }

        /// <summary>
        /// 结束右上对角改变模式
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsEndRightTopGrip(object sender, MouseEventArgs e)
        {
            this.isRightTopGrip = false;
            var rect = (Rectangle)sender;
            rect.ReleaseMouseCapture();
        }

        /// <summary>
        /// 结束右宽度改变模式
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsEndRightWiden(object sender, MouseEventArgs e)
        {
            this.isRightWinden = false;
            var rect = (Rectangle)sender;
            rect.ReleaseMouseCapture();
        }

        /// <summary>
        /// 结束顶部高度改变模式
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsEndTopHight(object sender, MouseEventArgs e)
        {
            this.isTopHightChanging = false;
            var rect = (Rectangle)sender;
            rect.ReleaseMouseCapture();
        }

        /// <summary>
        /// 进入底部高度改变模式
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsInitiateBottomHight(object sender, MouseEventArgs e)
        {
            this.isBottomHightChanging = true;
        }

        /// <summary>
        /// 进入左下对角改变模式
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsInitiateLeftBottomGrip(object sender, MouseEventArgs e)
        {
            this.isLeftBottomGrip = true;
        }

        /// <summary>
        /// 进入左上对角改变模式
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsInitiateLeftTopGrip(object sender, MouseEventArgs e)
        {
            this.isLeftTopGrip = true;
        }

        /// <summary>
        /// 进入左宽度改变模式
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsInitiateLeftWiden(object sender, MouseEventArgs e)
        {
            this.isLeftWinden = true;
        }

        /// <summary>
        /// 进入右下对角改变模式
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsInitiateRightBottomGrip(object sender, MouseEventArgs e)
        {
            this.isRightBottomGrip = true;
        }

        /// <summary>
        /// 进入右上对角改变模式
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsInitiateRightTopGrip(object sender, MouseEventArgs e)
        {
            this.isRightTopGrip = true;
        }

        /// <summary>
        /// 进入右宽度改变模式
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsInitiateRightWiden(object sender, MouseEventArgs e)
        {
            this.isRightWinden = true;
        }

        /// <summary>
        /// 进入顶部高度改变模式
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsInitiateTopHight(object sender, MouseEventArgs e)
        {
            this.isTopHightChanging = true;
        }

        /// <summary>
        /// 响应左下对角改变
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsLeftBottomGrip(object sender, MouseEventArgs e)
        {
            if (this.isLeftBottomGrip)
            {
                var rect = (Rectangle)sender;
                rect.CaptureMouse();
                var win = (Window)rect.TemplatedParent;
                double newLeft = win.Left + e.GetPosition(win).X - 5;
                double newWidth = win.Width + win.Left - newLeft;
                double newHeight = e.GetPosition(win).Y + 5;
                if (newLeft > 0 && newHeight > 0)
                {
                    win.Left = newLeft;
                    win.Width = newWidth;
                    win.Height = newHeight;
                }
            }
        }

        /// <summary>
        /// 响应左上对角改变
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsLeftTopGrip(object sender, MouseEventArgs e)
        {
            if (this.isLeftTopGrip)
            {
                var rect = (Rectangle)sender;
                rect.CaptureMouse();
                var win = (Window)rect.TemplatedParent;
                double newLeft = win.Left + e.GetPosition(win).X - 5;
                double newWidth = win.Width + win.Left - newLeft;
                double newTop = win.Top + e.GetPosition(win).Y - 5;
                double newHeight = win.Height + win.Top - newTop;
                if (newLeft > 0 && newTop > 0)
                {
                    win.Left = newLeft;
                    win.Width = newWidth;
                    win.Top = newTop;
                    win.Height = newHeight;
                }
            }
        }

        /// <summary>
        /// 响应左宽度改变
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsLeftWiden(object sender, MouseEventArgs e)
        {
            if (this.isLeftWinden)
            {
                var rect = (Rectangle)sender;
                rect.CaptureMouse();
                var win = (Window)rect.TemplatedParent;
                double newLeft = win.Left + e.GetPosition(win).X - 5;
                double newWidth = win.Width - newLeft + win.Left;
                if (newLeft > 0)
                {
                    win.Left = newLeft;
                    win.Width = newWidth;
                }
            }
        }

        /// <summary>
        /// 响应右下对角改变
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsRightBottomGrip(object sender, MouseEventArgs e)
        {
            if (this.isRightBottomGrip)
            {
                var rect = (Rectangle)sender;
                rect.CaptureMouse();
                var win = (Window)rect.TemplatedParent;
                double newWidth = e.GetPosition(win).X + 5;
                double newHeight = e.GetPosition(win).Y + 5;
                if (newWidth > 0 && newHeight > 0)
                {
                    win.Width = newWidth;
                    win.Height = newHeight;
                }
            }
        }

        /// <summary>
        /// 响应右上对角改变
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsRightTopGrip(object sender, MouseEventArgs e)
        {
            if (this.isRightTopGrip)
            {
                var rect = (Rectangle)sender;
                rect.CaptureMouse();
                var win = (Window)rect.TemplatedParent;
                double newWidth = e.GetPosition(win).X + 5;
                double newTop = win.Top + e.GetPosition(win).Y - 5;
                double newHeight = win.Height + win.Top - newTop;
                if (newWidth > 0 && newTop > 0)
                {
                    win.Width = newWidth;
                    win.Top = newTop;
                    win.Height = newHeight;
                }
            }
        }

        /// <summary>
        /// 响应右宽度改变
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsRightWiden(object sender, MouseEventArgs e)
        {
            if (this.isRightWinden)
            {
                var rect = (Rectangle)sender;
                rect.CaptureMouse();
                var win = (Window)rect.TemplatedParent;
                double newWidth = e.GetPosition(win).X + 5;
                if (newWidth > 0)
                {
                    win.Width = newWidth;
                }
            }
        }

        /// <summary>
        /// 标题栏左键按下弹起响应
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsTitleMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var win = (Window)((FrameworkElement)sender).TemplatedParent;

            if ((DateTime.Now - this.lasttitleLeftDownUp).TotalMilliseconds < 500)
            {
                win.WindowState = win.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
            }

            this.lasttitleLeftDownUp = DateTime.Now;
        }

        /// <summary>
        /// 标题栏左键按下响应
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsTitleMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var win = (Window)((FrameworkElement)sender).TemplatedParent;
            if (e.ClickCount == 2)
            {
                win.WindowState = win.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                win.DragMove();
            }
        }

        /// <summary>
        /// 响应顶部高度改变
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void WindowsTopHightChanging(object sender, MouseEventArgs e)
        {
            if (this.isTopHightChanging)
            {
                var rect = (Rectangle)sender;
                rect.CaptureMouse();
                var win = (Window)rect.TemplatedParent;
                double newTop = win.Top + e.GetPosition(win).Y - 5;
                double newHeight = win.Height - newTop + win.Top;
                if (newTop > 0)
                {
                    win.Top = newTop;
                    win.Height = newHeight;
                }
            }
        }

        #endregion
    }
}