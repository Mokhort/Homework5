using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
{
    class Route
    {
        public int Id { get; set; }
        public List<Airport>  Airport_dep { get; set; }
        public List<Airport> Airport_arr { get; set; }
        public int destination { get; set; }

    }
}
