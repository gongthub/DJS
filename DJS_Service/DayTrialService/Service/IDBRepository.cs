using DayTrialService.Model;
using DayTrialService.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayTrialService.Services
{
    public class IDBRepository : DbContext
    {

        public IDBRepository()
            : base(ConstUtility.IDBRepository)
        {
        }

        public DbSet<Store> Stores { get; set; }

        public DbSet<AMSService> AMSServices { get; set; }

        public DbSet<AMSServiceSet> AMSServiceSets { get; set; }

        public DbSet<AMSServiceSetEmail> AMSServiceSetEmails { get; set; }
    }
}
