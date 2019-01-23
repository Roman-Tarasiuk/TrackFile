using System;
using System.Timers;
using System.IO;

namespace TrackFile
{
    public class Tracker
    {
        private static System.Timers.Timer aTimer;
        private static string targetFilePath;
        private static string logFileName = "TrackFile.log";
        private static int checkInterval = 60;
        
        public static void Main(string[] args)
        {
            if (args.Length == 0 || args.Length > 2)
            {
                ShowUsage();
                return;
            }

            targetFilePath = args[0];

            if (!File.Exists(targetFilePath))
            {
                Console.WriteLine("The specified file (" + targetFilePath + ") does not exist.");
                Console.WriteLine("Exit.");
                return;
            }

            if (args.Length == 2)
            {
                int.TryParse(args[1], out checkInterval);
            }

            var intervalMs = checkInterval *= 1000;

            //

            SetTimer(intervalMs);

            var now = DateTime.Now;
            Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", now);

            CheckFileExists();

            Console.ReadLine();
            aTimer.Stop();
            aTimer.Dispose();
            
            Console.WriteLine("Terminating the application...");
        }

        private static void SetTimer(int interval)
        {
            aTimer = new System.Timers.Timer(interval);

            aTimer.Elapsed += (source, e) => {
                CheckFileExists();
            };

            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void CheckFileExists()
        {
            string info = targetFilePath;
            
            if (File.Exists(targetFilePath))
            {
                info += " : OK.";
            }
            else
            {
                info += " : File is gone.";
            }

            Console.WriteLine(DateTime.Now.ToString() + " : " + info);
            Log(info);
        }

        private static void Log(string info)
        {
            try
            {
                var time = DateTime.Now;

                using (var writer = new StreamWriter(logFileName, true))
                {
                    writer.WriteLine(time.ToString() + " : " + info);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Program execution error:");
                Console.WriteLine(exception.ToString());
                Console.WriteLine("Exit.");
            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine(
@"Program usage:
TrackFile.exe file_path [interval]
  Interval in seconds. The default interval is 60 seconds."
            );
        }
    }
}
