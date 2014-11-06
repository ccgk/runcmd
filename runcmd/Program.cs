using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace runcmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WindowHeight = 6;
            Console.BufferHeight = 6;
            Console.Clear();
            Show();
            Console.WriteLine();
            Console.Write("请输入要执行的命令：");
            Console.ForegroundColor = ConsoleColor.Red;
            string cmd = Console.ReadLine();
            run(cmd);
        }
        /// <summary>
        /// 执行一条命令，从指定的配置文件中根据命令取得程序名与参数
        /// 格式1：命令?程序名
        /// 格式2：命令?程序名|参数
        /// </summary>
        /// <param name="cmd">命令</param>
        private static void run(string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd))
                return;
            if (cmd == "0") return;
            try
            {
                string cmdfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cmd.txt");
                if (!File.Exists(cmdfile))
                {
                    Console.WriteLine("文件:[cmd.txt]不存在！");
                    Console.Beep(2000, 500);
                    Console.ReadKey();
                    return;
                }
                using (System.IO.FileStream fs = new FileStream(cmdfile, FileMode.Open, FileAccess.Read))
                {
                    System.IO.StreamReader sr = new StreamReader(fs);
                    bool ishavecmd = false;
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith(@"//") || line.StartsWith("--") || string.IsNullOrWhiteSpace(line))
                            continue;
                        string[] cmdandpath = line.Split('?');
                        if (cmdandpath[0].ToLower() == cmd.ToLower())
                        {
                            ishavecmd = true;
                            if (line.Substring(line.IndexOf('?') + 1).Contains("|"))
                            {
                                string se = line.Substring(line.IndexOf('?') + 1);
                                System.Diagnostics.Process.Start(se.Split('|')[0], se.Substring(se.IndexOf('|') + 1));
                            }
                            else System.Diagnostics.Process.Start(line.Substring(line.IndexOf('?') + 1));
                        }
                        else
                        {
                            line = "";
                        }
                    }
                    if (ishavecmd == false)
                    {
                        Console.Beep();
                        System.Diagnostics.Process.Start(cmd);
                    }
                }
            }
            catch (Exception xp)
            {
                Console.Beep(2000, 500);
                Console.WriteLine(xp.Message);
                Console.ReadKey();
            }
        }
        /// <summary>
        /// 从文件读取界面内容并显示到界面
        /// </summary>
        private static void Show()
        {
            string uifile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ui.txt");
            if (!File.Exists(uifile))
            {
                return;
            }
            string[] sm = File.ReadAllLines(uifile, Encoding.Default);
            if (sm.Length != 0)
            {
                foreach (string s in sm)
                    Console.WriteLine(s);
            }
        }
    }
}
