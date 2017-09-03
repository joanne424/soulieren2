

namespace DM2.Ent.Client.ViewModels.Report
{
    using System.Collections.ObjectModel;
    using System.Linq;

    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;

    public class BankAccountDetailViewModel : BaseVm
    {
        private readonly IBankAccountRepository bankAccountRepository;

        private readonly ICounterPartyRepository counterPartyRepository;

        private ObservableCollection<BankAccountModel> aboutBankAccounts;


        public BankAccountDetailViewModel(string owmerId = null)
            : base(owmerId)
        {
            this.DisplayName = RunTime.FindStringResource("BankAccountTitle");
            this.bankAccountRepository = this.GetRepository<IBankAccountRepository>();
            this.counterPartyRepository = this.GetRepository<ICounterPartyRepository>();
        }


        public void InitWithSettlementAccount(string businessUnitId, string currencyId)
        {
            this.AboutBankAccounts = this.bankAccountRepository.Filter(
                acc => acc.Enabled && !string.IsNullOrEmpty(acc.CounterpartyId) && acc.CurrencyId == currencyId)
                .Join(
                    this.counterPartyRepository.Filter(cp => cp.BusinessUnitId == businessUnitId),
                    left => left.CounterpartyId,
                    right => right.Id,
                    (left, right) => left)
                .ToObservableCollection();
        }

        public void InitWithNonSettlementAccount(string businessUnitId, string currencyId)
        {
            this.AboutBankAccounts =
                this.bankAccountRepository.Filter(
                    acc =>
                    acc.Enabled && acc.BusinessUnitId == businessUnitId && acc.CurrencyId == currencyId
                    && string.IsNullOrEmpty(acc.CounterpartyId)).ToObservableCollection();
        }

        public ObservableCollection<BankAccountModel> AboutBankAccounts
        {
            get
            {
                return this.aboutBankAccounts;
            }

            set
            {
                this.aboutBankAccounts = value;
                this.NotifyOfPropertyChange();
            }
        }

        public void Close()
        {
            this.TryClose();
        }
    }
}
