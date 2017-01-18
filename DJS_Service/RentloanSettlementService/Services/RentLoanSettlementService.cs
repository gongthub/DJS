using OpsModel.Charge;
using RentloanSettlementService.Utils;
using RentloanSettlementService.ContractSpace;
using RentloanSettlementService.PaymentSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterElectricService.Model.RentLoan;
using RentloanSettlementService.Common;

namespace RentloanSettlementService.Services
{
    public class RentLoanSettlementService
    {
        EFDbContext dbContext = new EFDbContext();

        public static DJS.SDK.ILog iLog = null;


        #region 构造函数

        static RentLoanSettlementService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
        }

        #endregion

        /// <summary>
        /// 租金贷结算服务
        /// </summary>
        public void SettlementService()
        {
            iLog.WriteLog("租金贷结算服务运行开始", 0);

            SettlementAction();

            RentLoanAuditChangeStatus();

            iLog.WriteLog("租金贷结算服务运行结束", 0);
        }
        /// <summary>
        /// 租金贷结算
        /// </summary>
        public void SettlementAction()
        {
            iLog.WriteLog("租金贷结算程序运行开始", 0);

            // 租金贷审批流程数据
            List<RentLoanAudit> rentLoanAudits = ReadRentLoanAuditInfo();

            if (rentLoanAudits.Count == 0)
            {
                iLog.WriteLog("未找到符合条件租金贷审批流程", 0);
                iLog.WriteLog("租金贷结算程序运行结束", 0);
                return;
            }

            // 结算缓存池数据
            List<RentLoanPool> rentLoanPools = ReadRentLoanPool();
            // 合同数据
            List<Contract> contracts = ReadContract();
            // 合同费用数据
            List<ContractFee> contractFees = ReadContractFee();
            // 结算银行信息
            List<BankColumn> bankColumns = ReadBankColumn();
            // 历史结算信息
            List<RentLoanHistoryPool> rentLoanHistoryPools = ReadRentHistoryLoanPool();

            dbContext.Configuration.AutoDetectChangesEnabled = false;

            string serialNo = ((byte)CommonEnum.PayMethodTypeSource.租金贷).ToString("00") + DateTime.Now.ToString("yyyyMMdd");
            int payCount = -1;

            List<PeriodicCharge> ListPeriodicCharge = new List<PeriodicCharge>();

            List<Charge> ListCharge = new List<Charge>();

            try
            {
                for (int i = 0; i < rentLoanAudits.Count; i++)
                {
                    var rlAuditId = rentLoanAudits[i].ID;
                    var rlAuditContracts = dbContext.RentLoanAuditContracts.Where(t => t.RentLoanAuditID == rlAuditId).OrderByDescending(t => t.ID).FirstOrDefault();
                    var rlStoreId = rlAuditContracts != null ? rlAuditContracts.StoreID : 0;

                    List<RentLoanPool> tempRentLoanPools = rentLoanPools.Where(p => p.CertificateNo == rentLoanAudits[i].Renter.SSN
                        && p.RenterName == rentLoanAudits[i].Renter.Name
                        && rentLoanAudits[i].BankColumn != null
                        && p.BankName != null
                        && (rentLoanAudits[i].BankColumn.BankName.Trim() + "-" + rentLoanAudits[i].BankColumn.SalesManOrDp.Trim())
                        .Equals(p.BankName.Trim())
                        && p.StoreID != null
                        && p.StoreID == rlStoreId
                        ).ToList<RentLoanPool>();

                    if (tempRentLoanPools.Count > 0)
                    {
                        //RentLoanPool rentLoanPool = tempRentLoanPools.FirstOrDefault<RentLoanPool>();

                        foreach (var PoolItem in tempRentLoanPools)
                        {
                            RentLoanPool rentLoanPool = PoolItem;

                            RentLoanHistoryPool rentLoanHistoryPool = new RentLoanHistoryPool();
                            rentLoanHistoryPool.RenterName = rentLoanPool.RenterName;
                            rentLoanHistoryPool.CertificateNo = rentLoanPool.CertificateNo;
                            rentLoanHistoryPool.SystemDate = rentLoanPool.SystemDate;
                            rentLoanHistoryPool.ApprovalAmount = rentLoanPool.ApprovalAmount;
                            rentLoanHistoryPool.AggregatePrice = rentLoanPool.AggregatePrice;
                            rentLoanHistoryPool.Periods = rentLoanPool.Periods;
                            rentLoanHistoryPool.CurrentPrincipal = rentLoanPool.CurrentPrincipal;
                            rentLoanHistoryPool.RepaymentAmount = rentLoanPool.RepaymentAmount;
                            rentLoanHistoryPool.RepaymentTime = rentLoanPool.RepaymentTime;
                            rentLoanHistoryPool.BankName = rentLoanPool.BankName;
                            rentLoanHistoryPool.CreateDate = DateTime.Now;
                            rentLoanHistoryPool.CreateUserId = rentLoanPool.CreateUserId;
                            rentLoanHistoryPool.StoreID = rentLoanPool.StoreID;

                            dbContext.RentLoanHistoryPools.Add(rentLoanHistoryPool);
                            iLog.WriteLog("生成租金贷结算缓存历史数据，租客：" + rentLoanHistoryPool.CertificateNo, 0);

                            // 正常还款、生成费用记录
                            // rentLoanAudits[i].Status = (byte)CommonEnum.RentLoanStatus.正常还款;
                            int alreadyAlsoPeriod = rentLoanAudits[i].AlreadyAlsoPeriod == null ? 0 : rentLoanAudits[i].AlreadyAlsoPeriod.Value;
                            rentLoanAudits[i].AlreadyAlsoPeriod = alreadyAlsoPeriod + 1;

                            //DateTime tempLastFeeDate = new DateTime();

                            //iLog.WriteLog("设置状态为正在还款，已还期数：" + rentLoanAudits[i].AlreadyAlsoPeriod + " 租客：" + rentLoanHistoryPool.CertificateNo);

                            List<Contract> tempContracts = contracts.Where(p => p.RenterID == rentLoanAudits[i].RenterID && p.Type == 1).ToList<Contract>();

                            iLog.WriteLog("合同号数量：" + tempContracts.Count, 0);

                            foreach (var item in tempContracts)
                            {
                                iLog.WriteLog("合同号：" + item.ContractNo + " 结算开始 ", 0);

                                ContractFee contractFee = contractFees.Where(p => p.ContractID == item.ID).ToList<ContractFee>().FirstOrDefault<ContractFee>();

                                iLog.WriteLog("合同号开始日期：" + item.StartDate + " 合同号结束日期: " + item.EndDate + " 租金:" + contractFee.CharterMoney, 0);

                                decimal dayRent = GetDayRent(item.StartDate, item.EndDate, contractFee.CharterMoney);

                                iLog.WriteLog("日租金: " + dayRent, 0);

                                decimal TotalAmount = 0;

                                //List<PeriodicCharge> PeriodicCharges = dbContext.PeriodicCharges.Where(c => c.ContractID == item.ID && c.Status == 1).OrderBy(p=>p.EndDate descending).ToList();
                                List<PeriodicCharge> PeriodicCharges = (from r in dbContext.PeriodicCharges
                                                                        where r.ContractID == item.ID
                                                                           && r.Status == 1
                                                                        orderby r.EndDate descending
                                                                        select r).ToList();

                                if (PeriodicCharges.Count > 0)
                                {
                                    TotalAmount = PeriodicCharges.Sum(c => c.Amount);
                                }

                                List<PeriodicCharge> tempPeriodicCharges = (from r in ListPeriodicCharge
                                                                            where r.ContractID == item.ID
                                                                            orderby r.EndDate descending
                                                                            select r).ToList();
                                if (tempPeriodicCharges.Count > 0)
                                {
                                    TotalAmount = TotalAmount + tempPeriodicCharges.Sum(c => c.Amount);
                                }

                                iLog.WriteLog("已付房租: " + TotalAmount, 0);

                                DateTime tempStartDate = item.StartDate;
                                int bDayCount = Math.Abs(int.Parse(Math.Round(TotalAmount / dayRent, 0).ToString()));

                                if (bDayCount > 0)
                                {
                                    tempStartDate = item.StartDate.AddDays(bDayCount);
                                }

                                int dayCount = Math.Abs(int.Parse(Math.Round((TotalAmount + contractFee.CharterMoney) / dayRent, 0).ToString()));

                                DateTime tempEndDate = item.StartDate.AddDays(-1).AddDays(dayCount);

                                if (payCount == -1) payCount = FirstPaySerialNo(serialNo);

                                // 保存支付主表数据
                                PaymentMethod paymentMethod = new PaymentMethod();
                                paymentMethod.Date = rentLoanPool.RepaymentTime;
                                paymentMethod.PayType = (byte)CommonEnum.PayMethodTypeSource.租金贷;
                                paymentMethod.CreateUserID = rentLoanAudits[i].CreateUserId;
                                paymentMethod.CreateDate = DateTime.Now;
                                paymentMethod.TotalAmount = contractFee.CharterMoney;
                                paymentMethod.Status = (byte)CommonEnum.PaymentStatus.已付款;
                                paymentMethod.PaySerialNo = GeneratePaySerialNo(serialNo, payCount + i);

                                // 保存支付子表数据
                                PaymentDetails paymentDetials = new PaymentDetails();
                                paymentDetials.Amount = contractFee.CharterMoney;
                                paymentDetials.Type = (byte)CommonEnum.PaymentDetailsType.RentLoan;
                                paymentDetials.SerialNo = GeneratePaySerialNo(serialNo, payCount + i);
                                paymentDetials.PaymentMethod = paymentMethod;

                                iLog.WriteLog("生成支付数据，合同号：" + item.ContractNo + " 租客：" + rentLoanHistoryPool.CertificateNo + " 支付金额：" + paymentMethod.TotalAmount + " 流水号：" + paymentMethod.PaySerialNo, 0);

                                PeriodicCharge periodicCharge = new PeriodicCharge();
                                PeriodicChargeDetail periodicChargeDetail = new PeriodicChargeDetail();
                                Charge charge = new Charge();
                                ChargeDetail chargeDetail = new ChargeDetail();
                                if (rentLoanAudits[i].Status != 7)
                                {

                                    periodicCharge.Contract = item;
                                    periodicCharge.StartDate = tempStartDate;
                                    periodicCharge.EndDate = tempEndDate;
                                    periodicCharge.Amount = contractFee.CharterMoney;
                                    periodicCharge.FeeType = (byte)CommonEnum.PeriodicFeeType.租金;
                                    periodicCharge.PayDate = rentLoanPool.RepaymentTime;
                                    periodicCharge.PaymentMethod = paymentMethod;
                                    periodicCharge.ReceivableAmount = contractFee.CharterMoney;

                                    ListPeriodicCharge.Add(periodicCharge);

                                    periodicChargeDetail.PeriodicCharge = periodicCharge;
                                    periodicChargeDetail.Name = "租金";
                                    periodicChargeDetail.UnitPrice = contractFee.CharterMoney;
                                    periodicChargeDetail.Quantity = 1;
                                    periodicChargeDetail.Unit = CommonEnum.Unit_Month;
                                    periodicChargeDetail.Type = (byte)CommonEnum.PeriodicFeeType.租金;
                                    periodicChargeDetail.ProductType = (byte)CommonEnum.ChargeProductType.产品;
                                    periodicChargeDetail.ProductCode = "131001000001";

                                    iLog.WriteLog("生成收费数据，合同号：" + item.ContractNo + " 租客：" + rentLoanHistoryPool.CertificateNo + " 收费金额：" + periodicCharge.Amount + " 覆盖期间：" + periodicCharge.StartDate + " 至 " + periodicCharge.EndDate, 0);
                                    dbContext.PeriodicCharges.Add(periodicCharge);
                                    dbContext.PeriodicChargeDetails.Add(periodicChargeDetail);
                                }
                                else
                                {
                                    charge.Contract = item;
                                    charge.Amount = contractFee.CharterMoney;
                                    charge.FeeType = (int)CommonEnum.FeeType.违约金;
                                    charge.PaymentMethod = paymentMethod;
                                    charge.ReceivableAmount = contractFee.CharterMoney;

                                    ListCharge.Add(charge);

                                    chargeDetail.Charge = charge;
                                    chargeDetail.Name = "违约金";
                                    chargeDetail.UnitPrice = contractFee.CharterMoney;
                                    chargeDetail.Quantity = 1;
                                    chargeDetail.Unit = CommonEnum.Unit_One;
                                    chargeDetail.Type = (int)CommonEnum.FeeType.违约金;
                                    chargeDetail.ProductType = (byte)CommonEnum.ChargeProductType.产品;
                                    chargeDetail.ProductCode = "114001000001";

                                    iLog.WriteLog("生成收费数据，合同号：" + item.ContractNo + " 租客：" + rentLoanHistoryPool.CertificateNo + " 收费金额：" + charge.Amount, 0);

                                    dbContext.Charges.Add(charge);
                                    dbContext.ChargeDetails.Add(chargeDetail);
                                }

                                // 保存结算日志
                                RentLoanSettlementLog setmentLog = new RentLoanSettlementLog();
                                setmentLog.ContractID = item.ID;
                                setmentLog.RentLoanHistoryPool = rentLoanHistoryPool;
                                setmentLog.RepaymentTime = rentLoanPool.RepaymentTime;
                                setmentLog.RepaymentAmount = contractFee.CharterMoney;
                                setmentLog.CreateDate = DateTime.Now;
                                setmentLog.Periods = alreadyAlsoPeriod + 1;
                                setmentLog.CreateUserId = rentLoanPool.CreateUserId;

                                iLog.WriteLog("生成结算日志数据，合同号：" + item.ContractNo + " 租客：" + rentLoanHistoryPool.CertificateNo + " 结算金额：" + setmentLog.RepaymentAmount + " 还款日期：" + setmentLog.RepaymentTime + " 申请银行：" + rentLoanHistoryPool.BankName, 0);

                                dbContext.PaymentMethods.Add(paymentMethod);
                                dbContext.PaymentDetails.Add(paymentDetials);


                                dbContext.RentLoanSettlementLogs.Add(setmentLog);
                            }

                            dbContext.RentLoanPools.Remove(rentLoanPool);
                            iLog.WriteLog("清除结算缓存池数据，SSN：" + rentLoanPool.CertificateNo, 0);
                        }
                    }

                    dbContext.Entry(rentLoanAudits[i]).State = System.Data.Entity.EntityState.Modified;
                }

                dbContext.SaveChanges();

            }
            catch
            {
                //iLog.WriteLog("租金贷结算程序运行异常：" + e.Message, 1);
                //if (e.InnerException != null && e.InnerException.InnerException != null)
                //    iLog.WriteLog("租金贷结算程序运行异常：" + e.InnerException.InnerException.Message, 1);
                throw;
            }

            iLog.WriteLog("租金贷结算程序运行结束", 0);
        }

