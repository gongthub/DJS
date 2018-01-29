using DJS.Common;
using DJS.Model;
using DJS.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DJS.WebApp.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public virtual ActionResult Index()
        {
            var test = string.Format("{0:E2}", 1);
            return View();
        }
        [HttpGet]
        public ActionResult GetAuthCode()
        {
            return File(new VerifyCode().GetVerifyCode(), @"image/Gif");
        }
        [HttpGet]
        public ActionResult OutLogin()
        {
            //new LogApp().WriteDbLog(new LogEntity
            //{
            //    ModuleName = "系统登录",
            //    Type = DbLogType.Exit.ToString(),
            //    Account = OperatorProvider.Provider.GetCurrent().UserCode,
            //    NickName = OperatorProvider.Provider.GetCurrent().UserName,
            //    Result = true,
            //    Description = "安全退出系统",
            //});
            Session.Abandon();
            Session.Clear();
            OperatorProvider.Provider.RemoveCurrent();
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult CheckLogin(string username, string password, string code)
        {
            //LogEntity logEntity = new LogEntity();
            //logEntity.ModuleName = "系统登录";
            //logEntity.Type = DbLogType.Login.ToString();
            try
            {
                if (Session["djs_session_verifycode"].IsEmpty() || SecurityHelp.md5(code.ToLower(), 16) != Session["djs_session_verifycode"].ToString())
                {
                    throw new Exception("验证码错误，请重新输入");
                }

                UserEntity userEntity = new UserEntity();
                if (BLL.UserMgr.CheckLogin(username, password, ref userEntity))
                {
                    if (userEntity != null)
                    {
                        OperatorModel operatorModel = new OperatorModel();
                        operatorModel.UserId = userEntity.ID;
                        operatorModel.UserCode = userEntity.Account;
                        operatorModel.UserName = userEntity.RealName;
                        operatorModel.LoginIPAddress = NetHelp.Ip;
                        operatorModel.LoginTime = DateTime.Now;
                        operatorModel.LoginToken = SecurityHelp.Encrypt(Guid.NewGuid().ToString());
                        if (userEntity.Account == "admin")
                        {
                            operatorModel.IsSystem = true;
                        }
                        else
                        {
                            operatorModel.IsSystem = false;
                        }
                        OperatorProvider.Provider.AddCurrent(operatorModel);
                        //logEntity.Account = userEntity.Account;
                        //logEntity.NickName = userEntity.RealName;
                        //logEntity.Result = true;
                        //logEntity.Description = "登录成功";
                        //new LogApp().WriteDbLog(logEntity);
                    }
                    return Content(new AjaxResult { state = ResultType.success.ToString(), message = "登录成功。" }.ToJson());
                }
                else
                {
                    return Content(new AjaxResult { state = ResultType.error.ToString(), message = "用户名或密码错误" }.ToJson());
                }
            }
            catch (Exception ex)
            {
                //logEntity.Account = username;
                //logEntity.NickName = username;
                //logEntity.Result = false;
                //logEntity.Description = "登录失败，" + ex.Message;
                //new LogApp().WriteDbLog(logEntity);
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }
    }
}
