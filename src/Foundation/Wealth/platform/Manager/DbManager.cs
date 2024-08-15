using System;
using System.Configuration;
using Xcms.Sitecore.Foundation.Basic.DbContext;

namespace Foundation.Wealth.Manager
{
    public static class DbManager
    {
        [ThreadStatic]
        private static IDataAccess _custom;

        [ThreadStatic]
        private static IDataAccess _cif;

        [ThreadStatic]
        private static IDataAccess _master;

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
                    ConnectionString = ConfigurationManager.ConnectionStrings["custom"].ConnectionString,
                    DataBaseType = DataBaseType.MsSql
                };
                _custom = DataAccessFactory.Create(cp);
                return _custom;
            }
        }

        public static IDataAccess Cif
        {
            get
            {
                if (_cif != null)
                {
                    return _cif;
                }

                var cp = new ConnectionParameter
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["cif"].ConnectionString,
                    DataBaseType = DataBaseType.Oracle
                };
                _cif = DataAccessFactory.Create(cp);
                return _cif;
            }
        }

        public static IDataAccess Master
        {
            get
            {
                if (_master != null)
                {
                    return _master;
                }

                var cp = new ConnectionParameter
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["master"].ConnectionString,
                    DataBaseType = DataBaseType.MsSql
                };
                _master = DataAccessFactory.Create(cp);
                return _master;
            }
        }
    }
}