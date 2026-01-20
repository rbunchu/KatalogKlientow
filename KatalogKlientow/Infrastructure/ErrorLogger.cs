using KatalogKlientow.Configuration;
using System;
using System.IO;
using System.Text;

namespace KatalogKlientow.Infrastructure
{
    public class ErrorLogger
    {
        private readonly IAppConfiguration _appConfiguration;
        private readonly object _sync = new object();

        public ErrorLogger(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration ?? throw new ArgumentNullException(nameof(appConfiguration));
        }

        public string Log(Exception exception, string context = null)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            try
            {
                var fileName = _appConfiguration.ErrorFileName;
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = "error.log";
                }

                string fullPath;
                if (Path.IsPathRooted(fileName))
                {
                    fullPath = fileName;
                    var dir = Path.GetDirectoryName(fullPath);
                    if (!string.IsNullOrWhiteSpace(dir))
                        Directory.CreateDirectory(dir);
                }
                else
                {
                    var baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KatalogKlientow", "Logs");
                    Directory.CreateDirectory(baseDir);
                    fullPath = Path.Combine(baseDir, fileName);
                }

                var sb = new StringBuilder();
                sb.AppendLine("Timestamp: " + DateTime.Now.ToString("o"));
                if (!string.IsNullOrEmpty(context))
                    sb.AppendLine("Context: " + context);
                sb.AppendLine("Machine: " + Environment.MachineName);
                sb.AppendLine("User: " + Environment.UserName);
                sb.AppendLine();

                AppendExceptionDetails(sb, exception);
                sb.AppendLine(new string('-', 80));
                sb.AppendLine();

                lock (_sync)
                {
                    File.AppendAllText(fullPath, sb.ToString(), Encoding.UTF8);
                }

                return fullPath;
            }
            catch
            {
                return string.Empty;
            }
        }

        private void AppendExceptionDetails(StringBuilder sb, Exception ex)
        {
            if (ex == null) return;

            sb.AppendLine("Exception Type: " + ex.GetType().FullName);
            sb.AppendLine("Message: " + ex.Message);
            sb.AppendLine("StackTrace:");
            sb.AppendLine(ex.StackTrace ?? string.Empty);
            sb.AppendLine();

            if (ex.InnerException != null)
            {
                sb.AppendLine("---- Inner Exception ----");
                AppendExceptionDetails(sb, ex.InnerException);
            }
        }
    }
}