using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Timers;

class Program
{
    private static System.Timers.Timer timer = new System.Timers.Timer();
    private static int lastBatteryLevel = -1;

    static void Main()
    {
        timer = new System.Timers.Timer(3000); // Check every 3 seconds
        timer.Elapsed += CheckBatteryStatus;
        timer.Start();

        Console.WriteLine("Battery status checker started. Press Enter to exit...");
        Console.ReadLine(); // Keep the console application running
    }

    private static void CheckBatteryStatus(object? sender, ElapsedEventArgs e)
    {
        // Path to the Python executable
        string pythonPath = "py";

        // Path to the Python script
        string relativeScriptPath = "..\\..\\..\\main.py";
        string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeScriptPath);

        // Create a new process to run the Python script
        ProcessStartInfo start = new ProcessStartInfo
        {
            FileName = pythonPath,
            Arguments = scriptPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process? process = Process.Start(start);
        if (process == null)
        {
            Console.WriteLine("Failed to start process.");
            return;
        }

        using (process)
        {
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                Console.WriteLine(result);

                // Extract battery level and status using regular expressions
                var match = Regex.Match(result, @"Battery level: (\d+)%, Status: (\w+ \w+)");
                if (match.Success)
                {
                    int batteryLevel = int.Parse(match.Groups[1].Value);
                    string status = match.Groups[2].Value;

                    Console.WriteLine($"Extracted Battery Level: {batteryLevel}%");
                    Console.WriteLine($"Extracted Status: {status}");

                    // Print a warning if the battery level is 5% or below
                    if (batteryLevel <= 5 && batteryLevel != lastBatteryLevel)
                    {
                        Console.WriteLine($"Battery Warning: Battery level is {batteryLevel}%");
                    }

                    // Update the last battery level
                    lastBatteryLevel = batteryLevel;
                }
                else
                {
                    Console.WriteLine("Failed to extract battery information.");
                }
            }

            // Read and display any errors
            using (StreamReader errorReader = process.StandardError)
            {
                string error = errorReader.ReadToEnd();
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine("Error: " + error);
                }
            }

            process.WaitForExit();
        }
    }
}