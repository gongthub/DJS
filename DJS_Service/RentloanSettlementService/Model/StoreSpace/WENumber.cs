using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentloanSettlementService.StoreSpace
{
    /// <summary>
    /// 房间水电
    /// </summary>
    public class WENumber
    {
        public int ID{get;set;}
        public int RoomID{get;set;}
        public virtual Room Room { get; set; }
        public byte Type { get; set; }
        public decimal Number{get;set;}
	    public DateTime Date{get;set;}
        public bool IsPaid { get; set; }
        public int? CreateUserID { get; set; }
        public short OperationType { get; set; }

        private short _Status = 1;
        public short Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public int? PreId { get; set; }

    }

    public class WENumberModifyModel
    {
        public int StoreID { get; set; }
        public int RoomID { get; set; }
        public decimal ColdWater { get; set; }
        public decimal HotWater { get; set; }
        public String FullName { get; set; }
    }


    public class WENumberInitModel
    {
        public int StoreID { get; set; }
        public int RoomID { get; set; }
        public decimal ColdWater { get; set; }
        public decimal HotWater { get; set; }
        public String FullName { get; set; }
        public String ContractID { get; set; }
        public decimal ElectricNumber { get; set; }
    }

   
}
