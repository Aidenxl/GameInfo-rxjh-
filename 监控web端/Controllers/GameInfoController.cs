using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Models.ApiRequestModel;
using Models.ApiReturnModel;
using Models.Helper;
using Newtonsoft.Json;
using Services;

namespace 监控web端.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameInfoController : ControllerBase
    {
        private readonly LoginService loginService;

        public GameInfoController()
        {
            loginService = new LoginService();
        }

        /// <summary>
        /// 获取游戏信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetGameInfo")]
        public IActionResult GetGameInfo(GetGameInfo model)
        {
            var result = new ReturnGameInfo();
            result.GameInfoModel = null;
            result.Status = loginService.CheckLoginStatus(model.Account, model.Token);
            if (result.Status == 1)
            {
                var infoModelValues = GetInfoModelValues();
                if (infoModelValues != null && infoModelValues.InfoModels.Count > 0)
                {
                    result.GameInfoModel = infoModelValues;
                }
            }
            return Ok(result);
        }

        private GameInfoModel GetInfoModelValues()
        {
            GameInfoModel homePageModel = new GameInfoModel();
            try
            {
                //var keyvalues = StaticRedisHelp.GetRedis().GetLikeKeyValue(LoginInfo.UserId + ":*");
                var keyvalues = StaticRedisHelp.GetRedis().GetLikeKeyValue();
                if (keyvalues != null && keyvalues.Count > 0)
                {
                    var list = keyvalues.Where(i => i.Value.Contains("Account")).Select(i => JsonConvert.DeserializeObject<InfoModel>(i.Value)).OrderBy(i => i.Account);
                    homePageModel.TotilGold = "游戏币总计：" + list.Sum(i => i.CurrentGold) / 10000 + "万";
                    homePageModel.InfoModels = list.ToList();
                }
                else
                {
                    StaticRedisHelp.RestRedis();
                }
                return homePageModel;

            }
            catch (Exception ex)
            {
                var filePath = $"Errorlog /{ DateTime.Now.ToString("yyyy-MM-dd")}.txt";
                if (!Directory.Exists("Errorlog"))
                {
                    Directory.CreateDirectory("Errorlog");
                }
                if (!System.IO.File.Exists(filePath))
                {
                    using (System.IO.File.Create(filePath))
                    {

                    }
                }
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(DateTime.Now + ex.ToString());
                }
                StaticRedisHelp.RestRedis();
                return null;
            }
        }
    }
}