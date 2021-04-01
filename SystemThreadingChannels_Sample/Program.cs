using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SystemThreadingChannels_Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string type = "";
            do
            {
                Console.Write("What kind of channel? custom/SystemThreadingChannels (c/s): ");
                type = Console.ReadLine();
            }
            while (type != "c" && type != "s");
            switch (type)
            {
                case "c":
                    await CustomChannelAsync();
                    break;
                case "s":
                    await SystemThreadingChannelAsync();
                    break;
                default:
                    break;
            }

            
        }

        private static async Task CustomChannelAsync()
        {
            var channel = new CustomChannel<int>();
            _ = Task.Run(async delegate
            {
                for (int i = 0; ; i++)
                {
                    await Task.Delay(1000);
                    channel.Write(i);
                }
            });

            while (true)
            {
                Console.WriteLine(await channel.ReadAsync());
            }
        }

        private static async Task SystemThreadingChannelAsync()
        {
            var channel = Channel.CreateUnbounded<int>();
            _ = Task.Run(async delegate
            {
                for (int i = 0; ; i++)
                {
                    await Task.Delay(1000);
                    await channel.Writer.WriteAsync(i);
                }
            });

            while (true)
            {
                Console.WriteLine(await channel.Reader.ReadAsync());
            }
        }
    }
}
