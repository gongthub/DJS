using RentloanSettlementService.ItemSpace;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentloanSettlementService.StoreSpace
{
    public class Store
    {
        public int ID { get; set; }

        public int? ItemID { get; set; }

        public virtual Item Item { get; set; }

        [Display(Name = "店名")]
        public string Name { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "总房间数")]
        public int RoomNo { get; set; }

        [Display(Name = "开业时间")]
        [DataType(DataType.Date)]
        public DateTime? OpeningDate { get; set; }

        [Display(Name = "空房数")]
        public int EmptyRoom { get; set; }

        [Display(Name = "已预定房间数")]
        public int BookedRoom { get; set; }

        [Display(Name = "已出租房间数")]
        public int LeasedRoom { get; set; }

        [Display(Name = "维修")]
        public int FixingRoom { get; set; }

        [Display(Name = "自用")]
        public int StaffOccupied { get; set; }

        [Display(Name = "结束招租时间")]
        [DataType(DataType.Date)]
        public DateTime? EndLease { get; set; }

        [NotMapped]
        [Display(Name = "店长")]
        public string Manager { get; set; }

        [Display(Name = "联系方式")]
        public string ManagerPhone { get; set; }

        [Display(Name = "店编码")]
        public string StoreCode { get; set; }

        [NotMapped]
        public decimal MinRoomFee { get; set; }
        [NotMapped]
        public decimal MaxRoomFee { get; set; }

        [Display(Name="修改时间")]
        public DateTime? ModifiedDate { get; set; }
        [Display(Name="修改人")]
        public int? ModifiedUser { get; set; }

        public short Status { get; set; }

      
    }

}