using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ChartingBasics;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.FSharp.Collections;
using Microsoft.VisualBasic;
using SomNet.Interfaces;
using SomNet.ML;
using SomNet.Models;
using static FSharp.Stats.ML.SurprisalAnalysis;

namespace SomNet.Charting
{
    public class FChartAdapter
    {

        public FChartAdapter()
        {
            //TO DO
        }

        public int Plotter(SOMap som, List<Results> outputSeries, List<ISomNeuron> outputMatrix, bool hasHeader, string plotType)
        {
            //Plot
            int statusCode = 99;

            //Adapt C# to F#
            //Vectors
            List<double> vX = new List<double>();
            List<double> vY = new List<double>();
            List<int> inputLists = new List<int>();
            List<int?> idList = new List<int?>();
            List<string?> labelList = new List<string?>();
            List<DateTime?> timestampList = new List<DateTime?>();
            List<int?> overAllResultList = new List<int?>();
            List<double?> cycleTimeList = new List<double?>();
            List<int?> capnutTypeResultList = new List<int?>();
            List<int?> omvSpringResultList = new List<int?>();
            List<double?> nozzlePreLoadForceList = new List<double?>();
            List<double?> nozzlePreLoadPositionList = new List<double?>();
            List<int?> stackBuildResultList = new List<int?>();
            List<double?> capnutTorqueList = new List<double?>();
            List<double?> capnutTorqueAngleList = new List<double?>();
            List<double?> capnutFinalAngleList = new List<double?>();
            List<int?> capnutAssemblyResultList = new List<int?>();
            //
            int vectorCount = outputSeries.Count();
            foreach (var result in outputSeries)
            {
                //Get resulting vectors locations
                var vectorX = result.x;
                var vectorY = result.y;
                //Get number of vectors at location
                var elementCount = result.InputList.Count;
                //Get the details of each vector
                foreach (var resultInList in result.InputList)
                {
                    Sta70DataDTO vector = som.GetVectorDetails(resultInList, hasHeader);
                    //Vector details
                    //Added
                    idList.Add(vector.Id);
                    labelList.Add(vector.Label);
                    timestampList.Add(vector.Timestamp);
                    //Mapped
                    overAllResultList.Add(vector.OverAllResult);
                    cycleTimeList.Add(vector.CycleTime);
                    capnutTypeResultList.Add(vector.CapnutTypeResult);
                    omvSpringResultList.Add(vector.OMVSpringResult);
                    nozzlePreLoadForceList.Add(vector.NozzlePreLoadForce);
                    nozzlePreLoadPositionList.Add(vector.NozzlePreLoadPosition);
                    stackBuildResultList.Add(vector.StackBuildResult);
                    capnutTorqueList.Add(vector.CapnutTorque);
                    capnutTorqueAngleList.Add(vector.CapnutTorqueAngle);
                    capnutFinalAngleList.Add(vector.CapnutFinalAngle);
                    capnutAssemblyResultList.Add(vector.CapnutAssemblyResult);
                }
                vX.Add(vectorX);
                vY.Add(vectorY);
            }

            //Neurons
            List<double> nX = new List<double>();
            List<double> nY = new List<double>();
            List<double> nW = new List<double>();
            //
            int neuronCount = outputMatrix.Count();
            foreach (var neuron in outputMatrix)
            {
                var nWeights = neuron.Weights;
                var neuronX = neuron.x;
                var neuronY = neuron.y;
                nX.Add(neuronX);
                nY.Add(neuronY);
                nW.AddRange(nWeights); //n' / n' of vector elements for weights at a position
            }

            //Start plotting chart
            Console.WriteLine("Starting plotting of chart", outputMatrix.Count());
            Console.WriteLine("Matrix count: {0}", outputMatrix.Count());
            Console.WriteLine("Series count: {0}", outputSeries.Count());

            //Choose the plot type for this chart
            BasicCharts chart = new BasicCharts();

            switch (plotType)
            {
                case "scatter":
                    //statusCode = chart.simpleScatterChart(ListModule.OfSeq(nX), ListModule.OfSeq(nY), ListModule.OfSeq(mLabels));
                    break;
                case "heatmap":
                    //TO DO
                    statusCode = chart.simpleHeatmap(ListModule.OfSeq(nX), ListModule.OfSeq(nW));
                    break;
                case "bubble":
                    //TO DO
                    statusCode = chart.simpleHeatmap(ListModule.OfSeq(nX), ListModule.OfSeq(nY));
                    break;
                default:
                    //TO DO

                    break;
            }

            //Return the status of the operation 99 = nothing happened
            return statusCode;
        }


    }
}
