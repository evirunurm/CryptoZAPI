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
		Task<List<History>> GetAllHistoriesForUser(int userId, int? limit);
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
            Currency currency = await _context.Currencies.FirstOrDefaultAsync(c => c.Id == id) ??
                throw new ArgumentNullException("No existe la moneda parameter");

            return currency;
        }

        public async Task<Currency?> GetOneCurrency(string code)
        {
            Currency currency = await _context.Currencies.FirstOrDefaultAsync(c => c.Code == code) ??
                throw new ArgumentNullException("No existe la moneda parameter");

            return currency;
        }

        public async Task<Currency> CreateCurrency(Currency currency) {
			var old_currency = await _context.Currencies.FirstOrDefaultAsync(c => c.Code == currency.Code);

			if (old_currency == null) {
				await _context.Currencies.AddAsync(currency);

				await saveDB();
				old_currency = currency;
			}
			else {
                await ModifyCurrency(old_currency.Id, currency);
			}


			return old_currency;
		}

        public async Task<Currency> ModifyCurrency(int id, Currency currency)
        {
            // (Luis) Asumo que solamente se deben cambiar Name, Price, PriceDate y LogoUrl

            var old_currency = await _context.Currencies.FirstOrDefaultAsync(c => c.Id == id);

            if (old_currency == null)
            {
                return null;
            }

            old_currency.Name = currency.Name;
            old_currency.Price = currency.Price;
            old_currency.PriceDate = currency.PriceDate;
            old_currency.LogoUrl = currency.LogoUrl;

            await saveDB();

            return currency;
        }



        // HISTORY
        public async Task<List<History>> GetAllHistoriesForUser(int userId, int? limit)
        {
            // TODO: Refactor

            if (limit != null)
            {
                return await _context.Histories
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.Date)
                .Take((int) limit)
                .ToListAsync();
            }

            return await _context.Histories
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.Date)
                .ToListAsync();
        }

        public async Task<History> CreateHistory(History history) 
        {
            try{
                history.Date = DateTime.Now;
                await _context.Histories.AddAsync(history);

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
            return await _context.Users.FirstAsync(u => u.Id == id); // Throws exception in case it's not found
        }

        public async Task<User> CreateUser(User user) {
            try
            {
                // Validate user doesn't exist
                if (_context.Users.Any(x => x.Email == user.Email))
                {
                    throw new Exception($"User with the email { user.Email } already exists");
                }
                // Here we should map userDto to User

                // Hash password and generate salt
                user.Salt = BCrypt.Net.BCrypt.GenerateSalt();
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, user.Salt);

                // Save user
                await _context.Users.AddAsync(user);
                _context.SaveChanges();

                return user;
            }
            catch (Exception e)
            {
                throw new Exception();
            }

            
        }

		public async Task<User> ModifyUser(int id, User user) {
            // throw new NotImplementedException();

            // TODO: finish
            _context.Users.Update(user);
            return user;
        }


        // DB
		private async Task saveDB() {
			await _context.SaveChangesAsync();
		}
	}
}