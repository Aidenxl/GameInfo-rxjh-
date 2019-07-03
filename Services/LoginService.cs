using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Models.Helper;
using Models.DBModel;
using Models.ApiReturnModel;

namespace Services
{
    public class LoginService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public ReturnBase<ReturnLoginInfo> Login(string account, string pwd)
        {
            var result = new ReturnBase<ReturnLoginInfo>();
            string condition = "[Account]=@Account and [Password]=@Password";
            List<SqlParameter> parameters = new List<SqlParameter>() {
                new SqlParameter("@Account", account),
                new SqlParameter("@Password", pwd),
            };
            User userInfo = new User();
            userInfo = userInfo.QueryByCondition(condition, parameters);

            if (userInfo != null)
            {
                if (userInfo.OutTime <= DateTime.Now)
                {
                    result.Data = null;
                    result.IsSuccess = false;
                    result.Message = "账号已到期，请激活";
                    return result;
                }
                ReturnLoginInfo loginInfo = new ReturnLoginInfo();
                loginInfo.Account = userInfo.Account;
                loginInfo.OutTime = userInfo.OutTime;
                loginInfo.Token = (loginInfo.Account + loginInfo.OutTime + DateTime.Now).MD5Encrypt32();
                UpdateLoginInfo(loginInfo);
                result.Data = loginInfo;
                result.IsSuccess = true;
                return result;
            }
            return null;

        }

        /// <summary>
        /// 信息采集端登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public ReturnBase<string> ClientLogin(string account, string pwd, string deviceCode)
        {
            ReturnBase<string> result = new ReturnBase<string>();
            if (Login(account, pwd).IsSuccess)
            {
                if (CheckAccountDevice(account, deviceCode))
                {
                    result.IsSuccess = true;
                    result.Message = "成功";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "登录失败,该设备未绑定";
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "登录失败";
            }
            return result;
        }

        /// <summary>
        /// 更新登录信息
        /// </summary>
        /// <param name="model"></param>
        private void UpdateLoginInfo(ReturnLoginInfo model)
        {
            LoginInfo loginInfo = new LoginInfo() { Account = model.Account, OutTime = model.OutTime, Token = model.Token };
            var result = SelectDbLoginInfo(loginInfo.Account);
            string sql = string.Empty;
            if (result == null)
            {
                loginInfo.Add();
            }
            else
            {
                loginInfo.Id = result.Id;
                loginInfo.Update();
            }
        }

        /// <summary>
        /// 根据账号查询登录信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public LoginInfo SelectDbLoginInfo(string account)
        {
            LoginInfo loginInfo = new LoginInfo();
            string condition = "[Account]=@Account";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@Account", account) };
            var result = loginInfo.QueryByCondition(condition, parameters);

            if (result != null)
            {
                loginInfo.Account = result.Account;
                loginInfo.OutTime = result.OutTime;
                loginInfo.Token = result.Token;
                return loginInfo;
            }
            return null;

        }

        /// <summary>
        /// 检查登录状态
        /// </summary>
        /// <param name="account"></param>
        /// <param name="token"></param>
        /// <returns>返回状态码 1成功 -1 token不正确 -2账号过期  0未登录</returns>
        public int CheckLoginStatus(string account, string token)
        {
            if (string.IsNullOrEmpty(account))
            {
                account = "";
            }
            if (string.IsNullOrEmpty(token))
            {
                token = "";
            }
            var result = SelectDbLoginInfo(account.Trim());
            if (result != null)
            {
                if (result.Token.Trim() != token.Trim())
                {
                    return -1;
                }
                if (result.OutTime < DateTime.Now)
                {
                    return -2;
                }
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 检查本地版设备激活状态
        /// </summary>
        /// <returns></returns>
        public ReturnBase CheckMachineRegStatus(string machineCode)
        {
            var result = new ReturnBase();
            var rCode = new RegCode();

            string condition = "[UseAccountOrMachine]=@UseAccountOrMachine and [CodeType]=0";
            rCode = rCode.QueryByCondition(condition, new List<SqlParameter>() { new SqlParameter("@UseAccountOrMachine", machineCode) });
            result.IsSuccess = rCode != null;
            return result;
        }

        /// <summary>
        /// 检查账号和设备绑定列表中有没有当前数据
        /// </summary>
        /// <param name="account"></param>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        private bool CheckAccountDevice(string account, string deviceCode)
        {
            AccountDevie accountDevie = new AccountDevie();

            string condition = "[Account]=@Account and [DeviceCode]=@DeviceCode";
            List<SqlParameter> sqlParameter = new List<SqlParameter>() { new SqlParameter("@Account", account), new SqlParameter("@DeviceCode", deviceCode) };

            var result = accountDevie.QueryByCondition(condition, sqlParameter);

            return result != null;
        }
    }
}
