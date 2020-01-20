using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesmanGeneticAlgorithm
{

    class BestPath
    {
        public Path Path { get; set; }
        public int NmbrOfGeneration { get; set; }

    }

    class Population
    {
        public GraphMatrix GraphOfCities { get; set; }
        public BestPath bestPath { get; set; }
        public int NmbrOfGenerations { get; set; }
        public int CurrentGeneration { get; set; }
        public double MutationRate { get; set; }
        public int NmbrOfPaths { get; set; }
        public int NmbrOfCities { get; set; }
        public List<Path> Paths { get; set; }

        public List<Path> MatingPool { get; set; }
        public Population(int nmbrOfPaths, int nmbrOfCities, double mutationRate, int nmbrOfGenerations)
        {
            this.GraphOfCities = new GraphMatrix(nmbrOfCities);
            this.GraphOfCities.InitializeState();
            this.NmbrOfCities = nmbrOfCities;
            this.NmbrOfPaths = nmbrOfPaths;
            this.MutationRate = mutationRate;
            this.NmbrOfGenerations = nmbrOfGenerations;
            this.CurrentGeneration = 1;
            this.bestPath = new BestPath();
            Paths = new List<Path>(nmbrOfPaths);
            this.MatingPool = new List<Path>();
            this.InitializePaths();
        }
        void InitializePaths()
        {
            for (int i = 0; i < this.NmbrOfPaths; i++)
            {
                Path tmp = new Path(this.NmbrOfCities, this.GraphOfCities);
                tmp.InitializePath();
                Paths.Add(tmp);
            }
        }

        public void EvalPathsFitness()
        {
            double max = this.bestPath.Path != null ? this.bestPath.Path.Fitness : 0;
            for (int i = 0; i < this.NmbrOfPaths; ++i)
            {
                this.Paths[i].EvalFitness();
                if (max < this.Paths[i].Fitness)
                {
                    this.bestPath.Path = this.Paths[i];
                    this.bestPath.NmbrOfGeneration = this.CurrentGeneration;
                }
            }
        }

        public void CreateMatingPool()
        {
            double sumOfFitness = 0;
            foreach (Path path in this.Paths)
            {
                sumOfFitness += path.Fitness;
            }
            int probabilityOfSuccess;
            foreach (Path path in Paths)
            {
                probabilityOfSuccess = (int)Math.Ceiling((path.Fitness / sumOfFitness) * 100);
                for (int i = 0; i < probabilityOfSuccess; i++)
                {
                    MatingPool.Add(path);
                }
            }
        }

        public void Reproduction()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            for (int i = 1; i < this.NmbrOfPaths - 1; i += 2)
            {
                int x = r.Next(0, this.MatingPool.Count());
                int y = r.Next(0, this.MatingPool.Count());
                Path[] children = this.MatingPool[x].Crossover(this.MatingPool[y]);
                Path firstChild = children[0].Mutate(this.MutationRate);
                Path secondChild = children[1].Mutate(this.MutationRate);
                this.Paths[i] = firstChild;
                this.Paths[i + 1] = secondChild;
            }
            this.Paths[0] = bestPath.Path;
            this.CurrentGeneration++;
        }

        public bool Evolve()
        {
            this.EvalPathsFitness();
            this.CreateMatingPool();
            this.Reproduction();
            this.PrintBestPath();
            if (this.CurrentGeneration > this.NmbrOfGenerations)
            {
                return false;
            }
            return true;
        }
        public void PrintBestPath()
        {
            Console.WriteLine("Best path: " + bestPath.Path.Fitness);
            this.bestPath.Path.PrintPath();
            Console.WriteLine("Generation: " + this.bestPath.NmbrOfGeneration);
        }
    }
}
