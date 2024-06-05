namespace Feature.Wealth.Component.Models.ETF
{
    public class RespEtf
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public dynamic Body { get; set; }
    }
}