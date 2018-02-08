using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLowPowerRoomService.Model
{
    public class LowPowerRoom
    {
        public int ID { get; set; }

        public string StoreCode { get; set; }

        public string RoomName { get; set; }

        public int? Electric { get; set; }

        public DateTime? CreateTime { get; set; }

        private int _Status = 1;
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public int SystemType { get; set; }
    }
}
