namespace ClientTelegram.Utility
{
    public class MethodUtility
    {
        private readonly string _logPath;
        private static readonly object _lock = new object();

        public MethodUtility(string logPath)
        {
            _logPath = logPath;
        }

        public void Log(string type, string message)
        {
            lock (_lock)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_logPath));
                string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}[{type}] - {message}{Environment.NewLine}";
                File.AppendAllText(_logPath, line);
            }
        }
    }
}
