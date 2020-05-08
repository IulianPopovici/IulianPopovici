using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Lab_4
{ 
    public class TSP
    {
        private List<Trip> P;
        private List<Trip> PX;
        private List<Trip> PM;
        private List<Trip> parents;
        private List<City> roads;

        public TSP()
        {
            P = new List<Trip>();
            PX = new List<Trip>();
            PM = new List<Trip>();
            parents = new List<Trip>();
            roads = new List<City>();
        }

        /// <summary>
        /// Adds a new city.
        /// </summary>
        /// <param name="city"></param>
        public void addCity(City city)
        {
            roads.Add(city);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param> Number of individuals from a population.
        /// <param name="m"></param> Number of total generations.
        /// <param name="k"></param> Number of individuals that is choosen for the selection of parents.
        /// <param name="mm"></param> Total number of parents.
        /// <returns></returns>
        public Trip Evolution(int n, int m, int k, int mm)
        {
            P.Clear();

            getRandomTrips(n);

            for (int t = 0; t < m; t++)
            {
                parents.Clear();
                PX.Clear();
                PM.Clear();
                parents = TournamentSelection(k, mm);

                for (int i = 0; i < mm; i += 2)
                {
                    OX(parents.ElementAt(i), parents.ElementAt(i + 1));
                }

                for (int i = 0; i < mm - 1; i++)
                {
                    Swap(PX.ElementAt(i));
                }

                P = P.OrderByDescending(x => x.totalDistanceBetweenCities()).ToList();
                PX = PX.OrderByDescending(x => x.totalDistanceBetweenCities()).ToList();
                PM = PM.OrderByDescending(x => x.totalDistanceBetweenCities()).ToList();

                int a = 0;

                for (int i = PX.Count - 1; i >= 0; i--)
                {
                    if (PX.ElementAt(i).totalDistanceBetweenCities() < P.ElementAt(a).totalDistanceBetweenCities())
                    {
                        P[a] = PX.ElementAt(i);
                        a++;
                    }
                }

                a = 0;
                P = P.OrderByDescending(x => x.totalDistanceBetweenCities()).ToList();

                for (int i = PM.Count - 1; i >= 0; i--)
                {
                    if (PM.ElementAt(i).totalDistanceBetweenCities() < P.ElementAt(i).totalDistanceBetweenCities())
                    {
                        P[a] = PM.ElementAt(i);
                        a++;
                    }
                }
            }

            Trip min = P.ElementAt(0);
            for (int i = 0; i < P.Count; i++)
            {
                if (P.ElementAt(i).totalDistanceBetweenCities() < min.totalDistanceBetweenCities())
                {
                    min = P.ElementAt(i);
                }
            }

            return min;
        }

        /// <summary>
        /// This function will mutate a Trip using: Swap
        /// </summary>
        /// <param name="t"></param>
        public void Swap(Trip t)
        {
            Random rand = new Random();

            int k = rand.Next(t.cities.Count - 2);
            int l = rand.Next(k, t.cities.Count - 1);

            t.swapTwoCity(k, l);
            PM.Add(t);
        }

        /// <summary>
        /// Mix 2 parents and creates 2 new ofsprings using: ORDER CROSSOVER
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        public void OX(Trip t1, Trip t2)
        {
            Random rand = new Random();

            int k = rand.Next(t1.cities.Count - 2);
            int l = rand.Next(k, t1.cities.Count - 1);

            List<City> o1 = new List<City>();
            List<City> o2 = new List<City>();

            foreach (var el in t1.cities)
            {
                o1.Add(new City(0, 0, 0));
                o2.Add(new City(0, 0, 0));
            }

            for (int i = k; i < l; i++)
            {
                o1[i] = t1.cities.ElementAt(i);
                o2[i] = t2.cities.ElementAt(i);
            }

            int a = 0;

            for (int i = 0; i < t2.cities.Count; i++)
            {
                if (a == k)
                {
                    a = l;
                }

                if (!o1.Contains(t2.cities.ElementAt(i)))
                {
                    o1[a] = t2.cities.ElementAt(i);
                    a++;
                }
            }

            a = 0;

            for (int i = 0; i < t1.cities.Count; ++i)
            {
                if (a == k)
                {
                    a = l;
                }

                if (!o2.Contains(t1.cities.ElementAt(i)))
                {
                    o2[a] = t1.cities.ElementAt(i);
                    a++;
                }
            }

            PX.Add(new Trip(o1));
            PX.Add(new Trip(o2));
        }

        /// <summary>
        /// Tournament selection for the parents.
        /// </summary>
        /// <param name="k"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<Trip> TournamentSelection(int k, int n)
        {
            Random rand = new Random();
            parents.Clear();

            Trip minimElement = new Trip();

            for(int i = 1; i <= n; i++)
            {
                int val = int.MaxValue;

                for(int j = 1; j <= k; j++)
                {
                    int r = rand.Next(0, P.Count-1);
                    Trip randomElement = P[r];
                    if(randomElement.totalDistanceBetweenCities() < val)
                    {
                        val = randomElement.totalDistanceBetweenCities();
                        minimElement = randomElement;
                    }

                }
                parents.Add(minimElement);
            }
            return parents;
        }

        /// <summary>
        /// Generates random trips for the initial population.
        /// </summary>
        /// <param name="n"></param>
        public void getRandomTrips(int n)
        {
            for (int i = 0; i < n; i++)
            {
                List<City> solution = new List<City>();

                for (int j = 0; j < roads.Count; j++)
                {
                    solution.Add(roads.ElementAt(j));
                }

                Trip mainTrip = new Trip(solution);
                mainTrip.Shuffle();

                P.Add(mainTrip);
            }
        }

        /// <summary>
        /// Clear the cities from a Trip.
        /// </summary>
        public void clearTrip()
        {
            roads.Clear();
        }


    }
}
