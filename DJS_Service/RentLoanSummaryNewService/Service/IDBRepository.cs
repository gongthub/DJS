using RentLoanSummaryNewService.Model;
using RentLoanSummaryNewService.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanSummaryNewService.Service
{
    public class IDBRepository : DbContext
    {

        public IDBRepository()
            : base(ConstUtility.IDBEmailService)
        {
        }
        public DbSet<AMSService> AMSServices { get; set; }
        public DbSet<AMSServiceSet> AMSServiceSets { get; set; }
        public DbSet<AMSServiceSetEmail> AMSServiceSetEmails { get; set; }
        public DbSet<RentLoanSummary> RentLoanSummaries { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<RentLoanRisk> RentLoanRisks { get; set; }
    }
}
