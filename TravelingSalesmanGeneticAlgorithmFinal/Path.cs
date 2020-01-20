using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesmanGeneticAlgorithm
{
    class Path
    {
        public GraphMatrix Graph { get; set; }
        public int[] OrderOfTraverse { get; set; }
        public int NmbrOfCities { get; set; }
        public double Fitness { get; set; }
        //public int probabilityOfSuccess { get; set; } //in percentage

        public Path(int nmbrOfCities, GraphMatrix graph)
        {
            this.OrderOfTraverse = new int[nmbrOfCities];
            for (int i = 0; i < nmbrOfCities; i++)
            {
                this.OrderOfTraverse[i] = -1;
            }
            this.NmbrOfCities = nmbrOfCities;
            this.Graph = graph;
        }

        public void InitializePath()
        {
            OrderOfTraverse[0] = 0;
            bool isDone = false;

            Random r = new Random((int)DateTime.Now.Ticks);
            int nextCity = 1;
            while (!isDone)
            {
                int index = r.Next(1, NmbrOfCities);

                if (this.OrderOfTraverse[index] != -1)
                {
                    continue;
                }
                this.OrderOfTraverse[index] = nextCity;
                nextCity++;
                if (nextCity == this.NmbrOfCities)
                {
                    isDone = true;
                }
            }
        }

        public void EvalFitness()
        {
            int totalCost = 0;
            for (int i = 0; i < this.NmbrOfCities - 1; i++)
            {
                int x = this.OrderOfTraverse[i];
                int y = this.OrderOfTraverse[i + 1];
                totalCost += this.Graph.Matrix[x][y];
            }
            int xx = this.OrderOfTraverse[this.NmbrOfCities - 1];
            totalCost += this.Graph.Matrix[xx][0];
            if (totalCost == 0)
            {
                throw new System.InvalidOperationException("TotalCost cannot be 0");
            }
            this.Fitness = 1 / (double)totalCost;
        }

        bool Exists(int element)
        {
            for (int i = 0; i < this.NmbrOfCities; i++)
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
                int i = r.Next(1, NmbrOfCities - 1);
                int j = r.Next(1, NmbrOfCities - 1);
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
            int crosspoint1 = r.Next(1, this.NmbrOfCities / 2);
            int crosspoint2 = r.Next(crosspoint1, this.NmbrOfCities);
            Path offspring1 = new Path(this.NmbrOfCities, this.Graph);
            Path offspring2 = new Path(this.NmbrOfCities, this.Graph);
            offsprings[0] = offspring1;
            offsprings[1] = offspring2;
            for (int i = 0; i < crosspoint1; i++)
            {
                offspring1.OrderOfTraverse[i] = this.OrderOfTraverse[i];
                offspring2.OrderOfTraverse[i] = parent2.OrderOfTraverse[i];
            }
            for (int i = crosspoint2; i < NmbrOfCities; i++)
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
                        for (int j = crosspoint2; j < NmbrOfCities; j++)
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
                        for (int j = crosspoint2; j < NmbrOfCities; j++)
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
            for (int i = 0; i < this.NmbrOfCities; i++)
            {
                Console.Write(this.OrderOfTraverse[i] + "->");
            }
        }
    }
}