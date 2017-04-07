using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractChangeLogService.Model
{
    public class ContractChangeLogView
    {
        [Key]
        public Int64 ID { get; set; }

        public int StoreID { get; set; }
        public string StoreName { get; set; }

        public string FullName { get; set; }

        public string Name { get; set; }

        public string ContractNo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime LastChangeEndDate { get; set; }

        public short Status { get; set; }

        public string Reason { get; set; }

        public string UserName { get; set; }

        public DateTime CreateDate { get; set; }

        public string DataStatus { get; set; }
    }
}
