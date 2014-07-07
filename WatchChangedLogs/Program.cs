using System;
using System.IO;

namespace WatchChangedLogs
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("WatchChangedLogs [filePath]");
                Console.WriteLine("请输入监控文件地址----");
                return;
            }

            var path = args[0];

            //检查文件是否存在
            if (!File.Exists(path))
            {
                Console.WriteLine("{0}不存在", path);
                return;
            }

            var logWatcher = new LogWatcher(path);
            logWatcher.NewLogs += (sender, e) => Console.Write(e.Logs);
            logWatcher.Start();
            Console.Read();

        }
    }
}
