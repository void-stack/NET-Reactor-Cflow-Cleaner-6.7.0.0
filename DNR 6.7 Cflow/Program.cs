using System.Diagnostics;
using System.Drawing;
using Colorful;
using DNR.Core;
using DNR.Utils;

namespace DNR
{
    internal static class Program
    {
        private static readonly string asciiArt = @"
        _________                __                 
        \_   ___ \  ____________/  |_  ____ ___  ___
        /    \  \/ /  _ \_  __ \   __\/ __ \\  \/  /
        \     \___(  <_> )  | \/|  | \  ___/ >    < 
         \______  /\____/|__|   |__|  \___  >__/\_ \
                \/                        \/      \/
                   .NET Reactor 6.7.0.0 Cflow
                 By https://github.com/Hussaryn
                       Credits SychicBoy

        ";

        public static void Main(string[] args) {
            Console.Title = "DNR 6.7.0.0 Cflow Cleaner";
            Console.WriteLine(asciiArt, Color.IndianRed);

            var logger = new Logger();
            var stopwatch = Stopwatch.StartNew();
            
            if (args.Length < 1) {
                logger.Warning("Usage: DNR 6.7 Cflow.exe <path>");
                Console.ReadLine();
                return;
            }
          
            var options = new CtxOptions(args[0], logger);
            var ctx = new Context(options);

            // There is only that so we are just making simple call
            CflowRemover.Execute(ctx);
            ctx.Save();

            logger.Success($"Cleaned {CflowRemover.FixedMethods} methods!");
            logger.Success($"Saved in {ctx.Options.OutputPath}!");
            
            stopwatch.Stop();
            logger.Success($"Finished all tasks in {stopwatch.Elapsed}");
            Console.ReadKey();
        }
    }
}