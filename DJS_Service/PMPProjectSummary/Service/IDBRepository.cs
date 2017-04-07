using PMPProjectSummary.Model;
using PMPProjectSummary.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMPProjectSummary.Service
{
    public class IDBRepository : DbContext
    {
        public IDBRepository()
            : base(ConstUtility.IDBRepository)
        {

        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<ProjectDetail> ProjectDetails { get; set; }

        public DbSet<ProjectNode> ProjectNodes { get; set; }

        public DbSet<AMSService> AMSServices { get; set; }

        public DbSet<AMSServiceSetEmail> AMSServiceSetEmails { get; set; }

        public DbSet<AMSServiceSet> AMSServiceSets { get; set; }


    }
}
