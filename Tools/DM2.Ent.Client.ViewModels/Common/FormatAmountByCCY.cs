namespace DM2.Ent.Client.ViewModels.Common
{
    //public static class FormatAmount
    //{
    //    /// <summary>
    //    /// 货币仓储
    //    /// </summary>
    //    private static ICurrencyCacheRepository currencyCacheRepository;

    //    /// <summary>
    //    /// 货币仓储
    //    /// </summary>
    //    public static ICurrencyCacheRepository CurrencyCacheRepository
    //    {
    //        get
    //        {
    //            if (currencyCacheRepository == null)
    //            {
    //                var parameter = new ParameterOverrides { { "varOwnerId", string.Empty } };
    //                currencyCacheRepository = IOCContainer.Instance.Container.Resolve<ICurrencyCacheRepository>(parameter);
    //            }
    //            return currencyCacheRepository;
    //        }
    //        private set { currencyCacheRepository = value; }
    //    }

    //    /// <summary>
    //    /// 根据Currency设置格式化Amount
    //    /// </summary>
    //    /// <param name="ccyVM">BaseCurrencyVM</param>
    //    /// <param name="amountTemp">格式化后的Amount</param>
    //    /// <param name="amountStrTemp">格式化后的AmountStr</param>
    //    public static string FormatAmountByCCYID(this decimal amountTemp, string currencyId)
    //    {
    //        string amountStrTemp = amountTemp.ToString();
    //        if (string.IsNullOrEmpty(currencyId))
    //        {
    //            return amountStrTemp;
    //        }
    //        ICurrencyCacheRepository ccyRep = CurrencyCacheRepository;
    //        if (ccyRep == null)
    //        {
    //            return amountStrTemp;
    //        }
    //        var currency = ccyRep.FindByID(currencyId);
    //        if (currency == null)
    //        {
    //            return amountStrTemp;
    //        }
    //        return amountTemp.FormatAmountByCCY(currency);
    //    }

    //    /// <summary>
    //    /// 根据Currency设置格式化Amount
    //    /// </summary>
    //    /// <param name="ccyModel">BaseCurrencyVM</param>
    //    /// <param name="amountTemp">格式化后的Amount</param>
    //    /// <param name="amountStrTemp">格式化后的AmountStr</param>
    //    public static string FormatAmountByCCY(this decimal amountTemp, BaseCurrencyVM ccyModel)
    //    {
    //        string amountStrTemp = amountTemp.ToString();
    //        if (ccyModel == null)
    //        {
    //            return amountStrTemp;
    //        }
    //        try
    //        {
    //            string amountFormater = "#,##0.";
    //            for (int i = 0; i < ccyModel.AmountDecimals; i++)
    //            {
    //                amountFormater += "0";
    //            }

    //            //this.inputDetailAmount = value.FormatAmountByCCYConfig(ccyVM.RoundingMethod, ccyVM.AmountDecimals);
    //            if (ccyModel.RoundingMethod == RoundingEmun.Rounding)
    //            {
    //                amountTemp = Math.Round(amountTemp, ccyModel.AmountDecimals);
    //                amountStrTemp = amountTemp.ToString(amountFormater);
    //                amountTemp = System.Convert.ToDecimal(amountStrTemp);
    //            }
    //            else
    //            {
    //                amountStrTemp = amountTemp.ToString(amountFormater + "00");
    //                //string valueStr = this.Amount.ToString("f" + (ccyVM.AmountDecimals + 1).ToString());
    //                amountStrTemp = amountStrTemp.Substring(0, amountStrTemp.IndexOf('.') + ccyModel.AmountDecimals + 1);
    //                amountTemp = Convert.ToDecimal(amountStrTemp);
    //            }

    //            if (amountTemp < 0)
    //            {
    //                amountStrTemp = string.Format("({0})", amountStrTemp.Substring(1, amountStrTemp.Length - 1));
    //            }
    //        }
    //        catch
    //        {
    //            amountStrTemp = string.Empty;
    //        }
    //        return amountStrTemp;
    //    }
    //}
}
