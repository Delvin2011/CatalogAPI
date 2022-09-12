using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Services.Utility
{
    public class MyLogger : ILogger //use 
    {
        //signleton pattern example. Can only be instantiated once
        private static MyLogger instance; //single instance of this class
        private static Logger logger; //static variable to hold a single instance of the nlog logger

        //single design pattern
        private MyLogger () //empty construcote
        {

        }
        //this function creates an instance of the class, if it hasn't yet been instantiated
        //If the class already exists then send them the reference to the original
        public static MyLogger GetInstance() 
        {
            if (instance == null)
                instance = new MyLogger();
            return instance;
        }

        private Logger GetLogger(string theLogger)
        {
            if (MyLogger.logger == null)
                MyLogger.logger = LogManager.GetLogger(theLogger);
            return MyLogger.logger;
        }

        public void Debug(string message, string arg = null)
        {
            if (arg == null)
                GetLogger("ItemsAPILoggerRules").Debug(message);
            else
                GetLogger("ItemsAPILoggerRules").Debug(message, arg);
        }

        public void Error(string message, string arg = null)
        {
            if (arg == null)
                GetLogger("ItemsAPILoggerRules").Error(message);
            else
                GetLogger("ItemsAPILoggerRules").Error(message, arg);
        }

        public void Info(string message, string arg = null)
        {
            if (arg == null)
                GetLogger("ItemsAPILoggerRules").Info(message);
            else
                GetLogger("ItemsAPILoggerRules").Info(message, arg);
        }

        public void Warning(string message, string arg = null)
        {
            if (arg == null)
                GetLogger("ItemsAPILoggerRules").Warn(message);
            else
                GetLogger("ItemsAPILoggerRules").Warn(message, arg);
        }
    }
}
