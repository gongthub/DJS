using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace PMPProjectSummary.Model
{
    public  class PMPServiceSetEmail
    {
      
       /// <summary>
        /// 主键ID
       /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 所在项目ID
        /// </summary>
        public int AMSServiceSetID { get; set; }

       /// <summary>
        /// 门店ID
       /// </summary>
        public string StoreID { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string TOPeople { get; set; }

        /// <summary>
        /// 抄送人
        /// </summary>
        public string CCPeople { get; set; }

        /// <summary>
        /// 是否发送汇总
        /// </summary>
        public bool IsSum { get; set; }

        private int _Status = 1;
        /// <summary>
        /// 状态
        /// </summary>
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
    }
}
