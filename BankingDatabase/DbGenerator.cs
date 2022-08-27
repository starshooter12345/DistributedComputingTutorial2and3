using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingDatabase
{
    public class DbGenerator : DataStruct
    {
        private string GetFirstName
        {
            get { return firstName; }

        }

        private string GetLastName
        {
            get { return lastName; }

        }

        private uint GetPIN
        {
            get { return pin; }

        }

        private uint GetAcctNo
        {
            get { return acctNo; }

        }

        private int GetBalance
        {
            get { return balance; }

        }

        private string GetImage
        {
            get { return img; }
        }

        public void GetNextAccount(out uint pin, out uint acctNo, out string firstName, out string lastName, out int balance, out string img)
        {
            pin = GetPIN;
            acctNo = GetAcctNo;
            firstName = GetFirstName;
            lastName = GetLastName;
            balance = GetBalance;
            img= GetImage;

        }
    }
}
