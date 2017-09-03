using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Log;
using Infrastructure.Utils;
using SOU.Model;

namespace Infrastracture.Data
{
    public class CustomerRepository : IDBRepository<Customer>
    {
        private const string AddText = @"INSERT INTO sou_customer " +
                                       "(`name` ,pwd ,rpwd ,state ,nickname ,email ,is_send_email ,Is_send_sms ,sex ,province ,city ,county ,phone ,role ,ur ,state1 ,reg_channel ,address ,actualName ,headerimg ,birthday ,last_login_time ,`status` ,create_time ,isvirtual ) VALUES " +
                                       "(@name ,@pwd ,@rpwd ,@state ,@nickname ,@email ,@is_send_email ,@Is_send_sms ,@sex ,@province ,@city ,@county ,@phone ,@role ,@ur ,@state1 ,@reg_channel ,@address ,@actualName ,@headerimg ,@birthday ,@last_login_time ,@status ,@create_time ,@isvirtual);";

        private const string QueryText = "SELECT * FROM sou_customer WHERE isvirtual = 1;";

        public bool Add(Customer item)
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

        public bool Update(Customer item)
        {
            return true;
        }

        public bool Remove(string id)
        {
            return true;
        }

        public List<Customer> Roots()
        {
            var resultList =
                MySqlDbHelper.QueryList<Customer>(MySqlDbHelper.GetConnection(ConfigParameter.SqlConnectionStr),
                    QueryText);
            if (resultList == null || resultList.Any() == false)
            {
                resultList = new List<Customer>();
            }

            return resultList;
        }

        public Customer FindByID(string id)
        {
            throw new NotImplementedException();
        }

        public bool Any(Func<Customer, bool> filter)
        {
            return true;
        }
    }
}