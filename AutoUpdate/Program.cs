using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoUpdate
{
    public class testmodel
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
    class Program
    {
        private static string FilePath = @"../";
        //private static string UpdateUrl = "http://localhost:5000/api/Update/Update";
        private static string UpdateUrl = "http://118.24.18.189:33333/api/Update/Update";
        static void Main(string[] args)
        {
            Console.WriteLine("开始检查文件数据...");
            var files = Directory.GetFiles(FilePath);
            List<CheckFileInfo> fileInfos = new List<CheckFileInfo>();
            foreach (var item in files)
            {
                var file = new FileInfo(item);//遍历文件信息
                fileInfos.Add(new CheckFileInfo()
                {
                    FileName = file.Name,
                    FileMd5Value = GetMD5HashFromFile(item)
                });
            }

            Console.WriteLine("数据检查完成，开始和服务器数据比对...");
            EasyHttp.Http.HttpClient htpclient = new EasyHttp.Http.HttpClient();
            var response = htpclient.Post(UpdateUrl, fileInfos, EasyHttp.Http.HttpContentTypes.ApplicationJson);
            var updateFileList = JsonConvert.DeserializeObject<List<UpdateFileInfo>>(response.RawText);
            Console.WriteLine("获得文件完成,开始更新...");
            WebClient webClient = new WebClient();
            if (updateFileList == null)
            {
                Console.WriteLine("无需更新，即将结束更新程序");
                Thread.Sleep(1000);
                return;
            }
            foreach (var item in updateFileList)
            {
                try
                {
                    Console.WriteLine($"正在下载:{item.FileName}");
                    webClient.DownloadFile(item.Url, FilePath + item.FileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"程序出现错误：{ex.ToString()}");
                    Console.ReadKey();
                }

            }
            Console.WriteLine("更新完成，程序即将关闭");
            Thread.Sleep(1000);
        }

        private static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }



        private static string HttpPost(string url, string body)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

    }
}
