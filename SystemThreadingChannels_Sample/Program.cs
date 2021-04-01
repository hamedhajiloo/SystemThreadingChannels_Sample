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
                Console.Write("What kind of channel? custom/SystemThreadingChannels/SystemThreadingBoundedChannels (c/s/b): ");
                type = Console.ReadLine();
            }
            while (type != "c" && type != "s" && type != "b");
            switch (type)
            {
                case "c":
                    await CustomChannelAsync();
                    break;
                case "s":
                    await SystemThreadingChannelAsync();
                    break;
                case "b":
                    await SystemThreadingBoundedChannelAsync();
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

        private static async Task SystemThreadingBoundedChannelAsync()
        {
            var channel = Channel.CreateBounded<int>(5);
            _ = Task.Run(async delegate
            {
                for (int i = 0; i < 10; i++)
                {
                    await Task.Delay(100);
                    await channel.Writer.WriteAsync(i);
                }
                channel.Writer.Complete();
            });

            await foreach (var item in channel.Reader.ReadAllAsync())
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Done!");
        }
    }
}