        /// <summary>
        /// 租金贷审核流程状态更改
        /// </summary>
        public void RentLoanAuditChangeStatus()
        {
            try
            {
                iLog.WriteLog("租金贷审批状态更改程序运行开始", 0);

                // 审批状态为已放款未进入还款流程的数据
                List<RentLoanAudit> rentLoanAudits = ReadRentLoanAuditNoNormal();
                // 结算银行信息
                List<BankColumn> bankColumns = ReadBankColumn();

                dbContext.Configuration.AutoDetectChangesEnabled = false;

                foreach (var item in rentLoanAudits)
                {
                    BankColumn bankColumn = (from r in bankColumns
                                             where r.BankCode == item.BankCode
                                             select r).ToList<BankColumn>().FirstOrDefault<BankColumn>();

                    // 账单日
                    int iDate = bankColumn.StatementDate == null ? 1 : bankColumn.StatementDate.Value;
                    DateTime statementDate = DateTime.Now.AddDays(DateTime.Now.Day * -1).AddDays(iDate).Date;

                    if (statementDate > DateTime.Now.Date)
                    {
                        statementDate = statementDate.AddMonths(-1);
                    }

                    // 放款日期
                    DateTime dateOfLoan = (item.DateOfLoan == null ? DateTime.Now : item.DateOfLoan).Value;

                    DateTime paymentDate = dateOfLoan.AddDays(dateOfLoan.Day * -1).AddDays(iDate).Date;

                    if (dateOfLoan > paymentDate)
                    {
                        paymentDate = paymentDate.AddMonths(1);
                    }

                    paymentDate = paymentDate.AddMonths(item.AlreadyAlsoPeriod == null ? 0 : item.AlreadyAlsoPeriod.Value);

                    //rentLoanSettlementLog
                    if (statementDate > item.DateOfLoan)
                    {
                        // 最后还款日期
                        //List<RentLoanSettlementLog> rentLoanSettlementLogList = dbContext.RentLoanSettlementLogs.Where(p => p.Contract.RenterID == item.RenterID).OrderBy(r => r.RepaymentTime).OrderBy(r => r.RepaymentTime).ToList();
                        Contract contract = dbContext.Contracts.Where(p => p.RenterID == item.RenterID && p.Status == 3).FirstOrDefault();
                        if (contract == null) continue;

                        DateTime RepaymentTime = contract.StartDate.AddMonths(item.AlreadyAlsoPeriod == null ? 0 : item.AlreadyAlsoPeriod.Value);

                        //RentLoanSettlementLog rentLoanSettlementLog = rentLoanSettlementLogList.Count > 0 ? rentLoanSettlementLogList.LastOrDefault() : null;

                        //dbContext.RentLoanSettlementLogs.Where(p => p.Contract.RenterID == item.RenterID).OrderBy(r => r.RepaymentTime).LastOrDefault();
                        //if (rentLoanSettlementLog != null && rentLoanSettlementLog.RepaymentTime >= statementDate)
                        if (paymentDate >= statementDate)
                        {
                            item.Status = (byte)CommonEnum.RentLoanStatus.正常还款;
                            iLog.WriteLog(item.Renter.Name + "： 最后还款日[" + RepaymentTime + "]大于本月账单日[" + statementDate + "]，转为正常还款", 0);
                        }
                        else
                        {
                            if (item.Status == (byte)CommonEnum.RentLoanStatus.正常还款)
                            {
                                item.Status = (byte)CommonEnum.RentLoanStatus.逾期不还款;
                                iLog.WriteLog(item.Renter.Name + "： 最后还款日小于本月账单日[" + statementDate + "]，转为逾期不还款", 0);
                            }
                        }

                        dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        item.Status = (byte)CommonEnum.RentLoanStatus.已放款未进入还款流程;
                        iLog.WriteLog(item.Renter.Name + "： 放款日[" + dateOfLoan + "]大于本月账单日[" + statementDate + "]，已放款未进入还款流程", 0);
                    }
                }

                dbContext.SaveChanges();

                iLog.WriteLog("租金贷审批状态更改程序运行结束", 0);
            }
            catch
            {
                //iLog.WriteLog("程序异常：" + e.Message, 1);
                throw;
            }
        }
        /// <summary>
        /// 读取租金贷结算缓存池
        /// </summary>
        /// <returns></returns>
        public List<RentLoanPool> ReadRentLoanPool()
        {
            return dbContext.RentLoanPools.ToList<RentLoanPool>();
        }

