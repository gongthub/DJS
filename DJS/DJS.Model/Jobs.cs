﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Model
{
    public class Jobs : IEntity<Jobs>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string ID { set; get; }
        public string Name { set; get; }
        public string GroupName { set; get; }
        public string TriggerName { set; get; }
        public string TriggerGroup { set; get; }
        public string Crons { set; get; }
        public DateTime Time { set; get; }
        public int Type { set; get; }
        public string TypeName { set; get; }
        public Type AssType { set; get; }
        public int State { set; get; }
        public string StateName { set; get; }
        public string DLLID { set; get; }
        public string DLLName { set; get; }
        public string ConfigName { set; get; }
        public bool IsAuto { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { set; get; }

        public bool? DeleteMark { get; set; }
        public bool? EnabledMark { get; set; }
        public DateTime? CreatorTime { get; set; }
        public string CreatorUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }
        public string LastModifyUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string DeleteUserId { get; set; }

    }
}
