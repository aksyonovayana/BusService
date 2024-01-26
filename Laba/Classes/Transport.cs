using System;
using System.Collections.Generic;
using System.Text;

namespace Buses.Classes
{
    public abstract class Transport
    {
        public int Id { get; set; }
        public int Capacity {  get; set; }
        public virtual Journey? Journey { get; set; }

        public Transport()
        {
            Capacity = 0;
            Journey = new Journey();
        }
        public Transport(int capacity):base() 
        {
            Capacity = capacity;
        }

        public abstract override string ToString();
    }
}
