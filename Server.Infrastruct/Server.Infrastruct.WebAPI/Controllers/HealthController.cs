using Common.Toolkit.Helper;
using Microsoft.AspNetCore.Mvc;
using Server.Infrastruct.WebAPI.BuilderExtension;
using Server.Infrastruct.WebAPI.Filter;
using Server.Infrastruct.WebAPI.Model;
using Server.Infrastruct.WebAPI.Model.Dto.OutDto;
using Server.Infrastruct.WebAPI.ServiceExtension.Redis;

namespace Server.Infrastruct.WebAPI.Controllers
{
    /// <summary>
    /// Consul心跳监测
    /// </summary>
    [Route("api/[controller]/[action]")]
    [APIResultFilter]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Consul心跳监测
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiIgnoreAttribute]
        public ActionResult Index()
        {
            return Ok();
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ServerInfoOutDto GetServerInfo()
        {
            return new();
        }

        /// <summary>
        /// 服务验证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiIgnoreAttribute]
        public ActionResult Check()
        {
            Dictionary<string, string> result = new();
            try
            {
                RedisService redisService = new();
                result.Add("redis", "ok");
            }
            catch (System.Exception ex)
            {
                result.Add("redis", "!!!!!!!!!error!!!!!!!!!" + ex.StackTrace ?? "");
            }

            var consulAlive = ConsulBuilderExtension.CheckAlive();
            result.Add("consul", consulAlive ? "ok" : GenerateErr("not alive"));

            string ApplicationIP = AppSettingsHelper.GetSetting("Publish", "ApplicationIP");
            string ApplicationPort = AppSettingsHelper.GetSetting("Publish", "ApplicationPort");
            string ConsulName = AppSettingsHelper.GetSetting("Publish", "ConsulName");
            string GateWay = DBConfigSingleton.GetConfig().GateWay;


            return Ok(new
            {
                ServerCheck = result,
                Config = new
                {
                    ApplicationIP,
                    ApplicationPort,
                    ConsulName,
                    GateWay
                },
                ServerTime = DateTime.Now
            });
        }

        private string GenerateErr(string msg)
        {
            return $"!!!!!!!!!!!!{msg}!!!!!!!!!!!!";
        }
    }
}
