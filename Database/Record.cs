using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    [DebuggerDisplay("{Value}")]
    class Record
    {
        public double Value { get; set; }
        private bool IsInteger { get => Math.Abs(Value - Math.Floor(Value)) < (Double.Epsilon); }

        public Record (double value)
        {
            Value = value;
        }

        public static bool operator >(Record a, Record b)
        {
            if (!a.IsInteger && b.IsInteger)
            {
                return false;
            } 
            else if (a.IsInteger && !b.IsInteger)
            {
                return true;
            }
            else
            {
                return a.Value > b.Value;
            }
        }

        public static bool operator <(Record a, Record b)
        {
            if (!a.IsInteger && b.IsInteger)
            {
                return true;
            }
            else if (a.IsInteger && !b.IsInteger)
            {
                return false;
            }
            else
            {
                return a.Value < b.Value;
            }
        }
    }
}
