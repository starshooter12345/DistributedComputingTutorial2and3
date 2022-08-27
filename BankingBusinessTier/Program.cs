using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BankingBusinessTier
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hey so like welcome to my business server");
            //This is the actual host service system
            ServiceHost host;
            //This represents a tcp/ip binding in the windows network stack
            NetTcpBinding tcp = new NetTcpBinding();
            //Bind server to the implementation of the server interface
            host = new ServiceHost(typeof(BusinessServer));

            host.AddServiceEndpoint(typeof(BusinessServerInterface), tcp, "net.tcp://0.0.0.0:8200/BankBusinessService");
            //And open the host for Business
            host.Open();
            Console.WriteLine("System online");
            Console.ReadLine();
            //Don't forget to close the host after you are done
            host.Close();

        }
    }
}
