using RentloanSettlementService.StoreSpace;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentloanSettlementService.Reservation
{
    public class Reservation
    {
        public int ID{get;set;}

        public int RoomID{get;set;}

        public virtual Room Room{get;set;}

        public int RenterID{get;set;}

        public virtual Renter Renter { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime CheckInDate{get;set;}

        [Display(Name = "预订号码")]
        public string ReservationNo{get;set;}

        public byte Status { get; set; }

        public String RenterName { get; set; }
    }
}
