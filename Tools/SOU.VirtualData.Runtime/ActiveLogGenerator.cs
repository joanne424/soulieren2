using DestributeService.Dto;
using Infrastructure.Common;
using Infrastructure.Common.Enums;
using Infrastructure.Data;
using Infrastructure.MainContext;
using Infrastructure.Models;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DM2.Manager.Models
{
    /// <summary>
    /// ActiveLog生成器
    /// </summary>
    public class ActiveLogGenerator
    {
        /// <summary>
        /// ActiveLogGenerator的一个实例
        /// </summary>
        public readonly static ActiveLogGenerator Instance = new ActiveLogGenerator();

        private readonly Lazy<ISymbolCacheRepository> symbolRepository;
        private readonly Lazy<ICurrencyCacheRepository> currencyRepository; 
        private readonly Lazy<IBusinessUnitCacheRepository> buRepository; 

        private ActiveLogGenerator()
        {
            //this.symbolRepository = IOCContainer.Instance.Container.Resolve<ISymbolCacheRepository>();
            var parameter = new ParameterOverrides { { "varOwnerId", string.Empty } };
            this.symbolRepository = new Lazy<ISymbolCacheRepository>(() =>
                IOCContainer.Instance.Container.Resolve<ISymbolCacheRepository>(parameter));
            this.currencyRepository = new Lazy<ICurrencyCacheRepository>(() =>
                IOCContainer.Instance.Container.Resolve<ICurrencyCacheRepository>(parameter));
            this.buRepository = new Lazy<IBusinessUnitCacheRepository>(() =>
                IOCContainer.Instance.Container.Resolve<IBusinessUnitCacheRepository>(parameter));
        }

        public LogModel Generate(ActiveLogTempEnum logid, params object[] args)
        {
            if ((int)logid == -1 || args == null || args.Length == 0)
                return null;

            ActiveLogTempInfo loginfo = ActiveLogTempInfo.GetTemInfo(logid);
            loginfo.FormatLogMessage(args);

            return LogModel.CreateBy(loginfo);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        public LogModel GenerateByStaffLogin(string staffId, CommonResult rst)
        {
            ActiveLogTempEnum logid = (ActiveLogTempEnum)(-1);
            if (rst.success)
            {
                logid = ActiveLogTempEnum.LOG_ACT_M501;
            }
            else
            {
                switch (rst.msgcode)
                {
                    case "MSG_ERROR_M034":
                        logid = ActiveLogTempEnum.LOG_ACT_M502;
                        break;
                    case "MSG_ERROR_M035":
                        logid = ActiveLogTempEnum.LOG_ACT_M504;
                        break;
                    case "MSG_ERROR_M036":
                        logid = ActiveLogTempEnum.LOG_ACT_M505;
                        break;
                    //case "MSG_ERROR_M040":
                    //case "MSG_ERROR_M041":
                    //case "MSG_ERROR_M042":
                    //    logid = ActiveLogTempEnum.LOG_ACT_M507;
                    //    break;
                }
            }


            return Generate(logid, staffId);
        }

        /// <summary>
        /// 用户退出
        /// </summary>
        public LogModel GenerateByStaffLogout(string staffId, string msgcode)
        {
            ActiveLogTempEnum logid = ActiveLogTempEnum.LOG_ACT_M509;
            if (msgcode == "MSG_ERROR_M044")
            {
                logid = ActiveLogTempEnum.LOG_ACT_M510;
            }

            return Generate(logid, staffId);
        }

        /// <summary>
        /// 用户修改密码
        /// </summary>
        public LogModel GenerateByStaffChangePWD(string staffId)
        {
            return Generate(ActiveLogTempEnum.LOG_ACT_M507, staffId);
        }

        /// <summary>
        /// 交易员配置
        /// </summary>
        public IEnumerable<LogModel> GenerateByDealerSetting(string staffId, 
            bool? automation, 
            bool? autoconnect,
            bool? newrequest,
            bool? trackrequest,
            bool? deviation)
        {
            List<LogModel> list = new List<LogModel>();

            LogModel log;
            if (automation.HasValue)
            {
                log = Generate(automation.Value ? ActiveLogTempEnum.LOG_ACT_M536 : ActiveLogTempEnum.LOG_ACT_M537, staffId);
                list.Add(log);
            }
            if (autoconnect.HasValue)
            {
                log = Generate(ActiveLogTempEnum.LOG_ACT_M539, staffId, autoconnect.Value ? "enabled" : "disabled");
                list.Add(log);
            }
            if (newrequest.HasValue)
            {
                log = Generate(ActiveLogTempEnum.LOG_ACT_M540, staffId, newrequest.Value ? "enabled" : "disabled");
                list.Add(log);
            }
            if (trackrequest.HasValue)
            {
                log = Generate(ActiveLogTempEnum.LOG_ACT_M541, staffId, newrequest.Value ? "enabled" : "disabled");
                list.Add(log);
            }
            if (deviation.HasValue)
            {
                log = Generate(ActiveLogTempEnum.LOG_ACT_M542, staffId, deviation.Value ? "enabled" : "disabled");
                list.Add(log);
            }

            return list;
        }

        /// <summary>
        /// 按条件查询银行账号现金流
        /// </summary>
        public LogModel GenerateByBankCashflow(string staffId, string bankAccountNo, string bankAccountName, string instrument, DateTime dateFrom, DateTime dateTo)
        {
            var queryMsg = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(bankAccountNo))
            {
                queryMsg.AppendFormat("BankAccountNo.:'{0}',", bankAccountNo);
            }

            if (!string.IsNullOrWhiteSpace(bankAccountName))
            {
                queryMsg.AppendFormat("BankAccountName:'{0}',", bankAccountName);
            }

            if (!string.IsNullOrWhiteSpace(instrument) && !instrument.Equals("All"))
            {
                queryMsg.AppendFormat("Instrument:'{0}',", instrument);
            }

            queryMsg.AppendFormat("DateFrom:'{0}',", dateFrom.Date.ToGMT0ByCurrStaff().ToString("MM-dd-yyyy HH:mm"));

            queryMsg.AppendFormat("DateTo:'{0}'", dateTo.Date.AddHours(23.9999).ToGMT0ByCurrStaff().ToString("MM-dd-yyyy HH:mm"));


            return Generate(ActiveLogTempEnum.LOG_ACT_M587, staffId, queryMsg);
        }

        /// <summary>
        /// 按条件查询银行转账单
        /// </summary>
        public LogModel GenerateByBankTransfer(string staffId, string bankAccountNo, string bankAccountName, string status, string type, DateTime dateFrom, DateTime dateTo)
        {
            var queryMsg = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(bankAccountNo))
            {
                queryMsg.AppendFormat("BankAccountNo.:'{0}',", bankAccountNo);
            }

            if (!string.IsNullOrWhiteSpace(bankAccountName))
            {
                queryMsg.AppendFormat("BankAccountName:'{0}',", bankAccountName);
            }

            if (!string.IsNullOrWhiteSpace(status) && !status.Equals("All"))
            {
                queryMsg.AppendFormat("Status:'{0}',", status);
            }

            if (!string.IsNullOrWhiteSpace(type) && !type.Equals("All"))
            {
                queryMsg.AppendFormat("Type:'{0}',", type);
            }

            queryMsg.AppendFormat("DateFrom:'{0}',", dateFrom.Date.ToGMT0ByCurrStaff().ToString("MM-dd-yyyy HH:mm"));

            queryMsg.AppendFormat("DateTo:'{0}'", dateTo.Date.AddHours(23.9999).ToGMT0ByCurrStaff().ToString("MM-dd-yyyy HH:mm"));


            return Generate(ActiveLogTempEnum.LOG_ACT_M588, staffId, queryMsg);
        }


        /// <summary>
        /// 按条件查询银行出入金单
        /// </summary>
        public LogModel GenerateByBankDW(string staffId, string bankAccountNo, BankCashflowTypeEnum? type, DateTime dateFrom, DateTime dateTo)
        {
            var queryMsg = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(bankAccountNo))
            {
                queryMsg.AppendFormat("AccountNumber:'{0}',", bankAccountNo);
            }

            if (type.HasValue)
            {
                queryMsg.AppendFormat("Type:'{0}',", type.ToDisplayString());
            }

            queryMsg.AppendFormat("DateFrom:'{0}',", dateFrom.Date.ToGMT0ByCurrStaff().ToString("MM-dd-yyyy HH:mm"));

            queryMsg.AppendFormat("DateTo:'{0}'", dateTo.Date.AddHours(23.9999).ToGMT0ByCurrStaff().ToString("MM-dd-yyyy HH:mm"));


            return Generate(ActiveLogTempEnum.LOG_ACT_M589, staffId, queryMsg);
        }

        /// <summary>
        /// 按条件查找帐户
        /// </summary>
        public LogModel GenerateBySearchCustomer(string staffId, string customerNo, string customerName, string userName, string firstName, string lastName, string telephone)
        {
            var queryMsg = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(customerNo))
            {
                queryMsg.AppendFormat("CustomerNo:'{0}',", customerNo);
            }

            if (!string.IsNullOrWhiteSpace(customerName))
            {
                queryMsg.AppendFormat("CustomerName:'{0}',", customerName);
            }

            if (!string.IsNullOrWhiteSpace(userName))
            {
                queryMsg.AppendFormat("UserName:'{0}',", userName);
            }

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                queryMsg.AppendFormat("FirstName:'{0}',", firstName);
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                queryMsg.AppendFormat("LastName:'{0}',", lastName);
            }

            if (!string.IsNullOrWhiteSpace(telephone))
            {
                queryMsg.AppendFormat("Telephone:'{0}',", telephone);
            }

            return Generate(ActiveLogTempEnum.LOG_ACT_M515, staffId, queryMsg);
        }

        /// <summary>
        /// 按条件查询订单 
        /// </summary>
        public LogModel GenerateBySearchDeal(string staffId, string customerNo, string dealId, string externalDealSetID, 
            string symbol, string channel, string instrument,
            DateTime? openTime, DateTime? openTimeTo, DateTime? valueDate, DateTime? valueDateTo)
        {
            var queryMsg = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(customerNo))
            {
                queryMsg.AppendFormat("CustomerNo:'{0}',", customerNo);
            }

            if (!string.IsNullOrWhiteSpace(dealId))
            {
                queryMsg.AppendFormat("DealId:'{0}',", dealId);
            }

            if (!string.IsNullOrWhiteSpace(externalDealSetID))
            {
                queryMsg.AppendFormat("ExternalDealSetID:'{0}',", externalDealSetID);
            }

            if (!string.IsNullOrWhiteSpace(symbol))
            {
                queryMsg.AppendFormat("Symbol:'{0}',", symbolRepository.Value.GetName(symbol));
            }

            if (!string.IsNullOrWhiteSpace(channel))
            {
                queryMsg.AppendFormat("Channel:'{0}',", channel);
            }

            if (!string.IsNullOrWhiteSpace(instrument))
            {
                queryMsg.AppendFormat("Instrument:'{0}',", instrument);
            }

            if (openTime.HasValue || openTimeTo.HasValue)
            {
                queryMsg.AppendFormat("OpenTime:'{0} ~ {1}'",
                    (openTime.HasValue ? openTime.Value.Date.ToGMT0ByCurrStaff() : DateTime.MinValue).ToString("MM-dd-yyyy HH:mm"),
                    (openTimeTo.HasValue ? openTimeTo.Value.Date.ToGMT0ByCurrStaff() : DateTime.MaxValue).ToString("MM-dd-yyyy HH:mm"));
            }


            if (valueDate.HasValue || valueDateTo.HasValue)
            {
                queryMsg.AppendFormat("ValueDate:'{0} ~ {1}'",
                    (valueDate.HasValue ? valueDate.Value.Date.ToGMT0ByCurrStaff() : DateTime.MinValue).ToString("MM-dd-yyyy HH:mm"),
                    (valueDateTo.HasValue ? valueDateTo.Value.Date.ToGMT0ByCurrStaff() : DateTime.MaxValue).ToString("MM-dd-yyyy HH:mm"));
            }

            return Generate(ActiveLogTempEnum.LOG_ACT_M707, staffId, queryMsg);
        }

        /// <summary>
        /// 按条件查询挂单
        /// </summary>
        public LogModel GenerateBySearchOrder(string staffId, string customerNo, string symbol, string orderRate, string traderSpotRate,
            DateTime? openTime, DateTime? openTimeTo, string status)
        {
            var queryMsg = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(customerNo))
            {
                queryMsg.AppendFormat("CustomerNo:'{0}',", customerNo);
            }

            if (!string.IsNullOrWhiteSpace(symbol))
            {
                queryMsg.AppendFormat("Symbol:'{0}',", symbolRepository.Value.GetName(symbol));
            }

            if (!string.IsNullOrWhiteSpace(orderRate))
            {
                queryMsg.AppendFormat("Order Rate:'{0}',", orderRate);
            }

            if (!string.IsNullOrWhiteSpace(traderSpotRate))
            {
                queryMsg.AppendFormat("Trader Spot Rate:'{0}',", traderSpotRate);
            }

            if (openTime.HasValue || openTimeTo.HasValue)
            {
                queryMsg.AppendFormat("OpenTime:'{0} ~ {1}'",
                    (openTime.HasValue ? openTime.Value.Date.ToGMT0ByCurrStaff() : DateTime.MinValue).ToString("MM-dd-yyyy HH:mm"),
                    (openTimeTo.HasValue ? openTimeTo.Value.Date.ToGMT0ByCurrStaff() : DateTime.MaxValue).ToString("MM-dd-yyyy HH:mm"));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                queryMsg.AppendFormat("Status:'{0}',", status);
            }


            return Generate(ActiveLogTempEnum.LOG_ACT_M708, staffId, queryMsg);
        }

        /// <summary>
        /// 生成ForwardBook查询
        /// </summary>
        /// <param name="staffId">Staff Id</param>
        /// <param name="businessUnitName">BusinessUnit Name</param>
        /// <returns>日志</returns>
        public LogModel GenerateBySearchForwardBook(string staffId, string businessUnitName)
        {
            return this.Generate(ActiveLogTempEnum.LOG_ACT_M522, staffId, businessUnitName);
        }

        /// <summary>
        /// 生成DealBlotter查询
        /// </summary>
        /// <param name="staffId">
        /// Staff Id
        /// </param>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <param name="buyCurrencyId">
        /// The buy Currency Id.
        /// </param>
        /// <param name="andOr">
        /// The and Or.
        /// </param>
        /// <param name="sellCurrencyId">
        /// The sell Currency Id.
        /// </param>
        /// <param name="symbolId">
        /// The symbol Id.
        /// </param>
        /// <param name="contractRate">
        /// The contract Rate.
        /// </param>
        /// <returns>
        /// 日志
        /// </returns>
        public LogModel GenerateBySearchDealBlotter(string staffId, DateTime? from, DateTime? to, string buyCurrencyId, string andOr, string sellCurrencyId, string symbolId, decimal contractRate)
        {
            var queryMsg = new StringBuilder();
            if (from.HasValue)
            {
                queryMsg.AppendFormat("From: {0} ", from.Value.ToString("dd-MM-yyyy"));
            }

            if (to.HasValue)
            {
                queryMsg.AppendFormat("To: {0} ", to.Value.ToString("dd-MM-yyyy"));
            }

            if (!string.IsNullOrEmpty(buyCurrencyId))
            {
                queryMsg.AppendFormat("Buy currency: {0} ", this.currencyRepository.Value.GetNameByID(buyCurrencyId));
            }

            if (!string.IsNullOrEmpty(andOr))
            {
                queryMsg.AppendFormat("And/Or: {0} ", andOr);
            }

            if (!string.IsNullOrEmpty(sellCurrencyId))
            {
                queryMsg.AppendFormat("Sell currency: {0} ", this.currencyRepository.Value.GetNameByID(sellCurrencyId));
            }

            if (!string.IsNullOrEmpty(symbolId))
            {
                queryMsg.AppendFormat("Symbol: {0} ", this.symbolRepository.Value.GetName(symbolId));
            }

            queryMsg.AppendFormat("Contract Rate: {0} ", contractRate);
            return this.Generate(ActiveLogTempEnum.LOG_ACT_M525, staffId, queryMsg);
        }

        /// <summary>
        /// 生成ODAlert
        /// </summary>
        /// <param name="staffId">StaffId</param>
        /// <param name="accountId">AccountId</param>
        /// <param name="currencyId">CurrencyId</param>
        /// <returns>日志</returns>
        public LogModel GenerateBySearchOdAlert(string staffId, string accountId, string currencyId)
        {
            return this.Generate(
                ActiveLogTempEnum.LOG_ACT_M533,
                staffId,
                accountId,
                this.currencyRepository.Value.GetNameByID(currencyId));
        }


        /// <summary>
        /// 按条件查询日志
        /// </summary>
        public LogModel GenerateBySearchLog(string staffId, DateTime start, DateTime end, string businessUnitID, string customerNo, string dealID, string operatorID, string keyWord)
        {
            var queryMsg = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(customerNo))
            {
                queryMsg.AppendFormat("CustomerNo:'{0}',", customerNo);
            }

            if (!string.IsNullOrWhiteSpace(businessUnitID))
            {
                queryMsg.AppendFormat("BusinessUnit:'{0}',", buRepository.Value.GetName(businessUnitID));
            }

            if (!string.IsNullOrWhiteSpace(dealID))
            {
                queryMsg.AppendFormat("DealID:'{0}',", dealID);
            }

            if (!string.IsNullOrWhiteSpace(operatorID))
            {
                queryMsg.AppendFormat("OperatorID:'{0}',", operatorID);
            }

            if (!string.IsNullOrWhiteSpace(keyWord))
            {
                queryMsg.AppendFormat("KeyWord:'{0}',", keyWord);
            }

            queryMsg.AppendFormat("LogTime:'{0} ~ {1}'",
                start.ToGMT0ByCurrStaff().ToString("dd-MM-yyyy HH:mm"),
                end.ToGMT0ByCurrStaff().ToString("dd-MM-yyyy HH:mm"));


            return Generate(ActiveLogTempEnum.LOG_ACT_M708, staffId, queryMsg);
        }

        /// <summary>
        /// 查询quoteWatchar 日志
        /// </summary>
        /// <param name="staffId">
        /// The staff Id.
        /// </param>
        /// <param name="symbolName">
        /// The symbol Name.
        /// </param>
        /// <param name="buName">
        /// The bu Name.
        /// </param>
        /// <param name="localTradeDate">
        /// The local Trade Date.
        /// </param>
        /// <param name="buySellAmount">
        /// The buy Sell Amount.
        /// </param>
        /// <param name="custNoOrQuoteGroup">
        /// The cust No Or Quote Group.
        /// </param>
        public LogModel GenerateByQuoteWatchar(string staffId, string symbolName, string buName, string localTradeDate, string buySellAmount, string custNoOrQuoteGroup)
        {
            return Generate(ActiveLogTempEnum.LOG_ACT_M512, staffId, symbolName, buName, localTradeDate, buySellAmount, custNoOrQuoteGroup);
        }

        /// <summary>
        /// ForwardBook超过员工用户限额
        /// </summary>
        /// <param name="staffId">StaffId</param>
        /// <param name="currencyId">CurrencyId</param>
        /// <param name="tenor">Tenor</param>
        /// <param name="netAmount">NetAmount</param>
        /// <param name="netAmountLimit">NetAmountLimit</param>
        /// <returns>日志</returns>
        public LogModel GenerateByLimitForwardBook(
            string staffId,
            string currencyId,
            TenorEnum tenor,
            string netAmount,
            decimal netAmountLimit)
        {
            return this.Generate(
                ActiveLogTempEnum.LOG_ACT_M751,
                staffId,
                this.currencyRepository.Value.GetNameByID(currencyId),
                tenor.ToString(),
                netAmount,
                netAmountLimit.ToString());
        }

        /// <summary>
        /// DealingBook超过员工用户限额
        /// </summary>
        /// <param name="staffId">
        /// StaffId
        /// </param>
        /// <param name="currencyId">
        /// CurrencyId
        /// </param>
        /// <param name="netAmount">
        /// NetAmount
        /// </param>
        /// <param name="netAmountLimit">
        /// NetAmountLimit
        /// </param>
        /// <returns>
        /// 日志
        /// </returns>
        public LogModel GenerateByLimitDealingBook(
            string staffId,
            string currencyId,
            decimal netAmount,
            decimal netAmountLimit)
        {
            return this.Generate(
                ActiveLogTempEnum.LOG_ACT_M523,
                staffId,
                this.currencyRepository.Value.GetNameByID(currencyId),
                netAmount.ToFixed(2),
                netAmountLimit.ToString());
        }

        /// <summary>
        /// 生成TodayOpenAdjustment的ActiveLog
        /// </summary>
        /// <param name="staffId">
        /// Staff Id
        /// </param>
        /// <param name="snapshotFrom">
        /// The snapshot From.
        /// </param>
        /// <param name="currencyName">
        /// The currency Name.
        /// </param>
        /// <param name="businessUnitName">
        /// BusinessUnit Name
        /// </param>
        /// <param name="originalAdjustment">
        /// The original Adjustment.
        /// </param>
        /// <param name="newAdjustment">
        /// The new Adjustment.
        /// </param>
        /// <returns>
        /// 日志
        /// </returns>
        public LogModel GenerateByTodayOpenAdjustment(string staffId, DateTime snapshotFrom, string currencyName, string businessUnitName, decimal originalAdjustment, decimal newAdjustment)
        {
            int decimalPlace = 3;
            var currency = this.currencyRepository.Value.GetByName(currencyName);
            if (currency != null)
            {
                decimalPlace = currency.AmountDecimals;
            }

            return this.Generate(
                ActiveLogTempEnum.LOG_ACT_M550,
                staffId,
                snapshotFrom.ToString("dd-MM-yyyy HH:mm"),
                currencyName,
                businessUnitName,
                originalAdjustment.FormatPriceBySymbolPoint(decimalPlace),
                newAdjustment.FormatPriceBySymbolPoint(decimalPlace));
        }

        /// <summary>
        /// 生成移除MarginCall的ActiveLog
        /// </summary>
        /// <param name="staffId">
        /// The staff id.
        /// </param>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <param name="equity">
        /// The equity.
        /// </param>
        /// <param name="bop">
        /// The bop.
        /// </param>
        /// <param name="marginUsed">
        /// The margin used.
        /// </param>
        /// <param name="marginCallLevel">
        /// The margin call level.
        /// </param>
        /// <param name="marginLevel">
        /// The margin level.
        /// </param>
        /// <param name="success">
        /// The success.
        /// </param>
        /// <param name="failReason">
        /// The fail reason.
        /// </param>
        /// <returns>
        /// The <see cref="LogModel"/>.
        /// </returns>
        public LogModel GenerateByRemoveMarginCall(string staffId, string customerId, decimal equity, decimal bop, decimal marginUsed, decimal marginCallLevel, decimal marginLevel, bool success, string failReason)
        {
            if (success)
            {
                return this.Generate(
                    ActiveLogTempEnum.LOG_ACT_M721,
                    staffId,
                    customerId,
                    equity,
                    bop,
                    marginUsed,
                    marginCallLevel,
                    marginLevel);
            }
            else
            {
                return this.Generate(
                    ActiveLogTempEnum.LOG_ACT_M722,
                    staffId,
                    customerId,
                    equity,
                    bop,
                    marginUsed,
                    marginCallLevel,
                    marginLevel,
                    failReason);
            }
        }

        /// <summary>
        /// 暂停客户端网上交易
        /// </summary>
        public LogModel GenerateSuspendOnlineTrading(BaseViewModel.BaseBusinessUnitVM bu)
        {
            if (bu == null)
            {
                return null;
            }
            LogModel logModel = null;
            switch (bu.ClientTradeStatus)
            {
                case ClientTradeStatusEnum.TotalForbidden:
                    logModel = Generate(ActiveLogTempEnum.LOG_ACT_M545, RunTime.GetCurrentRunTime().CurrentStaff.StaffBaseInfo.StaffID, bu.BusinessUnitName);
                    break;
                case ClientTradeStatusEnum.TradeForbidden:
                    logModel = Generate(ActiveLogTempEnum.LOG_ACT_M546, RunTime.GetCurrentRunTime().CurrentStaff.StaffBaseInfo.StaffID, bu.BusinessUnitName);
                    break;
                case ClientTradeStatusEnum.FullAccess:
                    logModel = Generate(ActiveLogTempEnum.LOG_ACT_M547, RunTime.GetCurrentRunTime().CurrentStaff.StaffBaseInfo.StaffID, bu.BusinessUnitName);
                    break;
                default:
                    logModel = null;
                    break;
            }
            return logModel;
        }

        /// <summary>
        /// 生成MarginCallComment的ActiveLog
        /// </summary>
        /// <param name="staffId">
        /// Staff Id
        /// </param>
        /// <param name="customerId">
        /// The customer Id.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// 日志
        /// </returns>
        public LogModel GenerateByMarginCallComment(string staffId, string customerId, string comment)
        {
            return this.Generate(ActiveLogTempEnum.LOG_ACT_M720, staffId, customerId, comment);
        }
    }
}
