using CryptoZAPI.Models;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;

namespace Repo {
    public interface IRepository {
        // All return types might have to be nullable, in case there's a captured Exception.
        // Currencies
        // GET
        Task<List<Currency>> GetAllCurrencies();
        Task<Currency?> GetOneCurrency(int id);
         Task<Currency?> GetOneCurrency(string code);
        // PUT
        Task<Currency> ModifyCurrency(int id, Currency currency);
        // POST
        Task<Currency> CreateCurrency(Currency currency);


        // Users
        // GET
        Task<User> GetOneUser(int id);
        // POST
        Task<User> CreateUser(User user);
        // PUT
        Task<User> ModifyUser(int id, User user);


		// Histories
		// GET
		Task<List<History>> GetAllHistoriesForUser(Guid userId, int limit);
		// POST
		Task<History> CreateHistory(History history);


	}

	public class Repository : IRepository {

		CryptoZContext _context = new CryptoZContext();


        // CURRENCY
        public async Task<List<Currency>> GetAllCurrencies()
        {
            var currencies = await _context.Currencies.ToListAsync() ?? throw new ArgumentNullException();

            return currencies;
        }

        public async Task<Currency?> GetOneCurrency(int id)
        {
            Currency currency = _context.Currencies.FirstOrDefault(c => c.Id == id) ??
                throw new ArgumentNullException("No existe la moneda parameter");

            return currency;
        }

        public async Task<Currency?> GetOneCurrency(string code)
        {
            Currency currency = _context.Currencies.FirstOrDefault(c => c.Code == code) ??
                throw new ArgumentNullException("No existe la moneda parameter");

            return currency;
        }

        public async Task<Currency> CreateCurrency(Currency currency) {
			var old_currency = _context.Currencies.FirstOrDefault(c => c.Code == currency.Code);

			if (old_currency == null) {
				_context.Currencies.Add(currency); // TODO: Make async 

				await saveDB(); // TODO: Use separate method for saving saveDB()
				old_currency = currency;
			}
			else {
				ModifyCurrency(old_currency.Id, currency); // TODO: Make async
			}


			return old_currency;
		}

        public async Task<Currency> ModifyCurrency(int id, Currency currency)
        {

            //throw new NotImplementedException();
            // (Luis) Asumo que solamente se deben cambiar Name, Price, PriceDate y LogoUrl

            var old_currency = _context.Currencies.FirstOrDefault(c => c.Id == id);

            if (old_currency == null)
            {
                return null;
            }

            old_currency.Name = currency.Name;
            old_currency.Price = currency.Price;
            old_currency.PriceDate = currency.PriceDate;
            old_currency.LogoUrl = currency.LogoUrl;

            _context.SaveChanges();

            return currency;
        }



        // HISTORY
        public async Task<List<History>> GetAllHistoriesForUser(Guid userId, int limit)
        {
            throw new NotImplementedException();
        }

        public async Task<History> CreateHistory(History history) 
        {
            try{
                history.Date = DateTime.Now;
                await _context.History.AddAsync(history);
                await saveDB();

                return history;
             
            }
            catch (Exception e)
            {
                throw new Exception();
            }
          			
		}


        // USER
        public async Task<User> GetOneUser(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> CreateUser(User user) {
			throw new NotImplementedException();
		}

		public async Task<User> ModifyUser(int id, User user) {
			throw new NotImplementedException();
		}


        // DB
		private async Task saveDB() {
			await _context.SaveChangesAsync();
		}
	}
}