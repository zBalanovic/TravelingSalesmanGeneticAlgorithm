using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesmanGeneticAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Population population = new Population(30, 40, 0.4, 100);
            //population.GraphOfCities.Print();
            while (population.Evolve()) ;
        }
    }
}
