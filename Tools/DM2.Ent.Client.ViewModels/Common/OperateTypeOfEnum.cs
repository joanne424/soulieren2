//// <copyright file="OperateTypeOfEnum.cs" company="BancLogix">
//// Copyright (c) Banclogix. All rights reserved.
//// </copyright>
//// <author> zhanggq </author>
//// <date> 2016/05/12 10:49:24 </date>
//// <modify>
////   修改人：zhanggq
////   修改时间：2016/05/12 10:49:24
////   修改描述：新建 OperateTypeOfEnum
////   版本：1.0
//// </modify>
//// <review>
////   review人：
////   review时间：
//// </review >

namespace DM2.Ent.Client.ViewModels.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using DM2.Ent.Client.Runtime;

    /// <summary>
    /// 枚举转换
    /// </summary>
    public class OperateTypeOfEnum
    {
        /// <summary>
        /// 将枚举类型转换为Hashtable
        /// </summary>
        /// <typeparam name="T">枚举名称</typeparam>
        /// <returns>返回Hashtable</returns>
        public static Hashtable EnumToHashtable<T>()
        {
            Hashtable table = new Hashtable();
            foreach (int i in Enum.GetValues(typeof(T)))
            {
                table.Add(i.ToString(), Enum.GetName(typeof(T), i));
            }

            return table;
        }

        /// <summary>
        /// 将枚举转换为泛型类
        /// </summary>
        /// <typeparam name="T">枚举名称</typeparam>
        /// <returns>返回枚举列表</returns>
        public static List<EnumKVP> EnumToList<T>()
        {
            List<EnumKVP> list = new List<EnumKVP>();
            foreach (int i in Enum.GetValues(typeof(T)))
            {
                EnumKVP kvp = new EnumKVP(i, Enum.GetName(typeof(T), i));
                list.Add(kvp);
            }

            return list;
        }

        /// <summary>
        /// 将枚举转换为泛型类
        /// </summary>
        /// <typeparam name="T">枚举名称</typeparam>
        /// <returns>返回枚举列表</returns>
        public static ObservableCollection<EnumKVP> EnumToObservable<T>()
        {
            ObservableCollection<EnumKVP> list = new ObservableCollection<EnumKVP>();
            foreach (int i in Enum.GetValues(typeof(T)))
            {
                string value = Enum.GetName(typeof(T), i);
                EnumKVP kvp = new EnumKVP(i, value);
                list.Add(kvp);
            }

            return list;
        }

        /// <summary>
        /// 将枚举转换为泛型类
        /// </summary>
        /// <typeparam name="T">枚举名称</typeparam>
        /// <returns>返回枚举列表</returns>
        public static ObservableCollection<EnumKVP> EnumToObservableFormat<T>()
        {
            ObservableCollection<EnumKVP> list = new ObservableCollection<EnumKVP>();
            foreach (int i in Enum.GetValues(typeof(T)))
            {
                string value = RunTime.FindStringResource(Enum.GetName(typeof(T), i));
                EnumKVP kvp = new EnumKVP(i, value);
                list.Add(kvp);
            }

            return list;
        }       

        ///// <summary>
        ///// 通过枚举的值获取对应的枚举变量
        ///// </summary>
        ///// <typeparam name="T">枚举名称</typeparam>
        ///// <param name="key">枚举值</param>
        ///// <returns>返回枚举变量</returns>
        //public string GetEnumVariable<T>(int key)
        //{
        //    Hashtable table = new Hashtable();
        //    foreach (int i in Enum.GetValues(typeof(T), i))
        //    {
        //        table.Add(i, Enum.GetName(typeof(T), i));
        //    }

        //    return table[key].ToString();
        //}

        ///// <summary>
        ///// 通过枚举的值获取对应的枚举变量
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public IEnumerable GetEnumVariable<T>(int key)
        //{
        //    foreach (int i in Enum.GetValues(typeof(T)))
        //    {
        //        if (i == key)
        //        {
        //            return Enum.GetName(typeof(T), i);
        //            break;
        //        }
        //    }
        //}

    }
}
