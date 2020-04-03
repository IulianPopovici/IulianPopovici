using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_SA___Popovici_Iulian
{
    public class Trip
    {
        public List<City> cities { get; private set; }
        private int distance;

        private Random rng = new Random();

        public Trip()
        {
            cities = new List<City>();
            distance = 0;
        }

        public Trip(List<City> oldTrip)
        {
            cities = oldTrip;
            distance = 0;
        }

        public void swapTwoCity(int indexA, int indexB)
        {
            City tmp = cities[indexA];
            cities[indexA] = cities[indexB];
            cities[indexB] = tmp;
        }

        public void Shuffle()
        {
            int n = cities.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                City value = cities[k];
                cities[k] = cities[n];
                cities[n] = value;
            }
        }

        public int totalDistanceBetweenCities()
        {
            if (distance == 0)
            {
                int tripDistance = 0;
                for (int i = 0; i < cities.Count; i++)
                {
                    City fromCity = cities.ElementAt(i); ;
                    City toCity;
                    if (i + 1 < cities.Count)
                    {
                        toCity = cities.ElementAt(i + 1);
                    }
                    else
                    {
                        toCity = cities.ElementAt(0); ;
                    }
                    tripDistance += CalculateDistranceBetween2Cities(fromCity, toCity);
                }
                distance = tripDistance;
            }
            return distance;
        }

        public int CalculateDistranceBetween2Cities(City city1, City city2)
        {
            int newX = Math.Abs(city1.X - city2.X);
            int newY = Math.Abs(city1.Y - city2.Y);

            return Convert.ToInt32(Math.Sqrt((newX * newX) + (newY * newY)));
        }
    }
}
