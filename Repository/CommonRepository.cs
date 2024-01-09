using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefreshAPIEF.Controllers;
using RefreshAPIEF.Data;
using RefreshAPIEF.Models;

namespace RefreshAPIEF.Repository
{
    public class CommonRepository
    {
        private readonly RefreshAPIEFContext _context;
        public CommonRepository(RefreshAPIEFContext context)
        {
            _context = context;
        }
        public int CreateCashBoxer(int clubId, int? userId, double? amount, int adminId, DateTime todayWithUTC, string? adminName, int? shift, int? typeOperation, 
            int? check, string? cashbox, double? old_amount, double? old_bonus, bool? status_check)
        {
            Cashbox cashboxer = new()
            {
                club_id = clubId,
                user_id = userId,
                amount = amount,
                admin_id = adminId,
                dt_operation = todayWithUTC,
                admin_name = adminName,
                shift = shift,
                type_operation = typeOperation,
                check = check,
                cashbox = cashbox,
                old_amount = old_amount,
                old_bonus = old_bonus,
                status_check = status_check
            };
            _context.Cashbox.Add(cashboxer);
            _context.SaveChanges();

            return cashboxer.id;
        }
        public void SaveFinance(int clubId, string type, double? amount)
        {
            var finances = _context.Finance.Where(f => f.ClubId == clubId).Where(p => p.Status == true).FirstOrDefault();
            if (finances != null)
            {
                switch (type)
                {
                    case "cash":
                        finances.CashNum++;
                        finances.Cash += amount;
                        break;
                    case "nocash":
                        finances.NocashNum++;
                        finances.Nocash += amount;
                        break;
                    case "bonus":
                        finances.BonusNum++;
                        finances.Bonus += amount;
                        break;
                }
                _context.Finance.Attach(finances);
                _context.SaveChanges();
            }
        }
        public int ClubCreate(string name, int idGroup, string address, string ip, string apiKey, string localIp, string cashbox, string cashboxPort, int maxBonus, int timeZone)
        {
            Club club = new()
            {
                Name = name,
                IdGroup = idGroup,
                Address = address,
                Ip = ip,
                ApiKey = apiKey,
                LocalIp = localIp,
                CashBox = cashbox,
                CashBoxPort = cashboxPort,
                MaxBonus = maxBonus,
                TimeZone = timeZone
            };

            _context.Club.Add(club);
            _context.SaveChanges();

            return club.Id;
        }
        public object ClubGetName()
        {
            var jsonResult = _context.Club
                .Select(c => new
                {
                    c.Name,
                    c.Address,
                    c.ApiKeyMobile,
                    c.phone
                })
                .ToList()
                .GroupBy(x => 1)
                .Select(g => new
                {
                    json_agg = g.Select(x => new
                    {
                        name = x.Name,
                        address = x.Address,
                        api_key_mobile = x.ApiKeyMobile,
                        x.phone
                    })
                })
                .FirstOrDefault();
            return jsonResult;
        }
        public int ClubSteamAdd(int clubId, string game, string steamId, string loginSteam, string passSteam, bool status)
        {
            ClubSteamAccount clubSteamAccount = new()
            {
                ClubId = clubId,
                Game = game,
                SteamId = steamId,
                LoginSteam = loginSteam,
                PassSteam = passSteam,
                Status = status
            };

            _context.ClubSteamAccount.Add(clubSteamAccount);
            _context.SaveChanges();
            return clubSteamAccount.Id;
        }
        public Dictionary<string, object> ClubSteamAccountViewOrUpdate(string steamId, int clubId, DateTime calulateTime)
        {
            Dictionary<string, object> okResponse = new();
            int id = 0;
            var clubSteamAccount = _context.ClubSteamAccount.FirstOrDefault(p => p.SteamId == steamId && p.ClubId == clubId && p.LastUpdate < calulateTime);
            if (clubSteamAccount != null)
            {
                okResponse["login_steam"] = clubSteamAccount.LoginSteam;
                okResponse["pass_steam"] = clubSteamAccount.PassSteam;
                id = clubSteamAccount.Id;
            }

            if (id != 0)
            {
                clubSteamAccount = _context.ClubSteamAccount.FirstOrDefault(p => p.Id == id);
                clubSteamAccount.LastUpdate = DateTime.UtcNow;

                _context.ClubSteamAccount.Attach(clubSteamAccount);
                _context.SaveChanges();
            }
            return okResponse;
        }
        public IQueryable<Game>? GetGameList(int clubId, int mapCompId)
        {
            return _context.Game.Where(p => p.ClubId == clubId && p.MapCompId == mapCompId);
        }
        public int GameUpdate(int id, string name, string description, string link, string icon, string param, int type, string steamId, bool clubAccount) 
        {
            var game = _context.Game.FirstOrDefault(p => p.Id == id);
            game.Name = name;
            game.Description = description;
            game.Link = link;
            game.Icon = icon;
            game.Param = param;
            game.Type = type;
            game.SteamId = steamId;
            game.ClubAccount = clubAccount;
            _context.Game.Attach(game);
            _context.SaveChanges();
            return game.Id;
        }
        public Finance? GetFinanceShifts(int clubId)
        {
            return _context.Finance.Where(f => f.ClubId == clubId && f.Status == true).FirstOrDefault();
        }
        public double? GetPrice(int id)
        {
            return _context.Store.Where(s => s.id == id).Select(s => s.price).FirstOrDefault();
        }
        public List<Store>? GetStores(List<int> ids)
        {
            return _context.Store.Where(g => ids.Contains(g.id)).ToList();
        } 
        public List<Store>? FindStore(List<int> storeIds)
        {
            return _context.Store.Where(c => storeIds.Contains(c.id)).ToList();
        }
        public void CreateApp(int userId, int clubId, double? amount, int? adminId, int paytype, bool? delivery, int nums, int? shift, int compId, List<Amount> amountList, List<Datum> data)
        {
            var appPay = new AppPay
            {
                clients = userId,
                club_id = clubId,
                status = true,
                amount = amount,
                created_at = DateTime.Now,
                admin_id = adminId,
                pay_datetime = DateTime.Now,
                pay_type = paytype,
                delivery = delivery,
                delivery_complete = true,
                product_num = nums,
                shift = shift,
                comp_id = compId
            };

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.AppPay.Add(appPay);
                _context.SaveChanges();

                foreach (var aList in amountList)
                {
                    AppPayList pay = new()
                    {
                        app_pay_id = appPay.id,
                        amount = aList.price,
                        num = aList.num,
                        store_id = aList.id
                    };
                    _context.AppPayList.Add(pay);
                }
                _context.SaveChanges();

                foreach (var d in data)
                {
                    var storeOut = new StoreOut
                    {
                        club_id = clubId,
                        store_id = d.id,
                        admin = adminId,
                        num = d.num,
                        dateout = DateTime.Now,
                        store_operation_type_id = 1,
                        app_id = appPay.id
                    };
                    _context.StoreOut.Add(storeOut);
                }
                _context.SaveChanges();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                var Error = new
                {
                    status = ex.InnerException?.Message
                };
            }
        }
        public void DebitingFunds(Client? clientFunds, double? amount, int paytype)
        {
            if (paytype == 0) //Деньги
            {
                if (clientFunds != null)
                {
                    clientFunds.Amount = clientFunds?.Amount - amount;
                    _context.SaveChanges();
                }
            }
            if (paytype == 1) //Бонусы
            {
                if (clientFunds != null)
                {
                    clientFunds.Amount = clientFunds?.Bonus - amount;
                    _context.SaveChanges();
                }
            }
        }
        public Store? GetStoreById(List<Store>? stores,int Id)
        {
            return stores.FirstOrDefault(s => s.id == Id);
        }
        public IQueryable<Price>? GetAvailableTariffs(TimeOnly todayWithUTC, int idZone, int dweek, int clubId)
        {
            return _context.Price.Where(c => c.TimeStart <= todayWithUTC && c.TimeStop > todayWithUTC && c.IdZone == idZone && c.WeekDay == dweek && c.ClubId == clubId && c.StatusActive == true);
        }
        public int AddPrice(int clubId, int zoneId, double price1, int weekday, int duration, int tariffType, bool statusActive, TimeOnly timeStart, TimeOnly timeStop, TimeOnly timeFixed, string name)
        {
            Price price = new()
            {
                ClubId = clubId,
                IdZone = zoneId,
                Price1 = price1,
                WeekDay = weekday,
                Duration = duration,
                TariffType = tariffType,
                StatusActive = statusActive,
                TimeStart = timeStart,
                TimeStop = timeStop,
                TimeFixed = timeFixed,
                Name = name
            };
            _context.Price.Add(price);
            _context.SaveChanges();
            return price.Id;
        }
        public List<StoreAgg> GetPriceToWeekDay(int clubId, int? weekDay)
        {
            return _context.Set<StoreAgg>().FromSqlRaw("select  json_agg(row_to_json(row(price, id_zone, duration,tariff_type,time_start , time_stop, time_fixed , name, id))) " +
                "from price where club_id = {0} and week_day = {1}", clubId, weekDay).ToList();
        }
        public Promo? GetPromoById(int id)
        {
            return _context.Promo.FirstOrDefault(e => e.id == id);
        }
        public void PromoActivations(Promo? promo)
        {
            if (promo != null)
            {
                int? activations = promo.activations;
                if (activations != null)
                {
                    activations++;
                    promo.activations = activations;
                    _context.SaveChanges();
                }
            }
        }
        public Users? GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.email == email);
        }
        public int FinanceAdd(int adminId, int clubId, int shift, string cashBoxSerial, DateTime todayWithUTCDT)
        {
            Finance finance = new()
            {
                AdminId = adminId,
                ClubId = clubId,
                Shift = shift,
                CashBoxSerial = cashBoxSerial,
                Status = true,
                OpenShift = todayWithUTCDT,
                DtCreate = todayWithUTCDT
            };
            _context.Finance.Add(finance);
            _context.SaveChanges();
            return finance.Id;
        }
        public Finance? ShiftClose(int shift, int adminId, DateTime todayWithUTCDT)
        {
            var finance = _context.Finance.FirstOrDefault(p => p.Status == true && p.Shift == shift && p.AdminId == adminId);
            if (finance != null)
            {
                finance.Status = false;
                finance.CloseShift = todayWithUTCDT;
                _context.Finance.Attach(finance);
                _context.SaveChanges();                
            }
            return finance;
        }
        public GameStatistic? GetStatistic(int bookingId, int gameId, int clientId, int clubId)
        {
            return _context.GameStatistic
                .Where(gs => gs.booking_id == bookingId && gs.games_id == gameId && gs.clients_id == clientId && gs.club_id == clubId)
                .FirstOrDefault();
        }
        public void AddGameStatistic(int bookingId, int gameId, int clientId, int clubId, GameStatistic? gameStatistic)
        {
            if (gameStatistic == null)
            {
                var gameStatisticNew = new GameStatistic
                {
                    booking_id = bookingId,
                    games_id = gameId,
                    clients_id = clientId,
                    club_id = clubId
                };
                _context.GameStatistic.Add(gameStatisticNew);
                _context.SaveChanges();
            }
            else
            {
                gameStatistic.duration_id += 1;
                _context.SaveChanges();                
            }            
        }
        public List<StoreAgg> GetProduct(int clubId)
        {
            return _context.Set<StoreAgg>().FromSqlRaw("select json_agg(row_to_json(row(id, product, product_param, icon, discount, price))) as product from store where club_id={0} and shell_show=true", clubId).ToList();
        }
        public IOrderedQueryable<Zone> GetZones(int clubId)
        {
            return _context.Zone.Where(p => p.club_id == clubId).OrderBy(p => p.num);
        }
    }
#pragma warning disable IDE1006 // Стили именования
    public class Datum
    {
        public int id { get; set; }
        public int num { get; set; }
    }
}
