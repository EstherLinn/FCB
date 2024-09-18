namespace Feature.Wealth.Component.Models.Bond
{
    public static class DataSanitizer
    {
        public static BondListDto WithoutSensitiveData(this BondListDto bond)
        {
            bond.InterestRate = null;
            bond.MinIncrementAmount = string.Empty;
            bond.ISINCode = string.Empty;
            bond.SubscriptionFee = default(int);
            bond.RedemptionFee = null;
            bond.Date = string.Empty;
            bond.PreviousInterest = null;
            bond.YieldRateYTM = null;
            bond.UpsAndDownsMonth = null;
            return bond;
        }
    }

    public class SafeBondListDto
    {
        /// <summary>
        /// 第一銀行債券代碼
        /// </summary>
        public string BondCode { get; set; }

        /// <summary>
        /// 第一銀行債券名稱
        /// </summary>
        public string BondName { get; set; }

        /// <summary>
        /// 開放個網
        /// </summary>
        public string OpenToPublic { get; set; }

        /// <summary>
        /// 是否上架
        /// 國外債判斷(與結構型商品、結構型金融債一樣)
        /// 1.DUE_DAY&TFTFU_BOND_CALL_DAY<=SYSDATE :N
        /// 2.上架日期 > SYSDATE : N 若沒特別輸入日期一樣為Y
        /// 3.下架日期<=SYSDATE : N 若沒特別輸入日期一樣為Y
        /// </summary>
        public string Listed { get; set; }
    }
}