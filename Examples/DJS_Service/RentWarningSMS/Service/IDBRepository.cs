using RentWarningSMS.Model;
using RentWarningSMS.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace RentWarningSMS.Service
{
    public class IDBRepository : DbContext
    {

        public IDBRepository()
            : base(ConstUtility.IDBRepository)
        {
        }

        public DbSet<RentExpireView> RentExpireViews { get; set; }
    }
}
