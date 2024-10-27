namespace Server.Infrastruct.Helper.Consul
{
    public class ConsulSetting
    {
        public int AgentServiceCheckInterval { get; set; }
        public int AgentServiceCheckTimeout { get; set; }
        public int AgentServiceCheckDeregister { get; set; }
    }

}
