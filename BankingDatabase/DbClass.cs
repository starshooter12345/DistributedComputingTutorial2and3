using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BankingDatabase
{
    public class DbClass
    {
        List<DataStruct> dataentries;

        public DbClass()
        {
            dataentries = new List<DataStruct>();

            string imgloca = Directory.GetCurrentDirectory() + @"\Images\";

            string[] firstNames = { "Michael", "Tushan", "Britney", "Christina", "Timberlake", "Yash", "Jessy", "Chloe", "Oscar", "Indi" };
            string[] lastNames = { "Jackson", "Yamana", "Spears", "Aguilera", "Clohessy", "Tajput", "Yorker", "Tilan", "Milan", "Lamboise" };
            //Using Random with a seed value
            Random random = new Random(1);

            
            for (int i=0;i<100000;i++)
            {
                DataStruct account1 = new DataStruct();
                account1.firstName=firstNames[random.Next(0,10)];
                account1.lastName = lastNames[random.Next(0, 10)];
                account1.balance = random.Next(0, 1000000);
                account1.pin = (uint)random.Next(111,999);
                account1.acctNo = (uint)random.Next(10, 1000);
                account1.img = imgloca+random.Next(0, 2)+".jpg";
                dataentries.Add(account1);


            }






        }
        
        public uint GetAcctNoByIndex(int index)
        {
            return dataentries[index - 1].acctNo;
        }

        public uint GetPINByIndex(int index)
        {
            return dataentries[index - 1].pin;
        }
        public string GetFirstNameByIndex(int index)
        {
            return dataentries[index - 1].firstName;
        }
        public string GetLastNameByIndex(int index)
        {
            return dataentries[index - 1].lastName;
        }
        public int GetBalanceByIndex(int index)
        {
            return dataentries[index - 1].balance;
        }
        public string GetImgLocaByIndex(int index)
        {
            return dataentries[index - 1].img;
        }
        public int GetNumRecords()
        {
            return dataentries.Count;
        }

    }
}
