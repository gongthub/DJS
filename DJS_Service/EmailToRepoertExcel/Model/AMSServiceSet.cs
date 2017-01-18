using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EmailToRepoertExcel.Model
{
    public class AMSServiceSet
    {
        [Display(Name = "主键ID")]
        public int ID { get; set; }

        [Display(Name = "所在服务ID")]
        public int AMSServiceID { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "发送类型")]
        public int? SendType { get; set; }
         

        [Display(Name = "描述")]
        public string Descript { get; set; }

        private int _Status = 1;
        [Display(Name = "状态")]
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
    }
}
