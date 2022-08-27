using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using BankingDatabase;

namespace BankingServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class ServerImplementation : BankingServerInterface.ServerInterface
    {
        public ServerImplementation()
        {

        }

        int BankingServerInterface.ServerInterface.GetNumEntries()
        {
            var dataclass = new DbClass();
            return dataclass.GetNumRecords();
        }

        void BankingServerInterface.ServerInterface.GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName,out string img)
        {
            var dataclass = new DbClass();
            fName = dataclass.GetFirstNameByIndex(index);
            lName = dataclass.GetLastNameByIndex(index);
            acctNo = dataclass.GetAcctNoByIndex(index);
            pin = dataclass.GetPINByIndex(index);
            bal = dataclass.GetBalanceByIndex(index);
            img = dataclass.GetImgLocaByIndex(index);

        }
    }
}
