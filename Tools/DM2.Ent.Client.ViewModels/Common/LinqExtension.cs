// <copyright file="LinqExtension.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>one</author>
// <date> 2015/7/7 </date>
// <summary></summary>
// <modify>
//      修改人：one
//      修改时间：2014/10/30 17:46:11
//      修改描述：新建 EnumerableExtensionMethods
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>

namespace DM2.Ent.Client.ViewModels.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;

    using DM2.Ent.Presentation.Models.Base;

    /// <summary>
    /// The enumerable extension methods.
    /// </summary>
    public static class LinqExtension
    {
        /// <summary>
        /// ForEach
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="source">source</param>
        /// <param name="action">action</param>
        /// <returns>ObservableCollection</returns>
        public static ObservableCollection<T> ForEach<T>(this ObservableCollection<T> source, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            foreach (T item in source)
            {
                action(item);
            }

            return source;
        }

        /// <summary>
        /// ForEach
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="source">source</param>
        /// <param name="action">action</param>
        /// <returns> ForEachT</returns>
        public static T[] ForEach<T>(this T[] source, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            foreach (T item in source)
            {
                action(item);
            }

            return source;
        }

        /// <summary>
        /// Replace
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="sequence">sequence</param>
        /// <param name="find">find</param>
        /// <param name="replaceWith">replaceWith</param>
        /// <param name="comparer">comparer</param>
        /// <returns>IEnumerable</returns>
        public static IEnumerable<T> Replace<T>(this IEnumerable<T> sequence, T find, T replaceWith, IEqualityComparer<T> comparer)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            return ReplaceImpl(sequence, find, replaceWith, comparer);
        }

        /// <summary>
        /// Replaces the specified sequence.
        /// </summary>
        /// <typeparam name="T">T
        /// </typeparam>
        /// <param name="sequence">
        /// The sequence.
        /// </param>
        /// <param name="find">
        /// The find.
        /// </param>
        /// <param name="replaceWith">
        /// The replace with.
        /// </param>
        /// <returns>
        /// Collection of type T
        /// </returns>
        public static IEnumerable<T> Replace<T>(this IEnumerable<T> sequence, T find, T replaceWith)
        {
            return Replace(sequence, find, replaceWith, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// ToObservableCollection
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="ienumerable">iEnumerable</param>
        /// <returns>ObservableCollection</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> ienumerable)
        {
            var ob = new ObservableCollection<T>();
            foreach (T item in ienumerable)
            {
                ob.Add(item);
            }

            return ob;
        }

        /// <summary>
        /// ToObservableCollection
        /// </summary>
        /// <typeparam name="T">
        /// T
        /// </typeparam>
        /// <param name="ienumerable">
        /// iEnumerable
        /// </param>
        /// <param name="isAll">
        /// 是否添加查询所有项，默认为不加all
        /// </param>
        /// <param name="isAsc">
        /// 显示列表排序方式，true = asc ，false = desc ，不传为asc
        /// </param>
        /// <returns>
        /// ObservableCollection
        /// </returns>
        public static ObservableCollection<T> ToComboboxBinding<T>(this IEnumerable<T> ienumerable, bool isAll = false, bool isAsc = true) where T : BaseVm, new()
        {
            var ob = new ObservableCollection<T>();
            var orderList = isAsc ? ienumerable.ToArray().OrderBy(o => o._Name) : ienumerable.ToArray().OrderByDescending(o => o._Name);
            foreach (T item in orderList)
            {
                ob.Add(item);
            }

            if (isAll)
            {
                ob.Insert(0, new T() { _Name = string.Empty });
            }

            return ob;
        }

        /// <summary>
        /// The to data table.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <typeparam name="T">T
        /// </typeparam>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            var dt = new DataTable();
            PropertyDescriptorCollection ps = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor dp in ps)
            {
                dt.Columns.Add(dp.Name, dp.PropertyType);
            }

            foreach (T t in data)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyDescriptor dp in ps)
                {
                    dr[dp.Name] = dp.GetValue(t);
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// The to data table.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <typeparam name="T">T
        /// </typeparam>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        public static DataTable ToDataTable<T>(this ObservableCollection<T> data)
        {
            var dt = new DataTable();
            PropertyDescriptorCollection ps = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor dp in ps)
            {
                dt.Columns.Add(dp.Name, dp.PropertyType);
            }

            foreach (T t in data)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyDescriptor dp in ps)
                {
                    dr[dp.Name] = dp.GetValue(t);
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// Replaces the impl.
        /// </summary>
        /// <typeparam name="T">T
        /// </typeparam>
        /// <param name="sequence">
        /// The sequence.
        /// </param>
        /// <param name="find">
        /// The find.
        /// </param>
        /// <param name="replaceWith">
        /// The replace with parameter
        /// </param>
        /// <param name="comparer">
        /// The comparer.
        /// </param>
        /// <returns>
        /// Collection of type T
        /// </returns>
        private static IEnumerable<T> ReplaceImpl<T>(IEnumerable<T> sequence, T find, T replaceWith, IEqualityComparer<T> comparer)
        {
            foreach (T item in sequence)
            {
                bool match = comparer.Equals(find, item);
                T x = match ? replaceWith : item;
                yield return x;
            }
        }
    }
}