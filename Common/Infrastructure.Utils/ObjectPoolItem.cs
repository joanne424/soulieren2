// <copyright file="ObjectPoolItem.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2014/11/19 03:25:38 </date>
// <summary> 对象池项 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2014/11/19 03:25:38
//      修改描述：新建 ObjectPoolItem.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace Infrastructure
{
    #region

    using System;

    #endregion

    /// <summary>
    /// 对象池项
    /// </summary>
    /// <typeparam name="T">
    /// 管理对象的回收与释放
    /// </typeparam>
    public class ObjectPoolItem<T> : IDisposable
        where T : class
    {
        #region Fields

        /// <summary>
        /// 父对象池
        /// </summary>
        private ObjectPool<T> fatherPool;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPoolItem{T}"/> class.
        /// </summary>
        /// <param name="father">
        /// The father.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public ObjectPoolItem(ObjectPool<T> father, T value)
        {
            this.fatherPool = father;
            this.Value = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 值
        /// </summary>
        public T Value { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The close.
        /// </summary>
        public void Close()
        {
            if (this.fatherPool == null)
            {
                return;
            }

            this.fatherPool.PutObject(this.Value);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.fatherPool.PutObject(this.Value);
        }

        #endregion
    }
}