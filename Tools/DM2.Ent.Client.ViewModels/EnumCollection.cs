using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// <copyright file="EnumCollection.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>donggj</author>
// <date> 2015/7/6 18:35:30 </date>
// <summary></summary>
// <modify>
//      修改人：guanxb
//      修改时间：2015/7/6 18:35:30
//      修改描述：新建 *
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
namespace DM2.Manager.ViewModels
{
    ///// <summary>
    ///// 存放下拉框可选项集合
    ///// </summary>
    public class EnumCollection : BaseVm
    {
        ///// <summary>
        ///// BusinessUnit对应的集合 字段
        ///// </summary>
        private Dictionary<string, string> buDicList = new Dictionary<string, string>();

        ///// <summary>
        ///// BusinessUnit对应的集合 属性
        ///// </summary>
        public Dictionary<string, string> BuDicList
        {
            get
            {
                return this.buDicList;
            }

            set
            {
                this.buDicList = value;
                this.NotifyOfPropertyChange("BuDicList");
            }
        }

        ///// <summary>
        ///// CustomerGrading对应的集合 字段
        ///// </summary>
        private Dictionary<string, string> custGradingDicList = new Dictionary<string, string>();

        ///// <summary>
        ///// CustomerGrading对应的集合 字段
        ///// </summary>
        public Dictionary<string, string> CustGradingDicList
        {
            get
            {
                return this.custGradingDicList;
            }

            set
            {
                this.custGradingDicList = value;
                this.NotifyOfPropertyChange("CustGradingDicList");
            }
        }

        ///// <summary>
        ///// CustomerGroup对应的集合 字段
        ///// </summary>
        private Dictionary<string, string> custGroupDicList = new Dictionary<string, string>();

        ///// <summary>
        ///// CustomerGroup对应的集合 字段
        ///// </summary>
        public Dictionary<string, string> CustGroupDicList
        {
            get
            {
                return this.custGroupDicList;
            }

            set
            {
                this.custGroupDicList = value;
                this.NotifyOfPropertyChange("CustGroupDicList");
            }
        }

        ///// <summary>
        ///// QuoteGroup对应的集合 字段
        ///// </summary>
        private Dictionary<string, string> quoteGroupDicList = new Dictionary<string, string>();

        ///// <summary>
        ///// QuoteGroup对应的集合 属性
        ///// </summary>
        public Dictionary<string, string> QuoteGroupDicList
        {
            get
            {
                return this.quoteGroupDicList;
            }

            set
            {
                this.quoteGroupDicList = value;
                this.NotifyOfPropertyChange("QuoteGroupDicList");
            }
        }

        ///// QuoteGroup对应的集合 字段
        ///// </summary>
        private Dictionary<string, string> custCategoryDicList = new Dictionary<string, string>();

        ///// <summary>
        ///// 客户类型对应的集合 属性
        ///// </summary>
        public Dictionary<string, string> CustCategoryDicList
        {
            get
            {
                return this.custCategoryDicList;
            }

            set
            {
                this.custCategoryDicList = value;
                this.NotifyOfPropertyChange("CustCategoryDicList");
            }
        }

        ///// 国家对应的集合 字段
        ///// </summary>
        private Dictionary<string, string> countryDicList = new Dictionary<string, string>();

        ///// <summary>
        ///// 国家对应的集合 属性
        ///// </summary>
        public Dictionary<string, string> CountryDicList
        {
            get
            {
                return this.countryDicList;
            }

            set
            {
                this.countryDicList = value;
                this.NotifyOfPropertyChange("CountryDicList");
            }
        }

        ////IQuoteGroupCacheRepository
        ///// <summary>
        ///// Initializes a new instance of the EnumCollection class.
        ///// </summary>
        public EnumCollection()
            : base(null)
        {
            this.InitalBUDicList();
            this.InitalCustGrading();
            this.InitalCustGroup();
            this.InitalQuoteGroup();
            this.InitalCustCategory();
            this.InitalCountry();
        }

        ///// <summary>
        ///// Initializes a new instance of the EnumCollection class.
        ///// </summary>
        ///// <param name="varOwnerId">拥有者ID</param>
        public EnumCollection(string varOwnerId = null)
            : base(varOwnerId)
        {
        }

