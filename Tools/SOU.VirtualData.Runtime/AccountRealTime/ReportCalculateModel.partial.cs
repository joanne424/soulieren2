// <copyright file="ReportCalculateModel.partial.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2015/12/11 05:50:43 </date>
// <summary> 报表计算模型 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2015/12/11 05:50:43
//      修改描述：新建 ReportCalculateModel.partial.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace DM2.Manager.Models
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    using BaseViewModel;

    using Infrastructure.Common;
    using Infrastructure.Common.Enums;
    using Infrastructure.Data;
    using Infrastructure.Data.Tools;
    using Infrastructure.Log;
    using Infrastructure.Models;

    #endregion

    /// <summary>
    ///     报表的计算模型
    /// </summary>
    public partial class ReportCalculateModel
    {
        #region Fields

        /// <summary>
        ///     业务区信息字典
        /// </summary>
        private readonly Dictionary<string, BusinessUnitInfo> businessUnitInfoDic =
            new Dictionary<string, BusinessUnitInfo>();

        #endregion

        #region Methods

        /// <summary>
        /// 构建新的Bu的信息
        /// </summary>
        /// <param name="targetBussinessUnit">
        /// 目标Bu
        /// </param>
        private void BuildNewBusinessUnitInfo(BaseBusinessUnitVM targetBussinessUnit)
        {
            var newBuInfo = new BusinessUnitInfo(targetBussinessUnit, this);
            this.businessUnitInfoDic.Add(targetBussinessUnit.GetID(), newBuInfo);
        }

        /// <summary>
        /// 根据Buid获取其对应的Buinfo信息集
        ///     如果尚不存在，但是buid存在对应的bu，则新建
        /// </summary>
        /// <param name="businessUnitId">
        /// 业务区Id
        /// </param>
        /// <returns>
        /// The <see cref="BusinessUnitInfo"/>.
        /// </returns>
        private BusinessUnitInfo GetBusinessUnitInfo(string businessUnitId)
        {
            if (this.businessUnitInfoDic.ContainsKey(businessUnitId))
            {
                return this.businessUnitInfoDic[businessUnitId];
            }

            BaseBusinessUnitVM targetBussinessUnit = this.InnerBusinessUnitCacheRepository.GetByID(businessUnitId);
            if (targetBussinessUnit == null)
            {
                TraceManager.Error.Write("ReportCalculate", "尝试获取Bu信息集时，找不到id对应的Bu，BussinessUnitId：{0}", businessUnitId);
                return null;
            }

            // 在业务处理线程内进行再次尝试获取或者新建
            lock (this.consumerSnycFlag)
            {
                if (this.businessUnitInfoDic.ContainsKey(businessUnitId))
                {
                    return this.businessUnitInfoDic[businessUnitId];
                }

                this.BuildNewBusinessUnitInfo(targetBussinessUnit);
            }

            return this.businessUnitInfoDic[businessUnitId];
        }

        #endregion

        /// <summary>
        ///     业务区信息
        ///     存储业务区的快照信息列表
        /// </summary>
        private class BusinessUnitInfo
        {
            #region Fields

            /// <summary>
            ///     业务区相关快照的字典存储
            ///     用于快照的快速查找定位
            /// </summary>
            private readonly Dictionary<string, BaseDealingBookSnapshotInfoVM> bussinessUnitSnapshotDic =
                new Dictionary<string, BaseDealingBookSnapshotInfoVM>();

            /// <summary>
            ///     业务区相关快照列表
            /// </summary>
            private readonly ObservableCollection<BaseDealingBookSnapshotInfoVM> bussinessUnitSnapshotList =
                new ObservableCollection<BaseDealingBookSnapshotInfoVM>();

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="BusinessUnitInfo"/> class.
            /// </summary>
            /// <param name="targetbu">
            /// 目标业务区
            /// </param>
            /// <param name="varBelowReportCalculateModel">
            /// 所属计算模型
            /// </param>
            public BusinessUnitInfo(BaseBusinessUnitVM targetbu, ReportCalculateModel varBelowReportCalculateModel)
            {
                this.BelowReportCalculateModel = varBelowReportCalculateModel;
                this.BusinessUnitVm = targetbu;
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     所属报表计算模型
            /// </summary>
            public ReportCalculateModel BelowReportCalculateModel { get; private set; }

            /// <summary>
            ///     当前未关闭的快照信息
            /// </summary>
            public SnapshotInfo CurrentSnapshotInfo { get; private set; }

            #endregion

            #region Properties

            /// <summary>
            ///     Bu的VM
            /// </summary>
            private BaseBusinessUnitVM BusinessUnitVm { get; set; }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// 添加或者更新当前的Snapshot
            /// </summary>
            /// <param name="snapshot">
            /// 新值
            /// </param>
            public void AddOrUpdate(BaseDealingBookSnapshotInfoVM snapshot)
            {
                if (this.bussinessUnitSnapshotDic.ContainsKey(snapshot.GetID()))
                {
                    this.bussinessUnitSnapshotDic[snapshot.GetID()].Copy(snapshot);
                    if (this.CurrentSnapshotInfo != null && snapshot.SnapshotId == this.CurrentSnapshotInfo.SnapshotId)
                    {
                        this.CurrentSnapshotInfo = null;
                    }
                }
                else
                {
                    if (snapshot.IsNewestSnapshot)
                    {
                        if (this.CurrentSnapshotInfo == null
                            || this.CurrentSnapshotInfo.SnapshotId != snapshot.SnapshotId)
                        {
                            this.InitialCurrentSnapshotInfo(snapshot);
                        }
                    }

                    this.bussinessUnitSnapshotDic.Add(snapshot.GetID(), snapshot);

                    // 找到快照应该插入的位置，按快照时间排序
                    BaseDealingBookSnapshotInfoVM item =
                        this.bussinessUnitSnapshotList.Where(o => o.SnapshotTimeFrom > snapshot.SnapshotTimeFrom)
                            .OrderBy(p => p.SnapshotTimeFrom)
                            .FirstOrDefault();
                    int index = 0;
                    if (item != null)
                    {
                        index = this.bussinessUnitSnapshotList.IndexOf(item) + 1;
                    }

                    Application.Current.Dispatcher.Invoke(
                        new Action(() => this.bussinessUnitSnapshotList.Insert(index, snapshot)), 
                        null);
                }
            }

            /// <summary>
            ///     获取Bu下Snapshot的可绑定列表
            /// </summary>
            /// <returns>可绑定列表</returns>
            public ObservableCollection<BaseDealingBookSnapshotInfoVM> GetSnapshotBindList()
            {
                return this.bussinessUnitSnapshotList;
            }

            /// <summary>
            /// 获取指定的Snapshort
            /// </summary>
            /// <param name="snapshotId">
            /// 指定Id
            /// </param>
            /// <returns>
            /// 目标Snapshot
            /// </returns>
            public BaseDealingBookSnapshotInfoVM GetSnapshotInfo(string snapshotId)
            {
                if (this.bussinessUnitSnapshotDic.ContainsKey(snapshotId))
                {
                    return this.bussinessUnitSnapshotDic[snapshotId];
                }

                return null;
            }

            /// <summary>
            /// 初始化当前未完成快照
            /// </summary>
            /// <param name="snapshotItme">
            /// 快照信息
            /// </param>
            public void InitialCurrentSnapshotInfo(BaseDealingBookSnapshotInfoVM snapshotItme)
            {
                var snapinfo = new SnapshotInfo(this)
                                   {
                                       SnapshotId = snapshotItme.SnapshotId, 
                                       LocalCcy =
                                           this.BelowReportCalculateModel.InnerCurrencyCacheRepository
                                           .FindByID(this.BusinessUnitVm.LocalCCYID)
                                   };
                snapinfo.DealingBookPlDetailShowCcy = snapinfo.LocalCcy;
                snapinfo.DealingBookShowCcy = snapinfo.LocalCcy;
                this.CurrentSnapshotInfo = snapinfo;
            }

            #endregion
        }

        /// <summary>
        ///     未关闭活跃快照信息
        /// </summary>
        private class SnapshotInfo
        {
            #region Constants

            /// <summary>
            ///     Cash/CNY DealingBook所有数字对应的小数位
            /// </summary>
            private const int DealingBookDecimalPlace = 2;

            #endregion

            #region Fields

            /// <summary>
            ///     隶属Bu信息集
            /// </summary>
            private readonly BusinessUnitInfo belowBusinessUnitInfo;

            /// <summary>
            ///     快照的DealingBook字典
            /// </summary>
            private readonly Dictionary<string, BaseDealingBookItemVM> sanpshortDealingBookDic =
                new Dictionary<string, BaseDealingBookItemVM>();

            /// <summary>
            ///     快照的DealingBook列表
            /// </summary>
            private readonly ObservableCollection<BaseDealingBookItemVM> sanpshortDealingBookList =
                new ObservableCollection<BaseDealingBookItemVM>();

            /// <summary>
            ///     快照的PlDetail字典
            /// </summary>
            private readonly Dictionary<string, BaseDealingBookPlDetailItemVM> sanpshortPlDetailDic =
                new Dictionary<string, BaseDealingBookPlDetailItemVM>();

            /// <summary>
            ///     快照的PlDetail列表
            /// </summary>
            private readonly ObservableCollection<BaseDealingBookPlDetailItemVM> sanpshortPlDetailList =
                new ObservableCollection<BaseDealingBookPlDetailItemVM>();

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="SnapshotInfo"/> class.
            /// </summary>
            /// <param name="varBelowBuinfo">
            /// The var below buinfo.
            /// </param>
            public SnapshotInfo(BusinessUnitInfo varBelowBuinfo)
            {
                this.belowBusinessUnitInfo = varBelowBuinfo;
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     DealingBookPlDetail对应的转换显示货币
            /// </summary>
            public BaseCurrencyVM DealingBookPlDetailShowCcy { private get; set; }

            /// <summary>
            ///     DealingBook对应的转换显示货币
            /// </summary>
            public BaseCurrencyVM DealingBookShowCcy { private get; set; }

            /// <summary>
            ///     本币
            /// </summary>
            public BaseCurrencyVM LocalCcy { get; set; }

            /// <summary>
            ///     快照Id
            /// </summary>
            public string SnapshotId { get; set; }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// 添加或者更新当前的DealingBook
            /// </summary>
            /// <param name="dealingbook">
            /// 新值
            /// </param>
            public void AddOrUpdate(BaseDealingBookItemVM dealingbook)
            {
                if (this.sanpshortDealingBookDic.ContainsKey(dealingbook.CCYId))
                {
                    this.sanpshortDealingBookDic[dealingbook.CCYId].Copy(dealingbook);
                }
                else
                {
                    this.sanpshortDealingBookDic.Add(dealingbook.CCYId, dealingbook);
                    Application.Current.Dispatcher.Invoke(
                        new Action(
                            () =>
                                {
                                    this.sanpshortDealingBookList.Add(dealingbook);
                                    List<BaseDealingBookItemVM> tempOrderedList =
                                        this.sanpshortDealingBookList.OrderBy(item => item.CurrencyName).ToList();
                                    BaseDealingBookItemVM total =
                                        this.sanpshortDealingBookList.FirstOrDefault(
                                            item =>
                                            item.CCYId == this.belowBusinessUnitInfo.BelowReportCalculateModel.TotalTile);
                                    this.sanpshortDealingBookList.Clear();
                                    tempOrderedList.ForEach(item => this.sanpshortDealingBookList.Add(item));
                                    if (total != null)
                                    {
                                        this.sanpshortDealingBookList.Remove(total);
                                        this.sanpshortDealingBookList.Add(total);
                                    }
                                }), 
                        null);
                }
            }

            /// <summary>
            /// 添加或者更新当前的PlDetail
            /// </summary>
            /// <param name="pldetail">
            /// 新值
            /// </param>
            public void AddOrUpdate(BaseDealingBookPlDetailItemVM pldetail)
            {
                if (this.sanpshortPlDetailDic.ContainsKey(pldetail.GetID()))
                {
                    this.sanpshortPlDetailDic[pldetail.GetID()].Copy(pldetail);
                }
                else
                {
                    this.sanpshortPlDetailDic.Add(pldetail.GetID(), pldetail);
                    Application.Current.Dispatcher.Invoke(
                        new Action(() => this.sanpshortPlDetailList.Insert(0, pldetail)), 
                        null);
                    this.StatisticPlDetialInDealingBookItem(pldetail);
                }
            }

            /// <summary>
            ///     获取Snapshot下DealingBook的可绑定列表
            /// </summary>
            /// <returns>可绑定列表</returns>
            public ObservableCollection<BaseDealingBookItemVM> GetDealingBookBindList()
            {
                return this.sanpshortDealingBookList;
            }

            /// <summary>
            ///     获取Snapshot下PlDetail的可绑定列表
            /// </summary>
            /// <returns>可绑定列表</returns>
            public ObservableCollection<BaseDealingBookPlDetailItemVM> GetPlDetailBindList()
            {
                return this.sanpshortPlDetailList;
            }

            /// <summary>
            ///     进行重算
            /// </summary>
            public void Recalculate()
            {
                try
                {
                    TransPrice dealingBookPldetailChange;
                    if (this.DealingBookPlDetailShowCcy == null)
                    {
                        dealingBookPldetailChange = new TransPrice { PriceDirection = EnumDirection.Equals };
                    }
                    else
                    {
                        dealingBookPldetailChange = PriceCore.Instance.GetTransitionPrice(
                            this.LocalCcy.CurrencyID, 
                            this.DealingBookPlDetailShowCcy.CurrencyID);
                    }

                    // 清空dealingbook上的GrossPlXXX
                    foreach (BaseDealingBookItemVM value in this.sanpshortDealingBookDic.Values)
                    {
                        value.Model.GrossPlXXX = decimal.Zero;
                        value.Model.MonthlyGrossPlXXX = decimal.Zero;
                        value.Model.TodayNetPositionXXX = decimal.Zero;
                        value.Model.TotalNetPositionXXX = decimal.Zero;
                    }

                    foreach (BaseDealingBookPlDetailItemVM pldetail in this.sanpshortPlDetailDic.Values)
                    {
                        BaseForwardRate forwardRate;

                        // 计算当前的tener
                        TenorEnum tenorNow;
                        DateTime expiretime;
                        if (
                            !ValueDateCore.Instance.GetTenorOrNextTenorByValueDateForCalculate(
                                pldetail.BusinessUnitId, 
                                pldetail.ValueDate, 
                                pldetail.Symbol, 
                                out tenorNow, 
                                out expiretime))
                        {
                            continue;
                        }

                        pldetail.Tenor = tenorNow;

                        try
                        {
                            forwardRate = PriceCore.Instance.TempCalcCurrentDsRate(
                                pldetail.Symbol, 
                                pldetail.ValueDate, 
                                pldetail.BusinessUnitId);
                            if (forwardRate == null)
                            {
                                TraceManager.Error.Write(
                                    "ReportCalculateModel", 
                                    "CalDsForwardRate error dsForwardRate is null");
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            TraceManager.Error.Write("ReportCalculateModel", ex, "CalDsForwardRate error");
                            continue;
                        }

                        if (pldetail.TransactionType == TransactionTypeEnum.Sell)
                        {
                            pldetail.Model.MTMDSTraderRate = forwardRate.DsBid;
                            pldetail.Model.GrossPlXXX = pldetail.WeBuyAmount / forwardRate.DsBid - pldetail.WeSellAmount;
                        }
                        else
                        {
                            pldetail.Model.MTMDSTraderRate = forwardRate.DsAsk;
                            pldetail.Model.GrossPlXXX = pldetail.WeBuyAmount * forwardRate.DsAsk - pldetail.WeSellAmount;
                        }

                        TransPrice sellLocalQuote = PriceCore.Instance.GetTransitionPrice(
                            this.LocalCcy.CurrencyID, 
                            pldetail.WeSellCcy);
                        switch (sellLocalQuote.PriceDirection)
                        {
                            case EnumDirection.NotExisting:
                                pldetail.Model.MTMMidRate = 0.00M;
                                break;
                            case EnumDirection.Equals:
                                pldetail.Model.MTMMidRate = 1.00M;
                                break;
                            case EnumDirection.Before:
                                pldetail.Model.MTMMidRate = sellLocalQuote.QuotePrice.Mid;
                                pldetail.Model.GrossPlXXX /= pldetail.Model.MTMMidRate;
                                break;
                            case EnumDirection.After:
                                pldetail.Model.MTMMidRate = sellLocalQuote.QuotePrice.Mid;
                                pldetail.Model.GrossPlXXX *= pldetail.Model.MTMMidRate;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        pldetail.SetGrossPlForCcyNoNotify(pldetail.Model.GrossPlXXX);
                        switch (dealingBookPldetailChange.PriceDirection)
                        {
                            case EnumDirection.NotExisting:
                            case EnumDirection.Equals:
                                break;
                            case EnumDirection.Before:
                                pldetail.SetGrossPlForCcyNoNotify(
                                    pldetail.Model.GrossPlXXX * dealingBookPldetailChange.QuotePrice.Mid);
                                break;
                            case EnumDirection.After:
                                pldetail.SetGrossPlForCcyNoNotify(
                                    pldetail.Model.GrossPlXXX / dealingBookPldetailChange.QuotePrice.Mid);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        this.sanpshortDealingBookDic[pldetail.WeSellCcy].GrossPlXXX += pldetail.GrossPlXXX;
                    }

                    TransPrice dealingBookChange;
                    if (this.DealingBookShowCcy == null)
                    {
                        dealingBookChange = new TransPrice { PriceDirection = EnumDirection.Equals };
                    }
                    else
                    {
                        dealingBookChange = PriceCore.Instance.GetTransitionPrice(
                            this.LocalCcy.CurrencyID, 
                            this.DealingBookShowCcy.CurrencyID);
                    }

                    if (!this.sanpshortDealingBookDic.ContainsKey(this.GetTotalTitle()))
                    {
                        this.AddOrUpdate(
                            new BaseDealingBookItemVM(new DealingBookItemModel())
                                {
                                    CCYId = this.GetTotalTitle(), 
                                    Adjustment = -1
                                });
                    }

                    BaseDealingBookItemVM totalDealingbookItem = this.sanpshortDealingBookDic[this.GetTotalTitle()];

                    // FixBug 3534, 3535
                    decimal todayNetPositionForCcy = 0;
                    decimal totalNetPositionForCcy = 0;
                    decimal grossPlForCcy = 0;
                    decimal monthlyGrossPlForCcy = 0;

                    foreach (BaseDealingBookItemVM dealingBookItem in this.sanpshortDealingBookDic.Values)
                    {
                        if (dealingBookItem.CCYId == this.GetTotalTitle())
                        {
                            continue;
                        }

                        TransPrice transqote = PriceCore.Instance.GetTransitionPrice(
                            this.LocalCcy.CurrencyID, 
                            dealingBookItem.CCYId);
                        switch (transqote.PriceDirection)
                        {
                            case EnumDirection.NotExisting:
                            case EnumDirection.Equals:
                                dealingBookItem.TodayNetPositionXXX =
                                    dealingBookItem.TodayNetPosition.FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                                dealingBookItem.TotalNetPositionXXX =
                                    dealingBookItem.TotalNetPosition.FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                                break;
                            case EnumDirection.Before:
                                dealingBookItem.TodayNetPositionXXX =
                                    (dealingBookItem.TodayNetPosition / transqote.QuotePrice.Mid)
                                        .FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                                dealingBookItem.TotalNetPositionXXX =
                                    (dealingBookItem.TotalNetPosition / transqote.QuotePrice.Mid)
                                        .FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                                break;
                            case EnumDirection.After:
                                dealingBookItem.TodayNetPositionXXX =
                                    (dealingBookItem.TodayNetPosition * transqote.QuotePrice.Mid)
                                        .FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                                dealingBookItem.TotalNetPositionXXX =
                                    (dealingBookItem.TotalNetPosition * transqote.QuotePrice.Mid)
                                        .FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        dealingBookItem.MonthlyGrossPlXXX =
                            (dealingBookItem.BeforeMonthlyGrossPlXXX.FormatPriceBySymbolPoint(DealingBookDecimalPlace)
                             + dealingBookItem.GrossPlXXX.FormatPriceBySymbolPoint(DealingBookDecimalPlace))
                                .FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                        switch (dealingBookChange.PriceDirection)
                        {
                            case EnumDirection.NotExisting:
                            case EnumDirection.Equals:
                                dealingBookItem.SetGrossPlForCcyNoNotify(dealingBookItem.GrossPlXXX);
                                dealingBookItem.SetMonthlyGrossPlForCcyNoNotify(dealingBookItem.MonthlyGrossPlXXX);
                                dealingBookItem.SetTodayNetPositionForCcyNoNotify(dealingBookItem.TodayNetPositionXXX);
                                dealingBookItem.SetTotalNetPositionForCcyNoNotify(dealingBookItem.TotalNetPositionXXX);

                                // FixBug 3534, 3535
                                todayNetPositionForCcy +=
                                    dealingBookItem.TodayNetPositionXXX.FormatPriceBySymbolPoint(
                                        DealingBookDecimalPlace);
                                totalNetPositionForCcy +=
                                    dealingBookItem.TotalNetPositionXXX.FormatPriceBySymbolPoint(
                                        DealingBookDecimalPlace);
                                grossPlForCcy +=
                                    dealingBookItem.GrossPlXXX.FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                                monthlyGrossPlForCcy +=
                                    dealingBookItem.MonthlyGrossPlXXX.FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                                break;
                            case EnumDirection.Before:
                                dealingBookItem.SetGrossPlForCcyNoNotify(
                                    dealingBookItem.GrossPlXXX * dealingBookChange.QuotePrice.Mid);
                                dealingBookItem.SetMonthlyGrossPlForCcyNoNotify(
                                    dealingBookItem.MonthlyGrossPlXXX * dealingBookChange.QuotePrice.Mid);
                                dealingBookItem.SetTodayNetPositionForCcyNoNotify(
                                    dealingBookItem.TodayNetPositionXXX * dealingBookChange.QuotePrice.Mid);
                                dealingBookItem.SetTotalNetPositionForCcyNoNotify(
                                    dealingBookItem.TotalNetPositionXXX * dealingBookChange.QuotePrice.Mid);

                                // FixBug 3534, 3535
                                todayNetPositionForCcy +=
                                    (dealingBookItem.TodayNetPositionXXX * dealingBookChange.QuotePrice.Mid)
                                        .FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                                totalNetPositionForCcy +=
                                    (dealingBookItem.TotalNetPositionXXX * dealingBookChange.QuotePrice.Mid)
                                        .FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                                grossPlForCcy +=
                                    (dealingBookItem.GrossPlXXX * dealingBookChange.QuotePrice.Mid)
                                        .FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                                monthlyGrossPlForCcy +=
                                    (dealingBookItem.MonthlyGrossPlXXX * dealingBookChange.QuotePrice.Mid)
                                        .FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                                break;
                            case EnumDirection.After:
                                dealingBookItem.SetGrossPlForCcyNoNotify(
                                    dealingBookItem.GrossPlXXX / dealingBookChange.QuotePrice.Mid);
                                dealingBookItem.SetMonthlyGrossPlForCcyNoNotify(
                                    dealingBookItem.MonthlyGrossPlXXX / dealingBookChange.QuotePrice.Mid);
                                dealingBookItem.SetTodayNetPositionForCcyNoNotify(
                                    dealingBookItem.TodayNetPositionXXX / dealingBookChange.QuotePrice.Mid);
                                dealingBookItem.SetTotalNetPositionForCcyNoNotify(
                                    dealingBookItem.TotalNetPositionXXX / dealingBookChange.QuotePrice.Mid);

                                // FixBug 3534, 3535
                                todayNetPositionForCcy +=
                                    (dealingBookItem.TodayNetPositionXXX / dealingBookChange.QuotePrice.Mid)
                                        .FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                                totalNetPositionForCcy +=
                                    (dealingBookItem.TotalNetPositionXXX / dealingBookChange.QuotePrice.Mid)
                                        .FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                                grossPlForCcy +=
                                    (dealingBookItem.GrossPlXXX / dealingBookChange.QuotePrice.Mid)
                                        .FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                                monthlyGrossPlForCcy +=
                                    (dealingBookItem.MonthlyGrossPlXXX / dealingBookChange.QuotePrice.Mid)
                                        .FormatPriceBySymbolPoint(DealingBookDecimalPlace);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    // FixBug 3534, 3535
                    totalDealingbookItem.SetGrossPlForCcyNoNotify(grossPlForCcy);
                    totalDealingbookItem.SetMonthlyGrossPlForCcyNoNotify(monthlyGrossPlForCcy);
                    totalDealingbookItem.SetTodayNetPositionForCcyNoNotify(todayNetPositionForCcy);
                    totalDealingbookItem.SetTotalNetPositionForCcyNoNotify(totalNetPositionForCcy);
                    Application.Current.Dispatcher.Invoke(
                        new Action(
                            () =>
                                {
                                    try
                                    {
                                        foreach (BaseDealingBookItemVM value in this.sanpshortDealingBookDic.Values)
                                        {
                                            value.NotifyRealTimeProperty();
                                        }

                                        foreach (BaseDealingBookPlDetailItemVM value in this.sanpshortPlDetailDic.Values)
                                        {
                                            value.NotifyRealTimeProperty();
                                        }
                                    }
                                    catch (Exception exception)
                                    {
                                        TraceManager.Warn.Write(this.GetLogTag(), exception, "界面通知时后台变更，此错误无业务影响");
                                    }
                                }), 
                        null);
                }
                catch (Exception exception)
                {
                    TraceManager.Error.Write(
                        this.belowBusinessUnitInfo.BelowReportCalculateModel.LogTag, 
                        exception, 
                        "快照结构进行重算时出现异常。");
                }
            }

            /// <summary>
            /// 移除对应的Pldetail信息
            /// </summary>
            /// <param name="removePldetail">
            /// 待移除Pldetail信息
            /// </param>
            public void RemovePldetailInfo(BaseDealingBookPlDetailItemVM removePldetail)
            {
                if (this.sanpshortPlDetailDic.ContainsKey(removePldetail.GetID()))
                {
                    this.sanpshortPlDetailDic.Remove(removePldetail.GetID());
                    this.StatisticDealingBookItemWhenPlDetialRemove(removePldetail);
                    BaseDealingBookPlDetailItemVM deletedItem =
                        this.sanpshortPlDetailList.FirstOrDefault(item => item.Id == removePldetail.Id);
                    Application.Current.Dispatcher.Invoke(
                        new Action(() => this.sanpshortPlDetailList.Remove(deletedItem)), 
                        null);
                }
                else
                {
                    TraceManager.Debug.WriteAdditional(
                        "ReportCalculate", 
                        removePldetail, 
                        "尝试在现有的快照结构中移除此Pldetail时，找不到对应的Detail。");
                }
            }

            /// <summary>
            /// 设置DealingBookPlDetail显示货币类型
            /// </summary>
            /// <param name="showCcyId">
            /// 显示货币Id
            /// </param>
            public void SetDealingBookPlDetailShowCcy(string showCcyId)
            {
                if (this.DealingBookPlDetailShowCcy != null && this.DealingBookPlDetailShowCcy.CurrencyID == showCcyId)
                {
                    return;
                }

                this.DealingBookPlDetailShowCcy =
                    this.belowBusinessUnitInfo.BelowReportCalculateModel.InnerCurrencyCacheRepository.FindByID(
                        showCcyId);
            }

            /// <summary>
            /// 设置DealingBook显示货币类型
            /// </summary>
            /// <param name="showCcyId">
            /// 显示货币Id
            /// </param>
            public void SetDealingBookShowCcy(string showCcyId)
            {
                if (this.DealingBookShowCcy != null && this.DealingBookShowCcy.CurrencyID == showCcyId)
                {
                    return;
                }

                this.DealingBookShowCcy =
                    this.belowBusinessUnitInfo.BelowReportCalculateModel.InnerCurrencyCacheRepository.FindByID(
                        showCcyId);
            }

            #endregion

            #region Methods

            /// <summary>
            ///     获取日志标签
            /// </summary>
            /// <returns>
            ///     The <see cref="string" />.
            /// </returns>
            private string GetLogTag()
            {
                return this.belowBusinessUnitInfo.BelowReportCalculateModel.LogTag;
            }

            /// <summary>
            ///     获取Total标题
            /// </summary>
            /// <returns>Total标题</returns>
            private string GetTotalTitle()
            {
                return this.belowBusinessUnitInfo.BelowReportCalculateModel.TotalTile;
            }

            /// <summary>
            /// 将PlDetail的信息从DealingBook中移除
            /// </summary>
            /// <param name="dealingBookDetail">
            /// DealingBookDetail信息
            /// </param>
            private void StatisticDealingBookItemWhenPlDetialRemove(BaseDealingBookPlDetailItemVM dealingBookDetail)
            {
                if (!this.sanpshortDealingBookDic.ContainsKey(dealingBookDetail.CCY1)
                    || !this.sanpshortDealingBookDic.ContainsKey(dealingBookDetail.CCY2))
                {
                    TraceManager.Debug.WriteAdditional(
                        this.GetLogTag(), 
                        dealingBookDetail, 
                        "删除Detail时，对应的DealingBookItem尚不存在。");
                    return;
                }

                BaseDealingBookItemVM ccy1DealingBookItem = this.sanpshortDealingBookDic[dealingBookDetail.CCY1];
                BaseDealingBookItemVM ccy2DealingBookItem = this.sanpshortDealingBookDic[dealingBookDetail.CCY2];

                if (dealingBookDetail.Instrument == DealingBookInstrumentEnum.FXForwardHedging
                    || dealingBookDetail.Instrument == DealingBookInstrumentEnum.FXSpotHedging
                    || dealingBookDetail.Instrument == DealingBookInstrumentEnum.FXSwapForwardHedging
                    || dealingBookDetail.Instrument == DealingBookInstrumentEnum.FXSwapSpotHedging)
                {
                    if (dealingBookDetail.TransactionType == TransactionTypeEnum.Buy)
                    {
                        ccy1DealingBookItem.TodayBuyHedge -= dealingBookDetail.CCY1Amount;
                        ccy2DealingBookItem.TodaySellHedge -= dealingBookDetail.CCY2Amount;
                    }
                    else
                    {
                        ccy1DealingBookItem.TodaySellHedge -= dealingBookDetail.CCY1Amount;
                        ccy2DealingBookItem.TodayBuyHedge -= dealingBookDetail.CCY2Amount;
                    }
                }
                else
                {
                    if (dealingBookDetail.TransactionType == TransactionTypeEnum.Buy)
                    {
                        ccy1DealingBookItem.TodayBuyCustomer -= dealingBookDetail.CCY1Amount;
                        ccy2DealingBookItem.TodaySellCustomer -= dealingBookDetail.CCY2Amount;
                    }
                    else
                    {
                        ccy1DealingBookItem.TodaySellCustomer -= dealingBookDetail.CCY1Amount;
                        ccy2DealingBookItem.TodayBuyCustomer -= dealingBookDetail.CCY2Amount;
                    }
                }

                ccy1DealingBookItem.ReCalcStaticValue();
                ccy2DealingBookItem.ReCalcStaticValue();
            }

            /// <summary>
            /// 将PlDetail的信息统计进DealingBook中
            /// </summary>
            /// <param name="dealingBookDetail">
            /// DealingBookDetail信息
            /// </param>
            private void StatisticPlDetialInDealingBookItem(BaseDealingBookPlDetailItemVM dealingBookDetail)
            {
                if (!this.sanpshortDealingBookDic.ContainsKey(dealingBookDetail.CCY1)
                    || !this.sanpshortDealingBookDic.ContainsKey(dealingBookDetail.CCY2))
                {
                    TraceManager.Debug.WriteAdditional(
                        this.GetLogTag(), 
                        dealingBookDetail, 
                        "添加Detail时，对应的DealingBookItem尚不存在。");
                    return;
                }

                BaseDealingBookItemVM ccy1DealingBookItem = this.sanpshortDealingBookDic[dealingBookDetail.CCY1];
                BaseDealingBookItemVM ccy2DealingBookItem = this.sanpshortDealingBookDic[dealingBookDetail.CCY2];

                if (dealingBookDetail.Instrument == DealingBookInstrumentEnum.FXForwardHedging
                    || dealingBookDetail.Instrument == DealingBookInstrumentEnum.FXSpotHedging
                    || dealingBookDetail.Instrument == DealingBookInstrumentEnum.FXSwapForwardHedging
                    || dealingBookDetail.Instrument == DealingBookInstrumentEnum.FXSwapSpotHedging)
                {
                    if (dealingBookDetail.TransactionType == TransactionTypeEnum.Buy)
                    {
                        ccy1DealingBookItem.TodayBuyHedge += dealingBookDetail.CCY1Amount;
                        ccy2DealingBookItem.TodaySellHedge += dealingBookDetail.CCY2Amount;
                    }
                    else
                    {
                        ccy1DealingBookItem.TodaySellHedge += dealingBookDetail.CCY1Amount;
                        ccy2DealingBookItem.TodayBuyHedge += dealingBookDetail.CCY2Amount;
                    }
                }
                else
                {
                    if (dealingBookDetail.TransactionType == TransactionTypeEnum.Buy)
                    {
                        ccy1DealingBookItem.TodayBuyCustomer += dealingBookDetail.CCY1Amount;
                        ccy2DealingBookItem.TodaySellCustomer += dealingBookDetail.CCY2Amount;
                    }
                    else
                    {
                        ccy1DealingBookItem.TodaySellCustomer += dealingBookDetail.CCY1Amount;
                        ccy2DealingBookItem.TodayBuyCustomer += dealingBookDetail.CCY2Amount;
                    }
                }

                ccy1DealingBookItem.ReCalcStaticValue();
                ccy2DealingBookItem.ReCalcStaticValue();
            }

            #endregion
        }
    }
}