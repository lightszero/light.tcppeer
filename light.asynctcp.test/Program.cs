using System;

namespace light.asynctcp.test
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncMain();
            while (true)
            {
                var l = Console.ReadLine();
                if(l=="l")
                {
                    TestLink();
                }
            }
        }

        static async void AsyncMain()
        {
            Console.WriteLine("Hello World!");
            light.asynctcp.ServerModule module = new ServerModule();
            module.Listen("127.0.0.1", 8888);
        }
        static async void TestLink()
        {

        }
            
    }
}
