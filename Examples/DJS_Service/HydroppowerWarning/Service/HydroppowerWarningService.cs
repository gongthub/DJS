using HydroppowerWarning.Model;
using HydroppowerWarning.Services;
using HydroppowerWarning.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Vlinker.Common.ApiInvoker;

namespace HydroppowerWarning.Service
{
    public class HydroppowerWarningService
    {
        private string apiBaseAddress = ConstUtility.WebApiUrl;

        private string HyUserName = ConstUtility.HyUserName;

        public static DJS.SDK.ILog iLog = null;

        //电余额阈值
        private static decimal EleWarning = 0.00M;
        //冷水余额阈值
        private static decimal ClodWarning = 0.00M;
        //热水余额阈值
        private static decimal HotWarning = 0.00M;



        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary> 

        #endregion

        #region 构造函数

        static HydroppowerWarningService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();

            Decimal.TryParse(ConstUtility.Ele, out EleWarning);
            Decimal.TryParse(ConstUtility.Clod, out ClodWarning);
            Decimal.TryParse(ConstUtility.Hot, out HotWarning);
        }

        #endregion

        /// <summary>
        /// 处理方法
        /// </summary>
        public void DoWork()
        {
            string times = DateTime.Now.ToString("HH:mm");
            iLog.WriteLog("水电表余额预警提醒开始", 0);

            try
            {
                DateTime StartDate = DateTime.Now;
                DateTime EndDate = DateTime.Now;
                string strs = GetAllBalances(StartDate, EndDate);
                Result result = Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(strs);
                if (result.success)
                {
                    List<Model.BalanceList> models = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Model.BalanceList>>(result.desc);
                    if (models != null && models.Count > 0)
                    {

                        int storeType = -1;
                        if (Int32.TryParse(ConstUtility.AmsType, out storeType))
                        {
                            //大V
                            if (storeType == 0)
                            {
                                List<UserPhoneList> userList = GetBigVUserList();
                                DoSendMsg(userList, models, storeType);
                            }
                            //小V
                            else if (storeType == 1)
                            {
                                List<UserPhoneList> userList = GetSmallVUserList();
                                DoSendMsg(userList, models, storeType);
                            }
                            else
                            {
                                iLog.WriteLog("水电表余额预警异常，AmsType只能为0：大V 1：小V", 1);
                            }
                        }
                        else
                        {
                            iLog.WriteLog("水电表余额预警异常，AmsType只能为0：大V 1：小V", 1);
                        }
                    }
                }
                else
                {
                    iLog.WriteLog("水电表余额预警异常，读取最新余额列表失败", 1);
                }
            }
            catch
            {
                throw;
            }

            iLog.WriteLog("水电表余额预警提醒结束", 0);
        }

