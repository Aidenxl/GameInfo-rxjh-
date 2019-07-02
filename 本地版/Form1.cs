using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using Models;
using Models.Helper;
using Models.ApiReturnModel;

namespace 本地版
{
    public partial class Form1 : Form
    {
        private List<int> PIdList { get; set; }
        private List<string> AccountList { get; set; }
        private List<DataInfo> dataInfos { get; set; }
        private int Sleep = 1500;
        private string Title { get; set; }
        private Form Form { get; set; }
        public Form1(Form form)
        {
            CheckForIllegalCrossThreadCalls = false;
            Form = form;
            InitAccountList();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PIdList = new List<int>();
            dataInfos = new List<DataInfo>();
            Title = this.Text;
            InfoModelCache.KeyValues = new Dictionary<string, InfoModel>();
            Task.Run(() =>
            {
                while (true)
                {
                    UpdateListView();
                    Thread.Sleep(Sleep);
                }

            });
        }
        private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            ColumnHeader header = this.listView1.Columns[e.ColumnIndex];
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void InitAccountList()
        {
            string filePath = ConfigurationManager.AppSettings["AccountFilePath"];
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath))
                {

                }
            }
            AccountList = new List<string>();
            string line;
            using (StreamReader sr = new StreamReader(filePath))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    AccountList.Add(line);
                }

            }
        }
        private void InitInfo()
        {
            try
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
                //Console.WriteLine($"持续检查中，当前游戏进程{PIdList.Count}个");
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
                        if (!InfoModelCache.KeyValues.Keys.Contains(infoModel.Account))
                        {
                            InfoModelCache.KeyValues.Add(infoModel.Account, infoModel);
                        }
                        else
                        {
                            InfoModelCache.KeyValues[infoModel.Account] = infoModel;
                        }
                        //Console.WriteLine($"已推送账号:{infoModel.Account},角色:{infoModel.Name}的游戏数据");
                    }
                    else
                    {
                        //Console.WriteLine($"账号:{infoModel.Account},正在登陆");
                        PIdList = MemoryHelp.GetPidByProcessName();
                    }
                }
                Thread.Sleep(Sleep);

            }
            catch (Exception ex)
            {
                var filePath = $"Errorlog /{ DateTime.Now.ToString("yyyy-MM-dd")}.txt";
                if (!Directory.Exists("Errorlog"))
                {
                    Directory.CreateDirectory("Errorlog");
                }
                if (!File.Exists(filePath))
                {
                    using (File.Create(filePath))
                    {

                    }
                }
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(ex.ToString());
                }
                InitInfo();
            }
        }

        private void UpdateListView()
        {
            try
            {
                InitInfo();
                var list = InfoModelCache.KeyValues.Select(i => i.Value);
                listView1.Items.Clear();
                listBox1.Items.Clear();
                List<string> temp = AccountList.Select(i => i).ToList();
                listView1.BeginUpdate();
                this.Text = Title + "           游戏币总计：" + list.Sum(i => Convert.ToInt64(i.CurrentGold)) / 10000 + "万";
                foreach (var infoModel in list)
                {
                    var listItem = new ListViewItem();
                    listItem.ImageIndex = 1;
                    listItem.Text = infoModel.Account;
                    listItem.SubItems.Add(infoModel.Name);
                    listItem.SubItems.Add(infoModel.Grade.ToString());
                    listItem.SubItems.Add($"{Math.Round(Convert.ToDouble(infoModel.CurrentExperience) / 10000, 2)}万");
                    listItem.SubItems.Add($"{Math.Round(Convert.ToDouble(infoModel.NeedExperience) / 10000, 2)}万");
                    listItem.SubItems.Add(infoModel.ExperienceRatio);
                    listItem.SubItems.Add(infoModel.WX.ToString());
                    listItem.SubItems.Add($"{Math.Round(Convert.ToDouble(infoModel.CurrentGold) / 10000, 2)}万");
                    listItem.SubItems.Add($"{Math.Round(Convert.ToDouble(infoModel.MinutesGold) / 10000, 2)}万");
                    listItem.SubItems.Add($"{Math.Round(Convert.ToDouble(infoModel.MinutesExperience) / 10000, 2)}万");
                    listItem.SubItems.Add($"{infoModel.UpGradeTime / 60}分{infoModel.UpGradeTime % 60}秒");
                    listItem.SubItems.Add($"{(int)infoModel.LoginTime / 60}分{(int)infoModel.LoginTime % 60}秒");
                    listView1.Items.Add(listItem);
                    temp.Remove(infoModel.Account);
                }
                listView1.EndUpdate();
                foreach (var item in temp)
                {
                    listBox1.Items.Add(item + "\r\n");
                }
            }
            catch (Exception ex)
            {
                var filePath = $"Errorlog /{ DateTime.Now.ToString("yyyy-MM-dd")}.txt";
                if (!Directory.Exists("Errorlog"))
                {
                    Directory.CreateDirectory("Errorlog");
                }
                if (!File.Exists(filePath))
                {
                    using (File.Create(filePath))
                    {

                    }
                }
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(ex.ToString());
                }

            }
            finally
            {
                GC.Collect();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form.Close();
        }
    }

    public class InfoModelCache
    {
        public static Dictionary<string, InfoModel> KeyValues { get; set; }
    }

}
