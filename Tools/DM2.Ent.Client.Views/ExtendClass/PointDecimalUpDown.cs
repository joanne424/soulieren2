// <copyright file="PointDecimalUpDown.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>tongly</author>
// <date> 2014/5/13 17:47:56 </date>
// <summary> </summary>
// <modify>
//      修改人：tongly
//      修改时间：2014/5/13 17:47:56
//      修改描述：新建 PointDecimalUpDown
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>

namespace DM2.Ent.Client.Views.ExtendClass
{
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;

    using Xceed.Wpf.Toolkit;

    /// <summary>
    /// 用于输入数字的文本编辑控件。
    /// </summary>
    public class PointDecimalUpDown : DecimalUpDown
    {
        /// <summary>
        /// 用于绑定的依赖属性。
        /// </summary>
        public static readonly DependencyProperty PointProperty = DependencyProperty.Register(
            "Point", typeof(int), typeof(PointDecimalUpDown), new PropertyMetadata(2, PointValueChanged));

        /// <summary>
        /// 允许的输入的小数位。
        /// </summary>
        private int point;

        /// <summary>
        /// 验证按键输入Key集合
        /// </summary>
        private List<Key> keys;

        /// <summary>
        /// 验证使用的小数位。
        /// </summary>
        private int usePoint;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PointDecimalUpDown" /> class.
        /// </summary>
        public PointDecimalUpDown()
        {
            this.keys = new List<Key>();
            this.keys.Add(Key.NumPad0);
            this.keys.Add(Key.NumPad1);
            this.keys.Add(Key.NumPad2);
            this.keys.Add(Key.NumPad3);
            this.keys.Add(Key.NumPad4);
            this.keys.Add(Key.NumPad5);
            this.keys.Add(Key.NumPad6);
            this.keys.Add(Key.NumPad7);
            this.keys.Add(Key.NumPad8);
            this.keys.Add(Key.NumPad9);
            this.keys.Add(Key.Decimal);
            this.keys.Add(Key.D0);
            this.keys.Add(Key.D1);
            this.keys.Add(Key.D2);
            this.keys.Add(Key.D3);
            this.keys.Add(Key.D4);
            this.keys.Add(Key.D5);
            this.keys.Add(Key.D6);
            this.keys.Add(Key.D7);
            this.keys.Add(Key.D8);
            this.keys.Add(Key.D9);
            this.keys.Add(Key.Left);
            this.keys.Add(Key.Right);
            this.keys.Add(Key.Up);
            this.keys.Add(Key.Down);
        }
        
        /// <summary>
        /// 允许的输入的小数位。
        /// </summary>
        public int Point
        {
            get
            {
                return this.point;
            }

            set
            {
                if (this.point == value)
                {
                    return;
                }

                this.point = value;
                this.SetValue(PointProperty, value);
            }
        }

        /// <summary>
        /// 文本更改方法
        /// </summary>
        /// <param name="oldValue">原始数值</param>
        /// <param name="newValue">新的数值</param>
        protected override void OnTextChanged(string oldValue, string newValue)
        {
            string[] val = newValue.Split('.');

            if (val.Length == 2)
            {
                this.usePoint = val[1].Length;

                if (this.usePoint == this.Point + 1)
                {
                    this.TextBox.Text = oldValue;
                }
            }

            // 删除默认值为0带有格式化的数值。
            if (string.IsNullOrEmpty(newValue))
            {
                this.TextBox.Text = "0";
            }

            base.OnTextChanged(oldValue, newValue);
        }

        /// <summary>
        /// 增加方法
        /// </summary>
        protected override void OnIncrement()
        {
            ////根据小数点位数增加
            decimal outValue;
            bool isParsed = decimal.TryParse(this.Text, out outValue);
            if (isParsed)
            {
                this.Text = decimal.Add(outValue, this.ConvertToIncrement()).ToString();
            }
        }

        /// <summary>
        /// 相减方法
        /// </summary>
        protected override void OnDecrement()
        {
            ////根据小数点位数相减
            decimal outValue;
            bool isParsed = decimal.TryParse(this.Text, out outValue);
            if (isParsed)
            {
                if (decimal.Subtract(outValue, this.ConvertToIncrement()) >= 0)
                {
                    this.Text = decimal.Subtract(outValue, this.ConvertToIncrement()).ToString();
                }
            }
        }
        
        /// <summary>
        /// 键盘按下事件
        /// </summary>
        /// <param name="e">键盘事件</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            ////「BackSpace」「Delete」后退键正常删除操作
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                return;
            }

            ////允许输入的字符外，
            if (!this.keys.Contains(e.Key))
            {
                e.Handled = true;
                return;
            }
            else
            {
                e.Handled = false;
            }
        }
        
        /// <summary>
        /// 依赖属性变更时触发。
        /// </summary>
        /// <param name="obj">属性变更的对象</param>
        /// <param name="e">事件的参数</param>
        private static void PointValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "Point":
                    ((PointDecimalUpDown)obj).Point = (int)e.NewValue;
                    break;
            }
        }

        /// <summary>
        /// 拼接转换的字符串
        /// </summary>
        /// <returns>转换后的Decimal</returns>
        private decimal ConvertToIncrement()
        {
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append("0.");

            for (int i = 1; i < this.usePoint; i++)
            {
                strbuilder.Append("0");
            }

            strbuilder.Append("1");
            return decimal.Parse(strbuilder.ToString());
        }

        /// <summary>
        /// 重写应用模板方法
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InputMethod.SetIsInputMethodEnabled(this.TextBox, false);
        }
    }
}
