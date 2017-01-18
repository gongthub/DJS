﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoChargeWarnService.Model
{
    public class NoChargeWarnView
    {
        /// <summary>
        /// PaymentMethodID
        /// </summary>
        public int ID { get; set; }

        public int StoreID { get; set; }

        public string StoreName { get; set; }

        public int RoomID { get; set; }

        public string FullName { get; set; }

        public int RenterID { get; set; }

        public string RenterName { get; set; }

        public string Phone { get; set; }

        public string ContractNo { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string ContractType { get; set; }
        public string ContractStatus { get; set; }

        public string PaySerialNo { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string FeeTypeName { get; set; }

        public string DifferDesc { get; set; }
    }
}
