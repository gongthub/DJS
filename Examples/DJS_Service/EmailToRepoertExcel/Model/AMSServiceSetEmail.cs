using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EmailToRepoertExcel.Model
{
    public class AMSServiceSetEmail
    {
        [Display(Name = "主键ID")]
        public int ID { get; set; }

        [Display(Name = "所在项目ID")]
        public int AMSServiceSetID { get; set; }

        [Display(Name = "门店ID")]
        public string StoreID { get; set; }

        [Display(Name = "收件人")]
        public string TOPeople { get; set; }

        [Display(Name = "抄送人")]
        public string CCPeople { get; set; }

        [Display(Name = "是否发送汇总")]
        public bool IsSum { get; set; }

        private int _Status = 1;
        [Display(Name = "状态")]
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
    }
}
