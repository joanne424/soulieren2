// <copyright file="RepositoryModel.cs" company="BancLogix">
// Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author> zhangwz </author>
// <date> 2013/10/28 9:48:52 </date>
// <modify>
//   修改人：zhangwz
//   修改时间：2013/10/28 9:48:52
//   修改描述：新建 RepositoryService
//   版本：1.0
// </modify>
// <review>
//   review人：
//   review时间：
// </review >

namespace DM2.Manager.Models
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using BaseViewModel;

    using Infrastructure.Common.Enums;
    using Infrastructure.Data;
    using Infrastructure.Log;
    using Infrastructure.Service;
    using Infrastructure.Utils;

    using PerformanceStatisticCore;

    #endregion

    /// <summary>
    ///     各个CacheRepository初始化
    /// </summary>
    public class RepositoryModel : BaseModel
    {
        #region Fields

        /// <summary>
        ///     银行初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent bankInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     银行初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent bankaccountInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     BTB交易单初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent btbdealInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     业务区初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent businessunitInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     国家初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent countryInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     授信初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent creditInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     所属RunTime
        /// </summary>
        private readonly RunTime currRunTime;

        /// <summary>
        ///     货币初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent currencyInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     账户类型初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent custcategoryInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     账户等级初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent custgradingInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     账户组初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent custgroupInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     客户初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent customerInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     订单初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent dealInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     默认银行账户初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent defaultbankacctInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     远期点数初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent forwardpointInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     通用配置初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent generalsettingInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     组初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent groupInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     对冲账户初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent hedgeaccountInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     对冲交易单初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent hedgedealInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     区域配置初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent localeInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     保证金呼叫初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent margincallInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     挂单初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent pendingorderInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     权限初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent permissionsInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     报价初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent quoteInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     报价组初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent quotegroupInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     角色初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent roleInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     交割节假日初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent settlementholidayInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     员工初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent staffInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     员工提醒设置初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent staffeventInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     商品初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent symbolInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     vip设置初始化完毕的线程通知事件
        /// </summary>
        private readonly ManualResetEvent vipsettingsInitialCompleteEvent = new ManualResetEvent(false);

        /// <summary>
        ///     The total customer no list.
        /// </summary>
        private volatile List<string> totalCustomerNoList;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the RepositoryModel class.
        /// </summary>
        /// <param name="currRunTime">
        /// The curr Run Time.
        /// </param>
        /// <param name="varOwnerId">
        /// 拥有者Id
        /// </param>
        public RepositoryModel(RunTime currRunTime, string varOwnerId = null)
            : base(varOwnerId)
        {
            this.currRunTime = currRunTime;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     清空仓储
        /// </summary>
        public void Clear()
        {
            // 银行账户
            this.GetRepository<IBankAccountCacheRepository>().ClearRepository();

            // 银行
            this.GetRepository<IBankCacheRepository>().ClearRepository();

            // BTB单
            this.GetRepository<IBTBDealCacheRepository>().ClearRepository();

            // 业务区
            this.GetRepository<IBusinessUnitCacheRepository>().ClearRepository();

            // 国家
            this.GetRepository<ICountryCacheRepository>().ClearRepository();

            // 授信
            this.GetRepository<ICreditCacheRepository>().ClearRepository();

            // 货币
            this.GetRepository<ICurrencyCacheRepository>().ClearRepository();

            // 账户类型
            this.GetRepository<ICustomerCategoryCacheRepository>().ClearRepository();

            // 账户等级
            this.GetRepository<ICustGradingCacheRepository>().ClearRepository();

            // 账户组
            this.GetRepository<ICustomerGroupCacheRepository>().ClearRepository();

            // 客户账户
            this.GetRepository<ICustomerCacheRepository>().ClearRepository();

            // 订单
            this.GetRepository<IDealCacheRepository>().ClearRepository();

            // 默认银行
            this.GetRepository<IDefaultBankAccCacheRepository>().ClearRepository();

            // Forward Point
            this.GetRepository<IForwardPointCacheRepository>().ClearRepository();

            // 通用配置
            this.GetRepository<IGeneralSettingCacheRepository>().ClearRepository();

            // 组
            this.GetRepository<IGroupCacheRepository>().ClearRepository();

            // 对冲账户
            this.GetRepository<IHedgeAccountCacheRepository>().ClearRepository();

            // 对冲订单
            this.GetRepository<IHedgeDealCacheRepository>().ClearRepository();

            // MarginCall
            this.GetRepository<IMarginCallCacheRepository>().ClearRepository();

            // 员工提醒
            this.GetRepository<IStaffEventCacheRepository>().ClearRepository();

            // 区域性
            this.GetRepository<ILocaleCacheRepository>().ClearRepository();

            // 挂单
            this.GetRepository<IOrderCacheRepository>().ClearRepository();

            // 权限
            this.GetRepository<IPermissionsCasheRepository>().ClearRepository();

            // 报价
            this.GetRepository<IQuoteCacheRepository>().ClearRepository();

            // 报价组
            this.GetRepository<IQuoteGroupCacheRepository>().ClearRepository();

            // 角色
            this.GetRepository<IRoleModelCasheRepository>().ClearRepository();

            // 交割节假日
            this.GetRepository<ISettlementHolidayCacheRepository>().ClearRepository();

            // 类型为Virtual Dealer 的Staff
            this.GetRepository<IStaffCacheRepository>().ClearRepository();

            // 商品
            this.GetRepository<ISymbolCacheRepository>().ClearRepository();

            // vip设置
            this.GetRepository<IVIPCustSettingCacheRepository>().ClearRepository();
        }

        /// <summary>
        ///     运行仓储服务
        /// </summary>
        public void Intital()
        {
            // 客户账户,无依赖
            ThreadPool.QueueUserWorkItem(this.InitCustomerAction);

            // 账户组,无资源依赖
            ThreadPool.QueueUserWorkItem(this.InitCustGroupAction);

            // 商品，无资源依赖
            ThreadPool.QueueUserWorkItem(this.InitSymbolAction);

            // 业务区,无资源依赖
            ThreadPool.QueueUserWorkItem(this.InitBusinessUnitAction);

            // 角色,无依赖
            ThreadPool.QueueUserWorkItem(this.InitRoleAction);

            // 货币，无资源依赖
            ThreadPool.QueueUserWorkItem(this.InitCurrencyAction);

            // 通用配置，无资源依赖
            ThreadPool.QueueUserWorkItem(this.InitGeneralSettingAction);

            // 报价组合,无资源依赖
            ThreadPool.QueueUserWorkItem(this.InitQuoteGroupAction);

            // ForwardPoint,无资源依赖
            ThreadPool.QueueUserWorkItem(this.InitForwardPointAction);

            // 交割节假日,无资源依赖
            ThreadPool.QueueUserWorkItem(this.InitSettlementHolidayAction);

            // 国家,无资源依赖
            ThreadPool.QueueUserWorkItem(this.InitCountryAction);

            // 账户类型,无资源依赖
            ThreadPool.QueueUserWorkItem(this.InitCustCategoryAction);

            // 账户等级,无资源依赖
            ThreadPool.QueueUserWorkItem(this.InitCustGradingAction);

            // 默认银行账户,无依赖
            ThreadPool.QueueUserWorkItem(this.InitDefaultBankAcctAction);

            // 客户组,无依赖
            ThreadPool.QueueUserWorkItem(this.InitGroupAction);

            // 银行，无依赖
            ThreadPool.QueueUserWorkItem(this.InitBankAction);

            // 区域性仓储,无依赖
            ThreadPool.QueueUserWorkItem(this.InitLocaleAction);

            // 员工提醒仓储,无依赖
            ThreadPool.QueueUserWorkItem(this.InitialStaffEventAction);

            // 初始化授信仓储,无依赖
            ThreadPool.QueueUserWorkItem(this.InitCreditAction);

            // 报价,无依赖
            ThreadPool.QueueUserWorkItem(this.InitQuoteAction);

            // 类型为Virtual Dealer 的Staff，依赖CustGroup
            ThreadPool.QueueUserWorkItem(this.InitStaffAction);

            // 银行账户,依赖bu
            ThreadPool.QueueUserWorkItem(this.InitBankAccountAction);

            // 权限,依赖role
            ThreadPool.QueueUserWorkItem(this.InitPermissionsAction);

            // vip设置，依赖Customer
            ThreadPool.QueueUserWorkItem(this.InitVipSettingsAction);

            // MarginCall,依赖Cusotmer
            ThreadPool.QueueUserWorkItem(this.InitialMarginCallAction);

            // 等待所有的初始化完毕
            this.WaitAllCompleteInitial();
        }

        /// <summary>
        ///     延迟初始化
        /// </summary>
        public void LazyInitial()
        {
            // 挂单,无依赖
            ThreadPool.QueueUserWorkItem(this.InitPendingOrderAction);

            // 订单，依赖Customer和symbol
            ThreadPool.QueueUserWorkItem(this.InitDealAction);

            // 对冲账户,无依赖
            ThreadPool.QueueUserWorkItem(this.InitHedgeAccountAction);

            // 对冲交易,无依赖
            ThreadPool.QueueUserWorkItem(this.InitHedgeDealAction);

            // BTB交易,无依赖
            ThreadPool.QueueUserWorkItem(this.InitBtBDealAction);

            this.WaitAllCompleteLazyInitial();

            // 清理资源初始化的痕迹
            this.ClearAfterInitial();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     初始化后清理
        /// </summary>
        private void ClearAfterInitial()
        {
            this.totalCustomerNoList = null;
            this.currencyInitialCompleteEvent.Reset();
            this.symbolInitialCompleteEvent.Reset();
            this.generalsettingInitialCompleteEvent.Reset();
            this.custgroupInitialCompleteEvent.Reset();
            this.businessunitInitialCompleteEvent.Reset();
            this.quotegroupInitialCompleteEvent.Reset();
            this.forwardpointInitialCompleteEvent.Reset();
            this.settlementholidayInitialCompleteEvent.Reset();
            this.countryInitialCompleteEvent.Reset();
            this.custcategoryInitialCompleteEvent.Reset();
            this.custgradingInitialCompleteEvent.Reset();
            this.staffInitialCompleteEvent.Reset();
            this.bankaccountInitialCompleteEvent.Reset();
            this.defaultbankacctInitialCompleteEvent.Reset();
            this.roleInitialCompleteEvent.Reset();
            this.permissionsInitialCompleteEvent.Reset();
            this.groupInitialCompleteEvent.Reset();
            this.hedgeaccountInitialCompleteEvent.Reset();
            this.hedgedealInitialCompleteEvent.Reset();
            this.btbdealInitialCompleteEvent.Reset();
            this.bankInitialCompleteEvent.Reset();
            this.customerInitialCompleteEvent.Reset();
            this.vipsettingsInitialCompleteEvent.Reset();
            this.dealInitialCompleteEvent.Reset();
            this.pendingorderInitialCompleteEvent.Reset();
            this.creditInitialCompleteEvent.Reset();
            this.quoteInitialCompleteEvent.Reset();
            this.localeInitialCompleteEvent.Reset();
            this.margincallInitialCompleteEvent.Reset();
            this.staffeventInitialCompleteEvent.Reset();
        }

        /// <summary>
        /// 初始化银行账户动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitBankAccountAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitBankAccountAction begin");

            this.businessunitInitialCompleteEvent.WaitOne();

            // 获取银行账户仓储
            var reps = this.GetRepository<IBankAccountCacheRepository>();

            // 实例化银行账户服务
            var service = this.GetSevice<BankAccountService>();

            IEnumerable<string> businessUnitIdList =
                this.GetRepository<IBusinessUnitCacheRepository>().Filter(o => true).Select(o => o.BusinessUnitID);

            var bankAcctVmList = new List<BaseBankAccountVM>();
            foreach (string businessUnitId in businessUnitIdList)
            {
                bankAcctVmList.AddRange(service.FindByBusinessUnitId(businessUnitId));
            }

            // 初始化银行账户仓储
            reps.InitialRepository(bankAcctVmList);

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitBankAccountAction end");

            this.bankaccountInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化Bank动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitBankAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitBankAction begin");

            // 银行仓储
            var bankRep = this.GetRepository<IBankCacheRepository>();

            // 获取银行服务
            var bankService = this.GetSevice<BankService>();

            // 查询Bank信息
            List<BaseBankVM> list = bankService.BankFindAll();

            bankRep.InitialRepository(list);

            TraceManager.Info.Write("RepositoryModel", "InitBankAction end");

            this.bankInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化BTB动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitBtBDealAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitBtBDealAction begin");

            // BTB仓储
            var btbRep = this.GetRepository<IBTBDealCacheRepository>();

            // 获取BTB订单服务
            var service = this.GetSevice<BtbDealService>();

            // 初始化
            btbRep.InitialRepository(service.FindAllBtdDeal());

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitBtBDealAction end");

            this.btbdealInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化业务区动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitBusinessUnitAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitBusinessUnitAction begin");

            // 获取商品仓储
            var businessUnitRep = this.GetRepository<IBusinessUnitCacheRepository>();

            // 实例化商品服务
            var service = this.GetSevice<BusinessUnitService>();

            // 初始化商品仓储
            businessUnitRep.InitialRepository(service.FindAll());

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitBusinessUnitAction end");

            this.businessunitInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化国家动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitCountryAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitCountryAction begin");

            // 获取国家仓储
            var reps = this.GetRepository<ICountryCacheRepository>();

            // 实例化国家服务
            var service = this.GetSevice<CountryService>();

            // 初始化国家仓储
            reps.InitialRepository(service.FindAll());

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitCountryAction end");

            this.countryInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化授信动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitCreditAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitCreditAction begin");

            // 获取授信仓储
            var creditRep = this.GetRepository<ICreditCacheRepository>();

            // 获取授信服务
            var service = this.GetSevice<CreditService>();

            this.customerInitialCompleteEvent.WaitOne();

            List<BaseCreditVM> creditList = service.FindAll(this.totalCustomerNoList);

            // 初始化订单仓储
            creditRep.InitialRepository(creditList);

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitCreditAction end");

            this.creditInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化货币动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitCurrencyAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitCurrencyAction begin");

            // 获取货币仓储
            var varCustomerRep = this.GetRepository<ICurrencyCacheRepository>();

            // 实例化货币服务
            var service = this.GetSevice<CurrencyService>();

            // 初始化仓储
            varCustomerRep.InitialRepository(service.FindAll());

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitCurrencyAction end");

            // 发送初始化完毕事件
            this.currencyInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化账户类型动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitCustCategoryAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitCustCategoryAction begin");

            // 获取账户类型仓储
            var reps = this.GetRepository<ICustomerCategoryCacheRepository>();

            // 实例化账户类型服务
            var service = this.GetSevice<CustomerCategoryService>();

            // 初始化账户类型仓储
            reps.InitialRepository(service.FindAll());

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitCustCategoryAction end");

            this.custcategoryInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化账户等级动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitCustGradingAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitCustGradingAction begin");

            // 获取账户等级仓储
            var reps = this.GetRepository<ICustGradingCacheRepository>();

            // 实例化账户等级服务
            var service = this.GetSevice<CustomerGradingService>();

            // 初始化账户等级仓储
            reps.InitialRepository(service.FindAll());

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitCustGradingAction end");

            this.custgradingInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化客户账户组动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitCustGroupAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitCustGroupAction begin");

            // 获取客户账户组仓储
            var reps = this.GetRepository<ICustomerGroupCacheRepository>();

            // 实例化客户账户组服务
            var service = this.GetSevice<CustomerGroupService>();

            // 初始化客户账户组仓储
            reps.InitialRepository(service.FindAll(this.currRunTime.CurrentStaff.StaffBaseInfo.CustomerGroup));

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitCustGroupAction end");

            this.custgroupInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化客户动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitCustomerAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitCustomerAction begin");

            // 获取客户仓储
            var custRepository = this.GetRepository<ICustomerCacheRepository>();

            // 实例化客户服务
            var service = this.GetSevice<CustomerService>();

            List<BaseCustomerViewModel> customerList = service.FindAll();

            List<string> custNoList = customerList.Select(c => c.CustmerNo).ToList();

            // 初始化客户仓储
            custRepository.InitialRepository(customerList);

            this.totalCustomerNoList = custNoList;

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitCustomerAction end");

            this.customerInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化订单动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitDealAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitDealAction begin");

            // 获取订单仓储
            var dealRep = this.GetRepository<IDealCacheRepository>();

            // 实例化订单服务
            var service = this.GetSevice<DealService>();
            var status = new List<int>
                             {
                                 (int)DealStatusEnum.Open, 
                                 (int)DealStatusEnum.DeletedPending, 
                                 (int)DealStatusEnum.DeletedApproval
                             };

            List<BaseDealVM> dealList = service.FindAll(status);

            // TODO: 此处待确认添加原因
            List<BaseDealVM> swapDealAnotherLegs =
                dealList.Where(item => item.ExecutionID != item.DealID)
                    .Select(item => service.FindAnotherLeg(item.DealModel))
                    .Where(deal => deal.Status != DealStatusEnum.Open)
                    .ToList();

            // 填充部分结算状态swap单的已结算那条腿
            dealList.AddRange(swapDealAnotherLegs);

            this.customerInitialCompleteEvent.WaitOne();

            this.symbolInitialCompleteEvent.WaitOne();

            // 初始化订单仓储
            dealRep.InitialRepository(dealList);

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitDealAction end");

            this.dealInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化默认银行账户动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitDefaultBankAcctAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitDefaultBankAcctAction begin");

            // 获取默认银行账户仓储
            var reps = this.GetRepository<IDefaultBankAccCacheRepository>();

            // 实例化默认银行账户服务
            var service = this.GetSevice<BankAcctDefaultService>();

            // 初始化默认银行账户仓储
            reps.InitialRepository(
                service.FindByBusinessUnitId(this.currRunTime.CurrentStaff.StaffBaseInfo.BusinessUnitID));

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitDefaultBankAcctAction end");

            this.defaultbankacctInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化ForwardPoint动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitForwardPointAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitForwardPointAction begin");

            // 等待货币初始化完毕
            this.currencyInitialCompleteEvent.WaitOne();

            // 等待货币对初始化完毕
            this.symbolInitialCompleteEvent.WaitOne();

            // 获取远期点数仓储
            var varForwardPointRep = this.GetRepository<IForwardPointCacheRepository>();

            // 实例化远期点数服务
            var service = this.GetSevice<ForwardPointService>();

            // 初始化仓储
            varForwardPointRep.InitialRepository(service.FindAll());

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitForwardPointAction end");

            this.forwardpointInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化通用配置动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitGeneralSettingAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitGeneralSettingAction begin");

            // 获取通用配置仓储
            var generalSettingReps = this.GetRepository<IGeneralSettingCacheRepository>();

            // 实例化通用配置服务
            var service = this.GetSevice<GeneralSettingService>();

            List<BaseGeneralSettingVM> generalSettings = service.GetGeneralSetting();
            BaseGeneralSettingVM generalSet = generalSettings.FirstOrDefault();
            if (generalSet != null)
            {
                SessionHelper.Instance.SetIdleTime(new TimeSpan(0, generalSet.LogoutIdleTime, 0));
            }

            // 初始化通用配置仓储
            generalSettingReps.InitialRepository(generalSettings);

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitGeneralSettingAction end");

            // 发送初始化完毕通知
            this.generalsettingInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化组动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitGroupAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitGroupAction begin");

            // 获取组仓储
            var reps = this.GetRepository<IGroupCacheRepository>();

            // 实例化组服务
            var service = this.GetSevice<GroupService>();

            // 初始化组仓储
            reps.InitialRepository(service.FindAll(string.Empty));

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitGroupAction end");

            this.groupInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化对冲账户动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitHedgeAccountAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitHedgeAccountAction begin");

            // 获取对冲账户仓储
            var reps = this.GetRepository<IHedgeAccountCacheRepository>();

            // 实例化对冲账户服务
            var service = this.GetSevice<HedgeAccountService>();

            // 初始化对冲账户仓储
            reps.InitialRepository(service.FindAll());

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitHedgeAccountAction end");

            this.hedgeaccountInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化对冲交易动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitHedgeDealAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitHedgeDealAction begin");

            // 获取对冲账户仓储
            var reps = this.GetRepository<IHedgeDealCacheRepository>();

            // 实例化对冲账户服务
            var service = this.GetSevice<HedgeDealService>();

            // 初始化对冲账户仓储
            reps.InitialRepository(service.FindAll());

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitHedgeDealAction end");

            this.hedgedealInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化区域性动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitLocaleAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitLocaleAction begin");

            // 获取区域化仓储
            var localeRep = this.GetRepository<ILocaleCacheRepository>();

            // 获取区域化服务
            var localeService = this.GetSevice<LocaleService>();

            // 初始化仓储
            localeRep.InitialRepository(localeService.FindAll());

            TraceManager.Info.Write("RepositoryModel", "InitLocaleAction end");

            this.localeInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化挂单动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitPendingOrderAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitPendingOrderAction begin");

            // 获取挂单仓储
            var dealRep = this.GetRepository<IOrderCacheRepository>();

            // 实例化挂单服务
            var service = this.GetSevice<OrderService>();

            // 初始化挂单仓储
            dealRep.InitialRepository(service.FindPendingOrder());

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitPendingOrderAction end");

            this.pendingorderInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化权限动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitPermissionsAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitPermissionsAction begin");

            this.roleInitialCompleteEvent.WaitOne();

            // 获取权限仓储
            var reps = this.GetRepository<IPermissionsCasheRepository>();

            // 实例化权限服务
            var service = this.GetSevice<PermissionsService>();

            var permissionList = new List<string>();

            BaseRoleVM role =
                this.GetRepository<IRoleModelCasheRepository>()
                    .FindByID(this.currRunTime.CurrentStaff.StaffBaseInfo.Role);

            if (role != null)
            {
                permissionList = role.Permissions;
            }

            // 初始化权限仓储
            reps.InitialRepository(service.FindAll(permissionList));

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitPermissionsAction end");

            this.permissionsInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化报价动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitQuoteAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitQuoteAction begin");

            // 获取报价仓储
            var quoteReps = this.GetRepository<IQuoteCacheRepository>();

            // 实例化报价服务
            var service = this.GetSevice<QuotePriceService>();

            this.symbolInitialCompleteEvent.WaitOne();

            this.generalsettingInitialCompleteEvent.WaitOne();

            // 初始化报价仓储
            quoteReps.InitialRepository(service.GetAllQuotePrice());

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitQuoteAction end");

            this.quoteInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化报价组合动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitQuoteGroupAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitQuoteGroupAction begin");

            // 获取报价组合仓储
            var quoteGroupRep = this.GetRepository<IQuoteGroupCacheRepository>();

            // 实例化报价组合服务
            var service = this.GetSevice<QuoteGroupService>();

            // 初始化报价组合仓储
            quoteGroupRep.InitialRepository(service.FindAll());

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitQuoteGroupAction end");

            this.quotegroupInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化用户角色动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitRoleAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitRoleAction begin");

            // 获取角色仓储
            var reps = this.GetRepository<IRoleModelCasheRepository>();

            // 实例化角色服务
            var service = this.GetSevice<RoleService>();

            // 初始化角色仓储
            reps.InitialRepository(service.FindByRole(this.currRunTime.CurrentStaff.StaffBaseInfo.Role));

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitRoleAction end");

            this.roleInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化交割节假日动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitSettlementHolidayAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitSettlementHolidayAction begin");

            // 获取交割节假日仓储
            var reps = this.GetRepository<ISettlementHolidayCacheRepository>();

            // 实例化交割节假日服务
            var service = this.GetSevice<SettlementHolidayService>();

            // 初始化交割节假日仓储
            reps.InitialRepository(service.FindAll());

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitSettlementHolidayAction end");

            this.settlementholidayInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化员工用户仓储,这里只查询用户类型是Virtual Dealer的员工用户动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitStaffAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitStaffAction begin");

            // 备注：该初始化必须在CustomerGroup初始化之后
            // 根据登录的Staff找到其管辖的所有CustomerGroup，再找到这些CustomerGroup所属的所有BusinessUnit，
            // 再根据BusinessUnit找到下面所有员工用户类型是Virtual Dealer的员工用户
            this.custgroupInitialCompleteEvent.WaitOne();

            // staff管辖的账户组
            IEnumerable<string> businessUnitIdList =
                this.GetRepository<ICustomerGroupCacheRepository>().Filter(o => true).Select(p => p.BusinessUnitID);

            var reps = this.GetRepository<IStaffCacheRepository>();
            var service = this.GetSevice<StaffService>();

            // 找到登录的Staff可选的BusinessUnit旗下所有Virtual Dealer
            var allBusUnitVirtualStaffList = new List<BaseStaffVM>();
            foreach (string businessUnitId in businessUnitIdList)
            {
                allBusUnitVirtualStaffList.AddRange(
                    service.FindVirtualStaffByParams(businessUnitId, StaffTypeEnum.VirtualDealer));
            }

            reps.InitialRepository(allBusUnitVirtualStaffList);

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitStaffAction end");

            this.staffInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化商品动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitSymbolAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitSymbolAction begin");

            // 获取商品仓储
            var symbolRep = this.GetRepository<ISymbolCacheRepository>();

            // 实例化商品服务
            var service = this.GetSevice<SymbolService>();

            // 初始化商品仓储
            symbolRep.InitialRepository(service.FindAll());

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitSymbolAction end");

            // 发送初始化完毕事件
            this.symbolInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化Vip设置动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitVipSettingsAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitVipSettingsAction begin");

            // 等待客户加载完毕
            this.customerInitialCompleteEvent.WaitOne();

            var vipSettingsRep = this.GetRepository<IVIPCustSettingCacheRepository>();
            var vipSettingsService = this.GetSevice<VipCustomerService>();
            IList<BaseVIPCustSettingVM> list = vipSettingsService.FindAll();
            var customerRep = this.GetRepository<ICustomerCacheRepository>();

            foreach (BaseVIPCustSettingVM item in list)
            {
                BaseCustomerViewModel cust = customerRep.FindByID(item.CustomerNo);
                if (cust != null)
                {
                    item.BuId = cust.BusinessUnitID;
                    cust.SetVipSettings(item);
                    customerRep.AddOrUpdate(cust);
                }
            }

            vipSettingsRep.InitialRepository(list);

            TraceManager.Info.Write("RepositoryModel", "InitVipSettingsAction end");

            this.vipsettingsInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化MarginCall动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitialMarginCallAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitialMarginCallAction begin");

            // 获取MarginCall仓储
            var maginRep = this.GetRepository<IMarginCallCacheRepository>();

            // 获取授信服务
            var service = this.GetSevice<MarginCallService>();

            List<BaseMarginCallVm> marginCallVms = service.FindAllBelongCurrentStaff();

            var custRep = this.GetRepository<ICustomerCacheRepository>();

            this.customerInitialCompleteEvent.WaitOne();

            foreach (BaseMarginCallVm baseMarginCallVm in marginCallVms)
            {
                baseMarginCallVm.BelongCustomer = custRep.FindByID(baseMarginCallVm.CustoemrNo);
                if (baseMarginCallVm.BelongCustomer == null)
                {
                    TraceManager.Error.WriteAdditional(
                        "MaginCall", 
                        baseMarginCallVm, 
                        "Cant find BelongCustomer for MarginCall");
                }
            }

            // 初始化仓储
            maginRep.InitialRepository(marginCallVms);

            // 添加完成信息到日志
            TraceManager.Info.Write("RepositoryModel", "InitialMarginCallAction end");

            this.margincallInitialCompleteEvent.Set();
        }

        /// <summary>
        /// 初始化员工提醒动作
        /// </summary>
        /// <param name="arg">
        /// 参数
        /// </param>
        [PerformanceStatistic]
        private void InitialStaffEventAction(object arg)
        {
            TraceManager.Info.Write("RepositoryModel", "InitialStaffEventAction begin");

            // 员工事件仓储
            var staffEventReps = this.GetRepository<IStaffEventCacheRepository>();

            // 员工事件服务
            var staffEventService = this.GetSevice<StaffEventService>();

            // 初始化仓储
            staffEventReps.InitialRepository(staffEventService.FindStaffEventByStaffId());

            TraceManager.Info.Write("RepositoryModel", "InitialStaffEventAction end");

            this.staffeventInitialCompleteEvent.Set();
        }

        /// <summary>
        ///     等待全部初始化完毕
        /// </summary>
        private void WaitAllCompleteInitial()
        {
            this.currencyInitialCompleteEvent.WaitOne();
            this.symbolInitialCompleteEvent.WaitOne();
            this.generalsettingInitialCompleteEvent.WaitOne();
            this.custgroupInitialCompleteEvent.WaitOne();
            this.businessunitInitialCompleteEvent.WaitOne();
            this.quotegroupInitialCompleteEvent.WaitOne();
            this.forwardpointInitialCompleteEvent.WaitOne();
            this.settlementholidayInitialCompleteEvent.WaitOne();
            this.countryInitialCompleteEvent.WaitOne();
            this.custcategoryInitialCompleteEvent.WaitOne();
            this.custgradingInitialCompleteEvent.WaitOne();
            this.staffInitialCompleteEvent.WaitOne();
            this.bankaccountInitialCompleteEvent.WaitOne();
            this.defaultbankacctInitialCompleteEvent.WaitOne();
            this.roleInitialCompleteEvent.WaitOne();
            this.permissionsInitialCompleteEvent.WaitOne();
            this.groupInitialCompleteEvent.WaitOne();
            this.bankInitialCompleteEvent.WaitOne();
            this.customerInitialCompleteEvent.WaitOne();
            this.vipsettingsInitialCompleteEvent.WaitOne();
            this.creditInitialCompleteEvent.WaitOne();
            this.quoteInitialCompleteEvent.WaitOne();
            this.localeInitialCompleteEvent.WaitOne();
            this.margincallInitialCompleteEvent.WaitOne();
            this.staffeventInitialCompleteEvent.WaitOne();
        }

        /// <summary>
        ///     延迟初始化
        /// </summary>
        private void WaitAllCompleteLazyInitial()
        {
            this.btbdealInitialCompleteEvent.WaitOne();
            this.hedgedealInitialCompleteEvent.WaitOne();
            this.hedgeaccountInitialCompleteEvent.WaitOne();
            this.dealInitialCompleteEvent.WaitOne();
            this.pendingorderInitialCompleteEvent.WaitOne();
        }

        #endregion
    }
}