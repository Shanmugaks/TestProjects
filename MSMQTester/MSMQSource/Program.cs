using System;
using System.Messaging;

namespace MSMQTester
{
    public enum LOGGER
    {
        DEBUG = 0,
        WARNING = 1,
        ERROR = 2,
        ALL = 3

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

            MessageQueue messageQueue = null;
            if (MessageQueue.Exists(@".\Private$\dLoggerQueue"))
            {
                messageQueue = new MessageQueue(@".\Private$\dLoggerQueue");
                messageQueue.Label = "Testing Queue";
            }
            else
            {
                // Create the Queue
                MessageQueue.Create(@".\Private$\dLoggerQueue");
                messageQueue = new MessageQueue(@".\Private$\dLoggerQueue");
                messageQueue.Label = "dLoggerQueue";
            }

            LogData log = new LogData();
            log.type = LOGGER.ERROR;
            log.message = "Hard disk Read Error";
            log.TimeStamp = DateTime.Now;
            log.ExtraMsg = "None";

            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(LogData) });
            messageQueue.Send(log);


            log = new LogData();
            log.type = LOGGER.DEBUG;
            log.message = "DEBUG MODE - Application calling MSMQ";
            log.TimeStamp = DateTime.Now;
            log.ExtraMsg = "None";

            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(LogData) });
            messageQueue.Send(log);


            log = new LogData();
            log.type = LOGGER.WARNING;
            log.message = "Warning - possibly deadlock can happen";
            log.TimeStamp = DateTime.Now;
            log.ExtraMsg = "None";

            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(LogData) });
            messageQueue.Send(log);


            log = new LogData();
            log.type = LOGGER.ALL;
            log.message = "General -Auditing";
            log.TimeStamp = DateTime.Now;
            log.ExtraMsg = "This is special message - showing call stack";

            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(LogData) });
            messageQueue.Send(log);

            Console.ReadLine();
        }
    }
}
