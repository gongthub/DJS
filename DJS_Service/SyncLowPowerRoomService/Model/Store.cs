using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLowPowerRoomService.Model
{
    public class Store
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int RoomNo { get; set; }

        public DateTime? OpeningDate { get; set; }

        public int EmptyRoom { get; set; }
        public int BookedRoom { get; set; }

        public int LeasedRoom { get; set; }

        public int FixingRoom { get; set; }

        public int StaffOccupied { get; set; }

        public DateTime? EndLease { get; set; }


        public string ManagerPhone { get; set; }

        public string StoreCode { get; set; }


        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedUser { get; set; }

        public short Status { get; set; }
    }
}
