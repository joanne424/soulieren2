// <copyright file="FullScreenManager.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author> donggj </author>
// <date> 2014/1/14 14:15:10 </date>
// <modify>
//   修改人：donggj
//   修改时间：2014/1/14 14:15:10
//   修改描述：新建 FullScreenManager
//   版本：1.0
// </modify>
// <review>
//   review人：
//   review时间：
// </review>

namespace DM2.Ent.Client.Views.ExtendClass
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;

    /// <summary>
    /// 修正WPF 全屏问题
    /// </summary>
    public static class FullScreenManager
    {
        /// <summary>
        /// 修改WPF窗口最大化问题
        /// </summary>
        /// <param name="wpfWindow">Window窗体对象</param>
        public static void RepairWpfWindowFullScreenBehavior(Window wpfWindow)
        {
            if (wpfWindow == null)
            {
                return;
            }

            if (wpfWindow.WindowState == WindowState.Maximized)
            {
                wpfWindow.WindowState = WindowState.Normal;
                wpfWindow.Loaded += delegate { wpfWindow.WindowState = WindowState.Maximized; };
            }

            wpfWindow.SourceInitialized += delegate
            {
                IntPtr handle = (new WindowInteropHelper(wpfWindow)).Handle;

                if (handle.ToInt32() == 0)
                {
                    return;
                }

                HwndSource source = HwndSource.FromHwnd(handle);
                if (source != null)
                {
                    source.AddHook(WindowProc);
                }
            };
        }

        /// <summary>
        /// 获取监控信息
        /// </summary>
        /// <param name="handleMonitor">监控句柄</param>
        /// <param name="monitoryInfo">MONITORINFO对象</param>
        /// <returns>返回</returns>
        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr handleMonitor, MONITORINFO monitoryInfo);

        /// <summary>
        /// 监控Window
        /// </summary>
        /// <param name="handle">句柄</param>
        /// <param name="flags">标识</param>
        /// <returns>返回句柄</returns>
        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        #region 私有方法
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="handle">句柄</param>
        /// <param name="msg">消息</param>
        /// <param name="firstParam">The parameter is not used.</param>
        /// <param name="secParam">参数</param>
        /// <param name="handled">handled</param>
        /// <returns>返回句柄</returns>
        private static IntPtr WindowProc(IntPtr handle, int msg, IntPtr firstParam, IntPtr secParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(handle, secParam);
                    handled = true;
                    break;
            }

            return (IntPtr)0;
        }

        /// <summary>
        /// 获取窗体最小最大相关信息
        /// </summary>
        /// <param name="handle">监控句柄</param>
        /// <param name="param">参数句柄</param>
        private static void WmGetMinMaxInfo(IntPtr handle, IntPtr param)
        {
            var mmi = (MINMAXINFO)Marshal.PtrToStructure(param, typeof(MINMAXINFO));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            int monitor_DefaultToneArest = 0x00000002;
            IntPtr monitor = MonitorFromWindow(handle, monitor_DefaultToneArest);

            if (monitor != IntPtr.Zero)
            {
                var monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rectWorkArea = monitorInfo.RectWork;
                RECT rectMonitorArea = monitorInfo.RectMonitor;
                mmi.MaxPositionPt.X = Math.Abs(rectWorkArea.Left - rectMonitorArea.Left);
                mmi.MaxPositionPt.Y = Math.Abs(rectWorkArea.Top - rectMonitorArea.Top);
                mmi.MaxSizePt.X = Math.Abs(rectWorkArea.Right - rectWorkArea.Left);
                mmi.MaxSizePt.Y = Math.Abs(rectWorkArea.Bottom - rectWorkArea.Top);
            }

            Marshal.StructureToPtr(mmi, param, true);
        }
        #endregion

        #region Nested type: MINMAXINFO

        /// <summary>
        /// 最小最大结构体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct MINMAXINFO
        {
            /// <summary>
            /// 恢复
            /// </summary>
            public POINT ReservedPt;

            /// <summary>
            /// 最大尺寸
            /// </summary>
            public POINT MaxSizePt;

            /// <summary>
            /// 最大位置
            /// </summary>
            public POINT MaxPositionPt;

            /// <summary>
            /// 最小拖动尺寸
            /// </summary>
            public POINT MinTrackSizePt;

            /// <summary>
            /// 最大拖动尺寸
            /// </summary>
            public POINT MaxTrackSizePt;
        }

        #endregion

        #region Nested type: POINT

        /// <summary>
        /// POINT aka POINTAPI
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            /// <summary>
            /// x coordinate of point.
            /// </summary>
            public int X;

            /// <summary>
            /// y coordinate of point.
            /// </summary>
            public int Y;

            /// <summary>
            /// Initializes a new instance of the <see cref="POINT" /> struct
            /// </summary>
            /// <param name="x">X坐标</param>
            /// <param name="y">Y坐标</param>
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        #endregion

        #region Nested type: RECT

        /// <summary>
        /// Win32
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal struct RECT
        {
            #region 字段
            /// <summary>
            /// Empty
            /// </summary>
            public static readonly RECT Empty;

            #endregion

            #region 属性

            /// <summary>
            /// Left
            /// </summary>
            public int Left;

            /// <summary>
            /// Top
            /// </summary>
            public int Top;

            /// <summary>
            /// Right
            /// </summary>
            public int Right;

            /// <summary>
            /// Bottom
            /// </summary>
            public int Bottom;

            #endregion

            /// <summary>
            /// Initializes a new instance of the <see cref="RECT" /> struct
            /// </summary>
            /// <param name="left">left</param>
            /// <param name="top">top</param>
            /// <param name="right">right</param>
            /// <param name="bottom">bottom</param>
            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RECT" /> struct
            /// </summary>
            /// <param name="rectSrc">Rect对象</param>
            public RECT(RECT rectSrc)
            {
                this.Left = rectSrc.Left;
                this.Top = rectSrc.Top;
                this.Right = rectSrc.Right;
                this.Bottom = rectSrc.Bottom;
            }

            /// <summary>
            /// Width
            /// </summary>
            public int Width
            {
                get
                {
                    // Abs needed for BIDI OS
                    return Math.Abs(this.Right - this.Left);
                }
            }

            /// <summary>
            /// Height
            /// </summary>
            public int Height
            {
                get
                {
                    return this.Bottom - this.Top;
                }
            }

            /// <summary>
            /// 是否为空
            /// </summary>
            public bool IsEmpty
            {
                get
                {
                    // BUGBUG : On Bidi OS (hebrew arabic) left > right
                    return this.Left >= this.Right || this.Top >= this.Bottom;
                }
            }

            #region 公开方法

            /// <summary>
            ///  Determine if 2 RECT are equal (deep compare)
            /// </summary>
            /// <param name="rect1">rect1</param>
            /// <param name="rect2">rect2</param>
            /// <returns>比较结果</returns>
            public static bool operator ==(RECT rect1, RECT rect2)
            {
                return rect1.Left == rect2.Left && rect1.Top == rect2.Top && rect1.Right == rect2.Right && rect1.Bottom == rect2.Bottom;
            }

            /// <summary>
            /// Determine if 2 RECT are different(deep compare)
            /// </summary>
            /// <param name="rect1">rect1</param>
            /// <param name="rect2">rect2</param>
            /// <returns>比较结果</returns>
            public static bool operator !=(RECT rect1, RECT rect2)
            {
                return !(rect1 == rect2);
            }

            /// <summary>
            /// Return a user friendly representation of this struct 
            /// </summary>
            /// <returns>返回Rect信息</returns>
            public override string ToString()
            {
                if (this == Empty)
                {
                    return "RECT {Empty}";
                }

                return "RECT { left : " + this.Left + " / top : " + this.Top + " / right : " + this.Right + " / bottom : " + this.Bottom + " }";
            }

            /// <summary>
            /// Determine if 2 RECT are equal (deep compare)
            /// </summary>
            /// <param name="obj">对象</param>
            /// <returns>返回结果</returns>
            public override bool Equals(object obj)
            {
                if (!(obj is Rect))
                {
                    return false;
                }

                return this == (RECT)obj;
            }

            /// <summary>
            /// Return the HashCode for this struct (not garanteed to be unique)
            /// </summary>
            /// <returns>Hash Code</returns>
            public override int GetHashCode()
            {
                return this.Left.GetHashCode() + this.Top.GetHashCode() + this.Right.GetHashCode() + this.Bottom.GetHashCode();
            }
            #endregion
        }

        #endregion

        #region Nested type: MONITORINFO

        /// <summary>
        /// 监控类
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class MONITORINFO
        {
            /// <summary>
            /// 尺寸
            /// </summary>            
            private int size = Marshal.SizeOf(typeof(MONITORINFO));

            /// <summary>
            /// 监控大小
            /// </summary>            
            public RECT RectMonitor { get; set; }

            /// <summary>
            /// 工作大小
            /// </summary>            
            public RECT RectWork { get; set; }

            /// <summary>
            /// 标识
            /// </summary>            
            public int Flags { get; set; }
        }

        #endregion
    }
}
