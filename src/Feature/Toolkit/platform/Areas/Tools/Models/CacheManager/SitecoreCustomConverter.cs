using Newtonsoft.Json;
using Sitecore.Data.Items;
using System;
using System.Linq;

namespace Feature.Wealth.Toolkit.Areas.Tools.Models.CacheManager
{
    public class SitecoreCustomConverter : JsonConverter
    {
        private readonly Type[] _types = { typeof(Item), typeof(DateTime) };

        public override bool CanConvert(Type objectType)
        {
            bool result = this._types.Any(t => t == objectType);
            return result;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => null;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = value.GetType();
            if (type == typeof(Item))
            {
                if (value is Item data)
                {
                    serializer.Serialize(writer, $"{data.ID}|{data.DisplayName}");
                }
                else
                {
                    serializer.Serialize(writer, "null sitecore item");
                }
            }

            if (type == typeof(DateTime))
            {
                var date = (DateTime)value;
                serializer.Serialize(writer, date.ToString("yyyy-MM-dd hh:mm:ss"));
            }
        }
    }
}