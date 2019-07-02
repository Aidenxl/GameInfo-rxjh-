using EasyHttp.Http;
using Models;
using Models.ApiRequestModel;
using Models.ApiReturnModel;
using Models.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace 注册码计算
{
    public partial class Form1 : Form
    {
        //private string baseUrl = "http://118.24.18.189:81";
        private string baseUrl = "http://localhost:5000";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int codeType = -1;
            int time = 0;
            if (cBox_CodeType.Text.Trim() == "本地版")
            {
                codeType = 0;
                time = -1;
            }
            else if (cBox_CodeType.Text.Trim() == "网络版")
            {
                codeType = 1;
                time = int.Parse(cBox_Time.Text);
            }
            int cont = int.Parse(comboBox1.Text);

            List<CodeInfo> codeInfos = new List<CodeInfo>();
            for (int i = 0; i < cont; i++)
            {
                codeInfos.Add(new CodeInfo() { CodeType = codeType, RegCode = Guid.NewGuid().ToString().MD5Encrypt32(), Time = time });
            }
            HttpClient httpClient = new HttpClient();
            var result = JsonConvert.DeserializeObject<ReturnBase>(httpClient.Post($@"{baseUrl}/api/Reg/AddRegCode", JsonConvert.SerializeObject(codeInfos), "application/json").RawText);
            if (result.IsSuccess)
            {
                MessageBox.Show("添加成功");
            }
            else
            {
                MessageBox.Show("添加失败" + result.Message);
            }

        }

        private void cBox_CodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combox = (ComboBox)sender;
            if (combox.Text.Contains("本地"))
            {
                cBox_Time.Enabled = false;
            }
            else
            {
                cBox_Time.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int codeType = -1;
            int time = 0;
            if (comboBox3.Text.Trim() == "本地版")
            {
                codeType = 0;
                time = -1;
            }
            else if (comboBox3.Text.Trim() == "网络版")
            {
                codeType = 1;
                time = int.Parse(comboBox2.Text.Trim());
            }

            HttpClient httpClient = new HttpClient();
            textBox1.Text = httpClient.Get($@"{baseUrl}/api/Reg/GetRegCode?codeType={codeType}&time={time}").RawText;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combox = (ComboBox)sender;
            if (combox.Text.Contains("本地"))
            {
                comboBox2.Enabled = false;
            }
            else
            {
                comboBox2.Enabled = true;
            }
        }
    }
}
