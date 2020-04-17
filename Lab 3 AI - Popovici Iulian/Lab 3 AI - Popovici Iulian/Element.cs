using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3_AI___Popovici_Iulian
{
    public class Element
    {
        public string Solution { get; set; }
        public int Fitness { get; set; }
        public Element(string solution,int fitness)
        {
            Solution = solution;
            Fitness = fitness;
        }

        public Element()
        {
        }
    }
}
