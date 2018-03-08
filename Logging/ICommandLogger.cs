using System;

namespace Arta.Infrastructure.Logging
{
    public interface ICommandLogger
    {
        void LogCommand(string category, string user, string uid, string description, string updatedValue, string originalValue);
    }
}