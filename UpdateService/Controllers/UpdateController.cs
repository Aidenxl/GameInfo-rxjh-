using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UpdateService.Model;

namespace UpdateService.Controllers
{
    public class testmodel
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        private string FilePath = @"C:\Client";
        private string ReturnFilePath = @"http://118.24.18.189:33333/Client/";


        [HttpPost]
        [Route("Update")]
        public IActionResult Update([FromBody]List<CheckFileInfo> updateFileInfos)
        {
            var result = CheckFile(updateFileInfos);
            return Ok(result);
        }


        private List<UpdateFileInfo> CheckFile(List<CheckFileInfo> updateFileInfos)
        {
            List<UpdateFileInfo> result = new List<UpdateFileInfo>();//需要更新文件的列表
            var files = Directory.GetFiles(FilePath);
            foreach (var item in files)
            {
                var file = new FileInfo(item);//遍历服务器文件信息

                //检查客户端文件时候和服务器文件一样 
                var cilenFile = updateFileInfos.FirstOrDefault(i => i.FileName == file.Name);
                if (cilenFile == null)
                {
                    result.Add(new UpdateFileInfo()
                    {
                        FileName = file.Name,
                        Url = ReturnFilePath + file.Name
                    });
                }
                else
                {
                    if (GetMD5HashFromFile(item) != cilenFile.FileMd5Value)
                    {
                        result.Add(new UpdateFileInfo()
                        {
                            FileName = file.Name,
                            Url = ReturnFilePath + file.Name
                        });

                    }
                }

            }
            return result;
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
    }
}