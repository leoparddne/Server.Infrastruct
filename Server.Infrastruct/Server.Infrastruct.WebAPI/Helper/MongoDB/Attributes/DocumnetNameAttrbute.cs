namespace Server.Infrastruct.WebAPI.Helper.MongoDB.Attributes
{
    public class DocumnetNameAttrbute : Attribute
    {
        /// <summary>
        /// 文档名称
        /// </summary>
        public string DocumentName { get; set; }
        public DocumnetNameAttrbute(string documentName)
        {
            this.DocumentName = documentName;
        }

        /// <summary>
        /// 传入T类型获取绑定的文档名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetDocumentName<T>()
        {
            var attr = typeof(T).GetCustomAttributes(false);
            if (attr != null)
            {
                foreach (var item in attr)
                {
                    if (item is DocumnetNameAttrbute nameAttr)
                    {
                        return nameAttr.DocumentName;
                    }
                }
            }

            return string.Empty;
        }

    }
}
