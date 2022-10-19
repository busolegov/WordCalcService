using System;
using System.IO;
using System.ServiceModel;
using System.Text;

namespace WordCalcClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            string configName = "BasicHttpBinding_IWCService";
            WordCalcService.WCServiceClient client;
            string text;

            try
            {
                client = new WordCalcService.WCServiceClient(configName);

                Console.WriteLine("Вы подключены к сервису WordCalc!\n");
                Console.WriteLine("Введите путь к файлу для подсчета слов.\n");
                text = File.ReadAllText(Console.ReadLine(), Encoding.UTF8);

                if (text != "")
                {
                    var mapFromService = client.WordCalculateText(text);
                    if (mapFromService != null)
                    {
                        using (StreamWriter streamWriter = new StreamWriter("result.txt", false, Encoding.UTF8))
                        {
                            foreach (var item in mapFromService)
                            {
                                streamWriter.WriteLine("{0,-30} {1,5}", item.Key, item.Value);
                            }
                        }
                        Console.WriteLine("Полученный от сервиса словарь записан в файл result.txt\n");
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка получения словаря от сервиса.");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Console.WriteLine("Файл пустой.");
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }
    }
}
