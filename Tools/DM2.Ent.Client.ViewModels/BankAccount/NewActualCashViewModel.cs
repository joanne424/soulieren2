// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActualCashViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DM2.Ent.Client.ViewModels.BankAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Caliburn.Micro;

    using DestributeService.Seedwork;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Service;

    using Infrastructure.Common;
    using Infrastructure.Common.Enums;

    using Microsoft.Practices.Unity;

    /// <summary>
    /// The actual cash view model.
    /// </summary>
    public class NewActualCashViewModel : ActualCashModel
    {
        #region Fields

        /// <summary>
        ///     所属企业
        /// </summary>
        private readonly EnterpriseModel enterprise;

        /// <summary>
        ///     Window管理器
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        private string bankAccountNo;

        private string bankAccountName;

        private decimal bankAvailableBalance;


        //private 

        /// <summary>
        /// The instruments.
        /// </summary>
        private IDictionary<string, string> instruments;

        /// <summary>
        /// The payment checked.
        /// </summary>
        private bool paymentChecked;

        /// <summary>
        /// The receipt checked.
        /// </summary>
        private bool receiptChecked;

        /// <summary>
        /// The tradable instrument.
        /// </summary>
        private TradableInstrumentEnum tradableInstrument;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NewActualCashViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        public NewActualCashViewModel(string ownerId)
            : base(ownerId)
        {
            this.paymentChecked = true;

            this.TradableInstruments = InstrumentTool.GetTradableInstruments(BusinessTypeEnum.ACTUAL_CASH);

            this.EnterpriseId = RunTime.GetCurrentRunTime().CurrentLoginUser.EntId;

            this.LocalTradeDate = DateTime.Today;

            this.enterprise = this.GetRepository<IEnterpriseRepository>().FindByID(this.EnterpriseId);

            this.IsReadied = false;
        }

        #endregion

        #region Public Properties

        public string BankAccountNo
        {
            get
            {
                return this.bankAccountNo;
            }
            set
            {
                this.bankAccountNo = value;
                this.NotifyOfPropertyChange();
            }
        }

        public string BankAccountName
        {
            get
            {
                return this.bankAccountName;
            }
            set
            {
                this.bankAccountName = value;
                this.NotifyOfPropertyChange();
            }
        }

        public decimal BankAvailableBalance
        {
            get
            {
                return this.bankAvailableBalance;
            }
            set
            {
                this.bankAvailableBalance = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     业务方式列表
        /// </summary>
        public IDictionary<string, string> Instruments
        {
            get
            {
                return this.instruments;
            }

            set
            {
                this.instruments = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     选中Payment
        /// </summary>
        public bool PaymentChecked
        {
            get
            {
                return this.paymentChecked;
            }

            set
            {
                this.paymentChecked = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     选中Receipt
        /// </summary>
        public bool ReceiptChecked
        {
            get
            {
                return this.receiptChecked;
            }

            set
            {
                this.receiptChecked = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     选中的业务类型
        /// </summary>
        public TradableInstrumentEnum TradableInstrument
        {
            get
            {
                return this.tradableInstrument;
            }

            set
            {
                this.tradableInstrument = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     业务类型列表
        /// </summary>
        public IDictionary<TradableInstrumentEnum, string> TradableInstruments { get; set; }


        /// <summary>
        /// 页面数据是否准备就绪
        /// </summary>
        protected bool IsReadied { get; set; }
        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     取消操作
        /// </summary>
        public void Cancel()
        {
            this.TryClose();
        }

        /// <summary>
        /// The find bank account.
        /// </summary>
        public void FindBankAccount()
        {
            var findBankAcct = new FindBankAcctViewModel();
            this.windowManager.ShowDialog(findBankAcct);
            if (findBankAcct.BankAccount != null)
            {
                this.BankAccountNo = findBankAcct.BankAccount.AccountNo;
                this.BankAccountName = string.Format("{0} - {1}", findBankAcct.BankAccount.AccountNo, findBankAcct.BankAccount.AccountName);
                this.BankAvailableBalance = findBankAcct.BankAccount.AvailableBalance;
                this.BankAccountId = findBankAcct.BankAccount.Id;
                this.BusinessUnitId = findBankAcct.BankAccount.BusinessUnitId;
                this.CurrencyId = findBankAcct.BankAccount.CurrencyId;
                this.InstitutionId = findBankAcct.BankAccount.InstitutionId;
            }
        }

        /// <summary>
        ///     关闭窗口
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }

        /// <summary>
        /// The on payment checked.
        /// </summary>
        public void OnPaymentChecked()
        {
            this.PaymentChecked = true;
            this.ReceiptChecked = false;

            this.SignType = SignTypeEnum.Payment;
        }

        /// <summary>
        /// The on receipt checked.
        /// </summary>
        public void OnReceiptChecked()
        {
            this.PaymentChecked = false;
            this.ReceiptChecked = true;

            this.SignType = SignTypeEnum.Receipt;
        }

        /// <summary>
        ///     The on saved.
        /// </summary>
        public void OnSaved()
        {
            this.IsReadied = true;

            if (!this.ValidateforSumbit())
            {
                return;
            }

            var prompt = string.Format(RunTime.FindStringResource("MSG_10047"), 
                this.GetRepository<IBusinessUnitRepository>().GetName(this.BusinessUnitId),
                this.BankAccountNo, this.Instruments[this.InstrumentId], 
                this.GetRepository<ICurrencyRepository>().GetName(this.CurrencyId),
                this.Amount.FormatAmountToStringByCurrencyId(this.CurrencyId), RunTime.FindStringResource(this.SignType.ToString()));

            if (!RunTime.ShowConfirmDialogWithoutRes(prompt, string.Empty, this.OwnerId))
            {
                return;
            }

            CmdResult result = this.GetSevice<ActualCashService>().Add(this);

            if (result.Success)
            {
                RunTime.ShowSuccessInfoDialog("MSG_00001", string.Empty, this.OwnerId);
                this.Reset();
            }
            else
            {
                RunTime.ShowFailInfoDialog(result.ErrorCode, string.Empty, this.OwnerId);
            }
        }

        /// <summary>
        /// The on tradable instrument changed.
        /// </summary>
        public void OnTradableInstrumentChanged()
        {
            if (this.enterprise == null)
            {
                return;
            }

            this.Instruments = InstrumentTool.GetInstruments(this.tradableInstrument, this.enterprise);
        }
        #endregion

        #region Methods

        /// <summary>
        /// The on validate.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        protected override string OnValidate(string propertyName)
        {
            if (!this.IsReadied)
            {
                return null;
            }

            if (propertyName == "BankAccountName" && string.IsNullOrEmpty(this.BankAccountName))
            {
                return RunTime.FindStringResource("MSG_00010");
            }

            if (propertyName == "InstrumentId" && string.IsNullOrEmpty(this.InstrumentId))
            {
                return RunTime.FindStringResource("MSG_00010");
            }

            if (propertyName == "Amount" && this.Amount <= decimal.Zero)
            {
                return RunTime.FindStringResource("MSG_00009");
            }

            return null;
        }

        /// <summary>
        /// The get tradable instrument name.
        /// </summary>
        /// <param name="tradableInstrument">
        /// The tradable instrument.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetTradableInstrumentName(TradableInstrumentEnum tradableInstrument)
        {
            string resourceKey = string.Format("TradableInstrumentEnum.{0}", tradableInstrument);
            return RunTime.FindStringResource(resourceKey);
        }

        /// <summary>
        ///     重置
        /// </summary>
        private void Reset()
        {
            this.IsReadied = false;

            this.BankAccountNo = string.Empty;
            this.BankAccountName = string.Empty;
            this.BankAvailableBalance = decimal.Zero;
            this.InstrumentId = string.Empty;
            this.Amount = decimal.Zero;
            this.HedgeDealId = string.Empty;
            this.Comment = string.Empty;
            this.BankAccountId = string.Empty;
            this.BusinessUnitId = string.Empty;
            this.CurrencyId = string.Empty;
            this.InstitutionId = string.Empty;
            this.StaffId = string.Empty;
            this.UserId = string.Empty;
        }

        #endregion
    }
}