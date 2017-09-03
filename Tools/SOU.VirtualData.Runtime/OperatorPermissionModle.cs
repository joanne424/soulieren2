// <copyright file="OperatorPermissionModle.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author> zhanggq </author>
// <date> 2016/03/03 15:28:37 </date>
// <summary> 界面权限标识全局变量实体 </summary>
// <modify>
//      修改人：zhanggq
//      修改时间：2016/03/03 15:28:37
//      修改描述：新建 OperatorPermissionModle.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DM2.Manager.Models
{
    /// <summary>
    /// 界面权限标识全局变量实体
    /// </summary>
    public class OperatorPermissionModle : BaseModel
    {
        /// <summary>
        /// Real-time Streaming Quotation
        /// 查看全局报价 权限标识, PermissionID = 14
        /// </summary>
        public bool HAS_STR_QUOTATION = false;

        /// <summary>
        /// Quote Watcher
        /// 查看QuoteWatcher 权限标识, PermissionID = 15
        /// </summary>
        public bool HAS_QUOTE_WATCHER = false;
        /// <summary>
        /// Send Price
        /// 手工发送报价 权限标识, PermissionID = 16
        /// </summary>
        public bool HAS_SEND_PRICE = false;

        /// <summary>
        /// Modify Forward Points
        /// 修改Forward Points 权限标识, PermissionID = 17
        /// </summary>
        public bool HAS_MODIFY_FORWARD_POINTS = false;

        /// <summary>
        /// Customer Profile Viewing
        /// 客户信息查看 权限标识, PermissionID = 18
        /// </summary>
        public bool HAS_CUSTOMER_PROFILE_VIEWING = false;

        /// <summary>
        /// Customer Creation / Profile Management
        /// 客户管理 权限标识, PermissionID = 19
        /// </summary>
        public bool HAS_CUSTOMER_CREATION_PROFILE_MANAGEMENT = false;

        /// <summary>
        /// Hedge Account Management
        /// 对冲账户管理 权限标识, PermissionID = 20
        /// </summary>
        public bool HAS_HEDEG_ACCOUNT_MANAGEMENT = false;

        /// <summary>
        /// Forward Book
        /// 查看Forward Book 权限标识, PermissionID = 21
        /// </summary>
        public bool HAS_FORWARD_BOOK = false;

        /// <summary>
        /// Dealing Book Reports
        /// 查看Dealing Book 权限标识, PermissionID = 22
        /// </summary>
        public bool HAS_DEALING_BOOK_REPORTS = false;

        /// <summary>
        /// Today open Adjustment
        /// 调用CNY / Cash Dealing Book 中Adjustment 值 权限标识, PermissionID = 23
        /// </summary>
        public bool HAS_TODAY_OPEN_ADJUSTMENT = false;

        /// <summary>
        /// Deal Request
        /// 交易请求管理 权限标识, PermissionID = 24
        /// </summary>
        public bool HAS_DEAL_REQUEST = false;

        /// <summary>
        /// OTC Transaction
        /// 代客交易 权限标识, PermissionID = 25
        /// </summary>
        public bool HAS_OTC_TRANSACTION = false;

        /// <summary>
        /// Deal Viewing
        /// 除Customer Profile 信息以外的所有信息都可以看 权限标识, PermissionID = 26
        /// </summary>
        public bool HAS_DEAL_VIEWING = false;

        /// <summary>
        /// Online Trading
        /// 暂停客户端交易 权限标识, PermissionID = 27
        /// </summary>
        public bool HAS_ONLLINE_TRADING = false;

        /// <summary>
        /// Ad Hoc Fee
        /// 收费单管理 权限标识, PermissionID = 29
        /// </summary>
        public bool HAS_AD_HOC_FEE = false;

        /// <summary>
        /// Pledge Management
        /// 抵押金管理 权限标识, PermissionID = 30
        /// </summary>
        public bool HAS_PLEDGE_MANAGEMENT = false;

        /// <summary>
        /// Credit Management
        /// 授信管理 权限标识, PermissionID = 31
        /// </summary>
        public bool HAS_CREDIT_MANAGEMENT = false;

        /// <summary>
        /// Announcement Management
        /// 公告管理 权限标识, PermissionID = 32
        /// </summary>
        public bool HAS_ANNOUNCEMENT_MANAGEMENT = false;

        /// <summary>
        /// Settlement Management
        /// 结算管理 权限标识, PermissionID = 33
        /// </summary>
        public bool HAS_SETTLEMENT_MANAGEMENT = false;

        /// <summary>
        /// Real-time Margin Call Monitoring
        /// 处理Margin call 权限标识, PermissionID = 34
        /// </summary>
        public bool HAS_MARGIN_CALL_MONITORING = false;

        /// <summary>
        /// Online security administrator Log
        /// 查看日志 权限标识, PermissionID = 44
        /// </summary>
        public bool HAS_SECURITY_ADMIN_LOG = false;

        /// <summary>
        /// Online activity Log
        /// 查看业务日志 权限标识, PermissionID = 45
        /// </summary>
        public bool HAS_ACTIVITY_LOG = false;

        /// <summary>
        /// Login log
        /// 登陆相关日志 权限标识, PermissionID = 46
        /// </summary>
        public bool HAS_LOGIN_LOG = false;

        /// <summary>
        /// Trade log
        /// 交易相关日志 权限标识, PermissionID = 47
        /// </summary>
        public bool HAS_TRADE_LOG = false;

        /// <summary>
        /// Account log
        /// 帐户相关日志 权限标识, PermissionID = 48
        /// </summary>
        public bool HAS_ACCOUNT_LOG = false;

        /// <summary>
        /// Error log
        /// 失败日志 权限标识, PermissionID = 49
        /// </summary>
        public bool HAS_ERROR_LOG = false;

        /// <summary>
        /// Admin log
        /// 配置管理日志 权限标识, PermissionID = 50
        /// </summary>
        public bool HAS_ADMIN_LOG = false;

        /// <summary>
        /// Customer List
        /// 查看账户列表 权限标识, PermissionID = 51
        /// </summary>
        public bool HAS_CUSTOMER_LIST = false;

        /// <summary>
        /// Customer Profile
        /// 查看账户信息、用户信息及外部账户信息（不含交易、出入金等信息） 权限标识, PermissionID = 52
        /// </summary>
        public bool HAS_CUSTOMER_PROFILE = false;

        /// <summary>
        /// Beneficiary Account Viewing
        /// 查询外部账户现金流 权限标识, PermissionID = 53
        /// </summary>
        public bool HAS_BENEFICIARY_ACCOUNT_VIEWING = false;

        /// <summary>
        /// VIP Customer Viewing
        /// 查看特殊客户 权限标识, PermissionID = 54
        /// </summary>
        public bool HAS_VIP_CUSTOMER_VIEWING = false;

        /// <summary>
        /// Customer Creation
        /// 开户 权限标识, PermissionID = 55
        /// </summary>
        public bool HAS_CUSTOMER_CREATION = false;

        /// <summary>
        /// Customer Modification
        /// 修改账户个人信息 权限标识, PermissionID = 56
        /// </summary>
        public bool HAS_CUSTOMER_MODIFICATION = false;

        /// <summary>
        /// Beneficiary Account Management
        /// 外部账户管理 权限标识, PermissionID = 57
        /// </summary>
        public bool HAS_BENEFICIARY_ACCOUNT_MANAGEMENT = false;

        /// <summary>
        /// VIP Customer Management
        /// 特殊客户配置 权限标识, PermissionID = 58
        /// </summary>
        public bool HAS_VIP_CUSTOMER_MANAGEMENT = false;

        /// <summary>
        /// Hedge Account List
        /// 查看对冲账户列表 权限标识, PermissionID = 59
        /// </summary>
        public bool HAS_HEDGE_ACCOUNT_LIST = false;

        /// <summary>
        /// Hedge Account Modification
        /// 创建、修改对冲账户 权限标识, PermissionID = 60
        /// </summary>
        public bool HAS_HEDGE_ACCOUNT_MODIFICATION = false;

        /// <summary>
        /// Hedge Deal Management
        /// 对冲交易管理 权限标识, PermissionID = 61
        /// </summary>
        public bool HAS_HEDGE_DEAL_MANAGEMENT = false;

        /// <summary>
        /// Hedge Account Viewing
        /// 查看对冲交易列表、对冲账户Cash Ladder 权限标识, PermissionID = 62
        /// </summary>
        public bool HAS_HEDGE_ACCOUNT_VIEWING = false;

        /// <summary>
        /// Quote / Deal
        /// 请求相关 权限标识, PermissionID = 63
        /// 询价请求、市价下单请求、订单确认请求、保证金不足展期请求
        /// Dealer配置、Automation配置、查看请求队列、查看历史请求、查看请求处理日志
        /// </summary>
        public bool HAS_QUOTE_DEAL = false;

        /// <summary>
        /// Fill Order
        /// 处理挂单激活请求、Dealer配置、Automation配置、查看请求处理日志 权限标识, PermissionID = 64
        /// </summary>
        public bool HAS_FILL_ORDER = false;

        /// <summary>
        /// Force Sell
        /// 处理force sell、Dealer配置、Automation配置、查看请求处理日志 权限标识, PermissionID = 65
        /// </summary>
        public bool HAS_FORCE_SELL = false;

        /// <summary>
        /// Deal Approval
        /// 处理交易删除请求、Dealer配置、Automation配置、查看请求队列、
        /// 查看历史请求、查看请求处理日志 权限标识, PermissionID = 66
        /// </summary>
        public bool HAS_DEAL_APPROVAL = false;

        /// <summary>
        /// Deal Delete Confirm
        /// 处理交易单状态为DeletedApproval的确认，拒绝操作
        /// PermissionID = 104
        /// </summary>
        public bool HAS_DEAL_DELETE_CONFIRM = false;

        /// <summary>
        /// Deposit / Withdrawal
        /// 出入金 权限标识, PermissionID = 67
        /// </summary>
        public bool HAS_DEPOSIT_WITHDRAWAL = false;

        /// <summary>
        /// Delete Deposit / Withdrawal
        /// 删除出入金 权限标识, PermissionID = 68
        /// </summary>
        public bool HAS_DELETE_DEPOSIT_WITHDRAWAL = false;

        /// <summary>
        /// Announcement Delivery
        /// 发布公告 权限标识, PermissionID = 69
        /// </summary>
        public bool HAS_ANNOUNCEMENT_DELIVERY = false;

        /// <summary>
        /// Announcement Approval
        /// 审批公告 权限标识, PermissionID = 70
        /// </summary>
        public bool HAS_ANNOUNCEMENT_APPROVAL = false;

        /// <summary>
        /// Manual Settlement
        /// 手工结算 权限标识, PermissionID = 71
        /// </summary>
        public bool HAS_MANUAL_SETTLEMENT = false;

        /// <summary>
        /// Settle Now
        /// 立即结算 权限标识, PermissionID = 72
        /// </summary>
        public bool HAS_SETTLE_NOW = false;

        /// <summary>
        /// Deposit/Withdrawal/Fee Modification
        /// 修改未结算 权限标识, PermissionID = 73
        /// </summary>
        public bool HAS_DEPOSIT_WITHDRAWAL_FEE_MODIFICATION = false;

        /// <summary>
        /// Settlement Reversal
        /// 结算Reversal 权限标识, PermissionID = 74
        /// </summary>
        public bool HAS_SETTLEMENT_REVERSAL = false;

        /// <summary>
        /// Delete Deposit/Withdrawal/Fee Approval
        /// 审批删除 权限标识, PermissionID = 75
        /// </summary>
        public bool HAS_DELETE_DEPOSIT_WITHDRAWAL_FEE_MODIFICATION = false;

        /// <summary>
        /// Bank Account Management
        /// 银行账户管理 权限标识, PermissionID = 76
        /// </summary>
        public bool HAS_BANK_ACCOUNT_MANAGEMENT = false;

        /// <summary>
        /// Bank Account Rounding
        /// 银行账号资金管理 权限标识, PermissionID = 77
        /// </summary>
        public bool HAS_BANK_ACCOUNT_ROUNDING = false;

        /// <summary>
        /// Bank Account Deposit Withdrawal
        /// 银行账号资金管理 权限标识, PermissionID = 78
        /// </summary>
        public bool HAS_BANK_ACCOUNT_DEPOSIT_WITHDRAWAL = false;

        /// <summary>
        /// Bank Account Viewing
        /// 银行账号资金管理 权限标识, PermissionID = 79
        /// </summary>
        public bool HAS_BANK_ACCOUNT_VIEW = false;

        /// <summary>
        /// Report -001 Transaction Listing -Consolidated
        /// 报表 001，PermissionID = 106
        /// </summary>
        public bool HAS_REPORT_001 = false;

        /// <summary>
        /// Report -002 Transaction Listing -Order Book
        /// 报表 002，PermissionID = 107
        /// </summary>
        public bool HAS_REPORT_002 = false;

        /// <summary>
        /// Report -003 KVB Client Report-Revenue Summary
        /// 报表 003，PermissionID = 108
        /// </summary>
        public bool HAS_REPORT_003 = false;

        /// <summary>
        /// Report -004 KVB Client Summary By Teller Group
        /// 报表 004，PermissionID = 109
        /// </summary>
        public bool HAS_REPORT_004 = false;

        /// <summary>
        /// Report -005 Front Office Cash Box Report
        /// 报表 005，PermissionID = 110
        /// </summary>
        public bool HAS_REPORT_005 = false;

        /// <summary>
        /// Pledge Viewing
        /// 抵押金查看，PermissionID = 111
        /// </summary>
        public bool HAS_PLEDGE_VIEWING = false;

        /// <summary>
        /// Credit Viewing
        /// 授权查看，PermissionID = 112
        /// </summary>
        public bool HAS_CREDIT_VIEWING = false;

        /// <summary>
        /// 配置类：Dealer配置Automation配置 114
        /// </summary>
        public bool HAS_DEALERSETTING = false;

        /// <summary>
        /// 队列：查看请求队列 查看历史请求 115
        /// </summary>
        public bool HAS_REQUESTQUEUE = false;

        /// <summary>
        /// 日志：查看请求处理日志 116
        /// </summary>
        public bool HAS_DEALINGLOG = false;

        /// <summary>
        /// 代客交易R10_UC06代客创建SpotForward单R10_UC08代客创建普通Swap单R10_UC10代客CloseOut117
        /// </summary>
        public bool HAS_DEALMANAGEMENT = false;

        /// <summary>
        /// R10_UC09 代客创建展期 Swap 单（按净仓位）R10_UC15 代客创建展期 Swap 单 （按单） 118
        /// </summary>
        public bool HAS_ROLLOVER = false;

        /// <summary>
        /// R10_UC07 代客创建现金交易 119
        /// </summary>
        public bool HAS_CASHDEAL = false;

        /// <summary>
        /// R10_UC14 删除交易申请 120
        /// </summary>
        public bool HAS_DELETEDEAL = false;

        /// <summary>
        /// R10_UC11 代客创建挂单R10_UC12 代客修改挂单R10_UC13 代客取消挂单121
        /// </summary>
        public bool HAS_PENDINGORDER = false;

        /// <summary>
        /// 权限仓储
        /// </summary>
        private IPermissionsCasheRepository permissionRep;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        public OperatorPermissionModle(string ownerId)
            : base(ownerId)
        {

        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public void Setdata()
        //{
        //    this.permissionRep = this.GetRepository<IPermissionsCasheRepository>();

        //    var pers = this.permissionRep.Filter(o => true).ToList();

        //    if (pers.Count > 0)
        //    {
        //        // PermissionID = 14
        //        HAS_STR_QUOTATION = pers.Where(x => x.PermissionID == "14").FirstOrDefault().HavePermission;

        //        // PermissionID = 15
        //        HAS_QUOTE_WATCHER = pers.Where(x => x.PermissionID == "15").FirstOrDefault().HavePermission;

        //        // PermissionID = 16
        //        HAS_SEND_PRICE = pers.Where(x => x.PermissionID == "16").FirstOrDefault().HavePermission;

        //        // PermissionID = 17
        //        HAS_MODIFY_FORWARD_POINTS = pers.Where(x => x.PermissionID == "17").FirstOrDefault().HavePermission;

        //        // PermissionID = 18
        //        HAS_CUSTOMER_PROFILE_VIEWING = pers.Where(x => x.PermissionID == "18").FirstOrDefault().HavePermission;

        //        // PermissionID = 19
        //        HAS_CUSTOMER_CREATION_PROFILE_MANAGEMENT = pers.Where(x => x.PermissionID == "19").FirstOrDefault().HavePermission;

        //        // PermissionID = 20
        //        HAS_HEDEG_ACCOUNT_MANAGEMENT = pers.Where(x => x.PermissionID == "20").FirstOrDefault().HavePermission;

        //        // PermissionID = 21
        //        HAS_FORWARD_BOOK = pers.Where(x => x.PermissionID == "21").FirstOrDefault().HavePermission;

        //        // PermissionID = 22
        //        HAS_DEALING_BOOK_REPORTS = pers.Where(x => x.PermissionID == "22").FirstOrDefault().HavePermission;

        //        // PermissionID = 23
        //        HAS_TODAY_OPEN_ADJUSTMENT = pers.Where(x => x.PermissionID == "23").FirstOrDefault().HavePermission;

        //        // PermissionID = 24
        //        HAS_DEAL_REQUEST = pers.Where(x => x.PermissionID == "24").FirstOrDefault().HavePermission;

        //        // PermissionID = 25
        //        HAS_OTC_TRANSACTION = pers.Where(x => x.PermissionID == "25").FirstOrDefault().HavePermission;

        //        // PermissionID = 26
        //        HAS_DEAL_VIEWING = pers.Where(x => x.PermissionID == "26").FirstOrDefault().HavePermission;

        //        // PermissionID = 27
        //        HAS_ONLLINE_TRADING = pers.Where(x => x.PermissionID == "27").FirstOrDefault().HavePermission;

        //        // PermissionID = 29
        //        HAS_AD_HOC_FEE = pers.Where(x => x.PermissionID == "29").FirstOrDefault().HavePermission;

        //        // PermissionID = 30
        //        HAS_PLEDGE_MANAGEMENT = pers.Where(x => x.PermissionID == "30").FirstOrDefault().HavePermission;

        //        // PermissionID = 31
        //        HAS_CREDIT_MANAGEMENT = pers.Where(x => x.PermissionID == "31").FirstOrDefault().HavePermission;

        //        // PermissionID = 32
        //        HAS_ANNOUNCEMENT_MANAGEMENT = pers.Where(x => x.PermissionID == "32").FirstOrDefault().HavePermission;

        //        // PermissionID = 33
        //        HAS_SETTLEMENT_MANAGEMENT = pers.Where(x => x.PermissionID == "33").FirstOrDefault().HavePermission;

        //        // PermissionID = 34
        //        HAS_MARGIN_CALL_MONITORING = pers.Where(x => x.PermissionID == "34").FirstOrDefault().HavePermission;

        //        // PermissionID = 44
        //        HAS_SECURITY_ADMIN_LOG = pers.Where(x => x.PermissionID == "44").FirstOrDefault().HavePermission;

        //        // PermissionID = 45
        //        HAS_ACTIVITY_LOG = pers.Where(x => x.PermissionID == "45").FirstOrDefault().HavePermission;

        //        // PermissionID = 46
        //        HAS_LOGIN_LOG = pers.Where(x => x.PermissionID == "46").FirstOrDefault().HavePermission;

        //        // PermissionID = 47
        //        HAS_TRADE_LOG = pers.Where(x => x.PermissionID == "47").FirstOrDefault().HavePermission;

        //        // PermissionID = 48
        //        HAS_ACCOUNT_LOG = pers.Where(x => x.PermissionID == "48").FirstOrDefault().HavePermission;

        //        // PermissionID = 49
        //        HAS_ERROR_LOG = pers.Where(x => x.PermissionID == "49").FirstOrDefault().HavePermission;

        //        // PermissionID = 50
        //        HAS_ADMIN_LOG = pers.Where(x => x.PermissionID == "50").FirstOrDefault().HavePermission;

        //        // PermissionID = 51
        //        HAS_CUSTOMER_LIST = pers.Where(x => x.PermissionID == "51").FirstOrDefault().HavePermission;

        //        // PermissionID = 52
        //        HAS_CUSTOMER_PROFILE = pers.Where(x => x.PermissionID == "52").FirstOrDefault().HavePermission;

        //        // PermissionID = 53
        //        HAS_BENEFICIARY_ACCOUNT_VIEWING = pers.Where(x => x.PermissionID == "53").FirstOrDefault().HavePermission;

        //        // PermissionID = 54
        //        HAS_VIP_CUSTOMER_VIEWING = pers.Where(x => x.PermissionID == "54").FirstOrDefault().HavePermission;

        //        // PermissionID = 55
        //        HAS_CUSTOMER_CREATION = pers.Where(x => x.PermissionID == "55").FirstOrDefault().HavePermission;

        //        // PermissionID = 56
        //        HAS_CUSTOMER_MODIFICATION = pers.Where(x => x.PermissionID == "56").FirstOrDefault().HavePermission;

        //        // PermissionID = 57
        //        HAS_BENEFICIARY_ACCOUNT_MANAGEMENT = pers.Where(x => x.PermissionID == "57").FirstOrDefault().HavePermission;

        //        // PermissionID = 58
        //        HAS_VIP_CUSTOMER_MANAGEMENT = pers.Where(x => x.PermissionID == "58").FirstOrDefault().HavePermission;

        //        // PermissionID = 59
        //        HAS_HEDGE_ACCOUNT_LIST = pers.Where(x => x.PermissionID == "59").FirstOrDefault().HavePermission;

        //        // PermissionID = 60
        //        HAS_HEDGE_ACCOUNT_MODIFICATION = pers.Where(x => x.PermissionID == "60").FirstOrDefault().HavePermission;

        //        // PermissionID = 61
        //        HAS_HEDGE_DEAL_MANAGEMENT = pers.Where(x => x.PermissionID == "61").FirstOrDefault().HavePermission;

        //        // PermissionID = 62
        //        HAS_HEDGE_ACCOUNT_VIEWING = pers.Where(x => x.PermissionID == "62").FirstOrDefault().HavePermission;

        //        // PermissionID = 63
        //        HAS_QUOTE_DEAL = pers.Where(x => x.PermissionID == "63").FirstOrDefault().HavePermission;

        //        // PermissionID = 64
        //        HAS_FILL_ORDER = pers.Where(x => x.PermissionID == "64").FirstOrDefault().HavePermission;

        //        // PermissionID = 65
        //        HAS_FORCE_SELL = pers.Where(x => x.PermissionID == "65").FirstOrDefault().HavePermission;

        //        // PermissionID = 66
        //        HAS_DEAL_APPROVAL = pers.Where(x => x.PermissionID == "66").FirstOrDefault().HavePermission;

        //        // PermissionID = 67
        //        HAS_DEPOSIT_WITHDRAWAL = pers.Where(x => x.PermissionID == "67").FirstOrDefault().HavePermission;

        //        // PermissionID = 68
        //        HAS_DELETE_DEPOSIT_WITHDRAWAL = pers.Where(x => x.PermissionID == "68").FirstOrDefault().HavePermission;

        //        // PermissionID = 69
        //        HAS_ANNOUNCEMENT_DELIVERY = pers.Where(x => x.PermissionID == "69").FirstOrDefault().HavePermission;

        //        // PermissionID = 70
        //        HAS_ANNOUNCEMENT_APPROVAL = pers.Where(x => x.PermissionID == "70").FirstOrDefault().HavePermission;

        //        // PermissionID = 71
        //        HAS_MANUAL_SETTLEMENT = pers.Where(x => x.PermissionID == "71").FirstOrDefault().HavePermission;

        //        // PermissionID = 72
        //        HAS_SETTLE_NOW = pers.Where(x => x.PermissionID == "72").FirstOrDefault().HavePermission;

        //        // PermissionID = 73
        //        HAS_DEPOSIT_WITHDRAWAL_FEE_MODIFICATION = pers.Where(x => x.PermissionID == "73").FirstOrDefault().HavePermission;

        //        // PermissionID = 74
        //        HAS_SETTLEMENT_REVERSAL = pers.Where(x => x.PermissionID == "74").FirstOrDefault().HavePermission;

        //        // PermissionID = 75
        //        HAS_DELETE_DEPOSIT_WITHDRAWAL_FEE_MODIFICATION = pers.Where(x => x.PermissionID == "75").FirstOrDefault().HavePermission;

        //        // PermissionID = 76
        //        HAS_BANK_ACCOUNT_MANAGEMENT = pers.Where(x => x.PermissionID == "76").FirstOrDefault().HavePermission;

        //        // PermissionID = 77
        //        HAS_BANK_ACCOUNT_ROUNDING = pers.Where(x => x.PermissionID == "77").FirstOrDefault().HavePermission;

        //        // PermissionID = 78
        //        HAS_BANK_ACCOUNT_DEPOSIT_WITHDRAWAL = pers.Where(x => x.PermissionID == "78").FirstOrDefault().HavePermission;

        //        // PermissionID = 79
        //        OperatorPermission.HAS_BANK_ACCOUNT_VIEW = pers.Where(x => x.PermissionID == "79").FirstOrDefault().HavePermission;
        //    }
        //}
    }
}
