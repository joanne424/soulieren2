// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModifyHedgeSpotForwardViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/18 05:30:30 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/18 05:30:30
//      修改描述：新建 ModifyHedgeSpotForwardViewModel.cs
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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using DestributeService.Seedwork;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;

    using GalaSoft.MvvmLight.Command;

    using Infrastructure.Common.Enums;
    using Infrastructure.Data;
    using Infrastructure.Utils;

    /// <summary>
    ///     The new hedge spot forward view model.
    /// </summary>
    public class ModifyHedgeSpotForwardViewModel : FxHedgingDealModel
    {
        #region Fields

        /// <summary>
        ///     The contract rep.
        /// </summary>
        private readonly IContractRepository contractRep;

        /// <summary>
        ///     货币仓储
        /// </summary>
        private readonly ICurrencyRepository currencyRep;

        /// <summary>
        ///     The current enterprise.
        /// </summary>
        private readonly EnterpriseModel currentEnterprise;

        /// <summary>
        ///     The business unit model.
        /// </summary>
        private BusinessUnitModel businessUnitModel;

        /// <summary>
        ///     Buy CCY index
        /// </summary>
        private int buyCCYIndex = -1;

        /// <summary>
        ///     The ccy list.
        /// </summary>
        private ObservableCollection<CurrencyModel> ccyList;

        /// <summary>
        ///     The comment.
        /// </summary>
        private string comment = string.Empty;

        /// <summary>
        ///     The hedge account.
        /// </summary>
        private CounterPartyModel counterParty;

        /// <summary>
        ///     The hedge accounts.
        /// </summary>
        private ObservableCollection<CounterPartyModel> counterPartys;

        /// <summary>
        ///     The value date.
        /// </summary>
        /// <summary>
        ///     The we buy amount.
        /// </summary>
        private decimal custBuyAmount;

        /// <summary>
        ///     The we buy ccy.
        /// </summary>
        private string custBuyCCY;

        /// <summary>
        ///     The we sell amount.
        /// </summary>
        private decimal custSellAmount;

        /// <summary>
        ///     The we sell ccy.
        /// </summary>
        private string custSellCCY;

        /// <summary>
        ///     The ent instrument.
        /// </summary>
        private Dictionary<int, string> entInstrument;

        // private TradableInstrumentEnum? instrument;
        /// <summary>
        ///     The instrument.
        /// </summary>
        /// <summary>
        ///     instrument 下拉列表
        /// </summary>
        private Dictionary<int, string> instrumentEnum;

        /// <summary>
        ///     The is we buy amount enable.
        /// </summary>
        private bool isCustBuyAmountEnable = true;

        /// <summary>
        ///     The is we sell amount enable.
        /// </summary>
        private bool isCustSellAmountEnable = true;

        /// <summary>
        ///     The is dealt wbccy.
        /// </summary>
        private bool isDealtBuyCCY;

        /// <summary>
        ///     The is hedge accounts enable.
        /// </summary>
        private bool isHedgeAccountsEnable;

        /// <summary>
        ///     The is open rate enable.
        /// </summary>
        private bool isOpenRateEnable;

        /// <summary>
        ///     Sell  CCy  选中的index
        /// </summary>
        private int sellCCYIndex = -1;

        private string title;

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
                this.NotifyOfPropertyChange();
            }
        }
        #endregion

        // private DateTime? valueDate;
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyHedgeSpotForwardViewModel"/> class.
        ///     Initializes a new instance of the <see cref="ModifySpotForwardViewModel"/> class.
        /// </summary>
        /// <param name="ownerID">
        /// The owner id.
        /// </param>
        public ModifyHedgeSpotForwardViewModel(FxHedgingDealModel model, string ownerID = null)
        {
            this.DisplayName = RunTime.FindStringResource("HedgeDeal") + " - " + model.Id;
            this.Title = RunTime.FindStringResource("HedgeDeal") + " - " + model.Id;
            this.Copy(model);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The cancel_ click.
        /// </summary>
        public void Cancel_Click()
        {
            this.TryClose();
        }

        /// <summary>
        ///     发布已取消事件
        /// </summary>
        /// <summary>
        ///     发布已确认事件
        /// </summary>
        /// <summary>
        ///     关闭窗口
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }
        #endregion
    }
}