using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanFinancialService.Model
{
    public class RentLoanAuditContract
    {
        public int ID { get; set; }

        /// <summary>
        /// 租金贷流程ID
        /// </summary>
        public int RentLoanAuditID { get; set; }

        /// <summary>
        /// 合同id
        /// </summary>
        public int ContractID { get; set; }

        /// <summary>
        /// 门店id
        /// </summary>
        public int StoreID { get; set; }

        /// <summary>
        /// 房间id
        /// </summary>
        public int RoomID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public int CreateUserID { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public int? ModifiedUserID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
