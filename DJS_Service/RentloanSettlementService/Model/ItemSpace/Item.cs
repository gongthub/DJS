using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentloanSettlementService.ItemSpace
{
    public class Item
    {
        public int ID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
        public int CityID { get; set; }
        public virtual City City { get; set; }
        public decimal? LeaseArea { get; set; }
        [NotMapped]
        public int? Room { get; set; }
        public DateTime? SignDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:F0}")]
        public decimal? YOC { get; set; }
        public bool? Cancel { get; set; }

        public string FinanceCode { get; set; }
        public string ChargeCode { get; set; }
    }
}