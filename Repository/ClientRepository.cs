using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using RefreshAPIEF.Data;
using RefreshAPIEF.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RefreshAPIEF.Repository
{
    public class ClientRepository
    {
        private readonly RefreshAPIEFContext _context;

        public ClientRepository(RefreshAPIEFContext context)
        {
            _context = context;
        }
        public IQueryable<Client>? GetClient(int clubId)
        {
            IQueryable<Client>? clients = _context.Client.Where(p => p.ClubId == clubId);
            return clients;
        }
        public Client? GetClientById(int id)
        {
            var client = _context.Client.FirstOrDefault(p => p.Id == id);
            return client;
        }
        public Client? GetClientByLogin(string login)
        {
            var client = _context.Client.FirstOrDefault(p => p.Login == login.ToLower());
            return client;
        }
        public Client RegisterNewClient(int clubId, string? login, string? password, string? phone, string? email, string? icon, double? amount, double? bonus, int? totalTime,
            string? fullName, bool? statusActive, string? telegramId, string? vkId, DateTime? bday, bool? verify, DateTime? verifyDt, string? name, string? surName, string? middleName)
        {
            Client clientNew = new();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                clientNew.ClubId = clubId;
                clientNew.Login = login.ToLower();
                clientNew.Password = passwordHash;
                clientNew.Phone = phone;
                clientNew.Email = email;
                clientNew.Icon = icon;
                clientNew.Amount = amount;
                clientNew.Bonus = bonus;
                clientNew.TotalTime = totalTime;
                clientNew.FullName = fullName;
                clientNew.StatusActive = statusActive;
                clientNew.TelegramId = telegramId;
                clientNew.VkId = vkId;
                clientNew.BDay = bday;
                clientNew.Verify = verify;
                clientNew.VerifyDt = verifyDt;
                clientNew.Name = name;
                clientNew.SurName = surName;
                clientNew.MiddleName = middleName;
                _context.Client.Attach(clientNew);
                _context.SaveChanges();

                ClientWallets clientWallets = new()
                {
                    club_id = clubId,
                    user_id = clientNew.Id,
                    bonus = 0,
                    money = 0,
                    create_date = DateTime.Now
                };
                _context.ClientWallets.Attach(clientWallets);
                _context.SaveChanges();

                transaction.Commit();
            }
            catch (DbUpdateException)
            {
                transaction.Rollback();
            }
            return clientNew;
        }
        public object GetClientFinance(int? clubId, int clientId)
        {
            var result = from c in _context.Cashbox
                         join u in _context.Users on c.admin_id equals u.id into adminJoin
                         from a in adminJoin.DefaultIfEmpty()
                         join cl in _context.Client on c.user_id equals cl.Id into clientJoin
                         from clj in clientJoin.DefaultIfEmpty()
                         join to in _context.TypeOperation on c.type_operation equals to.id into typeJoin
                         from tj in typeJoin.DefaultIfEmpty()
                         where c.club_id == clubId && (c.type_operation == 1 || c.type_operation == 2) && c.user_id == clientId
                         select new
                         {
                             Id = c.id,
                             Name = a.name,
                             clj.Login,
                             DtOperation = c.dt_operation,
                             Amount = c.amount,
                             OperationName = tj.name,
                             Shift = c.shift
                         };
            return result;
        }
        public IQueryable<Client>? FindClient(string sText, int clubId)
        {
            var clients = _context.Client.Where(p => EF.Functions.Like(p.Login.ToUpper(), "%" + sText + "%") || EF.Functions.Like(p.Phone.ToUpper(), "%" + sText + "%")
            || EF.Functions.Like(p.Email.ToUpper(), "%" + sText + "%") || EF.Functions.Like(p.TelegramId.ToUpper(), "%" + sText + "%") || EF.Functions.Like(p.VkId.ToUpper(), "%" + sText + "%")
            || EF.Functions.Like(p.FullName.ToUpper(), "%" + sText + "%") && p.ClubId == clubId);

            return clients;
        }
        public void DeleteClient(Client client)
        {
            _context.Client.Remove(client);
            _context.SaveChanges();
        }
        public List<StoreAgg> GetOperations(int clientId, int clubId, DateTime dateFrom, DateTime dateTo)
        {
            var result = _context.Set<StoreAgg>().FromSqlRaw(
                           "select json_agg(row_to_json(row(t1.amount , t1.dt_operation , t2.name))) from cashbox as t1 inner join type_operation as t2 on t1.type_operation = t2.id " +
                           "where user_id = {0} and club_id = {1} and t1.dt_operation >= {2} and t1.dt_operation <= {3}",
                           clientId, clubId, dateFrom, dateTo).ToList();
            return result;
        }
        public List<Purchase>? GetPurchases(DateTime timeStart, DateTime timeStop, int club_id)
        {
            List<Purchase>? result = _context.Set<Purchase>().FromSqlRaw(
                            "SELECT t1.id, json_agg(row_to_json(row(t1.clients, pay_type, t2.amount, t3.product, t3.product_param, t2.num, to_char(t1.created_at, 'DD.MM.YYYY HH24:MI'))))  AS purchase " +
                            "FROM app_pay AS t1 " +
                            "INNER JOIN app_pay_list AS t2 ON t1.id = t2.app_pay_id " +
                            "INNER JOIN store AS t3 ON t2.store_id = t3.id " +
                            "WHERE t1.created_at BETWEEN {0} AND {1} AND t1.club_id = {2} " +
                            "GROUP BY t1.id", timeStart, timeStop, club_id).ToList();
            return result;
        }
        public int VerifyUser(string login, string password)
        {
            var client = GetClientByLogin(login);
            if (client == null)
            {
                return -1;
            }
            bool passwordCorrect = BCrypt.Net.BCrypt.Verify(password, client.Password);
            if (!passwordCorrect)
            {
                return -2;
            }
            return client.Id;
        }
        public int GetClubId(string apiKey)
        {
            var club = _context.Club.FirstOrDefault(a => a.ApiKey == apiKey);
            if (club == null)
            {
                return 0;
            }
            int id = club.Id;
            return id;
        }
        public Club? GetClubById(int  clubId)
        {
            return _context.Club.FirstOrDefault(c => c.Id == clubId);
        }
        public int GetClubIdMobile(string apiKey, string mobileApikey)
        {
            Club? club = null;
            if (apiKey != null)
            {
                club = _context.Club.FirstOrDefault(a => a.ApiKey == apiKey);
            }
            if (mobileApikey != null && club == null)
            {
                club = _context.Club.FirstOrDefault(a => a.ApiKeyMobile == mobileApikey);
            }

            if (club == null)
            {
                return 0;
            }
            int id = club.Id;
            return id;
        }

        public int? GetPriceTariffType(int price_id)
        {
            var tariffType = _context.Price.FirstOrDefault(p => p.Id == price_id);
            return tariffType.TariffType;
        }

        public Dictionary<string, int> GetClubIdAndTimazone(string apiKey)
        {
            var club = _context.Club.FirstOrDefault(p => p.ApiKey == apiKey);
            if (club != null)
            {
                Dictionary<string, int> response = new()
                {
                    { "id", club.Id },
                    { "time_zone", (int)club.TimeZone }
                };
                return response;
            }
            else
            {
                return null;
            }
        }

        public int GetPriceDuration(int price_id)
        {
            var price = _context.Price.FirstOrDefault(p => p.Id != price_id);
            return (int)price.Duration;
        }
        public double GetAmount(string login)
        {
            var client = _context.Client.FirstOrDefault(p => p.Login == login);
            return (double)client.Amount;
        }

        public double GetBonus(string login)
        {
            var client = _context.Client.FirstOrDefault(p => p.Login == login);
            return (double)client.Bonus;
        }
        public bool CheckPassword(string? password, string? hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
