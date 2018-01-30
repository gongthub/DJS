using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DJS.BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Diagnostics;
namespace DJS.BLL.Tests
{
    [TestClass()]
    public class UserMgrTests
    {
        [TestMethod()]
        public void AddFormTest()
        {
            Mu();
            //Sg();
        }

        private void Mu()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        Model.UserEntity userModel = new Model.UserEntity();
                        userModel.ID = Guid.NewGuid().ToString();
                        userModel.Account = "测试";
                        userModel.SortCode = 1;
                        bool b1 = UserMgr.AddForm(userModel);

                        Debug.WriteLine("b1-add：" + b1);
                        userModel.Account = "测试" + 1;
                        userModel.SortCode = 1;
                        b1 = UserMgr.UpdateForm(userModel);
                        Debug.WriteLine("b1-update：" + b1);
                        //b1 = UserMgrT.DeleteForm(userModel.ID);
                        //Debug.WriteLine("b1-remove：" + b1);


                        Model.ModuleEntity moduleEntity = new Model.ModuleEntity();
                        moduleEntity.ID = Guid.NewGuid().ToString();
                        moduleEntity.FullName = "模块";
                        moduleEntity.SortCode = 1;
                        bool b2 = ModuleMgr.AddForm(moduleEntity);

                        Debug.WriteLine("b2-add：" + b2);
                        moduleEntity.FullName = "模块" + 1;
                        moduleEntity.SortCode = 1;
                        b2 = ModuleMgr.UpdateForm(moduleEntity);
                        Debug.WriteLine("b2-update：" + b2);
                        //b1 = ModuleMgrT.DeleteForm(moduleEntity.ID);
                        //Debug.WriteLine("b2-remove：" + b2);
                        List<Model.UserEntity> userModels = BLL.UserMgr.GetAllList();
                        List<Model.ModuleEntity> moduleModels = BLL.ModuleMgr.GetAllList();

                        Model.UserEntity userModelT = BLL.UserMgr.GetForm(userModel.ID);
                        Model.ModuleEntity moduleModelT = BLL.ModuleMgr.GetForm(moduleEntity.ID);


                        Debug.WriteLine("userModel.ID-" + userModel.ID + "：" + userModelT.ID);
                        Debug.WriteLine("moduleEntity.ID-" + moduleEntity.ID + "：" + moduleModelT.ID);


                    }
                    catch (Exception e)
                    {
                        if (e != null)
                            Debug.WriteLine("HH" + e.Message);
                        //Assert.Fail();
                    }
                });
                thread.Start();

            }

            Thread.Sleep(10000);
        }
        private void Sg()
        {
            //for (int i = 0; i < 10; i++)
            //{
            try
            {
                Model.UserEntity userModel = new Model.UserEntity();
                userModel.ID = Guid.NewGuid().ToString();
                userModel.Account = "测试";
                userModel.SortCode = 1;
                bool b1 = UserMgr.AddForm(userModel);

                Debug.WriteLine("b1-add：" + b1);
                userModel.Account = "测试" + 1;
                userModel.SortCode = 1;
                b1 = UserMgr.UpdateForm(userModel);
                Debug.WriteLine("b1-update：" + b1);
                //b1 = UserMgrT.DeleteForm(userModel.ID);
                //Debug.WriteLine("b1-remove：" + b1);


                Model.ModuleEntity moduleEntity = new Model.ModuleEntity();
                moduleEntity.ID = Guid.NewGuid().ToString();
                moduleEntity.FullName = "模块";
                moduleEntity.SortCode = 1;
                bool b2 = ModuleMgr.AddForm(moduleEntity);

                Debug.WriteLine("b2-add：" + b2);
                moduleEntity.FullName = "模块" + 1;
                moduleEntity.SortCode = 1;
                b2 = ModuleMgr.UpdateForm(moduleEntity);
                Debug.WriteLine("b2-update：" + b2);
                //b1 = ModuleMgrT.DeleteForm(moduleEntity.ID);
                //Debug.WriteLine("b2-remove：" + b2);

                List<Model.UserEntity> userModels = BLL.UserMgr.GetAllList();
                List<Model.ModuleEntity> moduleModels = BLL.ModuleMgr.GetAllList();

                Model.UserEntity userModelT = BLL.UserMgr.GetForm(userModel.ID);
                Model.ModuleEntity moduleModelT = BLL.ModuleMgr.GetForm(moduleEntity.ID);

            }
            catch (Exception e)
            {
                if (e != null)
                    Debug.WriteLine("HH" + e.Message);
                //Assert.Fail();
            }
            //}

        }
    }
}
