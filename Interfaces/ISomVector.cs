using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomNet.Interfaces
{
    //Used for all vectors in the system, with each vextor a list of n' doubles
    public interface ISomVector : IList<double> //IList<double>
    {
        int Id { get; set; }
        string Label { get; set; }
        public DateTime Timestamp { get; set; }
        double DistanceBetweenPoints(ISomVector mapVector, string calc);
    }
}
