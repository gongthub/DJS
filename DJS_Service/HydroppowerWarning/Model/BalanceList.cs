using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydroppowerWarning.Model
{
    public class BalanceList
    {
        public int ID { get; set; }

        public string community_no { get; set; }

        public string user_address_room { get; set; }

        public int meter_type { get; set; }

        public string operate_day { get; set; }

        public decimal balance { get; set; }
    }
}
