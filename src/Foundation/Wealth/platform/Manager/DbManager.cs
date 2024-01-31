using System;
using System.Configuration;
using Xcms.Sitecore.Foundation.Basic.DbContext;

namespace Foundation.Wealth.Manager
{
    public static class DbManager
    {
        [ThreadStatic]
        private static IDataAccess _custom;


        public static IDataAccess Custom
        {
            get
            {
                if (_custom != null)
                {
                    return _custom;
                }

                var cp = new ConnectionParameter
                {
                    ConnectionSetting = ConfigurationManager.ConnectionStrings["custom"],
                    DataBaseType = DataBaseTypeEnum.MsSql
                };
                _custom = DataAccessFactory.Create(cp);
                return _custom;
            }
        }
    }
}