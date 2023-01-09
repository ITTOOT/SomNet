using SomNet.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomNet.Models
{
    public class Results : ICoordinates
    {
        public double x { get; set; }
        public double y { get; set; }
        public double? z { get; set; }

        public List<int> InputList { get; set; }

        public Results()
        {
            InputList = new List<int>();
        }

        //Add a vectors index position to the neurons list
        public void AddCoords(double xAxis, double yAxis, double? zAxis = 0)
        {
            x = xAxis;
            y = yAxis;
            z = zAxis;
        }

        //Add a vectors index position to the neurons list
        public void InputToList(int id)
        {
            if (!InputList.Any(x => x == id))
            {
                InputList.Add(id);
            }
        }
    }
}
