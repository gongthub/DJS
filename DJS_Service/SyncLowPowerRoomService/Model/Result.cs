using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLowPowerRoomService.Model
{
    public class Result
    {
        public bool success { get; set; }

        public object data { get; set; }
    }

    public class ResultLowPowerRoom
    {
        public string building_id { get; set; }

        public string building_num { get; set; }

        public string room_name { get; set; }

        public string door_name { get; set; }

        public string electric { get; set; }
    }
}
