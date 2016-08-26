using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SyncCustomerCodeService
{
    /// <summary>
    /// 提供关于Ops的枚举变量
    /// </summary>
    public class UtilEnum
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
            过期 = 4
        }

        //合同类型－新增合同时
        public enum ContractAction 
        {
            [Description("出租")]
            出租 = 1,
            [Description("续租")]
            续租=2,
            [Description("退房")]
            退房=3,
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
            [Description("转租结束")]
            转租结束=5,
            [Description("换房开始")]
            换房开始=6,
            [Description("换房结束")]
            换房结束=7,
            [Description("续租")]
            续租 = 8,
            [Description("续费")]
            续费 = 9,
            [Description("其它")]
            其它 = 10,
            [Description("违约提前退房")]
            违约提前退房 = 11
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


        //支付类型
        public enum PaymentType
        {
            [Description("客户和公司结算")]
            客户和公司结算 = 1,
            [Description("公司内部结转")]
            公司内部结转 = 2
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
            [Description("代金券")]
            Voucher = 5
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
            宽带 = 35,
            [Description("其他")]
            其他 = 36
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
            [Description("其他")]
            其他 = 13,
            [Description("违约金")]
            违约金 = 14,
            [Description("洗衣费")]
            洗衣费 = 15
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
            Effective = 1
        }


        //优惠类别
        public enum ConponOn
        {
            [Description("合同周期")]
            合同周期 = 1, //续租时，通过CouponNo直接带出续租月数
            [Description("预订")]
            预订 = 2,

            [Description("合同房价优惠")]
            合同房价 = 20,

            [Description("租金")]
            租金 = 3,
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
        /// Room状态
        /// </summary>
        public enum RoomStatus
        {
            [Description("不可用")]
            NoUse = 0,
            [Description("空闲")]
            Leisure = 1,
            [Description("空闲预订")]
            LeisureReservation = 2,
            [Description("出租预订")]
            RentReservation = 3,
            [Description("维修预订")]
            RepairReservation = 4,
            [Description("出租")]
            Rent = 5,
            [Description("维修")]
            Repair = 6,
            [Description("出租中(未付费)")]   //新出租，未收费
            Renting = 7,
            [Description("占用")]   //新出租，未收费
            StaffOccupied = 8

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
            房价押金价格 = 3
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
            [Description("预订")]
            预订 = 3,
            [Description("取消预订")]
            取消预订 = 4,
            [Description("预订入住")]
            预订入住 = 5,
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

        public enum StoreChargeType
        {
            [Description("洗衣房")]
            洗衣房 = 1,
            [Description("其他")]
            其他 = 2
        }

        public enum StoreDailySetup
        {
            [Description("未日审")]
            未日审 = 1,
            [Description("已日审")]
            已日审 = 2
        }

        public enum StoreStatus
        {
            [Description("招租准备")]
            招租准备 = 1,
            [Description("运营")]
            运营 = 2,
            [Description("关闭")]
            关闭 = 3,
        }

        #endregion

        #region Reservation
        /// <summary>
        /// 预订状态
        /// </summary>
        public enum ReservationStatus
        {
            [Description("预订成功")]
            Scuess = 1,
            [Description("预订取消")]
            Cancel = 2,
            [Description("已使用")]           
            Used =3
        }

        /// <summary>
        /// 预订操作类型
        /// </summary>
        public enum ReservationOperation
        {
            [Description("新增")]
            New=1,
            [Description("取消")]
            Cancel=2
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

        #region ChargeEditType
        public enum ChargeEditType
        {
            [Description("费用修改")]
            费用修改 = 1,
            [Description("支付撤销")]
            支付撤销 = 2
        }
        #endregion

        #region 代金券
        //代金券状态
        public enum VoucherStatus
        {
            [Description("无效的")]
            Ineffective = 0,
            [Description("有效的")]
            Effective = 1
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
        public enum Status
        {
            [Description("有效")]
            有效 = 1,
            [Description("无效")]
            无效 = 0
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
            EmailUserCondition = 1
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





        public enum ItemStatus { 商务发展 = 1, 产品设计, 工程, 运营 }
        public enum DesignStatus { 创建 = 1, 平面图完成, 效果图完成, 施工方案初步确定, 运营设计确认 }
        public enum DesignOperateType { 修改数据, 修改状态 }
        //public enum RenterSex { 男, 女 }
        //public enum RenterPermits { 有, 无 }

        public enum ContractType { 普通 = 1, 免租宿舍, 打折宿舍 }
        public enum EmailUserCondition { BD结束 = 1, 产品设计结束, 工程结束, 工程结束前一个月, 招租结束, 工程进度, 工程开始, 开业, 施工方案初步确定 }
        //public enum PeriodicFeeType { 租金 = 1, 公共卫生费, 公共水电, 电视, 宽带, 其他 }
        //public enum FeeType { 押金 = 1, 电费, 冷水费, 热水费, 转房费, 设备租赁, 签约费, 公司优惠, 索赔, 其他 }
        public enum IntentionStatus { 创建 = 1, 区域立项, 部门立项, 公司会审, 合同条款谈判, 合同签署, 待交接, 交接完成 }
        public enum ProjectStatus { 作废 = 0, 进行中 = 1, 完成 = 2 }
        public enum ProjectOperateType { 修改数据, 修改状态 }
        public enum MaterialStatus { 下订 = 1, 到场 }
        public enum MainlineStatus { working, warning, error, passed }
        public enum NodeStatus { unfinished, finished, confirmed, audited, denied }
        public enum StoreOperationEnum
        {
            出租 = 1,
            退租 = 2,
            预订 = 3,
            取消预订 = 4,
            预订入住 = 5,
            开始维修 = 6,
            结束维修 = 7,
            员工入住 = 8,
            员工搬出 = 9,
            转租 = 10,
            续租 = 11,
            换房 = 12,
            开始占用 = 13,
            结束占用 = 14
        }

        public enum ProjectDetailStatus { 未开始 = 1, 进行中 = 2, 已完成 = 3 }
        public enum ProjectNodeStatus { 未到计划 = 1, 延时未开始 = 2, 进行中 = 3, 完成 = 4, 延时完成 = 5 }
        public enum ModelNodeNoticeType { 启动 = 1, 结束 = 2, 转运营 = 3 }
        public enum ProjectEmailLogType
        {
            通知 = 1,
            预警 = 2,
            项目预警 = 3
        }

    }
}
