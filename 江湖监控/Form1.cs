using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Models.Helper;
using Models.ApiReturnModel;

namespace 江湖监控
{
    public partial class Form1 : Form
    {
        private List<int> PIdList { get; set; }
        private List<string> AccountList { get; set; }
        private string Title { get; set; }
        private RedisHelper redis { get; set; }
        private string Account { get; set; }
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Account = "1209026461";
            Title = this.Text;
            redis = new RedisHelper(Account);
            InitAccountList();
            Task.Run(() =>
            {
                while (true)
                {
                    UpdateListView();
                    Thread.Sleep(1500);
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

        private void UpdateListView()
        {
            try
            {
                var keyvalues = redis.GetLikeKeyValue(Account + ":*");
                if (keyvalues != null && keyvalues.Count > 0)
                {
                    var list = keyvalues.Where(i => i.Value.Contains("Account")).Select(i => JsonConvert.DeserializeObject<InfoModel>(i.Value)).OrderBy(i => i.Account);
                    List<string> temp = AccountList.Select(i => i).ToList();
                    listView1.Items.Clear();
                    listBox1.Items.Clear();
                    listView1.BeginUpdate();
                    this.Text = Title + "           游戏币总计：" + list.Sum(i => i.CurrentGold) / 10000 + "万";
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
                else
                {
                    redis = new RedisHelper(Account);
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
                    sw.WriteLine(DateTime.Now + ex.ToString());
                }
                MessageBox.Show("程序出错");
                redis = new RedisHelper(Account);
            }
        }



    }
}
