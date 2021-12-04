using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    class RecordSorter
    {
        private Tape Records;
        private Tape FirstTape = new Tape("FirstTape.dat");
        private Tape SecondTape = new Tape("SecondTape.dat");
        private bool Sorted = false;
        public int Phases { get; set; } = 0;

        public RecordSorter(Tape records)
        {
            Records = records;
        }

        private void Distribute()
        {
            if (Settings.DEBUG) Console.WriteLine("Distributing Records");
            Record prevRecord = new Record(-0.1);
            Tape currTape = FirstTape;
            Sorted = true;
            if (Settings.DEBUG) Console.WriteLine($"\n{(currTape == FirstTape ? "First Tape" : "Second Tape")}");

            while (!Records.EOF)
            {
                Record currRecord = Records.Read();

                if (prevRecord > currRecord)
                {
                    currTape = currTape == FirstTape ? SecondTape : FirstTape;
                    Sorted = false;
                    if (Settings.DEBUG) Console.WriteLine($"\n{(currTape == FirstTape ? "First Tape" : "Second Tape")}");
                }

                prevRecord = currRecord;

                if (Settings.DEBUG)
                    Console.WriteLine(currRecord.Value);
                currTape.Write(currRecord);
            }

            if (Settings.DEBUG)
                Console.WriteLine("Finished Distributing Records\n");
        }

        private void Merge()
        {
            if (Settings.DEBUG)
                Console.WriteLine("Merging Records");
            Records.Clear();

            while (!FirstTape.EOF || !SecondTape.EOF)
            {
                Record firstRecord = FirstTape.Peek();
                Record secondRecord = SecondTape.Peek();

                if (FirstTape.EOF)
                {
                    while (!SecondTape.EOF)
                    {
                        secondRecord = SecondTape.Read();
                        if (Settings.DEBUG)
                            Console.WriteLine(secondRecord.Value);
                        Records.Write(secondRecord);
                    }

                    break;
                } else if (SecondTape.EOF)
                {
                    while (!FirstTape.EOF)
                    {
                        firstRecord = FirstTape.Read();
                        if (Settings.DEBUG)
                            Console.WriteLine(firstRecord.Value);
                        Records.Write(firstRecord);
                    }

                    break;
                }


                if (firstRecord < secondRecord)
                {
                    firstRecord = FirstTape.Read();
                    if (Settings.DEBUG)
                        Console.WriteLine(firstRecord.Value);
                    Records.Write(firstRecord);
                } else
                {
                    secondRecord = SecondTape.Read();
                    if (Settings.DEBUG)
                        Console.WriteLine(secondRecord.Value);
                    Records.Write(secondRecord);
                }
               
            }

            FirstTape.Clear();
            SecondTape.Clear();

            if (Settings.DEBUG)
                Console.WriteLine("Finished Merging Records\n");
        }
        
        public void Sort()
        {
            while (!Sorted)
            {
                Phases++;
                Console.WriteLine($"Phase: {Phases}");
                Distribute();
                Merge();
            }
        }
    }
}
