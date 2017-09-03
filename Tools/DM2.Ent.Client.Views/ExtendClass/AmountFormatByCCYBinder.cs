// <copyright file="AmountFormatByCCYBinder.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>wangrx</author>
// <date>2016/01/24 13:49:33</date>
// <modify>
//   修改人：wangrx
//   修改时间：2016/01/24 13:49:33
//   修改描述：新建 AmountFormatByCCYBinder.cs
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>

namespace DM2.Ent.Client.Views.ExtendClass
{
    using System;
    using System.Text;
    using System.Windows;

    /// <summary>
    /// 根据Currency配置格式化Amount
    /// </summary>
    public class AmountFormatByCCYBinder
        //:DependencyObject
    {
        /// <summary>
        /// 货币仓储
        /// </summary>
        //private static ICurrencyCacheRepository currencyCacheRepository;

        /// <summary>
        /// Initializes a new instance of the CurrencyIDToBaseVM class.
        /// </summary>
        public AmountFormatByCCYBinder()
        {
            //if (currencyCacheRepository == null)
            //{
            //    var parameter = new ParameterOverrides { { "varOwnerId", string.Empty } };
            //    currencyCacheRepository = IOCContainer.Instance.Container.Resolve<ICurrencyCacheRepository>(parameter);
            //}
        }

        #region By CurrencyID

        ///// <summary>
        ///// 货币ID
        ///// </summary>
        //public string CurrencyID
        //{
        //    get
        //    {
        //        return (string)GetValue(CurrencyIDProperty);
        //    }
        //    set
        //    {
        //        SetValue(CurrencyIDProperty, value);
        //    }
        //}

        /// <summary>
        /// 注册路径依赖属性
        /// </summary>
        public static readonly DependencyProperty CurrencyIDProperty = DependencyProperty.RegisterAttached(
            "CurrencyID", typeof(string), typeof(AmountFormatByCCYBinder), new PropertyMetadata(OnCurrencyIDPropertyValueChanged));

        /// <summary>
        /// 属性变化事件
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <param name="e">属性变化事件对象</param>
        private static void OnCurrencyIDPropertyValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            SetCurrencyID(obj, (string)e.NewValue);
        }

        /// <summary>
        /// 得到CurrencyID
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <returns>返回CurrencyID</returns>
        public static string GetCurrencyID(DependencyObject obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置CurrencyID
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <param name="value">依赖属性value</param>
        public static void SetCurrencyID(DependencyObject obj, string value)
        {
            //if (DesignerProperties.GetIsInDesignMode(obj))
            //{
            //    return;
            //}

            //if (string.IsNullOrEmpty(value))
            //{
            //    return;
            //}

            //BaseCurrencyVM currency = currencyCacheRepository.FindByID(value.ToString());
            //if (currency == null)
            //{
            //    return;
            //}

            //#region TextBox
            //TextBox textBox = obj as TextBox;
            //if (textBox != null)
            //{
            //    textBox.PreviewLostKeyboardFocus += (sender, e) => { textBox.Text = FormatNum(textBox.Text, currency); };
            //    textBox.PreviewTextInput += (sender, e) =>
            //    {
            //        if (!IsNumber(e.Text))
            //        {
            //            e.Handled = true;
            //        }
            //        else
            //        {
            //            e.Handled = false;
            //        }
            //    };
            //    return;
            //}
            //#endregion

            //#region TextBlock
            //TextBlock textBlock = obj as TextBlock;
            //if (textBlock != null)
            //{
            //    textBlock.TextInput += (sender, e) => { textBlock.Text = FormatNum(textBlock.Text, currency); };
            //    return;
            //}
            //#endregion

            //#region DataGridTextColumn
            //DataGridTextColumn dgTextColumn = obj as DataGridTextColumn;
            //if (dgTextColumn != null)
            //{
            //    //TO DO 待实现
            //    return;
            //}
            //#endregion
        }

        /// <summary>
        /// 数据格式化
        /// </summary>
        /// <param name="str">传入文本值</param>
        //public static string FormatNum(string str, BaseCurrencyVM currency)
        //{
        //    double d = convertToDouble(str);
        //    if (currency == null)
        //    {
        //        string strTemp = string.Format("{0:N3}", d);
        //        return strTemp;
        //    }

        //    double amountTemp = d;
        //    string amountStrTemp = string.Empty;

        //    string amountFormater = "#,##0.";
        //    for (int i = 0; i < currency.AmountDecimals; i++)
        //    {
        //        amountFormater += "0";
        //    }

        //    if (currency.RoundingMethod == RoundingEmun.Rounding)
        //    {
        //        amountTemp = Math.Round(amountTemp, currency.AmountDecimals);
        //        amountStrTemp = amountTemp.ToString(amountFormater);
        //    }
        //    else
        //    {
        //        amountStrTemp = amountTemp.ToString(amountFormater + "00");
        //        amountStrTemp = amountStrTemp.Substring(0, amountStrTemp.IndexOf('.') + currency.AmountDecimals + 1);
        //        amountTemp = Convert.ToDouble(amountStrTemp);
        //    }

        //    return amountStrTemp;
        //}

        /// <summary>
        /// 判断输入的文本是不是数字
        /// </summary>
        /// <param name="text">传入文本值</param>
        /// <returns>返回判断结果</returns>
        public static bool IsNumber(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            else
            {
                foreach (char c in text)
                {
                    if (!char.IsDigit(c))
                    {
                        if (c != '.' && c != ' ')
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 将字符串转换为Double（如字符无效将返回0）
        /// </summary>
        /// <param name="text">传入将要转换为Double的字符串</param>
        /// <returns>结果</returns>
        public static double convertToDouble(string text)
        {
            double d = 0d;
            StringBuilder result = new StringBuilder();
            char point = '.';
            int pointIndex = 0;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (char.IsDigit(c))
                {

                    result.Append(c);
                }
                else if (c == point)
                {
                    pointIndex = result.Length;
                }
            }
            if (pointIndex != 0)
            {
                result.Insert(pointIndex, point);
            }
            string str = result.ToString();
            double.TryParse(str, out d);
            return d;
        }

        #endregion
    }
}