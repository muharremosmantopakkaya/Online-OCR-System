namespace EkonLayer.Helpers.Models.Dtos.DbModelDtos
{
    public class StockQuoteDto
    {
        public double C { get; set; }  // Current price
        public double D { get; set; }  // Change
        public double DP { get; set; } // Percent change
        public double H { get; set; }  // High price of the day
        public double L { get; set; }  // Low price of the day
        public double O { get; set; }  // Open price of the day
        public double PC { get; set; } // Previous close price
    }
}
