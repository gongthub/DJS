﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.DAL.Redis
{
    public class Jobs : IDAL.IJobs
    {
        public List<Model.Jobs> GetModels()
        {
            throw new NotImplementedException();
        }

        public bool IsExist(string name)
        {
            throw new NotImplementedException();
        }

        public List<Model.Jobs> GetAllList()
        {
            throw new NotImplementedException();
        }

        public List<Model.Jobs> GetList()
        {
            throw new NotImplementedException();
        }

        public List<Model.Jobs> GetList(Common.Pagination pagination)
        {
            throw new NotImplementedException();
        }

        public List<Model.Jobs> GetList(Common.Pagination pagination, string keyword)
        {
            throw new NotImplementedException();
        }

        public Model.Jobs GetForm(string keyValue)
        {
            throw new NotImplementedException();
        }

        public bool DeleteForm(string keyValue)
        {
            throw new NotImplementedException();
        }

        public bool AddForm(Model.Jobs moduleEntity)
        {
            throw new NotImplementedException();
        }

        public bool UpdateForm(Model.Jobs moduleEntity)
        {
            throw new NotImplementedException();
        }
    }
}
