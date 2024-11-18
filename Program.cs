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
        string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "main.py");


        // Create a new process to run the Python script
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = pythonPath;
        start.Arguments = scriptPath;
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        start.CreateNoWindow = true;

        // Start the process and read the output
        using (Process process = Process.Start(start))
        {
            using (System.IO.StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                Console.WriteLine(result);
            }
        }
    }
}