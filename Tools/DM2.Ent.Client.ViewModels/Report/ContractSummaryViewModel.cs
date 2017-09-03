// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractSummaryViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels.Report
{
    using System;
    using System.Collections.Generic;

    using Caliburn.Micro;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;

    using Infrastructure.Common;

    /// <summary>
    ///     The contract summary view model.
    /// </summary>
    public class ContractSummaryViewModel : PropertyChangedBase, IComparable<ContractSummaryViewModel>
    {
        class ContractSummaryComparer : IComparer<ContractSummaryViewModel>
        {
            public int Compare(ContractSummaryViewModel x, ContractSummaryViewModel y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }

                //if (x.ContractId.ToInt32() < y.ContractId.ToInt32())
                //{
                //    return -1;
                //}
                //if (x.ContractId.ToInt32() > y.ContractId.ToInt32())
                //{
                //    return 1;
                //}
                var stringComparResult = string.Compare(x.ContractName, y.ContractName, StringComparison.OrdinalIgnoreCase);
                if (stringComparResult != 0)
                {
                    return stringComparResult;
                }

                if (x.ValueDate < y.ValueDate)
                {
                    return -1;
                }
                if (x.ValueDate > y.ValueDate)
                {
                    return 1;
                }

                return 0;
            }
        }

        #region Fields
        /// <summary>
        /// 比较器
        /// </summary>
        public static readonly IComparer<ContractSummaryViewModel> Comparer = new ContractSummaryComparer();

        /// <summary>
        ///     The ccy 1 amount.
        /// </summary>
        private decimal ccy1Amount;

        /// <summary>
        ///     the ccy1 Id
        /// </summary>
        private string ccy1Id;

        /// <summary>
        ///     The ccy 2 amount.
        /// </summary>
        private decimal ccy2Amount;

        /// <summary>
        ///     the ccy2 Id
        /// </summary>
        private string ccy2Id;

        /// <summary>
        ///     The contract id.
        /// </summary>
        private string contractId;

        private string contractName;

        /// <summary>
        ///     The forecast pl.
        /// </summary>
        private decimal forecastPl;

        /// <summary>
        ///     The forecast rate.
        /// </summary>
        private decimal forecastRate;

        /// <summary>
        ///     The market rate.
        /// </summary>
        private decimal marketRate;

        /// <summary>
        ///     The pl.
        /// </summary>
        private decimal pl;

        /// <summary>
        ///     The value date.
        /// </summary>
        private DateTime valueDate;

        //private RunTime currentRunTime;
        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the average rate.
        /// </summary>
        public decimal AverageRate
        {
            get
            {
                if (this.Ccy1Amount == decimal.Zero)
                {
                    return decimal.Zero;
                }
                return Math.Abs(this.Ccy2Amount / this.Ccy1Amount);
            }
        }

        /// <summary>
        ///     CCY1交易量
        /// </summary>
        public decimal Ccy1Amount
        {
            get
            {
                return this.ccy1Amount;
            }

            set
            {
                this.ccy1Amount = value;
                this.NotifyOfPropertyChange();
                this.NotifyOfPropertyChange("AverageRate");
            }
        }

        /// <summary>
        ///     CCY1ID
        /// </summary>
        public string Ccy1Id
        {
            get
            {
                return this.ccy1Id;
            }

            set
            {
                this.ccy1Id = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     CCY2交易量
        /// </summary>
        public decimal Ccy2Amount
        {
            get
            {
                return this.ccy2Amount;
            }

            set
            {
                this.ccy2Amount = value;
                this.NotifyOfPropertyChange();
                this.NotifyOfPropertyChange("AverageRate");
            }
        }

        /// <summary>
        ///     CCY2ID
        /// </summary>
        public string Ccy2Id
        {
            get
            {
                return this.ccy2Id;
            }

            set
            {
                this.ccy2Id = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     货币对id
        /// </summary>
        public string ContractId
        {
            get
            {
                return this.contractId;
            }

            set
            {
                this.contractId = value;
                this.NotifyOfPropertyChange();
            }
        }


        /// <summary>
        ///     货币对名称
        /// </summary>
        public string ContractName
        {
            get
            {
                return this.contractName;
            }

            set
            {
                this.contractName = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     推演盈亏
        /// </summary>
        public decimal ForecastPl
        {
            get
            {
                return this.forecastPl;
            }

            set
            {
                this.forecastPl = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     推演价格
        /// </summary>
        public decimal ForecastRate
        {
            get
            {
                return this.forecastRate;
            }

            set
            {
                this.forecastRate = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     市价
        /// </summary>
        public decimal MarketRate
        {
            get
            {
                return this.marketRate;
            }

            set
            {
                this.marketRate = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     盈亏
        /// </summary>
        public decimal Pl
        {
            get
            {
                return this.pl;
            }

            set
            {
                this.pl = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Value Date
        /// </summary>
        public DateTime ValueDate
        {
            get
            {
                return this.valueDate;
            }

            set
            {
                this.valueDate = value;
                this.NotifyOfPropertyChange();
            }
        }
        #endregion

        #region Public Methods and Operators

        public int CompareTo(ContractSummaryViewModel other)
        {
            return Comparer.Compare(this, other);
        }

        /// <summary>
        /// The update forecast rate.
        /// </summary>
        /// <param name="bid">
        /// The bid.
        /// </param>
        /// <param name="ask">
        /// The ask.
        /// </param>
        public void UpdateForecastRate(decimal bid, decimal ask, string transferCcyId, RunTime currentRunTime)
        {
            this.ForecastRate = this.ccy1Amount < decimal.Zero ? ask : bid;

            var pl = this.forecastRate == decimal.Zero
                               ? decimal.Zero
                               : (this.ccy1Amount * this.forecastRate) + this.ccy2Amount;
            if (this.ccy2Id == transferCcyId)
            {
                this.ForecastPl = pl;
                return;
            }

            var price = currentRunTime.GetTransferPrice(this.ccy2Id, transferCcyId);
            if (price == null || price.BelongContract == null || price.DsMid == decimal.Zero)
            {
                this.ForecastPl = pl;
                return;
            }

            if (price.BelongContract.Ccy1Id == transferCcyId)
            {
                this.ForecastPl = pl / price.DsMid;
                return;
            }

            this.ForecastPl = pl * price.DsMid;
        }

        public void UpdateAboutRate(string transferCcyId, RunTime currentRunTime)
        {
            var inputPrice = ForecastPriceDao.Instance.Get(this.ContractId);
            if (inputPrice != null)
            {
                this.ForecastRate = this.Ccy1Amount > decimal.Zero ? inputPrice.Bid : inputPrice.Ask;
            }

            var marketPrice = currentRunTime.GetCustomerDsRate(this.ContractId, this.ValueDate);
            if (marketPrice != null)
            {
                this.MarketRate =
                    (this.Ccy1Amount > decimal.Zero ? marketPrice.CustomerBid : marketPrice.CustomerAsk).ToFixed(
                        marketPrice.BelongContract.DecimalPlace);
            }


            var marketPl = this.marketRate == decimal.Zero
                               ? decimal.Zero
                               : (this.ccy1Amount * this.marketRate) + this.ccy2Amount;
            var estimatePl = this.forecastRate == decimal.Zero
                               ? decimal.Zero
                               : (this.ccy1Amount * this.forecastRate) + this.ccy2Amount;
            if (this.ccy2Id == transferCcyId)
            {
                this.Pl = marketPl;
                this.ForecastPl = estimatePl;
                return;
            }

            var price = currentRunTime.GetTransferPrice(this.ccy2Id, transferCcyId);
            if (price == null || price.BelongContract == null || price.DsMid == decimal.Zero)
            {
                this.Pl = decimal.Zero;
                this.ForecastPl = decimal.Zero;
                return;
            }

            if (price.BelongContract.Ccy1Id == transferCcyId)
            {
                this.Pl = marketPl / price.DsMid;
                this.ForecastPl = estimatePl / price.DsMid;
                return;
            }

            this.Pl = marketPl * price.DsMid;
            this.ForecastPl = estimatePl * price.DsMid;
        }
        #endregion
    }
}