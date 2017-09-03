// <copyright file="IDBRepository.cs" company="BancLogix"> 
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2013/9/22 10:39:28 </date>
// <summary> 缓存仓储接口 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2013/9/22 10:39:28
//      修改描述：新建 IDomainRepository.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>

using System;
using System.Collections.Generic;
namespace Infrastracture.Data
{
    /// <summary>
    /// 缓存仓储接口定义
    /// </summary>
    /// <typeparam name="TVMbase">VM对象</typeparam>
    public interface IDBRepository<TVMbase> where TVMbase : class
    {
        /// <summary>
        /// 向缓存中添加或者更新项
        /// </summary>
        /// <param name="item">要添加的项</param>
        /// <returns>是否添加成功</returns>
        bool Add(TVMbase item);

        /// <summary>
        /// 向缓存中添加或者更新项
        /// </summary>
        /// <param name="item">要添加的项</param>
        /// <returns>是否添加成功</returns>
        bool Update(TVMbase item);

        /// <summary>
        /// 删除缓存中的项
        /// </summary>
        /// <param name="id">项ID</param>
        /// <returns>是否删除成功</returns>
        bool Remove(string id);

        /// <summary>
        /// 获取能绑定的缓存集合
        /// </summary>
        /// <returns>返回能绑定的缓存集合</returns>
        List<TVMbase> Roots();
        
        /// <summary>
        /// 获取ID对应项
        /// </summary>
        /// <param name="id">Id 与实现GetHashCode的字符串相同</param>
        /// <returns>缓存对象</returns>
        TVMbase FindByID(string id);

        /// <summary>
        /// 判断是否有复合查询条件的结果存在。
        /// </summary>
        /// <param name="filter"> 谓词 </param>
        /// <returns> 返回检索到的对象的集合 </returns>
        bool Any(Func<TVMbase, bool> filter);
    }
}
