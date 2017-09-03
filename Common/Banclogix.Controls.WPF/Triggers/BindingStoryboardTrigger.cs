// <copyright file="BindingStoryboardTrigger.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>Tangl</author>
// <date>2013/10/16 1:17:09</date>
// <modify>
//   修改人：Tangl
//   修改时间：2013/10/16 1:17:09
//   修改描述：新建 BindingStoryboardTrigger.cs
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>

using System.Windows;
using System.Windows.Media.Animation;

namespace Banclogix.Controls.Triggers
{
    /// <summary>
    /// 动画故事板触发器
    /// </summary>
    public sealed class BindingStoryboardTrigger : System.Windows.Interactivity.TriggerBase<DependencyObject>
    {
        /// <summary>
        /// 自定义依赖属性
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(object), typeof(BindingStoryboardTrigger), new PropertyMetadata(OnValueChanged));

        /// <summary>
        /// 自定义依赖属性
        /// </summary>
        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register(
            "Binding", typeof(object), typeof(BindingStoryboardTrigger), new PropertyMetadata(OnValueChanged));

        /// <summary>
        /// 值（待定）
        /// </summary>
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// binding 属性
        /// </summary>
        public object Binding
        {
            get { return (object)GetValue(BindingProperty); }
            set { this.SetValue(BindingProperty, value); }
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
                case "Binding":
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