using CryptoZAPI.Models;
using Serilog;

namespace Utils
{
    public class Conversion
    {
        public static double Convert(Currency origin, Currency destination, double value)
        {
            // Possible future parameter: Precision --> 3
            try
            {        
                   if (origin.Code.Equals("USD"))
                    {
                        return Math.Round(value / destination.Price, 3);
                    }
                    return Math.Round(origin.Price * value / destination.Price, 3);
                
            }
            catch (OverflowException e)
            {
                Log.Error(e, e.Message);
                Console.WriteLine(e.Message);
            }
            return Math.Round(value, 3);
        }
    }
}