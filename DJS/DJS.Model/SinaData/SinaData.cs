using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Model
{
    public class SinaData
    {
        //主键
        public Guid ID { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 今日开盘价
        /// </summary>
        public decimal TodayOpenPrice { set; get; }

        /// <summary>
        /// 昨日收盘价
        /// </summary>
        public decimal TesterdayClosePrice { set; get; }

        /// <summary>
        /// 当前价格
        /// </summary>
        public decimal NowPrice { get; set; }
        /// <summary>
        /// 今日最高价
        /// </summary>
        public decimal TodayMaxPrice { get; set; }
        /// <summary>
        /// 今日最低价
        /// </summary>
        public decimal TodayMinPrice { get; set; }
        /// <summary>
        /// 竞买价
        /// </summary>
        public decimal BidPrice { get; set; }
        /// <summary>
        /// 竞卖价
        /// </summary>
        public decimal AuctionPrice { get; set; }
        /// <summary>
        /// 成交数 如4695股，即47手
        /// </summary>
        public decimal TransactionNum { get; set; }
        /// <summary>
        /// 成交金额 单位为“元”
        /// </summary>
        public decimal TransactionPrice { get; set; }
        /// <summary>
        /// 买一数量 如申请4695股，即47手
        /// </summary>
        public decimal BuyNum1 { get; set; }
        /// <summary>
        /// 买一价格
        /// </summary>
        public decimal BuyPrice1 { get; set; }
        /// <summary>
        /// 买二数量 如申请4695股，即47手
        /// </summary>
        public decimal BuyNum2 { get; set; }
        /// <summary>
        /// 买二价格
        /// </summary>
        public decimal BuyPrice2 { get; set; }
        /// <summary>
        /// 买三数量 如申请4695股，即47手
        /// </summary>
        public decimal BuyNum3 { get; set; }
        /// <summary>
        /// 买三价格
        /// </summary>
        public decimal BuyPrice3 { get; set; }
        /// <summary>
        /// 买四数量 如申请4695股，即47手
        /// </summary>
        public decimal BuyNum4 { get; set; }
        /// <summary>
        /// 买四价格
        /// </summary>
        public decimal BuyPrice4 { get; set; }
        /// <summary>
        /// 买五数量 如申请4695股，即47手
        /// </summary>
        public decimal BuyNum5 { get; set; }
        /// <summary>
        /// 买五价格
        /// </summary>
        public decimal BuyPrice5 { get; set; }


        /// <summary>
        /// 卖一数量 如申请4695股，即47手
        /// </summary>
        public decimal SellNum1 { get; set; }
        /// <summary>
        /// 卖一价格
        /// </summary>
        public decimal SellPrice1 { get; set; }
        /// <summary>
        /// 卖二数量 如申请4695股，即47手
        /// </summary>
        public decimal SellNum2 { get; set; }
        /// <summary>
        /// 卖二价格
        /// </summary>
        public decimal SellPrice2 { get; set; }
        /// <summary>
        /// 卖三数量 如申请4695股，即47手
        /// </summary>
        public decimal SellNum3 { get; set; }
        /// <summary>
        /// 卖三价格
        /// </summary>
        public decimal SellPrice3 { get; set; }
        /// <summary>
        /// 卖四数量 如申请4695股，即47手
        /// </summary>
        public decimal SellNum4 { get; set; }
        /// <summary>
        /// 卖四价格
        /// </summary>
        public decimal SellPrice4 { get; set; }
        /// <summary>
        /// 卖五数量 如申请4695股，即47手
        /// </summary>
        public decimal SellNum5 { get; set; }
        /// <summary>
        /// 卖五价格
        /// </summary>
        public decimal SellPrice5 { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}
