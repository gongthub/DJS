using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReserveOverdueService.Model
{
    public class ReserveOverdue
    {
        public int ID { get; set; }

        public int StoreID { get; set; }

        public string StoreName { get; set; }

        public string FullName { get; set; }

        public string YuDingName { get; set; }

        public string YuDingPhone { get; set; }

        public string ReservationNo { get; set; }

        public DateTime CheckInDate { get; set; }

        public string RenterName { get; set; }

        public int OverdueDay { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
