using System;
using System.Collections.Generic;
using System.Text;

namespace ds.pms.apicommon.Settings
{
    /// <summary>
    /// ConnectionString API settings
    /// </summary>
    public class ConnectionSettings
    {
        /// <summary>
        /// SQL provider name (MySql, PostgreSql96...)
        /// </summary>
        public string OperationsDbConnString { get; set; }
        public string OperationsDbSqlProvider { get; set; }
    }
}
