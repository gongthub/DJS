using NoChargeWarnService.Model;
using NoChargeWarnService.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoChargeWarnService.Service
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
        public DbSet<NoChargeWarnView> NoChargeWarnViews { get; set; }
    }
}
