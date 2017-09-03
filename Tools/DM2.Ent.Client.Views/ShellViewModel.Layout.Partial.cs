// <copyright file="ShellViewModel.Layout.Partial.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2017/04/18 02:13:26 </date>
// <summary> 界面布局相关分部类 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2017/04/18 02:13:26
//      修改描述：新建 ShellViewModel.Layout.Partial.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >
namespace DM2.Ent.Client.Views
{
    #region

    using System;
    using System.Windows;

    using DM2.Ent.Client.Views.Properties;

    #endregion

    /// <summary>
    /// The shell view model.
    /// </summary>
    public partial class ShellViewModel
    {
        /// <summary>
        /// 保存窗口布局
        /// </summary>
        private void SaveLayout()
        {
            try
            {
                if (this.shellView.WindowState == WindowState.Maximized)
                {
                    Settings.Default.IsMaximized = true;
                }
                else
                {
                    Settings.Default.IsMaximized = false;
                }

                Properties.Settings.Default.WindowsPosition = this.shellView.RestoreBounds;

                Properties.Settings.Default.ContentTopRow = this.shellView.ContentTopRow.Height;
                Properties.Settings.Default.ContentBottomRow = this.shellView.ContentBottomRow.Height;
                Properties.Settings.Default.ContentLeftColumn = this.shellView.ContentLeftColumn.Width;
                Properties.Settings.Default.ContentRightColumn = this.shellView.ContentRightColumn.Width;

                Settings.Default.Save();
            }
            catch (Exception exception)
            {
                Infrastructure.Log.TraceManager.Error.Write("ShellView", exception, "Excepiton when save layout！");
            }
        }

        /// <summary>
        /// 加载窗口布局
        /// </summary>
        private void LoadLayout()
        {
            try
            {
                const double Tolerance = 0.001;
                if (Math.Abs(Settings.Default.WindowsPosition.Top) > Tolerance || 
                    Math.Abs(Settings.Default.WindowsPosition.Left) > Tolerance || 
                    Math.Abs(Settings.Default.WindowsPosition.Width) > Tolerance || 
                    Math.Abs(Settings.Default.WindowsPosition.Height) > Tolerance)
                {
                    this.shellView.Top = Settings.Default.WindowsPosition.Top;
                    this.shellView.Left = Settings.Default.WindowsPosition.Left;
                    this.shellView.Width = Settings.Default.WindowsPosition.Width;
                    this.shellView.Height = Settings.Default.WindowsPosition.Height;
                }

                if (Settings.Default.IsMaximized)
                {
                    this.shellView.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.shellView.WindowState = WindowState.Normal;
                }

                this.shellView.ContentTopRow.Height = Properties.Settings.Default.ContentTopRow;
                this.shellView.ContentBottomRow.Height = Properties.Settings.Default.ContentBottomRow;
                this.shellView.ContentLeftColumn.Width = Properties.Settings.Default.ContentLeftColumn;
                this.shellView.ContentRightColumn.Width = Properties.Settings.Default.ContentRightColumn;
            }
            catch (Exception exception)
            {
                Infrastructure.Log.TraceManager.Error.Write("ShellView", exception, "Excepiton when load layout！");
            }
        }
    }
}