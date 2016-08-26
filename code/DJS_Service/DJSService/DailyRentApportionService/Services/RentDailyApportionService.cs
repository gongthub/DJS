using DailyRentApportionService.Models;
using DailyRentApportionService.Utils;
using DBHelper.MSSQLSERVER;
using System;
using System.Collections.Generic;
using System.Data;
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

            //ConstUtility.iLog.WriteLog("月租金分摊服务运行开始");
            //ApportionActions();
            //ConstUtility.iLog.WriteLog("月租金分摊服务运行结束");

            iLog.WriteLog("月租金分摊服务运行开始", 0);
            ApportionActions2();
            iLog.WriteLog("月租金分摊服务运行结束", 0);
        }

        /// <summary>
        /// 分摊日租金
        /// </summary>
        public void ApportionAction()
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

                for (int i = 1; i <= iCount; i++)
                {
                    DateTime actionDate = startDate.AddDays(i).Date;

                    List<string> sqlList = new List<string>();
                    int daySumCount = 0;
                    TimeSpan ts4 = new TimeSpan(DateTime.Now.Ticks);

                    iLog.WriteLog("日期:" + actionDate + "  开始分摊", 0);

                    strSQL = string.Format(@"
                        select ct.StartDate,ct.EndDate,cf.CharterMoney,cf.ContractID,sum(pc.Amount) as TotalAmount,ct.RoomID,pc.ID as PeriodicChargeID from PeriodicCharges pc 
	                        inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID
	                        inner join Contracts ct on pc.ContractID = ct.ID
	                        inner join ContractFees cf on pc.ContractID = cf.ContractID
                        where pm.Date >= '{0}' and pm.Date < '{1}' and  pc.Status = 1 and pm.Status IN (2,3,4) AND pc.FeeType=31 and ct.type != 1 
                        group by ct.StartDate,ct.EndDate,cf.CharterMoney,cf.ContractID,ct.RoomID,pc.ID
                        union all
                        select ct.StartDate,ct.EndDate,ra.MonthlyTotalAmount,ct.ID,ra.TotalAmount,ct.RoomID,null from RentLoanAudits ra 
	                        inner join Contracts ct on ra.RenterID = ct.RenterID
                        where  DateOfLoan >= '{0}' and DateOfLoan < '{1}' and ct.type = 1
                        union all
                        select ct.StartDate,ct.EndDate,ra.MonthlyTotalAmount,ct.ID,ra.TotalAmount * -1, ct.RoomID,null from RentLoanAudits ra 
                            inner join Contracts ct on ra.RenterID = ct.RenterID
                            left join (select ContractID,SUM(Amount) Amount,
                            MIN(StartDate) as StartDate,MAX(EndDate) EndDate From (
                            select ContractID,Amount Amount,StartDate,
                                case when Amount < 0 then StartDate else  EndDate end as EndDate
	                        from PeriodicCharges pc 
	                        inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID  
		                        where pc.status = 1 and pm.Status IN(2,3,4)  ) T group by ContractID) P on ct.ID = p.ContractID
                        where ct.Status = 4	 and  p.EndDate >= '{0}' and p.EndDate < '{1}' and ct.type = 1 
                        union all
                        select ct.StartDate,ct.EndDate,cf.CharterMoney,ct.ID,isnull(Amount,0), ct.RoomID,PeriodicChargeID from Contracts ct inner join (
	                        select ContractID,SUM(Amount) Amount,
		                        MIN(StartDate) as StartDate,MAX(EndDate) EndDate,PeriodicChargeID 
	                        From (
		                        select ContractID,Amount Amount,StartDate,
			                        case when Amount < 0 then StartDate else  EndDate end as EndDate,pc.ID as PeriodicChargeID
		                        from PeriodicCharges pc 
		                        inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID  
		                        where pc.status = 1 and pm.Status IN(2,3,4)  ) T 
	                        group by ContractID,PeriodicChargeID ) p
		                        on ct.ID = p.ContractID
	                        inner join ContractFees cf on ct.id = cf.ContractID
                        where ct.Status = 4	 and  p.EndDate >= '{0}' and p.EndDate < '{1}' and ct.type = 1                      
                        ", actionDate, actionDate.AddDays(1));
                    //And pc.ContractID = 26936

                    DataTable paymentDt = SQLHelper.ExecuteDataset(ConstUtility.ConnectionStrings, CommandType.Text, strSQL).Tables[0];

                    sqlList.Add("DELETE FROM DailyRentApportion WHERE UpdateDate >= '" + actionDate + "'");

                    iLog.WriteLog("查询分摊当日:" + actionDate + " 所有租金支付数据，条数:" + paymentDt.Rows.Count, 0);

                    for (int j = 0; j < paymentDt.Rows.Count; j++)
                    {
                        decimal CharterMoney = decimal.Parse(paymentDt.Rows[j]["CharterMoney"].ToString());
                        decimal TotalAmount = decimal.Parse(paymentDt.Rows[j]["TotalAmount"].ToString());
                        DateTime tmpStartDate = DateTime.Parse(paymentDt.Rows[j]["StartDate"].ToString()).AddDays(-1);
                        decimal lastAmount = 0;

                        if (TotalAmount == 0) continue;

                        strSQL = string.Format(@"SELECT MAX(Date) as Date FROM DailyRentApportion WHERE ContractID = {0} AND UPDATEDate < '{1}'", paymentDt.Rows[j]["ContractID"].ToString(), actionDate);

                        DataTable infoDt = SQLHelper.ExecuteDataset(ConstUtility.ConnectionStrings, CommandType.Text, strSQL).Tables[0];
                        if (infoDt.Rows[0][0] != null && infoDt.Rows[0][0].ToString() != "")
                        {
                            tmpStartDate = DateTime.Parse(infoDt.Rows[0][0].ToString());

                            strSQL = string.Format(@"SELECT isnull(SUM(Amount),0) as Amount FROM DailyRentApportion WHERE ContractID = {0} AND Date = '{1}'", paymentDt.Rows[j]["ContractID"].ToString(), tmpStartDate);
                            DataTable amountDt = SQLHelper.ExecuteDataset(ConstUtility.ConnectionStrings, CommandType.Text, strSQL).Tables[0];

                            lastAmount = decimal.Parse(amountDt.Rows[0][0].ToString());
                            tmpStartDate = tmpStartDate.AddDays(-1);
                        }

                        int iDirection = 1;

                        if (TotalAmount < 0)
                        {
                            iDirection = -1;
                            tmpStartDate = tmpStartDate.AddDays(2);
                        }

                        decimal dayRent = GetDayRent(
                              DateTime.Parse(paymentDt.Rows[j]["StartDate"].ToString()),
                              DateTime.Parse(paymentDt.Rows[j]["EndDate"].ToString())
                            , decimal.Parse(paymentDt.Rows[j]["CharterMoney"].ToString()));

                        decimal firstRent = iDirection == 1 ? dayRent - lastAmount : lastAmount * iDirection;

                        int dayCount = int.Parse(Math.Ceiling(Math.Abs((TotalAmount - firstRent) / dayRent)).ToString()) + 1;

                        for (int k = 1; k <= dayCount; k++)
                        {
                            decimal Amount = Math.Abs(TotalAmount) > dayRent * 1.5M ? dayRent * iDirection : TotalAmount;

                            if (k == 1)
                            {
                                Amount = firstRent;
                            }

                            if (Amount == 0) continue;

                            strSQL = string.Format(@"
                                INSERT INTO [DailyRentApportion]([ContractID],[RoomID],[Amount],[Date],[UpdateDate],[PeriodicChargeID])
                                VALUES({0},{1},{2},'{3}','{4}',{5})"
                                        , paymentDt.Rows[j]["ContractID"].ToString(), paymentDt.Rows[j]["RoomID"].ToString()
                                        , Amount, tmpStartDate.AddDays(k * iDirection), actionDate
                                        , paymentDt.Rows[j]["PeriodicChargeID"] == null || paymentDt.Rows[j]["PeriodicChargeID"].ToString() == "" ? "null" : paymentDt.Rows[j]["PeriodicChargeID"].ToString());
                            sqlList.Add(strSQL);

                            TotalAmount = TotalAmount - Amount;
                            daySumCount++;
                        }

                    }

                    TimeSpan ts5 = new TimeSpan(DateTime.Now.Ticks);

                    strSQL = string.Format(@"INSERT INTO [DailyRentApportionLog]([Date],[UpdateTime],[TakeTime],[ICount])VALUES('{0}','{1}','{2}',{3})"
                                        , actionDate, DateTime.Now, ts5.Subtract(ts4).Duration().TotalSeconds + "秒", daySumCount);

                    sqlList.Add(strSQL);

                    SQLHelper.ExecuteNonQuery(ConstUtility.ConnectionStrings, sqlList);

                    totalCount = totalCount + daySumCount;

                    iLog.WriteLog("日期:" + actionDate + "  结束分摊，记录:" + daySumCount + " 耗时:" + (new TimeSpan(DateTime.Now.Ticks)).Subtract(ts4).Duration().TotalSeconds + "秒", 0);
                }

                iLog.WriteLog("日租金分摊结束 总记录:" + totalCount + " 耗时:" + (new TimeSpan(DateTime.Now.Ticks)).Subtract(ts2).Duration().TotalSeconds + "秒", 0);
            }
            catch (Exception e)
            {
                iLog.WriteLog("日租金分摊异常:" + e.Message, 1);
            }
        }

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
                                    where isnull(pm.Date,pm.CreateDate) >= '{0}' and isnull(pm.Date,pm.CreateDate) < '{1}'
	                                    and pc.Status = 1 and pm.Status IN (2,3,4) AND pc.FeeType= 31)
                    ", startDate.AddDays(1), DateTime.Now.Date.AddDays(1));

                strList.Add(strDelete);

                SQLHelper.ExecuteNonQuery(ConstUtility.ConnectionStrings, strList);

                strSQL = string.Format(@"
                        select ct.StartDate,ct.EndDate,cf.CharterMoney,cf.ContractID,pc.Amount as TotalAmount,
                                ct.RoomID,pc.ID as PeriodicChargeID,isnull(pm.Date,pm.CreateDate) Date,LastPayDate from PeriodicCharges pc 
                            inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID
                            inner join Contracts ct on pc.ContractID = ct.ID
                            inner join ContractFees cf on pc.ContractID = cf.ContractID
                            inner join (
                                select ContractID,MAX(isnull(pm.Date,pm.CreateDate)) as LastPayDate from PaymentMethods pm 
	                                inner join PeriodicCharges pc on pm.ID = pc.PaymentMethodID
                                where isnull(pm.Date,pm.CreateDate) >= '{0}' and isnull(pm.Date,pm.CreateDate) < '{1}'
	                                and pc.Status = 1 and pm.Status IN (2,3,4) AND pc.FeeType= 31 group by ContractID
                                ) T on ct.ID = T.ContractID
                        where  pc.Status = 1 and pm.Status IN (2,3,4) AND pc.FeeType= 31 and ct.type != 1 and isnull(pm.Date,pm.CreateDate) < '{1}'
                        union all
                        select ct.StartDate,ct.EndDate,ra.MonthlyTotalAmount,ct.ID,ra.TotalAmount,ct.RoomID,null,DateOfLoan,DateOfLoan from RentLoanAudits ra 
	                        inner join Contracts ct on ra.RenterID = ct.RenterID
                        where  DateOfLoan >= '{0}' and DateOfLoan < '{1}' and ct.type = 1
                        union all
                        select ct.StartDate,ct.EndDate,cf.CharterMoney,ct.ID,isnull(Amount,0), ct.RoomID,PeriodicChargeID,p.EndDate,p.EndDate from Contracts ct inner join (
	                        select ContractID,SUM(Amount) Amount,
		                        MIN(StartDate) as StartDate,MAX(EndDate) EndDate,PeriodicChargeID 
	                        From (
		                        select ContractID,Amount Amount,StartDate,
			                        case when Amount < 0 then StartDate else  EndDate end as EndDate,pc.ID as PeriodicChargeID
		                        from PeriodicCharges pc 
		                        inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID  
		                        where pc.status = 1 and pm.Status IN(2,3,4)  ) T 
	                        group by ContractID,PeriodicChargeID ) p
		                        on ct.ID = p.ContractID
	                        inner join ContractFees cf on ct.id = cf.ContractID
                        where ct.Status = 4	 and  p.EndDate >= '{0}' and p.EndDate < '{1}' and ct.type = 1                      
                    ", startDate.AddDays(1), DateTime.Now.Date.AddDays(1));

                DataTable allPaymentDt = SQLHelper.ExecuteDataset(ConstUtility.ConnectionStrings, CommandType.Text, strSQL).Tables[0];

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

                        decimal dayRent = GetDayRent(cStartDate, cEndDate, iCharterMoney);

                        for (int j = 0; j < rows.Length; j++)
                        {
                            DateTime PaymentDate = DateTime.Parse(rows[j]["Date"].ToString());
                            int PeriodicChargeID = rows[j]["PeriodicChargeID"] == null || rows[j]["PeriodicChargeID"].ToString() == "" ? 0 : int.Parse(rows[j]["PeriodicChargeID"].ToString());
                            decimal TotalAmount = decimal.Parse(rows[j]["TotalAmount"].ToString());

                            int iDirection = TotalAmount > 0 ? 1 : -1;

                            MonthRentApportion lastItem = ContractRentList.LastOrDefault();
                            DateTime LastDate = lastItem == null ? cStartDate : lastItem.Date;
                            decimal lastAmount = lastItem == null ? 0 : lastItem.Amount;

                            decimal firstRent = iDirection == 1 ? dayRent - lastAmount : lastAmount * iDirection;
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
                                    PeriodicChargeID = PeriodicChargeID
                                };

                                ContractRentList.Add(rent);
                                TotalAmount = TotalAmount - Amount;
                            }
                        }
                        AllRentList.AddRange(ContractRentList);

                    }

                    string tempSQL = "INSERT INTO [DailyRentApportion]([ContractID],[RoomID],[Amount],[Date],[UpdateDate],[PeriodicChargeID]) VALUES" + string.Format(@"({0},{1},{2},'{3}','{4}',{5})"
                                    , AllRentList[0].ContractID, AllRentList[0].RoomID
                                    , AllRentList[0].Amount, AllRentList[0].Date, actionDate
                                    , AllRentList[0].PeriodicChargeID);

                    for (int y = 1; y < AllRentList.Count; y++)
                    {
                        if (y % 500 == 0)
                        {
                            sqlList.Add(tempSQL);
                            tempSQL = "INSERT INTO [DailyRentApportion]([ContractID],[RoomID],[Amount],[Date],[UpdateDate],[PeriodicChargeID]) VALUES" + string.Format(@"({0},{1},{2},'{3}','{4}',{5})"
                                    , AllRentList[y].ContractID, AllRentList[y].RoomID
                                    , AllRentList[y].Amount, AllRentList[y].Date, actionDate
                                    , AllRentList[y].PeriodicChargeID);
                        }
                        else
                        {
                            tempSQL = tempSQL + string.Format(@",({0},{1},{2},'{3}','{4}',{5})"
                                , AllRentList[y].ContractID, AllRentList[y].RoomID
                                , AllRentList[y].Amount, AllRentList[y].Date, actionDate
                                , AllRentList[y].PeriodicChargeID);
                        }

                        daySumCount++;
                    }

                    if (AllRentList.Count > 0)
                        sqlList.Add(tempSQL);

                    TimeSpan ts5 = new TimeSpan(DateTime.Now.Ticks);

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
            catch (Exception e)
            {
                iLog.WriteLog("日租金分摊异常:" + e.Message, 1);
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

                string strSQL = string.Format(@"SELECT MAX(PaymentDate) FROM MonthRentApportion");
                DataTable dt = SQLHelper.ExecuteDataset(ConstUtility.ConnectionStrings, CommandType.Text, strSQL).Tables[0];
                DateTime startDate = ConstUtility.EXECUTE_STARTDATE.AddDays(-1);

                if (dt.Rows[0][0] != null && dt.Rows[0][0].ToString() != "")
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
                                    where isnull(pm.Date,pm.CreateDate) >= '{0}' and isnull(pm.Date,pm.CreateDate) < '{1}'
	                                    and pc.Status = 1 and pm.Status IN (2,3,4) AND pc.FeeType= 31)
                    ", startDate.AddDays(1), DateTime.Now.Date.AddDays(1));

                strList.Add(strDelete);

                SQLHelper.ExecuteNonQuery(ConstUtility.ConnectionStrings, strList);

                strSQL = string.Format(@"
                        select ct.StartDate,ct.EndDate,cf.CharterMoney,cf.ContractID,pc.Amount as TotalAmount,
                                ct.RoomID,pc.ID as PeriodicChargeID,isnull(pm.Date,pm.CreateDate) Date,LastPayDate from PeriodicCharges pc 
                            inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID
                            inner join Contracts ct on pc.ContractID = ct.ID
                            inner join ContractFees cf on pc.ContractID = cf.ContractID
                            inner join (
                                select ContractID,MAX(isnull(pm.Date,pm.CreateDate)) as LastPayDate from PaymentMethods pm 
	                                inner join PeriodicCharges pc on pm.ID = pc.PaymentMethodID
                                where isnull(pm.Date,pm.CreateDate) >= '{0}' and isnull(pm.Date,pm.CreateDate) < '{1}'
	                                and pc.Status = 1 and pm.Status IN (2,3,4) AND pc.FeeType= 31 group by ContractID
                                ) T on ct.ID = T.ContractID
                        where  pc.Status = 1 and pm.Status IN (2,3,4) AND pc.FeeType= 31 and ct.type != 1 and isnull(pm.Date,pm.CreateDate) < '{1}'
                        union all
                        select ct.StartDate,ct.EndDate,ra.MonthlyTotalAmount,ct.ID,ra.TotalAmount,ct.RoomID,null,DateOfLoan,DateOfLoan from RentLoanAudits ra 
	                        inner join Contracts ct on ra.RenterID = ct.RenterID
                        where  DateOfLoan >= '{0}' and DateOfLoan < '{1}' and ct.type = 1
                        union all
                        select ct.StartDate,ct.EndDate,cf.CharterMoney,ct.ID,isnull(Amount,0), ct.RoomID,PeriodicChargeID,p.EndDate,p.EndDate from Contracts ct inner join (
	                        select ContractID,SUM(Amount) Amount,
		                        MIN(StartDate) as StartDate,MAX(EndDate) EndDate,PeriodicChargeID 
	                        From (
		                        select ContractID,Amount Amount,StartDate,
			                        case when Amount < 0 then StartDate else  EndDate end as EndDate,pc.ID as PeriodicChargeID
		                        from PeriodicCharges pc 
		                        inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID  
		                        where pc.status = 1 and pm.Status IN(2,3,4)  ) T 
	                        group by ContractID,PeriodicChargeID ) p
		                        on ct.ID = p.ContractID
	                        inner join ContractFees cf on ct.id = cf.ContractID
                        where ct.Status = 4	 and  p.EndDate >= '{0}' and p.EndDate < '{1}' and ct.type = 1                      
                    ", startDate.AddDays(1), DateTime.Now.Date.AddDays(1));

                DataTable allPaymentDt = SQLHelper.ExecuteDataset(ConstUtility.ConnectionStrings, CommandType.Text, strSQL).Tables[0];

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

                        for (int j = 0; j < rows.Length; j++)
                        {
                            DateTime PaymentDate = DateTime.Parse(rows[j]["Date"].ToString());
                            int PeriodicChargeID = rows[j]["PeriodicChargeID"] == null || rows[j]["PeriodicChargeID"].ToString() == "" ? 0 : int.Parse(rows[j]["PeriodicChargeID"].ToString());
                            decimal TotalAmount = decimal.Parse(rows[j]["TotalAmount"].ToString());//当日实收

                            if (TotalAmount > 0)
                            {
                                DateTime ShareDate = cStartDate > PaymentDate ? PaymentDate : cStartDate;
                                if (ContractRentList.Count > 0)
                                {
                                    MonthRentApportion LastItem = ContractRentList.OrderByDescending(p => p.Month).OrderByDescending(p => p.Year).First();
                                    ShareDate = DateTime.Parse(LastItem.Year + "-" + LastItem.Month + "-01");
                                }

                                ShareDate = ShareDate > PaymentDate ? PaymentDate : ShareDate;

                                SetMonthRent(contractID, cRoomID, cStartDate, cEndDate, PaymentDate, ShareDate, iCharterMoney, TotalAmount, TotalAmount, PeriodicChargeID, ref ContractRentList);
                            }
                            else
                            {
                                MonthRentApportion LastItem = ContractRentList.OrderByDescending(p => p.Month).OrderByDescending(p => p.Year).First();
                                DateTime LastDate = DateTime.Parse(LastItem.Year + "-" + LastItem.Month + "-01");

                                LastDate = LastDate > PaymentDate ? LastDate : PaymentDate;

                                SetMonthRent(contractID, cRoomID, cStartDate, cEndDate, PaymentDate, LastDate, iCharterMoney, TotalAmount, TotalAmount, PeriodicChargeID, ref ContractRentList);
                            }
                        }

                        AllRentList.AddRange(ContractRentList);
                    }

                    for (int x = 0; x < AllRentList.Count; x++)
                    {
                        strSQL = string.Format(@"
                                INSERT INTO [dbo].[MonthRentApportion] ([ContractID],[RoomID],[Year],[Month],[RealAmount],[Amount],[RsvAmount],[PaymentDate] ,[PeriodicChargeID])
                                VALUES ({0},{1},{2},{3},{4},{5},{6},'{7}',{8})
                                ", AllRentList[x].ContractID, AllRentList[x].RoomID, AllRentList[x].Year, AllRentList[x].Month, AllRentList[x].RealAmount, AllRentList[x].Amount, AllRentList[x].RsvAmount, AllRentList[x].PaymentDate, AllRentList[x].PeriodicChargeID);
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
            catch (Exception e)
            {
                iLog.WriteLog("月租金分摊异常:" + e.Message, 1);
            }

        }

        /// <summary>
        /// 分摊月租金
        /// </summary>
        public void ApportionActions()
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

                string strSQL = string.Format(@"SELECT MAX(PaymentDate) FROM MonthRentApportion");
                DataTable dt = SQLHelper.ExecuteDataset(ConstUtility.ConnectionStrings, CommandType.Text, strSQL).Tables[0];
                DateTime startDate = ConstUtility.EXECUTE_STARTDATE.AddDays(-1);

                if (dt.Rows[0][0] != null && dt.Rows[0][0].ToString() != "")
                {
                    startDate = DateTime.Parse(dt.Rows[0][0].ToString());
                }

                DateTime nowDate = DateTime.Now;

                TimeSpan ts4 = new TimeSpan(startDate.Ticks);
                TimeSpan ts5 = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts6 = ts5.Subtract(ts4).Duration();
                int iCount = ts6.Days;

                for (int i = 1; i <= iCount; i++)
                {
                    DateTime actionDate = startDate.AddDays(i).Date;

                    List<string> sqlList = new List<string>();

                    TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);

                    iLog.WriteLog("日期:" + actionDate + "  开始分摊", 0);

                    //                    strSQL = string.Format(@"
                    //                                    SELECT ct.StartDate,ct.EndDate,cf.CharterMoney,cf.ContractID,sum(pc.Amount) as TotalAmount,ct.RoomID,pm.Date 
                    //                                    FROM PeriodicCharges pc 
                    //	                                    INNER JOIN PaymentMethods pm ON pc.PaymentMethodID = pm.ID
                    //	                                    INNER JOIN Contracts ct ON pc.ContractID = ct.ID
                    //	                                    INNER JOIN ContractFees cf ON pc.ContractID = cf.ContractID
                    //                                    WHERE pm.Date >= '{0}' AND pm.Date < '{1}' AND  pc.Status = 1 AND pc.FeeType=31 AND pm.Status = 2 and ct.type != 1
                    //                                    GROUP BY ct.StartDate,ct.EndDate,cf.CharterMoney,cf.ContractID,ct.RoomID,pm.Date
                    //                                    union all
                    //                                    select ct.StartDate,ct.EndDate,ra.MonthlyTotalAmount,ct.ID,sum(ra.TotalAmount) TotalAmount,ct.RoomID,DateOfLoan from RentLoanAudits ra 
                    //                                        inner join Contracts ct on ra.RenterID = ct.RenterID
                    //                                    where  DateOfLoan >= '{0}' and DateOfLoan < '{1}' and ct.type = 1 
                    //                                    GROUP BY ct.StartDate,ct.EndDate,ra.MonthlyTotalAmount,ct.ID,ct.RoomID,DateOfLoan
                    //                                    union all
                    //                                    select StartDate,EndDate,MonthlyTotalAmount,ID,sum(TotalAmount) as TotalAmount,RoomID,Date  from (
                    //	                                    select ct.StartDate,ct.EndDate,ra.MonthlyTotalAmount,ct.ID,isnull(Amount,0) - (case when ra.DateOfLoan IS NULL then 0 else ra.TotalAmount end) as TotalAmount, ct.RoomID,p.EndDate as Date  from RentLoanAudits ra 
                    //		                                    inner join Contracts ct on ra.RenterID = ct.RenterID
                    //		                                    left join (select ContractID,SUM(Amount) Amount,
                    //		                                    MIN(StartDate) as StartDate,MAX(EndDate) EndDate From (
                    //		                                    select ContractID,Amount Amount,StartDate,
                    //			                                    case when Amount < 0 then StartDate else  EndDate end as EndDate
                    //	                                    from PeriodicCharges where status = 1 ) T group by ContractID) P on ct.ID = p.ContractID
                    //	                                    where ct.Status = 4	 and  p.EndDate >= '{0}' and p.EndDate < '{1}' and ct.type = 1  ) T
                    //                                    GROUP BY StartDate,EndDate,MonthlyTotalAmount,ID,RoomID,Date "
                    //                        , actionDate, actionDate.AddDays(1));

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
                        ", actionDate, actionDate.AddDays(1));

                    List<string> strList = new List<string>();
                    strList.Add(strDelete);
                    SQLHelper.ExecuteNonQuery(ConstUtility.ConnectionStrings, strList);

                    strSQL = string.Format(@"
                        select ct.StartDate,ct.EndDate,cf.CharterMoney,cf.ContractID,sum(pc.Amount) as TotalAmount,ct.RoomID,pc.ID as PeriodicChargeID,pm.Date  from PeriodicCharges pc 
	                        inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID
	                        inner join Contracts ct on pc.ContractID = ct.ID
	                        inner join ContractFees cf on pc.ContractID = cf.ContractID
                        where pm.Date >= '{0}' and pm.Date < '{1}' and  pc.Status = 1 and pm.Status IN (2,3,4) AND pc.FeeType=31 and ct.type != 1
                        group by ct.StartDate,ct.EndDate,cf.CharterMoney,cf.ContractID,ct.RoomID,pc.ID,pm.Date 
                        union all
                        select ct.StartDate,ct.EndDate,ra.MonthlyTotalAmount,ct.ID,ra.TotalAmount,ct.RoomID,null,DateOfLoan from RentLoanAudits ra 
	                        inner join Contracts ct on ra.RenterID = ct.RenterID
                        where  DateOfLoan >= '{0}' and DateOfLoan < '{1}' and ct.type = 1
                        union all
                        select ct.StartDate,ct.EndDate,cf.CharterMoney,ct.ID,isnull(Amount,0), ct.RoomID,PeriodicChargeID,p.EndDate from Contracts ct inner join (
	                        select ContractID,SUM(Amount) Amount,
		                        MIN(StartDate) as StartDate,MAX(EndDate) EndDate,PeriodicChargeID 
	                        From (
		                        select ContractID,Amount Amount,StartDate,
			                        case when Amount < 0 then StartDate else  EndDate end as EndDate,pc.ID as PeriodicChargeID
		                        from PeriodicCharges pc 
		                        inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID  
		                        where pc.status = 1 and pm.Status IN(2,3,4)  ) T 
	                        group by ContractID,PeriodicChargeID ) p
		                        on ct.ID = p.ContractID
	                        inner join ContractFees cf on ct.id = cf.ContractID
                        where ct.Status = 4	 and  p.EndDate >= '{0}' and p.EndDate < '{1}' and ct.type = 1                      
                        ", actionDate, actionDate.AddDays(1));

                    strSQL = string.Format(@"
                        select ct.StartDate,ct.EndDate,cf.CharterMoney,cf.ContractID,sum(pc.Amount) as TotalAmount,ct.RoomID,pc.ID as PeriodicChargeID,pm.Date  from PeriodicCharges pc 
	                        inner join PaymentMethods pm on pc.PaymentMethodID = pm.ID
	                        inner join Contracts ct on pc.ContractID = ct.ID
	                        inner join ContractFees cf on pc.ContractID = cf.ContractID
                        where pm.Date >= '{0}' and pm.Date < '{1}' and  pc.Status = 1 and pm.Status IN (2,3,4) AND pc.FeeType=31 and ct.type != 1  and ct.ID = 27366 
                        group by ct.StartDate,ct.EndDate,cf.CharterMoney,cf.ContractID,ct.RoomID,pc.ID,pm.Date                 
                        ", actionDate, actionDate.AddDays(1));

                    DataTable paymentDt = SQLHelper.ExecuteDataset(ConstUtility.ConnectionStrings, CommandType.Text, strSQL).Tables[0];

                    for (int j = 0; j < paymentDt.Rows.Count; j++)
                    {
                        int cContractID = int.Parse(paymentDt.Rows[j]["ContractID"].ToString());//合同ID
                        int cRoomID = int.Parse(paymentDt.Rows[j]["RoomID"].ToString());//房间ID
                        DateTime cStartDate = DateTime.Parse(paymentDt.Rows[j]["StartDate"].ToString());//合同开始日
                        DateTime cEndDate = DateTime.Parse(paymentDt.Rows[j]["EndDate"].ToString());//合同结束日
                        DateTime pDate = DateTime.Parse(paymentDt.Rows[j]["Date"].ToString());//费用实收日
                        decimal iCharterMoney = decimal.Parse(paymentDt.Rows[j]["CharterMoney"].ToString());//标准月租金
                        decimal iTotalAmount = decimal.Parse(paymentDt.Rows[j]["TotalAmount"].ToString());//当日实收

                        string PeriodicChargeID = paymentDt.Rows[j]["PeriodicChargeID"] == null || paymentDt.Rows[j]["PeriodicChargeID"].ToString() == "" ? "null" : paymentDt.Rows[j]["PeriodicChargeID"].ToString();

                        strSQL = string.Format(@"SELECT MRA.Year,MRA.Month,SUM(Amount) AS AmountSum,Sum(RsvAmount) as RsvAmount FROM MonthRentApportion AS MRA
                                                 INNER JOIN (Select TOP 1 YEAR,MONTH From MonthRentApportion Where ContractID={0} ORDER BY ID DESC) AS T ON MRA.YEAR=T.YEAR AND MRA.MONTH=T.MONTH
                                                 WHERE MRA.ContractID={0} 
                                                 GROUP BY MRA.Year,MRA.Month ORDER BY MRA.Year,MRA.Month", cContractID);
                        DataTable dtt = SQLHelper.ExecuteDataset(ConstUtility.ConnectionStrings, CommandType.Text, strSQL).Tables[0];

                        if (dtt.Rows.Count == 0)
                        {
                            if (iTotalAmount > 0)
                            {
                                double iFirstMonthDays = ((new DateTime(cStartDate.Year, cStartDate.Month, 1)).AddMonths(1).AddDays(-1) - cStartDate).TotalDays + 1;
                                decimal dayRent = GetDayRent(cStartDate, cEndDate, iCharterMoney);
                                decimal iFirstMonthRealAmount = iTotalAmount;
                                decimal iFirstMonthAmount = 0;
                                decimal iFirstMonthRsvAmount = 0;
                                decimal iAmount = 0;

                                if (iTotalAmount <= Math.Round(decimal.Parse(iFirstMonthDays.ToString()) * dayRent, 0))
                                {
                                    iFirstMonthAmount = iTotalAmount;
                                    iFirstMonthRsvAmount = 0;
                                }
                                else
                                {
                                    if (cStartDate.Day == 1)
                                    {
                                        iFirstMonthAmount = iCharterMoney;
                                    }
                                    else
                                    {
                                        iFirstMonthAmount = Math.Round(decimal.Parse(iFirstMonthDays.ToString()) * dayRent, 0);
                                    }
                                    iFirstMonthRsvAmount = iTotalAmount - iFirstMonthAmount;
                                }

                                // 开始日期和支付日期不在同一月份
                                if (cStartDate.Year != pDate.Year || cStartDate.Month != pDate.Month)
                                {
                                    iFirstMonthRealAmount = 0;
                                    iFirstMonthRsvAmount = iFirstMonthAmount * -1;
                                }

                                strSQL = string.Format(@"
                                         INSERT INTO [dbo].[MonthRentApportion] ([ContractID],[RoomID],[Year],[Month],[RealAmount],[Amount],[RsvAmount],[PaymentDate],[PeriodicChargeID])
                                         VALUES ({0},{1},{2},{3},{4},{5},{6},'{7}',{8})
                                           ", cContractID, cRoomID, cStartDate.Year, cStartDate.Month, iFirstMonthRealAmount, iFirstMonthAmount, iFirstMonthRsvAmount, actionDate
                                            , PeriodicChargeID);
                                sqlList.Add(strSQL);

                                // 待分摊金额
                                iAmount = iTotalAmount - iFirstMonthAmount;

                                if (iAmount > 0)
                                {
                                    SetMonthRent(cContractID, cRoomID, cStartDate.AddMonths(1), pDate, iTotalAmount, iCharterMoney, false, iAmount, true, iFirstMonthAmount, iFirstMonthRsvAmount, actionDate, ref sqlList, PeriodicChargeID);
                                }
                            }
                        }
                        else
                        {
                            if (iTotalAmount > 0)
                            {
                                decimal lastMonthAmount = decimal.Parse(dtt.Rows[0]["AmountSum"].ToString());
                                decimal lastMonthRsvAmount = decimal.Parse(dtt.Rows[0]["RsvAmount"].ToString());

                                string iYear = dtt.Rows[0]["Year"].ToString();
                                string iMonth = dtt.Rows[0]["Month"].ToString().PadLeft(2, '0');

                                SetMonthRent(cContractID, cRoomID, DateTime.Parse(iYear + "-" + iMonth + "-01").AddMonths(1), pDate, iTotalAmount, iCharterMoney, true, iTotalAmount, false, lastMonthAmount, lastMonthRsvAmount, actionDate, ref sqlList, PeriodicChargeID);
                            }
                            else
                            {
                                strSQL = string.Format(@"
                                                       SELECT ContractID,Year,Month,SUM(Amount) AS AmountSum FROM MonthRentApportion WHERE ContractID={0} GROUP BY ContractID,Year,Month
                                                       ", cContractID);
                                DataTable dt_Refund = SQLHelper.ExecuteDataset(ConstUtility.ConnectionStrings, CommandType.Text, strSQL).Tables[0];
                                decimal iTotalAmountCopy = iTotalAmount;
                                for (int k = dt_Refund.Rows.Count - 1; k >= 0; k--)
                                {
                                    decimal AmountSum = decimal.Parse(dt_Refund.Rows[k]["AmountSum"].ToString());
                                    string iYear = dt_Refund.Rows[k]["Year"].ToString();
                                    string iMonth = dt_Refund.Rows[k]["Month"].ToString();
                                    if (Math.Abs(iTotalAmountCopy) <= AmountSum)
                                    {
                                        strSQL = string.Format(@"
                                                               INSERT INTO [dbo].[MonthRentApportion] ([ContractID],[RoomID],[Year],[Month],[RealAmount],[Amount],[RsvAmount],[PaymentDate],[PeriodicChargeID])
                                                               VALUES ({0},{1},{2},{3},{4},{5},{6},'{7}',{8})
                                           ", cContractID, cRoomID, iYear, iMonth, iTotalAmount, iTotalAmountCopy, 0, actionDate, PeriodicChargeID);
                                        sqlList.Add(strSQL);
                                        break;
                                    }
                                    else
                                    {
                                        strSQL = string.Format(@"
                                                               INSERT INTO [dbo].[MonthRentApportion] ([ContractID],[RoomID],[Year],[Month],[RealAmount],[Amount],[RsvAmount],[PaymentDate] ,[PeriodicChargeID])
                                                               VALUES ({0},{1},{2},{3},{4},{5},{6},'{7}',{8})
                                           ", cContractID, cRoomID, iYear, iMonth, 0, AmountSum * (-1), 0, actionDate, PeriodicChargeID);
                                        sqlList.Add(strSQL);
                                        iTotalAmountCopy = iTotalAmountCopy + AmountSum;
                                    }
                                }
                            }
                        }
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
            catch (Exception e)
            {
                iLog.WriteLog("月租金分摊异常:" + e.Message, 1);
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
        public static void SetMonthRent(int ContractID, int RoomID, DateTime StartDate, DateTime EndDate, DateTime PaymentDate, DateTime ShareDate, decimal normalCharterMoney, decimal TotalAmount, decimal ShareRealAmount, int PeriodicChargeID, ref List<MonthRentApportion> ContractRentList)
        {
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
                double iFirstMonthDays = ((new DateTime(StartDate.Year, StartDate.Month, 1)).AddMonths(1).AddDays(-1) - StartDate).TotalDays + 1;
                decimal dayRent = GetDayRent(StartDate, EndDate, normalCharterMoney);
                decimal iFirstMonthAmount = 0;

                if (ShareDate.Day == 1)
                {
                    iFirstMonthAmount = normalCharterMoney;
                }
                else
                {
                    iFirstMonthAmount = Math.Round(decimal.Parse(iFirstMonthDays.ToString()) * dayRent, 0);
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

            // 本次预收 = 上月预收 + 本月实收 + 本次实收 -本月收入 - 本月预收 - 本次收入
            decimal CurrRsvAmount = CurrAmount >= 0 ? LastRsvAmount + RealAmount + CurrenRealAmount - Amount - RsvAmount - CurrAmount : RsvAmount * -1;

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
                    PeriodicChargeID = PeriodicChargeID
                };
                ContractRentList.Add(rent);
            }

            TotalAmount = TotalAmount - CurrAmount;

            if (ShareRealAmount - CurrenRealAmount != 0 || TotalAmount != 0)
            {
                if (ShareRealAmount - CurrenRealAmount < 0 || TotalAmount < 0)
                {
                    SetMonthRent(ContractID, RoomID, StartDate, EndDate, PaymentDate, ShareDate.AddMonths(-1), normalCharterMoney, TotalAmount, ShareRealAmount, PeriodicChargeID, ref ContractRentList);
                    return;
                }
                SetMonthRent(ContractID, RoomID, StartDate, EndDate, PaymentDate, ShareDate.AddMonths(1), normalCharterMoney, TotalAmount, ShareRealAmount, PeriodicChargeID, ref ContractRentList);
                return;
            }
        }
    }

}
