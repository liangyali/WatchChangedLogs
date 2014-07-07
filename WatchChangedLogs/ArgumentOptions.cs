using CommandLine;
using CommandLine.Text;

namespace WatchChangedLogs
{

    public class ArgumentOptions
    {
        [Option('f', "file", Required = true, HelpText = "请输入监控日志文件")]
        public string FilePath { get; set; }
    }
}
