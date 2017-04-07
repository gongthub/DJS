using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanFinancialService.Service
{
    public interface IRentLoanFinancialService
    {
        //放款
        void BankLoanDetail();

        //还款
        void UploadRentLoanPool();
        //逾期
        void OverdueDetailList();
        //拉取互联网金融数据
        void PullRentLoanFinancialData();
    }
}
