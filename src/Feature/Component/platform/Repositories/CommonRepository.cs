using Feature.Wealth.Component.Models;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Feature.Wealth.Component.Repositories
{
    public class CommonRepository
    {
        /// <summary>
        /// 取得Cache過期時間
        /// </summary>
        /// <param name="CacheTime">Cache 時間字串</param>
        /// <returns>Cache過期時間</returns>
        public DateTime GetCacheExpireTime(string CacheTime)
        {
            DateTime expireTime;
            TimeUnit timeUnit;

            if (!string.IsNullOrEmpty(CacheTime))
            {
                Regex eg = new Regex("[a-z]");

                if (eg.IsMatch(CacheTime))
                {
                    timeUnit = (TimeUnit)Enum.Parse(typeof(TimeUnit), eg.Match(CacheTime).Value);

                    var time = CacheTime.Replace(timeUnit.ToString(), "");

                    double offset = double.Parse(time);
                    switch (timeUnit)
                    {
                        case TimeUnit.s:
                            expireTime = DateTime.Now.AddSeconds(offset);
                            break;
                        case TimeUnit.m:
                            expireTime = DateTime.Now.AddMinutes(offset);
                            break;
                        case TimeUnit.h:
                            expireTime = DateTime.Now.AddHours(offset);
                            break;
                        case TimeUnit.d:
                            expireTime = DateTime.Now.AddDays(offset);
                            break;
                        default:
                            expireTime = DateTime.Now.AddHours(1);
                            break;
                    }
                }
                else
                {
                    expireTime = DateTime.Now.AddHours(1);
                }
            }
            else
            {
                expireTime = DateTime.Now.AddHours(1);
            }

            return expireTime;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                    {
                        return field.Name;
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return field.Name;
                    }
                }
            }
            return null;
        }
    }
}
