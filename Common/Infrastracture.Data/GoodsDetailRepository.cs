using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Utils;
using SOU.Model;

namespace Infrastracture.Data
{
    public class GoodsDetailRepository : IDBRepository<Goods>
    {
        private const string QueryText = "SELECT * FROM sou_order_details WHERE isvirtual = 1; ";

        public bool Add(Goods item)
        {
            return true;
        }

        public bool Update(Goods item)
        {
            return true;
        }

        public List<Goods> Roots()
        {
            var resultList =
                MySqlDbHelper.QueryList<Goods>(MySqlDbHelper.GetConnection(ConfigParameter.SqlConnectionStr),
                    QueryText);
            if (resultList == null || resultList.Any() == false)
            {
                resultList = new List<Goods>();
            }

            return resultList;
        }

        public Goods FindByID(string id)
        {
            return null;
        }

        public bool Any(Func<Goods, bool> filter)
        {
            return true;
        }

        public bool Remove(string id)
        {
            return true;
        }
    }
}