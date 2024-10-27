using System.ComponentModel;

namespace Server.DBEx.Model.Enums
{
    /// <summary>
    /// 数据库连接方式
    /// </summary>
    public enum ConnectTypeEnum
    {
        /// <summary>
        /// Oracle驱动
        /// </summary>
        [Description("Oracle驱动")]
        Oracle = 0,

        /// <summary>
        /// SQLServer驱动
        /// </summary>
        [Description("SQLServer驱动")]
        SQLServer = 1,

        /// <summary>
        /// MySQL驱动
        /// </summary>
        [Description("MySQL驱动")]
        MySQL = 2,

        /// <summary>
        /// Oleb驱动
        /// </summary>
        [Description("Oledb驱动")]
        Oleb = 3,

        /// <summary>
        /// ODBC驱动
        /// </summary>
        [Description("ODBC驱动")]
        ODBC = 4,
        Sqlite = 5,

        [Description("PGSql")]
        PGSql = 6
    }
}
