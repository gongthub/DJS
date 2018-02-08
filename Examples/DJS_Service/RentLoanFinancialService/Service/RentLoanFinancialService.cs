using RentLoanFinancialService.Model;
using RentLoanFinancialService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanFinancialService.Service
{
    public class RentLoanFinancialService:IRentLoanFinancialService
    {
        IDBRepository dbContext = new IDBRepository();
        private List<DateTime> pullStartTimes = new List<DateTime>();
        public void BankLoanDetail()
        {
            //调用CRM接口
            DateTime StartDate = DateTime.Now.Date;
            //根据Log文件，找到出错时间，重新拉取
            string pullTimeSql = @"select  CAST(CreateDate  as date) as CreateDate
							from (select GUID,convert(varchar(10),CreateDate,23) as CreateDate	
                            from dbo.RentLoanFinancialLogs
                            where IsSuccess = 0 and Type = 0
                            group by GUID,convert(varchar(10),CreateDate,23)) as a
                            left join (select GUID 
                            from dbo.RentLoanFinancialLogs
                            where IsSuccess = 1 and Type = 0
                            group by GUID
                            ) as b ON a.GUID = b.GUID 
                            where b.GUID is null
                            order by CreateDate";
            var pullTime = dbContext.Database.SqlQuery<DateTime>(pullTimeSql).FirstOrDefault();
            if (pullTime != default(DateTime))
            {
                StartDate = pullTime.Date;
            }
            //网络异常
            RentLoanFinancialLog InternetExceptionFinLog = dbContext.RentLoanFinancialLogs.Where(rlfl => rlfl.Type == 3 && rlfl.IsSuccess == false).OrderBy(rlfl => rlfl.CreateDate).FirstOrDefault();
            if (InternetExceptionFinLog != null)
            {
                if (InternetExceptionFinLog.CreateDate.Date < StartDate)
                {
                    StartDate = InternetExceptionFinLog.CreateDate.Date;
                }
            }
            //判断手动设置拉取数据开始时间与StartDate对比
            //if (Convert.ToDateTime(ConstUtility.PullStartDateTime) < StartDate)
            //{
            //    StartDate = Convert.ToDateTime(ConstUtility.PullStartDateTime);
            //}
            pullStartTimes.Add(StartDate);
            DateTime EndDate = DateTime.Now.Date.AddDays(1);
            BankLoanDetailResult result = null;
            try
            {
                Dictionary<String, String> dic = new Dictionary<string, string>();
                dic.Add("ApiUserId", ConstUtility.APIUSERID);
                dic.Add("Signature", Encrypt.MD5Encrypt(ConstUtility.APIUSERID + ConstUtility.SIGNATURE + DateTime.Now.ToString("yyyyMMdd")));
                dic.Add("StartDate", StartDate.ToString());
                dic.Add("EndDate", EndDate.ToString());
                dic.Add("PageIndex", "1");
                dic.Add("PageSize", "10000");
                WebApiInvoker webPaiInvoker = new WebApiInvoker(ConstUtility.crmURL);
                String resultJson = webPaiInvoker.InvokePostRequest("Loan", "GetBankLoanDetailList", dic).Result;
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<BankLoanDetailResult>(resultJson);
            }
            catch (Exception e)
            {
                throw new CustomInternetException("放款网络异常",e);
            }
            if (result.Code == 1 && result.Count > 0)
            {
                foreach (LoanDetailResult loanDetailResult in result.Data)
                {
                    try
                    {
                        //是否重复导入
                        var rlfLog = dbContext.RentLoanFinancialLogs.Where(rlfl => rlfl.Type == 0 && rlfl.GUID == loanDetailResult.GUID && rlfl.IsSuccess == true).FirstOrDefault();
                        if (rlfLog != null)
                        {
                            continue;
                        }

                        //获得合同信息
                        string rentLoanContInforSql = @"select ct.ID as ContractId,ct.Type as ContractType,ct.Status as ContractStatus,rm.StoreID,rm.ID as RoomId,cf.CharterMoney
                                        from dbo.Contracts as ct 
                                        left join dbo.ContractFees as cf on ct.ID = cf.ContractID
                                        left join dbo.Rooms as rm on ct.RoomID = rm.ID
                                        where ct.ContractNo = '" + loanDetailResult.ContractNO + "'";
                        RentLoanContInfor rentLoanContInfor = dbContext.Database.SqlQuery<RentLoanContInfor>(rentLoanContInforSql).FirstOrDefault();
                        if (rentLoanContInfor != null)
                        {
                            if (rentLoanContInfor.ContractStatus == 2 || rentLoanContInfor.ContractStatus == 4)
                            {
                                //记录异常，合同状态不是执行中
                                RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                                {
                                    GUID = loanDetailResult.GUID,
                                    CardNO = loanDetailResult.CardNO,
                                    CustomerCode = loanDetailResult.CustomerCode,
                                    ContractNO = loanDetailResult.ContractNO,
                                    LoanDate = loanDetailResult.LoanDate,
                                    Reason = "放款异常，合同状态不是待办合同",
                                    CreateDate = DateTime.Now,
                                    IsSuccess = false,
                                    Type = 0
                                };
                                dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                                dbContext.SaveChanges();
                                continue;
                            }

                            //判断合同类型
                            if (rentLoanContInfor.ContractType == 0)
                            {
                                //添加租金贷申请流程
                                string renterIdSql = @"select ID
                                         from dbo.Renters where SSN = '" + loanDetailResult.CardNO + "' and Status = 1";
                                var renterId = dbContext.Database.SqlQuery<int>(renterIdSql).FirstOrDefault();
                                if (renterId == 0)
                                {
                                    //记录异常，根据身份证号找不到用户
                                    RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                                    {
                                        GUID = loanDetailResult.GUID,
                                        CardNO = loanDetailResult.CardNO,
                                        CustomerCode = loanDetailResult.CustomerCode,
                                        ContractNO = loanDetailResult.ContractNO,
                                        LoanDate = loanDetailResult.LoanDate,
                                        Reason = "放款异常，根据身份证号找不到用户",
                                        CreateDate = DateTime.Now,
                                        IsSuccess = false,
                                        Type = 0
                                    };
                                    dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                                    dbContext.SaveChanges();
                                    continue;
                                }
                                //判断租金贷月还款是否和合同月租金一致
                                if ((loanDetailResult.LoanTotalMoney / loanDetailResult.LoanPeriods) != rentLoanContInfor.CharterMoney)
                                {
                                    //放款异常，放款月还款金额与合同月租金不一致
                                    RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                                    {
                                        GUID = loanDetailResult.GUID,
                                        CardNO = loanDetailResult.CardNO,
                                        CustomerCode = loanDetailResult.CustomerCode,
                                        ContractNO = loanDetailResult.ContractNO,
                                        LoanDate = loanDetailResult.LoanDate,
                                        Reason = "放款异常，放款月还款金额与合同月租金不一致",
                                        CreateDate = DateTime.Now,
                                        IsSuccess = false,
                                        Type = 0
                                    };
                                    dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                                    dbContext.SaveChanges();
                                    continue;
                                }
                                //判断是否是租金贷转现金合同
                                RentLoanAuditContract rLAContract = dbContext.RentLoanAuditContracts.Where(rlac => rlac.ContractID == rentLoanContInfor.ContractId).FirstOrDefault();
                                if (rLAContract != null)
                                {

                                    RentLoanAudit rentLoanAudit = dbContext.RentLoanAudits.Where(rla => rla.ID == rLAContract.RentLoanAuditID).First();
                                    if (dbContext.RentLoanAuditContracts.Where(rlac => rlac.RentLoanAuditID == rLAContract.RentLoanAuditID).Count() == 1)
                                    {
                                        if (rentLoanAudit.Status == 7)
                                        {
                                            //更新租金贷流程信息
                                            rentLoanAudit.Status = 4;
                                            rentLoanAudit.DateOfLoan = loanDetailResult.LoanDate;
                                            rentLoanAudit.AggregatePrice = loanDetailResult.LoanTotalMoney;
                                            rentLoanAudit.ApplyQuota = loanDetailResult.LoanTotalMoney;
                                            rentLoanAudit.TotalAmount = loanDetailResult.LoanTotalMoney;
                                            rentLoanAudit.DownPayment = loanDetailResult.LoanTotalMoney / loanDetailResult.LoanPeriods;
                                            rentLoanAudit.MonthlyTotalAmount = loanDetailResult.LoanTotalMoney / loanDetailResult.LoanPeriods;
                                            rentLoanAudit.Periods = loanDetailResult.LoanPeriods;
                                            rentLoanAudit.BankColumnID = 5;
                                            rentLoanAudit.StagesAndType = "ZJ";
                                            rentLoanAudit.BankCode = "9879";
                                            rentLoanAudit.RecomPersonCode = "11014309";
                                            rentLoanAudit.SalesManOrDp = "京东白条";
                                            rentLoanAudit.CityCode = "SH";
                                            rentLoanAudit.ActiveCode = "2099NN";
                                            rentLoanAudit.OtherDesc = null;
                                            rentLoanAudit.RenterID = renterId;
                                            rentLoanAudit.CreateDate = DateTime.Now;
                                            rentLoanAudit.CreateUserId = 0;
                                            //租金贷流程合同关联表状态更新
                                            rLAContract.ModifiedDate = DateTime.Now;
                                            rLAContract.ModifiedUserID = 0;
                                            //添加放款
                                            BankLoanDetail bankLoan = new BankLoanDetail()
                                            {
                                                RenterName = loanDetailResult.CustomerName,
                                                CertificateNo = loanDetailResult.CardNO,
                                                CardNo = "",
                                                Periods = loanDetailResult.LoanPeriods,
                                                TradeAmount = loanDetailResult.LoanTotalMoney,
                                                StoreID = rentLoanContInfor.StoreID,
                                                BankName = "微分期-京东白条",
                                                RentLoanAuditID = rentLoanAudit.ID,
                                                CounterFee = 0.0M,
                                                SettlementAmount = loanDetailResult.LoanTotalMoney,
                                                TradeDate = loanDetailResult.LoanDate,
                                                SettlementDate = loanDetailResult.LoanDate,
                                                MerchantName = "",
                                                MerchantNo = "",
                                                StatementDay=loanDetailResult.StatementDay
                                            };
                                            dbContext.BankLoanDetails.Add(bankLoan);

                                            rentLoanAudit.RentLoanAuditLogs.Add(new RentLoanAuditLog() { ModifiedDate = DateTime.Now, ModifiedUserId = 0, Desc = "创建申请", Status = 0 });
                                            rentLoanAudit.RentLoanAuditLogs.Add(new RentLoanAuditLog() { ModifiedDate = DateTime.Now, ModifiedUserId = 0, Desc = "", Status = 1 });
                                            rentLoanAudit.RentLoanAuditLogs.Add(new RentLoanAuditLog() { ModifiedDate = DateTime.Now, ModifiedUserId = 0, Desc = "", Status = 2 });

                                            //更新租金贷流程状态
                                            rentLoanAudit.Status = 4;
                                            rentLoanAudit.DateOfLoan = loanDetailResult.LoanDate;

                                            //更改合同状态为租金贷
                                            Contract contract = dbContext.Contracts.Where(ct => ct.ContractNo == loanDetailResult.ContractNO).First();
                                            contract.Type = 1;
                                            //增加ContractOps
                                            ContractOps contractOps = new ContractOps()
                                            {
                                                ContractID = contract.ID,
                                                CreateDate = DateTime.Now,
                                                CreateUserID = 0,
                                                OperationType = 31,
                                                Comment = "非租金贷转租金贷",
                                                CheckOutType = null
                                            };
                                            dbContext.ContractOps.Add(contractOps);

                                            //记录放款成功
                                            RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                                            {
                                                GUID = loanDetailResult.GUID,
                                                CardNO = loanDetailResult.CardNO,
                                                CustomerCode = loanDetailResult.CustomerCode,
                                                ContractNO = loanDetailResult.ContractNO,
                                                LoanDate = loanDetailResult.LoanDate,
                                                Type = 0,
                                                IsSuccess = true,
                                                CreateDate = DateTime.Now
                                            };
                                            dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                                            dbContext.SaveChanges();
                                        }
                                        else
                                        {
                                            //记录异常，租金贷流程状态不是异常终止
                                            RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                                            {
                                                GUID = loanDetailResult.GUID,
                                                CardNO = loanDetailResult.CardNO,
                                                CustomerCode = loanDetailResult.CustomerCode,
                                                ContractNO = loanDetailResult.ContractNO,
                                                LoanDate = loanDetailResult.LoanDate,
                                                Reason = "放款异常，合同是租金贷转现金合同，租金贷流程状态不是异常终止",
                                                Type = 0,
                                                IsSuccess = false,
                                                CreateDate = DateTime.Now
                                            };
                                            dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                                            dbContext.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        //记录异常，一个租金贷流程，两个合同
                                        RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                                        {
                                            GUID = loanDetailResult.GUID,
                                            CardNO = loanDetailResult.CardNO,
                                            CustomerCode = loanDetailResult.CustomerCode,
                                            ContractNO = loanDetailResult.ContractNO,
                                            LoanDate = loanDetailResult.LoanDate,
                                            Reason = "放款异常，合同是租金贷转现金合同，一个租金贷流程含有两个合同",
                                            Type = 0,
                                            IsSuccess = false,
                                            CreateDate = DateTime.Now
                                        };
                                        dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                                        dbContext.SaveChanges();
                                    }
                                }
                                else
                                {
                                    RentLoanAudit rentLoanAudit = new RentLoanAudit()
                                    {
                                        AggregatePrice = loanDetailResult.LoanTotalMoney,
                                        ApplyQuota = loanDetailResult.LoanTotalMoney,
                                        TotalAmount = loanDetailResult.LoanTotalMoney,
                                        DownPayment = loanDetailResult.LoanTotalMoney / loanDetailResult.LoanPeriods,
                                        MonthlyTotalAmount = loanDetailResult.LoanTotalMoney / loanDetailResult.LoanPeriods,
                                        Periods = loanDetailResult.LoanPeriods,
                                        BankColumnID = 5,
                                        StagesAndType = "ZJ",
                                        BankCode = "9879",
                                        RecomPersonCode = "11014309",
                                        SalesManOrDp = "京东白条",
                                        CityCode = "SH",
                                        ActiveCode = "2099NN",
                                        OtherDesc = null,
                                        RenterID = renterId,
                                        CreateDate = DateTime.Now,
                                        CreateUserId = 0,
                                    };
                                    rentLoanAudit.RentLoanAuditContracts = new List<RentLoanAuditContract>();
                                    //增加租金贷合同关联表
                                    RentLoanAuditContract rentLoanAuditContract = new RentLoanAuditContract()
                                    {
                                        ContractID = rentLoanContInfor.ContractId,
                                        CreateDate = DateTime.Now,
                                        CreateUserID = 0,
                                        StoreID = rentLoanContInfor.StoreID,
                                        RoomID = rentLoanContInfor.RoomId,
                                        Status = 1
                                    };
                                    rentLoanAudit.RentLoanAuditContracts.Add(rentLoanAuditContract);
                                    rentLoanAudit.RentLoanAuditLogs = new List<RentLoanAuditLog>();
                                    rentLoanAudit.RentLoanAuditLogs.Add(new RentLoanAuditLog() { ModifiedDate = DateTime.Now, ModifiedUserId = 0, Desc = "创建申请", Status = 0 });
                                    rentLoanAudit.RentLoanAuditLogs.Add(new RentLoanAuditLog() { ModifiedDate = DateTime.Now, ModifiedUserId = 0, Desc = "", Status = 1 });
                                    rentLoanAudit.RentLoanAuditLogs.Add(new RentLoanAuditLog() { ModifiedDate = DateTime.Now, ModifiedUserId = 0, Desc = "", Status = 2 });
                                    dbContext.RentLoanAudits.Add(rentLoanAudit);
                                    BankLoanDetail bankLoan = new BankLoanDetail()
                                    {
                                        RenterName = loanDetailResult.CustomerName,
                                        CertificateNo = loanDetailResult.CardNO,
                                        CardNo = "",
                                        Periods = loanDetailResult.LoanPeriods,
                                        TradeAmount = loanDetailResult.LoanTotalMoney,
                                        StoreID = rentLoanContInfor.StoreID,
                                        BankName = "微分期-京东白条",
                                        rentLoanAudit = rentLoanAudit,
                                        CounterFee = 0.0M,
                                        SettlementAmount = loanDetailResult.LoanTotalMoney,
                                        TradeDate = loanDetailResult.LoanDate,
                                        SettlementDate = loanDetailResult.LoanDate,
                                        MerchantName = "",
                                        MerchantNo = "",
                                        StatementDay = loanDetailResult.StatementDay
                                    };
                                    dbContext.BankLoanDetails.Add(bankLoan);
                                    //更新租金贷流程状态
                                    rentLoanAudit.Status = 4;
                                    rentLoanAudit.DateOfLoan = loanDetailResult.LoanDate;

                                    //更改合同状态为租金贷
                                    Contract contract = dbContext.Contracts.Where(ct => ct.ContractNo == loanDetailResult.ContractNO).First();
                                    contract.Type = 1;
                                    //增加ContractOps
                                    ContractOps contractOps = new ContractOps()
                                    {
                                        ContractID = contract.ID,
                                        CreateDate = DateTime.Now,
                                        CreateUserID = 0,
                                        OperationType = 31,
                                        Comment = "非租金贷转租金贷",
                                        CheckOutType = null
                                    };
                                    dbContext.ContractOps.Add(contractOps);

                                    //记录放款成功
                                    RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                                    {
                                        GUID = loanDetailResult.GUID,
                                        CardNO = loanDetailResult.CardNO,
                                        CustomerCode = loanDetailResult.CustomerCode,
                                        ContractNO = loanDetailResult.ContractNO,
                                        Type = 0,
                                        IsSuccess = true,
                                        CreateDate = DateTime.Now,
                                        LoanDate = loanDetailResult.LoanDate
                                    };
                                    dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                                    dbContext.SaveChanges();

                                }
                            }
                            else
                            {
                                //记录异常，异常信息是已是租金贷合同
                                RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                                {
                                    GUID = loanDetailResult.GUID,
                                    CardNO = loanDetailResult.CardNO,
                                    CustomerCode = loanDetailResult.CustomerCode,
                                    ContractNO = loanDetailResult.ContractNO,
                                    LoanDate = loanDetailResult.LoanDate,
                                    Reason = "放款异常，合同已是租金贷合同",
                                    IsSuccess = false,
                                    CreateDate = DateTime.Now,
                                    Type = 0
                                };
                                dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                                dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            //记录异常，找不到合同信息
                            RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                            {
                                GUID = loanDetailResult.GUID,
                                CardNO = loanDetailResult.CardNO,
                                CustomerCode = loanDetailResult.CustomerCode,
                                ContractNO = loanDetailResult.ContractNO,
                                LoanDate = loanDetailResult.LoanDate,
                                Reason = "放款异常，找不到合同信息",
                                IsSuccess = false,
                                CreateDate = DateTime.Now,
                                Type = 0
                            };
                            dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                            dbContext.SaveChanges();
                        }
                    }
                    catch (Exception e)
                    {
                        //记录异常
                        RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                        {
                            GUID = loanDetailResult.GUID,
                            CardNO = loanDetailResult.CardNO,
                            CustomerCode = loanDetailResult.CustomerCode,
                            ContractNO = loanDetailResult.ContractNO,
                            LoanDate= loanDetailResult.LoanDate,
                            Reason = "放款异常，"+e.ToString(),
                            IsSuccess = false,
                            CreateDate = DateTime.Now,
                            Type = 0
                        };
                        dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                        dbContext.SaveChanges();
                    }
                }

            }

        }

        public void UploadRentLoanPool()
        {
            //调用CRM接口
            DateTime StartDate = DateTime.Now.Date;
            //根据Log文件，找到出错时间，重新拉取
            string pullTimeSql = @"select  CAST(CreateDate  as date) as CreateDate
							from (select GUID,convert(varchar(10),CreateDate,23) as CreateDate	
                            from dbo.RentLoanFinancialLogs
                            where IsSuccess = 0 and Type = 1
                            group by GUID,convert(varchar(10),CreateDate,23)) as a
                            left join (select GUID 
                            from dbo.RentLoanFinancialLogs
                            where IsSuccess = 1 and Type = 1
                            group by GUID
                            ) as b ON a.GUID = b.GUID 
                            where b.GUID is null
                            order by CreateDate";
            var pullTime = dbContext.Database.SqlQuery<DateTime>(pullTimeSql).FirstOrDefault();
            if (pullTime != default(DateTime))
            {
                StartDate = pullTime.Date;
            }
            //网络异常
            RentLoanFinancialLog InternetExceptionFinLog = dbContext.RentLoanFinancialLogs.Where(rlfl => rlfl.Type == 3 && rlfl.IsSuccess == false).OrderBy(rlfl => rlfl.CreateDate).FirstOrDefault();
            if (InternetExceptionFinLog != null)
            {
                if (InternetExceptionFinLog.CreateDate.Date < StartDate)
                {
                    StartDate = InternetExceptionFinLog.CreateDate.Date;
                }
            }
            //判断手动设置拉取数据开始时间与StartDate对比
            //if (Convert.ToDateTime(ConstUtility.PullStartDateTime) < StartDate)
            //{
            //    StartDate = Convert.ToDateTime(ConstUtility.PullStartDateTime);
            //}
            pullStartTimes.Add(StartDate);
            DateTime EndDate = DateTime.Now.Date.AddDays(1);
            RentLoanPoolResult result = null;
            try
            {
                Dictionary<String, String> dic = new Dictionary<string, string>();
                dic.Add("ApiUserId", ConstUtility.APIUSERID);
                dic.Add("Signature", Encrypt.MD5Encrypt(ConstUtility.APIUSERID + ConstUtility.SIGNATURE + DateTime.Now.ToString("yyyyMMdd")));
                dic.Add("StartDate", StartDate.ToString());
                dic.Add("EndDate", EndDate.ToString());
                dic.Add("PageIndex", "1");
                dic.Add("PageSize", "10000");
                WebApiInvoker webPaiInvoker = new WebApiInvoker(ConstUtility.crmURL);
                String resultJson = webPaiInvoker.InvokePostRequest("Loan", "GetRenterLoanDetailList", dic).Result;
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<RentLoanPoolResult>(resultJson);
            }
            catch (Exception e)
            {
                throw new CustomInternetException("还款网络异常", e);
            }
            if (result.Code == 1 && result.Count > 0)
            {
                foreach (LoanPoolResult rentLoanPoolResult in result.Data)
                {
                    try
                    {
                        //是否重复导入
                        var rlfLog = dbContext.RentLoanFinancialLogs.Where(rlfl => rlfl.Type == 1 && rlfl.GUID == rentLoanPoolResult.GUID && rlfl.IsSuccess == true).FirstOrDefault();
                        if (rlfLog != null)
                        {
                            continue;
                        }

                        //得到合同信息
                        string rentLoanContInforSql = @"select ct.ID as ContractId,ct.Type as ContractType,ct.Status as ContractStatus,rm.StoreID,rm.ID as RoomId,cf.CharterMoney
                                        from dbo.Contracts as ct 
                                        left join dbo.ContractFees as cf on ct.ID = cf.ContractID
                                        left join dbo.Rooms as rm on ct.RoomID = rm.ID
                                        where ct.ContractNo = '" + rentLoanPoolResult.ContractNO + "'";
                        RentLoanContInfor rentLoanContInfor = dbContext.Database.SqlQuery<RentLoanContInfor>(rentLoanContInforSql).FirstOrDefault();
                        if (rentLoanContInfor == null)
                        {
                            //记录异常，找不到合同信息
                            RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                            {
                                GUID = rentLoanPoolResult.GUID,
                                CardNO = rentLoanPoolResult.CardNO,
                                CustomerCode = rentLoanPoolResult.CustomerCode,
                                ContractNO = rentLoanPoolResult.ContractNO,
                                LoanDate = rentLoanPoolResult.StatementDay,
                                Reason = "还款异常，找不到合同信息",
                                Type = 1,
                                IsSuccess = false,
                                CreateDate = DateTime.Now
                            };
                            dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                            dbContext.SaveChanges();
                            continue;
                        }
                        RentLoanAuditContract rentLoanAuditContract = dbContext.RentLoanAuditContracts.Where(rlac => rlac.ContractID == rentLoanContInfor.ContractId).FirstOrDefault();
                        if (rentLoanAuditContract == null)
                        {
                            //记录异常，异常信息是没有租金贷合同关联表信息
                            RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                            {
                                GUID = rentLoanPoolResult.GUID,
                                CardNO = rentLoanPoolResult.CardNO,
                                CustomerCode = rentLoanPoolResult.CustomerCode,
                                ContractNO = rentLoanPoolResult.ContractNO,
                                LoanDate = rentLoanPoolResult.StatementDay,
                                Reason = "还款异常，没有租金贷合同关联表信息",
                                Type = 1,
                                IsSuccess = false,
                                CreateDate = DateTime.Now
                            };
                            dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                            dbContext.SaveChanges();
                            continue;
                        }
                        RentLoanAudit rentLoanAudit = dbContext.RentLoanAudits.Where(rla => rla.ID == rentLoanAuditContract.RentLoanAuditID && rla.Status > 3 && rla.Status != 10).FirstOrDefault();
                        if (rentLoanAudit == null)
                        {
                            //记录异常，异常信息是没有租金贷流程信息
                            RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                            {
                                GUID = rentLoanPoolResult.GUID,
                                CardNO = rentLoanPoolResult.CardNO,
                                CustomerCode = rentLoanPoolResult.CustomerCode,
                                ContractNO = rentLoanPoolResult.ContractNO,
                                LoanDate = rentLoanPoolResult.StatementDay,
                                Reason = "还款异常，没有租金贷流程信息",
                                Type = 1,
                                IsSuccess = false,
                                CreateDate = DateTime.Now
                            };
                            dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                            dbContext.SaveChanges();
                            continue;
                        }
                        if (rentLoanAudit.MonthlyTotalAmount != rentLoanPoolResult.LoanMoney)
                        {
                            //记录异常，还款金融与租金贷流程记录金额不一致
                            RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                            {
                                GUID = rentLoanPoolResult.GUID,
                                CardNO = rentLoanPoolResult.CardNO,
                                CustomerCode = rentLoanPoolResult.CustomerCode,
                                ContractNO = rentLoanPoolResult.ContractNO,
                                LoanDate = rentLoanPoolResult.StatementDay,
                                Reason = "还款异常，还款金融与租金贷流程记录金额不一致",
                                Type = 1,
                                IsSuccess = false,
                                CreateDate = DateTime.Now
                            };
                            dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                            dbContext.SaveChanges();
                            continue;
                        }

                        //根据账单日，判断重复导入异常
                        var RlflStatementDay = dbContext.RentLoanFinancialLogs.Where(rlfl => rlfl.Type == 1 && rlfl.IsSuccess == true && rlfl.LoanDate == rentLoanPoolResult.StatementDay && rlfl.ContractNO == rentLoanPoolResult.ContractNO).FirstOrDefault();
                        if (RlflStatementDay != null)
                        {
                            //记录异常，相同账单日重复导入
                            RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                            {
                                GUID = rentLoanPoolResult.GUID,
                                CardNO = rentLoanPoolResult.CardNO,
                                CustomerCode = rentLoanPoolResult.CustomerCode,
                                ContractNO = rentLoanPoolResult.ContractNO,
                                LoanDate = rentLoanPoolResult.StatementDay,
                                Reason = "还款异常，相同账单日重复导入",
                                Type = 1,
                                IsSuccess = false,
                                CreateDate = DateTime.Now
                            };
                            dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                            dbContext.SaveChanges();
                            continue;
                        }

                        //判断期数是否异常
                        if (rentLoanAudit.AlreadyAlsoPeriod >= rentLoanPoolResult.LoanPeriods)
                        {
                            
                            RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                            {
                                GUID = rentLoanPoolResult.GUID,
                                CardNO = rentLoanPoolResult.CardNO,
                                CustomerCode = rentLoanPoolResult.CustomerCode,
                                ContractNO = rentLoanPoolResult.ContractNO,
                                LoanDate = rentLoanPoolResult.StatementDay,
                                Reason = "还款异常，还款期数小于或等于已还期数",
                                Type = 1,
                                IsSuccess = false,
                                CreateDate = DateTime.Now
                            };
                            dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                            dbContext.SaveChanges();
                            continue;
                        }

                        RentLoanPool rentLoanPool = new RentLoanPool()
                        {
                            RenterName = rentLoanPoolResult.CustomerName,
                            CertificateNo = rentLoanPoolResult.CardNO,
                            SystemDate = rentLoanPoolResult.StatementDay,
                            ApprovalAmount = rentLoanAudit.AggregatePrice.Value,
                            Periods = rentLoanAudit.Periods.Value,
                            CurrentPrincipal = rentLoanContInfor.CharterMoney,
                            RepaymentAmount = rentLoanPoolResult.LoanMoney,
                            RepaymentTime = rentLoanPoolResult.LoanDate,
                            BankName = "微分期-京东白条",
                            CreateDate = DateTime.Now,
                            CreateUserId = 0,
                            StoreID = rentLoanContInfor.StoreID
                        };
                        dbContext.RentLoanPools.Add(rentLoanPool);

                        rentLoanAudit.Status = 5;

                        RentLoanFinancialLog rentLoanFinancialLogSuccess = new RentLoanFinancialLog()
                        {
                            GUID = rentLoanPoolResult.GUID,
                            CardNO = rentLoanPoolResult.CardNO,
                            CustomerCode = rentLoanPoolResult.CustomerCode,
                            ContractNO = rentLoanPoolResult.ContractNO,
                            LoanDate = rentLoanPoolResult.StatementDay,
                            Type = 1,
                            IsSuccess = true,
                            CreateDate = DateTime.Now
                        };
                        dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLogSuccess);

                        dbContext.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        //记录异常
                        RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                        {
                            GUID = rentLoanPoolResult.GUID,
                            CardNO = rentLoanPoolResult.CardNO,
                            CustomerCode = rentLoanPoolResult.CustomerCode,
                            ContractNO = rentLoanPoolResult.ContractNO,
                            LoanDate = rentLoanPoolResult.StatementDay,
                            Reason = "还款异常，"+e.ToString(),
                            Type = 1,
                            IsSuccess = false,
                            CreateDate = DateTime.Now
                        };
                        dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                        dbContext.SaveChanges();
                    }
                }
            }
        }


        public void OverdueDetailList()
        {
            //调用CRM接口
            DateTime StartDate = DateTime.Now.Date;
            //根据Log文件，找到出错时间，重新拉取
            string pullTimeSql = @"select  CAST(CreateDate  as date) as CreateDate
							from (select GUID,convert(varchar(10),CreateDate,23) as CreateDate	
                            from dbo.RentLoanFinancialLogs
                            where IsSuccess = 0 and Type = 2
                            group by GUID,convert(varchar(10),CreateDate,23)) as a
                            left join (select GUID 
                            from dbo.RentLoanFinancialLogs
                            where IsSuccess = 1 and Type = 2
                            group by GUID
                            ) as b ON a.GUID = b.GUID 
                            where b.GUID is null
                            order by CreateDate";
            var pullTime = dbContext.Database.SqlQuery<DateTime>(pullTimeSql).FirstOrDefault();
            if (pullTime != default(DateTime))
            {
                StartDate = pullTime.Date;
            }
            //网络异常
            RentLoanFinancialLog InternetExceptionFinLog = dbContext.RentLoanFinancialLogs.Where(rlfl => rlfl.Type == 3 && rlfl.IsSuccess == false).OrderBy(rlfl => rlfl.CreateDate).FirstOrDefault();
            if (InternetExceptionFinLog != null)
            {
                if (InternetExceptionFinLog.CreateDate.Date < StartDate)
                {
                    StartDate = InternetExceptionFinLog.CreateDate.Date;
                }
            }
            //判断手动设置拉取数据开始时间与StartDate对比
            //if (Convert.ToDateTime(ConstUtility.PullStartDateTime) < StartDate)
            //{
            //    StartDate = Convert.ToDateTime(ConstUtility.PullStartDateTime);
            //}
            pullStartTimes.Add(StartDate);
            DateTime EndDate = DateTime.Now.Date.AddDays(1);
            OverdueDetailResult result = null;
            try
            {
                Dictionary<String, String> dic = new Dictionary<string, string>();
                dic.Add("ApiUserId", ConstUtility.APIUSERID);
                dic.Add("Signature", Encrypt.MD5Encrypt(ConstUtility.APIUSERID + ConstUtility.SIGNATURE + DateTime.Now.ToString("yyyyMMdd")));
                dic.Add("StartDate", StartDate.ToString());
                dic.Add("EndDate", EndDate.ToString());
                dic.Add("PageIndex", "1");
                dic.Add("PageSize", "10000");
                WebApiInvoker webPaiInvoker = new WebApiInvoker(ConstUtility.crmURL);
                String resultJson = webPaiInvoker.InvokePostRequest("Loan", "GetOverdueDetailList", dic).Result;
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<OverdueDetailResult>(resultJson);
            }
            catch (Exception e)
            {
                throw new CustomInternetException("逾期网络异常", e);
            }
            if (result.Code == 1 && result.Count > 0)
            {
                foreach (OverdueDetail overdueDetail in result.Data)
                {
                    //是否重复导入
                    var rlfLog = dbContext.RentLoanFinancialLogs.Where(rlfl => rlfl.Type == 2 && rlfl.GUID == overdueDetail.GUID && rlfl.IsSuccess == true).FirstOrDefault();
                    if (rlfLog != null)
                    {
                        continue;
                    }
                    try
                    {
                        //得到合同信息
                        string rentLoanContInforSql = @"select ct.ID as ContractId,ct.Type as ContractType,ct.Status as ContractStatus,rm.StoreID,rm.ID as RoomId,cf.CharterMoney
                                        from dbo.Contracts as ct 
                                        left join dbo.ContractFees as cf on ct.ID = cf.ContractID
                                        left join dbo.Rooms as rm on ct.RoomID = rm.ID
                                        where ct.ContractNo = '" + overdueDetail.ContractNO + "'";
                        RentLoanContInfor rentLoanContInfor = dbContext.Database.SqlQuery<RentLoanContInfor>(rentLoanContInforSql).FirstOrDefault();
                        if (rentLoanContInfor == null)
                        {
                            //记录异常，找不到合同信息
                            RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                            {
                                GUID = overdueDetail.GUID,
                                CardNO = overdueDetail.CardNO,
                                CustomerCode = overdueDetail.CustomerCode,
                                ContractNO = overdueDetail.ContractNO,
                                Reason = "逾期异常，找不到合同信息",
                                Type = 2,
                                IsSuccess = false,
                                CreateDate = DateTime.Now
                            };
                            dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                            dbContext.SaveChanges();
                            continue;
                        }
                        RentLoanAuditContract rentLoanAuditContract = dbContext.RentLoanAuditContracts.Where(rlac => rlac.ContractID == rentLoanContInfor.ContractId).FirstOrDefault();
                        if (rentLoanAuditContract == null)
                        {
                            //记录异常，找不到租金贷流程合同关联表信息
                            RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                            {
                                GUID = overdueDetail.GUID,
                                CardNO = overdueDetail.CardNO,
                                CustomerCode = overdueDetail.CustomerCode,
                                ContractNO = overdueDetail.ContractNO,
                                Reason = "逾期异常，找不到租金贷流程合同关联表信息",
                                Type = 2,
                                IsSuccess = false,
                                CreateDate = DateTime.Now
                            };
                            dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                            dbContext.SaveChanges();
                            continue;
                        }
                        RentLoanAudit rentLoanAudit = dbContext.RentLoanAudits.Where(rla => rla.ID == rentLoanAuditContract.RentLoanAuditID).FirstOrDefault();
                        if (rentLoanAudit == null)
                        {
                            //记录异常，找不到租金贷流程
                            RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                            {
                                GUID = overdueDetail.GUID,
                                CardNO = overdueDetail.CardNO,
                                CustomerCode = overdueDetail.CustomerCode,
                                ContractNO = overdueDetail.ContractNO,
                                Reason = "逾期异常，找不到租金贷流程",
                                Type = 2,
                                IsSuccess = false,
                                CreateDate = DateTime.Now
                            };
                            dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                            dbContext.SaveChanges();
                            continue;
                        }
                        //更新状态
                        rentLoanAudit.Status = 9;
                        RentLoanFinancialLog rentLoanFinancialSuccessLog = new RentLoanFinancialLog()
                        {
                            GUID = overdueDetail.GUID,
                            CardNO = overdueDetail.CardNO,
                            CustomerCode = overdueDetail.CustomerCode,
                            ContractNO = overdueDetail.ContractNO,
                            Type = 2,
                            IsSuccess = true,
                            CreateDate = DateTime.Now
                        };
                        dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialSuccessLog);
                        dbContext.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        //记录异常
                        RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                        {
                            GUID = overdueDetail.GUID,
                            CardNO = overdueDetail.CardNO,
                            CustomerCode = overdueDetail.CustomerCode,
                            ContractNO = overdueDetail.ContractNO,
                            Reason = "逾期异常，"+e.ToString(),
                            Type = 2,
                            IsSuccess = false,
                            CreateDate = DateTime.Now
                        };
                        dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        public void PullRentLoanFinancialData()
        {
            try
            {
                BankLoanDetail();
                UploadRentLoanPool();
                OverdueDetailList();
                //得到放款、还款、逾期每个接口开始时间取最小值
                pullStartTimes.Sort();
                DateTime startTime = pullStartTimes[0];
                var InternetExceptionLogs = dbContext.RentLoanFinancialLogs.Where(rlfl => rlfl.Type ==3 && rlfl.IsSuccess == false && rlfl.CreateDate >= startTime).ToList();
                if (InternetExceptionLogs.Count != 0)
                {
                    InternetExceptionLogs.ForEach(iel => iel.IsSuccess = true);
                    dbContext.SaveChanges();
                }

            }
            catch (CustomInternetException e)
            {
                DateTime StartTime = DateTime.Now;
                //记录网络异常
                RentLoanFinancialLog rentLoanFinancialLog = new RentLoanFinancialLog()
                {
                    GUID = StartTime.Year.ToString() + StartTime.Month.ToString() + StartTime.Day.ToString(),
                    CardNO = "",
                    CustomerCode = "",
                    ContractNO = "",
                    Reason = "网络异常，" + e.ToString(),
                    Type = 3,
                    IsSuccess = false,
                    CreateDate = DateTime.Now
                };
                dbContext.RentLoanFinancialLogs.Add(rentLoanFinancialLog);
                dbContext.SaveChanges();
            }
        }
    }
}
