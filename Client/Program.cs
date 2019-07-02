using Models;
using Models.ApiReturnModel;
using Models.Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace MonitorClient
{
    class Program
    {
        private static string baseUrl = "118.24.18.189:81";
        private static RedisHelper redis;
        private static string Account { get; set; }
        private static string Password { get; set; }
        static void Main(string[] args)
        {
            Console.WriteLine("请输入账号");
            Account = Console.ReadLine();
            Console.WriteLine("请输入密码");
            Account = Console.ReadLine();
            HttpClient httpClient = new HttpClient();
            textBox1.Text = httpClient.Get($@"{baseUrl}/api/Reg/GetRegCode?codeType={codeType}&time={time}").RawText;
            InitInfo();
            // redis = new RedisHelper();
        }

        private static void InitInfo()
        {
            try
            {
                List<int> PIdList = new List<int>();
                List<DataInfo> dataInfos = new List<DataInfo>();
                int Sleep = 1500;
                redis = new RedisHelper(Account);
                while (true)
                {
                    if (PIdList.Count == 0)
                    {
                        dataInfos.Clear();
                    }
                    if (PIdList.Count < 2)
                    {
                        Console.WriteLine("初始化进程");
                        PIdList = MemoryHelp.GetPidByProcessName();
                        foreach (var item in PIdList)
                        {
                            if (dataInfos.FirstOrDefault(i => i.Pid == item) == null)
                            {
                                dataInfos.Add(new DataInfo()
                                {
                                    Pid = item,
                                    CurrentExperience = MemoryHelp.ReadMemoryValue(MemoryHelp.dqjyAddress, item),
                                    CurrentGold = MemoryHelp.ReadMemoryValue(MemoryHelp.yxbAddress, item),
                                    CurrentLv = MemoryHelp.GetGameLv(MemoryHelp.djAddress, item),
                                    NeedExperience = MemoryHelp.ReadMemoryValue(MemoryHelp.sjzjyAddress, item),
                                    MillisecondExperience = 0,
                                    TimeCount = 0,
                                    MillisecondGold = 0
                                });
                            }
                        }
                    }
                    Console.WriteLine($"持续检查中，当前游戏进程{PIdList.Count}个");
                    foreach (var item in PIdList)
                    {
                        InfoModel infoModel = new InfoModel();
                        infoModel.CurrentExperience = MemoryHelp.ReadMemoryValue(MemoryHelp.dqjyAddress, item);
                        infoModel.CurrentGold = MemoryHelp.ReadMemoryValue(MemoryHelp.yxbAddress, item);
                        infoModel.Grade = MemoryHelp.GetGameLv(MemoryHelp.djAddress, item);
                        infoModel.Account = MemoryHelp.ReadMemoryString(MemoryHelp.accountAddress, item);
                        infoModel.Name = MemoryHelp.ReadMemoryString(MemoryHelp.nameAddress, item);
                        infoModel.NeedExperience = MemoryHelp.ReadMemoryValue(MemoryHelp.sjzjyAddress, item);
                        infoModel.WX = MemoryHelp.ReadMemoryValue(MemoryHelp.wxAddress, item);
                        infoModel.ExperienceRatio = Math.Round(Convert.ToDouble(infoModel.CurrentExperience) / Convert.ToDouble(infoModel.NeedExperience) * 100, 2).ToString() + "%";

                        var datainfo = dataInfos.FirstOrDefault(i => i.Pid == item);
                        if (datainfo != null)
                        {
                            if (datainfo.NeedExperience == 0 || datainfo.CurrentLv == 0 || datainfo.CurrentGold == 0)
                            {
                                datainfo.CurrentExperience = MemoryHelp.ReadMemoryValue(MemoryHelp.dqjyAddress, item);
                                datainfo.CurrentGold = MemoryHelp.ReadMemoryValue(MemoryHelp.yxbAddress, item);
                                datainfo.CurrentLv = MemoryHelp.GetGameLv(MemoryHelp.djAddress, item);
                                datainfo.NeedExperience = MemoryHelp.ReadMemoryValue(MemoryHelp.sjzjyAddress, item);
                                datainfo.MillisecondExperience = 0;
                                datainfo.MillisecondGold = 0;
                            }

                            infoModel.LoginTime = datainfo.TimeCount;
                            if (datainfo.CurrentLv != 0 && infoModel.Grade > datainfo.CurrentLv)
                            {
                                datainfo.CurrentLv = infoModel.Grade;
                                datainfo.CurrentExperience -= datainfo.NeedExperience;
                                datainfo.NeedExperience = infoModel.NeedExperience;
                            }
                            if (datainfo.TimeCount != 0)
                            {
                                datainfo.MillisecondExperience = Convert.ToInt32((infoModel.CurrentExperience - datainfo.CurrentExperience) / datainfo.TimeCount);

                                datainfo.MillisecondGold = Convert.ToInt32((infoModel.CurrentGold - datainfo.CurrentGold) / datainfo.TimeCount);
                            }
                            infoModel.MinutesExperience = datainfo.MillisecondExperience * 60;
                            infoModel.MinutesGold = datainfo.MillisecondGold * 60;
                            var MillisecondExperience = datainfo.MillisecondExperience;
                            if (datainfo.MillisecondExperience != 0)
                            {
                                infoModel.UpGradeTime = Convert.ToInt32((infoModel.NeedExperience - infoModel.CurrentExperience) / datainfo.MillisecondExperience);
                            }
                            else
                            {
                                infoModel.UpGradeTime = 0;
                            }
                            datainfo.TimeCount += Convert.ToDouble(Sleep) / 1000;
                        }

                        if (infoModel.CurrentGold != 0)
                        {
                            redis.StringSet(infoModel.Account, Newtonsoft.Json.JsonConvert.SerializeObject(infoModel), new TimeSpan(0, 0, 10));
                            Console.WriteLine($"已推送账号:{infoModel.Account},角色:{infoModel.Name}的游戏数据");
                        }
                        else
                        {
                            Console.WriteLine($"账号:{infoModel.Account},正在登陆");
                            PIdList = MemoryHelp.GetPidByProcessName();
                        }
                    }
                    Thread.Sleep(Sleep);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                InitInfo();
            }
        }
    }
}
