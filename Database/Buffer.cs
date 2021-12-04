using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    class Buffer : IEnumerable
    {
        private Record[] Records;
        private int Size = 0;
        private int Current = 0;
        private int Capacity;
        public bool Full { get => Size == Capacity; }
        public bool Finished { get => Current == Size; }
        public bool Empty { get => Size == 0; }

        public Buffer(int capacity)
        {
            Capacity = capacity;
            Records = new Record[capacity];
        }

        public void Append(Record record)
        {
            if (Full) throw new Exception("Can't append to a full buffer");

            Records[Size] = record;
            Size++;
        }

        public Record Peek()
        {
            return Records[Current];
        }

        public Record Next()
        {
            Record record = Records[Current];
            Current++;

            

            if (Finished) Clear();

            return record;
        }

        public void Clear()
        {
            // if (Settings.DEBUG) Console.WriteLine("Clearing");
            Size = 0;
            Current = 0;
        }

        public IEnumerator GetEnumerator()
        {
            for (int index = 0; index < Size; index++)
            {
                yield return Records[index];
            }
        }
    }
}
