using RentLoanFinancialService.Model;
using RentLoanFinancialService.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanFinancialService.Service
{
    public class IDBRepository:DbContext
    {
        public IDBRepository()
            : base(ConstUtility.IDBRepositoy)
        {
        }

        public DbSet<BankLoanDetail> BankLoanDetails { get; set; }

        public DbSet<RentLoanAudit> RentLoanAudits { get; set; }

        public DbSet<RentLoanAuditContract> RentLoanAuditContracts { get; set; }

        public DbSet<RentLoanPool> RentLoanPools { get; set; }

        public DbSet<RentLoanFinancialLog> RentLoanFinancialLogs { get; set; }

        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractOps> ContractOps { get; set; }
    }
}
