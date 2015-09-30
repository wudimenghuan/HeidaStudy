using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeidaStudy
{
    static class Program
    {
        static Mutex myMutex;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool mutexNotCreated = false;
            try
            {
                myMutex = new Mutex(false, "HeidaStudy", out mutexNotCreated);
            }
            catch (Exception)
            {
                MessageBox.Show("程序即将关闭", "出现错误");
                Application.Exit();
            }
            if (mutexNotCreated)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                string note = "1. 本版本仅支持2014-2015春季学期课程。\r\n2. 运行本软件时，不要同时登陆网络教学平台或同时运行其他同样功能的软件，这会影响软件的效果，发生不可预知的错误。\r\n是否已经了解？";
                if (MessageBox.Show(note, "黑大网络教学学习管家", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Application.Run(new MainForm());
                }
                else
                {
                    Application.Exit();
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                MessageBox.Show("学习管家已经运行", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }
    }
}
