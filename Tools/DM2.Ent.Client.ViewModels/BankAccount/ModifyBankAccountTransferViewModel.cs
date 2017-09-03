// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModifyBankAccountTransferViewModel.cs" company="">
//   
// </copyright>
// <author>zoukp</author>
// <date> 2017/04/25 09:44:11 </date>
// <modify>
//      修改人：zoukp
//      修改时间：2017/04/25 09:44:11
//      修改描述：新建 ModifyBankAccountTransferViewModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review>
// --------------------------------------------------------------------------------------------------------------------
namespace DM2.Ent.Client.ViewModels
{
    using System.Windows;

    using DM2.Ent.Client.Models;
    using DM2.Ent.Client.Runtime;
    using DM2.Ent.Presentation.Models;

    /// <summary>
    ///     The counterparty add view model.
    /// </summary>
    public class ModifyBankAccountTransferViewModel : BankCashTransferModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyBankAccountTransferViewModel"/> class.
        /// </summary>
        /// <param name="bankCashTransferModel">
        /// The bank Cash Transfer Model.
        /// </param>
        public ModifyBankAccountTransferViewModel(BankCashTransferModel bankCashTransferModel)
        {
            this.Copy(bankCashTransferModel);
            this.DisplayName = RunTime.FindStringResource("BankCashTransfer") + this.Id;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     取消操作
        /// </summary>
        public void Close()
        {
            this.TryClose();
        }

        /// <summary>
        ///     关闭窗口
        /// </summary>
        public void OnClosed()
        {
            this.TryClose(true);
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        public void OnMinimizeWindowCommand(object window)
        {
            var win = window as Window;
            win.WindowState = WindowState.Minimized;
        }

        #endregion
    }
}