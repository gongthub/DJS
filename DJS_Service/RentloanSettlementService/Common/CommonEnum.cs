using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RentloanSettlementService.Common
{
    /// <summary>
    /// 提供关于Ops的枚举变量
    /// </summary>
    public class CommonEnum
    {
        //合同状态
        public enum ContractStatus
        {
            [Description("待办合同")]
            待办 = 1,
            [Description("无效合同")]
            无效 = 2,
            [Description("执行中合同")]
            执行中 = 3,
            [Description("旧合同")]
            过期 = 4,
            [Description("待退房")]
            待退房 = 5
        }

        //合同类型－新增合同时
        public enum ContractAction 
        {
            [Description("出租")]
            出租 = 1,
            [Description("续租")]
            续租=2,
            [Description("退房")]
            退房 = 3,                   
            [Description("换房")]
            换房=4,
            [Description("转租")]
            转租=5,
            [Description("异店换房")]
            异店换房 = 6                
        }

        //合同操作类型
        public enum ContractOperationType
        {
            [Description("开始")]
            开始=0,
            [Description("到期退房")]
            到期退房 = 1,
            [Description("提前退房")]
            提前退房=2,
            [Description("违约退房")]
            违约退房=3,
            [Description("转租开始")]
            转租开始=4,
            [Description("转租退房")]
            转租退房 = 5,
            [Description("换房开始")]
            换房开始=6,
            [Description("换房退房")]
            换房退房 = 7,
             [Description("续租开始")]
            续租开始 = 8,
             [Description("续租退房")]
             续租退房 = 9,
            [Description("其它")]
            其它 = 10,
            [Description("违约提前退房")]
            违约提前退房 = 11,
            [Description("待退房")]
            待退房 = 12,
            [Description("待退房取消")]
            待退房取消 = 14,
            [Description("合同作废")]
            合同作废 = 15,
            [Description("取消签约")]
            取消签约 = 17,
            [Description("转租撤销")]
            转租撤销 = 18,
            [Description("异店换房退房")]
            异店换房退房 = 19,
            [Description("续费")]
            续费 = 20,
            [Description("续租退房取消")]
            续租退房取消 = 21,
            [Description("换房退房取消")]
            换房退房取消 = 22,
            [Description("转租合同取消")]
            转租合同取消 = 23
        }

        //客户渠道类型
        public enum CustomerChannelType
        {
            [Description("出租")]
            出租=1,
            [Description("续租")]
            续租=2,
            [Description("换房")]
            换房=3,
            [Description("转租")]
            转租=4
        }


      

        //支付详细类型
        public enum PaymentDetailsType
        {
            [Description("现金")]
            Cash = 1,
            [Description("POS刷卡")]
            POS = 2,
            [Description("支票")]
            Check = 3,
            [Description("银行转帐")]
            BankTransfer = 4,
            [Description("现金抵用券")]
            Voucher = 5,
            [Description("支付宝")]
            Online_alipay = 6,
            [Description("银联")]
            Online_UnionPay = 7,
            [Description("微信")]
            Online_WeChat = 8,
            [Description("租金贷")]
            RentLoan = 9
        }

        //支付类型
        public enum PaymentPayType
        {
            [Description("客户和公司结算")]
            客户和公司结算 = 1,
            [Description("公司内部结转")]
            公司内部结转 = 2,
            [Description("CRM结算")]
            CRM结算 = 3
        }

        //支付状态
        public enum PaymentStatus
        {
            [Description("待付款")]
            待付款 = 1,
            [Description("已付款")]
            已付款 = 2,
            [Description("待退费")]
            待退费 = 3,
            [Description("已退费")]
            已退费 = 4,
            [Description("作废")]
            作废 = 5

        }

        //阶段性收费科目类型
        public enum PeriodicFeeType
        {
            [Description("租金")]
            租金 = 31,
            [Description("公共卫生费")]
            公共卫生费 = 32,
            [Description("公摊水电费")]
            公摊水电费 = 33,
            [Description("电视")]
            电视 = 34,
            [Description("宽带")]
            宽带 = 35
        }

        //周期性单位
        public static string PeriodicDay="D";
        public static string PeriodicMonth="M";
        public static string Unit_Day="天";
        public static string Unit_Month="月";
        public static string Unit_Year="年";
        public static string Unit_An="个";
        public static string Unit_One = "次";
        //一般性收费科目类型
        public enum FeeType 
        {
            [Description("押金")]
            押金 = 1,
            [Description("电费")]
            电费=2,
            [Description("冷水费")]
            冷水费=3,
            [Description("热水费")]
            热水费=4,
            [Description("转房费")]
            转房费=5,
            [Description("设备租赁")]
            设备租赁=6,
            [Description("签约费")]
            签约费=7,
            [Description("公司优惠")]
            公司优惠=8,
            [Description("索赔")]
            索赔=9,
            [Description("结转")]
            结转=10,
            [Description("逾期金")]
            逾期金=11,
            [Description("预约费")]
            预约费 = 12,
            [Description("违约金")]
            违约金 = 14,
            [Description("水卡押金")]
            水卡押金 = 15,
            [Description("电卡押金")]
            电卡押金 = 16,
            [Description("门禁卡押金")]
            门禁卡押金 = 17,
            [Description("其他")]
            其他 = 112
        }

        public enum ChargeType
        {
            [Description("周期性费用")]
            周期性费用 = 1,
            [Description("一次性费用")]
            一次性费用 = 2,
            [Description("门店收费")]
            门店收费 = 3,
            [Description("预定费用")]
            预定费用 = 4
        }

        public enum ChargeTypeStatus
        {
            [Description("可用")]
            可用 = 1,
            [Description("不可用")]
            不可用 = 2
        }

        #region  Coupon
        //打折类型
        public enum DiscountType
        {
            [Description("直减")]
            Reduce = 1,    //如减去100
            [Description("折扣")]
            Discount = 2,     //如95折
            [Description("折扣到")]
            DiscountTo = 3     //100，折扣到100
        }

        //优惠类型
        public enum CouponType
        {
            [Description("一次使用")]
            Once=0,
            [Description("多次使用")]
            MoreThanOnce=1
        }

        //Coupon状态
        public enum CouponStatus
        {
            [Description("无效的")]
            Ineffective= 0,
            [Description("有效的")]
            Effective = 1,
            [Description("作废的")]
            Cancel = 2
        }


        //优惠类别
        public enum ConponOn
        {
            [Description("合同周期")]
            合同周期 = 1, //续租时，通过CouponNo直接带出续租月数
            [Description("取消预定")]
            取消预定 = 2,

            [Description("房屋租金")]
            房屋租金 = 20,

            [Description("房租抵用券")]
            房租抵用券 = 3,

            [Description("公共卫生费")]
            公共卫生费 = 4,
            [Description("公摊水电费")]
            公摊水电费 = 5,
            [Description("电视")]
            电视 = 6,
            [Description("宽带")]
            宽带 = 7,

            [Description("押金")]
            押金 = 8,
            [Description("电费")]
            电费 = 9,
            [Description("冷水费")]
            冷水费 = 10,
            [Description("热水费")]
            热水费 = 11,
            [Description("转房费")]
            转房费 = 12,
            [Description("设备租赁")]
            设备租赁 = 13,
            [Description("签约费")]
            签约费 = 14,
            [Description("索赔")]
            索赔 = 15,
            [Description("结转")]
            结转 = 16,
            [Description("逾期金")]
            逾期金 = 17,
            [Description("违约金")]
            违约金 = 18,
            [Description("其它")]
            其它=19
        }

        #endregion

        //ContractFee
        public enum ContractFeeType
        {
            [Description("合同房价")]
            合同房价 = 1,
            [Description("公摊水电费")]
            公摊水电费 = 2,
            [Description("公共卫生费")]
            公共卫生费 = 3,
            [Description("TV费用")]
            TV费用 = 4,
            [Description("宽带费用")]
            宽带费用 = 5,
            [Description("电费费用")]
            电费费用 = 6,
            [Description("冷水费用")]
            冷水费用 = 7,
            [Description("热水费用")]
            热水费用 = 8,
            [Description("押金")]
            押金 = 9
        }

        //租客性别
        public enum RenterSex 
        {
            [Description("男")]
            Male=1,
            [Description("女")]
            Female=0
        }

        //租客是否有许可证
        public enum RenterPermits
        {           
           [Description("无")]
            No = 0,
           [Description("有")]
            Yes = 1
        }

        /// <summary>
        /// 证件类型
        /// </summary>
        public enum RenterNationality
        {
            [Description("身份证")]
            ID=1,
            [Description("军官证")]
            Officers = 2,
            [Description("其它")]
            Other = 3,
        }

        /// <summary>
        /// 门店状态
        /// </summary>
        public enum StoreStatus
        {
            [Description("招租准备")]
            招租准备 = 1,
            [Description("运营")]
            运营 = 2,
            [Description("关闭")]
            关闭 = 3,
        }

        /// <summary>
        /// Room状态
        /// </summary>
        public enum RoomStatus
        {
            [Description("关闭")]
            关闭 = 0,
            [Description("空闲")]
            Leisure = 1,
            //[Description("空闲预定")]
            //LeisureReservation = 2,    //作废
            //[Description("出租预定")]
            //RentReservation = 3,      //作废
            //[Description("维修预定")]
            //RepairReservation = 4,    //作废
            [Description("出租")]
            Rent = 5,
            [Description("维修")]
            Repair = 6,
            [Description("出租中(未付费)")]   //新出租，未收费
            Renting = 7,
            //[Description("占用")]
            //StaffOccupied = 8,       //作废
            [Description("退房中(待确认)")]   //退房中
            CheckOuting=9,
            [Description("转租中")]   //转租中
            ChangeRenting = 10
        }

        /// <summary>
        /// WENumber类型
        /// </summary>
        public enum WENumberType
        {
            [Description("冷水")]
            冷水=1,
            [Description("热水")]
            热水=2
        }

        public enum WEOperationType
        {
            [Description("初始值")]
            初始值 = 1,
            [Description("非初始值")]
            非初始值 = 2,
            [Description("修改值")]
            修改值 = 3
        }



        /// <summary>
        /// 设备损坏类型
        /// </summary>
        public enum BreakType
        {
            [Description("设备损坏")]
            设备损坏 = 1,
            [Description("装修损坏")]
            装修损坏 = 2
        }

        /// <summary>
        /// 电费截图的类型
        /// </summary>
        public enum FeeFilesType
        {
            [Description("店电费截图")]
            店电费截图 = 1,
            [Description("房间电费截图")]
            房间电费截图 = 2,
            [Description("房价押金价格")]
            房价押金价格 = 3,
            [Description("门店房间号")]
            门店房间号 = 4,
            [Description("房间属性")]
            房间属性 = 5
        }

        #region Store
        /// <summary>
        /// StoreOperation
        /// </summary>
        public enum StoreOperation
        {
            [Description("出租")]
            出租 = 1,
            [Description("退租")]
            退租 = 2,
            [Description("预定")]
            预定 = 3,
            [Description("取消预定")]
            取消预定 = 4,
            [Description("预定入住")]
            预定入住 = 5,
            [Description("开始维修")]
            开始维修 = 6,
            [Description("结束维修")]
            结束维修 = 7,
            [Description("转租")]
            转租 = 10,
            [Description("续租")]
            续租 = 11,
            [Description("换房")]
            换房 = 12,
            [Description("开始占用")]
            开始占用 = 13,
            [Description("结束占用")]
            结束占用 = 14,
        }

       

        public enum StoreDailySetup
        {
            [Description("未日审")]
            未日审 = 1,
            [Description("已日审")]
            已日审 = 2
        }
        #endregion

        #region Reservation
        /// <summary>
        /// 预定状态
        /// </summary>
        public enum ReservationStatus
        {
            [Description("预定中")]
            Scuess = 1,
            [Description("已取消")]
            Cancel = 2,
            [Description("已入住")]           
            Used =3,
            [Description("预定变更")]
            ChangeScuess = 4,
            [Description("预定（未付费）")]
            Booking = 5
        }

        /// <summary>
        /// 预定操作类型
        /// </summary>
        public enum ReservationOperation
        {
            [Description("预定")]
            New=1,
            [Description("取消预定")]
            Cancel=2,
            [Description("预定入住")]
            ReservationRent = 3,
            [Description("取消预定（入住）")]
            CancelRent = 4,
            [Description("取消预定（变更）")]
            CancelChange = 5,
            [Description("预定（变更）")]
            NewChange = 6
        }
        #endregion

        #region Repair
        /// <summary>
        /// 维修Opereation
        /// </summary>
        public enum RepairType
        {
            [Description("新增维修")]
            NewRepair = 1,
            [Description("结束维修")]
            CloseRepair = 2
        }

        #endregion

        #region StaffOccupied
        /// <summary>
        /// 占用Opereation
        /// </summary>
        public enum StaffOccupiedType
        {
            [Description("新增占用")]
            NewStaffOccupied = 1,
            [Description("结束占用")]
            CloseStaffOccupied = 2
        }
        #endregion


        #region 现金抵用券
        //现金抵用券状态
        public enum VoucherStatus
        {
            [Description("无效的")]
            Ineffective = 0,
            [Description("有效的")]
            Effective = 1,
            [Description("作废")]
            Cancel = 2
        }

        #endregion

        #region 撤销日志
        //日志操作大类
        public enum OperationType
        {
            [Description("支付")]
            支付 = 1,
            [Description("水表读数")]
            水表读数 = 2,
            [Description("合同")]
            合同 = 3,
            [Description("现金存银行")]
            现金存银行 = 4,
            [Description("取消签约")]
            取消签约 = 5,
            [Description("待退房取消")]
            待退房取消 = 6,
            [Description("转租撤销")]
            转租撤销 =7
        }
        //日志操作子类
        public enum OperationAction
        {
            [Description("支付作废")]
            支付作废 = 1,
            [Description("水表读数作废")]
            水表读数作废 = 2,
            [Description("合同作废")]
            合同作废 = 3,
            [Description("现金存银行作废")]
            现金存银行作废 = 4,
            [Description("支付撤销")]
            支付撤销 = 5,
            [Description("待退房支付作废")]
            待退房支付作废 = 6,
            [Description("取消签约")]
            取消签约 = 7,
            [Description("转租撤销")]
            转租撤销 = 8,
            [Description("水表读数修改")]
            水表读数修改 = 9
        }
        #endregion

        #region 现金存银行
        public enum CashSavingStatus
        {
            [Description("正常")]
            正常 = 1,
            [Description("作废")]
            作废 = 2
        }

        #endregion

        #region 数据状态
        public enum RecordStatus
        {
            [Description("正常")]
            正常 = 1,
            [Description("作废")]
            作废 = 2
        }
        #endregion

        #region 支付类型表枚举
        /// <summary>
        /// 数据来源
        /// </summary>
        public enum PayMethodTypeSource
        {

            [Description("AMS")]
            AMS = 1,
            [Description("网站预约")]
            网站预约 = 2,
            [Description("微信")]
            微信 = 3,
            [Description("App")]
            App = 4,
            [Description("租金贷")]
            租金贷 = 5
        }

        ///// <summary>
        ///// 数据来源
        ///// </summary>
        //public enum PayMethodTypeIsDailyCheck
        //{
        //    [Description("需要日审")]
        //    需要日审 = 1
        //}

        /// <summary>
        /// 是否有单据号
        /// </summary>
        public enum PayMethodTypeHasSerialNo
        {
            [Description("无")]
            无 = 0,
            [Description("有")]
            有 = 1
        }
        /// <summary>
        /// 是否可退费
        /// </summary>
        public enum PayMethodTypeIsReturn
        {
            [Description("可退费")]
            可退费 = 1,
            [Description("不退费")]
            不退费 = 0
        }
        /// <summary>
        /// 是否验证单据号
        /// </summary>
        public enum PayMethodTypeIsCheckNo
        {
            [Description("不验证")]
            不验证 = 0,
            [Description("验证")]
            验证 = 1
        }
        #endregion

        #region ProjectSource
        public enum ProjectSource
        {
            [Description("AMS")]
            AMS = 1,
            [Description("Project")]
            Project = 2,
            [Description("Portal")]
            Portal = 4
        }
        #endregion

        public enum DictionaryType
        {
            [Description("自动邮箱推送节点")]
            EmailUserCondition = 1,
            [Description("开业邮箱推送")]
            OpenStoreEmailUser = 2
        }

        public enum ModuleLevel
        {
            [Description("功能模块")]
            Level1 = 1,
            [Description("功能点")]
            Level2 = 2,
            [Description("Action")]
            Level3 = 3
        }

        public enum SynStatus
        {
            [Description("未处理")]
            未处理 = 0,
            [Description("已处理")]
            已处理 = 1
        }

        public enum EnabledStatus
        {
            [Description("可用")]
            可用 = 1,
            [Description("不可用")]
            不可用 = 2
        }


        public enum CheckOutReasonType
        {
            [Description("原因分类")]
            原因分类 = 1,
            [Description("具体分类")]
            具体分类 = 2
        }

        public enum ChargeProductType
        {
            [Description("产品")]
            产品 = 1,
            [Description("优惠")]
            优惠 = 2
        }

        public enum StoreElecType
        {
            [Description("南京")]
            南京 = 1,
            [Description("上海")]
            上海 = 2
        }

        public enum ElecLogs
        {
            [Description("AMS错误")]
            AMS错误 = 1,
            [Description("电表接口错误")]
            电表接口错误 = 2
        }

        public enum RoomFeeRuleType
        {
            [Description("金额")]
            金额 = 1,    //如减去100
            [Description("百分比")]
            百分比 = 2     
        }

        public enum EmailUserCondition { BD结束 = 1, 产品设计结束, 工程结束, 工程结束前一个月, 招租结束, 工程进度, 工程开始, 开业 }

        public enum RefundStatus
        {
            [Description("待上传资料")]
            待上传资料 = 1,
            [Description("待回访核实")]
            待回访核实 = 2,
            [Description("待审核")]
            待审核 = 3,
            [Description("待退款")]
            待退款 = 4,
            [Description("已退款")]
            已退款 = 5,
            [Description("完成")]
            完成 = 6,
            [Description("退回待修改")]
            退回待修改 = 7,
            [Description("修改待批准")]
            修改待批准 = 8,
            [Description("作废")]
            作废 = 0
        }
        public enum RefundOpType
        {
            [Description("退房")]
            退房 = 1,
            [Description("退预定")]
            退预定 = 2,
            [Description("提交申请")]
            提交申请 = 3,
            [Description("回访确认")]
            回访确认 = 4,
            [Description("审核确认")]
            审核确认 = 5,
            [Description("退回")]
            退回 = 6,
            [Description("修改批准")]
            修改批准 = 7,
            [Description("修改不批准")]
            修改不批准 = 8,
            [Description("退款")]
            退款 = 9,
            [Description("确认到账")]
            确认到账 = 10

        }

        public enum FileSubject
        { 
          [Description("退房结算单")]
            退房结算单=1,

          [Description("押金单")]
            押金单=2,
          
          [Description("住户合同(正反面)")]
            住户合同=3,

          [Description("电费截图")]
            电费截图=4,

          [Description("未到期房租收据")]
            未到期房租收据 = 5,

          [Description("随从人员补充协议")]
            随从人员补充协议 = 6,

          [Description("授权委托书")]
            授权委托书 = 7,
            
          [Description("其它")]
           其它 = 8,

        }

        /// <summary>
        /// 产品
        /// </summary>
        public enum ProductType {
            [Description("住宅")]
            住宅 = 0,
            [Description("商铺")]
            商铺 = 1
        }

        /// <summary>
        /// 来人心智
        /// </summary>
        public enum VisitorNature {
            [Description("个人")]
            个人 = 0,
            [Description("企业")]
            企业 = 1
        }

        /// <summary>
        /// 是否
        /// </summary>
        public enum CancelOrOK {
            [Description("否")]
            否 = 0,
            [Description("是")]
            是 = 1
        }

        /// <summary>
        /// 渠道
        /// </summary>
        public enum Channels {
            [Description("拜访")]
            拜访 = 0,
            [Description("DM")]
            DM = 1,
            [Description("户外")]
            户外 = 2,
            [Description("时代报")]
            时代报 = 3,
            [Description("微博")]
            微博 = 4,
            [Description("微信")]
            微信 = 5,
            [Description("百度")]
            百度 = 6,
            [Description("大众点评")]
            大众点评 = 7,
            [Description("安居客")]
            安居客 = 8,
            [Description("赶集网")]
            赶集网 = 9,
            [Description("58同城")]
            五八同城 = 10,
            [Description("路过")]
            路过 = 11,
            [Description("熟人")]
            熟人 = 12
        }

        public enum NotTurnoverReason {
            [Description("租金")]
            租金 = 0,
            [Description("面积")]
            面积 = 1,
            [Description("人数")]
            人数 = 2,
            [Description("厨房")]
            厨房 = 3,
            [Description("洗衣")]
            洗衣 = 4,
            [Description("押金")]
            押金 = 5,
            [Description("租期")]
            租期 = 6,
            [Description("办证")]
            办证 = 7,
            [Description("劳动合同")]
            劳动合同 = 8,
            [Description("其他")]
            其他 = 9
        }

        /// <summary>
        /// 是否
        /// </summary>
        public enum IsFollow
        {
            [Description("未回访")]
            未回访 = 0,
            [Description("已回访")]
            已回访 = 1
        }

        /// <summary>
        /// 合同类型
        /// </summary>
        public enum ContractType {
            [Description("普通合同")]
            普通合同 = 0,
            [Description("租金贷合同")]
            租金贷合同 = 1
        }

        /// <summary>
        /// 租金贷审核状态
        /// </summary>
        public enum RentLoanStatus
        {
            [Description("待提交")]
            待提交 = 0,
            [Description("待审核")]
            待审核 = 1,
            [Description("审核通过")]
            审核通过 = 2,
            [Description("审核未通过")]
            审核未通过 = 3,
            [Description("已放款未进入还款流程")]
            已放款未进入还款流程 = 4,
            [Description("正常还款")]
            正常还款 = 5,
            [Description("正常终止")]
            正常终止 =6,
            [Description("异常终止")]
            异常终止 = 7,
            [Description("逾期不还款")]
            逾期不还款 = 9
        }

        /// <summary>
        /// 联系人关系
        /// </summary>
        public enum RelationtType
        {
            [Description("配偶")]
            配偶 = 1,
            [Description("兄弟姐妹")]
            兄弟姐妹 = 2,
            [Description("父母")]
            父母 = 3,
            [Description("子女")]
            子女 = 4
        }

        /// <summary>
        /// 单位性质
        /// </summary>
        public enum CompanyType
        {
            [Description("机关/事业")]
            机关事业 = 1,
            [Description("国企")]
            国企 = 2,
            [Description("三资")]
            三资 = 3,
            [Description("上市")]
            上市 = 4
        }

        /// <summary>
        /// 是否虚房
        /// </summary>
        public enum VirtualRoom { 
            [Description("否")]
            否 = 0,
            [Description("是")]
            是 = 1
        }
    }
}
