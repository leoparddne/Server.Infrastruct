namespace Server.Infrastruct.Filter
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HistoryBindAttribute : Attribute
    {
        public Type DBEntityType;

        public HistoryBindAttribute(Type type)
        {
            DBEntityType = type;
        }
    }
}
