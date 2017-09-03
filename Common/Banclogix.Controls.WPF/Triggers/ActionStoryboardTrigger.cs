// <copyright file="ActionStoryboardTrigger.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>tangl</author>
// <date>2014/1/10 17:18:55</date>
// <modify>
//   修改人：tangl
//   修改时间：2014/1/10 17:18:55
//   修改描述：新建 ActionStoryboardTrigger.cs
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace Banclogix.Controls.Triggers
{
    /// <summary>
    /// 故事版动作触发器。
    /// </summary>
    public class ActionStoryboardTrigger : System.Windows.Interactivity.TriggerBase<DependencyObject>
    {
        /// <summary>
        /// 自定义依赖属性
        /// </summary>
        public static readonly DependencyProperty ActionProperty = DependencyProperty.Register(
            "Action", typeof(string), typeof(BindingStoryboardTrigger), new PropertyMetadata(OnValueChanged));
        
        /// <summary>
        /// Action 属性
        /// </summary>
        public string Action
        {
            get { return (string)GetValue(ActionProperty); }
            set { this.SetValue(ActionProperty, value); }
        }

        /// <summary>
        /// 故事板属性
        /// </summary>
        public Storyboard Storyboard { get; set; }

        /// <summary>
        /// 值改变时触发
        /// </summary>
        /// <param name="obj">依赖对象</param>
        /// <param name="e">相关参数</param>
        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var instance = obj as BindingStoryboardTrigger;

            switch (e.Property.Name)
            {
                case "Action":
                    instance.Binding = e.NewValue;
                    if (instance.Binding == null || instance.Storyboard == null)
                    {
                        return;
                    }

                    if (instance.Binding.Equals(instance.Value))
                    {
                        instance.Storyboard.Begin();
                    }

                    break;
                case "Value":
                    instance.Value = e.NewValue;
                    break;
            }
        }
    }
}