﻿using System;

namespace Arta.Infrastructure.Logging
{
    public interface IApiLogger
    {
        void LogInformation<T>(T message, LoggingType? loggingType = null);
        void LogException(Exception ex);
        void LogException(string message, Exception ex);
        void LogError(string error);
    }
}