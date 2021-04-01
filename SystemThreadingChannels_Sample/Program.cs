using System;
using System.Threading.Tasks;

namespace SystemThreadingChannels_Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var customChannel = new CustomChannel<int>();
            _ = Task.Run(async delegate
            {
                for (int i = 0; ; i++)
                {
                    await Task.Delay(1000);
                    customChannel.Write(i);
                }
            });

            while (true)
            {
                Console.WriteLine(await customChannel.ReadAsync());
            }
        }
    }
}
