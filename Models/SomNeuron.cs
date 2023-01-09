using SomNet.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;

namespace SomNet.Models
{
    internal class SomNeuron : ISomNeuron
    {
        public double x { get; set; }
        public double y { get; set; }
        public ISomVector Weights { get; }
        public string hexLabel { get; set; }

        //CONSTRUCTOR
        public SomNeuron(int numOfWeights)
        {
            var random = new Random();
            Weights = new SomVector();

            //Initialise weights to random values
            for (int i = 0; i < numOfWeights; i++)
            {
                Weights.Add(random.NextDouble());
            }
        }

        //Calculates the distance from that neuron to another neuron in the matrix.
        public double Distance(ISomNeuron neuron)
        {
            //Euclidean 
            //return Math.Pow((x - neuron.x), 2) + Math.Pow((y - neuron.y), 2);
            double[] X = { x, y };
            double[] Y = { neuron.x, neuron.y };
            return MathNet.Numerics.Distance.Euclidean(X, Y);
            //Manhatten
            //return Math.Abs(x - neuron.x) + Math.Abs(y - neuron.y);
            //return MathNet.Numerics.Distance.Manhattan(X, Y);
        }

        //Sets a value to a weight defined by the index.
        public void SetWeight(int index, double value)
        {
            if (index >= Weights.Count)
                throw new ArgumentException("Wrong index!");

            Weights[index] = value;
        }

        //Retrieves a value of a weight defined by the index.
        public double GetWeight(int index)
        {
            if (index >= Weights.Count)
                throw new ArgumentException("Wrong index!");

            return Weights[index];
        }

        //Updates weights of a neuron based on the input, learning rate and the distance decay.
        public void UpdateWeights(ISomVector input, double distanceDecay, double learningRate)
        {
            if (input.Count != Weights.Count)
                throw new ArgumentException("Wrong input!");

            //Update the weights on each connection
            //Formula - https://rubikscode.net/2018/08/20/introduction-to-self-organizing-maps/
            for (int i = 0; i < Weights.Count; i++)
            {
                Weights[i] += distanceDecay * learningRate * (input[i] - Weights[i]);
            }
        }
    }
}
