using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentloanSettlementService.Reservation
{
    public class ReservationOps
    {
        public int ID{get;set;}
        public int ReservationID{get;set;}
        public virtual Reservation Reservation{get;set;}
        public byte OperationType { get; set; }
        public string Comment{get;set;}
        public int CreateUserID{get;set;}
	    public DateTime CreateDate{get;set;}
    }
}
