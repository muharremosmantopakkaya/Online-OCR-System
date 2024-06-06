using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Helpers.Models.Dtos.DbModelDtos
{
    public class StockApiDto
    {
        public double C { get; set; }  // Current price
        public double H { get; set; }  // High price of the day
        public double L { get; set; }  // Low price of the day
        public double O { get; set; }  // Open price of the day
        public double Pc { get; set; } // Previous close price
    }
}
