using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace PMPRoomChangeService.Model
{
    [Table("ProjectUpdateLogs")]
    public class ProjectUpdateLog
    {
        public int id { get; set; }

        public int ProjectID { get; set; }

        public int? RoomNumber { get; set; }

        public int? BedNumber { get; set; }

        public int? NotSellRoomNumber { get; set; }

        public int? NotSellBedNumber { get; set; }

        public int? CreateUserID { get; set; }

        public DateTime? CreateUserDate { get; set; }
          
        public int? OldRoomNumber { get; set; }

        public int? OldBedNumber { get; set; }

        public int? OldNotSellRoomNumber { get; set; }

        public int? OldNotSellBedNumber { get; set; } 

        public string ProjectName { get; set; }

        public string ItemName { get; set; }

        public string UserName { get; set; }

        public string ModifyReason { get; set; }
    }
}
