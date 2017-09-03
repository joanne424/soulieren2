using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Models;

namespace DM2.Manager.Models
{
    public class DealCreatedNotifier
    {
        /// <summary>
        /// 出金单创建事件
        /// </summary>
        public event Action<DepositWithdrawModel> DepositWithdrawCreated;

        /// <summary>
        /// 发布出金单创建通知
        /// </summary>
        /// <param name="dwDeal">新建出入金单</param>
        public void NotifyDepositWithdrawCreated(DepositWithdrawModel dwDeal)
        {
            if (DepositWithdrawCreated != null)
            {
                try
                {
                    DepositWithdrawCreated(dwDeal);
                }
                catch(Exception ex)
                {
                    Infrastructure.Log.TraceManager.Warn.Write("出入金单创建事件通知处理逻辑出错", ex);
                }
            }
        }

        /// <summary>
        /// 费用单创建事件
        /// </summary>
        public event Action<AdHocFeeModel> AdHocFeeCreated;

        /// <summary>
        /// 发布费用单新建通知
        /// </summary>
        /// <param name="feeDeal">新建费用单</param>
        public void NotifyAdHocFeeCreated(AdHocFeeModel feeDeal)
        {
            if (AdHocFeeCreated != null)
            {
                try
                {
                    AdHocFeeCreated(feeDeal);
                }
                catch(Exception ex)
                {
                    Infrastructure.Log.TraceManager.Warn.Write("费用单创建事件通知处理逻辑出错", ex);
                }
            }
        }

        /// <summary>
        /// 内部转账单创建事件
        /// </summary>
        public event Action<InternalAcctTransferModel> InternalAcctTransferCreated;

        /// <summary>
        /// 发布内部转账单新建通知
        /// </summary>
        /// <param name="interanlTransferDeal">新建内部转账单</param>
        public void NotifyInternalTransferCreated(InternalAcctTransferModel interanlTransferDeal)
        {
            if (InternalAcctTransferCreated != null)
            {
                try
                {
                    InternalAcctTransferCreated(interanlTransferDeal);
                }
                catch (Exception ex)
                {
                    Infrastructure.Log.TraceManager.Warn.Write("内部转账单创建事件通知处理逻辑出错", ex);
                }
            }
        }
    }
}
