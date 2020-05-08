using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Lab_4
{
    class Program
    {
        static void Main(string[] args)
        {
            TSP tsp = new TSP();

            Console.WriteLine("Enter the number of iterations: ");
            int it = Convert.ToInt32(Console.ReadLine());

            int sum = 0, min = int.MaxValue;

            System.Diagnostics.Stopwatch sq = System.Diagnostics.Stopwatch.StartNew();
            string writeToFile = "Don't forget to modify the path from line 43 and line 50!";

            for(int i = 0; i < it; i++)
            {
                readFormFile(tsp);
                Trip trip = tsp.Evolution(1500,1500,200,100);
                writeToFile += "Total distance is: " + trip.totalDistanceBetweenCities() + "\n";

                sum += trip.totalDistanceBetweenCities();
                if (trip.totalDistanceBetweenCities() < min)
                {
                    min = trip.totalDistanceBetweenCities();
                }
            }
            sq.Stop();

            writeToFile += "\n";
            writeToFile+= "Total elapsed time = " + sq.Elapsed.TotalSeconds + "\n";
            writeToFile+= "The smallest diststance is: " + min + "\n";
            writeToFile+= "The average diststance is: " + sum / it + "\n";

            Console.WriteLine(writeToFile);
            System.IO.File.WriteAllText(@"C:\Users\iulian.popovici\Desktop\Artificial Intelligence\AI Lab 4 - Popovici Iulian\AI Lab 4\results.txt", writeToFile);

            Console.ReadLine();
        }

        private static void readFormFile(TSP tsp)
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\iulian.popovici\Desktop\Artificial Intelligence\AI Lab 4 - Popovici Iulian\AI Lab 4\input.txt");
            int i = 0;
            string line = lines.ElementAt(i);
            tsp.clearTrip();

            while (line != "EOF")
            {
                string[] numbers = line.Split(' ');
                tsp.addCity(new City(Convert.ToInt32(numbers[0]), Convert.ToInt32(numbers[1]), Convert.ToInt32(numbers[2])));
                line = lines[++i];
            }
        }

    }
}
