using RentloanSettlementService.ContractSpace;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentloanSettlementService.ContractSpace
{
    public class ExcelImport
    {
        public int ID { get; set; }
        public int ContractID { get; set; }
        public virtual Contract Contract { get; set; }

        public int Number { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }

        public string BuildingName { get; set; }

        public string FullName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal CharterMoney { get; set; }

        public decimal Deposit { get; set; }

        public string Year { get; set; }

        public int Status { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
