using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RentloanSettlementService.StoreSpace
{
    public class Room 
    {
        public int ID { get; set; }
        public int StoreID { get; set; }
        public virtual Store Store { get; set; }
        public short FloorNo { get; set; }

        [Display(Name = "房间")]
        public short RoomNo { get; set; }
        [Display(Name = "房间")]
        public string FullName { get; set; }
        public int? Status { get; set; }

        [Display(Name = "修改时间")]
        public DateTime? ModifiedDate { get; set; }
        [Display(Name = "修改人")]
        public int? ModifiedUser { get; set; }

        public int? RoomTypeID { get; set; }

        public String RoomCode { get; set; }
        public int BuildingID { get; set; }

        [NotMapped]
        public int IsBooked { get; set; }

    }
}