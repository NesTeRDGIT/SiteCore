using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using IdentiModel;

namespace IdentiScaner
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,IncludeExceptionDetailInFaults = true,ConcurrencyMode = ConcurrencyMode.Multiple)]
    class WCFService: IWCFIdentiScaner
    {
        public bool Connect()
        {
            BankAcc.AddAcc(OperationContext.Current);
            return true;
          
        }

        public bool Ping()
        {
            return true;
        }

        public EntriesMy[] GetEventLogEntry()
        {
            var rez = new List<EntriesMy>();
            try
            {
                if (!EventLog.Exists("IdentiServer")) return rez.ToArray();
                var EventLog1 = new EventLog { Source = "IdentiServer" };
                for (var i = EventLog1.Entries.Count-1; i >= 0; i--)
                {
                    var entry = EventLog1.Entries[i];
                    var item = new EntriesMy { Message = entry.Message, TimeGenerated = entry.TimeGenerated };
                    switch (entry.EntryType)
                    {
                        case EventLogEntryType.Error: item.Type = TypeEntries.error; break;
                        case EventLogEntryType.Warning: item.Type = TypeEntries.warning; break;
                        default:
                            item.Type = TypeEntries.message; break;
                    }
                    rez.Add(item);
                }
                return rez.ToArray();
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }
    }
}
