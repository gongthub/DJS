using ContractWarningSMS.Model;
using ContractWarningSMS.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace ContractWarningSMS.Service
{
    public class IDBRepository : DbContext
    {

        public IDBRepository()
            : base(ConstUtility.IDBRepository)
        {
        }

        public DbSet<ContractExpireForServiceView> ContractExpireForServiceViews { get; set; }
    }
}
