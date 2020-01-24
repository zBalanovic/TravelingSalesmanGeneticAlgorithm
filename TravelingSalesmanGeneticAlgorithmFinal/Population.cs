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
        public BestPath bestPath { get; set; }
        public int NmbrOfGenerations { get; set; }
        public int CurrentGeneration { get; set; }
        public double MutationRate { get; set; }
        public int NmbrOfPaths { get; set; }
        public List<Path> Paths { get; set; }
        public List<Path> MatingPool { get; set; }

        public Population(int nmbrOfPaths, double mutationRate, int nmbrOfGenerations, List<Landmark> landmarks)
        {
            this.NmbrOfPaths = nmbrOfPaths;
            this.MutationRate = mutationRate;
            this.NmbrOfGenerations = nmbrOfGenerations;
            this.CurrentGeneration = 1;
            this.bestPath = new BestPath();
            Paths = new List<Path>(nmbrOfPaths);
            this.MatingPool = new List<Path>();
            this.InitializePaths(landmarks, landmarks.Count());
        } 
        void InitializePaths(List<Landmark> landmarks, int nmbrOfLandmarks)
        {
            for (int i = 0; i < this.NmbrOfPaths; i++)
            {
                Path tmp = new Path(nmbrOfLandmarks, landmarks);
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
        public void Evolve()
        {
            while (this.CurrentGeneration <= this.NmbrOfGenerations)
            {
                this.EvalPathsFitness();
                this.CreateMatingPool();
                this.Reproduction();
                this.PrintBestPath();
            }

        }
        public void PrintBestPath()
        {
            Console.WriteLine("Best path: " + bestPath.Path.Fitness);
            this.bestPath.Path.PrintPath();
            Console.WriteLine("Generation: " + this.bestPath.NmbrOfGeneration);
        }
    }
}
