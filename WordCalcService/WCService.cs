using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using WordCalcDll;

namespace WordCalcService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WCService : IWCService
    {
        public Dictionary<string, int> WordCalculateText(string text)
        {
            WordCalculator wordCalc = new WordCalculator(text);
            var map = wordCalc.CalculateWordsFullText();
            return map;
        }
    }
}
