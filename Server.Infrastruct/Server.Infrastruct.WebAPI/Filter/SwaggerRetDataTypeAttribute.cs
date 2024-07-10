namespace Server.Infrastruct.WebAPI.Filter
{
    public class SwaggerRetDataTypeAttribute : Attribute
    {
        public Type RetType;
        public SwaggerRetDataTypeAttribute(Type type)
        {
            RetType = type;
        }
    }
}
