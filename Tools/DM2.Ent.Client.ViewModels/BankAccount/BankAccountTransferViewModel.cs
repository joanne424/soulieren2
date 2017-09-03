// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BankAccountTransferViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/25 09:44:11 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/25 09:44:11
//      修改描述：新建 BankAccountTransferViewModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels
{
    using System;
    using System.Windows;

    using Caliburn.Micro;

    using DestributeService.Seedwork;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Service;

    using GalaSoft.MvvmLight.Command;

    using Microsoft.Practices.Unity;

    /// <summary>
    ///     The counterparty add view model.
    /// </summary>
    public class BankAccountTransferViewModel : BankCashTransferModel
    {
        #region Fields
        /// <summary>
        ///     Window管理器
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        /// The business unit repository.
        /// </summary>
        private readonly IBusinessUnitRepository businessUnitRepository;

        /// <summary>
        /// The currency repository.
        /// </summary>
        private readonly ICurrencyRepository currencyRepository;

        /// <summary>
        ///     From Bank Account
        /// </summary>
        private BankAccountModel fromBankAccount;

        /// <summary>
        ///     to Bank Account
        /// </summary>
        private BankAccountModel toBankAccount;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BankAccountTransferViewModel" /> class.
        /// </summary>
        /// <param name="ownerId">
        ///     The owner id.
        /// </param>
        public BankAccountTransferViewModel()
        {
            this.DisplayName = RunTime.FindStringResource("BankCashTransfer");
            if (RunTime.GetCurrentRunTime().CurrentLoginUser == null)
            {
                return;
            }

            this.currencyRepository = this.GetRepository<ICurrencyRepository>();
            this.businessUnitRepository = this.GetRepository<IBusinessUnitRepository>();
            this.LocalTradeDate = RunTime.GetCurrentRunTime().GetCurrentTimeForCurrentUserBu();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the bank account.
        /// </summary>
        public BankAccountModel FromBankAccount
        {
            get
            {
                return this.fromBankAccount;
            }

            set
            {
                this.fromBankAccount = value;
                this.FromBankAccountId = value.Id;
                this.FromBuId = value.BusinessUnitId;
                this.FromCpId = value.InstitutionId;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     Gets or sets the bank account.
        /// </summary>
        public BankAccountModel ToBankAccount
        {
            get
            {
                return this.toBankAccount;
            }

            set
            {
                this.toBankAccount = value;
                this.ToBankAccountId = value.Id;
                this.ToBuId = value.BusinessUnitId;
                this.ToCpId = value.InstitutionId;
                this.NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     取消操作
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }

        /// <summary>
        ///     The find from acct bnt.
        /// </summary>
        public void FindFromAcctBnt()
        {
            FindBankAcctViewModel findBankAcct = this.ToBankAccount != null ? new FindBankAcctViewModel(this.ToBankAccount) : new FindBankAcctViewModel();

            this.windowManager.ShowDialog(findBankAcct);
            if (findBankAcct.BankAccount != null)
            {
                this.FromBankAccount = findBankAcct.BankAccount;
            }
        }

        /// <summary>
        ///     The find to acct bnt.
        /// </summary>
        public void FindToAcctBnt()
        {
            FindBankAcctViewModel findBankAcct = this.FromBankAccount != null ? new FindBankAcctViewModel(this.FromBankAccount) : new FindBankAcctViewModel();
            this.windowManager.ShowDialog(findBankAcct);
            if (findBankAcct.BankAccount != null)
            {
                this.ToBankAccount = findBankAcct.BankAccount;
            }
        }

        /// <summary>
        ///     关闭窗口
        /// </summary>
        public void OnClosed()
        {
            this.TryClose(true);
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        public void OnMinimizeWindowCommand(object window)
        {
            var win = window as Window;
            win.WindowState = WindowState.Minimized;
        }

        /// <summary>
        ///     The save.
        /// </summary>
        public void Save()
        {
            if (!this.ValidateforSumbit())
            {
                RunTime.ShowInfoDialogWithoutRes(this.Error, string.Empty, this.OwnerId);
                return;
            }

            string msg = string.Format(
                RunTime.FindStringResource("MSG_10048"),
                this.currencyRepository.GetName(this.FromBankAccount.CurrencyId),
                this.Amount.FormatAmountToStringByCurrencyId(this.FromBankAccount.CurrencyId),
                this.businessUnitRepository.GetName(this.FromBankAccount.BusinessUnitId),
                this.FromBankAccount.AccountNo,
                this.businessUnitRepository.GetName(this.ToBankAccount.BusinessUnitId),
                this.ToBankAccount.AccountNo);
            if (RunTime.ShowConfirmDialogWithoutRes(msg, string.Empty, this.OwnerId))
            {
                var service = new BankCashTransferService(this.OwnerId);
                CmdResult drs = service.Add(this);
                if (drs.Success)
                {
                    RunTime.ShowSuccessInfoDialogWithoutRes(
                        RunTime.FindStringResource("MSG_00001"),
                        string.Empty,
                        this.OwnerId);
                    this.Init();
                }
                else
                {
                    RunTime.ShowFailInfoDialog(drs.ErrorCode, string.Empty, this.OwnerId);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The on validated.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string OnValidated()
        {
            if (this.FromBankAccount == null || this.ToBankAccount == null)
            {
                return RunTime.FindStringResource("MSG_00010");
            }

            if (this.FromBankAccount.AccountNo == this.ToBankAccount.AccountNo)
            {
                return RunTime.FindStringResource("MSG_10049");
            }

            if (this.FromBankAccount.CurrencyId != this.ToBankAccount.CurrencyId)
            {
                return RunTime.FindStringResource("MSG_10050");
            }

            if (this.Amount <= 0)
            {
                return RunTime.FindStringResource("MSG_00009");
            }

            return string.Empty;
        }

        /// <summary>
        /// The init.
        /// </summary>
        private void Init()
        {
            this.fromBankAccount = null;
            this.toBankAccount = null;
            this.NotifyOfPropertyChange("FromBankAccount");
            this.NotifyOfPropertyChange("ToBankAccount");

            this.FromBankAccountId = string.Empty;
            this.FromBuId = string.Empty;
            this.FromCpId = string.Empty;

            this.ToBankAccountId = string.Empty;
            this.ToBuId = string.Empty;
            this.ToCpId = string.Empty;
            this.Amount = decimal.Zero;
            this.HedgingId = string.Empty;
            this.Comment = string.Empty;
            this.LocalTradeDate = RunTime.GetCurrentRunTime().GetCurrentTimeForCurrentUserBu();
        }

        #endregion
    }
}