// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByPositionRolloverItem.cs" company="">
//   
// </copyright>
// <author>wangmy</author>
// <date> 2016/05/25 07:52:06 </date>
// <modify>
//      修改人：wangmy
//      修改时间：2016/05/25 07:52:06
//      修改描述：新建 ByPositionRolloverItem.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels
{
    using System.Collections.Generic;

    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models.Base;

    /// <summary>
    ///     The by position rollover item.
    /// </summary>
    public class ByPositionRolloverItem : BaseVm
    {
        #region Fields

        /// <summary>
        /// The currency name.
        /// </summary>
        private string currencyName;

        /// <summary>
        ///     The ladder amount.
        /// </summary>
        private decimal totalAmount;

        /// <summary>
        ///     The ladder amount color.
        /// </summary>
        private string totalAmountColor;

        /// <summary>
        ///     The ladder amount str.
        /// </summary>
        private string totalAmountStr;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ByPositionRolloverItem" /> class.
        /// </summary>
        public ByPositionRolloverItem()
        {
            this.DealLadderUnitList = new List<DealLadderUnitVM>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the currency id.
        /// </summary>
        public string CurrencyId { get; set; }

        /// <summary>
        ///     Gets or sets the currency id.
        /// </summary>
        public string CurrencyName
        {
            get
            {
                return this.currencyName;
            }

            set
            {
                this.currencyName = value;
                this.NotifyOfPropertyChange(() => this.CurrencyName);
            }
        }

        /// <summary>
        ///     Gets or sets the deal ladder unit list.
        /// </summary>
        public List<DealLadderUnitVM> DealLadderUnitList { get; set; }

        /// <summary>
        ///     所选货币当天的交割金额
        /// </summary>
        public decimal TotalAmount
        {
            get
            {
                return this.totalAmount;
            }

            set
            {
                this.totalAmount = value;
                this.NotifyOfPropertyChange(() => this.TotalAmount);
                this.NotifyOfPropertyChange(() => this.TotalAmountStr);
                this.NotifyOfPropertyChange(() => this.TotalAmountColor);
            }
        }

        /// <summary>
        ///     LadderAmount的前景色
        /// </summary>
        public string TotalAmountColor
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.totalAmountColor))
                {
                    if (this.totalAmount < 0)
                    {
                        return "Red";
                    }

                    return "Black";
                }

                return this.totalAmountColor;
            }

            set
            {
                this.totalAmountColor = value;
                this.NotifyOfPropertyChange(() => this.TotalAmountColor);
            }
        }

        /// <summary>
        ///     LadderAmount的显示字符串
        /// </summary>
        public string TotalAmountStr
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.totalAmountStr))
                {
                    if (this.totalAmount == 0)
                    {
                        this.totalAmountStr = "0";
                    }

                    if (this.totalAmount < 0)
                    {
                        this.totalAmountStr = string.Format(
                            "({0})",
                            (0 - this.totalAmount).FormatAmountToStringByCurrencyId(this.CurrencyId));
                    }
                    else
                    {
                        this.totalAmountStr = this.totalAmount.FormatAmountToStringByCurrencyId(this.CurrencyId);
                    }
                }

                return this.totalAmountStr;
            }

            set
            {
                this.totalAmountStr = value;
                this.NotifyOfPropertyChange(() => this.TotalAmountStr);
            }
        }

        #endregion
    }
}