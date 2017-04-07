using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMPProjectSummary.Model
{
    public  class ProjectIntro
    {
        /// <summary>
        /// 工期类别
        /// </summary>
        public int ProjectType{get;set;}
        /// <summary>
        /// 项目类别
        /// </summary>
        public int ItemType { get; set; }
        /// <summary>
        /// 工期数
        /// </summary>
        public int? ProjectNum { get; set; }


        public int? Room { get; set; }

        public int? NotSellRoom { get; set; }

        public int? BedNumber { get; set; }

        public int? NotSellBedNumber { get; set; }
        /// <summary>
        /// 房间数
        /// </summary>
        public int? RoomNumber { get; set; }

        /// <summary>
        /// 床位数
        /// </summary>
        public int? BedNumber1 { get; set; }
    }
}
