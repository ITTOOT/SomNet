using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomNet.Interfaces
{
    public interface ICoordinates
    {
        double x { get; set; }
        double y { get; set; }
        double? z { get; set; }
        //ADD COORDINATION REQUIREMENTS BELOW
    }
}
