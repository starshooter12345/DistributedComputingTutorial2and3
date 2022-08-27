using BankingServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankingBusinessTier
{
    internal class BusinessServer : BusinessServerInterface
    {
        private BankingServerInterface.ServerInterface foob;
        uint LogNumber;
        public BusinessServer()
        {
            ChannelFactory<BankingServerInterface.ServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //setting the url and creating the connection:
            string URL = "net.tcp://localhost:8100/DataService";
            foobFactory = new ChannelFactory<BankingServerInterface.ServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }

        public int GetNumEntries()
        {
            try
            {
                return foob.GetNumEntries();
            }
            finally
            {
                Log("Total number of entries: " + foob.GetNumEntries());
            }


        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out string img)
        {
            foob.GetValuesForEntry(index, out acctNo, out pin, out bal, out fName, out lName, out img);
            Log("Values for index:" + index + "Has been returned");
        }

        public void GetValuesForSearch(string searchText, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out string img)
        {
            fName = null;
            lName = null;
            acctNo = 0;
            pin = 0;
            bal = 0;
            img = null;
            int numEntry = foob.GetNumEntries();
            for (int i = 1; i <= numEntry; i++)
            {
                string firstName;
                uint accountNo;
                string lastName;
                uint pinno;
                int balance;
                string image;

                foob.GetValuesForEntry(i, out accountNo, out pinno, out balance, out firstName, out lastName, out image);
                if (lastName.ToLower().Contains(searchText.ToLower()))
                {
                    acctNo = accountNo;
                    pin = pinno;
                    bal = balance;
                    fName = firstName;
                    lName = lastName;
                    img = image;
                    break;
                }

                Log("---------------------------------------");
                Log("Last Name: " + lastName);
                Log("First Name: " + firstName);
                Log("Pin: " + pinno);
                Log("Account number: " + accountNo);
                Log("Balance: " + balance);
                Log("Image: " + image);
                Log("---------------------------------------");

                WriteLog("---------------------------------------");
                WriteLog("Last Name: " + lastName);
                WriteLog("First Name: " + firstName);
                WriteLog("Pin: " + pinno);
                WriteLog("Account number: " + accountNo);
                WriteLog("Balance: " + balance);
                WriteLog("Image: " + image);
                WriteLog("---------------------------------------");



            }

            Thread.Sleep(5000);
        }

        public static void WriteLog(string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            /*string logFilePath = "C:\\Users\\Admins\\Desktop\\20907411_Tutorial 2 and 3\\BankingDatabase";*/
            string logFilePath = Directory.GetCurrentDirectory() + @"\Logs\";
            logFilePath = logFilePath + "\\Log-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine(strLog);
            log.Close();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        void Log(string logString)
        {
            LogNumber++;
            Console.WriteLine("--------------------------" + LogNumber);
            Console.WriteLine(logString);

        }
    }
}
