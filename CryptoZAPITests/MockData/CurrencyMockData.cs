using CryptoZAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoZAPITests.MockData;
class CurrencyMockData
{
    
    public static List<Currency> GetAll()
    {

        return null;
        /*
        return new List<Currency>{
             new Currency(
                 "USD",
                 "Dollar",
                 1,
                  DateTime.Parse("2022-08-02T00:00:00Z"),
                 ""
             ),
             new Currency(
                 "ETH",
                 "Ethereum",
                 1588.71613115,
                  DateTime.Parse("2022-08-02T00:00:00Z"),
                 ""
             ),
             new Currency(
                 "BTC",
                 "Bitcoin",
                 22873.54872707,
                 DateTime.Parse("2022-08-02T00:00:00Z"),
                 ""
             ),
              new Currency(
                 "USDC",
                 "USD Coin",
                 1.00283189,
                 DateTime.Parse("2022-08-02T00:00:00Z"),
                 ""
             ),
              new Currency(
                 "USDT",
                 "Tether",
                 1.00141885,
                 DateTime.Parse("2022-08-02T00:00:00Z"),
                 ""
             )
         };*/
    }

    public static List<Currency> GetEmpty()
    {
        return new List<Currency>();
    }
}

