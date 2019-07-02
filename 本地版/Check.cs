using EasyHttp.Http;
using Models;
using Models.ApiReturnModel;
using Models.Helper;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace 本地版
{


    public partial class Check : Form
    {
        //private string baseUrl = "http://118.24.18.189:81";
        private string baseUrl = "http://localhost:5000";
        public Check()
        {
            CheckReg();
            InitializeComponent();
        }

        private void Check_Load(object sender, EventArgs e)
        {
            textBox2.Text = GetMachineCode.GetMachineCodeString().MD5Encrypt32();
        }

        private void CheckReg()
        {
            var machineCode = GetMachineCode.GetMachineCodeString().MD5Encrypt32();
            HttpClient httpClient = new HttpClient();
            var result = httpClient.Get($@"{baseUrl}/api/Login/CheckMachineRegStatus?machineCode={machineCode}");

            var resultModel = JsonConvert.DeserializeObject<ReturnBase>(result.RawText);

            if (resultModel.IsSuccess)
            {
                this.Hide();
                Application.Run(new Form1(this));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            var result = httpClient.Get($@"{baseUrl}/api/Reg/RegMachine?machineCode={textBox2.Text.Trim()}&regCode={textBox1.Text}");

            var resultModel = JsonConvert.DeserializeObject<ReturnBase>(result.RawText);

            if (resultModel.IsSuccess)
            {
                MessageBox.Show(resultModel.Message);
                this.Hide();
                var form = new Form1(this);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show(resultModel.Message);
            }
        }
    }
}
