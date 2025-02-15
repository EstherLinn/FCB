﻿using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Sysjust
{
    /// <summary>
    /// 基金 – 基本資料，檔案名稱：Sysjust-Basic-Fund.txt
    /// </summary>
    [Delimiter(";@")]
    [HasHeaderRecord(false)]
    internal class SysjustBasicFund
    {
        /// <summary>
        /// 資料日期
        /// </summary>
        [Index(0)]
        public string DataDate { get; set; }

        /// <summary>
        /// 一銀代碼
        /// </summary>
        [Index(1)]
        public string FirstBankCode { get; set; }

        /// <summary>
        /// 基金基金代碼
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string SysjustFundCode { get; set; }


        /// <summary>
        /// 基金評等
        /// </summary>
        [Index(3)]
        public decimal? FundRating { get; set; }


        /// <summary>
        /// 費用備註
        /// </summary>
        [Index(4)]
        [NullValues("", "NULL", null)]
        public string FeeRemarks { get; set; }

        /// <summary>
        /// 一年配息次數
        /// </summary>
        [Index(5)]
        [NullValues("", "NULL", null)]
        public string DividendFrequencyOneYear { get; set; }

        /// <summary>
        /// 指標指數名稱
        /// </summary>
        [Index(6)]
        [NullValues("", "NULL", null)]
        public string IndicatorIndexName { get; set; }

        /// <summary>
        /// 指標指數代碼
        /// </summary>
        [Index(7)]
        [NullValues("", "NULL", null)]
        public string IndicatorIndexCode { get; set; }

        /// <summary>
        /// 投資區域
        /// </summary>
        [Index(8)]
        [NullValues("", "NULL", null)]
        public string InvestmentRegionName { get; set; }

        /// <summary>
        /// 主要投資區域
        /// </summary>
        [Index(9)]
        [NullValues("", "NULL", null)]
        public string PrimaryInvestmentRegion { get; set; }

        [Index(10)]
        [NullValues("", "NULL", null)]
        public string UnKnown { get; set; }

        [Index(11)]
        [NullValues("", "NULL", null)]
        public string FundType { get; set; }
    }
}