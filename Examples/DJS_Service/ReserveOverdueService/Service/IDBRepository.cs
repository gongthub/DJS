using ReserveOverdueService.Model;
using ReserveOverdueService.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReserveOverdueService.Service
{
    class IDBRepository : DbContext
    {
        public IDBRepository()
            : base(ConstUtility.IDBRepository)
        {
        }


        public DbSet<ReserveOverdue> ReserveOverdues { get; set; }

        public DbSet<AMSService> AMSServices { get; set; }

        public DbSet<AMSServiceSet> AMSServiceSets { get; set; }

        public DbSet<AMSServiceSetEmail> AMSServiceSetEmails { get; set; }
    }
}
