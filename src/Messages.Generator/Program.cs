using System;
using System.Threading.Tasks;
using Messages.Infrastructure;

namespace Messages.Generator
{
    class Program
    {
        int intervalMs = 100, busyTimeSeconds = 1;

        private void PrintUsage()
        {
            Console.WriteLine($"generator [--busy-time BUSY_TIME_IN_SECONDS] [--interval INTERVAL_IN_MILLISECONDS]");
        }

        private bool ParseCommandLine(string[] args)
        {
            int i = 0;
            while (i < args.Length)
            {
                switch (args[i])
                {
                    case "--busy-time":
                    case "-b":
                        i++;
                        busyTimeSeconds = int.Parse(args[i]);
                        break;
                    case "--interval":
                    case "-i":
                        i++;
                        intervalMs = int.Parse(args[i]);
                        break;
                    case "--help":
                    case "-h":
                        return false;
                    default:
                        throw new ArgumentException("Unknown argument: " + args[i]);
                }
                i++;
            }
            return true;
        }

        private void LoadFromEnvironment()
        {
            string BUSY_TIME_SECONDS = Environment.GetEnvironmentVariable("BUSY_TIME_SECONDS");
            string INTERVAL_MS = Environment.GetEnvironmentVariable("INTERVAL_MS");
            if (!string.IsNullOrWhiteSpace(INTERVAL_MS))
            {
                intervalMs = int.Parse(INTERVAL_MS);
            }
            if (!string.IsNullOrWhiteSpace(BUSY_TIME_SECONDS))
            {
                busyTimeSeconds = int.Parse(BUSY_TIME_SECONDS);
            }
        }

        public async Task StartAsync(string[] args)
        {
            LoadFromEnvironment();
            try
            {
                if (!ParseCommandLine(args))
                {
                    PrintUsage();
                    return;
                }
            }
            catch (ArgumentException exc)
            {
                Console.Error.WriteLine(exc.Message);
                PrintUsage();
            }

            var bus = EnvironmentMessageBusFactory.GetMessageBus();

            var generator = new Generator(
                bus,
                intervalMs,
                busyTimeSeconds);
            await generator.SendLoopAsync();
        }
        static void Main(string[] args)
        {
            new Program().StartAsync(args).Wait();
        }
    }
}
