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
    public class RegController : ControllerBase
    {
        private readonly AccountService accountService = new AccountService();

        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("AddAccount")]
        public IActionResult AddAccount(string account, string pwd)
        {
            return Ok(accountService.AddAccount(account, pwd));
        }

        /// <summary>
        /// 激活账号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RegAccount")]
        public IActionResult RegAccount(string account, string regCode)
        {
            return Ok(accountService.RegAccount(account, regCode));
        }

        /// <summary>
        /// 激活设备(本地版)
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RegMachine")]
        public IActionResult RegMachine(string machineCode, string regCode)
        {
            return Ok(accountService.RegMachine(machineCode, regCode));
        }

        /// <summary>
        /// 获取注册码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRegCode")]
        public IActionResult GetRegCode(int codeType, int time)
        {
            return Ok(accountService.GetRegCode(codeType, time));
        }

        /// <summary>
        /// 添加注册码
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddRegCode")]
        public IActionResult AddRegCode(List<CodeInfo> models)
        {
            return Ok(accountService.AddRegCode(models));
        }
    }
}