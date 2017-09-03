// <copyright file="CustomMenuItem.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author> donggj </author>
// <date> 2014/1/14 11:11:28 </date>
// <modify>
//   修改人：donggj
//   修改时间：2014/1/14 11:11:28
//   修改描述：新建 CustomMenuItem
//   版本：1.0
// </modify>
// <review>
//   review人：
//   review时间：
// </review>

namespace DM2.Ent.Client.Views.ExtendClass
{
    using System.ComponentModel;

    /// <summary>
    /// 菜单按钮类
    /// </summary>
    public class CustomMenuItem : INotifyPropertyChanged
    {
        /// <summary>
        /// 标题
        /// </summary>
        private string title;

        /// <summary>
        /// 菜单名称，唯一标识
        /// </summary>
        private string name;

        /// <summary>
        /// 图片路径
        /// </summary>
        private string source;

        /// <summary>
        /// 鼠标
        /// </summary>
        private string toolTip;

        /// <summary>
        /// 字段 宽度
        /// </summary>
        private double width = 86;

        /// <summary>
        /// 字段 鼠标悬停图片
        /// </summary> 
        private string overSource;

        private bool isEnabled = true;

        /// <summary>
        /// 字段 是否显示
        /// </summary>
        private System.Windows.Visibility visibilityFlag = System.Windows.Visibility.Visible;

        /// <summary>
        /// 属性通知事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性 宽度
        /// </summary>
        public double Width
        {
            get
            {
                return this.width;
            }

            set
            {
                this.width = value;
                this.NotifyOfPropertyChange("Width");
            }
        }

        /// <summary>
        /// 属性 鼠标悬停图片
        /// </summary>
        public string OverSource
        {
            get
            {
                return this.overSource;
            }

            set
            {
                this.overSource = value;
                this.NotifyOfPropertyChange("OverSource");
            }
        }


        /// <summary>
        /// 图片路径
        /// </summary>
        public string Source
        {
            get
            {
                return this.source;
            }

            set
            {
                this.source = value;
                this.NotifyOfPropertyChange("Source");
            }
        }

        /// <summary>
        /// 提示
        /// </summary>
        public string ToolTip
        {
            get
            {
                return this.toolTip;
            }

            set
            {
                this.toolTip = value;
                this.NotifyOfPropertyChange("ToolTip");
            }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
                this.NotifyOfPropertyChange("Title");
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                this.NotifyOfPropertyChange("Name");
            }
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }

            set
            {
                this.isEnabled = value;
                this.NotifyOfPropertyChange("IsEnabled");
            }
        }

        /// <summary>
        /// 触发的函数名称
        /// </summary>
        public string MethodName
        {
            get;
            set;
        }

        /// <summary>
        /// 字段 是否显示
        /// </summary>
        public System.Windows.Visibility VisibilityFlag
        {
            get
            {
                return this.visibilityFlag;
            }

            set
            {
                this.visibilityFlag = value;
                this.NotifyOfPropertyChange("VisibilityFlag");
            }
        }

        /// <summary>
        /// 属性通知方法
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        private void NotifyOfPropertyChange(string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
