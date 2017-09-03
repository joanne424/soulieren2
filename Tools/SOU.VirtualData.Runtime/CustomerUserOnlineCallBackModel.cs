// <copyright file="CustomerUserOnlineCallBackModel.cs" company="BancLogix">
//  Copyright (c) Banclogix. All rights reserved.
// </copyright>
// <author>wangmy</author>
// <date> 2016/08/19 02:28:23 </date>
// <summary>  </summary>
// <modify>
//      修改人：wangmy
//      修改时间：2016/08/19 02:28:23
//      修改描述：新建 CustomerUserOnlineCallBackModel.cs
//      版本：1.0
// </modify>
// <review>
//      review人：
//      review时间：
// </review >

namespace DM2.Manager.Models
{
    using System.Linq;

    using BaseViewModel;

    using Infrastructure.Data;

    /// <summary>
    ///     The staff alert call back model.
    /// </summary>
    public class CustomerUserOnlineCallBackModel : BaseModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerUserOnlineCallBackModel"/> class.
        /// </summary>
        /// <param name="ownerId">
        /// The owner id.
        /// </param>
        public CustomerUserOnlineCallBackModel(string ownerId)
            : base(ownerId)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The push back.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        public void PushBack(CustomerOnlineArg arg)
        {
            var reps = this.GetRepository<ICustomerCacheRepository>();
            BaseCustomerViewModel customer = reps.FindByID(arg.CustomerNo);
            if (customer != null)
            {
                BaseCustUserVM user = customer.CustAcctUserList.FirstOrDefault(p => p.UserNo == arg.CustomerUserNo);
                if (user != null)
                {
                    user.IsOnline = arg.IsOnline;
                }

                customer.UpdateOnlineStatus();
            }
        }

        #endregion
    }
}