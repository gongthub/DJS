using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayTrialAfterCashService.Model
{
    public class AMSService
    {
        public int ID { get; set; }

        public string ServiceCode { get; set; }

        public string ServiceName { get; set; }

        public int? ServiceType { get; set; }

        public string ProgramName { get; set; }

        public bool IsStart { get; set; }

        public string Descript { get; set; }

        public bool IsHasAttachment { get; set; }

        private int _Status = 1;
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
    }
}
