using CryptoZAPI.Models;
using Serilog;

namespace Utils
{
    public class Conversion
    {
        public static double Convert(Currency origin, Currency destination, double value)
        {
            // Possible future parameter: Precision --> 3
            int precision = 3;
            try
            {        
                   if (origin.Code.Equals("USD"))
                    {
                        return Math.Round(value / destination.Price, precision);
                    }
                    return Math.Round(origin.Price * value / destination.Price, precision);
                
            }
            catch (OverflowException e)
            {
                Log.Error(e, e.Message);
                Console.WriteLine(e.Message);
                return Math.Round(value, precision);
            }
        }
    }
}