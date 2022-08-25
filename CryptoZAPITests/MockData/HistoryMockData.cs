using CryptoZAPI.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoZAPITests.MockData;
class HistoryMockData {
    /*
    public static List<Histories> GetAll() {
        Users usuario = new Users("Mock_Test", "Mock@test.com", "1234", "1234");
        Currency origen = new Currency("USD", "Dollar", 1, DateTime.Parse("2022-08-02T00:00:00Z"), ""),
              destino = new Currency("ETH", "Ethereum", 1588.71613115, DateTime.Parse("2022-08-02T00:00:00Z"), "");

        return new List<Histories> {
           new Histories(origen, destino,10,20,DateTime.Now, usuario)
        };
    }
    */
    public static List<History> GetEmpty() {
        return new List<History>();
    }
}

