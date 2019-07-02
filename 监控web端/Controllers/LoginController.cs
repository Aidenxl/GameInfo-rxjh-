using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.ApiRequestModel;
using Services;

namespace 监控web端.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService loginService = new LoginService();

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody]UserLoginInfo model)
        {
            var result = loginService.Login(model.Account.Trim(), model.Password.Trim());
            return Ok(result);
        }



        /// <summary>
        /// 本地版验证激活状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckMachineRegStatus")]
        public IActionResult CheckMachineRegStatus(string machineCode)
        {
            return Ok(loginService.CheckMachineRegStatus(machineCode));
        }
    }
}