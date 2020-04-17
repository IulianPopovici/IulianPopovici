using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3_AI___Popovici_Iulian
{
    public class Logic
    {
        private List<Values> values = new List<Values>();
        private int weight;
        private int noOfIndexes;

        public void Run()
        {
            Read();

            Random rand = new Random();
            Console.WriteLine("Enter the number of generations= ");
            int maxGeneration = Convert.ToInt32(Console.ReadLine());

            List<Element> FinalResults = new List<Element>();

            for (int j = 0; j < 10; j++)
            {

                List<Element> Population = new List<Element>();

                // Initialize the Population list with Elements (contains solutions and fitness).
                for (int i = 0; i < 400; i++)
                {
                    string sol = GenerateRandomPopulation(rand);
                    Population.Add(new Element(sol, calculateCost(sol)));
                }

                int currentGeneration = 0;
                while (currentGeneration < maxGeneration)
                {
                    List<Element> Parents = TournamentSelectionParents(Population, rand); // Select the parents.

                    List<Element> CrossedOverParents = CrossParents(Parents, rand); // Cross over the parents.
                    List<Element> MutatedPopulation = MutationOfCrossedParent(CrossedOverParents, 0.8, rand);

                    Population = SelectSurvivors(Parents, CrossedOverParents, MutatedPopulation);

                    currentGeneration++;
                }

                List<Element> FinalPopulation = Population.OrderBy(x => x.Fitness).ToList();
                FinalResults.Add(FinalPopulation.ElementAt(FinalPopulation.Count - 1));
            }
            string file = string.Empty;
            int sum = 0;
            int best = 0;
            foreach(var result in FinalResults)
            {
                Console.WriteLine("The final solution is: " + result.Fitness.ToString());
                file += "The final solution is: " + result.Fitness.ToString() + " - " + result.Solution+ "\n";
                sum += result.Fitness;
                if(result.Fitness > best)
                {
                    best = result.Fitness;
                }
            }
            file += "Best solution is: " + best.ToString() + "\n";
            file += "Average solution is: " + sum / 10 + "\n";

            System.IO.File.WriteAllText(@"C:\Users\iulian.popovici\Desktop\Artificial Intelligence\Lab 3 AI - Popovici Iulian\Lab 3 AI - Popovici Iulian\results.txt",
                file);

            Console.WriteLine("Best solution is: " + best.ToString());
            Console.WriteLine("Average solution is: " + sum / 10);
            Console.WriteLine("Look into results.txt for detailed results!");
            Console.ReadLine();
        }

        private List<Element> SelectSurvivors(List<Element> Parents, List<Element> CrossedOverParents, List<Element> MutatedPopulation)
        {
            // Sort the lists by their fitness.
            List<Element> SortedParents = Parents.OrderBy(x => x.Fitness).ToList();
            List<Element> SortedCrossedOverParents = CrossedOverParents.OrderBy(x => x.Fitness).ToList();
            List<Element> SortedMutatedPopulation = MutatedPopulation.OrderBy(x => x.Fitness).ToList();

            // Replace in population all those elements that have the fitness smaller than the fitness of the childs.
            int a = 0;
            for (int i = SortedCrossedOverParents.Count - 1; i >= 0; i--)
            {
                if (SortedCrossedOverParents.ElementAt(i).Fitness > SortedParents.ElementAt(a).Fitness)
                {
                    SortedParents[a] = SortedCrossedOverParents.ElementAt(i);
                    a++;
                }
                else
                {
                    break;
                }
            }

            // Replace in the population all those elements that have the fitness smaller than the fitness of the mutated childs.
            a = 0;
            SortedParents = SortedParents.OrderBy(x => x.Fitness).ToList();
            for (int i = MutatedPopulation.Count - 1; i >= 0; i--)
            {
                if (MutatedPopulation.ElementAt(i).Fitness> SortedParents.ElementAt(a).Fitness)
                {
                    SortedParents[a]=  MutatedPopulation.ElementAt(i);
                    a++;
                }
                else
                {
                    break;
                }
            }

            return SortedParents;
        }

        /// <summary>
        /// This function implements the STRONG MUTATION on every crossed parent, but also checks for mistakes and resolves them.
        /// </summary>
        /// <param name="CrossedOverParents"></param>
        /// <param name="Pm"></param>
        /// <param name="rand"></param>
        /// <returns></returns>
        private List<Element> MutationOfCrossedParent(List<Element> CrossedOverParents, double Pm, Random rand)
        {
            foreach(var el in CrossedOverParents)
            {
                string mutatedVal = "";
                foreach(var s in el.Solution)
                {
                    double q = rand.NextDouble();

                    if (q < Pm)
                    {
                        // mutatedVal += rand.Next(0, 2); For weak mutation
                        if (s == '1')
                        {
                            mutatedVal += "0";
                        }
                        else
                        {
                            mutatedVal+="1";
                        }
                    }
                    else
                    {
                        mutatedVal += s;
                    }
                }

                if(calculateWeight(mutatedVal) >= weight)
                {
                    mutatedVal = ResolveMistake(mutatedVal, rand);
                }
                el.Solution = mutatedVal;
                el.Fitness = calculateCost(mutatedVal);
            }

            return CrossedOverParents;
        }

        /// <summary>
        /// This function helps to cross parents with single cut method.
        /// </summary>
        /// <param name="Parents"></param>
        /// <param name="rand"></param>
        /// <returns></returns> The new list of crossed elements.
        private List<Element> CrossParents(List<Element> Parents, Random rand)
        {
            List<Element> crossedOverParents = new List<Element>();
            for(int i = 0; i < Parents.Count; i += 2)
            {
                string p1 = Parents.ElementAt(i).Solution;
                string p2 = Parents.ElementAt(i+1).Solution;

                int sectionIndex = rand.Next(1, p1.Length - 1);

                string c1 = p1.Substring(0, sectionIndex) + p2.Substring(sectionIndex);
                string c2 = p2.Substring(0, sectionIndex) + p1.Substring(sectionIndex);

                if (calculateWeight(c1) >= weight)
                {
                    c1 = ResolveMistake(c1, rand);
                }
                if (calculateWeight(c2) >= weight)
                {
                    c2 = ResolveMistake(c2, rand);
                }

                crossedOverParents.Add(new Element(c1, calculateCost(c1)));
                crossedOverParents.Add(new Element(c2, calculateCost(c2)));

            }
            return crossedOverParents;
        }

        /// <summary>
        /// This function corrects an Element if it's total weight it is grater than the maximum weight.
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="rand"></param>
        /// <returns></returns>
        private string ResolveMistake(string elem, Random rand)
        {
            while (calculateWeight(elem) >= weight)
            {
                int randomPosition = rand.Next(1, elem.Length - 1);
                elem = elem.Substring(0, randomPosition) + "0" + elem.Substring(randomPosition + 1);
            }
            return elem;
        }

        /// <summary>
        /// This function use tournament method to generate a list of parents from an population.
        /// </summary>
        /// <param name="Population"></param>
        /// <param name="rand"></param>
        /// <returns></returns>
        private List<Element> TournamentSelectionParents(List<Element> Population, Random rand)
        {
            List<Element> Parents = new List<Element>();
            Element maxElem = new Element();
            for (int i = 1; i <= 100; i++)
            {
                int value = 0;
                for (int j = 1; j <= 5; j++)
                {
                    Element randomElement = Population.ElementAt(rand.Next(Population.Count));
                    if (randomElement.Fitness > value)
                    {
                        value = randomElement.Fitness;
                        maxElem = randomElement;
                    }

                }
                Parents.Add(maxElem);
            }
            return Parents;
        }

        /// <summary>
        /// This function will generate random parents that will be valid solutions.
        /// </summary>
        /// <param name="rand"></param> Random object
        /// <returns></returns> The solution as a string
        private string GenerateRandomPopulation(Random rand)
        {
            string solution = "";
            do
            {
                solution = string.Empty;
                for (int j = 0; j < noOfIndexes; j++)
                {
                    solution = solution + rand.Next(0, 2).ToString();
                }
            } while (calculateWeight(solution) > weight);
            return solution;
        }

        /// <summary>
        /// This function calculate the cost (fitness) of an solution.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private int calculateCost(string s)
        {
            int cost = 0;
            for (int k = 1; k <= noOfIndexes; k++)
            {
                if (s[k - 1] == '1')
                {
                    var el = values.Single(x => x.index == k);
                    cost = cost + el.cost;
                }
            }
            return cost;
        }

        /// <summary>
        /// This function will calculate the total weight of an solution.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private int calculateWeight(string s)
        {
            int cost = 0;
            for (int k = 1; k <= noOfIndexes; k++)
            {
                if (s[k - 1] == '1')
                {
                    var el = values.Single(x => x.index == k);
                    cost = cost + el.weight;
                }
            }
            return cost;
        }

        /// <summary>
        /// This function reads the data from file.
        /// </summary>
        private void Read()
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\iulian.popovici\Desktop\Artificial Intelligence\Lab 3 AI - Popovici Iulian\Lab 3 AI - Popovici Iulian\input.txt");
            noOfIndexes = Convert.ToInt32(lines[0]);
            for (int i = 1; i <= noOfIndexes; i++)
            {
                string[] numbers = lines[i].Split(' ');
                values.Add(new Values(Convert.ToInt32(numbers[0]), Convert.ToInt32(numbers[1]), Convert.ToInt32(numbers[2])));
            }
            weight = Convert.ToInt32(lines[noOfIndexes + 1]);
        }
    }
}
