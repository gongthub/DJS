﻿using EmailToRepoertExcel.Model;
using EmailToRepoertExcel.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;


namespace EmailToRepoertExcel.DBHelper
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