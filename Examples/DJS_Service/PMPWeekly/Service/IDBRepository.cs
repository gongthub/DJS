using PMPWeekly.Utils;
using PMPWeekly.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMPWeekly.Service
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
    }
}
