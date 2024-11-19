using System;
using System.Diagnostics;
using System.IO;
class Program
{
    static void Main()
    {
        // Path to the Python executable
        string pythonPath = "py";

        // Path to the Python script
        string relativeScriptPath = "..\\..\\..\\main.py";
        string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeScriptPath);

        // Create a new process to run the Python script
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = pythonPath;
        start.Arguments = scriptPath;
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        start.RedirectStandardError = true; // Redirect standard error to capture errors
        start.CreateNoWindow = true;

        // Start the process and read the output
        Process? process = Process.Start(start);
        if (process == null)
        {
            Console.WriteLine("Failed to start process.");
            return;
        }

        using (process)
        {
            using (System.IO.StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                Console.WriteLine(result);
            }

            // Read and display any errors
            using (System.IO.StreamReader errorReader = process.StandardError)
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