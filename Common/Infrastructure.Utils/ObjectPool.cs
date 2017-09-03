// <copyright file="ObjectPool.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2014/11/19 02:13:50 </date>
// <summary> 对象池 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2014/11/19 02:13:50
//      修改描述：新建 ObjectPool.cs
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
    using System.Collections.Concurrent;

    #endregion

    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T">
    /// 类型
    /// </typeparam>
    public class ObjectPool<T>
        where T : class
    {
        #region Fields

        /// <summary>
        /// 对象缓存
        /// </summary>
        private ConcurrentBag<T> buffer;

        /// <summary>
        /// 创建函数
        /// </summary>
        private Func<T> createFunc;

        /// <summary>
        /// 重置函数
        /// </summary>
        private Action<T> resetFunc;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPool{T}"/> class.
        /// </summary>
        /// <param name="createFunc">
        /// The create func.
        /// </param>
        /// <param name="resetFunc">
        /// The reset func.
        /// </param>
        /// <param name="capacity">
        /// The capacity.
        /// </param>
        public ObjectPool(Func<T> createFunc, Action<T> resetFunc, int capacity = 20)
        {
            if (createFunc == null)
            {
                throw new Exception("CreateFunc of the ObjectPool is must be valued!");
            }

            if (capacity <= 0)
            {
                throw new Exception("CreateFunc of the ObjectPool is must be valued!");
            }

            this.buffer = new ConcurrentBag<T>();
            this.createFunc = createFunc;
            this.resetFunc = resetFunc;

            this.Capacity = capacity;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the capacity.
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count
        {
            get
            {
                return this.buffer.Count;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 申请对象
        /// </summary>
        /// <returns>
        /// 申请的对象 <see cref="T"/>.
        /// </returns>
        public T GetObject()
        {
            T obj;

            if (!this.buffer.TryTake(out obj))
            {
                return this.createFunc();
            }

            return obj;
        }

        /// <summary>
        /// 获取对象池项
        /// </summary>
        /// <returns>对象池项</returns>
        public ObjectPoolItem<T> GetPoolItem()
        {
            return new ObjectPoolItem<T>(this, this.GetObject());
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        public void PutObject(T obj)
        {
            if (obj == null)
            {
                return;
            }

            if (this.Count >= this.Capacity)
            {
                // 超过容量了，不再需要
                return;
            }

            if (this.resetFunc != null)
            {
                this.resetFunc(obj);
            }

            this.buffer.Add(obj);
        }

        #endregion
    }
}