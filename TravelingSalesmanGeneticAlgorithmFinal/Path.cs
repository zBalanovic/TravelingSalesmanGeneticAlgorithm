using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesmanGeneticAlgorithm
{
    class Path
    {
        public List<Landmark> Landmarks { get; set; }
        public int[] OrderOfTraverse { get; set; }
        public int NmbrOfLandmarks { get; set; }
        public double Fitness { get; set; }

        public Path(int nmbrOfLandmarks, List<Landmark> landmarks)
        {
            this.OrderOfTraverse = new int[nmbrOfLandmarks];
            for (int i = 0; i < nmbrOfLandmarks; i++)
            {
                this.OrderOfTraverse[i] = -1;
            }
            this.NmbrOfLandmarks = nmbrOfLandmarks;
            this.Landmarks = landmarks;
        }

        public void InitializePath()
        {
            OrderOfTraverse[0] = 0;
            bool isDone = false;

            Random r = new Random((int)DateTime.Now.Ticks);
            int nextCity = 1;
            while (!isDone)
            {
                int index = r.Next(1, this.NmbrOfLandmarks);

                if (this.OrderOfTraverse[index] != -1)
                {
                    continue;
                }
                this.OrderOfTraverse[index] = nextCity;
                nextCity++;
                if (nextCity == this.NmbrOfLandmarks)
                {
                    isDone = true;
                }
            }
        }

        public void EvalFitness()
        {
            double totalCost = 0.0;
            for (int i = 0; i < this.NmbrOfLandmarks - 1; i++)
            {
                int x = this.OrderOfTraverse[i];
                int y = this.OrderOfTraverse[i + 1];
                totalCost += this.Landmarks[x].GetDistanceTo(this.Landmarks[y]);
            }
            int xx = this.OrderOfTraverse[this.NmbrOfLandmarks - 1];
            totalCost += this.Landmarks[xx].GetDistanceTo(this.Landmarks[0]);
            if (totalCost == 0)
            {
                throw new System.InvalidOperationException("TotalCost cannot be 0");
            }
            this.Fitness = 1 / totalCost;
        }

        bool Exists(int element)
        {
            for (int i = 0; i < this.NmbrOfLandmarks; i++)
            {
                if (this.OrderOfTraverse[i] == element)
                {
                    return true;
                }
            }
            return false;
        }
        public Path Mutate(double mutationRate)
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            if (r.NextDouble() < mutationRate)
            {
                int i = r.Next(1, NmbrOfLandmarks - 1);
                int j = r.Next(1, NmbrOfLandmarks - 1);
                int tmp = this.OrderOfTraverse[i];
                this.OrderOfTraverse[i] = this.OrderOfTraverse[j];
                this.OrderOfTraverse[j] = tmp;
            }
            return this;
        }

        public Path[] Crossover(Path parent2)
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            Path[] offsprings = new Path[2];
            int crosspoint1 = r.Next(1, this.NmbrOfLandmarks / 2);
            int crosspoint2 = r.Next(crosspoint1, this.NmbrOfLandmarks);
            Path offspring1 = new Path(this.NmbrOfLandmarks, this.Landmarks);
            Path offspring2 = new Path(this.NmbrOfLandmarks, this.Landmarks);
            offsprings[0] = offspring1;
            offsprings[1] = offspring2;
            for (int i = 0; i < crosspoint1; i++)
            {
                offspring1.OrderOfTraverse[i] = this.OrderOfTraverse[i];
                offspring2.OrderOfTraverse[i] = parent2.OrderOfTraverse[i];
            }
            for (int i = crosspoint2; i < NmbrOfLandmarks; i++)
            {
                offspring1.OrderOfTraverse[i] = this.OrderOfTraverse[i];
                offspring2.OrderOfTraverse[i] = parent2.OrderOfTraverse[i];
            }
            for (int i = crosspoint1; i < crosspoint2; i++)
            {
                if (!offspring1.Exists(parent2.OrderOfTraverse[i]))
                {
                    offspring1.OrderOfTraverse[i] = parent2.OrderOfTraverse[i];
                }
                else
                {
                    bool isFound = false;
                    for (int j = 0; j < crosspoint1; j++)
                    {
                        if (!offspring1.Exists(parent2.OrderOfTraverse[j]))
                        {
                            isFound = true;
                            offspring1.OrderOfTraverse[i] = parent2.OrderOfTraverse[j];
                            break;
                        }
                    }
                    if (!isFound)
                    {
                        for (int j = crosspoint2; j < NmbrOfLandmarks; j++)
                        {
                            if (!offspring1.Exists(parent2.OrderOfTraverse[j]))
                            {
                                offspring1.OrderOfTraverse[i] = parent2.OrderOfTraverse[j];
                                break;
                            }
                        }
                    }
                }

                if (!offspring2.Exists(this.OrderOfTraverse[i]))
                {
                    offspring2.OrderOfTraverse[i] = this.OrderOfTraverse[i];
                }
                else
                {
                    bool isFound = false;
                    for (int j = 0; j < crosspoint1; j++)
                    {
                        if (!offspring2.Exists(this.OrderOfTraverse[j]))
                        {
                            isFound = true;
                            offspring2.OrderOfTraverse[i] = OrderOfTraverse[j];
                            break;
                        }
                    }
                    if (!isFound)
                    {
                        for (int j = crosspoint2; j < NmbrOfLandmarks; j++)
                        {
                            if (!offspring2.Exists(this.OrderOfTraverse[j]))
                            {
                                offspring2.OrderOfTraverse[i] = this.OrderOfTraverse[j];
                                break;
                            }
                        }
                    }
                }
            }
            return offsprings;
        }

        public void PrintPath()
        {
            double sumOfDistance = 0.0;
            for (int i = 0; i < this.NmbrOfLandmarks - 1; i++)
            {
                double distance = this.Landmarks[this.OrderOfTraverse[i]].GetDistanceTo(this.Landmarks[this.OrderOfTraverse[i + 1]]);
                sumOfDistance += distance;
                distance = Math.Round(distance, 2);
                Console.Write(this.Landmarks[this.OrderOfTraverse[i]].Name + "-[" + 
                    distance.ToString() + "]->");
            }
            double distance2 = this.Landmarks[this.OrderOfTraverse[this.NmbrOfLandmarks - 1]].GetDistanceTo(this.Landmarks[this.OrderOfTraverse[0]]);
            sumOfDistance += distance2;
            sumOfDistance = Math.Round(sumOfDistance, 3);
            Console.WriteLine(this.Landmarks[this.OrderOfTraverse[this.NmbrOfLandmarks - 1]].Name + "-[" +
                    distance2.ToString() + "]->" + this.Landmarks[this.OrderOfTraverse[0]].Name);
            Console.WriteLine("Length of path: " + sumOfDistance);
        }
    }
}
