using CryptoZAPI.Models;
using Data;
using Models;
using Models.DTO;

namespace Repo {
    public interface IRepository {
        // All return types might have to be nullable, in case there's a captured Exception.
        // Currencies
        // GET
        List<Currency> GetAllCurrencies();
        Currency? GetOneCurrency(int id);
        // PUT
        Currency ModifyCurrency(int id, Currency currency);
        // POST
        Currency CreateCurrency(Currency currency);


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

    public class Repository : IRepository {

        CryptoZContext _context = new CryptoZContext();

        public Currency CreateCurrency(Currency currency) {
            var old_currency = _context.Currencies.FirstOrDefault(c => c.Code == currency.Code);

            if (old_currency == null) {
                _context.Currencies.Add(currency); // TODO: Make async 

                _context.SaveChanges(); // TODO: Use separate method for saving saveDB()
                old_currency = currency;
            }
            else {
                ModifyCurrency(old_currency.Id, currency); // TODO: Make async
            }


            return old_currency;
        }

        public History CreateHistory(History history) {
            throw new NotImplementedException();
        }

        public User CreateUser(User user) {
            throw new NotImplementedException();
        }

        public List<Currency> GetAllCurrencies() {

            return _context.Currencies.ToList();
        }

        public List<History> GetAllHistoriesForUser(Guid userId, int limit) {
            throw new NotImplementedException();
        }

        public Currency? GetOneCurrency(int id) {
            Currency currency = _context.Currencies.FirstOrDefault(c => c.Id == id) ?? 
                throw new ArgumentNullException("No existe la moneda parameter"); ;

            return currency;
        }

        public User GetOneUser(int id) {
            throw new NotImplementedException();
        }


        public Currency ModifyCurrency(int id, Currency currency) {

            //throw new NotImplementedException();
            // (Luis) Asumo que solamente se deben cambiar Name, Price, PriceDate y LogoUrl

            var old_currency = _context.Currencies.FirstOrDefault(c => c.Id == id);

            if (old_currency == null) {
                return null;
            }

            old_currency.Name = currency.Name;
            old_currency.Price = currency.Price;
            old_currency.PriceDate = currency.PriceDate;
            old_currency.LogoUrl = currency.LogoUrl;

            _context.SaveChanges();

            return currency;
        }

        public User ModifyUser(int id, User user) {
            throw new NotImplementedException();
        }

        private async Task saveDB() {
            await _context.SaveChangesAsync();
        }
    }
}