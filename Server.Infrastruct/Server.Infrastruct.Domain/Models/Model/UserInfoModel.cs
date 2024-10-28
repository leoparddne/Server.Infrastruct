using Server.Infrastruct.Model.Models.Enums;

namespace Server.Infrastruct.Model.Models.Model
{
    public class UserInfoModel
    {
        /// <summary>
        /// APPId
        /// </summary>
        public string APPId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string APPName { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserNo { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        public string LanguageCode { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 系统类型
        /// </summary>
        public SystemTypeEnum SystemType { get; set; }


        /// <summary>
        /// token类型(对应不同的生成、保存规则、校验模式)
        /// </summary>
        public TokenTypeEnum TokenType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 租户id
        /// </summary>
        public string TenantId { get; set; }
        public bool ISAdmin { get; set; }
    }
}
