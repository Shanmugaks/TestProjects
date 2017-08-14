using System;
using System.Messaging;

namespace MSMQReceiver
{
    public enum LOGGER
    {
        OFF = 0,
        ALL = 1,
        TRACE = 2,
        DEBUG = 3,
        INFO = 4,
        WARN = 5,
        ERROR = 6,
        FATAL = 7
    }
    public class LogData
    {
        public LOGGER type { get; set; }
        public string message { get; set; }
        public string ExtraMsg { get; set; }
        public DateTime TimeStamp { get; set; }

        public bool WriteToConsole()
        {
            Console.WriteLine("Log Type = " + type.ToString());
            Console.WriteLine("Log Msg = " + message);
            Console.WriteLine("Log ExtraMsg = " + ExtraMsg);
            Console.WriteLine("Log TimeStamp = " + TimeStamp);
            return true;

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Remote machine - MSMQ Name = FormatName:Direct=OS:machinename\\private$\\queuename   
            // Remote machine with IP address - MSMQ Name = FormatName:Direct=TCP:121.0.0.1\\private$\\queue
            MessageQueue messageQueue = new MessageQueue(@".\Private$\dLoggerQueue");
            System.Messaging.Message[] messages = messageQueue.GetAllMessages();
            while (true)
            {
                Console.WriteLine(" Pree Any Key... to Read");
                Console.ReadLine();
                messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(LogData) });
                LogData log = null;
                log = (LogData)messageQueue.Receive().Body;
                Console.WriteLine(log.WriteToConsole());             
            }
        }
    }
}
