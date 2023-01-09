using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SomNet.Interfaces;
using SomNet.Models;
using MathNet.Numerics;
using System.Reflection;
using static FSharp.Stats.Distributions.ContinuousDistributionModule;
using static FSharp.Stats.ML.SurprisalAnalysis;
using static FSharpAux.IO.SchemaReader;
using AutoMapper;
using Plotly.NET.TraceObjects;
using SomNet.Utilities.Mappers;
using static FSharpAux.Graph;
using System.IO;
using MathNet.Numerics.Statistics;

namespace SomNet.ML
{
    public class SOMap
    {
        internal ISomNeuron[,] _matrix;
        internal List<ISomNeuron> _outputMatrix;
        internal List<Results> _outputSeries;
        internal List<SomVector> _vectorDetailsList;
        internal int _height;
        internal int _width;
        internal double _matrixRadius;
        internal double _numberOfIterations;
        internal double _timeConstant;
        internal double _learningRate;
        internal readonly IMapper _mapper;

        //CONSTRUCTOR
        //Self-organising Map
        public SOMap(int height, int width, int inputDimension,
            double numberOfIterations, double learningRate, SomVector[] inputSeries)
        {
            //Constant values for SOM
            _height = height;
            _width = width;
            _numberOfIterations = numberOfIterations;
            _learningRate = learningRate;

            //Derived constants
            _matrix = new ISomNeuron[_width, _height];
            _outputMatrix = new List<ISomNeuron>();
            _outputSeries = new List<Results>();
            _vectorDetailsList = new List<SomVector>();
            _matrixRadius = Math.Max(width, height) / 2;
            _timeConstant = _numberOfIterations / Math.Log(_matrixRadius);
            //_mapper = new IMapper;

            //Create the complete map x,y and initialise the weights for the vector elements 
            InitializeConnections(inputDimension);
        }

        //Train - 
        public void Train(SomVector[] input, bool hasHeader)
        {
            Console.WriteLine("Training of SOM started");
            int iteration = 0;
            var learningRate = _learningRate;

            while (iteration < _numberOfIterations)
            {
                //Neighbourhood radius for the neuron over this iteration
                var currentRadius = CalculateNeighbourhoodRadius(iteration);

                //
                for (int i = 0; i < input.Length; i++)
                {
                    //COMPETITION
                    //Best Matching Unit for the current input vector, winning node...
                    //is the closest based on the weighted values for each element of the vector
                    var currentInput = input[i];
                    var bmu = CalculateBMU(currentInput);

                    //BMU RADIUS
                    //Use the radius to find the limits of the neighbourhood on the matrix
                    (int xStart, int xEnd, int yStart, int yEnd) = GetRadiusIndexes(bmu, currentRadius);

                    //ADAPTION & COOPERATION
                    for (int x = xStart; x < xEnd; x++)
                    {
                        for (int y = yStart; y < yEnd; y++)
                        {
                            //Neuron at x,y is the one to be processed this iteration
                            var processingNeuron = GetNeuron(x, y);
                            //The distance between the BMU and the this neuron
                            var distance = bmu.Distance(processingNeuron);
                            //The distance must be within the neighbourhoods radius
                            if (distance <= Math.Pow(currentRadius, 2.0))
                            {
                                //COOPERATION - Reduce the neighbourhoods radius
                                var distanceDrop = GetDistanceDrop(distance, currentRadius);
                                //ADAPTION - Update the weights of this neuron
                                processingNeuron.UpdateWeights(currentInput, learningRate, distanceDrop);
                            }
                        }
                    }
                }
                iteration++;
                //Adjust the learing rate to suit the iteration
                learningRate = _learningRate * Math.Exp(-(double)iteration / _numberOfIterations);
            }
            //OUTPUT
            UpdateOutput(input, hasHeader);
            Console.WriteLine("Training of SOM complete\n");
        }//Train

        //Result map - Neurons have been ordered into neighbourhoods of similar nodes
        internal void UpdateOutput(SomVector[] inputList, bool hasHeader)
        {
            //Match file rows to vector ID & index
            int idAdder = 0;
            if (hasHeader)
            {
                idAdder = 1;
            }
            //Update output matrix with BMU including updated input vector index list
            foreach (var neuron in _matrix)
            {
                Results results = new Results();
                for (int i = 0; i < inputList.Length; i++)
                {
                    //Find the BMU for the input vector
                    var bmu = CalculateBMU(inputList[i]);

                    //Add the vectors index to the list of inputs related to this neuron
                    if (neuron == bmu)
                    {
                        results.AddCoords(bmu.x, bmu.y);
                        results.InputToList(inputList[i].Id + idAdder);

                        //Add the input array to the output list
                        if (!_outputSeries.Exists(x => x == results))
                            _outputSeries.Add(results);
                    }
                }
                //Create a unique value based upon the weighting
                var mean = neuron.Weights.Mean();
                string hexValue = mean.ToString("X");
                neuron.hexLabel = hexValue;

                //Add the updated neuron to the output matrix
                _outputMatrix.Add(neuron);
            }
            //Vector details
            _vectorDetailsList.AddRange(inputList);
        }

