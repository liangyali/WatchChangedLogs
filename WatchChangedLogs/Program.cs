using System;
using System.IO;
using CommandLine.Text;

namespace WatchChangedLogs
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new ArgumentOptions();

            if (CommandLine.Parser.Default.ParseArgumentsStrict(args, options))
            {
                var path = options.FilePath;

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
}
