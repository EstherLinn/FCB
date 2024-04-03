using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Wealth.Component.Models.FundDetail
{
    public class FundReturnCompare
    {
        public string FirstBankCode { get; set; }
        public int OneMonthRank { get; set; }
        public int ThreeMonthRank { get; set; }
        public int SixMonthRank { get; set; }
        public int TwoYearRank { get; set; }
        public int ThreeYearRank { get; set; }
        public decimal? OneMonthReturn { get; set; }
        public decimal? ThreeMonthReturn { get; set; }
        public decimal? SixMonthReturn { get; set; }
        public decimal? TwoYearReturn { get; set; }
        public decimal? ThreeYearReturn { get; set; }
        public decimal? OneYearReturn { get; set; }
        public decimal? OneYearAvg { get; set; }
        public int OneYearRank { get; set; }
        public int OneYearCount { get; set; }
        public int FundCount { get; set; }
        public decimal? OneMonthAvg { get; set; }
        public decimal? ThreeMonthAvg { get; set; }
        public decimal? SixMonthAvg { get; set; }
        public decimal? TwoYearAvg { get; set; }
        public decimal? ThreeYearAvg { get; set; }
        public decimal? OneMonthRankWinFund { get; set; }
        public decimal? ThreeMonthWinFund { get; set; }
        public decimal? SixMonthWinFund { get; set; }
        public decimal? OneYearWinFund { get; set; }
        public decimal? TwoYearWinFund { get; set; }
        public decimal? ThreeYearWinFund { get; set; }
    }
}
