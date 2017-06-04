using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaCtrl
{
    class Program
    {
        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        static void Main(string[] args)
        {
            //ShowMusicBox(args);
            var cloudmusic = Process.GetProcessesByName("cloudmusic");
            var qqmusic = Process.GetProcessesByName("QQMusic");
            if (args.Length>0)
            {
                switch (args[0])
                {
                    case "p"://播放-暂停
                        keybd_event(Keys.MediaPlayPause, 0, 0, 0);
                        keybd_event(Keys.MediaPlayPause, 0, 2, 0);
                        if (qqmusic.Length > 0)//QQ音乐
                        {
                            keybd_event(Keys.ControlKey, 0, 0, 0);
                            keybd_event(Keys.Menu, 0, 0, 0);
                            keybd_event(Keys.F5, 0, 0, 0);
                            keybd_event(Keys.ControlKey, 0, 2, 0);
                            keybd_event(Keys.Menu, 0, 2, 0);
                            keybd_event(Keys.F5, 0, 2, 0);
                        }
                        break;
                    case "c"://桌面歌词 Ctrl+Alt+D
                        
                        if(cloudmusic.Length>0)//网易云音乐
                        {
                            keybd_event(Keys.ControlKey, 0, 0, 0);
                            keybd_event(Keys.Menu, 0, 0, 0);
                            keybd_event(Keys.D, 0, 0, 0);
                            keybd_event(Keys.ControlKey, 0, 2, 0);
                            keybd_event(Keys.Menu, 0, 2, 0);
                            keybd_event(Keys.D, 0, 2, 0);
                        }else if(qqmusic.Length>0)//QQ音乐
                        {
                            keybd_event(Keys.ControlKey, 0, 0, 0);
                            keybd_event(Keys.Menu, 0, 0, 0);
                            keybd_event(Keys.W, 0, 0, 0);
                            keybd_event(Keys.ControlKey, 0, 2, 0);
                            keybd_event(Keys.Menu, 0, 2, 0);
                            keybd_event(Keys.W, 0, 2, 0);
                        }
                        break;
                    case "next"://下一曲
                        keybd_event(Keys.MediaNextTrack, 0, 0, 0);
                        keybd_event(Keys.MediaNextTrack, 0, 2, 0);
                        if (qqmusic.Length > 0)//QQ音乐
                        {
                            keybd_event(Keys.ControlKey, 0, 0, 0);
                            keybd_event(Keys.Menu, 0, 0, 0);
                            keybd_event(Keys.Right, 0, 0, 0);
                            keybd_event(Keys.ControlKey, 0, 2, 0);
                            keybd_event(Keys.Menu, 0, 2, 0);
                            keybd_event(Keys.Right, 0, 2, 0);
                        }
                        break;  
                    case "pre"://上一曲
                        keybd_event(Keys.MediaPreviousTrack, 0, 0, 0);
                        keybd_event(Keys.MediaPreviousTrack, 0, 2, 0);
                        if (qqmusic.Length > 0)//QQ音乐
                        {
                            keybd_event(Keys.ControlKey, 0, 0, 0);
                            keybd_event(Keys.Menu, 0, 0, 0);
                            keybd_event(Keys.Left, 0, 0, 0);
                            keybd_event(Keys.ControlKey, 0, 2, 0);
                            keybd_event(Keys.Menu, 0, 2, 0);
                            keybd_event(Keys.Left, 0, 2, 0);
                        }
                        break;
                    case "about"://关于
                        new AboutBox().ShowDialog();
                        break;
                    default:
                        new AboutBox().ShowDialog();
                        break;
                }
            }
            else
            {
                new AboutBox().ShowDialog();
            }
        }

        private static void ShowMusicBox(string[] args)
        {
            var cloudmusic = Process.GetProcessesByName("cloudmusic");
            var groove= Process.GetProcessesByName("Music.UI");
            bool start = (cloudmusic.Length == 0 && groove.Length == 0) ? true : false;
            if (args.Length>0&& start)
            {
                if(args[0]=="p")
                {
                    StartGroove();
                }
                else if (args[0] == "c")
                {
                    StartMusic163();
                }
            }
        }

        static async void StartGroove()
        {
            System.Diagnostics.Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();
            p.StandardInput.WriteLine("cd %APPDATA%\\Microsoft\\Windows\\Start Menu\\Programs");
            p.StandardInput.WriteLine("Groove.lnk");
            await Task.Delay(1000);
            p.StandardInput.WriteLine("exit");
            //string output = p.StandardOutput.ReadToEnd();
        }

        static async void StartMusic163()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = false;//不显示程序窗口
            p.Start();
            var dir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Netease", "CloudMusic");
            p.StandardInput.WriteLine("cd " + dir);
            p.StandardInput.WriteLine("cloudmusic.exe");
            await Task.Delay(1000);
            p.StandardInput.WriteLine("exit");
            //string output = p.StandardOutput.ReadToEnd();
        }
    }
}
