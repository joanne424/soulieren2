// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CashLadderToolViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/21 10:33:08 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/21 10:33:08
//      修改描述：新建 CashLadderToolViewModel.cs
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
    using DM2.Ent.Presentation.Models;
    using DM2.Ent.Presentation.Models.Base;

    using Infrastructure.Common.Enums;
    using Infrastructure.Data;

    using Microsoft.Practices.Unity;

    /// <summary>
    ///     The cash ladder view model.
    /// </summary>
    public class CashLadderToolViewModel : BaseVm
    {
        #region Fields

        /// <summary>
        ///     The currency rep.
        /// </summary>
        private readonly ICurrencyRepository currencyRep;

        /// <summary>
        ///     The deal rep.
        /// </summary>
        private readonly IFxHedgingDealRepository dealRep;

        /// <summary>
        /// The tool dates.
        /// </summary>
        private readonly HashSet<DateTime> toolDates = new HashSet<DateTime>();

        /// <summary>
        ///     window消息命令
        /// </summary>
        private readonly IWindowManager windowManager = IocContainer.Instance.Container.Resolve<IWindowManager>();

        /// <summary>
        ///     动态生成By Position Rollover Tab页面的内容
        /// </summary>
        private StackPanel cashLadderContent;

        /// <summary>
        ///     By Position Rollover 列表
        /// </summary>
        private List<ByPositionRolloverItem> positionRolloverItems;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CashLadderToolViewModel" /> class.
        /// </summary>
        public CashLadderToolViewModel()
        {
            this.dealRep = this.GetRepository<IFxHedgingDealRepository>();
            this.currencyRep = this.GetRepository<ICurrencyRepository>();
            this.cashLadderContent = new StackPanel { Orientation = Orientation.Horizontal };
            this.positionRolloverItems = new List<ByPositionRolloverItem>();
            Task.Factory.StartNew(RunTime.GetCurrentRunTime().CurrentRepositoryCore.WaitAllInitial)
                .ContinueWith(this.WaitLoad);
        }

        #endregion

        #region Public Properties

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

        #endregion

        #region Methods

        /// <summary>
        /// (重新)计算By Position Rollover的数据
        /// </summary>
        /// <param name="deals">
        /// 订单列表
        /// </param>
        private void CalcByPositionRolloverData(IEnumerable<FxHedgingDealModel> deals)
        {
            this.toolDates.Clear();
            this.positionRolloverItems.Clear();
            if (!deals.Any())
            {
                return;
            }

            var dealTemp = new List<DealGroupByCCY>();
            IOrderedEnumerable<FxHedgingDealModel> dealListTemp = from item in deals
                                                                  orderby item.ValueDate.Date
                                                                  select item;
            DateTime first = dealListTemp.Min(o => o.ValueDate.Date);
            this.toolDates.Add(first);
            int total = dealListTemp.GroupBy(o => o.ValueDate.Date).Count();
            int k = total > 4 ? 4 : total;
            while (this.toolDates.Count < k)
            {
                first = first.AddDays(1);
                bool reslut = dealListTemp.Any(o => o.ValueDate.Date == first.Date);
                if (reslut)
                {
                    this.toolDates.Add(first);
                }
            }

            foreach (FxHedgingDealModel item in dealListTemp)
            {
                if (!this.toolDates.Contains(item.ValueDate.Date))
                {
                    continue;
                }

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
        ///     将交易单列表转换为报表数据列表
        /// </summary>
        /// <summary>
        ///     The create control.
        /// </summary>
        /// <param name="businessUnit">
        ///     The business Unit.
        /// </param>
        /// <param name="counterParty">
        ///     The counter Party.
        /// </param>
        /// <param name="from">
        ///     The from.
        /// </param>
        /// <param name="to">
        ///     The to.
        /// </param>
        private void CreateControl()
        {
            DataTable dataTable = this.CreateDataTable();
            RunTime.GetCurrentRunTime().ActionOnUiThread(
                () =>
                {
                    DataGrid dataGrid = CashLadderDataGridTool.CreateDataGrid(dataTable, this.ShowDetail, true);
                    if (dataGrid != null)
                    {
                        this.cashLadderContent.Children.Clear();
                        this.cashLadderContent.Children.Add(dataGrid);
                        this.NotifyOfPropertyChange(() => this.CashLadderContent);
                    }
                });
        }

        /// <summary>
        ///     创建数据源
        /// </summary>
        /// <param name="businessUnit">
        ///     The business Unit.
        /// </param>
        /// <param name="counterParty">
        ///     The counter Party.
        /// </param>
        /// <param name="from">
        ///     The from.
        /// </param>
        /// <param name="to">
        ///     The to.
        /// </param>
        /// <returns>
        ///     数据源
        /// </returns>
        private DataTable CreateDataTable()
        {
            // 基础数据
            if (this.positionRolloverItems == null || this.positionRolloverItems.Count == 0)
            {
                return null;
            }

            // 根据ValueDate排序
            IOrderedEnumerable<DateTime> valueDateOrderedList = this.toolDates.OrderBy(o => o);

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
            List<FxHedgingDealModel> list =
                this.dealRep.GetBindCollection().Where(o => o.Status == StatusEnum.OPERN).ToList();
            this.CalcByPositionRolloverData(list);
            this.CreateControl();
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
        /// The load.
        /// </summary>
        /// <param name="lastTask">
        /// The last task.
        /// </param>
        private void WaitLoad(Task lastTask)
        {
            this.Load();
            this.dealRep.SubscribeAddEvent(model => { this.Load(); });
            this.dealRep.SubscribeUpdateEvent((oldModel, newModel) => { this.Load(); });
            this.dealRep.SubscribeRemoveEvent(model => { this.Load(); });
        }

        #endregion
    }
}