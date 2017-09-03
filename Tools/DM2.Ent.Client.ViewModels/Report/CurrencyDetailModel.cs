

namespace DM2.Ent.Client.ViewModels.Report
{
    /// <summary>
    /// 每日资金统计货币明细
    /// </summary>
    public class CurrencyDetailModel
    {
        public CurrencyDetailModel()
        {
        }

        public CurrencyDetailModel(string currencyId, decimal settlementAccountBalance, decimal position)
        {
            this.CurrencyId = currencyId;
            this.SettlementAccountBalance = settlementAccountBalance;
            this.Position = position;

            this.Balance = settlementAccountBalance + position;
        }

        public CurrencyDetailModel(string currencyId, decimal settlementAccountBalance, decimal position, decimal nonSettlementAccountBalance)
            : this(currencyId, settlementAccountBalance, position)
        {
            this.NonSettlementAccountBalance = nonSettlementAccountBalance;
        }

        /// <summary>
        ///     结算后余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        ///     货币ID
        /// </summary>
        public string CurrencyId { get; set; }

        /// <summary>
        ///     非结算账户余额
        /// </summary>
        public decimal NonSettlementAccountBalance { get; set; }

        /// <summary>
        ///     仓位
        /// </summary>
        public decimal Position { get; set; }

        /// <summary>
        ///     结算账户余额
        /// </summary>
        public decimal SettlementAccountBalance { get; set; }

        public void IncreaseDealPosition(decimal position)
        {
            this.Position += position;
            this.Balance += position;
        }

        public void IncreaseSettlementAccountBalance(decimal balance)
        {
            this.SettlementAccountBalance += balance;
            this.Balance += balance;
        }

        public void IncreaseNonSettlementAccountBalance(decimal balance)
        {
            this.NonSettlementAccountBalance += balance;
        }

        public bool IsEmpty()
        {
            return this.SettlementAccountBalance == decimal.Zero && this.Position == decimal.Zero
                   && this.NonSettlementAccountBalance == decimal.Zero;
        }
    }
}
