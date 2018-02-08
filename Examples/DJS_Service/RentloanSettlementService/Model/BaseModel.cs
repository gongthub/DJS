using RentloanSettlementService.ContractSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSM.Model
{
    public class BaseModel
    {
        public List<Contract> ListContract { get; set; }

        public List<ContractFee> ListContractFee { get; set; }
    }
}
