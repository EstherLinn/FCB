using Feature.Wealth.Component.Models.ExclusiveRecommendation;
using Foundation.Wealth.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Feature.Wealth.Component.Repositories
{
    public class ExclusiveRecommendationRepository
    {
        /// <summary>
        /// 取得上方列表基金資料
        /// </summary>
        /// <param name="riskLevel"></param>
        /// <returns></returns>
        public IEnumerable<FundBasicDto> GetTopRiskList(string riskLevel)
        {
            RiskCompare.RiskCompareDic.TryGetValue(riskLevel, out List<string> riskLevels);

            string sql = $@"SELECT TOP(6) ProductCode,FundName,AvailabilityStatus,OnlineSubscriptionAvailability,OneMonthReturnOriginalCurrency FROM vw_BasicFund
                        WHERE RiskLevel IN @riskLevel ORDER BY SixMonthReturnOriginalCurrency DESC";
            var para = new { riskLevel = riskLevels };

            var datas = DbManager.Custom.ExecuteIList<FundBasicDto>(sql, para, CommandType.Text)?.ToList();
            if (datas == null)
            {
                return new List<FundBasicDto>();
            }
            return datas;
        }
        /// <summary>
        /// 取得全部基金折線圖資料
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<FundNavDto> GetChartData(List<string> ids)
        {

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < ids.Count; ++i)
            {
                string productCode = ids[i];
                stringBuilder.Append($@"
                                    SELECT * FROM (
                                            SELECT TOP (30)
                                                FirstBankCode AS ProductCode,
                                                NetAssetValueDate,
                                                NetAssetValue
                                            FROM Sysjust_Nav_Fund
                                    WHERE FirstBankCode = '{productCode}'
                                    ORDER BY [NetAssetValueDate] DESC) AS [{productCode}]");

                if (i < ids.Count - 1)
                {
                    stringBuilder.Append(" UNION ALL");
                }
            }

            var datas = DbManager.Custom.ExecuteIList<FundNavDto>(stringBuilder.ToString(), null, CommandType.Text)?.ToList();
            if (datas == null)
            {
                return new List<FundNavDto>();
            }
            return datas;
        }
        /// <summary>
        /// 取得同年齡層卡片基金資料
        /// </summary>
        /// <param name="age"></param>
        /// <returns></returns>
        public IEnumerable<FundBasicDto> GetSameAgeCard(int age)
        {
            age = age < 10 ? age : age / 10;
            string sql = $@"SELECT TOP(3) ProductCode,FundName,AvailabilityStatus,OnlineSubscriptionAvailability,OneMonthReturnOriginalCurrency FROM vw_BasicFund
                        WHERE ProductCode IN (SELECT value from (SELECT FUND_ID FROM Wms_age_profile_d_mf WHERE AGE=@age) A cross apply STRING_SPLIT (a.FUND_ID,','))
                        ORDER BY SixMonthReturnOriginalCurrency DESC";
            var para = new { age };

            var datas = DbManager.Custom.ExecuteIList<FundBasicDto>(sql, para, CommandType.Text)?.ToList();
            if (datas == null)
            {
                return new List<FundBasicDto>();
            }
            return datas;
        }
        /// <summary>
        /// 取得同星座卡片基金資料
        /// </summary>
        /// <param name="zodiacCode"></param>
        /// <returns></returns>
        public IEnumerable<FundBasicDto> GetZodiacCard(int zodiacCode)
        {

            string sql = $@"SELECT TOP(3) ProductCode,FundName,AvailabilityStatus,OnlineSubscriptionAvailability,OneMonthReturnOriginalCurrency FROM vw_BasicFund
                         WHERE ProductCode IN (SELECT value from (SELECT FUND_ID FROM Wms_zodiac_profile_d_mf WHERE ZODIAC=@zodiac) A cross apply STRING_SPLIT (a.FUND_ID,','))
                         ORDER BY SixMonthReturnOriginalCurrency DESC";
            var para = new { zodiac = zodiacCode };

            var datas = DbManager.Custom.ExecuteIList<FundBasicDto>(sql, para, CommandType.Text)?.ToList();
            if (datas == null)
            {
                return new List<FundBasicDto>();
            }
            return datas;
        }
        /// <summary>
        /// 計算年齡
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public int CalculateAge(DateTime? date)
        {
            if (!date.HasValue)
            {
                return 0;
            }
            DateTime today = DateTime.Today;
            int age = (int)(today.Year - date?.Year);

            if (date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
        /// <summary>
        /// 計算星座
        /// </summary>
        /// <param name="birthday"></param>
        /// <returns></returns>
        public int CalculateZodiac(DateTime? birthday)
        {
            int? day = birthday?.Day;
            int? month = birthday?.Month;

            if ((month == 1 && day >= 20) || (month == 2 && day <= 18))
                return (int)ZodiacEnum.水瓶座;
            else if ((month == 2 && day >= 19) || (month == 3 && day <= 20))
                return (int)ZodiacEnum.雙魚座;
            else if ((month == 3 && day >= 21) || (month == 4 && day <= 19))
                return (int)ZodiacEnum.牡羊座;
            else if ((month == 4 && day >= 20) || (month == 5 && day <= 20))
                return (int)ZodiacEnum.金牛座;
            else if ((month == 5 && day >= 21) || (month == 6 && day <= 20))
                return (int)ZodiacEnum.雙子座;
            else if ((month == 6 && day >= 21) || (month == 7 && day <= 22))
                return (int)ZodiacEnum.巨蟹座;
            else if ((month == 7 && day >= 23) || (month == 8 && day <= 22))
                return (int)ZodiacEnum.獅子座;
            else if ((month == 8 && day >= 23) || (month == 9 && day <= 22))
                return (int)ZodiacEnum.處女座;
            else if ((month == 9 && day >= 23) || (month == 10 && day <= 22))
                return (int)ZodiacEnum.天秤座;
            else if ((month == 10 && day >= 23) || (month == 11 && day <= 21))
                return (int)ZodiacEnum.天蠍座;
            else if ((month == 11 && day >= 22) || (month == 12 && day <= 21))
                return (int)ZodiacEnum.射手座;
            else
                return (int)ZodiacEnum.摩羯座;
        }
    }
}
