using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    class RecordGenerator
    {
        private static Random Random = new Random();
        private string FileName;

        public RecordGenerator(string fileName)
        {
            FileName = fileName;
        }

        private double GenerateDouble()
        {
            return Random.NextDouble() * 1000;
        }

        private double GenerateInteger()
        {
            return Random.Next(1000);
        }

        public Tape GenerateRecords(int count)
        {
            Tape records = new Tape(FileName);

            Console.WriteLine("Generating Records");

            for (int i = 0; i < count; i++)
            {
                double randomRecord;

                // Randomize the records so that half of them are 
                // Integers (Saved as 8bit Doubles) and half of them are Doubles 
                if (Random.Next(2) == 0)
                {
                    randomRecord = GenerateInteger();
                }
                else
                {
                    randomRecord = GenerateDouble();
                }

                Console.WriteLine(randomRecord);
                records.Write(new Record(randomRecord));
            }

            Console.WriteLine("Finished Generating Records\n");

            return records;
            
        }
    }
}
