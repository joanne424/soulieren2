// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DealLadderUnitVM.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/21 01:26:28 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/21 01:26:28
//      修改描述：新建 DealLadderUnitVM.cs
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
    using System.Collections.ObjectModel;

    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Client.Models;
    using DM2.Ent.Presentation.Models.Base;

    /// <summary>
    ///     用于显示的报表数据条目模型
    /// </summary>
    public class DealLadderUnitVM : BaseVm
    {
        #region Fields

        /// <summary>
        ///     The business unit id.
        /// </summary>
        private string businessUnitId;

        /// <summary>
        /// The counterparty.
        /// </summary>
        private string counterparty;

        /// <summary>
        ///     The currency id.
        /// </summary>
        private string currencyID;

        /// <summary>
        ///     The index.
        /// </summary>
        private int index = -1;

        /// <summary>
        ///     The ladder amount.
        /// </summary>
        private decimal ladderAmount;

        /// <summary>
        ///     The ladder amount color.
        /// </summary>
        private string ladderAmountColor;

        /// <summary>
        ///     The ladder amount str.
        /// </summary>
        private string ladderAmountStr;

        /// <summary>
        ///     The ladder day.
        /// </summary>
        private DateTime ladderDay;

        /// <summary>
        ///     The relate deal list.
        /// </summary>
        private ObservableCollection<string> relateDealList;

        #endregion

        #region Public Properties

        /// <summary>
        ///     货币名称
        /// </summary>
        public string CurrencyID
        {
            get
            {
                return this.currencyID;
            }

            set
            {
                this.currencyID = value;
                this.NotifyOfPropertyChange(() => this.CurrencyID);
            }
        }

        /// <summary>
        ///     索引号
        /// </summary>
        public int Index
        {
            get
            {
                return this.index;
            }

            set
            {
                this.index = value;
                this.NotifyOfPropertyChange(() => this.Index);
            }
        }

        /// <summary>
        ///     所选货币当天的交割金额
        /// </summary>
        public decimal LadderAmount
        {
            get
            {
                return this.ladderAmount;
            }

            set
            {
                this.ladderAmount = value;
                this.NotifyOfPropertyChange(() => this.LadderAmount);
                this.NotifyOfPropertyChange(() => this.LadderAmountStr);
                this.NotifyOfPropertyChange(() => this.LadderAmountColor);
            }
        }

        /// <summary>
        ///     LadderAmount的前景色
        /// </summary>
        public string LadderAmountColor
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.ladderAmountColor))
                {
                    if (this.ladderAmount < 0)
                    {
                        return "Red";
                    }

                    return "Black";
                }

                return this.ladderAmountColor;
            }

            set
            {
                this.ladderAmountColor = value;
                this.NotifyOfPropertyChange(() => this.LadderAmountColor);
            }
        }

        /// <summary>
        ///     LadderAmount的显示字符串
        /// </summary>
        public string LadderAmountStr
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.ladderAmountStr))
                {
                    if (this.ladderAmount == 0)
                    {
                        this.ladderAmountStr = "0";
                    }

                    if (this.ladderAmount < 0)
                    {
                        this.ladderAmountStr = string.Format(
                            "({0})",
                            (0 - this.ladderAmount).FormatAmountToStringByCurrencyId(this.CurrencyID));
                    }
                    else
                    {
                        this.ladderAmountStr = this.ladderAmount.FormatAmountToStringByCurrencyId(this.CurrencyID).ToString();
                    }
                }

                return this.ladderAmountStr;
            }

            set
            {
                this.ladderAmountStr = value;
                this.NotifyOfPropertyChange(() => this.LadderAmountStr);
            }
        }

        /// <summary>
        ///     报表时间
        /// </summary>
        public DateTime LadderDay
        {
            get
            {
                return this.ladderDay;
            }

            set
            {
                this.ladderDay = value;
                this.NotifyOfPropertyChange(() => this.LadderDay);
            }
        }

        /// <summary>
        ///     LadderAmount的显示字符串
        /// </summary>
        //public string LadderAmountStr
        //{
        //    get
        //    {
        //        if (string.IsNullOrWhiteSpace(this.ladderAmountStr))
        //        {
        //            if (this.ladderAmount < 0)
        //            {
        //                this.ladderAmountStr = string.Format(
        //                    "({0})",
        //                    (0 - this.ladderAmount).FormatAmountToDecimalByCCY(this.CurrencyID));
        //            }

        //                // else if (ladderAmount == 0)
        //            // {
        //            // ladderAmountStr = Decimal.Zero.FormatAmountByCCYID(this.CurrencyID);// "-";
        //            // }
        //            else
        //            {
        //                this.ladderAmountStr = this.ladderAmount.FormatAmountToDecimalByCCY(this.CurrencyID).ToString();
        //            }
        //        }

        //        return this.ladderAmountStr;

        //        // return ladderAmount == 0 ? "-" : ladderAmount.ToString();
        //    }

        //    set
        //    {
        //        this.ladderAmountStr = value;
        //        this.NotifyOfPropertyChange(() => this.LadderAmountStr);
        //    }
        //}

        /// <summary>
        ///     关联的对冲交易单
        /// </summary>
        public ObservableCollection<string> RelateDealList
        {
            get
            {
                if (this.relateDealList == null)
                {
                    this.relateDealList = new ObservableCollection<string>();
                }

                return this.relateDealList;
            }

            set
            {
                this.relateDealList = value;
                this.NotifyOfPropertyChange(() => this.RelateDealList);
            }
        }

        #endregion
    }
}