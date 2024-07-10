using SqlSugar;

namespace Server.Infrastruct.WebAPI.Extension.SqlSugar
{
    public static class SqlSugerEx
    {
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="isOutput"></param>
        public static void AddSugarParameter(this List<SugarParameter> parameters, string name, object value, bool isOutput = false)
        {
            if (isOutput == false && value != null)
            {
                if (value.GetType() == typeof(int))
                {
                    parameters.Add(new SugarParameter(name, value, System.Data.DbType.Int32));
                    return;
                }
                if (value.GetType() == typeof(double))
                {
                    parameters.Add(new SugarParameter(name, value, System.Data.DbType.Double));
                    return;
                }
            }
            parameters.Add(new SugarParameter(name, value, isOutput));
        }
    }
}
