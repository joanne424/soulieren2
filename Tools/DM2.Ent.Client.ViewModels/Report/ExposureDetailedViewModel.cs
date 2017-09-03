

namespace DM2.Ent.Client.ViewModels.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;

    public class ExposureDetailedViewModel : BaseVm
    {

        public ExposureDetailedViewModel(string currencyId,
            string businessUnitId = null,
            DateTime? valueDate = null,
            bool enableHedgingDeal = true,
            bool enableBankAccount = false)
        {
            var hedgingDealRepository = this.GetRepository<IFxHedgingDealRepository>();
            var bankAccountRepository = this.GetRepository<IBankAccountRepository>();


            if (enableHedgingDeal)
            {
                this.AboutHedgingDeals = FindHedgingDeals(hedgingDealRepository, currencyId, businessUnitId, valueDate);
            }

            if (enableBankAccount)
            {
                this.AboutBankAccounts = FindBankAccounts(bankAccountRepository, currencyId, businessUnitId);
            }
        }


        private static IList<FxHedgingDealModel> FindHedgingDeals(
            IFxHedgingDealRepository hedgingDealRepository,
            string currencyId,
            string businessUnitId = null,
            DateTime? valueDate = null)
        {
            var list = hedgingDealRepository.Filter(p => p.Ccy1Id == currencyId || p.Ccy2Id == currencyId);
            if (!string.IsNullOrEmpty(businessUnitId))
            {
                list = list.Where(p => p.BusinessUnitId == businessUnitId);
            }

            if (valueDate.HasValue)
            {
                list = list.Where(p => p.ValueDate <= valueDate.Value);
            }


            return list.ToList();
        }

        private static IList<BankAccountModel> FindBankAccounts(
            IBankAccountRepository bankAccountRepository,
            string currencyId,
            string businessUnitId = null)
        {
            var list =
                bankAccountRepository.Filter(
                    p => p.Enabled && p.CurrencyId == currencyId && p.AvailableBalance != decimal.Zero);
            if (!string.IsNullOrEmpty(businessUnitId))
            {
                list = list.Where(p => p.BusinessUnitId == businessUnitId);
            }

            return list.ToList();
        }


        /// <summary>
        /// Gets or sets the about deals.
        /// </summary>
        public IList<FxHedgingDealModel> AboutHedgingDeals { get; set; }

        /// <summary>
        /// Gets or sets the about deals.
        /// </summary>
        public IList<BankAccountModel> AboutBankAccounts { get; set; }

        /// <summary>
        ///     关闭操作
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }
    }
}
