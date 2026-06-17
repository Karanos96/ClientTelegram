namespace ClientTelegram.Utility
{
    public class MethodUtility
    {
        private readonly string _logPath;

        public MethodUtility(string logPath)
        {
            _logPath = logPath;
        }

        public void Log(string type , string message)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_logPath));
            string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}[{type}] - {message}{Environment.NewLine}";
            File.AppendAllText(_logPath, line);
        }
    }
}
