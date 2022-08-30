using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace ds.pms.bl.logger
{
    public class Logging
    {
        private readonly ILogger _logger;

        public Logging(ILogger logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message, params object[] obj)
        {
            _logger.LogInformation(message, obj);
        }

        public void LogInformation(EventId eventId, string message, params object[] obj)
        {
            _logger.LogInformation(eventId, message, obj);
        }
        public void LogInformation(Exception exception, string message, params object[] obj)
        {
            _logger.LogInformation(exception, message, obj);
        }
        public void LogInformation(EventId eventId, Exception exception, string message, params object[] obj)
        {
            _logger.LogInformation(eventId, exception, message, obj);
        }



        public void LogWarning(string message, params object[] obj)
        {
            _logger.LogWarning(message, obj);
        }

        public void LogWarning(EventId eventId, string message, params object[] obj)
        {
            _logger.LogWarning(eventId, message, obj);
        }
        public void LogWarning(Exception exception, string message, params object[] obj)
        {
            _logger.LogWarning(exception, message, obj);
        }
        public void LogWarning(EventId eventId, Exception exception, string message, params object[] obj)
        {
            _logger.LogWarning(eventId, exception, message, obj);
        }




        public void LogError(string message, params object[] obj)
        {
            _logger.LogError(message, obj);
        }

        public void LogError(EventId eventId, string message, params object[] obj)
        {
            _logger.LogError(eventId, message, obj);
        }
        public void LogError(Exception exception, string message, params object[] obj)
        {
            _logger.LogError(exception, message, obj);
        }
        public void LogError(EventId eventId, Exception exception, string message, params object[] obj)
        {
            _logger.LogError(eventId, exception, message, obj);
        }


        public string GetExceptionMessage(string methodName, string className)
        {
            return "Exception occured in " + methodName + " method of " + className;
        }

        public string GetExceptionMessage(ControllerContext controllerContext)
        {
            return "Exception occured in " + controllerContext.RouteData.Values["action"].ToString() 
                + " action method of " + controllerContext.RouteData.Values["controller"].ToString() + "Controller";
        }
    }
}
