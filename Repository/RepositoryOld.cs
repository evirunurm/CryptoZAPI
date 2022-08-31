using CryptoZAPI.Models;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;

namespace Repo {

    public interface IRepositoryOld {
        // -- Currencies --
        // ----- GET -----
        Task<List<Currency>> GetAllCurrencies(); // YES
        Task<Currency> GetOneCurrency(int id); // BORRAR ¿? // YES
        Task<Currency> GetOneCurrency(string code);
        // ----- PUT -----
        Task<Currency> ModifyCurrency(int id, Currency currency); // BORRAR ¿?
        // ----- POST -----
        Task<Currency> CreateCurrency(Currency currency);
        Task<List<Currency>> CreateMultipleCurrencies(List<Currency> currencies);


        // -- Users --
        // ----- GET -----
        Task<User> GetUserById(int id); // BORRAR ¿? // YES
        Task<User> GetUserByEmail(string email);
        // ----- POST -----
        Task<User> CreateUser(User user);
        // ----- PUT -----
        Task<User> ModifyUser(UserForUpdateDto user);


        // -- Histories
        // ----- GET -----
        Task<List<History>> GetAllHistoriesForUser(int userId, int limit); // BORRAR ¿?       
        Task<List<History>> GetAllHistoriesForUser(string userEmail, int limit);
        // ----- POST -----
        Task<History> CreateHistory(History history); 
    }

    public class RepositoryOld : IRepositoryOld {

        CryptoZContext _context = new CryptoZContext();


        // -- Currencies --
        // ----- GET -----

        public async Task<List<Currency>> GetAllCurrencies() {
            var currencies = await _context.Currencies.ToListAsync() ?? throw new ArgumentNullException();
            return currencies;
        }

        public async Task<Currency> GetOneCurrency(int id) {
            Currency currency = await _context.Currencies.FirstOrDefaultAsync(c => c.Id == id) ??
                throw new ArgumentNullException("No existe la moneda parameter");

            return currency;
        }

        public async Task<Currency> GetOneCurrency(string code) {
            Currency currency = await _context.Currencies.FirstOrDefaultAsync(c => c.Code == code) ??
                throw new ArgumentNullException("No existe la moneda parameter");

            return currency;
        }

        // ----- POST -----
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

        public async Task<List<Currency>> CreateMultipleCurrencies(List<Currency> currencies) {

            List<Currency> newCurrencies = currencies.Where(x => !_context.Currencies.Any(y => y.Code == x.Code)).ToList();
            List<Currency> oldCurrencies = currencies.Where(x => _context.Currencies.Any(y => y.Code == x.Code)).ToList();

            await _context.Currencies.AddRangeAsync(newCurrencies);
            // TODO UPDATE

            await saveDB();

            return await _context.Currencies.ToListAsync();
        }

        // ----- PUT -----
        public async Task<Currency> ModifyCurrency(int id, Currency currency) {
            // (Luis) Asumo que solamente se deben cambiar Name, Price, PriceDate y LogoUrl

            var old_currency = await _context.Currencies.FirstOrDefaultAsync(c => c.Id == id);

            if (old_currency == null) {
                return null;
            }

            old_currency.Name = currency.Name;
            old_currency.Price = currency.Price;
            old_currency.PriceDate = currency.PriceDate;
            old_currency.LogoUrl = currency.LogoUrl;

            await saveDB();

            return currency;
        }



        // -- History --
        // ----- GET -----
        public async Task<List<History>> GetAllHistoriesForUser(int userId, int limit) {
            // TODO: Refactor

            if (limit != 0) {
                return await _context.Histories
                    .Where(h => h.UserId == userId)
                    .OrderByDescending(h => h.Date)
                    .Take((int)limit)
                    .ToListAsync();
            }

            return await _context.Histories
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.Date)
                .ToListAsync();
        }

        public async Task<List<History>> GetAllHistoriesForUser(string userEmail, int limit) {

            try {
                List<History> histories;

                if (limit > 0) {
                    histories = await _context.Histories
                        .Where(h => h.User.Email == userEmail)
                        .OrderByDescending(h => h.Date)
                        .Take((int)limit)
                        .ToListAsync();
                }
                else {
                    histories = await _context.Histories
                        .Where(h => h.User.Email == userEmail)
                        .OrderByDescending(h => h.Date)
                        .ToListAsync();
                }
                return histories;
            }
            catch (Exception e) {
                throw;
            }
        }

        // ----- POST -----
        public async Task<History> CreateHistory(History history) {
            try {
                history.Date = DateTime.Now;

                // Si no está logeado, no almacenar en la BBDD, devolver
                // directamente
                if (history.User == null) {
                    return history;
                }

                await _context.Histories.AddAsync(history);
                await saveDB();
                return history;
            }
            catch (Exception e) {
                throw;
            }

        }


        // -- User --
        // ----- GET -----
        public async Task<User> GetUserById(int id) {
            return await _context.Users.FirstAsync(u => u.Id == id); // Throws exception in case it's not found
        }

        public async Task<User> GetUserByEmail(string email) {
            User user = await _context.Users.FirstAsync(u => u.Email == email); // Throws exception in case it's not found
            return user;
        }

        // ----- POST -----
        public async Task<User> CreateUser(User user) {
            try {
                // Validate user doesn't exist       
                if (await _context.Users.AnyAsync(x => x.Email == user.Email)) {
                    throw new Exception($"User with the email {user.Email} already exists");
                }

                // Here we should map userDto to User

                // Hash password and generate salt
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                // Save user
                await _context.Users.AddAsync(user);

                await saveDB();

                return user;
            }
            catch (Exception e) {
                throw;
            }


        }

        // ----- PUT -----
        public async Task<User> ModifyUser(UserForUpdateDto newUser) {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
            user.Name = newUser.Name ?? user.Name;
            // TODO: BCrypt verify password w/ salt 
            await saveDB();
            return user;
        }

        // -- Database --
        // ------ Save -----
        private async Task saveDB() {
            await _context.SaveChangesAsync();
        }

    }
}