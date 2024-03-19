using System.Collections.Generic;

namespace Feature.Wealth.Component.Models.FundReturn
{
    /// <summary>
    /// 基金 Model
    /// </summary>
    public class FundReturnFilterModel
    {
        /// <summary>
        /// 國內/境外基金註記
        /// </summary>
        public string DomesticForeignFundIndicator { get; set; }

        /// <summary>
        /// 基金公司列表
        /// </summary>
        public IList<FundCompany> FundCompanies { get; set; }
    }

    /// <summary>
    /// 基金公司資訊
    /// </summary>
    public class FundCompany
    {
        /// <summary>
        /// 基金公司ID
        /// </summary>
        public string FundCompanyID { get; set; }

        /// <summary>
        /// 基金公司名稱
        /// </summary>
        public string FundCompanyName { get; set; }

        /// <summary>
        /// 基金列表
        /// </summary>
        public IList<Fund> Funds { get; set; }
    }

    /// <summary>
    /// 基金商品資訊
    /// </summary>
    public class Fund
    {
        /// <summary>
        /// 銀行產品代碼
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 產品名稱
        /// </summary>
        public string ProductName { get; set; }
    }
}