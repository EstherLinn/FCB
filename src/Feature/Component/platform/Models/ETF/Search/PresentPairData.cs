using System;

namespace Feature.Wealth.Component.Models.ETF.Search
{
    public class Percentage : PresentBase
    {
        public Percentage() { }

        public bool IsUp { get; set; }
        public decimal? Value { get; set; }
        public string Style { get; set; }
    }

    public class VolumePair : PresentBase
    {
        public VolumePair() { }

        public decimal? Value { get; set; }
    }

    public class IdValuePair : PresentBase
    {
        public IdValuePair() { }

        public int Key { get; set; }
        public string Value { get; set; }
    }

    public class StringPair : PresentBase
    {
        public StringPair() { }

        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class IdPair : PresentBase
    {
        public IdPair() { }

        public int Value { get; set; }
    }

    public class PresentBase
    {
        public string Text { get; set; }
    }
}
