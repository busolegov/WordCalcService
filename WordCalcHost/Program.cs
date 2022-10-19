using System;
using System.ServiceModel;

namespace WordCalcHost
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(WordCalcService.WCService)))
            {
                host.Open();
                Console.WriteLine("Хост запущен!");
                Console.ReadLine();
            }
        }
    }
}
