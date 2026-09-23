using System;
using System.Threading;

class Program
{
    static long[] data = new long[10_000_000];

    static long[] partialSums = null!;

    static int numWorkers;

    static void SumSlice(object? arg)
    {
        int index = (int)arg!;

        int sliceSize = data.Length / numWorkers;

        int start = index * sliceSize;

        int end;

        if (index == numWorkers - 1)
        {
            end = data.Length;
        }
        else
        {
            end = start + sliceSize;
        }

        long sum = 0;

        for (int i = start; i < end; i++)
        {
            sum += data[i];
        }

        partialSums[index] = sum;
    }

    static void Main()
    {
        // Fill the array
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = i + 1;
        }

        // Number of worker threads
        numWorkers = Environment.ProcessorCount;

        Console.WriteLine($"Array size: {data.Length}");
        Console.WriteLine($"Worker threads: {numWorkers}");

        partialSums = new long[numWorkers];

        Thread[] threads = new Thread[numWorkers];

        // Create and start worker threads
        for (int i = 0; i < numWorkers; i++)
        {
            int index = i;

            threads[i] = new Thread(() => SumSlice(index));

            threads[i].Start();
        }

        // Wait for every thread
        for (int i = 0; i < numWorkers; i++)
        {
            threads[i].Join();
        }

        // Combine partial sums
        long threadedTotal = 0;

        foreach (long partial in partialSums)
        {
            threadedTotal += partial;
        }

        // Sequential calculation
        long sequentialTotal = 0;

        foreach (long value in data)
        {
            sequentialTotal += value;
        }

        Console.WriteLine($"Threaded total: {threadedTotal}");
        Console.WriteLine($"Sequential total: {sequentialTotal}");
        Console.WriteLine($"Match: {threadedTotal == sequentialTotal}");
    }
}