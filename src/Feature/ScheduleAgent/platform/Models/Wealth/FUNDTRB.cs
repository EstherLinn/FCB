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
        public string ProductCode { get; set; }

        /// <summary>
        /// 本行收取遞延申購手續費率(E)
        /// </summary>
        [NullValues("", "NULL", null)]
        public string BankDeferPurchase { get; set; }

        /// <summary>
        /// 基金公司收取轉換手續費率(F)
        /// </summary>
        [NullValues("", "NULL", null)]
        public string FundConversionFee { get; set; }

        /// <summary>
        /// 基金公司提供本行轉換手續費分成(G)
        /// </summary>
        [NullValues("", "NULL", null)]
        public string FundShareConvFeeToBank { get; set; }

        /// <summary>
        /// 基金經理費率(H)
        /// </summary>
        [NullValues("", "NULL", null)]
        public string FundMgmtFee { get; set; }

        /// <summary>
        /// 基金公司提供本行經理費分成(I)
        /// </summary>
        [NullValues("", "NULL", null)]
        public string FundShareMgmtFeeToBank { get; set; }

        /// <summary>
        /// 基金公司提供本行單筆銷售獎勵金(L)
        /// </summary>
        [NullValues("", "NULL", null)]
        public string FundSingleSaleToBank { get; set; }

        /// <summary>
        /// 基金公司提供本行定期定額銷售獎勵金(M)
        /// </summary>
        [NullValues("", "NULL", null)]
        public string FundRegInvToBank { get; set; }

        /// <summary>
        /// 基金公司提供本行說明會及教育訓練贊助金(N)
        /// </summary>
        [NullValues("", "NULL", null)]
        public string FundSponsorSeminarsEduToBank { get; set; }

        /// <summary>
        /// 基金公司提供本行其他行銷贊助金(O)
        /// </summary>
        [NullValues("", "NULL", null)]
        public string FundOtherMarketingToBank { get; set; }

        /// <summary>
        /// 手續費後收型註記
        /// </summary>
        [NullValues("", "NULL", null)]
        public string FeePostCollectionType { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public string UpdateTime => DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    }
}