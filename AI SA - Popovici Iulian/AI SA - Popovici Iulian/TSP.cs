using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SA___Popovici_Iulian
{
    public class TSP
    {
        private double temp = 100000;
        private double coolingRate = 0.003;

        public void Run()
        {
            Console.WriteLine("in the project folder, you can find an results.txt file that will contain more detailed results for this run");
            Console.WriteLine("Please don't forget to modify the string from line 94 and line 111");

            List<City> readedCities = readCitiesFromFile();

            logic(readedCities);

            Console.ReadLine();
        }

        private void logic(List<City> readedCities)
        {
            Trip trip = new Trip(readedCities);
            trip.Shuffle();
            Trip bestTrip = new Trip(trip.cities);

            int max = 999999999, sum = 0;
            string resultsToWriteToFile = "";

            Console.WriteLine("First random solution has the distance: " + trip.totalDistanceBetweenCities().ToString());
            Random rand = new Random();

            Console.WriteLine("Please enter the number of iterations. (recommandation: 100) :");
            int maxIterations = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < maxIterations; i++)
            {
                temp = 100000;
                while (temp > 1)
                {
                    Trip newTrip = new Trip(trip.cities);

                    int firstTripIndex = rand.Next(0, newTrip.cities.Count);
                    int secoundTripIndex;
                    do
                    {
                        secoundTripIndex = rand.Next(0, newTrip.cities.Count);
                    } while (firstTripIndex == secoundTripIndex);

                    newTrip.swapTwoCity(firstTripIndex, secoundTripIndex);

                    int currentDistance = newTrip.totalDistanceBetweenCities();
                    int oldDistance = trip.totalDistanceBetweenCities();

                    double randomDouble = (rand.Next(1000) / 1000.0);

                    if (acceptanceProbability(currentDistance, oldDistance, temp) > randomDouble)
                    {
                        trip = new Trip(newTrip.cities);
                    }

                    temp *= 1 - coolingRate;
                }
                if (max > trip.totalDistanceBetweenCities())
                {
                    max = trip.totalDistanceBetweenCities();
                }
                sum = sum + trip.totalDistanceBetweenCities();

                Console.WriteLine("This solution has the distance: " + trip.totalDistanceBetweenCities());

                resultsToWriteToFile += "This solution has the distance: " + trip.totalDistanceBetweenCities().ToString()+"\n";
                foreach (var el in trip.cities)
                {
                    resultsToWriteToFile += el.Id + " " + el.X + " " + el.Y + "\n";
                }
            }
            resultsToWriteToFile += "Best solution had the distance " + max.ToString() + "\n"+
            "Average distantance was " + (Convert.ToDouble(sum) / Convert.ToDouble(maxIterations)).ToString() + "\n";

            Console.WriteLine("Best solution had the distance " + max);
            Console.WriteLine("Average distantance was " + (Convert.ToDouble(sum) / Convert.ToDouble(maxIterations)).ToString());

            Console.WriteLine("in the project folder, you can find an results.txt file that will contain more detailed results for this run");
            Console.WriteLine("Please don't forget to modify the string from line 94 and line 111");

            System.IO.File.WriteAllText(@"C:\Users\iulian.popovici\Desktop\AI SA - Popovici Iulian\AI SA - Popovici Iulian\results.txt",
               resultsToWriteToFile);
        }

        public double acceptanceProbability(int currentDistance, int newDistance, double temperature)
        {
            if (newDistance < currentDistance)
            {
                return 1.0;
            }
            return Math.Exp((currentDistance - newDistance) / temperature);
        }


        private List<City> readCitiesFromFile()
        {
            List<City> fileCities = new List<City>();
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\iulian.popovici\Desktop\AI SA - Popovici Iulian\AI SA - Popovici Iulian\input.txt");
            int i = 0;
            string line = lines.ElementAt(i);

            while (line != "EOF")
            {
                string[] numbers = line.Split(' ');
                fileCities.Add(new City(Convert.ToInt32(numbers[0]), Convert.ToInt32(numbers[1]), Convert.ToInt32(numbers[2])));
                line = lines[++i];
            }
            return fileCities;
        }
    }
}
