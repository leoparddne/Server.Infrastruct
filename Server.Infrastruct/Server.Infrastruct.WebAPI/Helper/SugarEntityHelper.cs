using SqlSugar;

namespace Server.Infrastruct.WebAPI.Helper
{
    public static class SugarEntityHelper
    {
        public static string GetTableName(Type type)
        {
            var attr = type.GetCustomAttributes(false).FirstOrDefault(f => f.GetType() == typeof(SugarTable));
            if (attr == null)
            {
                throw new Exception("ErrorTableName");
            }
            if (attr is SugarTable sugarColumn)
            {
                return sugarColumn.TableName;
            }

            throw new Exception("ErrorTableName");
        }

        public static string GetSugarNameByEntityNamByObj(object obj, string fieldName)
        {
            var props = obj.GetType().GetProperties().Where(f => f.Name == fieldName);
            foreach (var prop in props)
            {
                var attr = prop.CustomAttributes.FirstOrDefault(f => f.AttributeType == typeof(SugarColumn));
                if (attr != null)
                {
                    var arg = attr.NamedArguments.FirstOrDefault(f => f.MemberName == "ColumnName");
                    if (arg != null)
                    {
                        return arg.TypedValue.Value.ToString();
                    }
                }
            }

            var fields = obj.GetType().GetFields().Where(f => f.Name == fieldName);

            foreach (var field in fields)
            {
                var attr = field.CustomAttributes.FirstOrDefault(f => f.AttributeType == typeof(SugarColumn));
                if (attr != null)
                {
                    var arg = attr.NamedArguments.FirstOrDefault(f => f.MemberName == "ColumnName");
                    if (arg != null)
                    {
                        return field.Name;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 根据实体字段名称获取数据库字段名称
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetSugarNameByEntityName<TIn>(string fieldName)
        {
            var props = typeof(TIn).GetProperties().Where(f => f.Name == fieldName).ToList();
            foreach (var prop in props)
            {
                var attr = prop.CustomAttributes.FirstOrDefault(f => f.AttributeType == typeof(SugarColumn));
                if (attr != null)
                {
                    var arg = attr.NamedArguments.FirstOrDefault(f => f.MemberName == "ColumnName");
                    if (arg != null)
                    {
                        return arg.TypedValue.Value.ToString();
                    }
                }
            }

            var fields = typeof(TIn).GetFields().Where(f => f.Name == fieldName).ToList();

            foreach (var field in fields)
            {
                var attr = field.CustomAttributes.FirstOrDefault(f => f.AttributeType == typeof(SugarColumn));
                if (attr != null)
                {
                    var arg = attr.NamedArguments.FirstOrDefault(f => f.MemberName == "ColumnName");
                    if (arg != null)
                    {
                        return field.Name;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 根据数据库字段获取对应字段类型
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static Type GetSugarFieldTypeByName<TIn>(string fieldName)
        {
            var props = typeof(TIn).GetProperties();
            foreach (var prop in props)
            {
                var attr = prop.CustomAttributes.FirstOrDefault(f => f.AttributeType == typeof(SugarColumn));
                if (attr != null)
                {
                    var arg = attr.NamedArguments.FirstOrDefault(f => f.MemberName == "ColumnName");
                    if (arg != null)
                    {
                        //arg.TypedValue
                        if (arg.TypedValue.Value.ToString().ToLower() == fieldName.ToLower())
                        {
                            return prop.PropertyType;
                        }
                    }
                }
            }

            var fields = typeof(TIn).GetFields();

            foreach (var field in fields)
            {
                var attr = field.CustomAttributes.FirstOrDefault(f => f.AttributeType == typeof(SugarColumn));
                if (attr != null)
                {
                    var arg = attr.NamedArguments.FirstOrDefault(f => f.MemberName == "ColumnName");
                    if (arg != null)
                    {
                        //arg.TypedValue
                        if (arg.TypedValue.Value.ToString().ToLower() == fieldName.ToLower())
                        {
                            return field.FieldType;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 获取数据库字段对应的实体字段名称
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetFieldNameBySugarName<TIn>(string fieldName)
        {
            var props = typeof(TIn).GetProperties();
            foreach (var prop in props)
            {
                var attr = prop.CustomAttributes.FirstOrDefault(f => f.AttributeType == typeof(SugarColumn));
                if (attr != null)
                {
                    var arg = attr.NamedArguments.FirstOrDefault(f => f.MemberName == "ColumnName");
                    if (arg != null)
                    {
                        //arg.TypedValue
                        if (arg.TypedValue.Value.ToString().ToLower() == fieldName.ToLower())
                        {
                            return prop.Name;
                        }
                    }
                }
            }

            var fields = typeof(TIn).GetFields();

            foreach (var field in fields)
            {
                var attr = field.CustomAttributes.FirstOrDefault(f => f.AttributeType == typeof(SugarColumn));
                if (attr != null)
                {
                    var arg = attr.NamedArguments.FirstOrDefault(f => f.MemberName == "ColumnName");
                    if (arg != null)
                    {
                        //arg.TypedValue
                        if (arg.TypedValue.Value.ToString().ToLower() == fieldName.ToLower())
                        {
                            return field.Name;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 根据数据库字段名称设置对应的字段值
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="data"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetSugarFieldTypeByName<TIn>(TIn data, string fieldName, object value)
        {
            var props = typeof(TIn).GetProperties();
            foreach (var prop in props)
            {
                var attr = prop.CustomAttributes.FirstOrDefault(f => f.AttributeType == typeof(SugarColumn));
                if (attr != null)
                {
                    var arg = attr.NamedArguments.FirstOrDefault(f => f.MemberName == "ColumnName");
                    if (arg != null)
                    {
                        //arg.TypedValue
                        if (arg.TypedValue.Value.ToString().ToLower() == fieldName.ToLower())
                        {
                            prop.SetValue(data, value);
                        }
                    }
                }
            }

            var fields = typeof(TIn).GetFields();

            foreach (var field in fields)
            {
                var attr = field.CustomAttributes.FirstOrDefault(f => f.AttributeType == typeof(SugarColumn));
                if (attr != null)
                {
                    var arg = attr.NamedArguments.FirstOrDefault(f => f.MemberName == "ColumnName");
                    if (arg != null)
                    {
                        //arg.TypedValue
                        if (arg.TypedValue.Value.ToString().ToLower() == fieldName.ToLower())
                        {
                            field.SetValue(data, value);
                        }
                    }
                }
            }
        }
    }
}