        /// <summary>
        /// 获取大V用户信息
        /// </summary>
        /// <returns></returns>
        private List<UserPhoneList> GetBigVUserList()
        {
            List<UserPhoneList> models = new List<UserPhoneList>();
            string sqls = @"
                            select ROW_NUMBER() over (order by StoreCode,RoomName) ID,RoomName,StoreCode,StoreName,ContractNo,UserName,UserPhone from (
	                            select distinct RoomName,StoreCode,StoreName,ContractNo,
	                            case when LEN(CheckInName)>0 and LEN(CheckInPhone)>0 then CheckInName
	                            else RenterName end UserName,
	                            case when LEN(CheckInName)>0 and LEN(CheckInPhone)>0 then CheckInPhone
	                            else RenterPhone end UserPhone 
	                            from ( 
		                            select distinct RoomID,FullName RoomName,StoreCode,StoreName,ContractNo,RenterName,RenterPhone,CheckInName,CheckInPhone from (
		                            --非大合同  开始
		                            select distinct A.ID RoomID,C.StoreCode,C.Name StoreName,B.ContractNo,A.FullName
		                            ,D.Name RenterName,D.Phone RenterPhone,E.Name CheckInName,E.Phone CheckInPhone
		                             from Rooms A
		                            left join Contracts B on A.ID=B.RoomID and B.Status=3
		                            left join Stores C on A.StoreID=C.ID
		                            left join Renters D on B.RenterID=D.ID
		                            left join CheckIns E on A.ID=E.RoomID and E.Status=1 and E.CheckInType=1
		                            and E.ID in (
			                            select distinct B.CheckInID from HyReChargeLogs A
			                            left join Charges B on A.ChargeID=B.ID 
			                            where B.CheckInID>0
		                            )

		                            where LEN(B.ContractNo)>0
		                            --and LEN(E.Name)>0
		                            and A.ID not in (
			                            select A.ID from Rooms A
			                            left join Contracts B on A.ID=B.RoomID
			                            where A.ID in (
				                            select ProroomId from ContractForRooms
				                            where Status=0
			                            )
		                            )
		                             --非大合同  结束
		                             union all 
		 
		                             --大合同  开始 
		                            select distinct B.RoomID,A.StoreCode,A.StoreName,A.ContractNo,C.FullName RoomName,A.RenterName
		                            ,A.RenterPhone,D.Name CheckInName,D.Phone CheckInPhone from (
			                            select distinct C.StoreCode,C.Name StoreName,A.ID RoomID,B.ContractNo,A.FullName
			                            ,D.Name RenterName,D.Phone RenterPhone
			                             from Rooms A
			                            left join Contracts B on A.ID=B.RoomID and B.Status=3
			                            left join Stores C on A.StoreID=C.ID
			                            left join Renters D on B.RenterID=D.ID 

			                            where LEN(B.ContractNo)>0 
			                            and A.ID in (
				                            select A.ID from Rooms A
				                            left join Contracts B on A.ID=B.RoomID
				                            where A.ID in (
					                            select ProroomId from ContractForRooms
					                            where Status=0
				                            )
			                            )
		                            ) A
		                            left join ContractForRooms B on A.RoomID =B.ProRoomID and B.Status=0
		                            left join Rooms C on B.RoomID=C.ID
		                            left join CheckIns D on C.ID=D.RoomID and D.Status=1 and D.CheckInType=1
		                            and D.ID in (
			                            select distinct B.CheckInID from HyReChargeLogs A
			                            left join Charges B on A.ChargeID=B.ID 
			                            where B.CheckInID>0
		                            )

		                             --大合同  结束
		                             ) A
		                             where LEN(ContractNo) > 0 and ( LEN(RenterPhone) > 0 or LEN(CheckInPhone) > 0)
	                            ) A
                             ) A";

            IDBRepository db = new IDBRepository();
            models = db.Database.SqlQuery<UserPhoneList>(sqls).ToList();

            return models;
        }

        /// <summary>
        /// 获取小V用户信息
        /// </summary>
        /// <returns></returns>
        private List<UserPhoneList> GetSmallVUserList()
        {
            List<UserPhoneList> models = new List<UserPhoneList>();

            string sqls = @"
                        select ROW_NUMBER() over (order by StoreCode,RoomName) ID,RoomName,StoreCode,StoreName,ContractNo,UserName,UserPhone from (
	                        select  distinct RoomName,StoreCode,StoreName,ContractNo,
	                        case when LEN(CheckInName)>0 and LEN(CheckInPhone)>0 then CheckInName
	                        else RenterName end UserName,
	                        case when LEN(CheckInName)>0 and LEN(CheckInPhone)>0 then CheckInPhone
	                        else RenterPhone end UserPhone 
	                        from ( 
		                        select distinct RoomID,FullName RoomName,StoreCode,StoreName,ContractNo,RenterName,RenterPhone,CheckInName,CheckInPhone  from (
			                        select distinct B.ID RoomID,D.StoreCode,D.Name StoreName,C.ContractNo,
			                        case when charindex('-',A.FullName) >0 then SUBSTRING(A.FullName,1,charindex('-',A.FullName)-1)
			                        else A.FullName end FullName,E.Name RenterName,E.Phone RenterPhone,F.Name CheckInName,F.Phone CheckInPhone
			                        from ContractForRooms A
			                        left join Rooms B on A.RoomID=B.ID and A.Status=0
			                        left join Contracts C on A.ProRoomID=C.RoomID and C.Status=3
			                        left join Stores D on B.StoreID=D.ID
			                        left join Renters E on C.RenterID=E.ID
			                        left join CheckIns F on B.ID=F.RoomID and F.Status=1 and F.CheckInType=1
			                        and F.ID in (
				                        select distinct B.CheckInID from HyReChargeLogs A
				                        left join Charges B on A.ChargeID=B.ID 
				                        where B.CheckInID>0
			                        )
			                        where B.ID>0 and C.ID>0
		                        ) A 
		                         where LEN(ContractNo) > 0 and ( LEN(RenterPhone) > 0 or LEN(CheckInPhone) > 0)
	                         ) A
                         ) A";

            IDBRepository db = new IDBRepository();
            models = db.Database.SqlQuery<UserPhoneList>(sqls).ToList();

            return models;
        }

