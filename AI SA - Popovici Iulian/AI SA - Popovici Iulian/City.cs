using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SA___Popovici_Iulian
{
    public class City
    {
        public int Id { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public City(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }
    }
}
