using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DayTrialService.Model
{
    public class AMSService
    {
        [Display(Name = "主键ID")]
        public int ID { get; set; }

        [Display(Name = "编码")]
        public string ServiceCode { get; set; }

        [Display(Name = "名称")]
        public string ServiceName { get; set; }

        [Display(Name = "类型")]
        public int? ServiceType { get; set; }

        [Display(Name = "程序名称")]
        public string ProgramName { get; set; }

        [Display(Name = "是否启用")]
        public bool IsStart { get; set; }

        [Display(Name = "描述")]
        public string Descript { get; set; }

        [Display(Name = "是否有附件")]
        public bool IsHasAttachment { get; set; }

        private int _Status = 1;
        [Display(Name = "状态")]
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
    }
}
