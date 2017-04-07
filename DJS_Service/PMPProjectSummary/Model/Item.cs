using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMPProjectSummary.Model
{
    public class Item
    {
        public int ID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
        public int CityID { get; set; }
        //[ForeignKey("CityID")]
        //public virtual Citys City { get; set; }
        public decimal? LeaseArea { get; set; }
        public DateTime? SignDate { get; set; }
        //[DisplayFormat(DataFormatString = "{0:F0}")]
        public decimal? YOC { get; set; }
        public bool? Cancel { get; set; }

        public string FinanceCode { get; set; }
        public string ChargeCode { get; set; }

        public int? ProvinceID { get; set; }

        //[ForeignKey("ProvinceID")]
        //public virtual Province Province { get; set; }

        public int? DistrictID { get; set; }

        //[ForeignKey("DistrictID")]
        //public virtual District District { get; set; }
        public int? Type { get; set; }

        //[ForeignKey("Type")]
        //public virtual CommonType CommonType { get; set; }

        public int? RoomNumber { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? FreeDate { get; set; }

        public DateTime? ExpectedStartTime { get; set; }

        public DateTime? ExpectedEndTime { get; set; }

        public DateTime? ScheduledTime { get; set; }

        public DateTime? ActualTime { get; set; }

        public DateTime? CreateDate { get; set; }

        public int CreateUserID { get; set; }
    }
}
