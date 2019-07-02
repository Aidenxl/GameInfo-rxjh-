using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace 本地版
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Check());
            }
            catch (Exception ex)
            {
                if(!ex.Message.Contains("已释放的对象"))
                {
                    MessageBox.Show($"程序发生错误：{ex.Message}");
                }
            }
        }
    }
}
