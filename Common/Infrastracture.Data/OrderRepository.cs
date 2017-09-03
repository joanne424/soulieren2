using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Log;
using Infrastructure.Utils;
using SOU.Model;

namespace Infrastracture.Data
{
    public class OrderRepository : IDBRepository<Order>
    {
        private const string AddText = @"INSERT INTO sou_order " +
                                       "( id ,order_sn ,user_id ,status ,best_time ,shipping_id ,shipping_num ,shipping_name ,pay_id ,pay_name ,goods_amount ,shipping_fee ,balance ,paid ,money_paid ,create_time ,confirm_time ,shipping_time ,pay_time ,done_time ,order_channel ,order_type ,invoice_status ,invoice_title ,depot_sn ,comments ,modify_time ,modify_user ,cancel_text ,is_comment ,web_source ,depot_id ,address ,isvirtual ) VALUES " +
                                       "(@id ,@order_sn ,@user_id ,@status ,@best_time ,@shipping_id ,@shipping_num ,@shipping_name ,@pay_id ,@pay_name ,@goods_amount ,@shipping_fee ,@balance ,@paid ,@money_paid ,@create_time ,@confirm_time ,@shipping_time ,@pay_time ,@done_time ,@order_channel ,@order_type ,@invoice_status ,@invoice_title ,@depot_sn ,@comments ,@modify_time ,@modify_user ,@cancel_text ,@is_comment ,@web_source ,@depot_id ,@address ,@isvirtual);";

        private const string QueryText = "SELECT * FROM sou_order WHERE isvirtual = 1; ";

        public bool Add(Order item)
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

        public bool Update(Order item)
        {
            throw new NotImplementedException();
        }

        List<Order> IDBRepository<Order>.Roots()
        {
            var resultList =
                MySqlDbHelper.QueryList<Order>(MySqlDbHelper.GetConnection(ConfigParameter.SqlConnectionStr),
                    QueryText);
            if (resultList == null || resultList.Any() == false)
            {
                resultList = new List<Order>();
            }

            return resultList;
        }

        Order IDBRepository<Order>.FindByID(string id)
        {
            throw new NotImplementedException();
        }

        public bool Any(Func<Order, bool> filter)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string id)
        {
            throw new NotImplementedException();
        }
    }
}