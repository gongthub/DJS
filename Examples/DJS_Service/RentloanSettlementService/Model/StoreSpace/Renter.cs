
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentloanSettlementService.StoreSpace
{
    /// <summary>
    /// 租客信息
    /// </summary>
    public class Renter
    {
        public int ID{get;set;}

        [Display(Name = "客户编码")]
        public string CustomerCode { get; set; }

        [Display(Name = "姓名")]
        public string Name{get;set;}

        [Display(Name = "性别")]
        public bool? Male { get; set; }

        [Display(Name = "证件号码")]
        public string SSN { get; set; }

        [Display(Name = "联系地址")]
        public string Address { get; set; }

        [Display(Name = "电话")]
	    public string Phone{get;set;}

        [Display(Name = "暂住证")]
        public bool? Permits{get;set;}

        [Display(Name = "证件类型")]
	    public byte? Nationality{get;set;}

        [Display(Name = "职业")]
        public string Career { get; set; }

        private short _Status = 1;
        public short Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public void SetRenter(Renter renter)
        {
            this.Name = renter.Name == null ? this.Name : renter.Name.Trim();
            this.Male = renter.Male == null ? this.Male : renter.Male;
            this.SSN = renter.SSN == null ? this.SSN : renter.SSN;
            this.Address = renter.Address == null ? this.Address : renter.Address;
            this.Phone = renter.Phone;
            this.Permits = renter.Permits == null ? this.Permits : renter.Permits;
            this.Nationality = renter.Nationality == null ? this.Nationality : renter.Nationality;
            this.Career = renter.Career == null ? this.Career : renter.Career;
            this.Status = renter.Status;
        }
    }
}
