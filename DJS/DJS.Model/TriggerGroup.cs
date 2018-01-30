using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Model
{
    public class TriggerGroup : IEntity<TriggerGroup>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        //主键
        public string ID { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int SortCode { set; get; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 触发器数量
        /// </summary>
        public int TriggerNum { get; set; }

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
