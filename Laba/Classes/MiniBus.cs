using System;
using System.Collections.Generic;
using System.Text;

namespace Buses.Classes
{
    public class MiniBus:Transport
    {
        public MiniBus():base() { }
        public MiniBus(int capacity):base(capacity) { }
        public override string ToString()
        {
            return $"[MiniBus {Id}]\n" +
                   $"Capacity: {Capacity}";
        }
    }
}