        ///// <summary>
        ///// 初始化BU可选项
        ///// </summary>
        private void InitalBUDicList()
        {
            //////// 初始化BU可选项
            ////var buList = this.GetRepository<IBusinessUnitCacheRepository>().GetBindCollection();

            ////if (buList != null)
            ////{
            ////    foreach (var forPointItem in buList)
            ////    {
            ////        this.buDicList.Add(forPointItem.BusinessUnitName, forPointItem.BusinessUnitID);
            ////    }
            ////}

            this.buDicList.Add("zvzv", "111");
            this.buDicList.Add("zvghjzv", "222");
            this.buDicList.Add("sdfas", "333");
            this.buDicList.Add("tgtbt", "444");

        }

        ///// <summary>
        ///// 初始化 客户等级
        ///// </summary>
        private void InitalCustGrading()
        {
            //////// 初始化BU可选项
            ////var custGradeList = this.GetRepository<ICustGradingCacheRepository>().GetBindCollection();

            ////if (custGradeList != null)
            ////{
            ////    foreach (var forPointItem in custGradeList)
            ////    {
            ////        this.custGradingDicList.Add(forPointItem.GradingID, forPointItem.GradingName);
            ////    }
            ////}

            this.custGradingDicList.Add("zvzv", "111");
            this.custGradingDicList.Add("zvghjzv", "222");
            this.custGradingDicList.Add("sdfas", "333");
            this.custGradingDicList.Add("tgtbt", "444");
        }

        ///// <summary>
        ///// 初始化 客户账户组
        ///// </summary>
        private void InitalCustGroup()
        {
            //////// 初始化BU可选项
            ////var custGropList = this.GetRepository<ICustomerGroupCacheRepository>().GetBindCollection();

            ////if (custGropList != null)
            ////{
            ////    foreach (var forPointItem in custGropList)
            ////    {
            ////        this.custGroupDicList.Add(forPointItem.CustGroupID, forPointItem.CustGroupName);
            ////    }
            ////}

            this.custGroupDicList.Add("zvzv", "111");
            this.custGroupDicList.Add("zvghjzv", "222");
            this.custGroupDicList.Add("sdfas", "333");
            this.custGroupDicList.Add("tgtbt", "444");
        }

        ///// <summary>
        ///// 初始化 报价组合下拉框Item
        ///// </summary>
        private void InitalQuoteGroup()
        {
            ////var quoteGropList = this.GetRepository<IQuoteGroupCacheRepository>().GetBindCollection();

            ////if (quoteGropList != null)
            ////{
            ////    foreach (var forPointItem in quoteGropList)
            ////    {
            ////        this.custGroupDicList.Add(forPointItem.GetID(), forPointItem.GetName());
            ////    }
            ////}

            this.quoteGroupDicList.Add("zvzv", "111");
            this.quoteGroupDicList.Add("zvghjzv", "222");
            this.quoteGroupDicList.Add("sdfas", "333");
            this.quoteGroupDicList.Add("tgtbt", "444");
        }

        ///// <summary>
        ///// 初始化 账户类型（Category）下拉框Item
        ///// </summary>
        private void InitalCustCategory()
        {
            ////var categoryList = this.GetRepository<IQuoteGroupCacheRepository>().GetBindCollection();

            ////if (quoteGropList != null)
            ////{
            ////    foreach (var forPointItem in quoteGropList)
            ////    {
            ////        this.custGroupDicList.Add(forPointItem.GetID(), forPointItem.GetName());
            ////    }
            ////}

            this.custCategoryDicList.Add("zvzv", "111");
            this.custCategoryDicList.Add("zvghjzv", "222");
            this.custCategoryDicList.Add("sdfas", "333");
            this.custCategoryDicList.Add("tgtbt", "444");
        }

        ///// <summary>
        ///// 初始化 国家下拉框Item
        ///// </summary>
        private void InitalCountry()
        {
            ////var categoryList = this.GetRepository<IQuoteGroupCacheRepository>().GetBindCollection();

            ////if (quoteGropList != null)
            ////{
            ////    foreach (var forPointItem in quoteGropList)
            ////    {
            ////        this.custGroupDicList.Add(forPointItem.GetID(), forPointItem.GetName());
            ////    }
            ////}

            this.countryDicList.Add("zvzv", "111");
            this.countryDicList.Add("zvghjzv", "222");
            this.countryDicList.Add("sdfas", "333");
            this.countryDicList.Add("tgtbt", "444");
        }
    }
}
