using DJS.Common;
using DJS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.IDAL
{
    public interface IUserMgr : IBaseMgr<UserEntity>
    {
        UserEntity GetFormEnableByUserName(string username);
    }
}
