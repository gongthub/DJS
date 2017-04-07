using PMPRoomChangeService.Model;
using PMPRoomChangeService.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMPRoomChangeService.Service
{
    public class IDBAMSRepository : DbContext
    {
        public IDBAMSRepository()
            : base(ConstUtility.IDBAMSRepository)
        { }
        public DbSet<AMSService> AMSServices { get; set; }
        public DbSet<AMSServiceSet> AMSServiceSets { get; set; }
        public DbSet<AMSServiceSetEmail> AMSServiceSetEmails { get; set; }

    }
}
