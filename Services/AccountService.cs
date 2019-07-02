using Models.ApiRequestModel;
using Models.ApiReturnModel;
using Models.DBModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Services
{
    public class AccountService
    {
        /// <summary>
        /// 批量添加注册卡
        /// </summary>
        /// <param name="models"></param>
        public ReturnBase AddRegCode(List<CodeInfo> models)
        {
            var result = new ReturnBase();

            var codeList = new List<RegCode>();

            foreach (var item in models)
            {
                codeList.Add(new RegCode()
                {
                    CodeType = item.CodeType,
                    Code = item.RegCode,
                    Term = item.Time,
                    UseAccountOrMachine = string.Empty,
                    IsUse = 0
                });
            }

            result.IsSuccess = codeList.AddRange();
            return result;
        }

        /// <summary>
        /// 获得注册卡
        /// </summary>
        /// <param name="codeType"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public string GetRegCode(int codeType, int time)
        {
            RegCode regCode = new RegCode();
            string condition = "[CodeType]=@CodeType and [Term]=@Term and IsUse=0";
            var parameters = new List<SqlParameter>() { new SqlParameter("@CodeType", codeType), new SqlParameter("@Term", time) };
            var result = regCode.QueryByCondition(condition, parameters);
            if (result != null)
            {
                return result.Code;
            }
            else
            {
                return "未提取到符合条件的激活码";
            }
        }

        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public ReturnBase AddAccount(string account, string pwd)
        {
            var result = new ReturnBase();
            var user = new User();
            var condition = "[Account]=@Account";
            user = user.QueryByCondition(condition, new List<SqlParameter>() { new SqlParameter("@Account", account) });
            if (user != null)
            {
                result.Message = "已存在相同账号";
                result.IsSuccess = false;
                return result;
            }
            user = new User();
            user.Account = account;
            user.Password = pwd;
            user.OutTime = DateTime.Now;
            result.IsSuccess = user.Add();
            return result;
        }

        /// <summary>
        /// 网络版激活
        /// </summary>
        /// <param name="account"></param>
        /// <param name="regCode"></param>
        /// <returns></returns>
        public ReturnBase RegAccount(string account, string regCode)
        {
            var result = new ReturnBase();
            var rCode = new RegCode();

            string condition = "[Code]=@Code and [IsUse]=0 and [CodeType]=1";
            rCode = rCode.QueryByCondition(condition, new List<SqlParameter>() { new SqlParameter("@Code", regCode) });
            if (rCode == null)
            {
                result.IsSuccess = false;
                result.Message = "激活码不存在或已经被使用";
                return result;
            }

            var user = new User();
            condition = "[Account]=@Account";
            user = user.QueryByCondition(condition, new List<SqlParameter>() { new SqlParameter("@Account", account) });
            if (user != null)
            {
                if (user.OutTime > DateTime.Now)
                {
                    user.OutTime = user.OutTime.AddDays(rCode.Term);
                }
                else
                {
                    user.OutTime = DateTime.Now.AddDays(rCode.Term);
                }
                rCode.IsUse = 1;
                rCode.UseAccountOrMachine = user.Account;
                user.Update();
                rCode.Update();
                result.IsSuccess = true;
                result.Message = $"激活成功，账号使用时间增加{rCode.Term}天";
                return result;
            }
            result.IsSuccess = false;
            result.Message = "用户不存在";
            return result;
        }

        /// <summary>
        /// 本地版激活
        /// </summary>
        /// <param name="account"></param>
        /// <param name="regCode"></param>
        /// <returns></returns>
        public ReturnBase RegMachine(string machineCode, string regCode)
        {
            var result = new ReturnBase();
            var rCode = new RegCode();

            string condition = "[Code]=@Code and [IsUse]=0 and [CodeType]=0";
            rCode = rCode.QueryByCondition(condition, new List<SqlParameter>() { new SqlParameter("@Code", regCode) });
            if (rCode == null)
            {
                result.IsSuccess = false;
                result.Message = "激活码不存在或已经被使用";
                return result;
            }

            rCode.IsUse = 1;
            rCode.UseAccountOrMachine = machineCode;
            rCode.Update();
            result.IsSuccess = true;
            result.Message = $"设备绑定成功";
            return result;

        }
    }
}
