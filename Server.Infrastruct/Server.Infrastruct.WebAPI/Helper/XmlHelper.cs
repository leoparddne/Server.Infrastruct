using Newtonsoft.Json;

namespace Server.Infrastruct.WebAPI.Helper
{
    public static class XmlHelper
    {

        /// <summary>
        /// xml转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strXML"></param>
        /// <returns></returns>
        public static T XMLTOObject<T>(string strXML) where T : class
        {
            var xml = new System.Xml.XmlDocument();
            xml.LoadXml(strXML);
            return JsonConvert.DeserializeObject<T>(xml.InnerText);
        }
    }
}
