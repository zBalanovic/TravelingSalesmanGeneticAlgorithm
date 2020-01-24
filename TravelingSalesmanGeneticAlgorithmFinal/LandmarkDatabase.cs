using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TravelingSalesmanGeneticAlgorithm
{
    class LandmarkDatabase
    {
        public List<Landmark> Database { get; set; }

        public LandmarkDatabase()
        {
            this.Database = new List<Landmark>();
        }
        public void WriteToFile()
        {
            try
            {
                var path = Environment.CurrentDirectory + "\\LandmarkDB.txt";
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.WriteLine(this.Database.Count());
                    for (int i = 0; i < Database.Count(); i++)
                    {
                        writer.WriteLine(Database[i].Name);
                        writer.WriteLine(Database[i].Coordinates.X);
                        writer.WriteLine(Database[i].Coordinates.Y);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        public void ReadFromFile()
        {
            try
            {
                var path = Environment.CurrentDirectory + "\\LandmarkDB.txt";
                using (StreamReader reader = new StreamReader(path))
                {
                    int nmbrOfLandmarks = Int32.Parse(reader.ReadLine());
                    for (int i = 0; i < nmbrOfLandmarks; i++)
                    {
                        Landmark tmp = new Landmark();
                        tmp.Name = reader.ReadLine();
                        tmp.Coordinates.X = Double.Parse(reader.ReadLine());
                        tmp.Coordinates.Y = Double.Parse(reader.ReadLine());
                        this.Database.Add(tmp);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}
