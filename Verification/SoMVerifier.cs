using ICSharpCode.SharpZipLib.Tar;
using SomNet.Interfaces;
using SomNet.ML;
using SomNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomNet.Verification
{
    public class SoMVerifier
    {
        public void OutputTest_1(SOMap som, List<ISomNeuron> outputMatrix, List<Results> outputSeries, bool hasHeader)
        {
            //Output the results of the SoM to the console
            //Should display the weighting of the resulting neuron map with
            //the locations of the input vectors on the map, as a list of indexes.
            //This can then be used to display the details of these vectors, i.e.
            //element values, label names, timestamps etc...

            //Set limits
            int maxNeuron = 100;
            int maxSeries = 50;

            //Output Neuron Matrix
            if (true)
            {
                var i = 0;
                Console.WriteLine("TEST: Output matrix count: {0}\n", outputMatrix.Count());
                while (i < maxNeuron)
                {
                    foreach (var neuron in outputMatrix)
                    {
                        Console.WriteLine("Neuron no: {0}", i);
                        Console.WriteLine("X: {0}, Y: {1}", neuron.x, neuron.y);
                        foreach (var weight in neuron.Weights)
                            Console.WriteLine("    Vector weights: {0}", weight);

                        Console.WriteLine("\n");
                        i++;
                    }
                }
            }

            //Output Vector Series
            if (true)
            {
                var j = 0;
                Console.WriteLine("TEST: Output series count: {0}\n", outputSeries.Count());
                while (j < maxSeries)
                {
                    foreach (var result in outputSeries)
                    {
                        Console.WriteLine("Neuron: X: {0}, Y: {1}", result.x, result.y);

                        foreach (var resultInList in result.InputList)
                        {
                            var vector = som.GetVectorDetails(resultInList, hasHeader);
                            //Label name & timestamp of vector
                            Console.WriteLine("ID : {0}", vector.Id);
                            Console.WriteLine("Item : {0}", vector.Label);
                            Console.WriteLine("    Details:");
                            Console.WriteLine("    Timestamp: {0}", vector.Timestamp);
                            //Vector elements used for weighting
                            //Console.WriteLine("    Over All Result: {0}", vector.OverAllResult);
                            //Console.WriteLine("    Cycle Time: {0}", vector.CycleTime);
                            //Console.WriteLine("    Capnut Type Result: {0}", vector.CapNutTypeResult);
                            //Console.WriteLine("    OMV Spring Result: {0}", vector.OMVSpringResult_PI);
                            //Console.WriteLine("    Nozzle Preload Force: {0}", vector.NozzlePreLoadForce);
                            //Console.WriteLine("    Nozzle Preload Position: {0}", vector.NozzlePreLoadPosition);
                            //Console.WriteLine("    Stack Build Result: {0}", vector.StackBuildResult);
                            //Console.WriteLine("    Capnut Torque: {0}", vector.CapNutTorque);
                            //Console.WriteLine("    Capnut Torque Angle: {0}", vector.CapNutTorqueAngle);
                            //Console.WriteLine("    Capnut Final Angle: {0}", vector.CapNutFinalAngle);
                            //Console.WriteLine("    Capnut Assembly Result: {0}", vector.CapNutAssemblyResult);
                        }

                        Console.WriteLine("\n");
                        j++;
                    }
                }
            }
        }
    }
}
