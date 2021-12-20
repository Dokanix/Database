using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    static class Settings
    {
        public static int PAGE_SIZE { get; } = 100;
        public static int RECORD_SIZE { get; } = 8;
        public static bool DEBUG { get; } = true;
    }
}
