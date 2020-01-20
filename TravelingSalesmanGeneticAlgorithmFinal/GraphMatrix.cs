using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesmanGeneticAlgorithm
{
    class GraphMatrix
    {
        public int[][] Matrix { get; set; }
        public int n { get; set; }
        public GraphMatrix(int n)
        {
            this.n = n;
            Matrix = new int[n][];
            for (int i = 0; i < n; i++)
            {
                Matrix[i] = new int[n];
            }
        }

        public void InitializeState()
        {
            Random r = new Random();
            for (int i = 0; i < this.n; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    if (i == j)
                    {
                        this.Matrix[i][j] = 0;
                    }
                    else
                    {
                        this.Matrix[i][j] = r.Next(1, 10);
                        this.Matrix[j][i] = this.Matrix[i][j];
                    }
                }
            }
        }

        public void Print()
        {
            for (int i = 0; i < this.n; i++)
            {
                for (int j = 0; j < this.n; j++)
                {
                    Console.Write(Matrix[i][j] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}