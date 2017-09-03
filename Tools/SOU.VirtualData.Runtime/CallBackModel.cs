// <copyright file="CallBackModel.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2014/2/24 11:10:07 </date>
// <summary> 推送处理类 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2014/2/24 11:10:07
//      修改描述：新建 CallBackModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace DM2.Manager.Models
{
    #region

    using BaseViewModel;
    using DestributeService.Dto;
    using DestributeService.Seedwork;
    using Infrastructure.Common;
    using Infrastructure.Common.Enums;
    using Infrastructure.Data;
    using Infrastructure.Log;
    using Infrastructure.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    #endregion

    /// <summary>
    /// 推送处理类
    /// </summary>
    public partial class CallBackModel : BaseModel
    {
        #region Fields

        /// <summary>
        /// 推送处理者字典
        /// </summary>
        private readonly Dictionary<string, Action<PushTypeEnum, string>> callbackHandlerMap =
            new Dictionary<string, Action<PushTypeEnum, string>>();

        /// <summary>
        /// 由于Swap的推送每次只推送一张订单，这里暂存订单，一起显示创建Swap成功信息
        /// </summary>
        private readonly Dictionary<string, DealModel> swapDealTemp;

        /// <summary>
        /// 当前RunTime
        /// </summary>
        private readonly RunTime currRunTime;

        /// <summary>
        /// 货币仓储
        /// </summary>
        private readonly ICurrencyCacheRepository currencyRep;

        /// <summary>
        /// 商品仓储
        /// </summary>
        private readonly ISymbolCacheRepository symbolRep;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CallBackModel"/> class.
        /// </summary>
        /// <param name="currRunTime">
        /// 运行时对象
        /// </param>
        /// <param name="ownerId">
        /// 拥有者ID
        /// </param>
        public CallBackModel(RunTime currRunTime, string ownerId)
            : base(ownerId)
        {
            this.currRunTime = currRunTime;
            this.currencyRep = this.GetRepository<ICurrencyCacheRepository>();
            this.symbolRep = this.GetRepository<ISymbolCacheRepository>();
            this.swapDealTemp = new Dictionary<string, DealModel>();
            this.InitialMethodMap();
        }

        #endregion

        #region Delegates

        /// <summary>
        /// 显示信息
        /// </summary>
        private delegate void ShowInfo();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 接受Router的推送
        /// </summary>
        /// <param name="actionType">
        /// 推送的类型
        /// </param>
        /// <param name="contentData">
        /// 推送的数据
        /// </param>
        /// <param name="contentType">
        /// DTO类型
        /// </param>
        public void PushBack(PushTypeEnum actionType, string contentData, string contentType)
        {
            // 推送记录日志
            TraceManager.Info.Write("Searchpush推送", "{0}", "PushType : " + actionType + "; DtoType : " + contentType + "; DtoData ：" + JsonConvert.ToString(contentData) + ".");

            try
            {
                if (this.callbackHandlerMap.ContainsKey(contentType))
                {
                    this.callbackHandlerMap[contentType](actionType, contentData);
                }
                else
                {
                    TraceManager.Warn.WriteAdditional("Searchpush推送", "{0}", "No handler for DtoType : " + contentType);
                }
            }
            catch (Exception exception)
            {
                TraceManager.Error.WriteAdditional(
                    "Searchpush",
                    "{0}",
                    exception,
                    "Error while hangle push back for actionType:{0},  contentData:{1}, contentType:{2}",
                    actionType,
                    contentData,
                    contentType);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 添加或者更新费用单
        /// </summary>
        /// <param name="dtoHedgeDeal">
        /// The dto Hedge Deal.
        /// </param>
        private void AddAndUpdateAdHocFee(DtoAdHocFee dtoHedgeDeal)
        {
            if (dtoHedgeDeal != null)
            {
                BaseAdHocFeeVM baseVm = new BaseAdHocFeeVM((AdHocFeeModel)dtoHedgeDeal);
                this.GetRepository<IAdHocFeeCacheRepository>().AddOrUpdate(baseVm);
            }
        }

        /// <summary>
        /// 添加或者更新内部交易
        /// </summary>
        /// <param name="btbDeal">
        /// 内部交易Dto
        /// </param>
        private void AddAndUpdateBTBDeal(DtoBTBDeal btbDeal)
        {
            if (btbDeal == null)
            {
                return;
            }

            BaseBTBDealVM baseBTBDeal = new BaseBTBDealVM((BTBDealModel)btbDeal);

            // 添加或更新
            this.GetRepository<IBTBDealCacheRepository>().AddOrUpdate(baseBTBDeal);
        }

        /// <summary>
        /// 推送 添加授信
        /// </summary>
        /// <param name="cedit">
        /// 授信Dto
        /// </param>
        private void AddAndUpdateCredit(DtoCredit cedit)
        {
            if (cedit == null)
            {
                return;
            }

            BaseCreditVM baseCreditVM = new BaseCreditVM((CreditModel)cedit);

            // 添加或更新
            this.GetRepository<ICreditCacheRepository>().AddOrUpdate(baseCreditVM);
        }

        /// <summary>
        /// 推送 交易单
        /// </summary>
        /// <param name="deal">
        /// 交易Dto
        /// </param>
        private void AddAndUpdateDeal(DealModel deal)
        {
            if (deal == null)
            {
                return;
            }

            BaseDealVM baseDealVM = new BaseDealVM(deal);

            // 添加或更新
            this.GetRepository<IDealCacheRepository>().AddOrUpdate(baseDealVM);
        }

        /// <summary>
        /// 推送 出入金单
        /// </summary>
        /// <param name="depositWithdraw">
        /// 出入金单Dto
        /// </param>
        private void AddAndUpdateDepositWithdraw(DtoDepositWithdraw depositWithdraw)
        {
            if (depositWithdraw == null)
            {
                return;
            }

            BaseDepositWithdrawVM baseDepositWithdraw = new BaseDepositWithdrawVM((DepositWithdrawModel)depositWithdraw);

            // 添加或更新
            this.GetRepository<IDepositWithdrawCacheRepository>().AddOrUpdate(baseDepositWithdraw);
        }

        /// <summary>
        /// 添加或者更新对冲账户
        /// </summary>
        /// <param name="dtoHedge">
        /// 对冲账户Dto
        /// </param>
        private void AddAndUpdateHedgeAccount(DtoHedgeAccount dtoHedge)
        {
            if (dtoHedge != null)
            {
                BaseHedgeAccountVM baseVm = new BaseHedgeAccountVM((HedgeAccountModel)dtoHedge);
                this.GetRepository<IHedgeAccountCacheRepository>().AddOrUpdate(baseVm);
            }
        }

        /// <summary>
        /// 添加或者更新对冲交易
        /// </summary>
        /// <param name="dtoHedgeDeal">
        /// The dto Hedge Deal.
        /// </param>
        private void AddAndUpdateHedgeDeal(DtoHedgeDeal dtoHedgeDeal)
        {
            if (dtoHedgeDeal != null)
            {
                BaseHedgeDealVM baseVm = new BaseHedgeDealVM((HedgeDealModel)dtoHedgeDeal);
                this.GetRepository<IHedgeDealCacheRepository>().AddOrUpdate(baseVm);
            }
        }

        /// <summary>
        /// 添加或者更新内部转账
        /// </summary>
        /// <param name="dtoInternalTransDeal">
        /// The dto InternalAcctTransfer Deal.
        /// </param>
        private void AddAndUpdateInternalAcctTransfer(DtoInternalAcctTransfer dtoInternalTransDeal)
        {
            if (dtoInternalTransDeal != null)
            {
                BaseInternalAcctTransferVM baseVm = new BaseInternalAcctTransferVM((InternalAcctTransferModel)dtoInternalTransDeal);
                this.GetRepository<IInternalAcctTransferCacheRepository>().AddOrUpdate(baseVm);
            }
        }

        /// <summary>
        /// 添加或者更新PendingOrder挂单
        /// </summary>
        /// <param name="pendingorder">
        /// 挂单Dto
        /// </param>
        private void AddAndUpdatePendingOrder(PendingOrderModel pendingorder)
        {
            if (pendingorder == null)
            {
                return;
            }

            BaseOrderVM baseDepositWithdraw = new BaseOrderVM(pendingorder);

            // 添加或更新
            this.GetRepository<IOrderCacheRepository>().AddOrUpdate(baseDepositWithdraw);
        }

        /// <summary>
        /// 添加或修改vip setting
        /// </summary>
        /// <param name="dtoVIPCustSettings">
        /// </param>
        private void AddAndUpdateVipSettings(DtoVIPCustSettings dtoVIPCustSettings)
        {
            if (dtoVIPCustSettings == null)
            {
                return;
            }

            BaseVIPCustSettingVM baseVipSettingVM = new BaseVIPCustSettingVM((VIPCustSettingsModel)dtoVIPCustSettings);

            // 添加或更新
            this.GetRepository<IVIPCustSettingCacheRepository>().AddOrUpdate(baseVipSettingVM);
            var custRep = this.GetRepository<ICustomerCacheRepository>();
            var customer = custRep.FindByID(baseVipSettingVM.CustomerNo);
           customer.SetVipSettings(baseVipSettingVM);
            custRep.AddOrUpdate(customer);
        }

        /// <summary>
        /// 推送 添加账户
        /// </summary>
        /// <param name="customer">
        /// 账户Dto
        /// </param>
        private void AddCustomer(DtoCustomer customer)
        {
            if (
                !this.currRunTime.CurrentStaff.StaffBaseInfo.CustomerGroup.Any(
                    x => x.Contains(customer.BaseInfo.CustomerGroupID)))
            {
                return;
            }

            // 获取账户仓储
            var custReps = this.GetRepository<ICustomerCacheRepository>();
            BaseCustomerViewModel baseCustVM = new BaseCustomerViewModel((CustomerModel)customer);
            var vipRep = this.GetRepository<IVIPCustSettingCacheRepository>();
            var vip = vipRep.FindByID(baseCustVM.CustmerNo);
            if (vip != null)
            {
                baseCustVM.SetVipSettings(vip);
            }

            // 添加或更新
            custReps.AddOrUpdate(baseCustVM);
        }

        /// <summary>
        /// 添加交易日志
        /// </summary>
        /// <param name="dto">
        /// The dto.
        /// </param>
        private void AddDealingLog(DtoDealingLog dto)
        {
            if (dto == null)
            {
                return;
            }

            var model = new BaseDealingLogVM((DealingLogModel)dto);

            var currentStaff = RunTime.GetCurrentRunTime().CurrentStaff.StaffBaseInfo;

            var customer = this.GetRepository<ICustomerCacheRepository>().FindByID(model.CustomerID);
            if (customer == null)
            {
                return;
            }

            if (!currentStaff.CustomerGroup.Contains(customer.CustGroupID, StringComparer.CurrentCultureIgnoreCase))
            {
                return;
            }

            model.StaffID = dto.StaffID;

            this.GetRepository<IDealingLogCacheRepository>().AddOrUpdate(model);
        }

        /// <summary>
        /// 删除费用单
        /// </summary>
        /// <param name="dtoHedgeDeal">
        /// The dto Hedge Deal.
        /// </param>
        private void DeleteAdHocFee(DtoAdHocFee dtoHedgeDeal)
        {
            // TODO：目前删除也只是更新状态所以临时也走AddAndUpdateHedgeDeal方法
            if (dtoHedgeDeal != null)
            {
                BaseAdHocFeeVM baseVm = new BaseAdHocFeeVM((AdHocFeeModel)dtoHedgeDeal);
                this.GetRepository<IAdHocFeeCacheRepository>().AddOrUpdate(baseVm);
            }
        }

        /// <summary>
        /// 删除对冲交易
        /// </summary>
        /// <param name="dtoHedgeDeal">
        /// The dto Hedge Deal.
        /// </param>
        private void DeleteHedgeDeal(DtoHedgeDeal dtoHedgeDeal)
        {
            // TODO：目前删除也只是更新状态所以临时也走AddAndUpdateHedgeDeal方法
            if (dtoHedgeDeal != null)
            {
                BaseHedgeDealVM baseVm = new BaseHedgeDealVM((HedgeDealModel)dtoHedgeDeal);
                this.GetRepository<IHedgeDealCacheRepository>().AddOrUpdate(baseVm);
            }
        }

        /// <summary>
        /// 删除内部转账
        /// </summary>
        /// <param name="dtoInternalTransDeal">
        /// The dto InternalAcctTransfer Deal.
        /// </param>
        private void DeleteInternalAcctTransfer(DtoInternalAcctTransfer dtoInternalTransDeal)
        {
            if (dtoInternalTransDeal != null)
            {
                BaseInternalAcctTransferVM baseVm = new BaseInternalAcctTransferVM((InternalAcctTransferModel)dtoInternalTransDeal);
                this.GetRepository<IInternalAcctTransferCacheRepository>().AddOrUpdate(baseVm);
            }
        }

        /// <summary>
        /// 移除vip Setting
        /// </summary>
        /// <param name="dtoVIPCustSettings">
        /// </param>
        private void DeleteVipSettings(DtoVIPCustSettings dtoVIPCustSettings)
        {
            if (dtoVIPCustSettings == null)
            {
                return;
            }

            var vip = this.GetRepository<IVIPCustSettingCacheRepository>().FindByID(dtoVIPCustSettings.CustomerNo);
            if (vip != null)
            {
                this.GetRepository<IVIPCustSettingCacheRepository>().Remove(vip);
                var custRep = this.GetRepository<ICustomerCacheRepository>();
                var customer = custRep.FindByID(vip.CustomerNo);
                customer.SetVipSettings(null);
                custRep.AddOrUpdate(customer);
            }
        }

        /// <summary>
        /// 处理客户外部账户的推送
        /// </summary>
        /// <param name="backContextType">
        /// 修改类型
        /// </param>
        /// <param name="dtoData">
        /// 推送数据
        /// </param>
        /// <param name="dtoType">
        /// 推送类型
        /// </param>
        private void HandleCustBenefiAcctPush(PushTypeEnum backContextType, string dtoData, string dtoType)
        {
            if (dtoType.Equals(typeof(DtoCustomerBeneficiaryAccount).Name))
            {
                if (backContextType == PushTypeEnum.Create)
                {
                    var varBenefyAct = JsonConvert.DeserializeObject<DtoCustomerBeneficiaryAccount>(dtoData);
                    if (varBenefyAct != null)
                    {
                        var varBenefyActModel = (CustBeneficiaryAcctModel)varBenefyAct;

                        // 获取账户仓储
                        var custReps = this.GetRepository<ICustomerCacheRepository>();
                        var baseCustVm = custReps.FindByID(varBenefyAct.CustomerNo);
                        baseCustVm.BeneficiaryAccounts.Add(varBenefyActModel);

                        // 添加或更新
                        custReps.AddOrUpdate(baseCustVm);
                    }
                }
                else if (backContextType == PushTypeEnum.Update)
                {
                    var varBenefyAct = JsonConvert.DeserializeObject<DtoCustomerBeneficiaryAccount>(dtoData);
                    if (varBenefyAct != null)
                    {
                        var varBenefyActModel = (CustBeneficiaryAcctModel)varBenefyAct;

                        // 获取账户仓储
                        var custReps = this.GetRepository<ICustomerCacheRepository>();
                        var baseCustVm = custReps.FindByID(varBenefyAct.CustomerNo);
                        var beneficaryAct =
                            baseCustVm.BeneficiaryAccounts.Find(c => c.AccountId == varBenefyAct.AccountId);
                        EmitMap.Map(varBenefyActModel, beneficaryAct);

                        // 添加或更新
                        custReps.AddOrUpdate(baseCustVm);
                    }
                }
            }
        }

        /// <summary>
        /// 处理客户用户的推送
        /// </summary>
        /// <param name="backContextType">
        /// 修改类型
        /// </param>
        /// <param name="dtoData">
        /// 推送数据
        /// </param>
        /// <param name="dtoType">
        /// 推送类型
        /// </param>
        private void HandleCustUserPush(PushTypeEnum backContextType, string dtoData, string dtoType)
        {
            if (dtoType.Equals(typeof(DtoCustomerUser).Name))
            {
                var varCustUserDto = JsonConvert.DeserializeObject<DtoCustomerUser>(dtoData);
                if (varCustUserDto != null)
                {
                    if (backContextType == PushTypeEnum.Create)
                    {
                        var varCustUserModel = (CustomerUserModel)varCustUserDto;

                        // 获取账户仓储
                        var custReps = this.GetRepository<ICustomerCacheRepository>();
                        var baseCustVm = custReps.FindByID(varCustUserDto.CustomerBaseInfo.CustomerNo);
                        baseCustVm.CustAcctUserList.Add(new BaseCustUserVM(varCustUserModel));

                        // 添加或更新
                        custReps.AddOrUpdate(baseCustVm);
                    }
                    else if (backContextType == PushTypeEnum.Update)
                    {
                        var varCustUserModel = (CustomerUserModel)varCustUserDto;

                        // 获取账户仓储
                        var custReps = this.GetRepository<ICustomerCacheRepository>();
                        var baseCustVm = custReps.FindByID(varCustUserDto.CustomerBaseInfo.CustomerNo);
                        var custUserVm =
                            baseCustVm.CustAcctUserList.First(
                                c => c.CustUserModel.CustomerBaseInfo.UserNo == varCustUserModel.CustomerBaseInfo.UserNo);
                        custUserVm.Copy(new BaseCustUserVM(varCustUserModel));

                        // 添加或更新
                        custReps.AddOrUpdate(baseCustVm);
                    }
                }
            }
        }

        /// <summary>
        /// 处理MarginCall的推送
        /// </summary>
        /// <param name="backContextType">
        /// 推送类型
        /// </param>
        /// <param name="dtoData">
        /// 推送内容
        /// </param>
        private void HandleMarginCallPushBack(PushTypeEnum backContextType, string dtoData)
        {
            var dtomargincall = JsonConvert.DeserializeObject<DtoMarginCall>(dtoData);
            if (dtomargincall == null)
            {
                Infrastructure.Log.TraceManager.Error.WriteAdditional(
                    "MarginCall",
                    dtoData,
                    "无法将推送的DtoDate反序列化为DtoMarginCall，推送类型：{0}",
                    backContextType);
                return;
            }

            var marginModel = (MarginCallModel)dtomargincall;
            if (marginModel == null)
            {
                Infrastructure.Log.TraceManager.Error.WriteAdditional(
                    "MarginCall",
                    dtomargincall,
                    "无法将推送的DtoMarginCall转换为MarginCallModel，推送类型：{0}",
                    backContextType);
                return;
            }

            var marginVm = new BaseMarginCallVm(marginModel);
            var marginRep = this.GetRepository<IMarginCallCacheRepository>();

            marginVm.BelongCustomer = this.GetRepository<ICustomerCacheRepository>().FindByID(marginVm.CustoemrNo);
            if (marginVm.BelongCustomer == null)
            {
                TraceManager.Warn.WriteAdditional("MaginCall", marginVm, "推送MarginCall后，找不到MarginCall所属的Custoemr,继绝。");
                return;
            }

            switch (backContextType)
            {
                case PushTypeEnum.Create:
                    marginRep.AddOrUpdate(marginVm);
                    this.currRunTime.AddMarginCallList(marginVm.CustoemrNo);
                    break;
                case PushTypeEnum.Update:
                    marginRep.AddOrUpdate(marginVm);
                    break;
                case PushTypeEnum.Delete:
                    marginRep.Remove(marginVm);
                    this.currRunTime.RemoveFromMarginCallList(marginVm.CustoemrNo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("backContextType");
            }
        }

        /// <summary>
        /// 初始化推送处理字典
        /// </summary>
        private void InitialMethodMap()
        {
            var allPrivateMethds = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var handleMethod in allPrivateMethds)
            {
                var attribute =
                    handleMethod.GetCustomAttributes(typeof(CallBackHandleAttribute), false).FirstOrDefault();
                if (attribute == null)
                {
                    continue;
                }

                var targetAttribute = attribute as CallBackHandleAttribute;
                if (targetAttribute == null)
                {
                    continue;
                }

                var handlerType = targetAttribute.HangleType;
                if (this.callbackHandlerMap.ContainsKey(handlerType))
                {
                    Infrastructure.Log.TraceManager.Warn.Write("CallBack", "Callback type:{0} handler define repeat.");
                    continue;
                }

                MethodInfo method = handleMethod;
                Action<PushTypeEnum, string> action =
                    (pushType, data) => method.Invoke(this, new object[] { pushType, data });

                this.callbackHandlerMap.Add(handlerType, action);
            }
        }

        /// <summary>
        /// 推送 删除授信
        /// </summary>
        /// <param name="cedit">
        /// 授信Dto
        /// </param>
        private void RemoveCredit(DtoCredit cedit)
        {
            if (cedit == null)
            {
                return;
            }

            var creditRep = this.GetRepository<ICreditCacheRepository>();
            var baseCreditVm = new BaseCreditVM((CreditModel)cedit);
            this.GetRepository<ICreditCacheRepository>().Remove(baseCreditVm);
        }

        /// <summary>
        /// 删除 交易单
        /// </summary>
        /// <param name="deal">
        /// 交易Dto
        /// </param>
        private void RemoveDeal(DtoDeal deal)
        {
            if (deal == null)
            {
                return;
            }

            // 删除
            this.GetRepository<IDealCacheRepository>().Remove(new BaseDealVM((DealModel)deal));
        }

        /// <summary>
        /// 删除 交易单
        /// </summary>
        /// <param name="deal">
        /// 交易Dto
        /// </param>
        private void RemoveDeal(DealModel deal)
        {
            if (deal == null)
            {
                return;
            }

            // 删除
            this.GetRepository<IDealCacheRepository>().Remove(new BaseDealVM(deal));
        }

        /// <summary>
        /// 删除出入金单
        /// </summary>
        /// <param name="depositWithdraw">
        /// 出入金单Dto
        /// </param>
        private void RemoveDepositWithdraw(DtoDepositWithdraw depositWithdraw)
        {
            if (depositWithdraw == null)
            {
                return;
            }

            var dwDeal = new BaseDepositWithdrawVM((DepositWithdrawModel)depositWithdraw);
            if (dwDeal.Status == WithdrawDepoStatusEnum.DeletedPending)
            {
                this.GetRepository<IDepositWithdrawCacheRepository>().AddOrUpdate(dwDeal);
                return;
            }

            // 删除
            this.GetRepository<IDepositWithdrawCacheRepository>().Remove(dwDeal);
        }

        /// <summary>
        /// 删除对冲账户
        /// </summary>
        /// <param name="dtoHedge">
        /// 对冲账户Dto
        /// </param>
        private void RemoveHedgeAccount(DtoHedgeAccount dtoHedge)
        {
            if (dtoHedge == null)
            {
                return;
            }

            this.GetRepository<IHedgeAccountCacheRepository>()
                .Remove(new BaseHedgeAccountVM((HedgeAccountModel)dtoHedge));
        }

        /// <summary>
        /// 挂单删除
        /// </summary>
        /// <param name="pendingOrder">
        /// 挂单Dto
        /// </param>
        private void RemovePendingOrder(DtoPendingOrder pendingOrder)
        {
            if (pendingOrder == null)
            {
                return;
            }

            // 删除
            this.GetRepository<IOrderCacheRepository>().Remove(new BaseOrderVM((PendingOrderModel)pendingOrder));
        }

        /// <summary>
        /// 更新forwardpoint
        /// </summary>
        /// <param name="dtoForwardPointTenor">
        /// The dto Forward Point Tenor.
        /// </param>
        private void UpdateForwardPoint(DtoForwardPointTenor dtoForwardPointTenor)
        {
            var model = EmitMap.Map<DtoForwardPointTenor, ForwardPointTenorModel>(dtoForwardPointTenor);

            var forwardPointVM = this.GetRepository<IForwardPointCacheRepository>().FindByID(model.CurrencyID);
            if (forwardPointVM == null)
            {
                return;
            }

            var propSetModelToner =
                forwardPointVM.PropSet.Tenors.FirstOrDefault(
                    o => o.CurrencyID == model.CurrencyID && o.Tenor == model.Tenor);
            if (propSetModelToner == null)
            {
                return;
            }

            EmitMap.Map(model, propSetModelToner);
            this.GetRepository<IForwardPointCacheRepository>().AddOrUpdate(forwardPointVM);
        }

        #endregion

        /// <summary>
        /// 回调处理方法标识特性
        /// </summary>
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
        public class CallBackHandleAttribute : System.Attribute
        {
            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="CallBackHandleAttribute"/> class.
            /// </summary>
            /// <param name="hangleType">
            /// 处理类型
            /// </param>
            public CallBackHandleAttribute(string hangleType)
            {
                this.HangleType = hangleType;
            }

            #endregion

            #region Public Properties

            /// <summary>
            /// 处理类型
            /// </summary>
            public string HangleType { get; set; }

            #endregion
        }
    }
}