using SomNet.ML;
using SomNet.Models;
using System.Linq;
using System.Xml.Linq;
using ChartingBasics;
using System.Reflection.Emit;
using SomNet.Charting;
using static FSharp.Stats.ML.SurprisalAnalysis;
using SomNet.Utilities;
using System.IO;
using System.Collections.Generic;
using static Plotly.NET.StyleParam.DrawingStyle;
using System.Data;
using Microsoft.DotNet.Interactive.Utility;
using PuppeteerSharp;
using SomNet.Verification;
using AutoMapper;

namespace SomNet
{
    class Program
    {
        static void Main(string[] args)
        {
            //INPUT VARIABLES
            //Build the map as a n' x n' x,y square, with n' inputs,
            //over n' training iterations, with a starting learning rate of n'.n'
            string vectorName = "Item: ";
            int xAxis = 10; //X axis width
            int yAxis = 10; //Y axis length
            int vectorSize = 11; //Vector elements
            double iterations = 1000; //Training iterations (how many to cycle through to obtain the matrix)
            double learningRate = 0.5; //Initial learning rate
            int sampleSize = 50; //Size of the dataset
            bool hasHeader = true; //Does the dataset include a header

            //DATAPATH
            //string dataLocation = "..\\..\\Sta70_20k.csv"; //"..\\..\\Sta70_20k.csv" //"..\\..\\tblHistorical20k.csv"
            string dataLocation = "C:\\Users\\Player_1\\Desktop\\WORK\\SomNet\\Datasets\\Sta70_20k_Test.csv";

            //PREPROCESSOR
            PreprocessInputs preprocessInputs = new PreprocessInputs();
            var inputList = preprocessInputs.GetCsvVectors(dataLocation, sampleSize, hasHeader);
            //INPUT LIST STORED HERE

            //SOM SET-UP 
            var som = new SOMap(xAxis, yAxis, vectorSize, iterations, learningRate, inputList);

            //TRAIN - Train the map based on the inputs
            som.Train(inputList, hasHeader);

            //RESULTS - Result of training (an orgainsed map)
            (var outputMatrix, var outputSeries) = som.GetOutput();

            //Output mapper
            //mappedSeries = autoMapped(outputSeries, mapToUse);

            //SOM VERIFIER - 
            SoMVerifier soMVerifier = new SoMVerifier();
            soMVerifier.OutputTest_1(som, outputMatrix, outputSeries, hasHeader);

            //CHARTING - Add each series and matrix to a list then iterate to plot
            FChartAdapter fChartAdapter = new FChartAdapter();
            fChartAdapter.Plotter(som, outputSeries, outputMatrix, hasHeader, "DEFAULT");

        }//Main
    }//Program
}//Namespace

