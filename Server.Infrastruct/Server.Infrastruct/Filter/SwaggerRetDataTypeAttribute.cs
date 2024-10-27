namespace Server.Infrastruct.Filter
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SwaggerRetDataTypeAttribute : Attribute
    {
        public Type RetType;
        public SwaggerRetDataTypeAttribute(Type type)
        {
            RetType = type;
        }
    }
}
