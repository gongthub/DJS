using DailyRentApportionService.Models;
using DailyRentApportionService.Utils;
using DBHelper.MSSQLSERVER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DailyRentApportionService.Services
{
    public class RentDailyApportionService
    {

        public static DJS.SDK.ILog iLog = null;
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary> 

        #endregion

        #region 构造函数

        static RentDailyApportionService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
        }

        #endregion
 
        /// <summary>
        /// 租金贷结算服务
        /// </summary>
        public void ApportionService()
        {
            iLog.WriteLog("日租金分摊服务运行开始", 0);
            ApportionAction2();
            iLog.WriteLog("日租金分摊服务运行结束", 0);


            iLog.WriteLog("月租金分摊服务运行开始", 0);
            ApportionActions2();
            iLog.WriteLog("月租金分摊服务运行结束", 0);
        }

        /// <summary>
        /// 分摊日租金
        /// </summary>
        public void ApportionAction2()
        {
            try
            {
                iLog.WriteLog("日租金分摊开始", 0);
                TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                int totalCount = 0;

                List<string> deleteList = new List<string>();

                // 重算前几天的数据
                string deleteSQL = string.Format(@"DELETE  FROM DailyRentApportionLog where Date >= '{0}'", DateTime.Now.AddDays(ConstUtility.RE_CALCULATION_DAYS * -1));
                deleteList.Add(deleteSQL);
                deleteSQL = string.Format(@"DELETE  FROM DailyRentApportion where UpdateDate  >= '{0}'", DateTime.Now.AddDays(ConstUtility.RE_CALCULATION_DAYS * -1));
                deleteList.Add(deleteSQL);
                SQLHelper.ExecuteNonQuery(ConstUtility.ConnectionStrings, deleteList);

                string strSQL = string.Format(@"SELECT MAX(Date) FROM DailyRentApportionLog");
                DataTable dt = SQLHelper.ExecuteDataset(ConstUtility.ConnectionStrings, CommandType.Text, strSQL).Tables[0];
                DateTime startDate = ConstUtility.EXECUTE_STARTDATE.AddDays(-1);

                if (dt.Rows[0][0] != null && dt.Rows[0][0].ToString() != "")
                {
                    startDate = DateTime.Parse(dt.Rows[0][0].ToString());
                }

                TimeSpan ts1 = new TimeSpan(startDate.Ticks);
                TimeSpan ts3 = ts2.Subtract(ts1).Duration();
                int iCount = ts3.Days;

                string strDelete = string.Format(@"delete from DailyRentApportion where ContractID IN (
	                    select ct.ID from Contracts ct
		                    left join (select ContractID,SUM(Amount) Amount,
		                    MIN(StartDate) as StartDate,MAX(EndDate) EndDate From (
		                    select ContractID,Amount Amount,StartDate,
			                    case when Amount < 0 then StartDate else  EndDate end as EndDate
		                    from PeriodicCharges pc 
		                    inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID  
			                    where pc.status = 1 and pm.Status IN(2,3,4)  ) T group by ContractID) P on ct.ID = p.ContractID
	                    where ct.Status = 4	 and  p.EndDate >= '{0}' and p.EndDate < '{1}' and ct.type = 1 )
                    ", startDate.AddDays(1), DateTime.Now.Date.AddDays(1));

                List<string> strList = new List<string>();
                strList.Add(strDelete);

                strDelete = string.Format(@"delete from DailyRentApportion where ContractID IN (
                                    select pc.ContractID from PaymentMethods pm 
	                                    inner join PeriodicCharges pc on pm.ID = pc.PaymentMethodID
                                        inner join Contracts ct on ct.id= pc.ContractID 
                                    where isnull(pm.Date,pm.CreateDate) >= '{0}' and isnull(pm.Date,pm.CreateDate) < '{1}'
	                                    and pc.Status = 1 and pm.Status IN (2,3,4) AND pc.FeeType= 31 and ct.type <> 1 ) 
                    ", startDate.AddDays(1), DateTime.Now.Date.AddDays(1));

                strList.Add(strDelete);

                SQLHelper.ExecuteNonQuery(ConstUtility.ConnectionStrings, strList);

                strSQL = string.Format(@"
                        select ct.StartDate,ct.EndDate,cf.CharterMoney,cf.ContractID,pc.Amount as TotalAmount,
                                ct.RoomID,pc.ID as PeriodicChargeID,isnull(pm.Date,pm.CreateDate) Date,LastPayDate,pc.FeeType FeeType from PeriodicCharges pc 
                            inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID
                            inner join Contracts ct on pc.ContractID = ct.ID
                            inner join ContractFees cf on pc.ContractID = cf.ContractID
                            inner join (
                                select ContractID,MAX(isnull(pm.Date,pm.CreateDate)) as LastPayDate from PaymentMethods pm 
	                                inner join PeriodicCharges pc on pm.ID = pc.PaymentMethodID
                                where isnull(pm.Date,pm.CreateDate) >= '{0}' and isnull(pm.Date,pm.CreateDate) < '{1}'
	                                and pc.Status = 1 and pm.Status IN (2,3,4) group by ContractID
                                ) T on ct.ID = T.ContractID
                        where  pc.Status = 1 and pm.Status IN (2,3,4) AND ct.type != 1 and isnull(pm.Date,pm.CreateDate) < '{1}'
                        union all
                        select ct.StartDate,ct.EndDate,ra.MonthlyTotalAmount,ct.ID,ra.TotalAmount,ct.RoomID,null,DateOfLoan,DateOfLoan,31 FeeType from RentLoanAudits ra 
	                        inner join Contracts ct on ra.RenterID = ct.RenterID
                        where  DateOfLoan >= '{0}' and DateOfLoan < '{1}' and ct.type = 1 and ct.Status = 3
                        union all
                        select ct.StartDate,ct.EndDate,cf.CharterMoney,ct.ID,isnull(Amount,0), ct.RoomID,PeriodicChargeID,p.EndDate,p.EndDate,FeeType from Contracts ct inner join (
	                        select ContractID,SUM(Amount) Amount,
		                        MIN(StartDate) as StartDate,MAX(EndDate) EndDate,PeriodicChargeID,T.FeeType 
	                        From (
		                        select ContractID,Amount Amount,StartDate,
			                        case when Amount < 0 then StartDate else  EndDate end as EndDate,pc.ID as PeriodicChargeID,pc.FeeType FeeType
		                        from PeriodicCharges pc 
		                        inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID  
		                        where pc.status = 1 and pm.Status IN(2,3,4)  ) T 
	                        group by ContractID,PeriodicChargeID,FeeType ) p
		                        on ct.ID = p.ContractID
	                        inner join ContractFees cf on ct.id = cf.ContractID
                        where ct.ID IN (
	                    select ct.ID from Contracts ct
		                    left join (select ContractID,SUM(Amount) Amount,
		                    MIN(StartDate) as StartDate,MAX(EndDate) EndDate From (
		                    select ContractID,Amount Amount,StartDate,
			                    case when Amount < 0 then StartDate else  EndDate end as EndDate
		                    from PeriodicCharges pc 
		                    inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID  
			                    where pc.status = 1 and pm.Status IN(2,3,4)  ) T group by ContractID) P on ct.ID = p.ContractID
	                    where ct.Status = 4	 and  p.EndDate >= '{0}' and p.EndDate < '{1}' and ct.type = 1 )                 
                    ", startDate.AddDays(1), DateTime.Now.Date.AddDays(1));

                DataTable allPaymentDt = SQLHelper.ExecuteDataset(ConstUtility.ConnectionStrings, CommandType.Text, strSQL).Tables[0];

                //allPaymentDt = allPaymentDt.Select(" ContractID=28722").CopyToDataTable();

                for (int i = 1; i <= iCount; i++)
                {
                    DateTime actionDate = startDate.AddDays(i).Date;

                    List<string> sqlList = new List<string>();
                    int daySumCount = 0;
                    TimeSpan ts4 = new TimeSpan(DateTime.Now.Ticks);

                    iLog.WriteLog("日期:" + actionDate + "  开始分摊", 0);

                    DataRow[] allPaymentDtRows = allPaymentDt.Select("LastPayDate >= '" + actionDate + "' AND LastPayDate < '" + actionDate.AddDays(1) + "'");
                    if (allPaymentDtRows.Length == 0)
                    {
                        iLog.WriteLog("日期:" + actionDate + "  结束分摊，记录:" + daySumCount + " 耗时:" + (new TimeSpan(DateTime.Now.Ticks)).Subtract(ts4).Duration().TotalSeconds + "秒", 0);
                        continue;
                    }

                    DataTable paymentDt = allPaymentDtRows.CopyToDataTable();

                    DataTable dtContract = paymentDt.DefaultView.ToTable(true, "ContractID");
                    List<MonthRentApportion> AllRentList = new List<MonthRentApportion>();

                    iLog.WriteLog("查询分摊当日:" + actionDate + " 所有租金支付数据，条数:" + paymentDt.Rows.Count, 0);

                    for (int k = 0; k < dtContract.Rows.Count; k++)
                    {
                        int ContractID = int.Parse(dtContract.Rows[k][0].ToString());
                        DataRow[] rows = paymentDt.Select("ContractID = " + ContractID);

                        List<MonthRentApportion> ContractRentList = new List<MonthRentApportion>();
                        sqlList = new List<string>();

                        int RoomID = int.Parse(rows[0]["RoomID"].ToString());//房间ID
                        decimal iCharterMoney = decimal.Parse(rows[0]["CharterMoney"].ToString());//标准月租金
                        DateTime cStartDate = DateTime.Parse(rows[0]["StartDate"].ToString());//合同开始日
                        DateTime cEndDate = DateTime.Parse(rows[0]["EndDate"].ToString());//合同结束日              

                        //decimal dayRent = GetDayRent(cStartDate, cEndDate, iCharterMoney);
                        //New日租金
                        decimal dayRent = GetUnitPrice(cStartDate, cEndDate, iCharterMoney);
                        for (int j = 0; j < rows.Length; j++)
                        {
                            DateTime PaymentDate = DateTime.Parse(rows[j]["Date"].ToString());
                            int PeriodicChargeID = rows[j]["PeriodicChargeID"] == null || rows[j]["PeriodicChargeID"].ToString() == "" ? 0 : int.Parse(rows[j]["PeriodicChargeID"].ToString());
                            decimal TotalAmount = decimal.Parse(rows[j]["TotalAmount"].ToString());

                            int type = 0;
                            int.TryParse(paymentDt.Rows[0]["FeeType"].ToString(), out type);

                            int iDirection = TotalAmount > 0 ? 1 : -1;

                            MonthRentApportion lastItem = ContractRentList.LastOrDefault();

                            decimal lastTotalAmount = 0;

                            if (lastItem != null)
                            {
                                lastTotalAmount = ContractRentList.Where(p => p.Date == lastItem.Date).Sum(t => t.Amount);
                            }

                            DateTime LastDate = lastItem == null ? cStartDate : lastItem.Date;
                            decimal lastAmount = lastItem == null ? 0 : lastTotalAmount;

                            decimal firstRent = iDirection == 1 ? dayRent - lastAmount : lastAmount * iDirection;
                            if (iDirection == 1)
                            {
                                firstRent = dayRent - lastAmount > TotalAmount ? TotalAmount : dayRent - lastAmount;
                            }
                            else
                            {
                                firstRent = lastAmount > Math.Abs(TotalAmount) ? TotalAmount : lastAmount - dayRent;
                            }

                            int dayCount = int.Parse(Math.Ceiling(Math.Abs((TotalAmount - firstRent) / dayRent)).ToString()) + 1;

                            for (int y = 0; y < dayCount && TotalAmount != 0; y++)
                            {
                                decimal Amount = Math.Abs(TotalAmount) > dayRent ? dayRent * iDirection : TotalAmount;

                                if (j == rows.Length - 1 && TotalAmount > 0)
                                {
                                    Amount = Math.Abs(TotalAmount) > dayRent * 1.5M ? dayRent * iDirection : TotalAmount;
                                }
                                if (y == 0) Amount = firstRent;

                                MonthRentApportion rent = new MonthRentApportion()
                                {
                                    ContractID = ContractID,
                                    RoomID = RoomID,
                                    Date = LastDate.AddDays(y * iDirection),
                                    Amount = Amount,
                                    PaymentDate = PaymentDate,
                                    PeriodicChargeID = PeriodicChargeID,
                                    Type = type
                                };

                                ContractRentList.Add(rent);
                                TotalAmount = TotalAmount - Amount;
                            }
                        }
                        AllRentList.AddRange(ContractRentList);

                    }

                    DataTable newTable = SQLHelper.ExecuteDataset(ConstUtility.ConnectionStrings, CommandType.Text, "select Type,ContractID,RoomID,Amount,Date,UpdateDate,PeriodicChargeID from DailyRentApportion where 1=2").Tables[0];

                    for (int y = 0; y < AllRentList.Count; y++)
                    {
                        DataRow dr = newTable.NewRow();
                        dr["ContractID"] = AllRentList[y].ContractID;
                        dr["RoomID"] = AllRentList[y].RoomID;
                        dr["Amount"] = AllRentList[y].Amount;
                        dr["Date"] = AllRentList[y].Date.ToString();
                        dr["UpdateDate"] = actionDate;
                        dr["PeriodicChargeID"] = AllRentList[y].PeriodicChargeID;
                        dr["Type"] = AllRentList[y].Type;

                        newTable.Rows.Add(dr);

                        daySumCount++;
                    }

                    TimeSpan ts5 = new TimeSpan(DateTime.Now.Ticks);

                    SqlConnection sqlCon = new SqlConnection(ConstUtility.ConnectionStrings);
                    sqlCon.Open();
                    SqlBulkCopy bulk = new SqlBulkCopy(sqlCon);

                    foreach (DataColumn item in newTable.Columns)
                    {
                        bulk.ColumnMappings.Add(item.ColumnName, item.ColumnName);
                    }

                    bulk.BatchSize = 5000;
                    bulk.DestinationTableName = "DailyRentApportion";
                    bulk.WriteToServer(newTable);
                    sqlCon.Close();
                    sqlCon.Dispose();

                    sqlList = new List<string>();

                    strSQL = string.Format(@"INSERT INTO [DailyRentApportionLog]([Date],[UpdateTime],[TakeTime],[ICount])VALUES('{0}','{1}','{2}',{3})"
                                        , actionDate, DateTime.Now, ts5.Subtract(ts4).Duration().TotalSeconds + "秒", daySumCount);

                    sqlList.Add(strSQL);

                    TimeSpan ts6 = new TimeSpan(DateTime.Now.Ticks);
                    SQLHelper.ExecuteNonQuery(ConstUtility.ConnectionStrings, sqlList);
                    iLog.WriteLog("执行插入语句，条数:" + daySumCount + " 耗时:" + (new TimeSpan(DateTime.Now.Ticks)).Subtract(ts6).Duration().TotalSeconds + "秒", 0);

                    totalCount = totalCount + daySumCount;

                    iLog.WriteLog("日期:" + actionDate + "  结束分摊，记录:" + daySumCount + " 耗时:" + (new TimeSpan(DateTime.Now.Ticks)).Subtract(ts4).Duration().TotalSeconds + "秒", 0);
                }

                iLog.WriteLog("日租金分摊结束 总记录:" + totalCount + " 耗时:" + (new TimeSpan(DateTime.Now.Ticks)).Subtract(ts2).Duration().TotalSeconds + "秒", 0);
            }
            catch  
            {
                //iLog.WriteLog("日租金分摊异常:" + e.Message, 1);
                throw;
            }
        }
        /// <summary>
        /// 分摊月租金
        /// </summary>
        public void ApportionActions2()
        {
            try
            {
                iLog.WriteLog("月租金分摊开始", 0);
                TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
                int totalCount = 0;

                List<string> deleteList = new List<string>();
                string deleteSQL = string.Format(@"DELETE  FROM MonthRentApportion WHERE PaymentDate >= '{0}'", DateTime.Now.AddDays(ConstUtility.RE_CALCULATION_DAYS * -1));
                deleteList.Add(deleteSQL);
                deleteSQL = string.Format(@"DELETE  FROM MonthRentApportionLog WHERE PaymentDate >= '{0}'", DateTime.Now.AddDays(ConstUtility.RE_CALCULATION_DAYS * -1));
                deleteList.Add(deleteSQL);
                SQLHelper.ExecuteNonQuery(ConstUtility.ConnectionStrings, deleteList);

                string strSQL = string.Format(@"SELECT MAX(PaymentDate) FROM MonthRentApportionLog");
                DataTable dt = SQLHelper.ExecuteDataset(ConstUtility.ConnectionStrings, CommandType.Text, strSQL).Tables[0];
                DateTime startDate = ConstUtility.EXECUTE_STARTDATE.AddDays(-1);

                if (dt.Rows.Count != 0 && dt.Rows[0][0].ToString() != "")
                {
                    startDate = DateTime.Parse(dt.Rows[0][0].ToString());
                }

                DateTime nowDate = DateTime.Now;

                TimeSpan ts4 = new TimeSpan(startDate.Ticks);
                TimeSpan ts5 = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts6 = ts5.Subtract(ts4).Duration();
                int iCount = ts6.Days;

                string strDelete = string.Format(@"delete from MonthRentApportion where ContractID IN (
	                    select ct.ID from Contracts ct
		                    left join (select ContractID,SUM(Amount) Amount,
		                    MIN(StartDate) as StartDate,MAX(EndDate) EndDate From (
		                    select ContractID,Amount Amount,StartDate,
			                    case when Amount < 0 then StartDate else  EndDate end as EndDate
		                    from PeriodicCharges pc 
		                    inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID  
			                    where pc.status = 1 and pm.Status IN(2,3,4)  ) T group by ContractID) P on ct.ID = p.ContractID
	                    where ct.Status = 4	 and  p.EndDate >= '{0}' and p.EndDate < '{1}' and ct.type = 1 )
                    ", startDate.AddDays(1), DateTime.Now.Date.AddDays(1));

                List<string> strList = new List<string>();
                strList.Add(strDelete);

                strDelete = string.Format(@"delete from MonthRentApportion where ContractID IN (
                                    select pc.ContractID from PaymentMethods pm 
	                                    inner join PeriodicCharges pc on pm.ID = pc.PaymentMethodID
                                        inner join Contracts ct on ct.id= pc.ContractID 
                                    where isnull(pm.Date,pm.CreateDate) >= '{0}' and isnull(pm.Date,pm.CreateDate) < '{1}'
	                                    and pc.Status = 1 and pm.Status IN (2,3,4) AND pc.FeeType= 31 and ct.type <> 1)
                    ", startDate.AddDays(1), DateTime.Now.Date.AddDays(1));

                strList.Add(strDelete);

                SQLHelper.ExecuteNonQuery(ConstUtility.ConnectionStrings, strList);

                strSQL = string.Format(@"
                        select ct.StartDate,ct.EndDate,cf.CharterMoney,cf.ContractID,pc.Amount as TotalAmount,
                                ct.RoomID,pc.ID as PeriodicChargeID,isnull(pm.Date,pm.CreateDate) Date,LastPayDate,pc.FeeType FeeType from PeriodicCharges pc 
                            inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID
                            inner join Contracts ct on pc.ContractID = ct.ID
                            inner join ContractFees cf on pc.ContractID = cf.ContractID
                            inner join (
                                select ContractID,MAX(isnull(pm.Date,pm.CreateDate)) as LastPayDate from PaymentMethods pm 
	                                inner join PeriodicCharges pc on pm.ID = pc.PaymentMethodID
                                where isnull(pm.Date,pm.CreateDate) >= '{0}' and isnull(pm.Date,pm.CreateDate) < '{1}'
	                                and pc.Status = 1 and pm.Status IN (2,3,4) AND pc.FeeType= 31 group by ContractID
                                ) T on ct.ID = T.ContractID
                        where  pc.Status = 1 and pm.Status IN (2,3,4) AND pc.FeeType= 31 and ct.type != 1 and isnull(pm.Date,pm.CreateDate) < '{1}' and ISNULL(pc.Amount,0)<>0
                        union all
                        select ct.StartDate,ct.EndDate,ra.MonthlyTotalAmount,ct.ID,ra.TotalAmount,ct.RoomID,null,DateOfLoan,DateOfLoan,31 FeeType from RentLoanAudits ra 
	                        inner join Contracts ct on ra.RenterID = ct.RenterID
                        where  DateOfLoan >= '{0}' and DateOfLoan < '{1}' and ct.type = 1
                        union all
                        select ct.StartDate,ct.EndDate,cf.CharterMoney,ct.ID,isnull(Amount,0), ct.RoomID,PeriodicChargeID,p.EndDate,p.EndDate,FeeType from Contracts ct inner join (
	                        select ContractID,SUM(Amount) Amount,
		                        MIN(StartDate) as StartDate,MAX(EndDate) EndDate,PeriodicChargeID,T.FeeType 
	                        From (
		                        select ContractID,Amount Amount,StartDate,
			                        case when Amount < 0 then StartDate else  EndDate end as EndDate,pc.ID as PeriodicChargeID,pc.FeeType FeeType
		                        from PeriodicCharges pc 
		                        inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID  
		                        where pc.status = 1 and pm.Status IN(2,3,4)  ) T 
	                        group by ContractID,PeriodicChargeID,FeeType ) p
		                        on ct.ID = p.ContractID
	                        inner join ContractFees cf on ct.id = cf.ContractID
                        where ct.ID IN (
	                    select ct.ID from Contracts ct
		                    left join (select ContractID,SUM(Amount) Amount,
		                    MIN(StartDate) as StartDate,MAX(EndDate) EndDate From (
		                    select ContractID,Amount Amount,StartDate,
			                    case when Amount < 0 then StartDate else  EndDate end as EndDate
		                    from PeriodicCharges pc 
		                    inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID  
			                    where pc.status = 1 and pm.Status IN(2,3,4)  ) T group by ContractID) P on ct.ID = p.ContractID
	                    where ct.Status = 4	 and  p.EndDate >= '{0}' and p.EndDate < '{1}' and ct.type = 1 )                           
                    ", startDate.AddDays(1), DateTime.Now.Date.AddDays(1));

                DataTable allPaymentDt = SQLHelper.ExecuteDataset(ConstUtility.ConnectionStrings, CommandType.Text, strSQL).Tables[0];

                //allPaymentDt = allPaymentDt.Select(" ContractID=28611").CopyToDataTable();

                for (int i = 1; i <= iCount; i++)
                {
                    DateTime actionDate = startDate.AddDays(i).Date;

                    List<string> sqlList = new List<string>();

                    TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);

                    iLog.WriteLog("日期:" + actionDate + "  开始分摊", 0);

                    DataRow[] allPaymentDtRows = allPaymentDt.Select("LastPayDate >= '" + actionDate + "' AND LastPayDate < '" + actionDate.AddDays(1) + "'");
                    if (allPaymentDtRows.Length == 0)
                    {
                        iLog.WriteLog("日期:" + actionDate + "  结束分摊，记录:" + sqlList.Count + " 耗时:" + (new TimeSpan(DateTime.Now.Ticks)).Subtract(ts2).Duration().TotalSeconds + "秒", 0);
                        continue;
                    }

                    DataTable paymentDt = allPaymentDtRows.CopyToDataTable();

                    DataTable dtContract = paymentDt.DefaultView.ToTable(true, "ContractID");
                    List<MonthRentApportion> AllRentList = new List<MonthRentApportion>();

                    for (int k = 0; k < dtContract.Rows.Count; k++)
                    {
                        int contractID = int.Parse(dtContract.Rows[k][0].ToString());
                        DataRow[] rows = paymentDt.Select("ContractID = " + contractID);

                        List<MonthRentApportion> ContractRentList = new List<MonthRentApportion>();

                        int cRoomID = int.Parse(rows[0]["RoomID"].ToString());//房间ID
                        decimal iCharterMoney = decimal.Parse(rows[0]["CharterMoney"].ToString());//标准月租金
                        DateTime cStartDate = DateTime.Parse(rows[0]["StartDate"].ToString());//合同开始日
                        DateTime cEndDate = DateTime.Parse(rows[0]["EndDate"].ToString());//合同结束日     

                        int type = 0;
                        int.TryParse(paymentDt.Rows[0]["FeeType"].ToString(), out type);


                        for (int j = 0; j < rows.Length; j++)
                        {
                            DateTime PaymentDate = DateTime.Parse(rows[j]["Date"].ToString());
                            int PeriodicChargeID = rows[j]["PeriodicChargeID"] == null || rows[j]["PeriodicChargeID"].ToString() == "" ? 0 : int.Parse(rows[j]["PeriodicChargeID"].ToString());
                            decimal TotalAmount = decimal.Parse(rows[j]["TotalAmount"].ToString());//当日实收

                            if (TotalAmount > 0)
                            {
                                DateTime ShareDate = cStartDate > PaymentDate ? PaymentDate : cStartDate;

                                if (ContractRentList != null && ContractRentList.Count > 0)
                                {
                                    MonthRentApportion LastItem = ContractRentList.OrderByDescending(p => p.Month).OrderByDescending(p => p.Year).First();
                                    ShareDate = DateTime.Parse(LastItem.Year + "-" + LastItem.Month + "-01");
                                }

                                ShareDate = ShareDate > PaymentDate ? PaymentDate : ShareDate;

                                SetMonthRent(type, contractID, cRoomID, cStartDate, cEndDate, PaymentDate, ShareDate, iCharterMoney, TotalAmount, TotalAmount, PeriodicChargeID, ref ContractRentList);
                            }
                            else
                            {
                                if (ContractRentList != null && ContractRentList.Count > 0)
                                {
                                    MonthRentApportion LastItem = ContractRentList.OrderByDescending(p => p.Month).OrderByDescending(p => p.Year).First();
                                    DateTime LastDate = DateTime.Parse(LastItem.Year + "-" + LastItem.Month + "-01");

                                    LastDate = LastDate > PaymentDate ? LastDate : PaymentDate;

                                    SetMonthRent(type, contractID, cRoomID, cStartDate, cEndDate, PaymentDate, LastDate, iCharterMoney, TotalAmount, TotalAmount, PeriodicChargeID, ref ContractRentList);
                                }
                            }
                        }

                        AllRentList.AddRange(ContractRentList);
                    }

                    for (int x = 0; x < AllRentList.Count; x++)
                    {
                        strSQL = string.Format(@"
                                INSERT INTO [dbo].[MonthRentApportion] ([ContractID],[RoomID],[Year],[Month],[RealAmount],[Amount],[RsvAmount],[PaymentDate] ,[PeriodicChargeID],[Type])
                                VALUES ({0},{1},{2},{3},{4},{5},{6},'{7}',{8},{9})
                                ", AllRentList[x].ContractID, AllRentList[x].RoomID, AllRentList[x].Year, AllRentList[x].Month, AllRentList[x].RealAmount, AllRentList[x].Amount, AllRentList[x].RsvAmount, AllRentList[x].PaymentDate, AllRentList[x].PeriodicChargeID, AllRentList[x].Type);
                        sqlList.Add(strSQL);
                    }

                    TimeSpan ts3 = new TimeSpan(DateTime.Now.Ticks);

                    strSQL = string.Format(@"
                                INSERT INTO [dbo].[MonthRentApportionLog] ([Year],[Month],[OperateTime],[TakeTime],[ICount],[PaymentDate])
                                VALUES ({0},{1},'{2}','{3}',{4},'{5}')
                                        ", actionDate.Year, actionDate.Month, DateTime.Now.ToString(), ts3.Subtract(ts2).Duration().TotalSeconds + "秒", sqlList.Count, actionDate);

                    sqlList.Add(strSQL);

                    SQLHelper.ExecuteNonQuery(ConstUtility.ConnectionStrings, sqlList);

                    totalCount = totalCount + sqlList.Count;

                    iLog.WriteLog("日期:" + actionDate + "  结束分摊，记录:" + sqlList.Count + " 耗时:" + (new TimeSpan(DateTime.Now.Ticks)).Subtract(ts2).Duration().TotalSeconds + "秒", 0);
                }

                iLog.WriteLog("月租金分摊结束 总记录:" + totalCount + " 耗时:" + (new TimeSpan(DateTime.Now.Ticks)).Subtract(ts1).Duration().TotalSeconds + "秒", 0);
            }
            catch 
            {
                //iLog.WriteLog("月租金分摊异常:" + e.Message, 1);
                throw;
            }

        }


        /// <summary>
        /// 获取日租金
        /// </summary>
        /// <param name="StartDate">合同开始日期</param>
        /// <param name="EndDate">合同结束日期</param>
        /// <param name="MonthRent">月租金</param>
        /// <returns></returns>
        public static decimal GetDayRent(DateTime StartDate, DateTime EndDate, decimal MonthRent)
        {
            decimal CountDay = 365;

            TimeSpan ts1 = new TimeSpan(StartDate.Ticks);
            TimeSpan ts2 = new TimeSpan(EndDate.Ticks);
            TimeSpan ts3 = ts2.Subtract(ts1);

            if (ts3.Days < 365)
            {
                CountDay = 365;
            }
            else if (ts3.Days == 365)
            {
                CountDay = 366;
            }
            else if (ts3.Days >= 366)
            {
                int StrarYear = StartDate.Year;
                int EndYear = EndDate.Year;
                bool isLeapYear = false;
                //1、判断是否经过润年的2月29日
                for (int i = StrarYear; i <= EndYear; i++)
                {
                    if (i % 4 == 0)
                    {
                        DateTime tempDate = DateTime.Parse(i + "-02-29");

                        if (StartDate <= tempDate && tempDate <= EndDate)
                        {
                            isLeapYear = true;
                        }
                    }
                }

                if (isLeapYear)
                {
                    CountDay = 365.5M;
                }
                else
                {
                    CountDay = 365;
                }
            }

            return Math.Round(MonthRent * 12 / CountDay, 4);
        }
         
        #region 根据房间单价合同信息获取日租金 +decimal GetUnitPrice(deDateTime StartDate, DateTime EndDate, decimal MonthRent)
        /// <summary>
        /// 根据房间单价合同信息获取日租金
        /// </summary>
        /// <param name="roomRent">房间单价</param> 
        /// <returns>日租金</returns>
        public static decimal GetUnitPrice(DateTime StartDate, DateTime EndDate, decimal MonthRent)
        {
            decimal unitPrice = 0.00M;
            //unitPrice = Math.Round(roomRent * 12 / DateUtil.GetContractDays(contract.StartDate, contract.EndDate) * feeDays, 2);

            //if (contract.PreContract != null)
            //{
            //    contract = GetPreContract(contract);
            //}

            int month = 0;
            int days = 0;
            decimal otherdecs = 0.00M;
            month = GetMonthAndDaysBySTimeAndETime(StartDate, EndDate, out days);

            //计算多余天数租金
            if (days != 0)
            {
                otherdecs = MonthRent * 12 / 365 * days;
            }
            unitPrice = Math.Round((MonthRent * month + otherdecs) / GetDaysBySTimeAndETime(StartDate, EndDate, 1), 4);

            return unitPrice;
        }
        #endregion

        #region 根据房间单价合同信息获取日租金 +decimal GetUnitPrice(deDateTime StartDate, DateTime EndDate, decimal MonthRent)
        /// <summary>
        /// 根据房间单价合同信息获取日租金
        /// </summary>
        /// <param name="roomRent">房间单价</param> 
        /// <returns>日租金</returns>
        public static decimal GetFirstMonthUnitPrice(DateTime StartDate, DateTime EndDate, decimal MonthRent)
        {
            decimal unitPrice = 0.00M;

            int days = 0;
            days = DateTime.Parse(StartDate.Year + "-" + StartDate.Month + "-01").AddMonths(1).AddDays(-1).Day - StartDate.Day + 1;
            unitPrice = Math.Round(MonthRent / 30 * days, 4);

            return unitPrice;
        }
        #endregion

        #region 根据开始时间和结束时间获取月份和多余天数 + static int GetMonthAndDaysBySTimeAndETime(DateTime sTime, DateTime eTime,out int days)
        /// <summary>
        /// 根据开始时间和结束时间获取月份
        /// </summary>
        /// <param name="sTime">开始时间 如2016-1-20</param>
        /// <param name="eTime">结束时间 如2016-4-19</param>
        /// <returns>相差月数</returns>
        public static int GetMonthAndDaysBySTimeAndETime(DateTime sTime, DateTime eTime, out int days)
        {
            int month = 0;
            days = 0;
            DateTime tempDate = sTime;

            while (eTime.AddDays(1) > sTime && eTime.AddDays(1) >= sTime.AddMonths(1))
            {                
                month++;
                sTime = tempDate.AddMonths(month);
            }

            while (sTime <= eTime)
            {
                days++;
                sTime = sTime.AddDays(1);
            }

            return month;
        }
        #endregion
         
        #region 根据开始时间和结束时间获取天数 + static int GetDaysBySTimeAndETime(DateTime sTime, DateTime eTime)
        /// <summary>
        /// 根据开始时间和结束时间获取天数
        /// </summary>
        /// <param name="sTime">开始时间 如2016-1-20</param>
        /// <param name="eTime">结束时间 如2016-4-19</param>
        /// <param name="addday">加天数</param>
        /// <returns>相差天数</returns>
        public static int GetDaysBySTimeAndETime(DateTime sTime, DateTime eTime, int addday)
        {
            int num = Days(sTime, eTime);
            //DateTime start = Convert.ToDateTime(sTime.ToShortDateString());
            //DateTime end = Convert.ToDateTime(eTime.ToShortDateString());

            //TimeSpan sp = end.Subtract(start);
            int days = 0;
            days = GetDaysBySTimeAndETime(sTime, eTime);
            if (addday != 0)
            {
                days = days + addday;
            }
            return days;
        }
        /// <summary>
        /// 根据开始时间和结束时间获取天数
        /// </summary>
        /// <param name="sTime">开始时间 如2016-1-20</param>
        /// <param name="eTime">结束时间 如2016-4-19</param>
        /// <returns>相差天数</returns>
        public static int GetDaysBySTimeAndETime(DateTime sTime, DateTime eTime)
        {

            int num = Days(sTime, eTime);
            DateTime start = Convert.ToDateTime(sTime.ToShortDateString());
            DateTime end = Convert.ToDateTime(eTime.ToShortDateString());

            TimeSpan sp = end.Subtract(start);
            int days = 0;
            //days = sp.Days + 1;
            days = sp.Days;
            return days;
        } 

        public static int Days(DateTime d1, DateTime d2, int adjustDay = 0)
        {
            return d2.Subtract(d1).Days + adjustDay;
        }

        #endregion

        /// <summary>
        /// 设置月租金-收款
        /// </summary>
        /// <param name="cContractID">合同ID</param>
        /// <param name="cRoomID">房间ID</param>
        /// <param name="dDate">分摊的月份所在日期</param>
        /// <param name="pDate">支付日期</param>        
        /// <param name="iTotalAmount">支付金额</param>         
        /// <param name="iCharterMoney">标准月租金</param>
        /// <param name="hasPayment">当月是否实付</param>
        /// <param name="iAmount">待分摊金额</param>
        /// <param name="isSecondMonth">是否是第二个月</param>
        /// <param name="lastMonthAmount">上个月收入金额</param>
        /// <param name="lastMonthRsvAmount">上个月预收金额</param>        
        /// <param name="actionDate">分摊日期</param>
        /// <param name="sqlList">记录集</param>
        public static void SetMonthRent(int cContractID, int cRoomID, DateTime dDate, DateTime pDate, decimal iTotalAmount,
            decimal iCharterMoney, bool hasPayment, decimal iAmount, bool isSecondMonth, decimal lastMonthAmount, decimal lastMonthRsvAmount, DateTime actionDate, ref List<string> sqlList, string PeriodicChargeID)
        {
            string strSQL = "";
            decimal iRealAmount = 0;
            lastMonthRsvAmount = lastMonthRsvAmount - iCharterMoney;

            // 开始日期和支付日期不在同一月份
            if (dDate.Year == pDate.Year && dDate.Month == pDate.Month)
            {
                hasPayment = true;
                iRealAmount = iTotalAmount;
                lastMonthRsvAmount = lastMonthRsvAmount + iTotalAmount;
            }

            //判断是否是第二个月，如果是，则首月不用不足，直接写次月数据
            if (isSecondMonth)
            {
                //算出是否满足整月，满足-标准月租金，继续循环；不满足-拼接上月赋值跳出
                if (iAmount >= iCharterMoney)
                {
                    strSQL = string.Format(@"
                                    INSERT INTO [dbo].[MonthRentApportion] ([ContractID],[RoomID],[Year],[Month],[RealAmount],[Amount],[RsvAmount],[PaymentDate] ,[PeriodicChargeID])
                                    VALUES ({0},{1},{2},{3},{4},{5},{6},'{7}',{8})
                                           ", cContractID, cRoomID, dDate.Year, dDate.Month, iRealAmount, iCharterMoney, lastMonthRsvAmount, actionDate, PeriodicChargeID);
                    sqlList.Add(strSQL);
                    SetMonthRent(cContractID, cRoomID, dDate.AddMonths(1), pDate, iTotalAmount, iCharterMoney, false, iAmount - iCharterMoney, false, iCharterMoney, lastMonthRsvAmount, actionDate, ref sqlList, PeriodicChargeID);
                }
                else
                {
                    strSQL = string.Format(@"
                                    INSERT INTO [dbo].[MonthRentApportion] ([ContractID],[RoomID],[Year],[Month],[RealAmount],[Amount],[RsvAmount],[PaymentDate] ,[PeriodicChargeID])
                                    VALUES ({0},{1},{2},{3},{4},{5},{6},'{7}',{8})
                                           ", cContractID, cRoomID, dDate.Year, dDate.Month, iRealAmount, iAmount, 0, actionDate, PeriodicChargeID);
                    sqlList.Add(strSQL);
                }
            }
            else
            {
                if (lastMonthAmount < iCharterMoney)
                {
                    if (iAmount > iCharterMoney - lastMonthAmount)
                    {
                        strSQL = string.Format(@"
                                    INSERT INTO [dbo].[MonthRentApportion] ([ContractID],[RoomID],[Year],[Month],[RealAmount],[Amount],[RsvAmount],[PaymentDate] ,[PeriodicChargeID])
                                    VALUES ({0},{1},{2},{3},{4},{5},{6},'{7}',{8})
                                           ", cContractID, cRoomID, dDate.AddMonths(-1).Year, dDate.AddMonths(-1).Month, iRealAmount, iCharterMoney - lastMonthAmount, lastMonthRsvAmount + lastMonthAmount, actionDate, PeriodicChargeID);
                        sqlList.Add(strSQL);
                        SetMonthRent(cContractID, cRoomID, dDate, pDate, iTotalAmount, iCharterMoney, false, iAmount - (iCharterMoney - lastMonthAmount), false, iCharterMoney, lastMonthRsvAmount, actionDate, ref sqlList, PeriodicChargeID);
                    }
                    else
                    {
                        strSQL = string.Format(@"
                                    INSERT INTO [dbo].[MonthRentApportion] ([ContractID],[RoomID],[Year],[Month],[RealAmount],[Amount],[RsvAmount],[PaymentDate] ,[PeriodicChargeID])
                                    VALUES ({0},{1},{2},{3},{4},{5},{6},'{7}',{8})
                                           ", cContractID, cRoomID, dDate.AddMonths(-1).Year, dDate.AddMonths(-1).Month, iRealAmount, iAmount, 0, actionDate, PeriodicChargeID);
                        sqlList.Add(strSQL);
                    }
                }
                else
                {
                    if (iAmount > iCharterMoney)
                    {
                        strSQL = string.Format(@"
                                    INSERT INTO [dbo].[MonthRentApportion] ([ContractID],[RoomID],[Year],[Month],[RealAmount],[Amount],[RsvAmount],[PaymentDate] ,[PeriodicChargeID])
                                    VALUES ({0},{1},{2},{3},{4},{5},{6},'{7}',{8})
                                           ", cContractID, cRoomID, dDate.Year, dDate.Month, iRealAmount, iCharterMoney, lastMonthRsvAmount, actionDate, PeriodicChargeID);
                        sqlList.Add(strSQL);
                        SetMonthRent(cContractID, cRoomID, dDate.AddMonths(1), pDate, iTotalAmount, iCharterMoney, false, iAmount - iCharterMoney, false, iCharterMoney, lastMonthRsvAmount, actionDate, ref sqlList, PeriodicChargeID);
                    }
                    else
                    {
                        strSQL = string.Format(@"
                                    INSERT INTO [dbo].[MonthRentApportion] ([ContractID],[RoomID],[Year],[Month],[RealAmount],[Amount],[RsvAmount],[PaymentDate] ,[PeriodicChargeID])
                                    VALUES ({0},{1},{2},{3},{4},{5},{6},'{7}',{8})
                                           ", cContractID, cRoomID, dDate.Year, dDate.Month, iRealAmount, iAmount, 0, actionDate, PeriodicChargeID);
                        sqlList.Add(strSQL);
                    }
                }
            }

        }

        /// <summary>        
        /// </summary>
        /// <param name="ContractID">合同ID</param>
        /// <param name="RoomID">房间ID</param>
        /// <param name="PaymentDate">支付日期</param>
        /// <param name="ShareDate">分摊日期</param>
        /// <param name="CharterMoney">月租金</param>
        /// <param name="TotalAmount">分摊金额</param>
        /// <param name="ContractRentList">结果集</param>
        public static void SetMonthRent(int type, int ContractID, int RoomID, DateTime StartDate, DateTime EndDate, DateTime PaymentDate, DateTime ShareDate, decimal normalCharterMoney, decimal TotalAmount, decimal ShareRealAmount, int PeriodicChargeID, ref List<MonthRentApportion> ContractRentList)
        {
            iLog.WriteLog("合同ID" + ContractID, 0);

            // 本次实收
            decimal CurrenRealAmount = 0;
            decimal CurrenCharterMoney = normalCharterMoney;

            if (TotalAmount == 0 && ShareRealAmount == 0) return;

            // 分摊日期与支付日期相同则为实收
            if (PaymentDate.Year == ShareDate.Year && PaymentDate.Month == ShareDate.Month)
            {
                CurrenRealAmount = ShareRealAmount;
                ShareRealAmount = 0;
            }

            // 合同首月
            if (StartDate.Year == ShareDate.Year && StartDate.Month == ShareDate.Month)
            {
                //double iFirstMonthDays = ((new DateTime(StartDate.Year, StartDate.Month, 1)).AddMonths(1).AddDays(-1) - StartDate).TotalDays + 1;
                double iFirstMonthDays = StartDate.Day;
                //decimal dayRent = GetDayRent(StartDate, EndDate, normalCharterMoney);
                //New 日租金
                decimal dayRent = GetUnitPrice(StartDate, EndDate, normalCharterMoney);
                decimal iFirstMonthAmount = 0;

                if (StartDate.Day == 1)
                {
                    iFirstMonthAmount = normalCharterMoney;
                }
                else
                {

                    iFirstMonthAmount = Math.Round(Math.Round(normalCharterMoney / new decimal(30.0), 2) * new decimal((30 - iFirstMonthDays + 1)), 0);

                    //iFirstMonthAmount = Math.Round(decimal.Parse(iFirstMonthDays.ToString()) * Math.Round(normalCharterMoney / 30,2), 0);
                    //iFirstMonthAmount = Math.Round(decimal.Parse(iFirstMonthDays.ToString()) * dayRent, 0);
                }

                CurrenCharterMoney = iFirstMonthAmount;
            }

            // 房租提前支付
            if (DateTime.Parse(StartDate.Year + "-" + StartDate.Month + "-01") > DateTime.Parse(ShareDate.Year + "-" + ShareDate.Month + "-01"))
            {
                CurrenCharterMoney = 0;
            }

            // 本月收入
            decimal Amount = ContractRentList.Where(p => p.Year == ShareDate.Year && p.Month == ShareDate.Month).Sum(p => p.Amount);
            // 本月实收
            decimal RealAmount = ContractRentList.Where(p => p.Year == ShareDate.Year && p.Month == ShareDate.Month).Sum(p => p.RealAmount);
            // 上月预收
            decimal LastRsvAmount = ContractRentList.Where(p => p.Year == ShareDate.AddMonths(-1).Year && p.Month == ShareDate.AddMonths(-1).Month).Sum(p => p.RsvAmount);
            // 本月预收
            decimal RsvAmount = ContractRentList.Where(p => p.Year == ShareDate.Year && p.Month == ShareDate.Month).Sum(p => p.RsvAmount);

            // 本次收入
            decimal CurrAmount = TotalAmount > 0 ? (TotalAmount > CurrenCharterMoney - Amount ? CurrenCharterMoney - Amount : TotalAmount) : (TotalAmount * -1 > Amount ? Amount * -1 : TotalAmount);

            decimal CurrRsvAmount = 0;

            if (ShareRealAmount < 0 || TotalAmount < 0)
            {
                // 下月预收
                decimal nextRsvAmount = ContractRentList.Where(p => p.Year == ShareDate.AddMonths(1).Year && p.Month == ShareDate.AddMonths(1).Month).Sum(p => p.RsvAmount);
                // 下月实收
                decimal nextRealAmount = ContractRentList.Where(p => p.Year == ShareDate.AddMonths(1).Year && p.Month == ShareDate.AddMonths(1).Month).Sum(p => p.RealAmount);
                // 下月收入
                decimal nextAmount = ContractRentList.Where(p => p.Year == ShareDate.AddMonths(1).Year && p.Month == ShareDate.AddMonths(1).Month).Sum(p => p.Amount);

                // 退款预收：下月实收 - 下月收入 + 下月预收 - 本月预收
                CurrRsvAmount = nextRealAmount - nextAmount + nextRsvAmount - RsvAmount;
            }
            else
            {
                // 本次预收 = 上月预收 + 本月实收 + 本次实收 -本月收入 - 本月预收 - 本次收入
                CurrRsvAmount = CurrAmount >= 0 ? LastRsvAmount + RealAmount + CurrenRealAmount - Amount - RsvAmount - CurrAmount : RsvAmount * -1;
            }


            if (CurrenRealAmount != 0 || CurrAmount != 0 || CurrRsvAmount != 0)
            {
                MonthRentApportion rent = new MonthRentApportion()
                {
                    ContractID = ContractID,
                    RoomID = RoomID,
                    Year = ShareDate.Year,
                    Month = ShareDate.Month,
                    RealAmount = CurrenRealAmount,
                    Amount = CurrAmount,
                    RsvAmount = CurrRsvAmount,
                    PaymentDate = PaymentDate,
                    PeriodicChargeID = PeriodicChargeID,
                    Type = type
                };
                ContractRentList.Add(rent);
            }

            TotalAmount = TotalAmount - CurrAmount;

            if (TotalAmount - CurrAmount != 0 || TotalAmount != 0)
            {
                if (TotalAmount < 0 || TotalAmount - CurrAmount < 0)
                {
                    // 退款时：合同开始日期 > 分摊日期 2年  跳出
                    if (StartDate.Year - ShareDate.Year > 1) return;

                    SetMonthRent(type, ContractID, RoomID, StartDate, EndDate, PaymentDate, ShareDate.AddMonths(-1), normalCharterMoney, TotalAmount, ShareRealAmount, PeriodicChargeID, ref ContractRentList);
                    return;
                }

                // 收款时：分摊日期 >  合同结束日期 2年  跳出
                if (ShareDate.Year - EndDate.Year > 1) return;

                SetMonthRent(type, ContractID, RoomID, StartDate, EndDate, PaymentDate, ShareDate.AddMonths(1), normalCharterMoney, TotalAmount, ShareRealAmount, PeriodicChargeID, ref ContractRentList);
                return;
            }
        }
    }

}