        /// <summary>
        /// 获取上月历史结算信息
        /// </summary>
        /// <returns></returns>
        public List<RentLoanHistoryPool> ReadRentHistoryLoanPool()
        {
            DateTime dStartDate = DateTime.Now.AddMonths(-1).AddDays(DateTime.Now.Day * -1 + 1);
            //DateTime dEndDate = DateTime.Now.AddDays(DateTime.Now.Day * -1);

            return dbContext.RentLoanHistoryPools.Where(p => p.RepaymentTime >= dStartDate).ToList<RentLoanHistoryPool>();
        }
        /// <summary>
        /// 读取租金贷审批信息（状态为5和9）
        /// </summary>
        /// <param name="rentorID">租户ID</param>
        /// <returns></returns>
        public List<RentLoanAudit> ReadRentLoanAuditInfo()
        {
            //List<RentLoanAudit> RentLoanAudits = dbContext.RentLoanAudits.Where(p => p.Status == 5 || p.Status == 9).ToList<RentLoanAudit>();

            List<RentLoanAudit> RentLoanAudits = (from r in dbContext.RentLoanAudits
                                                  join c in dbContext.Renters on r.RenterID equals c.ID
                                                  where r.Status > 3
                                                  select r).ToList();

            return RentLoanAudits;
        }

