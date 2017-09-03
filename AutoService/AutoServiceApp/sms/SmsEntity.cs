using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceApp
{
    public class SmsEntity
    {
        public int id { get; set; }

        public string to_phone { get; set; }

        public string result_body { get; set; }

        public bool result { get; set; }

        public string template_id { get; set; }

        public string param_list { get; set; }

        public int status { get; set; }

        public int opt_type { get; set; }

        public int priority { get; set; }

        public int try_time { get; set; }

        public DateTime create_time { get; set; }

        public DateTime last_update_time { get; set; }

        public int operate_id { get; set; }

    }
}
