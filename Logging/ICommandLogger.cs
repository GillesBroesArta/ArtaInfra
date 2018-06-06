using System;
using Arta.Subscriptions.Activate.Events.ArtaInfra.Logging;

namespace Arta.Infrastructure.Logging
{
    public interface ICommandLogger
    {
        void LogCommand(string account, TableType tableType, string tableKey, string command, string value = null,
            string changed = null);

        void LogCommand(string partner, string account, TableType tableType, string tableKey, string command,
            string value = null, string changed = null);


        void LogCommand(string partner, DateTime datetime, AccountType accountType, string account, TableType tableType,
            string tableKey, string command, string value = null, string changed = null);

    }
}