        public List<RentLoanAudit> ReadRentLoanAuditNoNormal()
        {
            // 已放款未进入还款流程、正常还款、逾期不还款
            List<RentLoanAudit> RentLoanAudits = dbContext.RentLoanAudits.Where(p => (p.Status == 4 || p.Status == 5 || p.Status == 9)
                && p.Periods > p.AlreadyAlsoPeriod
                && p.DateOfLoan != null).ToList<RentLoanAudit>();

            return RentLoanAudits;
        }

        /// <summary>
        /// 获取所有符合条件的合同数据
        /// </summary>
        /// <param name="certificateNo"></param>
        /// <returns></returns>
        public List<Contract> ReadContract()
        {
            List<Contract> contracts = (from r in dbContext.Contracts
                                        //join c in dbContext.RentLoanPools on r.Renter.SSN equals c.CertificateNo
                                        where r.Type == 1
                                        select r).ToList<Contract>();
            return contracts;
        }

        /// <summary>
        /// 获取所有符合条件的合同费用数据
        /// </summary>
        /// <param name="contracts"></param>
        /// <returns></returns>
        public List<ContractFee> ReadContractFee()
        {
            List<ContractFee> ContractFees = (from f in dbContext.ContractFees
                                              join r in dbContext.Contracts on f.ContractID equals r.ID
                                              join c in dbContext.RentLoanPools on r.Renter.SSN equals c.CertificateNo
                                              where r.Type == 1
                                              select f).ToList<ContractFee>();
            return ContractFees;
        }

        /// <summary>
        /// 获取银行结算信息
        /// </summary>
        /// <returns></returns>
        public List<BankColumn> ReadBankColumn()
        {
            List<BankColumn> BankColumns = dbContext.BankColumns.ToList<BankColumn>();

            return BankColumns;
        }

        //生成流水号
        public string GeneratePaySerialNo(string serialNo, int count)
        {
            string no = (count + 1).ToString("00000");
            return serialNo + no;
        }

        /// <summary>
        /// 获取第一次生成的流水号
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public int FirstPaySerialNo(string serialNo)
        {
            var c = from a in dbContext.PaymentMethods
                    where a.PaySerialNo.StartsWith(serialNo)
                    select a;
            return c.Count();
        }

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

    }
}
