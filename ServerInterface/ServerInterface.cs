using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface
{
    [ServiceContract]
    //This is the public interface for the .NET server
    public interface ServerInterface
    {
        [OperationContract]

        int GetNumEntries();

        [OperationContract]

        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out string img);
    }
}
