// <copyright file="SellBankAcctBalanceVM.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>zoukp</author>
// <date> 2017/07/06 02:54:42 </date>
// <summary>  </summary>
// <modify>
//      修改人：zoukp
//      修改时间：2017/07/06 02:54:42
//      修改描述：新建 SellBankAcctBalanceVM.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace DM2.Ent.Client.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;

    using Infrastructure.Common.Enums;

    /// <summary>
    ///     交易支出货币余额信息
    /// </summary>
    public class SellBankAcctBalanceVM : BaseVm
    {
        #region Fields

        /// <summary>
        ///     The deal rep.
        /// </summary>
        private readonly IFxHedgingDealRepository dealRep;

        /// <summary>
        ///     The bank account.
        /// </summary>
        private BankAccountModel bankAccount;

        /// <summary>
        ///     The currency id.
        /// </summary>
        private CurrencyModel currency;

        /// <summary>
        ///     The today balance.
        /// </summary>
        private decimal todayBalance;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SellBankAcctBalanceVM" /> class.
        ///     Prevents a default instance of the <see cref="SellBankAcctBalanceVM" /> class from being created.
        /// </summary>
        public SellBankAcctBalanceVM()
        {
            this.dealRep = this.GetRepository<IFxHedgingDealRepository>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the bank account.
        /// </summary>
        public BankAccountModel BankAccount
        {
            get
            {
                return this.bankAccount;
            }

            set
            {
                this.bankAccount = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the currency id.
        /// </summary>
        public CurrencyModel Currency
        {
            get
            {
                return this.currency;
            }

            set
            {
                this.currency = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the today balance.
        /// </summary>
        public string TodayBalance
        {
            get
            {
                return this.todayBalance.ToString();
            }

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.todayBalance = decimal.Zero;
                }
                else
                {
                    this.todayBalance = Convert.ToDecimal(value);
                }

                this.NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The calc today balance.
        /// </summary>
        /// <param name="valueDay">
        /// The local Day.
        /// </param>
        /// <param name="counterpartyId">
        /// The counterparty Id.
        /// </param>
        public void CalcTodayBalance(DateTime valueDay, string counterpartyId)
        {
            if (this.BankAccount == null)
            {
                this.TodayBalance = string.Empty;
                return;
            }

            if (this.Currency == null || valueDay == default(DateTime))
            {
                this.todayBalance = this.BankAccount.AvailableBalance;
                this.NotifyOfPropertyChange("TodayBalance");
                return;
            }

            var dealsTemp =
                this.dealRep.Filter(
                    o =>
                    o.Status == StatusEnum.OPERN && o.ValueDate.Date <= valueDay.Date
                    && o.CounterpartyId == counterpartyId);
            decimal buyAmount = dealsTemp.Where(o => o.BuyCCY == this.Currency.Id).Sum(o => o.BuyAmount);
            decimal sellAmount = dealsTemp.Where(o => o.SellCCY == this.Currency.Id).Sum(o => o.SellAmount);
            this.todayBalance = this.BankAccount.AvailableBalance + buyAmount - sellAmount;
            this.NotifyOfPropertyChange("TodayBalance");
        }

        #endregion
    }
}