        //Get output matrix and the input series with lablels added to for identification
        public (List<ISomNeuron> outputMatrix, List<Results> outputSeries) GetOutput()
        {
            //OUTPUT VECTOR ELEMENTS CAN BE MAPPED BACK HERE

            //Output matrix neuron[x,y] holds a list of input vectors indexes.
            //Input series is a series of vectors each has a label attached
            return (_outputMatrix, _outputSeries);
        }

        //Match input vector to results
        public dynamic GetVectorDetails(int id, bool hasHeader)
        {
            //Match file rows to vector ID & index
            int idAdder = 0;
            if (hasHeader)
            {
                idAdder = 1;
            }

            //Map vector data to display data
            DtoMapping dtoMapping = new DtoMapping();
            //Get the vector
            var data = dtoMapping.Mapping(_vectorDetailsList.Find(x => x.Id + idAdder == id));

            return data;
        }

        //Get the x/y axis limits
        internal (int xStart, int xEnd, int yStart, int yEnd) GetRadiusIndexes(ISomNeuron bmu, double currentRadius)
        {
            //Find the limits of the neighbourhood radius in indicies limits
            //Start x index is the BMUs x-axis minus the radius size, - 1 for zero-based array
            var xStart = (int)(bmu.x - currentRadius - 1);
            xStart = (xStart < 0) ? 0 : xStart; //If start position is less than 0 then use 0, else calculation.
                                                //End x index is the start position, plus double the radius, + 1 for zero-based array
            var xEnd = (int)(xStart + (currentRadius * 2) + 1);
            if (xEnd > _width) //Maximum x axis position is the width
                xEnd = _width;
            //
            var yStart = (int)(bmu.y - currentRadius - 1);
            yStart = (yStart < 0) ? 0 : yStart;
            //
            var yEnd = (int)(yStart + (currentRadius * 2) + 1);
            if (yEnd > _height) //Maximum y axis position is the hight
                yEnd = _height;

            return (xStart, xEnd, yStart, yEnd);
        }

        //Find the neuron at this position in the matrix
        internal ISomNeuron GetNeuron(int indexX, int indexY)
        {
            //Invalid coordinates
            if (indexX > _width || indexY > _height)
                throw new ArgumentException("Wrong index!");

            //Neuron coordinates retrieve a neuron from the matrix
            return _matrix[indexX, indexY];
        }

        //Return neighbourhood radius of the matrix for this iteration - Neurons within the perimeter are similar
        internal double CalculateNeighbourhoodRadius(double iteration)
        {
            //2.71828 raised to the power (-x) = iteration multipled by the time constant, 
            //each iteration reduces the matrix, 0 (first iteration) = 0 reduction of the matrix
            return _matrixRadius * Math.Exp(-iteration / _timeConstant);
        }

        //Return the reduction in the distance between the nodes - Brings close neurons closer
        internal double GetDistanceDrop(double distance, double radius)
        {
            //2.71828 raised to the power (-x) = distance by the radius size, 
            //each iteration reduces the matrix, 0 (first iteration) = 0 reduction of the matrix
            return Math.Exp(-(Math.Pow(distance, 2.0) / Math.Pow(radius, 2.0)));
        }

        //Find the Best Matching Unit - i.e. the closest neuron
        internal ISomNeuron CalculateBMU(ISomVector input)
        {
            //Find the distance between the input neuron and the BMU neuron,
            //The initial BMU is the first point in the map at coords 0,0
            ISomNeuron bmu = _matrix[0, 0];
            double winningDistance = input.DistanceBetweenPoints(bmu.Weights, "euc");

            //COMPETITION
            //Find the distance between the input vector and each of the map vectors compared to the
            //winning distance between the input and the BMU, iterate through the whole matrix, updating the
            //BMU and the winning distance accordingly.
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    //Distance between the input neuron and the map neuron at the coordinates i,j
                    var distance = input.DistanceBetweenPoints(_matrix[i, j].Weights, "euc");

                    //Matrix neuron distance Vs currently winning distance
                    if (distance < winningDistance)
                    {
                        //Map neuron is new BMU
                        bmu = _matrix[i, j];
                        //Map neurons distance is new winning distance 
                        winningDistance = distance;
                    }
                }
            }
            return bmu;
        }

        //Build the map x/y lengths & match the vector size with the input vector size,
        //then initialise the vector element weights with a random initial value.
        private void InitializeConnections(int inputDimention)
        {
            //x = width, y = height
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    //Update the matrix with a new neuron - weights are initialised in the constructor
                    _matrix[i, j] = new SomNeuron(inputDimention) { x = i, y = j };
                }
            }
        }
    }
}
