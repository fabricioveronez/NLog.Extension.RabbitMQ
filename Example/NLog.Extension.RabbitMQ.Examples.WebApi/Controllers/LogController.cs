using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog.Extension.RabbitMQ.Examples.WebApi.Model;

namespace NLog.Extension.RabbitMQ.Examples.WebApi.Controllers
{
    [Route("api/log")]
    public class LogController : Controller
    {

        private ILogger<LogController> _log;

        public LogController(ILogger<LogController> log)
        {
            this._log = log;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Log log)
        {
            try
            {
                throw new Exception("This exception is a test.");
            }
            catch (Exception ex)
            {
                switch (log.LogLevel)
                {
                    case Model.LogLevel.Trace:
                        this._log.LogTrace(ex, log.Message);
                        break;
                    case Model.LogLevel.Debug:
                        this._log.LogDebug(ex, log.Message);
                        break;
                    case Model.LogLevel.Info:
                        this._log.LogInformation(ex, log.Message);
                        break;
                    case Model.LogLevel.Warn:
                        this._log.LogWarning(ex, log.Message);
                        break;
                    case Model.LogLevel.Error:
                        this._log.LogError(ex, log.Message);
                        break;
                    case Model.LogLevel.Fatal:
                        this._log.LogCritical(ex, log.Message);
                        break;
                }
            }

            return Ok();
        }
    }
}