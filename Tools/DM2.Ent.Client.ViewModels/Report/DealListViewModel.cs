// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DealListViewModel.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels.Report
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;

    using Infrastructure.Common.Enums;

    /// <summary>
    ///     The deal list view model.
    /// </summary>
    public class DealListViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        ///     The hedging deal repository.
        /// </summary>
        private readonly IFxHedgingDealRepository hedgingDealRepository;


        /// <summary>
        /// The about deals.
        /// </summary>
        private ObservableCollection<FxHedgingDealModel> aboutDeals;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DealListViewModel"/> class.
        /// </summary>
        /// <param name="owmerId">
        /// The owmer id.
        /// </param>
        public DealListViewModel(string owmerId = null)
            : base(owmerId)
        {
            this.DisplayName = RunTime.FindStringResource("HedgeDealList");
            this.hedgingDealRepository = this.GetRepository<IFxHedgingDealRepository>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the about deals.
        /// </summary>
        public ObservableCollection<FxHedgingDealModel> AboutDeals
        {
            get
            {
                return this.aboutDeals;
            }

            set
            {
                this.aboutDeals = value;
                this.NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The cancel.
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }

        /// <summary>
        /// The init with daily exposure by counter party.
        /// </summary>
        /// <param name="counterPartyId">
        /// The counter party id.
        /// </param>
        /// <param name="currencyId">
        /// The currency id.
        /// </param>
        /// <param name="valueDate">
        /// The value date.
        /// </param>
        public void InitWithDailyExposureByCounterParty(string counterPartyId, string currencyId, DateTime valueDate)
        {
            Func<FxHedgingDealModel, bool> predicate =
                deal =>
                deal.CounterpartyId == counterPartyId && deal.ValueDate <= valueDate && deal.Status == StatusEnum.OPERN
                && (deal.Ccy1Id == currencyId || deal.Ccy2Id == currencyId);

            this.AboutDeals =
                this.hedgingDealRepository.Filter(predicate)
                    .OrderBy(deal => deal.ContractId)
                    .ThenBy(deal => deal.ValueDate)
                    .ToObservableCollection();

            this.hedgingDealRepository.SubscribeAddEvent(
                deal =>
                    {
                        if (predicate(deal))
                        {
                            this.AboutDeals.Add(deal);
                        }
                    });
            this.hedgingDealRepository.SubscribeRemoveEvent(
                deal =>
                    {
                        FxHedgingDealModel item = this.AboutDeals.FirstOrDefault(m => m.Id == deal.Id);
                        if (item != null)
                        {
                            this.AboutDeals.Remove(item);
                        }
                    });
        }

        /// <summary>
        /// The init with daily exposure by counter party business unit.
        /// </summary>
        /// <param name="businessUnitId">
        /// The business unit id.
        /// </param>
        /// <param name="currencyId">
        /// The currency id.
        /// </param>
        /// <param name="valueDate">
        /// The value date.
        /// </param>
        public void InitWithDailyExposureByBusinessUnit(
            string businessUnitId, 
            string currencyId, 
            DateTime valueDate)
        {
            Func<FxHedgingDealModel, bool> predicate =
                deal =>
                deal.BusinessUnitId == businessUnitId && deal.ValueDate <= valueDate && deal.Status == StatusEnum.OPERN
                && (deal.Ccy1Id == currencyId || deal.Ccy2Id == currencyId);

            this.AboutDeals =
                this.hedgingDealRepository.Filter(predicate)
                    .OrderBy(deal => deal.CounterpartyId)
                    .ThenBy(deal => deal.ContractId)
                    .ThenBy(deal => deal.ValueDate)
                    .ToObservableCollection();

            this.hedgingDealRepository.SubscribeAddEvent(
                deal =>
                    {
                        if (predicate(deal))
                        {
                            this.AboutDeals.Add(deal);
                        }
                    });
            this.hedgingDealRepository.SubscribeRemoveEvent(
                deal =>
                    {
                        FxHedgingDealModel item = this.AboutDeals.FirstOrDefault(m => m.Id == deal.Id);
                        if (item != null)
                        {
                            this.AboutDeals.Remove(item);
                        }
                    });
        }

        /// <summary>
        /// The init with forecast.
        /// </summary>
        /// <param name="contractId">
        /// The contract id.
        /// </param>
        /// <param name="valueDate">
        /// The value date.
        /// </param>
        public void InitWithForecast(string contractId, DateTime valueDate)
        {
            Func<FxHedgingDealModel, bool> predicate =
                deal => deal.ContractId == contractId && deal.ValueDate == valueDate && deal.Status == StatusEnum.OPERN;

            this.AboutDeals =
                this.hedgingDealRepository.Filter(predicate)
                    .OrderBy(deal => deal.ContractId)
                    .ThenBy(deal => deal.ValueDate)
                    .ToObservableCollection();

            this.hedgingDealRepository.SubscribeAddEvent(
                deal =>
                    {
                        if (predicate(deal))
                        {
                            this.AboutDeals.Add(deal);
                        }
                    });
            this.hedgingDealRepository.SubscribeRemoveEvent(
                deal =>
                    {
                        FxHedgingDealModel item = this.AboutDeals.FirstOrDefault(m => m.Id == deal.Id);
                        if (item != null)
                        {
                            this.AboutDeals.Remove(item);
                        }
                    });
        }

        #endregion
    }
}