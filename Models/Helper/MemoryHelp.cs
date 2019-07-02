using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Models.Helper
{
    /// <summary>
    /// 内存操作帮助类
    /// </summary>
    public class MemoryHelp
    {
        //游戏进程内存基址
        private static int baseAddress = 0x00400000;
        //当前经验
        public static int dqjyAddress = baseAddress + 0x050484B8;
        //升级总经验
        public static int sjzjyAddress = baseAddress + 0x050484C0;
        //名字
        public static int nameAddress = baseAddress + 0x05048420;
        //等级
        public static int djAddress = baseAddress + 0x05048454;
        //武勋
        public static int wxAddress = baseAddress + 0x050484E0;
        //游戏币
        public static int yxbAddress = baseAddress + 0x05048500;
        //账号
        public static int accountAddress = baseAddress + 0x0504A110;
        //地图
        public static int mapAddress = baseAddress + 0x01EDA3C8;
        //地图X坐标
        public static int mapXAddress = 0x01EDA3C8;
        //地图Y坐标
        public static int mapYAddress = 0x01EDA3D0;
        //游戏进程名字
        private static string processName = "client";

        #region API

        //从指定内存中读取字节集数据
        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, IntPtr lpNumberOfBytesRead);

        //从指定内存中写入字节集数据
        [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, int[] lpBuffer, int nSize, IntPtr lpNumberOfBytesWritten);

        //打开一个已存在的进程对象，并返回进程的句柄
        [DllImport("kernel32.dll", EntryPoint = "OpenProcess")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        //关闭一个内核对象。其中包括文件、文件映射、进程、线程、安全和同步对象等。
        [DllImport("kernel32.dll")]
        private static extern void CloseHandle(IntPtr hObject);

        #endregion

        #region 使用方法

        /// <summary>
        /// 根据进程名获取PID
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static List<int> GetPidByProcessName()
        {
            Process[] arrayProcess = Process.GetProcessesByName(processName);
            return arrayProcess.Select(i => i.Id).ToList();
        }

        /// <summary>
        /// 读取内存中的值
        /// </summary>
        /// <param name="address">内存地址</param>
        /// <param name="Pid">进程ID</param>
        /// <returns></returns>
        public static int ReadMemoryValue(int address, int pId)
        {
            try
            {
                byte[] buffer = new byte[4];
                //获取缓冲区地址
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                //打开一个已存在的进程对象  0x1F0FFF 最高权限
                IntPtr hProcess = OpenProcess(0x1F0FFF, false, pId);
                //将指定内存中的值读入缓冲区
                ReadProcessMemory(hProcess, (IntPtr)address, byteAddress, 4, IntPtr.Zero);
                //关闭操作
                CloseHandle(hProcess);
                var t = BitConverter.ToInt32(buffer, 0);
                //从非托管内存中读取一个 32 位带符号整数。
                return t;//Marshal.ReadInt32(byteAddress);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 读取内存中的字符串
        /// </summary>
        /// <param name="address">内存地址</param>
        /// <param name="Pid">进程ID</param>
        /// <returns></returns>
        public static string ReadMemoryString(int address, int pId)
        {
            try
            {
                byte[] buffer = new byte[12];
                //获取缓冲区地址
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                //打开一个已存在的进程对象  0x1F0FFF 最高权限
                IntPtr hProcess = OpenProcess(0x1F0FFF, false, pId);
                //将指定内存中的值读入缓冲区
                ReadProcessMemory(hProcess, (IntPtr)address, byteAddress, 12, IntPtr.Zero);
                //关闭操作
                CloseHandle(hProcess);
                var a = Encoding.Default.GetString(buffer.Where(i => i != 0).ToArray());
                return a;
            }
            catch
            {
                return "异常";
            }
        }

        /// <summary>
        /// 读取内存中的byte
        /// </summary>
        /// <param name="address">内存地址</param>
        /// <param name="Pid">进程ID</param>
        /// <returns></returns>
        public static byte GetGameLv(int address, int pId)
        {
            try
            {
                byte[] buffer = new byte[4];
                //获取缓冲区地址
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                //打开一个已存在的进程对象  0x1F0FFF 最高权限
                IntPtr hProcess = OpenProcess(0x1F0FFF, false, pId);
                //将指定内存中的值读入缓冲区
                ReadProcessMemory(hProcess, (IntPtr)address, byteAddress, 4, IntPtr.Zero);
                //关闭操作
                CloseHandle(hProcess);
                return Marshal.ReadByte(byteAddress);
            }
            catch
            {
                return 0;
            }
        }

        //将值写入指定内存地址中
        public static void WriteMemoryValue(int baseAddress, int pId, int value)
        {
            try
            {
                //打开一个已存在的进程对象  0x1F0FFF 最高权限
                IntPtr hProcess = OpenProcess(0x1F0FFF, false, pId);
                //从指定内存中写入字节集数据
                WriteProcessMemory(hProcess, (IntPtr)baseAddress, new int[] { value }, 4, IntPtr.Zero);
                //关闭操作
                CloseHandle(hProcess);
            }
            catch (Exception e)
            {

            }
        }
        #endregion
    }
}
