namespace Server.Infrastruct.Helper.Consul
{
    public class ConsulConfig
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public string ApplicationIP { get; set; }
        public int ApplicationPort { get; set; }
        public string ConsulIP { get; set; }
        public int ConsulPort { get; set; }
        public string ConsulName { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
    }
}
