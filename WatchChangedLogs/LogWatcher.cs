using System;
using System.IO;
using System.Text;

namespace WatchChangedLogs
{
    /// <summary>
    /// 监控日志文件变化@liangali2014/7/17
    /// </summary>

    public class LogWatcher
    {
        private readonly string _path;
        private long _currentSize = 0;
        private readonly FileSystemWatcher _watcher;

        public LogWatcher(string path)
        {
            _path = path;

            _currentSize = GetSize();

            var folder = Path.GetDirectoryName(_path);
            var filter = Path.GetFileName(_path);

            if (folder == null)
            {
                throw new ArgumentNullException();
            }

            if (filter == null)
            {
                throw new ArgumentNullException();
            }

            // 初始化日志监听器
            _watcher = new FileSystemWatcher(folder, filter)
           {
               NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                              | NotifyFilters.FileName | NotifyFilters.DirectoryName
           };

            // 注册监听时间
            _watcher.Changed += (sender, e) =>
            {
                var pre = _currentSize;
                _currentSize = GetSize();

                if (pre < _currentSize)
                {
                    OnNewLogs(ReadLogs(pre, _currentSize));
                }
            };

        }

        /// <summary>
        /// 获取文件的Size
        /// </summary>
        /// <returns></returns>
        private long GetSize()
        {
            return new FileInfo(this._path).Length;
        }

        /// <summary>
        /// 新日志事件 
        /// </summary>
        public event EventHandler<NewLogsEvents> NewLogs;
        private void OnNewLogs(string lines)
        {
            if (NewLogs != null)
            {
                NewLogs(this, new NewLogsEvents(lines));
            }
        }

        /// <summary>
        /// 根据偏移获取新日志
        /// </summary>
        /// <param name="pre"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        private String ReadLogs(long pre, long current)
        {
            using (var fs = new FileStream(this._path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(pre, SeekOrigin.Begin);

                var size = (int)(current - pre);
                var bytes = new byte[size];

                fs.Read(bytes, 0, size);
                fs.Close();
                return Encoding.Default.GetString(bytes);
            }
        }

        /// <summary>
        /// 开始监控
        /// </summary>
        public void Start()
        {
            _watcher.EnableRaisingEvents = true;
            Console.WriteLine("Started Watch:{0}", _path);
        }

        /// <summary>
        /// 停止监控
        /// </summary>
        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
            Console.WriteLine("Stopped Watch:{0}", _path);
        }
    }
}
