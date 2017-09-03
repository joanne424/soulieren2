//// <copyright file="OperateInstrumentOfEnum.cs" company="BancLogix">
//// Copyright (c) Banclogix. All rights reserved.
//// </copyright>
//// <author> zhanggq </author>
//// <date> 2016/05/23 09:44:24 </date>
//// <modify>
////   修改人：zhanggq
////   修改时间：2016/05/23 09:44:24
////   修改描述：新建 OperateInstrumentOfEnum
////   版本：1.0
//// </modify>
//// <review>
////   review人：
////   review时间：
//// </review >

namespace DM2.Ent.Client.ViewModels.Common
{
    using System;
    using System.Collections.ObjectModel;

    using DM2.Ent.Client.Runtime;

    /// <summary>
    /// Rport 001 用Instrument转换方法
    /// </summary>
    public class OperateInstrumentOfEnum
    {
        /// <summary>
        /// 将枚举转换为泛型类
        /// </summary>
        /// <typeparam name="T">枚举名称</typeparam>
        /// <returns>返回枚举列表</returns>
        public static ObservableCollection<InstrumentEnumKVP> EnumToObservable<T>(int l, string n)
        {
            ObservableCollection<InstrumentEnumKVP> list = new ObservableCollection<InstrumentEnumKVP>();

            foreach (int i in Enum.GetValues(typeof(T)))
            {
                string value = n + " - " + RunTime.FindStringResource(Enum.GetName(typeof(T), i));
                InstrumentEnumKVP kvp = new InstrumentEnumKVP(i, l, value);
                list.Add(kvp);
            }

            return list;
        }


        /// <summary>
        /// 将枚举转换为泛型类(DealInstrumentEnum 专用)
        /// </summary>
        /// <typeparam name="T">枚举名称</typeparam>
        /// <returns>返回枚举列表</returns>
        public static ObservableCollection<InstrumentEnumKVP> EnumToObservableForDeal<T>(int l, string n)
        {
            ObservableCollection<InstrumentEnumKVP> list = new ObservableCollection<InstrumentEnumKVP>();

            string value = string.Empty;
            foreach (int i in Enum.GetValues(typeof(T)))
            {
                if (i == 0 || i == 1 || i == 2 || i == 4)
                {
                    value = RunTime.FindStringResource(n) + " - " + RunTime.FindStringResource(Enum.GetName(typeof(T), i));
                    InstrumentEnumKVP kvp = new InstrumentEnumKVP(i, l, value);
                    list.Add(kvp);                  
                }
            }

            return list;
        }

        /// <summary>
        /// 将枚举转换为泛型类
        /// </summary>
        /// <typeparam name="T">枚举名称</typeparam>
        /// <returns>返回枚举列表</returns>
        public static ObservableCollection<InstrumentEnumKVP> EnumToObservableForOrder<T>(int l, string n)
        {
            ObservableCollection<InstrumentEnumKVP> list = new ObservableCollection<InstrumentEnumKVP>();
            foreach (int i in Enum.GetValues(typeof(T)))
            {
                if (i == 10 || i == 11)
                {
                    string value = n + " - " + RunTime.FindStringResource(Enum.GetName(typeof(T), i));
                    InstrumentEnumKVP kvp = new InstrumentEnumKVP(i, l, value);
                    list.Add(kvp);
                }
            }

            return list;
        }

        /// <summary>
        /// 将枚举转换为泛型类
        /// </summary>
        /// <typeparam name="T">枚举名称</typeparam>
        /// <returns>返回枚举列表</returns>
        public static ObservableCollection<InstrumentEnumKVP> EnumToObservableForDeposit<T>(int l, string n)
        {
            ObservableCollection<InstrumentEnumKVP> list = new ObservableCollection<InstrumentEnumKVP>();
            foreach (int i in Enum.GetValues(typeof(T)))
            {
                if (i == 21)
                {
                    string value = n + " - " + RunTime.FindStringResource(Enum.GetName(typeof(T), i));
                    InstrumentEnumKVP kvp = new InstrumentEnumKVP(i, l, value);
                    list.Add(kvp);
                }
            }

            return list;
        }

        /// <summary>
        /// 将枚举转换为泛型类
        /// </summary>
        /// <typeparam name="T">枚举名称</typeparam>
        /// <returns>返回枚举列表</returns>
        public static ObservableCollection<InstrumentEnumKVP> EnumToObservableForWithdrawal<T>(int l, string n)
        {
            ObservableCollection<InstrumentEnumKVP> list = new ObservableCollection<InstrumentEnumKVP>();
            foreach (int i in Enum.GetValues(typeof(T)))
            {
                if (i == 20)
                {
                    string value = n + " - " + RunTime.FindStringResource(Enum.GetName(typeof(T), i));
                    InstrumentEnumKVP kvp = new InstrumentEnumKVP(i, l, value);
                    list.Add(kvp);
                }
            }

            return list;
        }

        /// <summary>
        /// 将枚举转换为泛型类
        /// </summary>
        /// <typeparam name="T">枚举名称</typeparam>
        /// <returns>返回枚举列表</returns>
        public static ObservableCollection<InstrumentEnumKVP> EnumToObservableForAdHocFee<T>(int l, string n)
        {
            ObservableCollection<InstrumentEnumKVP> list = new ObservableCollection<InstrumentEnumKVP>();
            foreach (int i in Enum.GetValues(typeof(T)))
            {
                if (i == 30 || i == 31 || i == 32 || i == 33 || i == 34 || i == 35)
                {
                    string value = n + " - " + RunTime.FindStringResource(Enum.GetName(typeof(T), i));
                    InstrumentEnumKVP kvp = new InstrumentEnumKVP(i, l, value);
                    list.Add(kvp);
                }
            }

            return list;
        }
    }
}
