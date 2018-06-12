using System;
using Arta.Subscriptions.Activate.Events.ArtaInfra.Logging;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Arta.Infrastructure.Logging
{
    public class CommandLogger : ICommandLogger
    {

        private readonly IBus _bus;

        //public CommandLogger(IBus bus)
        //{
        //    _bus = bus;
        //}

        public void LogCommand(string account, TableType tableType, string tableKey, string command, string value = null, string changed = null)
        {
            LogCommand(null, account, tableType, tableKey, command, value, changed);
        }

        public void LogCommand(string partner, string account, TableType tableType, string tableKey, string command, string value = null, string changed = null)
        {
            LogCommand(partner, DateTime.Now, AccountType.WebUser, account, tableType, tableKey, command, value, changed);
        }

        public void LogCommand(string partner, DateTime datetime, AccountType accountType, string account, TableType tableType, string tableKey, string command, string value = null, string changed = null)
        {
            //_bus.Publish(
            //    new CommandLogEvent
            //    {
            //        Partner = partner,
            //        DateTime = datetime,
            //        AccountType = accountType,
            //        Account = account,
            //        TableType = tableType,
            //        TableKey = tableKey,
            //        Command = command,
            //        Value = value,
            //        Changed = changed
            //    });
        }
    }
}
