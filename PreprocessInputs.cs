using AutoMapper.Internal.Mappers;
using CsvHelper;
using CsvHelper.Configuration;
using FSharp.Stats.Fitting;
using MathNet.Numerics;
using Microsoft.VisualBasic;
using Plotly.NET;
using SomNet.Interfaces;
using SomNet.Models;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FSharpAux.IO.SchemaReader;

namespace SomNet
{
    public class PreprocessInputs
    {
        //Set a unique idntifier for the input vector
        public void SetVectorLabel(SomVector vector, string name)
        {
            vector.Label = name;
        }

        //
        //Create a file that simulates input vectors
        public int GetFileVectors(string dataLocation, int sampleSize, out SomVector[] vectorList)
        {
            // Read data and labels into memory
            Console.WriteLine("Reading data from file into memory");

            //Add this file at the storage location - Test file is in >bin - C:\Users\Player_1\Desktop\WORK\SomNet\bin\tblHistorical20k.csv
            FileStream fileStream = new FileStream(dataLocation, FileMode.Open);
            StreamReader reader = new StreamReader(fileStream);

            string[] tokens = null;
            string line = "";
            int row = 0;

            //Test data of [sample size] [vector]
            vectorList = new SomVector[sampleSize];

            //Read each line from the open file to a data table, each line  represents a hand drawn digit
            while ((line = reader.ReadLine()) != null)
            {
                //Each line of the file is split at the comma, into seperate token strings
                vectorList[row] = new SomVector();
                vectorList[row].Label = "Item No: " + row.ToString();

                //
                tokens = line.Split(',');

                //Vector has a value added, parsed as a number.
                foreach (var token in tokens)
                {
                    vectorList[row].Add(double.Parse(token));
                }
                ++row;
            }
            fileStream.Close();
            reader.Close();

            return 0;
        }

        //Create a file that simulates input vectors
        public SomVector[] GetCsvVectors(string dataLocation, int sampleCount, bool hasHeader)
        {
            // Read data and labels into memory
            Console.WriteLine("Reading data from file into memory");

            //Config info for stream reader
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = hasHeader,
                Comment = '#',
                AllowComments = true,
                Delimiter = ",",
                MissingFieldFound = null,
                TrimOptions = TrimOptions.None,
            };

            //Add this file at the storage location - Test file is in >bin - C:\Users\Player_1\Desktop\WORK\SomNet\bin\tblHistorical20k.csv
            FileStream fileStream = new FileStream(dataLocation, FileMode.Open);

            //Match file rows to vector ID & index
            int skipRows = 0;
            if (hasHeader)
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                skipRows = 2;
            }
            else
            {
                skipRows = 1;
            }

            //Read as a srteam
            StreamReader streamReader = new StreamReader(fileStream);
            var csvReader = new CsvReader(streamReader, csvConfig);

            //Records for:
            //Station 70
            var records = csvReader.GetRecords<Sta70Data>();

            DateTime dateTime;
            double[] token = new double[11];
            SomVector[] vectorList = new SomVector[sampleCount];

            //Read each line from the open file to a data table, each line  represents a hand drawn digit
            int row = 0;
            foreach (var record in records)
            {
                vectorList[row] = new SomVector();

                //Each record/line of the file is split at the comma, into seperate token strings
                DateTime.TryParse(record.EntryTime, out dateTime);
                double.TryParse(record.OverAllResult, out token[0]);
                double.TryParse(record.CycleTime, out token[1]);
                double.TryParse(record.CapnutTypeResult, out token[2]);
                double.TryParse(record.OMVSpringResult, out token[3]);
                double.TryParse(record.NozzlePreLoadForce, out token[4]);
                double.TryParse(record.NozzlePreLoadPosition, out token[5]);
                double.TryParse(record.StackBuildResult, out token[6]);
                double.TryParse(record.CapnutTorque, out token[7]);
                double.TryParse(record.CapnutTorqueAngle, out token[8]);
                double.TryParse(record.CapnutFinalAngle, out token[9]);
                double.TryParse(record.CapnutAssemblyResult, out token[10]);
                //Update the vector
                vectorList[row].Id = row + skipRows;
                vectorList[row].Label = "Item No: " + (row + skipRows).ToString();
                vectorList[row].Timestamp = dateTime;
                //From dataset
                vectorList[row].Add(token[0]);
                vectorList[row].Add(token[1]);
                vectorList[row].Add(token[2]);
                vectorList[row].Add(token[3]);
                vectorList[row].Add(token[4]);
                vectorList[row].Add(token[5]);
                vectorList[row].Add(token[6]);
                vectorList[row].Add(token[7]);
                vectorList[row].Add(token[8]);
                vectorList[row].Add(token[9]);
                vectorList[row].Add(token[10]);
                row++;
            }
            // Read data and labels into memory
            Console.WriteLine("Data read complete\n");
            return vectorList;
        }
    }
}
