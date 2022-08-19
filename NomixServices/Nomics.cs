using CryptoZAPI.Models;

namespace NomixServices
{
    public interface INomics
    {
        List<Currency> getCurrencies();
    }

    public class Nomics : INomics
    {
        public List<Currency> getCurrencies()
        {
            return new List<Currency>();
        }
    }
}