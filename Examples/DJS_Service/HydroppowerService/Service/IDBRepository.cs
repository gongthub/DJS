using HydroppowerService.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydroppowerService.Services
{
    public class IDBRepository : DbContext
    {

        public IDBRepository()
            : base(ConstUtility.IDBRepository)
        {
        }
          
    }
}
