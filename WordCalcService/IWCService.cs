using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WordCalcService
{
    [ServiceContract]
    public interface IWCService
    {
        [OperationContract]
        Dictionary<string, int> WordCalculateText(string text);
    }
}
