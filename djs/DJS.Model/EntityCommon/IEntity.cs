using DJS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Model
{
    public class IEntity<TEntity>
    {
        public void Create()
        {
            var entity = this as ICreationAudited;
            entity.ID = Guid.NewGuid().ToString();
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            if (LoginInfo != null)
            {
                entity.CreatorUserId = LoginInfo.UserId;
            }
            entity.CreatorTime = DateTime.Now;
        }
        public void Modify(string keyValue)
        {
            var entity = this as IModificationAudited;
            entity.ID = keyValue;
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            if (LoginInfo != null)
            {
                entity.LastModifyUserId = LoginInfo.UserId;
            }
            entity.LastModifyTime = DateTime.Now;

        }
        public void Remove()
        {
            var entity = this as IDeleteAudited;
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            if (LoginInfo != null)
            {
                entity.DeleteUserId = LoginInfo.UserId;
            }
            entity.DeleteTime = DateTime.Now;
            entity.DeleteMark = true;
        }
    }
}
