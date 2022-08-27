using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BankingServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server connected!!");
            ServiceHost host;
            NetTcpBinding tcp = new NetTcpBinding();
            //ServerImplementation is the internal class that has the implementation of the Server
            host = new ServiceHost(typeof(ServerImplementation));
            host.AddServiceEndpoint(typeof(BankingServerInterface.ServerInterface), tcp, "net.tcp://0.0.0.0:8100/DataService");
            host.Open();
            Console.WriteLine("The System is online!!");
            Console.ReadLine();
            host.Close();
        }
    }
}
