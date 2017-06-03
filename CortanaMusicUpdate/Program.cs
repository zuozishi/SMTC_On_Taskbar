using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace CortanaMusicUpdate
{
    static class Program
    {
        [DllImport("user32.dll")]
        private extern static bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        private extern static void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "FindWindowExA")]
        private extern static IntPtr FindWindowExA(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);
        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int nMaxCount);
        [Flags]
        enum MouseEventFlag : uint //设置鼠标动作的键值
        {
            Move = 0x0001,               //发生移动
            LeftDown = 0x0002,           //鼠标按下左键
            LeftUp = 0x0004,             //鼠标松开左键
            RightDown = 0x0008,          //鼠标按下右键
            RightUp = 0x0010,            //鼠标松开右键
            MiddleDown = 0x0020,         //鼠标按下中键
            MiddleUp = 0x0040,           //鼠标松开中键
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,              //鼠标轮被移动
            VirtualDesk = 0x4000,        //虚拟桌面
            Absolute = 0x8000
        }

        static Timer timer = new Timer();
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var process=System.Diagnostics.Process.GetProcessesByName("CortanaMusicUpdate");
            if(process.Length>=2)
            {
                MessageBox.Show("CortanaMusicUpdate已经运行");
                return;//防止进程重复
            }
            timer.Interval = 2000;
            timer.Tick += Timer_Tick;
            timer.Start();
            Application.Run();
        }

        //定时(2s)检测更新
        private static void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                var process = System.Diagnostics.Process.GetProcessesByName("cloudmusic");
                if (process.Length == 0)
                {
                    if(GetCortanaText()!="请在这里输入你要搜索的内容")
                    {
                        int oldx, oldy;
                        SetText("请在这里输入你要搜索的内容");
                        oldx = Control.MousePosition.X;
                        oldy = Control.MousePosition.Y;
                        SetCursorPos(187, 1063);
                        mouse_event(MouseEventFlag.Move, 0, 0, 0, UIntPtr.Zero);
                        SetCursorPos(oldx, oldy);
                    }
                    return;
                }
                if (GetCortanaText() != GetMusicText())
                {
                    int oldx, oldy;
                    SetText(GetMusicText());
                    oldx = Control.MousePosition.X;
                    oldy = Control.MousePosition.Y;
                    SetCursorPos(187, 1063);
                    mouse_event(MouseEventFlag.Move, 0, 0, 0, UIntPtr.Zero);
                    SetCursorPos(oldx, oldy);
                }

            }
            catch (Exception)
            {
                
            }
        }

        //获取曲名
        static string GetMusicText()
        {
            var musicbox = FindWindow("OrpheusBrowserHost", null);
            int musiclen = GetWindowTextLength(musicbox);
            StringBuilder windowName = new StringBuilder(musiclen + 1);
            GetWindowText(musicbox, windowName, windowName.Capacity);
            return windowName.ToString();
        }


        //获取Cortana文本框文本
        static string GetCortanaText()
        {
            var taskbar = FindWindow("Shell_TrayWnd", null);
            if (taskbar == null) return "";
            var cortana = FindWindowExA(taskbar, IntPtr.Zero, "TrayDummySearchControl", null);
            if (cortana == null) return "";
            var cortext = FindWindowExA(cortana, IntPtr.Zero, "Static", null);
            var length = SendMessage(cortext, 0x000E, 0, 0);
            StringBuilder windowName = new StringBuilder(length + 1);
            GetWindowText(cortext, windowName, windowName.Capacity);
            return windowName.ToString();
        }

        //设置Cortana文本框文本
        static void SetText(string text)
        {
            if (text.Length == 0) return;
            var taskbar = FindWindow("Shell_TrayWnd", null);
            if (taskbar == null) return;
            var cortana = FindWindowExA(taskbar, IntPtr.Zero, "TrayDummySearchControl", null);
            if (cortana == null) return;
            var cortext = FindWindowExA(cortana, IntPtr.Zero, "Static", null);
            var length = SendMessage(cortext, 0x000E, 0, 0);
            SendMessage(cortext, 0x000C, 0, text);
            SendMessage(cortext, 0x00F5, 0, text);
        }
    }
}
