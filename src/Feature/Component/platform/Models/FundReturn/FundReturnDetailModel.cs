using System;

namespace Feature.Wealth.Component.Models.FundReturn
{
    public class FundReturnDetailModel
    {
        /// <summary>
        /// 基金名稱
        /// 來源: FUND_BSC.TXT
        /// </summary>
        public string FundName { get; set; }

        /// <summary>
        /// 更新日期
        /// 來源: FUNDTRB.TXT
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 申購費用
        /// 來源: WMS_DOC_RECM.TXT
        /// </summary>
        public string SubscriptionFee { get; set; }

        /// <summary>
        /// 基金轉換手續費率(F)
        /// 來源: FUNDTRB.TXT
        /// </summary>
        public decimal FundConversionFee { get; set; }

        /// <summary>
        /// 基金公司提供本行轉換手續費分成(G)
        /// 來源: FUNDTRB.TXT
        /// </summary>
        public decimal FundShareConvFeeToBank { get; set; }

        /// <summary>
        /// 手續費後收取型態註記
        /// 來源: FUNDTRB.TXT
        /// 說明: Y.後收型 其餘.前收型
        /// </summary>
        public string FeePostCollectionType { get; set; }

        /// <summary>
        /// 本行收取遞延申購手續費率(E)
        /// 來源: FUNDTRB.TXT
        /// </summary>
        public decimal BankDeferPurchase { get; set; }

        /// <summary>
        /// 基金管理費率(H)
        /// 來源: FUNDTRB.TXT
        /// </summary>
        public decimal FundMgmtFee { get; set; }

        /// <summary>
        /// 基金公司提供本行管理費分成(I)
        /// 來源: FUNDTRB.TXT
        /// </summary>
        public decimal FundShareMgmtFeeToBank { get; set; }

        /// <summary>
        /// 基金公司名稱
        /// 來源: FUND_BSC.TXT
        /// </summary>
        public string FundCompanyName { get; set; }

        /// <summary>
        /// 基金公司提供本行說明會及教育訓練贊助金(N)
        /// 來源: FUNDTRB.TXT
        /// </summary>
        public decimal FundSponsorSeminarsEduToBank { get; set; }

        /// <summary>
        /// 基金公司提供本行其他行銷贊助金(O)
        /// 來源: FUNDTRB.TXT
        /// </summary>
        public decimal FundOtherMarketingToBank { get; set; }
    }
}