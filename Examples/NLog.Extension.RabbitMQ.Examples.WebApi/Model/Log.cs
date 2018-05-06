using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NLog.Extension.RabbitMQ.Examples.WebApi.Model
{
    public class Log
    {
        public string Message { get; set; }
        public NLog.Extension.RabbitMQ.Examples.WebApi.Model.LogLevel LogLevel { get; set; }
    }

    public enum LogLevel
    {
        Trace = 0,
        Debug = 1,
        Info = 2,
        Warn = 3,
        Error = 4,
        Fatal = 5
    }
}
