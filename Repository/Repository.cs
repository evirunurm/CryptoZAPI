using CryptoZAPI.Models;
using Models;

namespace Repository
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


        // Users
        // GET
        User GetOneUser(int id);
        // POST
        User CreateUser(User user);
        // PUT
        User ModifyUser(int id, User user);


        // Histories
        // GET
        List<History> GetAllHistoriesForUser(int userId, int limit);
        // POST
        History CreateHistory(History history);


    }

    public class Repository : IRepository
    {
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
            throw new NotImplementedException();
        }

        public List<History> GetAllHistoriesForUser(int userId, int limit)
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
    }
}