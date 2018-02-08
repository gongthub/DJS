using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using RentloanSettlementService.StoreSpace;

namespace RentloanSettlementService.ContractSpace
{
    public class ContractWaterElectSet
    {
        public int ID { set; get; }

        public int ContractID { set; get; }

        public virtual Contract Contract { get; set; }

        public decimal? InitValue { get; set; }

        public decimal? LastValue { get; set; }

        public short SubjectType { get; set; }

        public DateTime ModifiedDate { get; set; }

        public short Status { get; set; }

      
    }

    public class WaterElectAccount
    {
        public int StoreID { get; set; }

        public string FullName { get; set; }

        public string ContractNo { get; set; }

        public decimal WaterElectCharge { get; set; }

        public decimal ColdCharge { get; set; }

        public decimal HotCharge { get; set; }

        public decimal ElectricCharge { get; set; }

        public decimal WaterElectChargeBalance { get; set; }

    }
}
