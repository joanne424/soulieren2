using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMSM
{
    public class ContractEntity
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public ContractEntity Clone()
        {
            return this.MemberwiseClone() as ContractEntity;
        }
    }
}
