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
    public class IDBRepository : DbContext
    {
        public IDBRepository()
            : base(ConstUtility.IDBRepository)
        {

        }

        public DbSet<ProjectUpdateLog> ProjectUpdateLogs { get; set; }
        public DbSet<AMSService> AMSServices { get; set; }

        public DbSet<AMSServiceSetEmail> AMSServiceSetEmails { get; set; }

        public DbSet<AMSServiceSet> AMSServiceSets { get; set; }


    }
}
