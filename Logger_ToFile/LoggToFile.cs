using NLog;
using System;
using System.Text;

namespace Logger_ToFile
{
    public class LoggToFile
    {
        //Assembly: System.Reflection.Assembly.GetExecutingAssembly().FullName;
        //Method: System.Reflection.MethodBase.GetCurrentMethod().ToString();
        public LoggToFile(string assemblyName)
        {
            logger = !String.IsNullOrEmpty(assemblyName) ? LogManager.GetLogger(assemblyName) : LogManager.GetCurrentClassLogger();
        }

        private static Logger logger;
        public void Info(string method, string message)
        {
            logger.Info(thisMessage(method, message));
        }
        public void Warn(string method, string message)
        {
            logger.Warn(thisMessage(method, message));
        }
        public void Error(string method, string message)
        {
            logger.Error(thisMessage(method, message));
        }
        private string thisMessage(string metod, string message)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine()
                .Append("Method: ")
                .AppendLine(metod)
                .Append("Message: ")
                .AppendLine(message);
            return s.ToString();
        }
    }
}
