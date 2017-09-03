using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Log;
using Infrastructure.Utils;
using SOU.Model;

namespace Infrastracture.Data
{
    public class OrderDetailsRepository : IDBRepository<OrderDetails>
    {
        private const string AddText = @"INSERT INTO sou_order_details" +
                                       "( id ,order_id ,goods_id ,sn ,name ,type_details1 ,type_details2 ,goods_price ,rebate_price ,paid_price ,goods_count ,goods_img ,goods_type ,depot_id ,refund_count ,create_time ,shopid ) VALUES" +
                                       "(@id ,@order_id ,@goods_id ,@sn ,@name ,@type_details1 ,@type_details2 ,@goods_price ,@rebate_price ,@paid_price ,@goods_count ,@goods_img ,@goods_type ,@depot_id ,@refund_count ,@create_time ,@shopid);";

        private const string QueryText = "SELECT * FROM sou_order_details WHERE isvirtual = 1; ";

        public bool Add(OrderDetails item)
        {
            try
            {
                MySqlDbHelper.Add(MySqlDbHelper.GetConnection(ConfigParameter.SqlConnectionStr), AddText, item);
            }
            catch (Exception ex)
            {
                TraceManager.Error.Write("CustomerRepository.Add", ex);
                return false;
            }

            return true;
        }

        public bool Update(OrderDetails item)
        {
            return true;
        }

        List<OrderDetails> IDBRepository<OrderDetails>.Roots()
        {
            var resultList =
                MySqlDbHelper.QueryList<OrderDetails>(MySqlDbHelper.GetConnection(ConfigParameter.SqlConnectionStr),
                    QueryText);
            if (resultList == null || resultList.Any() == false)
            {
                resultList = new List<OrderDetails>();
            }

            return resultList;
        }

        public OrderDetails FindByID(string id)
        {
            return null;
        }

        public bool Any(Func<OrderDetails, bool> filter)
        {
            return true;
        }

        public bool Remove(string id)
        {
            return true;
        }
    }
}