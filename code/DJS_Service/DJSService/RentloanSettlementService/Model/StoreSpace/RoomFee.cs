using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentloanSettlementService.StoreSpace
{
    public class RoomFee
    {
        public int ID { get; set; }
        
        public int RoomID { get; set; }
        
        public virtual Room Room { get; set; }
        
        public decimal Deposit{get;set;}

        public decimal FeeForSeason { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedUser { get; set; }
    }
}
