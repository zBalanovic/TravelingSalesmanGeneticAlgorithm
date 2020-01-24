using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TravelingSalesmanGeneticAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            double x, y;
            string answer;
            LandmarkDatabase db = new LandmarkDatabase();
            db.ReadFromFile();
        again:
            Console.WriteLine("Enter Number of Paths: ");
            int nmbrOfPaths = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter Mutation Rate : ");
            double mutationRate = Double.Parse(Console.ReadLine());
            Console.WriteLine("Enter Number of Generations: ");
            int nmbrOfGenerations = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter start coordinates:");
            x = Double.Parse(Console.ReadLine());
            y = Double.Parse(Console.ReadLine());
            Landmark start = new Landmark();
            start.Name = "Agency";
            start.Coordinates.X = x;
            start.Coordinates.Y = y;
            db.Database.Insert(0, start);
            Population population = new Population(nmbrOfPaths, mutationRate, nmbrOfGenerations, db.Database);
            population.Evolve();
            Console.WriteLine("To evaluate again pres Y");
            answer = Console.ReadLine();
            if(answer[0].ToString().ToLower() == "y")
            {
                db.Database.RemoveAt(0);
                goto again;
            }
            return;
        }
    }
}
