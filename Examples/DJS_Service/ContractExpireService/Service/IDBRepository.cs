using ContractExpireService.Model;
using ContractExpireService.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractExpireService.Service
{
    public class IDBRepository : DbContext
    {
        public IDBRepository()
            : base(ConstUtility.IDBRepository)
        {
        }

        public DbSet<AMSService> AMSServices { get; set; }
        public DbSet<AMSServiceSet> AMSServiceSets { get; set; }
        public DbSet<AMSServiceSetEmail> AMSServiceSetEmails { get; set; }
        public DbSet<ContractExpireView> ContractExpireViews { get; set; }
    }
}
