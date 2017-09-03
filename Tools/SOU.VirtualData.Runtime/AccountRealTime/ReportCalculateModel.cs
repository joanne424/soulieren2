// <copyright file="ReportCalculateModel.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2015/12/10 11:13:35 </date>
// <summary> 报表的计算模型 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2015/12/10 11:13:35
//      修改描述：新建 DealingBookCalculateModel.cs
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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;

    using BaseViewModel;

    using Infrastructure.Common;
    using Infrastructure.Data;
    using Infrastructure.Data.Base;
    using Infrastructure.Log;
    using Infrastructure.Models;
    using Infrastructure.Service;

    #endregion

    /// <summary>
    ///     报表的计算模型
    /// </summary>
    public partial class ReportCalculateModel : BaseModel
    {
        #region Fields

        /// <summary>
        ///     事件处理线程同步锁
        ///     用于处理其他线程抢占处理线程资源同步问题
        /// </summary>
        private readonly object consumerSnycFlag = new object();

        /// <summary>
        ///     报表计算事件高优先级队列
        /// </summary>
        private readonly BlockingCollection<ReportCalculateEvent> dealingBookHighLevelEventQueue =
            new BlockingCollection<ReportCalculateEvent>();

        /// <summary>
        ///     报表计算普通事件队列
        /// </summary>
        private readonly BlockingCollection<ReportCalculateEvent> dealingBookNormalEventQueue =
            new BlockingCollection<ReportCalculateEvent>();

        /// <summary>
        ///     新计算事件通知
        /// </summary>
        private readonly AutoResetEvent newCalcEventNotify = new AutoResetEvent(false);

        /// <summary>
        ///     新报价事件缓存队列
        /// </summary>
        private readonly BlockingCollection<ReportCalculateEvent> newPriceEventQueue =
            new BlockingCollection<ReportCalculateEvent>();

        /// <summary>
        ///     报价结构缓存
        /// </summary>
        private readonly Dictionary<string, TickQuoteModel> priceDic = new Dictionary<string, TickQuoteModel>();

        /// <summary>
        ///     DealingBook报表的窗口是否打开
        /// </summary>
        private bool isDealingBookWindowOpen;

        /// <summary>
        ///     是否初始化
        /// </summary>
        private bool isInitial;

        /// <summary>
        ///     上次报价重算时间
        /// </summary>
        private DateTime lastCalculateNewPriceTime;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportCalculateModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner Id.
        /// </param>
        public ReportCalculateModel(string ownerId)
            : base(ownerId)
        {
            this.LogTag = "ReportCalculate";
            this.InnerSymbolCacheRepository = this.GetRepository<ISymbolCacheRepository>();
            this.InnerCurrencyCacheRepository = this.GetRepository<ICurrencyCacheRepository>();
            this.InnerBusinessUnitCacheRepository = this.GetRepository<IBusinessUnitCacheRepository>();
            this.InnerQuoteCacheRepository = this.GetRepository<IQuoteCacheRepository>();
            this.TotalTile = RunTime.FindStringResource("Total");
            ThreadPool.QueueUserWorkItem(
                arg =>
                    {
                        Util.SetThreadName("ReportCalculateModel");
                        TraceManager.Info.Write(this.LogTag, "报表实时计算启动。");

                        var newPriceDic = new Dictionary<string, TickQuoteModel>();
                        while (true)
                        {
                            this.newCalcEventNotify.WaitOne();
                            TraceManager.Debug.Write(
                                this.LogTag, 
                                "实时计算收到新的计算请求，通用事件队列：{0}, 报价事件队列:{1}", 
                                this.dealingBookNormalEventQueue.Count, 
                                this.newPriceEventQueue.Count);

                            ReportCalculateEvent item;
                            while (this.dealingBookHighLevelEventQueue.TryTake(out item))
                            {
                                this.ConsumerReportEvent(item);
                            }

                            if (this.isInitial)
                            {
                                while (this.dealingBookNormalEventQueue.TryTake(out item))
                                {
                                    this.ConsumerReportEvent(item);
                                }
                            }

                            DateTime now = DateTime.Now;
                            TimeSpan diff = now - this.lastCalculateNewPriceTime;
                            if (diff.TotalMilliseconds < 400)
                            {
                                Thread.Sleep((int)(400 - diff.TotalMilliseconds));
                            }

                            while (this.newPriceEventQueue.TryTake(out item))
                            {
                                var tickquote = item.EvntArg as TickQuoteModel;
                                if (tickquote == null)
                                {
                                    TraceManager.Warn.Write(this.LogTag, "NewPriceEvnet 无法正常转换其携带的报价信息。");
                                    continue;
                                }

                                if (newPriceDic.ContainsKey(tickquote.SymbolID))
                                {
                                    newPriceDic[tickquote.SymbolID] = tickquote;
                                }
                                else
                                {
                                    newPriceDic.Add(tickquote.SymbolID, tickquote);
                                }
                            }

                            if (newPriceDic.Count > 0)
                            {
                                TraceManager.Debug.Write(this.LogTag, "新报价携带报价数量：{0}", newPriceDic.Count);
                                this.RecalculateWhenNewPrice(newPriceDic.Values.ToList());
                            }

                            newPriceDic.Clear();

                            this.lastCalculateNewPriceTime = DateTime.Now;
                        }
                    });
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     业务区仓储
        /// </summary>
        public IBusinessUnitCacheRepository InnerBusinessUnitCacheRepository { get; set; }

        /// <summary>
        ///     日志标签
        /// </summary>
        public string LogTag { get; set; }

        /// <summary>
        ///     汇总标题
        /// </summary>
        public string TotalTile { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///     货币仓储
        /// </summary>
        private ICurrencyCacheRepository InnerCurrencyCacheRepository { get; set; }

        /// <summary>
        ///     报价仓储
        /// </summary>
        private IQuoteCacheRepository InnerQuoteCacheRepository { get; set; }

        /// <summary>
        ///     货币仓储
        /// </summary>
        private ISymbolCacheRepository InnerSymbolCacheRepository { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 获取Snapshot下DealingBook的可绑定列表
        /// </summary>
        /// <param name="bussinessId">
        /// 业务区Id
        /// </param>
        /// <param name="snapshortId">
        /// 快照Id
        /// </param>
        /// <returns>
        /// 可绑定列表
        /// </returns>
        public ObservableCollection<BaseDealingBookItemVM> GetDealingBookBindList(
            string bussinessId, 
            string snapshortId)
        {
            BusinessUnitInfo busetInfo = this.GetBusinessUnitInfo(bussinessId);
            if (busetInfo != null)
            {
                if (busetInfo.CurrentSnapshotInfo != null && busetInfo.CurrentSnapshotInfo.SnapshotId == snapshortId)
                {
                    return busetInfo.CurrentSnapshotInfo.GetDealingBookBindList();
                }
            }

            return null;
        }

        /// <summary>
        /// 获取Snapshot下DealingBookPlDetail的可绑定列表
        /// </summary>
        /// <param name="bussinessId">
        /// 业务区Id
        /// </param>
        /// <param name="snapshortId">
        /// 快照Id
        /// </param>
        /// <returns>
        /// 可绑定列表
        /// </returns>
        public ObservableCollection<BaseDealingBookPlDetailItemVM> GetDealingBookPlDetailBindList(
            string bussinessId, 
            string snapshortId)
        {
            BusinessUnitInfo busetInfo = this.GetBusinessUnitInfo(bussinessId);
            if (busetInfo != null)
            {
                if (busetInfo.CurrentSnapshotInfo != null && busetInfo.CurrentSnapshotInfo.SnapshotId == snapshortId)
                {
                    return busetInfo.CurrentSnapshotInfo.GetPlDetailBindList();
                }
            }

            return null;
        }

        /// <summary>
        /// 获取Bu下Snapshot的可绑定列表
        /// </summary>
        /// <param name="businessUnitId">
        /// 业务区Id
        /// </param>
        /// <returns>
        /// 可绑定列表
        /// </returns>
        public ObservableCollection<BaseDealingBookSnapshotInfoVM> GetSnapshotBindList(string businessUnitId)
        {
            BusinessUnitInfo busetInfo = this.GetBusinessUnitInfo(businessUnitId);
            if (busetInfo == null)
            {
                TraceManager.Error.Write(this.LogTag, "尝试获取不存在Bu的快照列表，BussinessUnitId：{0}", businessUnitId);
                return null;
            }

            return busetInfo.GetSnapshotBindList();
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="evnt">
        /// 报表计算事件
        /// </param>
        public void PublishEvent(ReportCalculateEvent evnt)
        {
            if (evnt == null)
            {
                TraceManager.Warn.Write(this.LogTag, "The report calculate event is null, when publish.");
                return;
            }

            switch (evnt.EvntType)
            {
                case ReportCalculateEvent.ReportCalEvtTypeEnum.ResourceInitial:
                    this.dealingBookHighLevelEventQueue.Add(evnt);
                    break;
                case ReportCalculateEvent.ReportCalEvtTypeEnum.NewPrice:
                    if (this.isDealingBookWindowOpen)
                    {
                        this.newPriceEventQueue.Add(evnt);
                    }

                    break;
                case ReportCalculateEvent.ReportCalEvtTypeEnum.PushBack:
                case ReportCalculateEvent.ReportCalEvtTypeEnum.FocusSnapshotBussinessUnit:
                case ReportCalculateEvent.ReportCalEvtTypeEnum.DealingBookWindowOpen:
                case ReportCalculateEvent.ReportCalEvtTypeEnum.DealingBookWindowClose:
                case ReportCalculateEvent.ReportCalEvtTypeEnum.ChangeReportShowCcy:
                    this.dealingBookNormalEventQueue.Add(evnt);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.newCalcEventNotify.Set();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 消费报表事件
        /// </summary>
        /// <param name="evnt">
        /// 报表事件
        /// </param>
        private void ConsumerReportEvent(ReportCalculateEvent evnt)
        {
            lock (this.consumerSnycFlag)
            {
                try
                {
                    switch (evnt.EvntType)
                    {
                        case ReportCalculateEvent.ReportCalEvtTypeEnum.ResourceInitial:
                            this.HandInitial();
                            break;
                        case ReportCalculateEvent.ReportCalEvtTypeEnum.PushBack:
                            this.HandlerPushBackEvent(evnt);
                            break;
                        case ReportCalculateEvent.ReportCalEvtTypeEnum.FocusSnapshotBussinessUnit:

                            ////TODO:当前尚未处理，需要在界面添加，感知判断当前显示页面逻辑后，再完善，属于优化项
                            break;
                        case ReportCalculateEvent.ReportCalEvtTypeEnum.DealingBookWindowOpen:

                            ////TODO：当前会计算所有的Bu的未关闭报表信息，待优化
                            this.isDealingBookWindowOpen = true;
                            this.RecalculateFocusBuSanpshot();
                            PriceReceiveCore.Instance.ReadQuotePriceEvent += this.NewPriceHandler;
                            break;
                        case ReportCalculateEvent.ReportCalEvtTypeEnum.DealingBookWindowClose:
                            this.isDealingBookWindowOpen = false;
                            PriceReceiveCore.Instance.ReadQuotePriceEvent -= this.NewPriceHandler;
                            break;
                        case ReportCalculateEvent.ReportCalEvtTypeEnum.ChangeReportShowCcy:
                            this.HandleChangeReportShowCcy(evnt);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (Exception exception)
                {
                    TraceManager.Error.WriteAdditional(this.LogTag, evnt, exception, "处理报表事件时出现异常。");
                }
            }
        }

        /// <summary>
        ///     处理初始化
        /// </summary>
        private void HandInitial()
        {
            this.isInitial = true;
            foreach (BaseQuoteVM quoteVm in this.InnerQuoteCacheRepository.Filter(c => true))
            {
                this.priceDic.Add(quoteVm.SymbolID, quoteVm.PropSet.Clone());
            }
        }

        /// <summary>
        /// 处理报表显示Ccy变更事件
        /// </summary>
        /// <param name="evnt">
        /// 显示Ccy变更事件
        /// </param>
        private void HandleChangeReportShowCcy(ReportCalculateEvent evnt)
        {
            var arg = evnt.EvntArg as DealingBookShowCcyChangeArg;
            if (arg == null)
            {
                TraceManager.Error.WriteAdditional(this.LogTag, evnt, "处理报表显示Ccy变更事件时，无法正常转换事件参数。");
                return;
            }

            BusinessUnitInfo busetInfo = this.GetBusinessUnitInfo(arg.BussinessUnitId);
            if (busetInfo == null)
            {
                TraceManager.Error.WriteAdditional(
                    this.LogTag, 
                    evnt, 
                    "处理报表显示Ccy变更事件时，无法找到对应Bu信息集，Buid：{0}。", 
                    arg.BussinessUnitId);
                return;
            }

            if (busetInfo.CurrentSnapshotInfo == null)
            {
                TraceManager.Error.WriteAdditional(
                    this.LogTag, 
                    evnt, 
                    "处理报表显示Ccy变更事件时，bu下不存在正在处理的快照，Buid：{0}。", 
                    arg.BussinessUnitId);
                return;
            }

            if (arg.ReportType == DealingBookShowCcyChangeArg.ReportTypeEnum.DealingBook)
            {
                busetInfo.CurrentSnapshotInfo.SetDealingBookShowCcy(arg.ChangeCcyId);
            }
            else
            {
                busetInfo.CurrentSnapshotInfo.SetDealingBookPlDetailShowCcy(arg.ChangeCcyId);
            }

            this.RecalculateFocusBuSanpshot();
        }

        /// <summary>
        /// 处理报表的推送事件
        /// </summary>
        /// <param name="evnt">
        /// 事件
        /// </param>
        private void HandlerPushBackEvent(ReportCalculateEvent evnt)
        {
            var pushArg = evnt.EvntArg as DealingBookPushBackArg;
            if (pushArg == null)
            {
                TraceManager.Warn.WriteAdditional(
                    this.LogTag, 
                    evnt, 
                    "The report calculate event eventArg cant be change to DealingBookPushBackArg. EventInfo:");
                return;
            }

            if (pushArg.IsDelete)
            {
                foreach (BaseDealingBookPlDetailItemVM pldetailItem in pushArg.ChangeDealingBookPlDetailItemList)
                {
                    BusinessUnitInfo busetInfo = this.GetBusinessUnitInfo(pldetailItem.BusinessUnitId);
                    if (busetInfo == null)
                    {
                        TraceManager.Warn.WriteAdditional("ReportCalculte", pldetailItem, "推送PlDetail移除时，找不到其对应的Bu信息集。");
                        continue;
                    }

                    if (busetInfo.CurrentSnapshotInfo == null)
                    {
                        TraceManager.Warn.WriteAdditional(
                            "ReportCalculte", 
                            pldetailItem, 
                            "推送PlDetail移除时，对应的Bu信息集中不存在未关闭快照。");
                        continue;
                    }

                    busetInfo.CurrentSnapshotInfo.RemovePldetailInfo(pldetailItem);
                }
            }
            else
            {
                foreach (BaseDealingBookSnapshotInfoVM snapshotItme in
                    pushArg.ChangeSnapshortList.OrderBy(c => c.SnapshotTimeFrom))
                {
                    BusinessUnitInfo busetInfo = this.GetBusinessUnitInfo(snapshotItme.BusinessUnitId);
                    if (busetInfo == null)
                    {
                        TraceManager.Warn.WriteAdditional(
                            "ReportCalculte", 
                            snapshotItme, 
                            "推送snapshotItme时，找不到其对应的Bu信息集。");
                        continue;
                    }

                    busetInfo.AddOrUpdate(snapshotItme);
                }

                foreach (BaseDealingBookPlDetailItemVM pldetailItem in pushArg.ChangeDealingBookPlDetailItemList)
                {
                    BusinessUnitInfo busetInfo = this.GetBusinessUnitInfo(pldetailItem.BusinessUnitId);
                    if (busetInfo == null)
                    {
                        TraceManager.Warn.WriteAdditional("ReportCalculte", pldetailItem, "推送PlDetail时，找不到其对应的Bu信息集。");
                        continue;
                    }

                    if (busetInfo.CurrentSnapshotInfo == null)
                    {
                        TraceManager.Warn.WriteAdditional(
                            "ReportCalculte", 
                            pldetailItem, 
                            "推送PlDetail时，对应的Bu信息集中不存在未关闭快照。");
                        continue;
                    }

                    busetInfo.CurrentSnapshotInfo.AddOrUpdate(pldetailItem);
                }

                foreach (BaseDealingBookItemVM dealingBookItem in pushArg.ChangeDealingBookItemList)
                {
                    BusinessUnitInfo busetInfo = this.GetBusinessUnitInfo(dealingBookItem.BusinessUnitId);
                    if (busetInfo == null)
                    {
                        TraceManager.Warn.WriteAdditional(
                            "ReportCalculte", 
                            dealingBookItem, 
                            "推送DealingBook时，找不到其对应的Bu信息集。");
                        continue;
                    }

                    if (busetInfo.CurrentSnapshotInfo == null)
                    {
                        TraceManager.Warn.WriteAdditional(
                            "ReportCalculte", 
                            dealingBookItem, 
                            "推送DealingBook时，对应的Bu信息集中不存在未关闭快照。");
                        continue;
                    }

                    // 非Total项计算CurrencyName
                    if (dealingBookItem.CCYId != this.TotalTile)
                    {
                        dealingBookItem.CurrencyName = this.InnerCurrencyCacheRepository.GetName(dealingBookItem.CCYId);
                    }

                    busetInfo.CurrentSnapshotInfo.AddOrUpdate(dealingBookItem);
                }
            }

            this.RecalculateFocusBuSanpshot();
        }

        /// <summary>
        /// 接受报价处理
        /// </summary>
        /// <param name="tickQuote">
        /// 报价
        /// </param>
        private void NewPriceHandler(TickQuoteModel tickQuote)
        {
            this.PublishEvent(
                new ReportCalculateEvent
                    {
                        EvntType = ReportCalculateEvent.ReportCalEvtTypeEnum.NewPrice,
                        EvntArg = tickQuote.Clone()
                    });
        }

        /// <summary>
        ///     重算关注的Bu的未完成快照信息
        /// </summary>
        private void RecalculateFocusBuSanpshot()
        {
            if (!this.isDealingBookWindowOpen)
            {
                return;
            }

            foreach (BusinessUnitInfo businessUnitInfo in this.businessUnitInfoDic.Values)
            {
                if (businessUnitInfo.CurrentSnapshotInfo == null)
                {
                    continue;
                }

                businessUnitInfo.CurrentSnapshotInfo.Recalculate();
            }
        }

        /// <summary>
        /// 在新报价到来时进行重新计算
        /// </summary>
        /// <param name="newPriceList">
        /// 新报价列表
        /// </param>
        private void RecalculateWhenNewPrice(IEnumerable<TickQuoteModel> newPriceList)
        {
            foreach (TickQuoteModel tickQuoteModel in newPriceList)
            {
                if (this.priceDic.ContainsKey(tickQuoteModel.SymbolID))
                {
                    this.priceDic[tickQuoteModel.SymbolID] = tickQuoteModel;
                }
                else
                {
                    this.priceDic.Add(tickQuoteModel.SymbolID, tickQuoteModel);
                }
            }

            this.RecalculateFocusBuSanpshot();
        }

        #endregion
    }
}