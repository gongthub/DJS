using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopChargeWarnService.Model
{
    public class AMSServiceSet
    {
        public int ID { get; set; }

        public int AMSServiceID { get; set; }

        public string Name { get; set; }

        public int? SendType { get; set; }

        public string Descript { get; set; }

        private int _Status = 1;
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
    }
}
