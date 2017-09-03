// <copyright file="ValueDateCore.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>fangz</author>
// <date> 2016/05/16 06:04:23 </date>
// <summary> 交割日中心 </summary>
// <modify>
//      修改人：fangz
//      修改时间：2016/05/16 06:04:23
//      修改描述：新建 ValueDateCore.cs
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
    using System.Linq;

    using BaseViewModel;

    using Infrastructure.Common.Enums;
    using Infrastructure.Data;
    using Infrastructure.Log;
    using Infrastructure.Models;

    #endregion

    /// <summary>
    /// valuedate 类
    /// </summary>
    public class ValueDateCore : BaseVm
    {
        #region Static Fields

        /// <summary>
        /// 静态只读唯一实例
        /// </summary>
        private static readonly ValueDateCore StaticInstance = new ValueDateCore();

        #endregion

        #region Fields

        /// <summary>
        /// 业务区仓储
        /// </summary>
        private IBusinessUnitCacheRepository businessUnitReps;

        /// <summary>
        /// 通用配置仓储
        /// </summary>
        private IGeneralSettingCacheRepository genSettingsReps;

        /// <summary>
        /// 商品缓存仓储实例
        /// </summary>
        private ISymbolCacheRepository symbolReps;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueDateCore"/> class. 
        /// </summary>
        public ValueDateCore()
        {
            this.genSettingsReps = this.GetRepository<IGeneralSettingCacheRepository>();
            this.businessUnitReps = this.GetRepository<IBusinessUnitCacheRepository>();
            this.symbolReps = this.GetRepository<ISymbolCacheRepository>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 单例获取
        /// </summary>
        public static ValueDateCore Instance
        {
            get
            {
                return StaticInstance;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// 根据ValueDate获取对应的Tenor
        /// </summary>
        /// <param name="busUnitId">
        /// 业务区ID
        /// </param>
        /// <param name="varValueDate">
        /// 交割日
        /// </param>
        /// <param name="symbolId">
        /// 商品ID
        /// </param>
        /// <param name="rstPrompt">
        /// 操作结果码
        /// </param>
        /// <returns>
        /// 返回ValueDate对应的Tenor
        /// </returns>
        public TenorEnum GetTenorByValueDate(
            string busUnitId,
            DateTime varValueDate,
            string symbolId,
            out string rstPrompt)
        {
            rstPrompt = Infrastructure.Common.ErrorMsg.Common.Success;

            var busUnit = this.businessUnitReps.FindByID(busUnitId);
            if (busUnit == null)
            {
                TraceManager.Error.Write("ValueDate", "cant find business unit:{0}", busUnitId);
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.BU_NotExist;
                return TenorEnum.ON;
            }

            // 根据当前本地系统时间，得到localTradeDate
            DateTime localTradeDate = busUnit.GetLocalTradeDate(RunTime.GetCurrentRunTime().GetCurrentTimeForBu(busUnitId));
            if (localTradeDate == DateTime.MinValue)
            {
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return TenorEnum.ON;
            }

            // 屏蔽受Valuedate中携带的时间的影响
            var targetValueDate = varValueDate.Date;

            // 判断ValueDate是否小于LocalTradeDate
            if (targetValueDate < localTradeDate)
            {
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return TenorEnum.ON;
            }

            string localCcy = busUnit.LocalCCYID;

            var symbol = this.symbolReps.FindByID(symbolId);
            if (symbol == null)
            {
                TraceManager.Error.Write("ValueDate", "cant find symbol:{0}", symbolId);
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Symbol_NotExist;
                return TenorEnum.ON;
            }

            if (!this.VerifySettleHoliday(symbol.SymbolModel, targetValueDate))
            {
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return TenorEnum.ON;
            }

            if (targetValueDate == localTradeDate)
            {
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Success;
                return TenorEnum.ON;
            }

            // 获取Spot的ValueDate
            DateTime spotValueDate = this.GetSpotValueDate(symbol.SymbolModel, localTradeDate, localCcy);
            if (spotValueDate == DateTime.MinValue)
            {
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return TenorEnum.ON;
            }

            if (targetValueDate == spotValueDate)
            {
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Success;
                return TenorEnum.SP;
            }

            // 此算法对Tenor的取值进行了限定，如果更改当前的Tenor的取值，则此算法崩溃，
            // 另一种处理策略则是只有算完所有的Tenor对应的ValueDate，并不匹配后才进行BD的确认,当然此种算法效率较低
            for (int i = 1; i < 19; i++)
            {
                var tenorItem = (TenorEnum)i;

                if (i == 1)
                {
                    if (symbol.ValueDate == ValueDateEnum.T1)
                    {
                        continue;
                    }
                }

                // 根据Spot的ValueDate获取其他Tenor的ValueDate
                DateTime tenorValueDate = this.GetValueDateByTenor(tenorItem, localTradeDate, spotValueDate, symbol.SymbolModel);
                if (tenorValueDate == DateTime.MinValue)
                {
                    TraceManager.Debug.Write(
                        "ValueDate",
                        "Cant get tenor value date, please check, when GetTenorByValueDate, Bu:{0}, symbol:{1}, tenor:{2}",
                        busUnitId,
                        symbolId,
                        tenorItem);
                    rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                    return TenorEnum.ON;
                }

                if (tenorValueDate == targetValueDate)
                {
                    rstPrompt = Infrastructure.Common.ErrorMsg.Common.Success;
                    return tenorItem;
                }

                if (targetValueDate < tenorValueDate)
                {
                    // 此为异常控制，正常情况不会到达这里
                    if (i <= 3)
                    {
                        TraceManager.Error.Write(
                            "ValueDate",
                            "Invalid Logix, please check, when GetTenorByValueDate, Bu:{0}, symbol:{1}, valueDate:{2}",
                            busUnitId,
                            symbolId,
                            varValueDate);
                        rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                        return TenorEnum.ON;
                    }

                    rstPrompt = Infrastructure.Common.ErrorMsg.Common.Success;
                    return TenorEnum.BD;
                }
            }

            rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
            return TenorEnum.ON;
        }

        /// <summary>
        /// 根据ValueDate获取对应的Tenor
        /// </summary>
        /// <param name="busUnitId">
        /// 业务区ID
        /// </param>
        /// <param name="varValueDate">
        /// 交割日
        /// </param>
        /// <param name="symbolId">
        /// 商品ID
        /// </param>
        /// <param name="tenor">
        /// 对应的Tenor
        /// </param>
        /// <param name="expireTime">
        /// Tenor过期时间
        /// </param>
        /// <returns>
        /// 是否找到对应的Tenor，如果没有找到，不再计算Pl
        /// </returns>
        public bool GetTenorOrNextTenorByValueDateForCalculate(
            string busUnitId,
            DateTime varValueDate,
            string symbolId,
            out TenorEnum tenor,
            out DateTime expireTime)
        {
            var busUnit = this.businessUnitReps.FindByID(busUnitId);
            if (busUnit == null)
            {
                TraceManager.Error.Write("ValueDate", "cant find business unit, business unit:{0}, valuedate:{1}, symbolId:{2}", busUnitId, varValueDate, symbolId);
                expireTime = RunTime.GetCurrentRunTime().GetSystemGmtTime().AddHours(1);
                tenor = TenorEnum.ON;
                return false;
            }

            // 根据当前本地系统时间，得到localTradeDate
            DateTime localTradeDate = busUnit.GetCurrentOrNextTradeDateEndTime(RunTime.GetCurrentRunTime().GetCurrentTimeForBu(busUnitId), out expireTime);
            if (localTradeDate == DateTime.MinValue)
            {
                TraceManager.Error.Write("ValueDate", "cant get localTradeDate, business unit:{0}, valuedate:{1}, symbolId:{2}", busUnitId, varValueDate, symbolId);
                tenor = TenorEnum.ON;
                return false;
            }

            // 屏蔽受Valuedate中携带的时间的影响
            var targetValueDate = varValueDate.Date;

            // 判断ValueDate是否小于LocalTradeDate
            if (targetValueDate <= localTradeDate)
            {
                expireTime = DateTime.MaxValue;
                tenor = TenorEnum.ON;
                return true;
            }

            string localCcy = busUnit.LocalCCYID;

            var symbol = this.symbolReps.FindByID(symbolId);
            if (symbol == null)
            {
                TraceManager.Error.Write("ValueDate", "cant find symbol, business unit:{0}, valuedate:{1}, symbolId:{2}", busUnitId, varValueDate, symbolId);
                expireTime = RunTime.GetCurrentRunTime().GetSystemGmtTime().AddHours(1);
                tenor = TenorEnum.ON;
                return false;
            }

            if (!this.VerifySettleHoliday(symbol.SymbolModel, targetValueDate))
            {
                TraceManager.Error.Write("ValueDate", "Valuedate is settleholiday, business unit:{0}, valuedate:{1}, symbolId:{2}", busUnitId, varValueDate, symbolId);
                expireTime = RunTime.GetCurrentRunTime().GetSystemGmtTime().AddHours(1);
                tenor = TenorEnum.ON;
                return false;
            }

            // 获取Spot的ValueDate
            DateTime spotValueDate = this.GetSpotValueDate(symbol.SymbolModel, localTradeDate, localCcy);
            if (spotValueDate == DateTime.MinValue)
            {
                TraceManager.Error.Write("ValueDate", "cant get spotvaluedate, business unit:{0}, valuedate:{1}, symbolId:{2}", busUnitId, varValueDate, symbolId);
                tenor = TenorEnum.ON;
                return false;
            }

            if (targetValueDate == spotValueDate)
            {
                tenor = TenorEnum.SP;
                return true;
            }

            // 此算法对Tenor的取值进行了限定，如果更改当前的Tenor的取值，则此算法崩溃，
            // 另一种处理策略则是只有算完所有的Tenor对应的ValueDate，并不匹配后才进行BD的确认,当然此种算法效率较低
            for (int i = 1; i < 19; i++)
            {
                if (i == 1)
                {
                    if (symbol.ValueDate == ValueDateEnum.T1)
                    {
                        continue;
                    }
                }

                var tenorItem = (TenorEnum)i;

                // 根据Spot的ValueDate获取其他Tenor的ValueDate
                DateTime tenorValueDate = this.GetValueDateByTenor(tenorItem, localTradeDate, spotValueDate, symbol.SymbolModel);
                if (tenorValueDate == DateTime.MinValue)
                {
                    TraceManager.Error.Write(
                        "ValueDate", 
                        "Cant get tenor value date, please check, when GetTenorByValueDate, Bu:{0}, symbol:{1}, tenor:{2}",
                        busUnitId,
                        symbolId,
                        tenorItem);
                    expireTime = RunTime.GetCurrentRunTime().GetSystemGmtTime().AddHours(1);
                    tenor = TenorEnum.ON;
                    return false;
                }

                if (tenorValueDate == targetValueDate)
                {
                    tenor = tenorItem;
                    return true;
                }

                if (targetValueDate < tenorValueDate)
                {
                    // 此为异常控制，正常情况不会到达这里
                    if (i <= 3)
                    {
                        TraceManager.Error.Write(
                            "ValueDate", 
                            "Invalid Logix, please check, when GetTenorByValueDate, Bu:{0}, symbol:{1}, valueDate:{2}",
                            busUnitId,
                            symbolId,
                            varValueDate);
                        expireTime = RunTime.GetCurrentRunTime().GetSystemGmtTime().AddHours(1);
                        tenor = TenorEnum.ON;
                        return false;
                    }

                    tenor = TenorEnum.BD;
                    return true;
                }
            }

            TraceManager.Error.Write("ValueDate", "cant find tenor, business unit:{0}, valuedate:{1}, symbolId:{2}", busUnitId, varValueDate, symbolId);
            expireTime = RunTime.GetCurrentRunTime().GetSystemGmtTime().AddHours(1);
            tenor = TenorEnum.ON;
            return false;
        }

        /// <summary>
        /// 根据Tenor获取对应的交割日
        /// </summary>
        /// <param name="busUnitId">
        /// 业务区标识
        /// </param>
        /// <param name="tenor">
        /// 远期类型
        /// </param>
        /// <param name="symbolId">
        /// 商品标识
        /// </param>
        /// <param name="rstPrompt">
        /// 返回操作结果标识
        /// </param>
        /// <returns>
        /// 返回对应的交割日
        /// </returns>
        public DateTime GetValueDateByTenor(string busUnitId, TenorEnum tenor, string symbolId, out string rstPrompt)
        {
            rstPrompt = Infrastructure.Common.ErrorMsg.Common.Success;

            var busUnit = this.businessUnitReps.FindByID(busUnitId);
            if (busUnit == null)
            {
                TraceManager.Error.Write("ValueDate", "cant find business unit:{0}", busUnitId);
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.BU_NotExist;
                return DateTime.MinValue;
            }

            var symbol = this.symbolReps.FindByID(symbolId);
            if (symbol == null)
            {
                TraceManager.Error.Write("ValueDate", "cant find symbol:{0}", symbolId);
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Symbol_NotExist;
                return DateTime.MinValue;
            }

            // 根据当前本地系统时间，得到localTradeDate
            DateTime localTradeDate = busUnit.GetLocalTradeDate(RunTime.GetCurrentRunTime().GetCurrentTimeForBu(busUnitId));
            if (localTradeDate == DateTime.MinValue)
            {
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return DateTime.MinValue;
            }

            // 获取Spot的ValueDate
            DateTime spotValueDate = this.GetSpotValueDate(symbol.SymbolModel, localTradeDate, busUnit.LocalCCYID);
            if (spotValueDate == DateTime.MinValue)
            {
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return DateTime.MinValue;
            }

            // 根据Spot的ValueDate获取其他Tenor的ValueDate
            DateTime valueDate = this.GetValueDateByTenor(tenor, localTradeDate, spotValueDate, symbol.SymbolModel);
            if (valueDate == DateTime.MinValue)
            {
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return valueDate;
            }

            return valueDate;
        }

        /// <summary>
        /// 根据Tenor获取对应的交割日
        /// </summary>
        /// <param name="busUnitId">
        /// 业务区标识
        /// </param>
        /// <param name="tenor">
        /// 远期类型
        /// </param>
        /// <param name="symbolId">
        /// 商品标识
        /// </param>
        /// <param name="rstPrompt">
        /// 返回操作结果标识
        /// </param>
        /// <returns>
        /// 返回对应的交割日
        /// </returns>
        public DateTime GetValueDateByTenorWithCurrentOrNextLocalTradedate(string busUnitId, TenorEnum tenor, string symbolId, out string rstPrompt)
        {
            rstPrompt = Infrastructure.Common.ErrorMsg.Common.Success;

            var busUnit = this.businessUnitReps.FindByID(busUnitId);
            if (busUnit == null)
            {
                TraceManager.Error.Write("ValueDate", "cant find business unit:{0}", busUnitId);
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.BU_NotExist;
                return DateTime.MinValue;
            }

            var symbol = this.symbolReps.FindByID(symbolId);
            if (symbol == null)
            {
                TraceManager.Error.Write("ValueDate", "cant find symbol:{0}", symbolId);
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Symbol_NotExist;
                return DateTime.MinValue;
            }

            // 根据当前本地系统时间，得到localTradeDate
            DateTime expiredate;
            DateTime localTradeDate = busUnit.GetCurrentOrNextTradeDateEndTime(RunTime.GetCurrentRunTime().GetCurrentTimeForBu(busUnitId), out expiredate);
            if (localTradeDate == DateTime.MinValue)
            {
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return DateTime.MinValue;
            }

            // 获取Spot的ValueDate
            DateTime spotValueDate = this.GetSpotValueDate(symbol.SymbolModel, localTradeDate, busUnit.LocalCCYID);
            if (spotValueDate == DateTime.MinValue)
            {
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return DateTime.MinValue;
            }

            // 根据Spot的ValueDate获取其他Tenor的ValueDate
            DateTime valueDate = this.GetValueDateByTenor(tenor, localTradeDate, spotValueDate, symbol.SymbolModel);
            if (valueDate == DateTime.MinValue)
            {
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return valueDate;
            }

            return valueDate;
        }

        /// <summary>
        /// 验证提前交割的ValueDate设置是否正确
        /// </summary>
        /// <param name="symbolId">
        /// 商品
        /// </param>
        /// <param name="busUnitId">
        /// 业务区ID
        /// </param>
        /// <param name="valueDate">
        /// 原ValueDate
        /// </param>
        /// <param name="nearLegValueDate">
        /// Swap中NearLeg的ValueDate
        /// </param>
        /// <returns>
        /// 返回true为验证通过
        /// </returns>
        /// <code>
        /// public bool ValidatePredeliveryValueDate(string symbolId, string busUnitId, DateTime valueDate, DateTime nearLegValueDate)
        /// </code>
        public bool ValidatePredeliveryValueDate(
            string symbolId,
            string busUnitId,
            DateTime valueDate,
            DateTime nearLegValueDate)
        {
            // 1.判断Swap的ValueDate是否是有效的ValueDate
            var busUnit = this.businessUnitReps.FindByID(busUnitId);
            if (busUnit == null)
            {
                TraceManager.Error.Write("ValueDate", "cant find business unit:{0}", busUnitId);
                return false;
            }

            var symbol = this.symbolReps.FindByID(symbolId);
            if (symbol == null)
            {
                TraceManager.Error.Write("ValueDate", "cant find symbol:{0}", symbolId);
                return false;
            }

            // 2.获取最近的一个ValueDate
            DateTime localSysTime = RunTime.GetCurrentRunTime().GetCurrentTimeForBu(busUnitId);

            // 根据当前本地系统时间，得到localTradeDate
            DateTime localTradeDate = busUnit.GetLocalTradeDate(localSysTime);
            if (localTradeDate == DateTime.MinValue)
            {
                TraceManager.Error.Write("ValueDate", "Fail when ValidatePredeliveryValueDate, cant find local trade date.");
                return false;
            }

            DateTime nextValueDate = this.GetNextValueDate(symbol.SymbolModel, localTradeDate);
            if (nextValueDate == DateTime.MinValue)
            {
                TraceManager.Error.Write("ValueDate", "Fail when ValidatePredeliveryValueDate, cant find next valuedate.");
                return false;
            }

            // 这里的NextValueDate其实就是TN
            if (nearLegValueDate.Date >= nextValueDate.Date && nearLegValueDate.Date < valueDate.Date)
            {
                return true;
            }

            // 不应该加日志，外面加
            return false;
        }

        /// <summary>
        /// 验证展期的ValueDate设置是否正确
        /// </summary>
        /// <param name="symbolId">
        /// 商品
        /// </param>
        /// <param name="busUnitId">
        /// 业务区ID
        /// </param>
        /// <param name="valueDate">
        /// 原ValueDate
        /// </param>
        /// <param name="farLegValueDate">
        /// Swap中FarLeg的ValueDate
        /// </param>
        /// <returns>
        /// 返回true为验证通过
        /// </returns>
        /// <code>
        /// public bool ValidateRolloverValueDate(string symbolId, string busUnitId, DateTime valueDate, DateTime farLegValueDate)
        /// </code>
        public bool ValidateRolloverValueDate(
            string symbolId,
            string busUnitId,
            DateTime valueDate,
            DateTime farLegValueDate)
        {
            var symbol = this.symbolReps.FindByID(symbolId);

            if (this.VerifySettleHoliday(symbol.SymbolModel, farLegValueDate) == false
                || this.VerifySettleHoliday(symbol.SymbolModel, farLegValueDate) == false)
            {
                return false;
            }

            if (farLegValueDate.Date > valueDate.Date)
            {
                return true;
            }

            // 不应该加日志，外面加
            return false;
        }

        /// <summary>
        /// 验证ValueDate和Tenor是否对应
        /// </summary>
        /// <param name="custBuyCcy">
        /// 客户Buy方向货币
        /// </param>
        /// <param name="busUnitId">
        /// 业务区ID
        /// </param>
        /// <param name="tenor">
        /// 远期类型
        /// </param>
        /// <param name="symbolId">
        /// 商品ID
        /// </param>
        /// <param name="clientValueDate">
        /// 客户端传来的交割日
        /// </param>
        /// <param name="rstPrompt">
        /// 返回操作结果标识
        /// </param>
        /// <returns>
        /// 返回交割日是否正确
        /// </returns>
        /// <code>
        /// public bool ValidateValueDate(string custBuyCCY, string busUnitId, TenorEnum tenor, string symbolId, DateTime clientValueDate, out string rstPrompt)
        /// </code>
        public bool ValidateValueDate(
            string custBuyCcy,
            string busUnitId,
            TenorEnum tenor,
            string symbolId,
            DateTime clientValueDate,
            out string rstPrompt)
        {
            rstPrompt = Infrastructure.Common.ErrorMsg.Common.Success;

            if (tenor == TenorEnum.ON)
            {
                return this.ValidateOnValueDate(clientValueDate, custBuyCcy, busUnitId, symbolId, out rstPrompt);
            }

            if (tenor == TenorEnum.BD)
            {
                return this.ValidateBdValueDate(clientValueDate, busUnitId, symbolId, out rstPrompt);
            }

            // 非ON、非BD
            DateTime valueDate = this.GetValueDateByTenor(busUnitId, tenor, symbolId, out rstPrompt);

            if (rstPrompt != Infrastructure.Common.ErrorMsg.Common.Success)
            {
                TraceManager.Info.Write(
                    "ValidateValueDate",
                    "Error Reason:{0} ,Data:BusUnitID :{1}, SymbolID:{2}, Tenor:{3}, ValueDate:{4}, CustomerBuyCCY:{5}",
                    rstPrompt,
                    busUnitId,
                    symbolId,
                    tenor,
                    clientValueDate.Date.ToShortDateString(),
                    custBuyCcy);
                return false;
            }

            if (valueDate.Date == clientValueDate.Date)
            {
                return true;
            }

            TraceManager.Info.Write(
                "ValidateValueDate",
                "Error Reason:ValueDate is invalid ,Data:BusUnitID :{1}, SymbolID:{2}, Tenor:{3}, ValueDate:{4}, CustomerBuyCCY:{5}, ServerValueDate:{6}",
                rstPrompt,
                busUnitId,
                symbolId,
                tenor,
                clientValueDate.Date.ToShortDateString(),
                custBuyCcy,
                valueDate.Date.ToShortDateString());
            rstPrompt = Infrastructure.Common.ErrorMsg.Common.SystemInvalidParas;
            return false;
        }

        /// <summary>
        /// 判断是否为有效的交割日
        /// </summary>
        /// <returns>
        /// 返回判断结果：true为有效交割日，false为无效交割日
        /// </returns>
        /// <summary>
        /// 报表界面用判断是否为有效交割日
        /// </summary>
        /// <param name="exdate">
        /// 目标日期
        /// </param>
        /// <returns>
        /// 交割日
        /// </returns>
        public DateTime GetValueDateForReport(DateTime exdate)
        {
            DateTime valueDate = exdate;

            bool flg = false;

            while (!flg)
            {
                // 获取基准日的星期
                DayOfWeek dayOfWeek = exdate.DayOfWeek;

                // 如果为周六、周日，为无效交割日
                if (dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday)
                {
                    valueDate = exdate;
                    flg = true;
                }
                else
                {
                    exdate = exdate.AddDays(1);
                }
            }

            return valueDate;
        }

        /// <summary>
        /// 根据valueDate 获取 ValueDate对应的Tenor
        /// </summary>
        /// <param name="symbolId">
        /// 商品Id
        /// </param>
        /// <param name="valueDate">
        /// valuedate
        /// </param>
        /// <param name="buid">
        /// 业务区Id
        /// </param>
        /// <param name="isKvbSellCcy1">
        /// kvb sell 是否为ccy1
        /// </param>
        /// <returns>
        /// 返回vdVM，返回null代表找不到对应Tenor，交易时提示失败
        /// </returns>
        public BaseSymbolTenorVDVM GetTenorByValueDateWeSellCcy(
            string symbolId, 
            DateTime valueDate, 
            string buid, 
            bool isKvbSellCcy1)
        {
            string prompt;
            var tenor = this.GetTenorByValueDate(buid, valueDate, symbolId, out prompt);
            if (prompt == Infrastructure.Common.ErrorMsg.Common.Success)
            {
                if (tenor == TenorEnum.ON)
                {
                    var symbol = this.GetRepository<ISymbolCacheRepository>().FindByID(symbolId);
                    if (symbol == null)
                    {
                        return null;   
                    }

                    if (this.ValidateOnValueDate(valueDate, isKvbSellCcy1 ? symbol.CCY1 : symbol.CCY2, buid, symbolId, out prompt))
                    {
                        return new BaseSymbolTenorVDVM()
                        {
                            BuId = buid,
                            LocalTradeDate = DateTime.MinValue,
                            ONCCY1VauleDate = DateTime.MinValue,
                            ONCCY2VauleDate = DateTime.MinValue,
                            SymbolID = symbolId,
                            SymbolName = string.Empty,
                            Tenor = TenorEnum.ON,
                            ValueDate = valueDate
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return new BaseSymbolTenorVDVM()
                               {
                                   BuId = buid,
                                   LocalTradeDate = DateTime.MinValue,
                                   ONCCY1VauleDate = DateTime.MinValue,
                                   ONCCY2VauleDate = DateTime.MinValue,
                                   SymbolID = symbolId,
                                   SymbolName = string.Empty,
                                   Tenor = tenor,
                                   ValueDate = valueDate
                               };
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// The is sopt tenor.
        /// </summary>
        /// <param name="symbolId">
        /// The symbol id.
        /// </param>
        /// <param name="valueDate">
        /// The value date.
        /// </param>
        /// <param name="buid">
        /// The buid.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsSoptTenor(string symbolId, DateTime valueDate, string buid)
        {
            string prmptstr;
            var spvaluedate = this.GetValueDateByTenor(buid, TenorEnum.SP, symbolId, out prmptstr);
            if (spvaluedate == DateTime.MinValue)
            {
                return false;
            }

            if (valueDate.Date == spvaluedate)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// The verify value date.
        /// </summary>
        /// <param name="symbolId">
        /// The symbol id.
        /// </param>
        /// <param name="valueDate">
        /// The value date.
        /// </param>
        /// <param name="buid">
        /// The buid.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool VerifyValueDate(string symbolId, DateTime valueDate, string buid)
        {
            var symbol = this.GetRepository<ISymbolCacheRepository>().FindByID(symbolId);
            if (symbol == null)
            {
                return false;
            }

            string prompt;
            var tenor = this.GetTenorByValueDate(buid, valueDate, symbolId, out prompt);
            if (prompt != Infrastructure.Common.ErrorMsg.Common.Success)
            {
                return false;
            }

            if (tenor == TenorEnum.ON)
            {
                if (this.ValidateOnValueDate(valueDate, symbol.CCY1, buid, symbolId, out prompt) &&
                    this.ValidateOnValueDate(valueDate, symbol.CCY2, buid, symbolId, out prompt))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// The verify value date we sell ccy.
        /// </summary>
        /// <param name="symbolId">
        /// The symbol id.
        /// </param>
        /// <param name="valueDate">
        /// The value date.
        /// </param>
        /// <param name="buid">
        /// The buid.
        /// </param>
        /// <param name="isKvbSellCcy1">
        /// The is kvb sell ccy 1.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool VerifyValueDateWeSellCCY(string symbolId, DateTime valueDate, string buid, bool isKvbSellCcy1)
        {
            var symbol = this.GetRepository<ISymbolCacheRepository>().FindByID(symbolId);
            if (symbol == null)
            {
                return false;
            }

            string prompt;
            var tenor = this.GetTenorByValueDate(buid, valueDate, symbolId, out prompt);
            if (prompt != Infrastructure.Common.ErrorMsg.Common.Success)
            {
                return false;
            }

            if (tenor == TenorEnum.ON)
            {
                var kvbsellccy = isKvbSellCcy1 ? symbol.CCY1 : symbol.CCY2;
                if (this.ValidateOnValueDate(valueDate, kvbsellccy, buid, symbolId, out prompt))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取货币的Spot交割日,只有在货币的Spot设置为T2时才会使用
        /// </summary>
        /// <param name="ccyId">
        /// 货币Id
        /// </param>
        /// <param name="symbol">
        /// 商品
        /// </param>
        /// <param name="localTradeDate">
        /// LocalTradeDate
        /// </param>
        /// <param name="localCcyId">
        /// 本币ID
        /// </param>
        /// <returns>
        /// 返回货币的交割日
        /// </returns>
        private DateTime GetCurrencySpotValueDateWhenT2(
            string ccyId, 
            SymbolModel symbol, 
            DateTime localTradeDate, 
            string localCcyId)
        {
            DateTime valueDateT1 = localTradeDate.Date.AddDays(1);

            while (true)
            {
                if (this.VerifySettleHolidayT1(ccyId, localCcyId, symbol, valueDateT1))
                {
                    break;
                }

                valueDateT1 = valueDateT1.AddDays(1);
            }

            return this.GetNextValueDate(symbol, valueDateT1);
        }

        /// <summary>
        /// 获取所传日期当月最后一个valueDate
        /// </summary>
        /// <param name="currentValueDate">
        /// 当前ValueDate
        /// </param>
        /// <param name="symbol">
        /// 商品
        /// </param>
        /// <returns>
        /// 目标ValueDate,如果为
        /// </returns>
        private DateTime GetCurrentMonthLastValueDate(DateTime currentValueDate, SymbolModel symbol)
        {
            var nexMonthDay = currentValueDate.AddMonths(1);

            var nextMonthFirstDay = new DateTime(nexMonthDay.Year, nexMonthDay.Month, 1);
            DateTime currentMonthLastDay = nextMonthFirstDay.AddDays(-1);
            int currentMonth = currentMonthLastDay.Month;

            while (true)
            {
                if (currentMonthLastDay.Month != currentMonth)
                {
                    Infrastructure.Log.TraceManager.Error.Write("ValueDate", "Cant find a valuedate in all mounth,Date:{0}", currentValueDate);
                    return DateTime.MinValue;
                }

                if (this.VerifySettleHoliday(symbol, currentMonthLastDay))
                {
                    return currentMonthLastDay.Date;
                }

                currentMonthLastDay = currentMonthLastDay.AddDays(-1);
            }
        }

        /// <summary>
        /// 获取上一个可用的交割日
        /// </summary>
        /// <param name="symbol">
        /// 商品
        /// </param>
        /// <param name="valueDate">
        /// 当前ValueDate
        /// </param>
        /// <returns>
        /// 下一个ValueDate
        /// </returns>
        private DateTime GetLastValueDate(SymbolModel symbol, DateTime valueDate)
        {
            // 有效交割日
            var nextValueDate = valueDate.AddDays(-1);

            // 循环便利，停止条件：循环次数 〉参数默认即使交割日
            // 防止出现异常，那么如果循环次数超过180则错误推出
            var loopCount = 0;
            while (true)
            {
                loopCount++;
                if (loopCount > 180)
                {
                    return DateTime.MinValue;
                }

                // 判断当前日期是否为有效交割日
                if (this.VerifySettleHoliday(symbol, nextValueDate))
                {
                    return nextValueDate.Date;
                }

                nextValueDate = nextValueDate.AddDays(-1);
            }
        }

        /// <summary>
        /// 获取下n月或者下年的ValueDate
        /// </summary>
        /// <param name="symbol">
        /// 商品
        /// </param>
        /// <param name="spotValueDate">
        /// SpotVaueDate
        /// </param>
        /// <param name="tenorMonthDay">
        /// SpotVaueDate在下月的对应日志，AddMounth已经会保证，如果下月不存在这天，则会赋值为下月的最后一天
        /// </param>
        /// <returns>
        /// 目标ValueDate,如果为时间最小值，则表示没有找到
        /// </returns>
        private DateTime GetMonthYearValueDate(SymbolModel symbol, DateTime spotValueDate, DateTime tenorMonthDay)
        {
            // 首先处理Spot日期和下月日期不是同一天的情况
            if (tenorMonthDay.Day != spotValueDate.Day)
            {
                return this.GetCurrentMonthLastValueDate(tenorMonthDay, symbol);
            }

            // 处理Spot的ValueDate是当月的最后一个交割日的情况，此需求有过调整，最早版本没有指明是交割日
            DateTime currentLastValueDate = this.GetCurrentMonthLastValueDate(spotValueDate, symbol);
            if (spotValueDate.Date == currentLastValueDate.Date)
            {
                return this.GetCurrentMonthLastValueDate(tenorMonthDay, symbol);
            }

            // 处理向后顺延或者向前的处理找到交割日
            var beforValueDate = this.GetNextValueDate(symbol, tenorMonthDay.AddDays(-1));
            if (beforValueDate.Month == tenorMonthDay.Month)
            {
                return beforValueDate.Date;
            }

            var afterValueDate = this.GetLastValueDate(symbol, tenorMonthDay);
            if (afterValueDate.Month == tenorMonthDay.Month)
            {
                return afterValueDate.Date;
            }

            return DateTime.MinValue;
        }

        /// <summary>
        /// 获取下一个可用的交割日
        /// </summary>
        /// <param name="symbol">
        /// 商品
        /// </param>
        /// <param name="valueDate">
        /// 当前ValueDate
        /// </param>
        /// <returns>
        /// 下一个ValueDate
        /// </returns>
        private DateTime GetNextValueDate(SymbolModel symbol, DateTime valueDate)
        {
            // 有效交割日
            var nextValueDate = valueDate.AddDays(1);

            // 循环便利，停止条件：循环次数 〉参数默认即使交割日
            // 防止出现异常，那么如果循环次数超过180则错误推出
            var loopCount = 0;
            while (true)
            {
                loopCount++;
                if (loopCount > 180)
                {
                    return DateTime.MinValue;
                }

                // 判断当前日期是否为有效交割日
                if (this.VerifySettleHoliday(symbol, nextValueDate))
                {
                    return nextValueDate.Date;
                }

                nextValueDate = nextValueDate.AddDays(1);
            }
        }

        /// <summary>
        /// 获取商品的SpotValueDate
        /// </summary>
        /// <param name="symbol">
        /// 商品
        /// </param>
        /// <param name="localTradeDate">
        /// LocalTradeDay
        /// </param>
        /// <param name="localCcyId">
        /// 本币ID
        /// </param>
        /// <returns>
        /// 返回对应的Spot的ValueDate
        /// </returns>
        private DateTime GetSpotValueDate(SymbolModel symbol, DateTime localTradeDate, string localCcyId)
        {
            // 处理SpotValueDate配置为T1的情况
            if (symbol.ValueDate == ValueDateEnum.T1)
            {
                return this.GetNextValueDate(symbol, localTradeDate);
            }

            // 获取CCY1的交割日
            DateTime ccy1ValueDate = this.GetCurrencySpotValueDateWhenT2(
                symbol.CCY1, 
                symbol, 
                localTradeDate, 
                localCcyId);

            // 获取CCY2的交割日
            DateTime ccy2ValueDate = this.GetCurrencySpotValueDateWhenT2(
                symbol.CCY2, 
                symbol, 
                localTradeDate, 
                localCcyId);

            if (ccy1ValueDate > ccy2ValueDate)
            {
                return ccy1ValueDate;
            }

            return ccy2ValueDate;
        }

        /// <summary>
        /// 根据Tenor获取ValueDate
        /// </summary>
        /// <param name="tenor">
        /// 远期类型
        /// </param>
        /// <param name="localTradeDate">
        /// LocalTradeDate
        /// </param>
        /// <param name="spotValueDate">
        /// SpotValueDay
        /// </param>
        /// <param name="symbol">
        /// 商品信息
        /// </param>
        /// <returns>
        /// 结果ValueDate,如果结果是时间最小值，为无法获取到ValueDate
        /// </returns>
        private DateTime GetValueDateByTenor(
            TenorEnum tenor, 
            DateTime localTradeDate, 
            DateTime spotValueDate, 
            SymbolModel symbol)
        {
            if (localTradeDate == DateTime.MinValue)
            {
                return DateTime.MinValue;
            }

            // 根据Spot的ValueDate获取其他Tenor的ValueDate
            switch (tenor)
            {
                case TenorEnum.ON:
                    return localTradeDate;
                case TenorEnum.TN:
                    if (symbol.ValueDate == ValueDateEnum.T1)
                    {
                        return DateTime.MinValue;
                    }

                    return this.GetNextValueDate(symbol, localTradeDate);
                case TenorEnum.SP:
                    return spotValueDate;
                case TenorEnum.SN:
                    return this.GetNextValueDate(symbol, spotValueDate);
                case TenorEnum.W1:
                    return this.GetNextValueDate(symbol, spotValueDate.AddDays(6));
                case TenorEnum.W2:
                    return this.GetNextValueDate(symbol, spotValueDate.AddDays(13));
                case TenorEnum.W3:
                    return this.GetNextValueDate(symbol, spotValueDate.AddDays(20));
                case TenorEnum.M1:
                    return this.GetMonthYearValueDate(symbol, spotValueDate, spotValueDate.AddMonths(1));
                case TenorEnum.M2:
                    return this.GetMonthYearValueDate(symbol, spotValueDate, spotValueDate.AddMonths(2));
                case TenorEnum.M3:
                    return this.GetMonthYearValueDate(symbol, spotValueDate, spotValueDate.AddMonths(3));
                case TenorEnum.M4:
                    return this.GetMonthYearValueDate(symbol, spotValueDate, spotValueDate.AddMonths(4));
                case TenorEnum.M5:
                    return this.GetMonthYearValueDate(symbol, spotValueDate, spotValueDate.AddMonths(5));
                case TenorEnum.M6:
                    return this.GetMonthYearValueDate(symbol, spotValueDate, spotValueDate.AddMonths(6));
                case TenorEnum.M7:
                    return this.GetMonthYearValueDate(symbol, spotValueDate, spotValueDate.AddMonths(7));
                case TenorEnum.M8:
                    return this.GetMonthYearValueDate(symbol, spotValueDate, spotValueDate.AddMonths(8));
                case TenorEnum.M9:
                    return this.GetMonthYearValueDate(symbol, spotValueDate, spotValueDate.AddMonths(9));
                case TenorEnum.M10:
                    return this.GetMonthYearValueDate(symbol, spotValueDate, spotValueDate.AddMonths(10));
                case TenorEnum.M11:
                    return this.GetMonthYearValueDate(symbol, spotValueDate, spotValueDate.AddMonths(11));
                case TenorEnum.Y1:
                    return this.GetMonthYearValueDate(symbol, spotValueDate, spotValueDate.AddMonths(12));
                default:
                    Infrastructure.Log.TraceManager.Error.Write("ValueDate", "Invalid tenor enum, value:{0}", tenor);
                    return DateTime.MinValue;
            }
        }

        /// <summary>
        /// 验证是否可以在BrokenDate交割
        /// </summary>
        /// <param name="valuedate">
        /// valuedate
        /// </param>
        /// <param name="busUnitId">
        /// 业务区标识
        /// </param>
        /// <param name="symbolId">
        /// 商品标识
        /// </param>
        /// <param name="rstPrompt">
        /// 返回操作的结果标识
        /// </param>
        /// <returns>
        /// 可以交割返回True,否则返回False
        /// </returns>
        /// <code>
        /// private bool ValidateBDValueDate(DateTime valuedate, string busUnitId, string symbolId, out string rstPrompt)
        /// </code>
        private bool ValidateBdValueDate(DateTime valuedate, string busUnitId, string symbolId, out string rstPrompt)
        {
            rstPrompt = Infrastructure.Common.ErrorMsg.Common.Success;

            var busUnit = this.businessUnitReps.FindByID(busUnitId);
            if (busUnit == null)
            {
                Infrastructure.Log.TraceManager.Debug.Write("ValueDate", "cant find business unit by id:{0}", busUnitId);
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.BU_NotExist;
                return false;
            }

            var symbol = this.symbolReps.FindByID(symbolId);
            if (symbol == null)
            {
                Infrastructure.Log.TraceManager.Warn.Write("ValueDate", "cant find symbol:{0}", symbolId);
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Symbol_NotExist;
                return false;
            }

            if (this.VerifySettleHoliday(symbol.SymbolModel, valuedate) == false)
            {
                Infrastructure.Log.TraceManager.Warn.Write("ValueDate", "varify value date is not passed");
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return false;
            }

            DateTime localSysTime = RunTime.GetCurrentRunTime().GetCurrentTimeForBu(busUnitId);

            // 根据当前本地系统时间，得到localTradeDate
            DateTime localTradeDate = busUnit.GetLocalTradeDate(localSysTime);
            if (localTradeDate == DateTime.MinValue)
            {
                Infrastructure.Log.TraceManager.Warn.Write("ValueDate", "cant find localtradedate");
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return false;
            }

            if (valuedate.Date <= localTradeDate.Date)
            {
                Infrastructure.Log.TraceManager.Warn.Write("ValueDate", "small than localtradedate.");
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return false;
            }

            // 获取Spot的ValueDate
            DateTime spotValueDate = this.GetSpotValueDate(symbol.SymbolModel, localTradeDate, busUnit.LocalCCYID);
            if (spotValueDate == DateTime.MinValue)
            {
                Infrastructure.Log.TraceManager.Warn.Write("ValueDate", "cant find sptvaluedate");
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return false;
            }

            if (valuedate.Date <= spotValueDate.Date)
            {
                Infrastructure.Log.TraceManager.Warn.Write("ValueDate", "small than spotValueDate.");
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return false;
            }

            var snvaluedate = this.GetValueDateByTenor(TenorEnum.SN, localTradeDate, spotValueDate, symbol.SymbolModel);
            if (snvaluedate == DateTime.MinValue)
            {
                Infrastructure.Log.TraceManager.Warn.Write("ValueDate", "cant find snValueDate");
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return false;
            }

            if (valuedate.Date <= snvaluedate.Date)
            {
                Infrastructure.Log.TraceManager.Warn.Write("ValueDate", "small than snvaluedate.");
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return false;
            }

            // 验证是否为一个区间ValueDate
            for (int i = 4; i < 19; i++)
            {
                var tenorItem = (TenorEnum)i;

                // 根据Spot的ValueDate获取其他Tenor的ValueDate
                DateTime tenorValueDate = this.GetValueDateByTenor(tenorItem, localTradeDate, spotValueDate, symbol.SymbolModel);
                if (tenorValueDate == DateTime.MinValue)
                {
                    Infrastructure.Log.TraceManager.Warn.Write(
                        "ValueDate",
                        "Cant get tenor value date, please check, when GetTenorByValueDate, Bu:{0}, symbol:{1}, tenor:{2}",
                        busUnitId, 
                        symbolId, 
                        tenorItem);
                    rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                    return false;
                }

                if (tenorValueDate > valuedate.Date)
                {
                    rstPrompt = Infrastructure.Common.ErrorMsg.Common.Success;
                    return true;
                }
            }

            rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
            return false;
        }

        /// <summary>
        /// 验证是否可以在当天交割
        /// </summary>
        /// <param name="valueDate">
        /// 待验证交割日
        /// </param>
        /// <param name="custBuyCcy">
        /// 客户Buy货币
        /// </param>
        /// <param name="busUnitId">
        /// 业务区标识
        /// </param>
        /// <param name="symbolId">
        /// 商品标识
        /// </param>
        /// <param name="rstPrompt">
        /// 返回操作的结果标识
        /// </param>
        /// <returns>
        /// 返回是否可以在当天交割
        /// </returns>
        /// <code>
        /// private bool ValidateONValueDate(string custBuyCCY, string busUnitId, string symbolId, out string rstPrompt)
        /// </code>
        private bool ValidateOnValueDate(
            DateTime valueDate, 
            string custBuyCcy, 
            string busUnitId, 
            string symbolId, 
            out string rstPrompt)
        {
            rstPrompt = Infrastructure.Common.ErrorMsg.Common.Success;

            var busUnit = this.businessUnitReps.FindByID(busUnitId);
            if (busUnit == null)
            {
                Infrastructure.Log.TraceManager.Debug.Write("ValueDate", "cant find business unit：{0}", busUnitId);
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.BU_NotExist;
                return false;
            }

            DateTime localSysTime = RunTime.GetCurrentRunTime().GetCurrentTimeForBu(busUnitId);

            if (!busUnit.IsBeforeCutoffTime(custBuyCcy, localSysTime, out rstPrompt)
                || !busUnit.IsBeforeSettleTime(localSysTime, out rstPrompt))
            {
                Infrastructure.Log.TraceManager.Debug.Write("ValueDate", "not before cutofftime");
                return false;
            }

            var symbol = this.symbolReps.FindByID(symbolId);
            if (symbol == null)
            {
                Infrastructure.Log.TraceManager.Debug.Write("ValueDate", "cant find symbol：{0}", symbolId);
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Symbol_NotExist;
                return false;
            }

            // 根据当前本地系统时间，得到localTradeDate
            DateTime localTradeDate = busUnit.GetLocalTradeDate(localSysTime);
            if (localTradeDate == DateTime.MinValue)
            {
                Infrastructure.Log.TraceManager.Debug.Write("ValueDate", "cant find localtradedate");
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return false;
            }

            if (localTradeDate.Date != valueDate.Date)
            {
                Infrastructure.Log.TraceManager.Debug.Write("ValueDate", "Valuedate dont equal localtradedate.");
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_ValueDateError;
                return false;
            }

            if (this.VerifySettleHoliday(symbol.SymbolModel, localTradeDate) == false)
            {
                Infrastructure.Log.TraceManager.Debug.Write("ValueDate", "verify value date is not passed");
                rstPrompt = Infrastructure.Common.ErrorMsg.Common.Deal_TenorError;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否为可以value date 的日期
        /// </summary>
        /// <param name="symbol">
        /// 针对商品
        /// </param>
        /// <param name="valueDate">
        /// 待判断交割日
        /// </param>
        /// <returns>
        /// 是否验证通过
        /// </returns>
        private bool VerifySettleHoliday(SymbolModel symbol, DateTime valueDate)
        {
            DayOfWeek dayOfWeek = valueDate.DayOfWeek;

            // 如果为周六、周日，为无效交割日
            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            var attentionUsdHolidayCcypairs = new List<string>();
            var generalSetting = this.genSettingsReps.Filter(o => true).FirstOrDefault();
            if (generalSetting == null)
            {
                Infrastructure.Log.TraceManager.Warn.Write(
                    "ValueDate", 
                    "Cant find GeneralSetting when VerifySettleHoliday, for symbol:{0}", 
                    symbol.SymbolName);
            }
            else
            {
                if (generalSetting.SymbolIncludeUSDHoliday != null)
                {
                    attentionUsdHolidayCcypairs.AddRange(generalSetting.SymbolIncludeUSDHoliday);
                }
            }

            // 获取基准日对应的交割节假日
            var currHoliday =
                this.GetRepository<ISettlementHolidayCacheRepository>()
                    .Filter(s => Convert.ToDateTime(s.Date).Date == valueDate.Date)
                    .FirstOrDefault();

            var usdCcy =
                this.GetRepository<ICurrencyCacheRepository>().GetUsdCurrencyId();

            if (currHoliday == null)
            {
                return true;
            }

            if (currHoliday.CurrencyList.Exists(c => c == "ALL"))
            {
                // 应该在外面加日志
                return false;
            }

            return
                !currHoliday.CurrencyList.Any(
                    c =>
                    c == symbol.CCY1 || c == symbol.CCY2
                    || (c == usdCcy && attentionUsdHolidayCcypairs.Contains(symbol.SymbolID)));
        }

        /// <summary>
        /// 计算当货币的Spot设置非T1时，用于验证某个日期是否可以作为货币对的T+1的ValueDate
        /// </summary>
        /// <param name="ccyId">
        /// 针对货币
        /// </param>
        /// <param name="localCcyId">
        /// 本币
        /// </param>
        /// <param name="symbol">
        /// 针对商品
        /// </param>
        /// <param name="localTradeDate">
        /// LocalTradeDate
        /// </param>
        /// <returns>
        /// 是否可以作为ValueDate
        /// </returns>
        private bool VerifySettleHolidayT1(string ccyId, string localCcyId, SymbolModel symbol, DateTime localTradeDate)
        {
            DayOfWeek dayOfWeek = localTradeDate.DayOfWeek;

            // 如果为周六、周日，为无效交割日
            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            // 获取基准日对应的交割节假日
            var currHoliday =
                this.GetRepository<ISettlementHolidayCacheRepository>()
                    .Filter(s => Convert.ToDateTime(s.Date).Date == localTradeDate.Date)
                    .FirstOrDefault();
            var usdCcy =
                this.GetRepository<ICurrencyCacheRepository>().Filter(o => o.CurrencyName.Equals("USD")).FirstOrDefault();

            if (currHoliday == null)
            {
                return true;
            }

            // 如果货币为USD或BU本币，不受交割节假日的约束
            if (ccyId.Equals(usdCcy.CurrencyID) || ccyId.Equals(localCcyId))
            {
                return true;
            }

            if (currHoliday.CurrencyList.Exists(c => c == "ALL"))
            {
                return false;
            }

            return currHoliday.CurrencyList.All(c => c != ccyId);
        }

        #endregion
    }
}