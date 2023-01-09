using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomNet.Interfaces
{
    public interface ISomNeuron
    {
        double x { get; set; }
        double y { get; set; }
        ISomVector Weights { get; }
        string hexLabel { get; set; }

        double Distance(ISomNeuron neuron);
        void SetWeight(int index, double value);
        double GetWeight(int index);
        //distanceDecay = neighbourhood function
        void UpdateWeights(ISomVector input, double distanceDecay, double learningRate);
    }
}
