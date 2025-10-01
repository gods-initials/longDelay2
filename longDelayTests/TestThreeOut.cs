using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace longDelayTests
{
    public class TestThreeOut : Test
    {
        private Random rand;
        private int testDuration;
        private bool testSuccessful;
        private CancellationTokenSource cts;
        private string error;
        private int param1;
        private bool param2;
        private string param3;
        public TestThreeOut(CancellationTokenSource tokenSource)
        {
            cts = tokenSource;
            param1 = 0;
            param2 = false;
            param3 = "";
            error = "";
            rand = new Random();
            testDuration = 10000;
        }
        private string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, rand.Next(1, 11)).Select(s => s[rand.Next(s.Length)]).ToArray());
        }
        public override async Task Run()
        {
            await Task.Delay(testDuration, cts.Token);
            param1 = rand.Next();
            Console.WriteLine(param1);
            await Task.Delay(testDuration, cts.Token);
            param2 = Convert.ToBoolean(rand.Next(2));
            Console.WriteLine(param2);
            await Task.Delay(testDuration, cts.Token);
            param3 = RandomString();
            Console.WriteLine(param3);
            testSuccessful = Convert.ToBoolean(rand.Next(2));
            if (!testSuccessful)
            {
                switch (rand.Next(3))
                {
                    case 0:
                        error = "Изделие не найдено.";
                        break;
                    case 1:
                        error = "Некорректно заполнены параметры изделия.";
                        break;
                    case 2:
                        error = "Внутренняя ошибка.";
                        break;
                }
            }
        }
        public override Dictionary<string, string> ReturnResults()
        {
            return new Dictionary<string, string>
            {
                {"testSuccessful", testSuccessful.ToString()},
                {"error", error},
                {"param1", param1.ToString()},
                {"param2", param2.ToString()},
                {"param3", param3}
            };
        }
    }
}