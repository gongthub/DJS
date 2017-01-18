using EmailToDayReportSumExcel.Model;
using EmailToDayReportSumExcel.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;


namespace EmailToDayReportSumExcel.DBHelper
{
    public class IDBRepository : DbContext
    {
        public IDBRepository()
            : base(ConstUtility.ConnStr)
        {
        }

        public DbSet<AMSService> AMSServices { get; set; }

        public DbSet<AMSServiceSet> AMSServiceSets { get; set; }

        public DbSet<AMSServiceSetEmail> AMSServiceSetEmails { get; set; }


    }
}
