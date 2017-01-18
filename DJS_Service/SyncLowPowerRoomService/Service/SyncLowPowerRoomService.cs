using Newtonsoft.Json;
using SyncLowPowerRoomService.Model;
using SyncLowPowerRoomService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using Vlinker.Common.ApiInvoker;

namespace SyncLowPowerRoomService.Service
{
    public class SyncLowPowerRoomService
    {
        private string apiBaseAddress = ConstUtility.WebApiUrl;
        private string partnerId = ConstUtility.PartnerId;
        private string electric = ConstUtility.Electric;
        private int systemType = ConstUtility.SystemType;
        private int electricWarn = ConstUtility.ElectricWarn;

        private IDBRepository db = new IDBRepository();
        public static DJS.SDK.ILog iLog = null;

        #region 构造函数
        static SyncLowPowerRoomService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
        }

        #endregion


        private void InsertTable(string resurtData, string storeCode)
        {
            var result = JsonConvert.DeserializeObject<Result>(resurtData);
            if (result.data != null)
            {
                iLog.WriteLog("编码为：" + storeCode + "的门店插入低电量表中开始...", 0);
                var lowPowerRoomList = JsonConvert.DeserializeObject<List<ResultLowPowerRoom>>(result.data.ToString());
                if (lowPowerRoomList != null)
                {
                    foreach (var list in lowPowerRoomList)
                    {
                        var lowPowerRoom = db.LowPowerRooms.Where(t => t.StoreCode == list.building_id && t.RoomName == list.door_name && t.SystemType == systemType).FirstOrDefault();
                        if (lowPowerRoom != null)
                        {
                            int elecOld = !string.IsNullOrEmpty(lowPowerRoom.Electric.ToString()) ? Convert.ToInt32(lowPowerRoom.Electric) : 0;
                            int elecNew = !string.IsNullOrEmpty(list.electric) ? Convert.ToInt32(list.electric) : 0;
                            //下降在25以内,和上升都要更新，其余不更新
                            if (elecOld - elecNew < electricWarn || elecNew > elecOld)
                            {
                                lowPowerRoom.Electric = !string.IsNullOrEmpty(list.electric) ? Convert.ToInt32(list.electric) : 0;
                                lowPowerRoom.CreateTime = DateTime.Now.Date;
                            }
                        }
                        else
                        {
                            var lowPowerRoomT = new LowPowerRoom();
                            lowPowerRoomT.StoreCode = list.building_id;
                            lowPowerRoomT.RoomName = list.door_name;
                            lowPowerRoomT.CreateTime = DateTime.Now.Date;
                            lowPowerRoomT.Electric = !string.IsNullOrEmpty(list.electric) ? Convert.ToInt32(list.electric) : 0;
                            lowPowerRoomT.SystemType = systemType;
                            db.LowPowerRooms.Add(lowPowerRoomT);
                        }
                        db.SaveChanges();
                    }
                }
                iLog.WriteLog("编码为：" + storeCode + "的门店插入低电量表中结束...", 0);
            }
            else
            {
                iLog.WriteLog("编码为：" + storeCode + "的门店没有智能门锁房间", 0);
            }
        }

        /// <summary>
        /// 获取低电量房源
        /// </summary>        
        /// <returns></returns>
        public void GetLowPowerRooms()
        {
            string resurtData = null;
            var storeList = db.Stores.Where(t => t.Status != 1 && t.Status != 3).ToList();
            //string api = string.Format(@"https://alms.yeeuu.com/count/get_low_battery");
            string api = string.Format(@"/DoorLock/GetLowPowerRooms");

            storeList.ForEach(t =>
            {
                var josn = new
                {
                    SupplierCode = "Yunyou",
                    StoreCode = t.StoreCode,  //小区编码
                    //buildingNo = 1,            //楼栋名称 暂时不传
                    electric = electric        //电量
                };
                string strJson = JsonConvert.SerializeObject(josn);

                var requestParam = new Dictionary<string, string>
                {
                    { "jsonData", strJson },
                };

                resurtData =  WebApiInvokeGet(api, requestParam);

                //将获取到的低电量房源插入到表中
                InsertTable(resurtData, t.StoreCode);
            });
        }

        private string WebApiInvokeGet(string api, Dictionary<string, string> requestParam)
        {
            string result = "";

            try
            {
                try
                {
                    WebApiInvoker ApiInvoker = new WebApiInvoker(apiBaseAddress);
                    result = ApiInvoker.InvokeGetRequestForWindows(api, requestParam).Result;
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
