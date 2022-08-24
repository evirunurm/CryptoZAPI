using CryptoZAPI.Models;
using Data;
using Models;

namespace Repo
{
    public interface IRepository
    {
        // All return types might have to be nullable, in case there's a captured Exception.
        // Currencies
        // GET
        List<Currency> GetAllCurrencies();
        Currency GetOneCurrency(int id);
        // PUT
        Currency ModifyCurrency(int id, Currency currency);
        // POST
        Currency CreateCurrency(CurrencyDto currencyDto);


        // Users
        // GET
        User GetOneUser(int id);
        // POST
        User CreateUser(User user);
        // PUT
        User ModifyUser(int id, User user);


        // Histories
        // GET
        List<History> GetAllHistoriesForUser(Guid userId, int limit);
        // POST
        History CreateHistory(History history);


    }

    public class Repository : IRepository
    {

        CryptoZContext _context = new CryptoZContext();

        public Currency CreateCurrency(CurrencyDto currencyDto)
        {
            throw new NotImplementedException();
        }

        public History CreateHistory(History history)
        {
            throw new NotImplementedException();
        }

        public User CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public List<Currency> GetAllCurrencies()
        {

            return _context.Currencies.ToList();
        }

        public List<History> GetAllHistoriesForUser(Guid userId, int limit)
        {
            throw new NotImplementedException();
        }

        public Currency GetOneCurrency(int id)
        {
            throw new NotImplementedException();
        }

        public User GetOneUser(int id)
        {
            throw new NotImplementedException();
        }

        public Currency ModifyCurrency(int id, Currency currency)
        {
            throw new NotImplementedException();
        }

        public User ModifyUser(int id, User user)
        {
            throw new NotImplementedException();
        }

        private async Task saveDB()
        {
            await _context.SaveChangesAsync();
        }
    }
}