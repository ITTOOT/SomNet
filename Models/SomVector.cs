using SomNet.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using System.Linq.Expressions;

namespace SomNet.Models
{
    //SomVector - input vector, to be compared against map/matrix vector
    public class SomVector : List<double>, ISomVector
    {
        //Name for this vector - Used to identify inputs allocated to map locations
        public int Id { get; set; }

        //Name for this vector - Used to identify inputs allocated to map locations
        public string Label { get; set; }

        //Timestamp for the vector
        public DateTime Timestamp { get; set; }

        //Calculate the distance between the vector elements from this neuron (input) and the map neuron
        public double DistanceBetweenPoints(ISomVector mapVector, string calc)
        {
            //If the input list count and the map vector are different sizes, throw an exception!
            if (mapVector.Count != Count)
                throw new ArgumentException("Input list and map vector list are not the same size!");

            double result = 0;

            //Output the distance (x) between the input list elements and the map vector elements
            switch (calc)
            {
                case "euc":
                    //Euclidian 
                    result = this.Select(x => Math.Pow(x - mapVector[this.IndexOf(x)], 2)).Sum();
                    break;
                case "man":
                    //Manhatten

                    break;
                default:
                    // code block
                    result = 0;
                    break;
            }
            return result;
        }
    }
}