        /// <summary>
        /// 获取表类型名称
        /// </summary>
        /// <param name="meterType"></param>
        /// <returns></returns>
        private string GetMeterTypeName(int meterType)
        {
            string name = "";
            switch (meterType)
            {
                case 2:
                    name = "电表";
                    break;
                case 40:
                    name = "电表";
                    break;
                case 3:
                    name = "冷水表";
                    break;
                case 10:
                    name = "冷水表";
                    break;
                case 4:
                    name = "热水表";
                    break;
                case 11:
                    name = "热水表";
                    break;
            }
            return name;
        }

        /// <summary>
        /// 处理发送短信方法
        /// </summary>
        /// <param name="userList"></param>
        /// <param name="models"></param>
        /// <param name="storeType"></param>
        private void DoSendMsg(List<UserPhoneList> userList, List<Model.BalanceList> models, int storeType)
        {
            if (userList != null && userList.Count > 0)
            {
                foreach (var user in userList)
                {
                    //处理电费
                    List<Model.BalanceList> modelsTD = models.Where(m => m.community_no == user.StoreCode && m.user_address_room == user.RoomName && m.balance <= EleWarning && (m.meter_type == 40 || m.meter_type == 2)).ToList();
                    if (modelsTD != null && modelsTD.Count > 0)
                    {
                        foreach (var modelTD in modelsTD)
                        {
                            string meterTypeName = GetMeterTypeName(modelTD.meter_type);

                            string strs = SendHydropowerWarning(storeType, user.UserName, user.UserPhone, user.RoomName, meterTypeName, modelTD.balance.ToString());
                            Result result = Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(strs);
                            if (result.success)
                            {
                                iLog.WriteLog("短信发送成功！门店：" + user.StoreName + "；房间：" + user.RoomName + "；表类型：" + meterTypeName + "；租户：" + user.UserName + "；手机号：" + user.UserPhone + "；余额：" + modelTD.balance.ToString(), 0);
                            }
                            else
                            {
                                iLog.WriteLog("短信发送失败！门店：" + user.StoreName + "；房间：" + user.RoomName + "；表类型：" + meterTypeName + "；租户：" + user.UserName + "；手机号：" + user.UserPhone + "；余额：" + modelTD.balance.ToString(), 0);
                            }
                        }
                    }
                    //处理冷水费
                    List<Model.BalanceList> modelsTC = models.Where(m => m.community_no == user.StoreCode && m.user_address_room == user.RoomName && m.balance <= ClodWarning && (m.meter_type == 10 || m.meter_type == 3)).ToList();
                    if (modelsTC != null && modelsTC.Count > 0)
                    {
                        foreach (var modelTC in modelsTC)
                        {
                            string meterTypeName = GetMeterTypeName(modelTC.meter_type);
                            string strs = SendHydropowerWarning(storeType, user.UserName, user.UserPhone, user.RoomName, meterTypeName, modelTC.balance.ToString());
                            Result result = Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(strs);
                            if (result.success)
                            {
                                iLog.WriteLog("短信发送成功！门店：" + user.StoreName + "；房间：" + user.RoomName + "；表类型：" + meterTypeName + "；租户：" + user.UserName + "；手机号：" + user.UserPhone + "；余额：" + modelTC.balance.ToString(), 0);
                            }
                            else
                            {
                                iLog.WriteLog("短信发送失败！门店：" + user.StoreName + "；房间：" + user.RoomName + "；表类型：" + meterTypeName + "；租户：" + user.UserName + "；手机号：" + user.UserPhone + "；余额：" + modelTC.balance.ToString(), 0);
                            }
                        }
                    }
                    //处理热水费
                    List<Model.BalanceList> modelsTH = models.Where(m => m.community_no == user.StoreCode && m.user_address_room == user.RoomName && m.balance <= HotWarning && (m.meter_type == 11 || m.meter_type == 4)).ToList();
                    if (modelsTH != null && modelsTH.Count > 0)
                    {
                        foreach (var modelTH in modelsTH)
                        {
                            string meterTypeName = GetMeterTypeName(modelTH.meter_type);
                            string strs = SendHydropowerWarning(storeType, user.UserName, user.UserPhone, user.RoomName, meterTypeName, modelTH.balance.ToString());
                            Result result = Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(strs);
                            if (result.success)
                            {
                                iLog.WriteLog("短信发送成功！门店：" + user.StoreName + "；房间：" + user.RoomName + "；表类型：" + meterTypeName + "；租户：" + user.UserName + "；手机号：" + user.UserPhone + "；余额：" + modelTH.balance.ToString(), 0);
                            }
                            else
                            {
                                iLog.WriteLog("短信发送失败！门店：" + user.StoreName + "；房间：" + user.RoomName + "；表类型：" + meterTypeName + "；租户：" + user.UserName + "；手机号：" + user.UserPhone + "；余额：" + modelTH.balance.ToString(), 0);
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 根据开始时间和结束时候获取所有表最新余额
        /// </summary>
        /// <param name="meterNo"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public string GetAllBalances(DateTime startDate, DateTime endDate)
        {
            string api = string.Format(@"/Hydropower/GetAllBalances");

            string strJson = @"{'SupplierCode':'" + HyUserName + "','StartTime':'" + startDate.ToString("yyyy-MM-dd") + "','EndTime':'" + endDate.ToString("yyyy-MM-dd") + "'}";

            var requestParam = new Dictionary<string, string>
                {
                    { "jsonData", strJson },
                };

            return WebApiInvokePost(api, requestParam);
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="StoreType">门店类型</param>
        /// <param name="UserName">姓名</param>
        /// <param name="Phone">手机号</param>
        /// <param name="meterType">表类型</param>
        /// <param name="balances">余额</param>
        /// <returns></returns>
        public string SendHydropowerWarning(int StoreType, string UserName, string Phone, string RoomName, string meterType, string balances)
        {
            string api = string.Format(@"/SMS/SendHydropowerWarning");

            string strJson = @"{'StoreType':" + StoreType + ",'UserName':'" + UserName + "','Phone':'" + Phone + "','RoomName':'" + RoomName + "','meterType':'" + meterType + "','balances':'" + balances + "'}";

            var requestParam = new Dictionary<string, string>
                {
                    { "jsonData", strJson },
                };

            return WebApiInvokePost(api, requestParam);
        }

        private string WebApiInvokePost(string api, Dictionary<string, string> requestParam)
        {
            string result = "";

            try
            {
                try
                {
                    WebApiInvoker ApiInvoker = new WebApiInvoker(apiBaseAddress);
                    result = ApiInvoker.InvokePostRequestNoToken(api, requestParam).Result;
                }
                catch (HttpResponseException re)
                {
                    if (re.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        result = "{\"success\":false,\"desc\":\"无权限访问\"}";
                    }
                    else
                    {
                        result = "{\"success\":false,\"desc\":\"" + re.Response.Content.ReadAsStringAsync().Result + "\"}";
                    }
                }
            }
            catch (Exception e)
            {
                result = "{\"success\":false,\"desc\":\"" + e.Message + "\"}";
            }

            return result;
        }
    }
}
