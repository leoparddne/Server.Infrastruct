namespace Server.Infrastruct.WebAPI.Extension.SqlSugar
{
    public static class SqlSugarFunction
    {
        public static string OracleNVLNull<T>(T str)
        {
            throw new NotSupportedException("Can only be used in expressions");
        }

        public static string OracleDecode<T>(T str1, T str2)
        {
            throw new NotSupportedException("Can only be used in expressions");
        }
    }
}
