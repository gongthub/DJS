using OpsModel.Charge;
using RentloanSettlementService.ChargeSpace;
using RentloanSettlementService.ContractSpace;
using RentloanSettlementService.PaymentSpace;
using RentloanSettlementService.StoreSpace;
using RentloanSettlementService.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterElectricService.Model.RentLoan;

namespace RentloanSettlementService
{
    public class EFDbContext:DbContext
    {
        public EFDbContext()
            : base(ConstUtility.IDBEmailService)
        {

        }
      
        public DbSet<BankCard> BankCards { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PayMethodType> PayMethodTypes { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractFee> ContractFees { get; set; }
        public DbSet<ContractWaterElectSet> ContractWaterElectSets { get; set; }
        public DbSet<ChargeAccountType> ChargeAccountTypes { get; set; }
        public DbSet<Renter> Renters { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomFee> RoomFees { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreFee> StoreFees { get; set; }
        public DbSet<WENumber> WENumbers { get; set; }
        public DbSet<StoreElecShip> StoreElecShips { get; set; }
        public DbSet<Charge> Charges { get; set; }
        public DbSet<ChargeDetail> ChargeDetails { get; set; }
        public DbSet<PeriodicCharge> PeriodicCharges { get; set; }
        public DbSet<PeriodicChargeDetail> PeriodicChargeDetails { get; set; }
        public DbSet<ExcelImport> ExcelImports { get; set; }

        public DbSet<RentLoanPool> RentLoanPools { get; set; }
        public DbSet<RentLoanHistoryPool> RentLoanHistoryPools { get; set; }
        public DbSet<RentLoanSettlementLog> RentLoanSettlementLogs { get; set; }
        public DbSet<RentLoanAudit> RentLoanAudits { get; set; }
        public DbSet<RentLoanAuditLog> RentLoanAuditLogs { get; set; }
        public DbSet<BankColumn> BankColumns { get; set; }
        public DbSet<RentloanSettlementService.Model.RentLoan.RentLoanAuditContract> RentLoanAuditContracts { get; set; }
    }
}

