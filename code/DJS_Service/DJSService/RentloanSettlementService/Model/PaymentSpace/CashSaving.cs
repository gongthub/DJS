using RentloanSettlementService.Common;
using RentloanSettlementService.StoreSpace;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace RentloanSettlementService.PaymentSpace
{
    public class CashSaving
    {
        private int _Status = (int)CommonEnum.RecordStatus.正常;

        public int ID { get; set; }
        public int StoreID { get; set; }
        public virtual Store Store { get; set; }
        [Display(Name = "存款日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Date { get; set; }
        [Display(Name = "本次存入现金(元)")]
        public decimal SavingCash { get; set; }
        [Required,Display(Name = "存款单据号")]
        public string SavingSerialNo { get; set; }

        public int Status
        {
          get { return _Status; }
          set { _Status = value; }
        }


    }
}
