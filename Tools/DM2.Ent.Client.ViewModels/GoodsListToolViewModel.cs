// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DealListToolViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/20 05:10:14 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/20 05:10:14
//      修改描述：新建 DealListToolViewModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using Caliburn.Micro;

    using DM2.Ent.Presentation.Models.Base;

    using Infrastructure.Common.Enums;

    /// <summary>
    ///     The deal list tool view model.
    /// </summary>
    public class GoodsListToolViewModel : BaseVm
    {
        #region Fields
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GoodsListToolViewModel"/> class.
        /// </summary>
        /// <param name="varOwnerId">
        /// The var owner id.
        /// </param>
        public GoodsListToolViewModel(string varOwnerId = null)
            : base(varOwnerId)
        {
            //this.DisplayName = RunTime.FindStringResource("HedgeDealList");
            //this.dealReps = this.GetRepository<IFxHedgingDealRepository>();
            //Task.Factory.StartNew(RunTime.GetCurrentRunTime().CurrentRepositoryCore.WaitAllInitial)
            //    .ContinueWith(this.WaitLoad);
        }
        #endregion

        #region Public Properties
        #endregion

        #region Public Methods and Operators

        #endregion
    }
}