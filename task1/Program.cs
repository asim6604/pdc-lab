using System;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        // If --child argument is present, run as child
        if (args.Length > 0 && args[0] == "--child")
        {
            RunChild();
        }
        else
        {
            RunParent();
        }
    }

    static void RunChild()
    {
        Console.WriteLine($"[Child] PID = {Environment.ProcessId}");

        int counter = 100;

        counter += 50;

        Console.WriteLine($"[Child] Final counter = {counter}");
    }

    static void RunParent()
    {
        Console.WriteLine($"[Parent] PID = {Environment.ProcessId}");

        int counter = 100;

        counter += 1;

        string executablePath = Environment.ProcessPath!;

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = executablePath
        };

        startInfo.ArgumentList.Add("--child");

        Process? child = Process.Start(startInfo);

        if (child != null)
        {
            child.WaitForExit();
        }

        Console.WriteLine($"[Parent] Final counter = {counter}");

        Console.WriteLine(
            "[Parent] Parent and child counters were modified independently."
        );
    }
}