

namespace DM2.Ent.Client.ViewModels.BankAccount
{
    using DM2.Ent.Presentation.Models;

    using Infrastructure.Common.Enums;

    public class PreviewActualCashViewModel : NewActualCashViewModel
    {
        public PreviewActualCashViewModel(string ownerId, ActualCashModel model)
            : base(ownerId)
        {
            var bankAccount = this.GetRepository<IBankAccountRepository>().FindByID(model.BankAccountId);
            if (bankAccount != null)
            {
                this.BankAccountNo = bankAccount.AccountNo;
                this.BankAccountName = string.Format("{0} - {1}", bankAccount.AccountNo, bankAccount.AccountName);
                this.BankAvailableBalance = bankAccount.AvailableBalance;
            }
            var instrument = this.GetRepository<IInstrumentRepository>().FindByID(model.InstrumentId);
            if (instrument != null)
            {
                this.TradableInstrument = instrument.TradableInstrument;

                this.DisplayName = string.Format("{0} {1}", instrument.Name, this.Id);

                this.OnTradableInstrumentChanged();
            }


            this.Id = model.Id;
            this.BankAccountId = model.BankAccountId;
            this.BusinessUnitId = model.BusinessUnitId;
            this.EnterpriseId = model.EnterpriseId;
            this.InstitutionId = model.InstitutionId;
            this.CurrencyId = model.CurrencyId;
            this.InstrumentId = model.InstrumentId;
            this.PaymentChecked = model.SignType == SignTypeEnum.Payment;
            this.ReceiptChecked = model.SignType == SignTypeEnum.Receipt;
            this.Amount = model.Amount;
            this.HedgeDealId = model.HedgeDealId;
            this.LocalTradeDate = model.LocalTradeDate;
            this.Status = model.Status;
            this.UserId = model.UserId;
            this.StaffId = model.StaffId;
            this.Comment = model.Comment;
            this.CreationTime = model.CreationTime;
            this.LastUpdateTime = model.LastUpdateTime;
            this.Version = model.Version;
        }
    }
}
