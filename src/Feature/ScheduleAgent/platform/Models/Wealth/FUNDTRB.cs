using System;
using CsvHelper.Configuration.Attributes;

namespace Feature.Wealth.ScheduleAgent.Models.Wealth
{
    /// <summary>
    /// 基金通路報酬揭露資料檔，檔案名稱：FUNDTRB.txt
    /// </summary>
    [Delimiter(";")]
    [HasHeaderRecord(false)]
    public class FundTrb
    {
        /// <summary>
        /// 銀行產品代碼
        /// </summary>
        [Index(0)]
        public string ProductCode { get; set; }

        /// <summary>
        /// 本行收取遞延申購手續費率(E)
        /// </summary>
        [Index(1)]
        [NullValues("", "NULL", null)]
        public string BankDeferPurchase { get; set; }

        /// <summary>
        /// 基金公司收取轉換手續費率(F)
        /// </summary>
        [Index(2)]
        [NullValues("", "NULL", null)]
        public string FundConversionFee { get; set; }

        /// <summary>
        /// 基金公司提供本行轉換手續費分成(G)
        /// </summary>
        [Index(3)]
        [NullValues("", "NULL", null)]
        public string FundShareConvFeeToBank { get; set; }

        /// <summary>
        /// 基金經理費率(H)
        /// </summary>
        [Index(4)]
        [NullValues("", "NULL", null)]
        public string FundMgmtFee { get; set; }

        /// <summary>
        /// 基金公司提供本行經理費分成(I)
        /// </summary>
        [Index(5)]
        [NullValues("", "NULL", null)]
        public string FundShareMgmtFeeToBank { get; set; }

        /// <summary>
        /// 基金公司提供本行單筆銷售獎勵金(L)
        /// </summary>
        [Index(6)]
        [NullValues("", "NULL", null)]
        public string FundSingleSaleToBank { get; set; }

        /// <summary>
        /// 基金公司提供本行定期定額銷售獎勵金(M)
        /// </summary>
        [Index(7)]
        [NullValues("", "NULL", null)]
        public string FundRegInvToBank { get; set; }

        /// <summary>
        /// 基金公司提供本行說明會及教育訓練贊助金(N)
        /// </summary>
        [Index(8)]
        [NullValues("", "NULL", null)]
        public string FundSponsorSeminarsEduToBank { get; set; }

        /// <summary>
        /// 基金公司提供本行其他行銷贊助金(O)
        /// </summary>
        [Index(9)]
        [NullValues("", "NULL", null)]
        public string FundOtherMarketingToBank { get; set; }

        /// <summary>
        /// 手續費後收型註記
        /// </summary>
        [Index(10)]
        [NullValues("","NULL", null)]
        public string FeePostCollectionType { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        [Index(11)]
        public string UpdateTime => DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    }
}