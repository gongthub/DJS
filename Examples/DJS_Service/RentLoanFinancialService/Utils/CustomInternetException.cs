using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanFinancialService.Utils
{
    public class CustomInternetException:Exception
    {
        public  DateTime PullStartTime;
        public CustomInternetException()
            : base()
        {

        }

        public CustomInternetException(DateTime pullStartTime):base()
        {
            PullStartTime = pullStartTime;
        }

        public CustomInternetException(string message,DateTime pullStartTime,Exception innerException)
            : base(message,innerException)
        {
            PullStartTime = pullStartTime;
        }

        public CustomInternetException(string message)
            : base(message)
        {

        }

        public CustomInternetException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
