using SyncLowPowerRoomService.Model;
using SyncLowPowerRoomService.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLowPowerRoomService.Service
{
    public class IDBRepository : DbContext
    {
        public IDBRepository()
            : base(ConstUtility.IDBRepository)
        {
        }

        public DbSet<Store> Stores { get; set; }

        public DbSet<LowPowerRoom> LowPowerRooms { get; set; }
    }
}
