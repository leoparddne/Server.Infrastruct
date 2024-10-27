namespace Server.Infrastruct.Filter
{
    /// <summary>
    /// 是否需要包含返回值通用结构
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ApiIgnoreAttribute : Attribute
    {
    }
}
