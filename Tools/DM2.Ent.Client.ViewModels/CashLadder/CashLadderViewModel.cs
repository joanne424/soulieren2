// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CashLadderViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/21 10:33:08 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/21 10:33:08
//      修改描述：新建 CashLadderViewModel.cs
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
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Caliburn.Micro;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.ViewModels.Common;
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;

    using GalaSoft.MvvmLight.Command;

    using Infrastructure.Common.Enums;
    using Infrastructure.Data;

    using Microsoft.Practices.Unity;

    /// <summary>
    ///     The cash ladder view model.
    /// </summary>
    public class CashLadderViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        ///     The bu rep.
        /// </summary>
        private readonly IBusinessUnitRepository buRep;

        /// <summary>
        ///     The counterparty rep.
        /// </summary>
        private readonly ICounterPartyRepository counterpartyRep;

        /// <summary>
        ///     The currency rep.
        /// </summary>
        private readonly ICurrencyRepository currencyRep;

        /// <summary>
        ///     The deal rep.
        /// </summary>
        private readonly IFxHedgingDealRepository dealRep;

        /// <summary>
        ///     window消息命令
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        ///     The business unit.
        /// </summary>
        private BusinessUnitModel businessUnit;

        /// <summary>
        ///     The business unit list.
        /// </summary>
        private ObservableCollection<BusinessUnitModel> businessUnitList;

        /// <summary>
        ///     动态生成By Position Rollover Tab页面的内容
        /// </summary>
        private StackPanel cashLadderContent;

        /// <summary>
        ///     counterParty.
        /// </summary>
        private CounterPartyModel counterparty;

        /// <summary>
        ///     The hedge accounts.
        /// </summary>
        private ObservableCollection<CounterPartyModel> counterpartys;

        /// <summary>
        ///     By Position Rollover 列表
        /// </summary>
        private List<ByPositionRolloverItem> positionRolloverItems;

        /// <summary>
        ///     The value date from.
        /// </summary>
        private DateTime? valueDateFrom;

        /// <summary>
        ///     The value date to.
        /// </summary>
        private DateTime? valueDateTo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CashLadderViewModel" /> class.
        /// </summary>
        public CashLadderViewModel()
        {
            this.DisplayName = RunTime.FindStringResource("CashLadder");
            this.dealRep = this.GetRepository<IFxHedgingDealRepository>();
            this.buRep = this.GetRepository<IBusinessUnitRepository>();
            this.currencyRep = this.GetRepository<ICurrencyRepository>();
            this.counterpartyRep = this.GetRepository<ICounterPartyRepository>();
            this.BuSelectionChangedCommand = new RelayCommand(this.BuSelectionChanged);
            if (this.cashLadderContent == null)
            {
                this.cashLadderContent = new StackPanel { Orientation = Orientation.Horizontal };
            }

            Task.Factory.StartNew(RunTime.GetCurrentRunTime().CurrentRepositoryCore.WaitAllInitial)
                .ContinueWith(this.WaitLoad);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     CounterParty 选中事件
        /// </summary>
        public RelayCommand BuSelectionChangedCommand { get; private set; }

        /// <summary>
        ///     Gets or sets the business unit.
        /// </summary>
        public BusinessUnitModel BusinessUnit
        {
            get
            {
                return this.businessUnit;
            }

            set
            {
                if (value == null || string.IsNullOrEmpty(value.Name))
                {
                    this.businessUnit = null;
                    this.NotifyOfPropertyChange();
                    return;
                }

                this.businessUnit = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     symbolList 货币对列表
        /// </summary>
        public ObservableCollection<BusinessUnitModel> BusinessUnitList
        {
            get
            {
                return this.businessUnitList;
            }

            set
            {
                this.businessUnitList = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        ///     动态生成CashLadderContent Tab页面的内容
        /// </summary>
        public StackPanel CashLadderContent
        {
            get
            {
                return this.cashLadderContent;
            }

            set
            {
                this.cashLadderContent = value;
                this.NotifyOfPropertyChange(() => this.CashLadderContent);
            }
        }

        /// <summary>
        ///     HedgeAccount实体
        /// </summary>
        public CounterPartyModel Counterparty
        {
            get
            {
                return this.counterparty;
            }

            set
            {
                if (value == null || string.IsNullOrEmpty(value.Name))
                {
                    this.counterparty = null;
                    this.NotifyOfPropertyChange(() => this.Counterparty);
                    return;
                }

                this.counterparty = value;
                this.NotifyOfPropertyChange(() => this.Counterparty);
            }
        }

        /// <summary>
        ///     对冲账户列表
        /// </summary>
        public ObservableCollection<CounterPartyModel> Counterpartys
        {
            get
            {
                return this.counterpartys;
            }

            set
            {
                this.counterpartys = value;
                this.NotifyOfPropertyChange(() => this.Counterpartys);
            }
        }

        /// <summary>
        ///     Gets or sets the value date from.
        /// </summary>
        public DateTime? ValueDateFrom
        {
            get
            {
                return this.valueDateFrom;
            }

            set
            {
                this.valueDateFrom = value;
                this.NotifyOfPropertyChange(() => this.ValueDateFrom);
            }
        }

        /// <summary>
        ///     Gets or sets the value date to.
        /// </summary>
        public DateTime? ValueDateTo
        {
            get
            {
                return this.valueDateTo;
            }

            set
            {
                this.valueDateTo = value;
                this.NotifyOfPropertyChange(() => this.ValueDateTo);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The Close
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }

        /// <summary>
        ///     The search.
        /// </summary>
        public void Search()
        {
            ObservableCollection<FxHedgingDealModel> list = this.dealRep.GetBindCollection();

            this.CalcByPositionRolloverData(list);
            this.CreateControl(this.ValueDateFrom, this.ValueDateTo);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        private void BuSelectionChanged()
        {
            if (this.Counterpartys.Any())
            {
                this.Counterpartys.Clear();
            }

            if (this.BusinessUnit == null)
            {
                this.Counterpartys = this.counterpartyRep.GetBindCollection().ToComboboxBinding(true);
            }
            else
            {
                this.Counterpartys =
                    this.counterpartyRep.Filter(o => o.BusinessUnitId == this.BusinessUnit.Id)
                        .ToObservableCollection()
                        .ToComboboxBinding(true);
            }
        }

        /// <summary>
        /// (重新)计算By Position Rollover的数据
        /// </summary>
        /// <param name="deals">
        /// 订单列表
        /// </param>
        private void CalcByPositionRolloverData(IEnumerable<FxHedgingDealModel> deals)
        {
            if (!deals.Any())
            {
                return;
            }

            // 清理原来的数据，并重新计算
            this.positionRolloverItems.Clear();

            var dealTemp = new List<DealGroupByCCY>();
            IOrderedEnumerable<FxHedgingDealModel> dealListTemp = this.SearchDeals(deals);
            foreach (FxHedgingDealModel item in dealListTemp)
            {
                if (item.TransactionType == TransactionTypeEnum.Buy)
                {
                    dealTemp.Add(
                        new DealGroupByCCY
                            {
                                Amount = item.Ccy1Amount,
                                CCY = item.Ccy1Id,
                                DealID = item.Id,
                                ValueDate = item.ValueDate
                            });
                    dealTemp.Add(
                        new DealGroupByCCY
                            {
                                Amount = 0 - item.Ccy2Amount,
                                CCY = item.Ccy2Id,
                                DealID = item.Id,
                                ValueDate = item.ValueDate
                            });
                }
                else
                {
                    dealTemp.Add(
                        new DealGroupByCCY
                            {
                                Amount = 0 - item.Ccy1Amount,
                                CCY = item.Ccy1Id,
                                DealID = item.Id,
                                ValueDate = item.ValueDate
                            });
                    dealTemp.Add(
                        new DealGroupByCCY
                            {
                                Amount = item.Ccy2Amount,
                                CCY = item.Ccy2Id,
                                DealID = item.Id,
                                ValueDate = item.ValueDate
                            });
                }
            }

            // 根据CCY分组
            IEnumerable<IGrouping<string, DealGroupByCCY>> items = dealTemp.GroupBy(item => item.CCY);
            foreach (var groupItem in items)
            {
                // 根据ValueDate分组
                IEnumerable<IGrouping<DateTime, DealGroupByCCY>> dateGroups = groupItem.GroupBy(p => p.ValueDate.Date);

                // 创建ByPositionRolloverItem对象
                var ccyItem = new ByPositionRolloverItem
                                  {
                                      CurrencyId = groupItem.Key,
                                      CurrencyName = this.currencyRep.GetName(groupItem.Key)
                                  };
                foreach (var dataItem in dateGroups)
                {
                    var dealLadderUnit = new DealLadderUnitVM { CurrencyID = groupItem.Key };

                    decimal amount = decimal.Zero;
                    foreach (DealGroupByCCY item in dataItem)
                    {
                        amount += item.Amount;
                        dealLadderUnit.RelateDealList.Add(item.DealID);
                    }

                    dealLadderUnit.LadderAmount = amount;
                    dealLadderUnit.LadderDay = dataItem.Key.Date;
                    ccyItem.DealLadderUnitList.Add(dealLadderUnit);
                }

                ccyItem.TotalAmount = ccyItem.DealLadderUnitList.Sum(o => o.LadderAmount);
                this.positionRolloverItems.Add(ccyItem);
            }
        }

        /// <summary>
        /// 将交易单列表转换为报表数据列表
        /// </summary>
        /// <summary>
        /// The create control.
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        private void CreateControl(DateTime? from = null, DateTime? to = null)
        {
            DataTable dataTable = this.CreateDataTable(from, to);
            RunTime.GetCurrentRunTime().ActionOnUiThread(
                () =>
                {
                    DataGrid dataGrid = CashLadderDataGridTool.CreateDataGrid(dataTable, this.ShowDetail);
                    this.cashLadderContent.Children.Clear();
                    if (dataGrid != null)
                    {
                        this.cashLadderContent.Children.Add(dataGrid);
                        this.NotifyOfPropertyChange(() => this.CashLadderContent);
                    }
                });
        }

        /// <summary>
        /// 创建数据源
        /// </summary>
        /// <param name="from">
        /// The from.
        /// </param>
        /// <param name="to">
        /// The to.
        /// </param>
        /// <returns>
        /// 数据源
        /// </returns>
        private DataTable CreateDataTable(DateTime? from = null, DateTime? to = null)
        {
            // 基础数据
            if (this.positionRolloverItems == null || this.positionRolloverItems.Count == 0)
            {
                return null;
            }

            // 所有可用的ValueDate日期(HashSet去重)
            var valueDateHashSet = new HashSet<DateTime>();
            foreach (ByPositionRolloverItem positionRolloverItem in this.positionRolloverItems)
            {
                foreach (DealLadderUnitVM dealLadderUnit in positionRolloverItem.DealLadderUnitList)
                {
                    valueDateHashSet.Add(dealLadderUnit.LadderDay);
                }
            }

            if (from != null && to != null && from.Value <= to.Value)
            {
                // 排除不再From和To日期内的时间
                valueDateHashSet.RemoveWhere(o => o < from.Value || o > to.Value);
            }
            else if (from == null && to != null)
            {
                // 排除大于To日期的时间
                valueDateHashSet.RemoveWhere(o => o > to.Value);
            }
            else if (from != null && to == null)
            {
                // 排除小于From日期的时间
                valueDateHashSet.RemoveWhere(o => o < from.Value);
            }

            // 根据ValueDate排序
            IOrderedEnumerable<DateTime> valueDateOrderedList = valueDateHashSet.OrderBy(o => o);

            // 生成DataTable
            var dt = new DataTable();

            // CCY列
            dt.Columns.Add("CurrencyName", typeof(string));
            dt.Columns.Add("TotalAmountStr", typeof(string));
            dt.Columns.Add("TotalAmountColor", typeof(string));

            // 其他日期列
            foreach (DateTime valueDate in valueDateOrderedList)
            {
                dt.Columns.Add(valueDate.FormatDateByBu(), typeof(DealLadderUnitVM));
            }

            // 获取货币仓储
            var currencyRep = this.GetRepository<ICurrencyRepository>();

            // 根据CurrencyName排序
            this.positionRolloverItems =
                this.positionRolloverItems.OrderBy(o => currencyRep.GetName(o.CurrencyId)).ToList();

            // 生成行数据
            foreach (ByPositionRolloverItem positionRolloverItem in this.positionRolloverItems)
            {
                DataRow row = dt.NewRow();

                // 添加CCY单元格
                row[0] = currencyRep.GetName(positionRolloverItem.CurrencyId);
                row[1] = positionRolloverItem.TotalAmountStr;
                row[2] = positionRolloverItem.TotalAmountColor;
                for (int i = 2; i < dt.Columns.Count; i++)
                {
                    foreach (DealLadderUnitVM dealLadderUnit in positionRolloverItem.DealLadderUnitList)
                    {
                        // 如果有对应CCY对应日期的ValueDate则赋值
                        if (dealLadderUnit.LadderDay.FormatDateByBu().Equals(dt.Columns[i].ColumnName))
                        {
                            row[i] = dealLadderUnit;
                        }
                    }
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        /// <summary>
        ///     The load.
        /// </summary>
        private void Load()
        {
            this.Counterpartys = this.counterpartyRep.GetBindCollection().ToComboboxBinding(true);
            this.positionRolloverItems = new List<ByPositionRolloverItem>();
            this.BusinessUnitList = this.buRep.GetBindCollection().ToComboboxBinding(true);
            ObservableCollection<FxHedgingDealModel> list = this.dealRep.GetBindCollection();
            this.CalcByPositionRolloverData(list);
            this.CreateControl();
        }

        /// <summary>
        /// The search deals.
        /// </summary>
        /// <param name="deals">
        /// The deals.
        /// </param>
        /// <returns>
        /// The <see cref="IOrderedEnumerable"/>.
        /// </returns>
        private IOrderedEnumerable<FxHedgingDealModel> SearchDeals(IEnumerable<FxHedgingDealModel> deals)
        {
            IEnumerable<FxHedgingDealModel> dealListTemp = from item in deals select item;
            string buid = string.Empty;
            if (this.BusinessUnit != null)
            {
                buid = this.BusinessUnit.Id;
            }

            string countorty = string.Empty;
            if (this.Counterparty != null)
            {
                countorty = this.Counterparty.Id;
            }

            if (!string.IsNullOrEmpty(countorty))
            {
                dealListTemp = from item in dealListTemp where item.CounterpartyId == countorty select item;
            }

            if (!string.IsNullOrEmpty(buid))
            {
                dealListTemp = from item in dealListTemp where item.BusinessUnitId == buid select item;
            }

            if (this.ValueDateTo != null)
            {
                dealListTemp = from item in dealListTemp
                               where item.ValueDate.Date <= this.ValueDateTo.Value.Date
                               select item;
            }

            if (this.ValueDateFrom != null)
            {
                dealListTemp = from item in dealListTemp
                               where item.ValueDate.Date >= this.ValueDateFrom.Value.Date
                               select item;
            }

            return dealListTemp.OrderBy(o => o.ValueDate.Date);
        }

        /// <summary>
        /// 显示By Position Rollover的明细
        /// </summary>
        /// <param name="context">
        /// 数据上下文
        /// </param>
        /// <param name="e">
        /// 鼠标点击事件
        /// </param>
        private void ShowDetail(object context, MouseButtonEventArgs e)
        {
            var text = context as DataGridText;
            if (text == null)
            {
                return;
            }

            var content = text.StationInfoDisplay as DealLadderUnitVM;
            if (content == null || content.RelateDealList == null || content.RelateDealList.Count < 1)
            {
                return;
            }

            var dhvm = new CashLadderDealInfoViewModel(content.RelateDealList.Select(o => o).ToList())
                           {
                               DisplayName =
                                   RunTime
                                   .FindStringResource
                                   (
                                       "DetailedInformation")
                           };
            this.windowManager.ShowDialog(dhvm);
        }

        /// <summary>
        ///     开始计算
        /// </summary>
        private void StartCalculate()
        {
            ObservableCollection<FxHedgingDealModel> list = this.dealRep.GetBindCollection();
            this.CalcByPositionRolloverData(list);
        }

        /// <summary>
        /// The wait load.
        /// </summary>
        /// <param name="lastTask">
        /// The last task.
        /// </param>
        private void WaitLoad(Task lastTask)
        {
            this.Load();
            this.dealRep.SubscribeAddEvent(model => this.Load());
            this.dealRep.SubscribeUpdateEvent((oldModel, newModel) => this.Load());
            this.dealRep.SubscribeRemoveEvent(model => this.Load());
        }

        #endregion
    }

    /// <summary>
    ///     根据货币分组的临时交易单
    /// </summary>
    public class DealGroupByCCY
    {
        #region Properties

        /// <summary>
        ///     额度
        /// </summary>
        internal decimal Amount { get; set; }

        /// <summary>
        ///     货币
        /// </summary>
        internal string CCY { get; set; }

        /// <summary>
        ///     交易单号
        /// </summary>
        internal string DealID { get; set; }

        /// <summary>
        ///     交割日
        /// </summary>
        internal DateTime ValueDate { get; set; }

        #endregion
    }
}