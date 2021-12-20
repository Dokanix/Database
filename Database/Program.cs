using System;

namespace Database
{
    class Program
    {
        static void Main(string[] args)
        {
            RecordGenerator recordGenerator = new RecordGenerator("Records.dat");
            Tape records = recordGenerator.GenerateRecords(12);

            RecordSorter recordSorter = new RecordSorter(records);
            recordSorter.Sort();

            while (!records.EOF)
            {
                Console.WriteLine($"Sorted: {records.Read().Value}");
            }

            Console.WriteLine($"Accesses: {Tape.FileAccessCount}");
            Console.WriteLine($"Phases: {recordSorter.Phases}");
        }
    }
}
