

namespace DM2.Ent.Client.ViewModels.Counterparty
{
    using System.Linq;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Service;

    using Infrastructure.Common;

    /// <summary>
    /// The counterparty edit view model.
    /// </summary>
    public class CounterpartyEditViewModel : CounterpartyAddViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterpartyEditViewModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        public CounterpartyEditViewModel(string ownerId, CounterPartyModel model)
            : base(ownerId)
        {
            this.EnterpriseId = model.EnterpriseId;
            this.BusinessUnitId = model.BusinessUnitId;
            this.OnBusinessUnitChanged();

            this.InstitutionId = model.InstitutionId;
            this.ConnectionType = model.ConnectionType;
            this.CreationTime = model.CreationTime;
            this.FullName = model.FullName;
            this.GroupId = model.GroupId;
            this.Id = model.Id;
            this.LastUpdateTime = model.LastUpdateTime;
            this.Name = model.Name;
            this.Version = model.Version;
            this.EmailAddress = model.EmailAddress;
            this.EmailLoginName = model.EmailLoginName;
            this.EmailPassword = model.EmailPassword;
            this.EmailImportTemplate = model.EmailImportTemplate;
            this.EmailTimeZone = model.EmailTimeZone;

           var  bankAccountRepository = this.GetRepository<IBankAccountRepository>();

            foreach (var item in model.SettlementAccounts)
            {
                var settlementAccount = new SettlementAccountViewModel { CurrencyId = item.Key };
                settlementAccount.CurrencyName = this.Currencies.First(p => p.Id == item.Key).Name;

                var bankAccount = bankAccountRepository.FindByID(item.Value.ToString());
                this.Fill(settlementAccount, bankAccount);

                this.SettlementAccountList.Add(settlementAccount);
            }

            this.OnConnectionTypeChanged();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The on saved.
        /// </summary>
        public override void OnSaved()
        {
            this.IsReadied = true;

            if (!this.ValidateforSumbit())
            {
                return;
            }


            if (!RunTime.ShowConfirmDialog("MSG_00003", string.Empty, this.OwnerId))
            {
                return;
            }

            this.SettlementAccounts = this.SettlementAccountList.ToDictionary(
                item => item.CurrencyId,
                item => item.BankAccountId.ToInt32());

            var result = this.GetSevice<CounterPartyService>().UpdateCounterparty(this);

            if (result.Success)
            {
                RunTime.ShowSuccessInfoDialog("MSG_00001", string.Empty, this.OwnerId);
                this.Close();
            }
            else
            {
                RunTime.ShowFailInfoDialog(result.ErrorCode, string.Empty, this.OwnerId);
            }
        }

        #endregion
    }
}