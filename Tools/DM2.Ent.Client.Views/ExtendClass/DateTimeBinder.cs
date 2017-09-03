// <copyright file="DateTimeBinder.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>wangrx</author>
// <date>2013/10/31 7:49:33</date>
// <modify>
//   修改人：wangrx
//   修改时间：2013/10/31 7:49:33
//   修改描述：新建 DateTimeBinderHelper.cs
//   版本：1.0
// </modify>
// <review>
//   Review人：
//   Review时间：
// </review>

namespace DM2.Ent.Client.Views.ExtendClass
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// 枚举绑定帮助类
    /// </summary>
    public class DateTimeBinder
    {
        #region DatePicker

        #region BlackoutDates

        /// <summary>
        /// 起始时间
        /// </summary>
        private static DateTime BlackoutStart;

        /// <summary>
        /// 获取BlackoutStart
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <returns>返回BlackoutStart</returns>
        public static DateTime GetBlackoutStart(DependencyObject obj)
        {
            return BlackoutStart;
        }

        /// <summary>
        /// 设置BlackoutStart
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <param name="value">新的BlackoutStart值</param>
        public static void SetBlackoutStart(DependencyObject obj, object value)
        {
            if (DesignerProperties.GetIsInDesignMode(obj))
            {
                return;
            }

            if (value == null)
            {
                return;
            }

            DateTime blackoutStart;
            if (!DateTime.TryParse(value.ToString(), out blackoutStart))
            {
                return;
            }

            BlackoutStart = blackoutStart;
        }
        
        /// <summary>
        /// 属性变化事件
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <param name="e">属性变化事件对象</param>
        private static void OnBlackoutStartPropertyValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(obj))
            {
                return;
            }

            if (e.NewValue == null)
            {
                return;
            }

            DateTime blackoutStart;
            if (!DateTime.TryParse(e.NewValue.ToString(), out blackoutStart))
            {
                return;
            }

            BlackoutStart = blackoutStart;
            cdr.Start = blackoutStart;

            DatePicker dp = obj as DatePicker;
            if (!dp.BlackoutDates.Contains(cdr))
            {
                dp.BlackoutDates.Add(cdr);
            }
        }
        

        /// <summary>
        /// 截止时间
        /// </summary>
        private static DateTime BlackoutEnd;

        /// <summary>
        /// 获取BlackoutEnd
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <returns>返回BlackoutEnd</returns>
        public static DateTime GetBlackoutEnd(DependencyObject obj)
        {
            return BlackoutEnd;
        }

        /// <summary>
        /// 设置BlackoutEnd
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <param name="value">新的BlackoutEnd值</param>
        public static void SetBlackoutEnd(DependencyObject obj, object value)
        {
            if (DesignerProperties.GetIsInDesignMode(obj))
            {
                return;
            }

            if (value == null)
            {
                return;
            }

            DateTime blackoutEnd;
            if (!DateTime.TryParse(value.ToString(), out blackoutEnd))
            {
                return;
            }

            BlackoutEnd = blackoutEnd;
        }

        /// <summary>
        /// 属性变化事件
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <param name="e">属性变化事件对象</param>
        private static void OnBlackoutEndPropertyValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(obj))
            {
                return;
            }

            if (e.NewValue == null)
            {
                return;
            }

            DateTime blackoutEnd;
            if (!DateTime.TryParse(e.NewValue.ToString(), out blackoutEnd))
            {
                return;
            }

            BlackoutEnd = blackoutEnd;
            cdr.End = blackoutEnd;

            DatePicker dp = obj as DatePicker;
            if (!dp.BlackoutDates.Contains(cdr))
            {
                dp.BlackoutDates.Add(cdr);
            }

            //DatePicker dp = obj as DatePicker;

            List<CalendarDateRange> blackoutDatesTemp = new List<CalendarDateRange>();
            foreach (var item in dp.BlackoutDates)
            {
                blackoutDatesTemp.Add(item);
            }

            dp.BlackoutDates.Clear();

            if (!IsIncludeedWeekend)
            {
                DateTime dtStart = BlackoutEnd;
                while (dtStart.Date < DateTime.Now.AddYears(1).Date)
                {
                    dtStart = dtStart.AddDays(1);
                    if (dtStart.DayOfWeek == DayOfWeek.Saturday)
                    {
                        CalendarDateRange cdrWeekend = new CalendarDateRange();
                        cdrWeekend.Start = dtStart;
                        if (dtStart == DateTime.MaxValue)
                        {
                            cdrWeekend.End = dtStart;
                            dp.BlackoutDates.Add(cdrWeekend);
                            break;
                        }
                        cdrWeekend.End = dtStart.AddDays(1);
                        dp.BlackoutDates.Add(cdrWeekend);
                    }
                }
            }

            foreach (var item in blackoutDatesTemp)
            {
                dp.BlackoutDates.Add(item);
            }
        }

        private static CalendarDateRange cdr = new CalendarDateRange();

        /// <summary>
        /// 是否包含周末
        /// </summary>
        private static bool IsIncludeedWeekend;

        /// <summary>
        /// 获取IsIncludeWeekend
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <returns>返回IsIncludeWeekend</returns>
        public static bool GetIsIncludeWeekend(DependencyObject obj)
        {
            return IsIncludeedWeekend;
        }

        /// <summary>
        /// 设置IsIncludeWeekend
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <param name="value">新的IsIncludeWeekend值</param>
        public static void SetIsIncludeWeekend(DependencyObject obj, object value)
        {
            if (DesignerProperties.GetIsInDesignMode(obj))
            {
                return;
            }

            if (value == null)
            {
                return;
            }

            bool isIncludeWeekend;
            if (!bool.TryParse(value.ToString(), out isIncludeWeekend))
            {
                return;
            }

            IsIncludeedWeekend = isIncludeWeekend;
        }

        /// <summary>
        /// 属性变化事件
        /// </summary>
        /// <param name="obj">依赖属性</param>
        /// <param name="e">属性变化事件对象</param>
        private static void OnIsIncludeWeekendPropertyValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(obj))
            {
                return;
            }

            if (e.NewValue == null)
            {
                return;
            }

            bool isIncludeWeekend;
            if (!bool.TryParse(e.NewValue.ToString(), out isIncludeWeekend))
            {
                return;
            }

            IsIncludeedWeekend = isIncludeWeekend;

            DatePicker dp = obj as DatePicker;

            List<CalendarDateRange> blackoutDatesTemp = new List<CalendarDateRange>();
            foreach (var item in dp.BlackoutDates)
            {
                blackoutDatesTemp.Add(item);
            }

            dp.BlackoutDates.Clear();

            if (!IsIncludeedWeekend)
            {
                DateTime dtStart = BlackoutEnd;
                while (dtStart < DateTime.MaxValue)
                {
                    dtStart = dtStart.AddDays(1);
                    if (dtStart.DayOfWeek == DayOfWeek.Saturday)
                    {
                        CalendarDateRange cdrWeekend = new CalendarDateRange();
                        cdrWeekend.Start = dtStart;
                        if (dtStart == DateTime.MaxValue)
                        {
                            cdrWeekend.End = dtStart;
                            dp.BlackoutDates.Add(cdrWeekend);
                            break;
                        }
                        cdrWeekend.End = dtStart.AddDays(1);
                        dp.BlackoutDates.Add(cdrWeekend);
                    }
                }
            }

            foreach (var item in blackoutDatesTemp)
            {
                dp.BlackoutDates.Add(item);
            }
        }

        //private static void updateBlackoutDates(DatePicker dp)
        //{
        //    List<CalendarDateRange> blackoutDatesTemp = new List<CalendarDateRange>();
        //    foreach (var item in dp.BlackoutDates)
        //    {
        //        blackoutDatesTemp.Add(item);
        //    }

        //    dp.BlackoutDates.Clear();

        //    if (!IsIncludeWeekend)
        //    {
        //        DateTime dtStart = BlackoutEnd;
        //        while (dtStart < DateTime.MaxValue)
        //        {
        //            dtStart = dtStart.AddDays(1);
        //            if (dtStart.DayOfWeek == DayOfWeek.Saturday)
        //            {
        //                CalendarDateRange cdrWeekend = new CalendarDateRange();
        //                cdrWeekend.Start = dtStart;
        //                if (dtStart == DateTime.MaxValue)
        //                {
        //                    cdrWeekend.End = dtStart;
        //                    dp.BlackoutDates.Add(cdrWeekend);
        //                    break;
        //                }
        //                cdrWeekend.End = dtStart.AddDays(1);
        //                dp.BlackoutDates.Add(cdrWeekend);
        //            }
        //        }
        //    }

        //    foreach (var item in blackoutDatesTemp)
        //    {
        //        dp.BlackoutDates.Add(item);
        //    }
        //}

        #endregion

        /// <summary>
        /// 注册路径依赖属性
        /// </summary>
        public static readonly DependencyProperty BlackoutStartProperty = DependencyProperty.RegisterAttached(
            "BlackoutStart", typeof(DateTime), typeof(DateTimeBinder), new PropertyMetadata(OnBlackoutStartPropertyValueChanged));

        /// <summary>
        /// 注册路径依赖属性
        /// </summary>
        public static readonly DependencyProperty BlackoutEndProperty = DependencyProperty.RegisterAttached(
            "BlackoutEnd", typeof(DateTime), typeof(DateTimeBinder), new PropertyMetadata(OnBlackoutEndPropertyValueChanged));

        /// <summary>
        /// 注册路径依赖属性
        /// </summary>
        public static readonly DependencyProperty IsIncludeWeekendProperty = DependencyProperty.RegisterAttached(
            "IsIncludeWeekend", typeof(bool), typeof(DateTimeBinder), new PropertyMetadata(OnIsIncludeWeekendPropertyValueChanged));

        ///// <summary>
        ///// 设置PathWithAll
        ///// </summary>
        ///// <param name="obj">依赖属性</param>
        ///// <param name="value">依赖属性value</param>
        //public static void SetPathWithAll(DependencyObject obj, string value)
        //{
        //    if (DesignerProperties.GetIsInDesignMode(obj))
        //    {
        //        return;
        //    }
        //    DatePicker dp = new DatePicker();
        //    CalendarDateRange cdr = new CalendarDateRange();
            
        //    dp.BlackoutDates.Add(cdr);
            

        //    var comboBox = obj as ComboBox;
        //    if (comboBox == null)
        //    {
        //        // throw new Exception("无法对非 'ComboBox' 的派生对象进行 'Enum' 的绑定。");
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(value))
        //    {
        //        // throw new Exception("参数 'Path' 无效。");
        //        return;
        //    }

        //    Assembly assembly = null;
        //    string typePath = null;
        //    var content = value.Split('/');
        //    if (content.Length == 1)
        //    {
        //        assembly = Assembly.GetExecutingAssembly();
        //        typePath = content[0];
        //    }
        //    else if (content.Length == 2)
        //    {
        //        try
        //        {
        //            assembly = Assembly.LoadFrom(content[0].GetFullPath());
        //        }
        //        catch
        //        {
        //            return;
        //        }

        //        typePath = content[1];
        //        if (assembly == null)
        //        {
        //            // throw new Exception(string.Format("无法加载程序集 '{0}'。", content[0]));
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        // throw new Exception("参数 'Path' 应该为 'assembly.dll/namespace.enumname'");
        //        return;
        //    }

        //    var type = assembly.GetType(typePath);
        //    if (type == null)
        //    {
        //        // throw new Exception(string.Format("无法在程序集 '{0}' 中找到类型 '{1}' 的定义。", assembly.FullName, typePath));
        //        return;
        //    }

        //    if (!type.IsSubclassOf(typeof(Enum)))
        //    {
        //        // throw new Exception(string.Format("类型 '{0}' 不是一个有效的枚举类型。", typePath));
        //        return;
        //    }

        //    var names = Enum.GetNames(type);
        //    //var list = new object[names.Length];
        //    var list = new object[names.Length + 1];
        //    list[0] = new { Display = "All", Value = -1 };
        //    for (int i = 1; i < list.Length; i++)
        //    {
        //        list[i] = new
        //        {
        //            Display = App.Current.TryFindResource(type.Name + "." + names[i - 1]),
        //            Value = Enum.Parse(type, names[i - 1]),
        //        };
        //    }

        //    comboBox.ItemsSource = list;
        //    comboBox.DisplayMemberPath = "Display";
        //    comboBox.SelectedValuePath = "Value";
        //}

        #endregion
    }
}