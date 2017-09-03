//// <copyright file="TestDate.cs" company="BancLogix">
//// Copyright (c) Banclogix. All rights reserved.
//// </copyright>
//// <author> panglh </author>
//// <date> 2013/10/21 14:15:01 </date>
//// <modify>
////   修改人：myth
////   修改时间：2013年11月7日 19:16:11
////   修改描述：新建 DealingLogViewModel
////   版本：1.0
//// </modify>
//// <review>
////   review人：
////   review时间：
//// </review >
using BaseViewModel;
using Infrastructure.Common.Enums;
using Infrastructure.Data;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DM2.Manager.ViewModels
{
    /// <summary>
    ///  TestDate
    /// </summary>
    public class TestDate : BaseVm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestDate"/> class.
        /// </summary>
        public TestDate()
        {
            this.AddDealList();
            this.AddFeeDetail();
            this.AddDepositWithdraw();
            this.AddPendingOrderVM();
            this.AddCustomerVM();
            this.AddSymbol();
            this.AddQuote();
            this.AddOrderList();
            this.AddDealAndHistory();

            ////AddGroup();
            ////AddHedge();
            ////AddAdHocFee();
            ////AddRequest();
        }

        /// <summary>
        /// AddQuote
        /// </summary>
        public void AddQuote()
        {
            var tem = this.GetRepository<QuoteCacheRepository>();
            TickQuoteModel tm = new TickQuoteModel();
            tm.SymbolName = "RMBAUD";
            tm.TraderAsk = 1.333m;
            tm.TraderBid = 0.3332m;
            tm.SymbolID = "11";
            tm.ForwardPointAsk = 0.2233m;
            tm.ForwardPointBid = 0.3332m;
            tm.ForwardRateAsk = 0.3336m;
            tm.ForwardRateBid = 0.3226m;
            tm.Mid = 12.3m;
            tm.LowBid = 12.3m;
            tm.LowMid = 2.366m;
            tm.HighMid = 2.33666m;
            tm.HighBid = 0.3332m;
            tm.HighAsk = 0.33212m;
            tm.LowAsk = 0.3621m;
            BaseQuoteVM vm = new BaseQuoteVM(tm);
            tem.AddOrUpdate(vm);
        }

        /// <summary>
        /// AddSymbol
        /// </summary>
        public void AddSymbol()
        {
            var tem = this.GetRepository<SymbolCacheRepository>();
            SymbolModel mm = new SymbolModel();
            mm.SymbolName = "RMBAUD";
            mm.SymbolID = "11";
            mm.CCY1 = "RMB";
            mm.CCY2 = "AUD";
            BaseSymbolVM mn = new BaseSymbolVM(mm);
            tem.AddOrUpdate(mn);
        }

        /// <summary>
        /// AddHedge
        /// </summary>
        public void AddHedge()
        {
            var repository = this.GetRepository<IHedgeAccountCacheRepository>();
            HedgeAccountModel sss = new HedgeAccountModel();
            sss.HedgeAccountId = "123";
            sss.HedgeAccountName = "你好";
            sss.HedgeGroupId = "111";
            sss.LastUpdateTime = DateTime.Now;
            sss.BusinessUnitId = "111";
            sss.Email = "dadsdsad";
            sss.InternalAccounts = new List<HedgeInternalAccountModel>(1) { new HedgeInternalAccountModel() };
            HedgeAccountModel sss1 = new HedgeAccountModel();
            sss1.HedgeAccountId = "124";
            sss1.HedgeAccountName = "你好";
            sss1.HedgeGroupId = "111";
            sss1.LastUpdateTime = DateTime.Now;
            sss1.BusinessUnitId = "111";
            sss1.Email = "dadsdsad";
            sss1.InternalAccounts = new List<HedgeInternalAccountModel>(1) { new HedgeInternalAccountModel() };
            HedgeAccountModel sss2 = new HedgeAccountModel();
            sss2.HedgeAccountId = "125";
            sss2.HedgeAccountName = "你好";
            sss2.HedgeGroupId = "111";
            sss2.Email = "dadsdsad";
            sss2.InternalAccounts = new List<HedgeInternalAccountModel>(1) { new HedgeInternalAccountModel() };
            sss2.LastUpdateTime = DateTime.Now;
            sss2.BusinessUnitId = "111";
            BaseHedgeAccountVM vm = new BaseHedgeAccountVM(sss);
            BaseHedgeAccountVM vm1 = new BaseHedgeAccountVM(sss1);
            BaseHedgeAccountVM vm2 = new BaseHedgeAccountVM(sss2);
            repository.AddOrUpdate(vm);
            repository.AddOrUpdate(vm1);
            repository.AddOrUpdate(vm2);
        }

        /// <summary>
        /// AddGroup
        /// </summary>
        public void AddGroup()
        {
            var res = this.GetRepository<IGroupCacheRepository>();
            GroupModel dd = new GroupModel();
            dd.BusinessUnitID = "111";
            dd.GroupID = "111";
            dd.GroupName = "nihao";
            dd.GroupType = GroupTypeEnum.HedgeAccountGroup;
            GroupModel dd1 = new GroupModel();
            dd1.BusinessUnitID = "112";
            dd1.GroupID = "112";
            dd1.GroupName = "nihao";
            dd1.GroupType = GroupTypeEnum.HedgeAccountGroup;
            GroupModel dd2 = new GroupModel();
            dd2.BusinessUnitID = "113";
            dd2.GroupID = "113";
            dd2.GroupName = "nihao";
            dd2.GroupType = GroupTypeEnum.HedgeAccountGroup;
            GroupModel dd3 = new GroupModel();
            dd3.BusinessUnitID = "114";
            dd3.GroupID = "114";
            dd3.GroupName = "nihao";
            dd3.GroupType = GroupTypeEnum.HedgeAccountGroup;
            BaseGroupVM vm = new BaseGroupVM(dd);
            BaseGroupVM vm1 = new BaseGroupVM(dd1);
            BaseGroupVM vm2 = new BaseGroupVM(dd2);
            BaseGroupVM vm3 = new BaseGroupVM(dd3);
            res.AddOrUpdate(vm);
            res.AddOrUpdate(vm1);
            res.AddOrUpdate(vm2);
            res.AddOrUpdate(vm3);
        }

        /// <summary>
        /// AddDealList
        /// </summary>
        public void AddDealList()
        {
            // 输入金额
            int enteramount = 1000000;
            var custReps = this.GetRepository<IDealCacheRepository>();
            DealModel dm = new DealModel();
            dm.ExecutionID = "20140703322";
            dm.CustomerNo = "018888";
            dm.TransactionType = (TransactionTypeEnum)0;
            dm.MasterDealID = "123";
            dm.OpenTime = DateTime.Parse("2015-04-01 00:00:01");
            dm.DealID = "123";
            dm.Symbol = "RMBAUD";
            dm.Comment = "niahaonsdifndsfjhdsfhdkj";
            dm.ValueDate = DateTime.Parse("2015-04-21 00:00:01");
            dm.CCY1Amount = (decimal)100444;
            dm.CCY2Amount = (decimal)1111444;
            dm.OpenTime = DateTime.Now;
            if (enteramount >= (int)dm.CCY1Amount)
            {
                dm.CCY1 = "AUD";
            }
            else if (enteramount >= (int)dm.CCY2Amount)
            {
                dm.CCY1 = "AUD";
            }

            dm.CCY2 = "RMB";
            dm.UserNo = "123456";
            dm.Tenor = TenorEnum.BD;
            dm.Channel = DealChannelEnum.Online;
            dm.OpenRate = (decimal)100;
            dm.Instrument = DealInstrumentEnum.FXForward;
            dm.Status = DealStatusEnum.Open;
            dm.OrderID = "10001001";
            DealFeeDetailModel fe = new DealFeeDetailModel();
            fe.Amount = 120m;
            fe.Currency = "USD";
            fe.FeeType = FeeTypeEnum.TTFee;
            fe.SettlementType = SettlementTypeEnum.Cash;
            dm.Fee = fe;
            BaseDealVM vm = new BaseDealVM(dm);
            custReps.AddOrUpdate(vm);

            DealModel dml = new DealModel();
            dml.ExecutionID = "20140703321";
            dml.DealID = "123";
            dml.MasterDealID = "123";
            BaseDealVM vml = new BaseDealVM(dml);
            custReps.AddOrUpdate(vml);
        }
        
        /// <summary>
        /// AddRequest
        /// </summary>
        public void AddRequest()
        {
            var custReps = this.GetRepository<IRequestCacheRepository>();
            RequestModel r1requestmodel = new RequestModel();
            r1requestmodel.RequestID = "000001";
            r1requestmodel.RequestTime = DateTime.Parse("2015-01-01 00:00:01");
            var r2 = new RequestModel();
            r1requestmodel.RequestID = "000002";
            r1requestmodel.RequestTime = DateTime.Parse("2015-01-02 02:00:01");
            BaseRequestVM vm = new BaseRequestVM(r1requestmodel);
            var vm2 = new BaseRequestVM(r2);
            custReps.AddOrUpdate(vm);
            custReps.AddOrUpdate(vm2);
        }

        /// <summary>
        /// AddFeeDetail
        /// </summary>
        public void AddFeeDetail()
        {
            var fee = this.GetRepository<IFeeDetailModelRepository>();
            FeeDetailModel feedail = new FeeDetailModel();
            feedail.ActualAmount = 3.6523M;
            feedail.Amount = 123.263M;
            feedail.Currency = "12333333";
            feedail.FeeType = FeeTypeEnum.CashHandlingFee;
            feedail.InternalAccountNo = "201203013";
            BaseFeeVM vm = new BaseFeeVM(feedail);
            fee.AddOrUpdate(vm);
        }

        /// <summary>
        /// AddDepositWithdraw
        /// </summary>
        public void AddDepositWithdraw()
        {
            var dep = this.GetRepository<IDepositWithdrawCacheRepository>();
            DepositWithdrawModel depm = new DepositWithdrawModel();
            DepositWithdrawModel depm1 = new DepositWithdrawModel();
            DepositWithdrawModel.WithdrawDepoDetailModel aa = new DepositWithdrawModel.WithdrawDepoDetailModel();
            aa.Amount = 10m;
            aa.BeneficiaryAcct = "dddddd";
            aa.Comment = "你好啊 ";
            aa.SettlementTime = DateTime.Now;
            aa.PaymentBankAcct = "KVB12NJXNSJNJ";
            aa.ReceiptBankAcct = "KVB6666";
            depm.DetailList.Add(aa);
            DepositWithdrawModel.WithdrawDepoDetailModel aa1 = new DepositWithdrawModel.WithdrawDepoDetailModel();
            aa1.BeneficiaryAcct = "321223321322321";
            aa1.Amount = 10000m;
            aa1.Comment = "你你不好啊好啊 ";
            aa1.SettlementTime = DateTime.Now;
            aa1.PaymentBankAcct = "KVB1233333SNJXNSJNJ";
            aa1.ReceiptBankAcct = "1213afsafsaKVB6666";
            depm.DetailList.Add(aa1);
           
            DepositWithdrawModel.WithdrawDepoDetailModel aa2 = new DepositWithdrawModel.WithdrawDepoDetailModel();
            aa2.Amount = 10m;
            aa2.BeneficiaryAcct = "dddddd";
            aa2.Comment = "你好啊 ";
            aa2.SettlementTime = DateTime.Now;
            aa2.PaymentBankAcct = "KVB12NJXNSJNJ";
            aa2.ReceiptBankAcct = "KVB6666";
            depm1.DetailList.Add(aa2);
            
            DepositWithdrawModel.WithdrawDepoDetailModel aa12 = new DepositWithdrawModel.WithdrawDepoDetailModel();
            aa12.BeneficiaryAcct = "321223321322321";
            aa12.Amount = 10000m;
            aa12.Comment = "你你不好啊好fsdfsdfdsfsd啊 ";
            aa12.SettlementTime = DateTime.Now;
            aa12.PaymentBankAcct = "KVB1233333SNJXNSJNJ";
            aa12.ReceiptBankAcct = "1213afsafsaKVB6666";
            depm1.DetailList.Add(aa12);
            FeeDetailModel feedail = new FeeDetailModel();
            feedail.ActualAmount = 3.6523M;
            feedail.Amount = 123.263M;
            feedail.Currency = "0";
            feedail.FeeType = FeeTypeEnum.CashHandlingFee;
            feedail.InternalAccountNo = "201203013";
            feedail.SettlementType = SettlementTypeEnum.Cash;

            depm.FeeList.Add(feedail);
            depm.RelatedDealID = "20140703322";
            depm.InternalAcctNo = "111";
            depm.StaffID = "12333333";
            depm.TransID = "1";
            depm.BusinessUnitID = "20140703322";
            depm.Currency = "0";
            ////depm.Type = WithdrawDepoTypeEnum.Withdrawal;
            BaseDepositWithdrawVM depvm = new BaseDepositWithdrawVM(depm);
            depm1.DetailList.Add(aa1);
            depm1.RelatedDealID = "20140703322";
            depm1.InternalAcctNo = "111";
            depm1.StaffID = "12333333";
            depm1.TransID = "1";
            depm1.BusinessUnitID = "20140703322";
            depm1.Currency = "0";
            ////depm1.Type = WithdrawDepoTypeEnum.Deposit;
            BaseDepositWithdrawVM depvm2 = new BaseDepositWithdrawVM(depm1);
            dep.AddOrUpdate(depvm);
            dep.AddOrUpdate(depvm2);
        }

        /// <summary>
        /// AddCustomerVM
        /// </summary>
        public void AddCustomerVM()
        {
            var cu = this.GetRepository<CustomerCacheRepository>();
            BaseCustomerViewModel custVM = new BaseCustomerViewModel();
            CustomerModel cu = new CustomerModel();
            custVM.CustomerName = "你好";
            custVM.CustmerNo = "123";
            custVM.InternalAcctNo = "111";
            cu.BaseInfo.Country = "zhongguo";
            cu.BaseInfo.CustomerNo = "111";
            List<CustInternalAcctModel> list = new List<CustInternalAcctModel>();
            CustInternalAcctModel cc = new CustInternalAcctModel();
            cc.Type = InternalAccountTypeEnum.SettlementCash;
            cc.InternalAcctNo = "111";
            cc.CurrencyID = "111";
            cc.CreationTime = DateTime.Now;
            CustInternalAcctModel cc1 = new CustInternalAcctModel();
            cc1.Type = InternalAccountTypeEnum.CollateralAccount;
            cc1.InternalAcctNo = "111";
            cc1.CurrencyID = "112";
            cc1.CreationTime = DateTime.Now;
            list.Add(cc);
            list.Add(cc1);
            cu.SettlementAccounts = list;
            custVM.Account = cu;
            cus.AddOrUpdate(custVM);
        }

       /// <summary>
       /// 添加挂单数据
       /// </summary>
        public void AddPendingOrderVM()
        {
            var orderReps = this.GetRepository<IOrderCacheRepository>();
            PendingOrderModel pp = new PendingOrderModel();
            pp.CCY1 = "AUD";
            pp.CCY2 = "NZD";
            pp.CCY1Amount = 2.3333m;
            pp.CCY2Amount = 2.3312m;
            pp.Channel = PendingChannelEnum.Online;
            pp.CreationTime = DateTime.Now;
            pp.ExecutionID = "12356563363";
            pp.ExecutionType = PendingExecuteTypeEnum.Limit;
            pp.OrderID = "1232323";
            pp.OrderRate = 2.3332m;
            pp.OrderType = OrderTypeEnum.Stop;
            PendingOrderModel pp1 = new PendingOrderModel();
            pp1.CCY1 = "AUD";
            pp1.CCY2 = "NZD";
            pp1.CCY1Amount = 2.3333m;
            pp1.CCY2Amount = 2.3312m;
            pp1.Channel = PendingChannelEnum.Online;
            pp1.CreationTime = DateTime.Now;
            pp1.ExecutionID = "12356563363";
            pp1.ExecutionType = PendingExecuteTypeEnum.Limit;
            pp1.OrderID = "1232323";
            pp1.OrderRate = 2.3332m;
            pp1.CustomerNo = "123456";
            pp1.OrderType = OrderTypeEnum.Stop;
            BaseOrderVM ovm = new BaseOrderVM(pp);
            BaseOrderVM vv = new BaseOrderVM(pp1);
            orderReps.AddOrUpdate(ovm);
            orderReps.AddOrUpdate(vv);
        }

        /// <summary>
        /// AddAdHocFee
        /// </summary>
        public void AddAdHocFee()
        {
            var pp = this.GetRepository<IAdDocFeeCacheRepository>();
            AdHocFeeModel fee = new AdHocFeeModel();
            fee.Amount = 20m;
            fee.CreationTime = DateTime.Now;
            fee.CurrencyID = "0";
            fee.CustomerNo = "111";
            fee.TransID = "1";
            fee.InternalAcctNo = "111";
            fee.PaymentMethod = AdHocFeePayTypeEnum.SettlementAccount;
            fee.Instrument = AdHocFeeInstrumentEnum.Commission;
            fee.ValueDate = DateTime.Now;
            BaseAdHocFeeVM vm = new BaseAdHocFeeVM(fee);
            pp.AddOrUpdate(vm);
        }

        /// <summary>
        /// AddOrderList
        /// </summary>
        public void AddOrderList()
        {
            var ordercacherepository = new OrderCacheRepository(this.OwnerId);

            PendingOrderModel pom = new PendingOrderModel();
            pom.OrderID = "123";
            pom.CustomerNo = "345";
            pom.OrderRate = (decimal)456;
            pom.ExecutionID = "1234";
            pom.ExecutionType = PendingExecuteTypeEnum.Stop;
            pom.ExpiryTime = DateTime.Now;
            pom.Instrument = DealInstrumentEnum.FXForward;
            pom.LastUpdateTime = DateTime.Now;
            pom.MasterOrderID = "789";
            pom.OpenRate = 123;
            pom.OpenTime = DateTime.Now;
            pom.OrderID = "789";
            pom.OrderRate = (decimal)5.621;
            pom.OrderType = OrderTypeEnum.IfDone;
            pom.PerOrderPosition = (decimal)1.235;
            pom.PerOrderPositionCONVRate = (decimal)3.3354;
            pom.ProfitByCustBuyCCY = (decimal)5.3354;
            pom.ProfitByLocalCCY = (decimal)6.1235;
            pom.ProfitByUSD = (decimal)6.15;
            pom.RelatedOrderID = "1566256";
            pom.SalesDesk = SalesDeskEnum.JapaneseGroup;
            pom.StaffGroup = "54212";
            pom.Status = PendingStatusEnum.Cancelled;
            pom.Symbol = "10203";
            pom.SysComment = "5020";
            pom.TimeInForce = TimeinForceEnum.GTC;
            pom.TraderSpotRate = (decimal)36.15;
            pom.TransactionType = TransactionTypeEnum.Sell;
            pom.UserNo = "6446";
            pom.CustomerTyping = CustomerTradeTypingEnum.CCY1Amount;
            pom.CustomerSpread = 0;
            pom.CreationTime = DateTime.Now;
            pom.CommissionDeductedSide = CommissionSideEnum.CustomerSellAmount;
            pom.Comment = "wwwwdddwd";
            pom.Channel = PendingChannelEnum.OTC;
            pom.CCY2Amount = 12345;
            pom.CCY2 = "USB";
            pom.CCY1Amount = 354985;
            pom.CCY1 = "JPY";
            BaseOrderVM bovm = new BaseOrderVM(pom);
            ordercacherepository.AddOrUpdate(bovm);

            PendingOrderModel po = new PendingOrderModel();
            po.MasterOrderID = "789";
            po.OrderID = "789";
            po.ExecutionID = "12384";
            BaseOrderVM bo = new BaseOrderVM(po);
            ordercacherepository.AddOrUpdate(bo);
        }

        /// <summary>
        /// AddDealAndHistory
        /// </summary>
        public void AddDealAndHistory()
        {
            var hedgeDealCacheRepository = new HedgeDealCacheRepository(this.OwnerId);
            HedgeDealModel hd = new HedgeDealModel();
            hd.BusinessUnit = "20000";
            hd.CCY1 = "USD";
            hd.CCY1Amount = 5544;
            hd.CCY2 = "JPY";
            hd.CCY2Amount = 25655;
            hd.ExternalDealID = "556649";
            hd.ExternalDealSetID = "55652365";
            hd.ForwardPoint = (decimal)3.4862456;
            hd.HedgeBankExecutionID = "652";
            hd.Instrument = HedgeInstrumentEnum.FXSpotHedging;
            hd.LastUpdateTime = DateTime.Parse("2015-7-1");
            hd.LocalTradeDate = DateTime.Now;
            hd.MasterDealID = "545";
            hd.OpenRate = 6985556;
            hd.OrderType = HedgeOrderTypeEnum.MarketOrder;
            hd.RelatedDealID = "3265";
            hd.Symbol = "CCY";
            hd.SystemComment = "425466";
            hd.StaffID = "123";
            hd.HedgeAccountNo = "123";
            hd.ExecutionID = "234";
            hd.TransactionType = 0;
            hd.HedgeBankDealID = "7745";
            hd.OpenTime = DateTime.Parse("2015-3-1");
            hd.DealID = "545";
            hd.ForwardPoint = (decimal)0.123;
            hd.ValueDate = hd.OpenTime = DateTime.Parse("2015-5-1");
            hd.CCY2Amount = (decimal)456;
            hd.CCY1Amount = (decimal)156;
            hd.SpotRate = (decimal)1.560;
            hd.OpenRate = (decimal)1.520;
            hd.Instrument =HedgeInstrumentEnum.FXSpotHedging;
            hd.Status =HedgeDealStatusEnum.Open;
            hd.Comment = "0";
            BaseHedgeDealVM bh = new BaseHedgeDealVM(hd);
            hedgeDealCacheRepository.AddOrUpdate(bh);

            HedgeDealModel hdm = new HedgeDealModel();
            hd.DealID = "545";
            hd.ExecutionID = "222234";
            hd.MasterDealID = "545";
            BaseHedgeDealVM bhvm = new BaseHedgeDealVM(hdm);
            hedgeDealCacheRepository.AddOrUpdate(bhvm);
        }
    }
}