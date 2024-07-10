namespace Server.Infrastruct.WebAPI.Repository.Filter
{
    public class HistoryBindAttribute : Attribute
    {
        public Type DBEntityType;

        public HistoryBindAttribute(Type type)
        {
            DBEntityType = type;
        }
    }
}
