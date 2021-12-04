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

        private List<Record> getRun(Tape tape)
        {
            List<Record> list = new List<Record>();

            if (!tape.EOF)
            {
                Record record = tape.Read();

                list.Add(record);

                while (!tape.EOF && !(record > tape.Peek()))
                {
                    record = tape.Read();
                    list.Add(record);
                }
            }

            return list;
        }

        private void Merge()
        {
            if (Settings.DEBUG)
                Console.WriteLine("Merging Records");
            Records.Clear();

            while (!FirstTape.EOF || !SecondTape.EOF)
            {
                List<Record> firstRecords = getRun(FirstTape);
                List<Record> secondRecords = getRun(SecondTape);

                int firstCounter = 0;
                int secondCounter = 0;

                while (true)
                {
                    if (firstCounter >= firstRecords.Count && secondCounter >= secondRecords.Count) break;

                    if (firstCounter >= firstRecords.Count)
                    {
                        while (secondCounter < secondRecords.Count)
                        {
                            if (Settings.DEBUG) Console.WriteLine(secondRecords[secondCounter].Value);
                            Records.Write(secondRecords[secondCounter]);
                            secondCounter++;
                        }

                        break;
                    }

                    if (secondCounter >= secondRecords.Count)
                    {
                        while (firstCounter < firstRecords.Count)
                        {
                            if (Settings.DEBUG) Console.WriteLine(firstRecords[firstCounter].Value);
                            Records.Write(firstRecords[firstCounter]);
                            firstCounter++;
                        }

                        break;
                    }

                    if (firstRecords[firstCounter] < secondRecords[secondCounter])
                    {
                        if (Settings.DEBUG) Console.WriteLine(firstRecords[firstCounter].Value);
                        Records.Write(firstRecords[firstCounter]);
                        firstCounter++;
                    } else
                    {
                        if (Settings.DEBUG) Console.WriteLine(secondRecords[secondCounter].Value);
                        Records.Write(secondRecords[secondCounter]);
                        secondCounter++;
                    }
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
