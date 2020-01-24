using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesmanGeneticAlgorithm
{

    class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public double GetDistanceTo(Point p)
        {
            double tmpX = Math.Pow((this.X - p.X), 2);
            double tmpY = Math.Pow((this.Y - p.Y), 2);
            return Math.Sqrt(tmpX + tmpY);
        }
    }

    class Landmark
    {
        public string Name { get; set; }
        public Point Coordinates { get; set; }

        public Landmark()
        {
            this.Coordinates = new Point();
            this.Name = "";
        }
        
        public double GetDistanceTo(Landmark l)
        {
            return this.Coordinates.GetDistanceTo(l.Coordinates);
        }
    }
}
