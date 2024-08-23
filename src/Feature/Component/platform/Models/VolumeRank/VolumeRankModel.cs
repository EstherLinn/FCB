using Foundation.Wealth.Extensions;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.VolumeRank
{
    public class VolumeRankModel
    {
        public Item Item { get; set; }
        public IList<ETFs> VolumeRanks { get; set; }
        public string DetailLink { get; set; }

        /// <summary>
        /// ETF商品資訊
        /// </summary>
        public class ETFs
        {
            /// 一銀產品代號
            /// </summary>
            public string ProductCode { get; set; }

            /// <summary>
            /// 一銀產品名稱
            /// </summary>
            public string ProductName { get; set; }

            /// <summary>
            /// 交易所代碼
            /// </summary>
            public string ExchangeCode { get; set; }

            /// <summary>
            /// 淨值日期
            /// </summary>
            public DateTime MarketPriceDate { get; set; }

            /// <summary>
            /// 淨值日期
            /// </summary>
            public string MarketPriceDateFormat => MarketPriceDate.ToString("yyyy/MM/dd");

            /// <summary>
            /// 市價
            /// </summary>
            public decimal MarketPrice { get; set; }

            /// <summary>
            /// 折溢價 
            /// </summary>
            public decimal DiscountPremium { get; set; }

            /// <summary>
            /// 最新量
            /// </summary>
            public decimal LatestVolumeTradingVolume { get; set; }
            public string LatestVolumeTradingVolumeFormat => NumberExtensions.FormatNumber(LatestVolumeTradingVolume);

            /// <summary>
            /// 最新量-十日均價
            /// </summary>
            public decimal TenDayAverageVolume { get; set; }
            public string TenDayAverageVolumeFormat => NumberExtensions.FormatNumber(TenDayAverageVolume);

            /// <summary>
            /// 可否網路申購
            /// </summary>
            public string OnlineSubscriptionAvailability { get; set; }

            /// <summary>
            /// 是否上架
            /// </summary>
            public string AvailabilityStatus { get; set; }

            public string[] ETFDiscountTags { get; set; }


        }
    }

    public class Template
    {
        public struct VolumeRank
        {
            public static readonly ID Id = new ID("{E4C22549-EB3A-414B-8B3D-924301977A14}");

            public struct Fields
            {
                /// <summary>
                /// 主標題
                /// </summary>
                public static readonly ID MainTitle = new ID("{D69D113E-304D-4610-BC70-70790E4B9CE9}");

                /// <summary>
                /// 副標題
                /// </summary>
                public static readonly ID SubTitle = new ID("{73569CDE-837E-4DCC-8B5D-C51021ECB499}");

                /// <summary>
                /// 標題備註
                /// </summary>
                public static readonly ID Content = new ID("{7A85A0BD-110A-4E93-BFED-0FBF5C88322D}");
            }
        }
    }
}
