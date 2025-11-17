using Microsoft.EntityFrameworkCore;
using RefreshAPIEF.Data;
using RefreshAPIEF.Models;

namespace RefreshAPIEF.Repository
{
    public class BookingRepository
    {
        private readonly RefreshAPIEFContext _context;
        public BookingRepository(RefreshAPIEFContext context)
        {
            _context = context;
        }
        public List<StoreAgg>? GetBooking(int clubId, DateTime timeStart, DateTime timeStop)
        {
            var result = _context.Set<StoreAgg>().FromSqlRaw(
                "select json_build_object('data',json_agg(row_to_json(row(t1.map_comp_id, t1.id_zone, t1.user_id ,t1.tariff_type , t1.status , t1.session_pause, t1.time_start, t1.time_stop)))) " +
                "from booking as t1 where t1.club_id={0} and t1.time_start>{1} and t1.time_stop<{2}", clubId, timeStart, timeStop).ToList();
            return result;
        }
        public IQueryable<Price> VerifyTariff(TimeOnly timeOnlyStart, int clubId, int weekday, int mapCompId, int priceId)
        {
            //SQL запрос
            //SELECT p.id, p.id_zone, p.week_day, p.time_start, p.time_stop, p.time_fixed, p.duration
            //FROM Price p
            //INNER JOIN Map t2 ON p.club_id = t2.club_id
            //WHERE p.id = 35 AND p.id_zone = t2.zone AND t2.id_comp = 2
            //AND p.time_start < '19-10-2023 15:44:23' AND p.time_stop > '19-10-2023 15:44:23'
            //AND p.club_id = 2 AND p.week_day = 4;   
            var query = from p in _context.Price
                        join t2 in _context.Map on p.ClubId equals t2.ClubId
                        where p.Id == priceId && p.IdZone == t2.Zone && t2.IdComp == mapCompId
                              && p.TimeStart < timeOnlyStart && p.TimeStop > timeOnlyStart
                              && p.ClubId == clubId && p.WeekDay == weekday
                        select p;
            return query;
        }
        public IQueryable<VerifyFinanceData> VerifyFinance(int priceId, string login)
        {
            IQueryable<VerifyFinanceData> money = (IQueryable<VerifyFinanceData>)(from t1 in _context.Client
                                              join t2 in _context.Price on t1.ClubId equals t2.ClubId into temp
                        from t3 in temp.DefaultIfEmpty()
                        where t3.Id == priceId && t1.Login == login && t1.Amount >= t3.Price1
                        select new { t1.Amount, t3.Price1 });
            return money;
        }
        public List<Booking> VerifyReserv(int clubId, int mapCompId, DateTime timeStart, DateTime newFullDate)
        {
            var reserv = _context.Booking.Where(t =>
                                (t.ClubId == clubId && t.MapCompId == mapCompId && t.TimeStart >= timeStart && t.TimeStop >= newFullDate)
                                || (t.ClubId == 1 && t.MapCompId == 1 && t.TimeStart <= timeStart && t.TimeStop >= newFullDate)
                                || (t.ClubId == 1 && t.MapCompId == 1 && t.TimeStart <= timeStart && t.TimeStop <= newFullDate)
                                || (t.ClubId == 1 && t.MapCompId == 1 && t.TimeStart >= timeStart && t.TimeStop <= newFullDate)
                            ).ToList();
            return reserv;
        }
        public void AddToBookingWithAmount(int clientId, int clubId, int mapCompId, int priceId, DateTime timeStart, DateTime newFullDate, int? id_zone, TimeSpan res)
        {
            var newBooking = new Booking
            {
                UserId = clientId,
                ClubId = clubId,
                MapCompId = mapCompId,
                PriceId = priceId,
                Status = -1,
                TimeStart = timeStart,
                TimeStop = newFullDate,
                IdZone = id_zone,
                Res = (int)res.TotalMinutes,
                Duration = 0,
                Amount = 0,
                Bonus = 0,
                SessionPause = 0,
            };
            _context.Booking.Add(newBooking);
            _context.SaveChanges();
            string subAmount = string.Format("update clients SET amount=amount-price.price FROM price where clients.id={0};", clientId);
            _context.Database.ExecuteSqlRaw(subAmount);
        }
        public void AddToBooking(int user_id, int priceId, int clubId, int mapCompId, int tariffType, double bonus, double amount, DateTime timeStart, DateTime timeStop)
        {
            Booking booking = new()
            {
                UserId = user_id,
                PriceId = priceId,
                ClubId = clubId,
                MapCompId = mapCompId,
                TariffType = tariffType,
                StartAmount = amount,
                Bonus = bonus,
                TimeStart = timeStart.ToUniversalTime(),
                TimeStop = timeStop.ToUniversalTime(),
                TimeUpdate = timeStart.ToUniversalTime(),
            };
            _context.Booking.Add(booking);
            _context.SaveChanges();
        }
        public IQueryable<Statistic> GetStatistic(int clubId, DateTime timeStart, DateTime timeStop, int idZone, int tariffType)
        {
            IQueryable<Statistic> results = (IQueryable<Statistic>)(from b in _context.Booking
                          join p in _context.Price on b.PriceId equals p.Id
            where b.ClubId == clubId && b.TimeStart > timeStart && b.TimeStart < timeStop && p.IdZone == idZone && p.TariffType == tariffType
            group new { b.IdZone, p.Id, p.Name, p.TariffType, p.WeekDay } by new { b.IdZone, p.Name, p.TariffType, p.WeekDay } into g
            select new
            {
                g.Key.IdZone,
                Count = g.Count(),
                              g.Key.Name,
                              g.Key.TariffType,
                              g.Key.WeekDay
                          });
            return results;
        }
        public List<Booking> BusyComp(int clubId, DateTime timeStart, DateTime timeStop) 
        {
            var busyComp = _context.Booking
                            .Where(c => c.ClubId == clubId &&
                                ((c.TimeStart >= timeStart && c.TimeStop <= timeStop) ||
                                (c.TimeStart <= timeStart && c.TimeStop >= timeStop) ||
                                (c.TimeStart >= timeStart && c.TimeStop >= timeStop) ||
                                (c.TimeStart <= timeStart && c.TimeStop >= timeStart)))
                            .ToList();
            return busyComp;
        }
        public List<StoreAgg> NextBookingUser(int clubId, int? userId)
        {
            return _context.Set<StoreAgg>().FromSqlRaw("select json_agg(row_to_json(row(t1.map_comp_id , t1.res ,t1.time_start , t1.time_stop, t2.name, t2.price))) " +
                "from booking as t1 left join price as t2 on t1.price_id = t2.id where t1.club_id={0} and t1.status =-1 and t1.user_id={1}", clubId, userId).ToList();
        }
        public List<StoreAgg> NextBooking(int clubId)
        {
            return _context.Set<StoreAgg>().FromSqlRaw("select json_agg(row_to_json(row(t1.map_comp_id , t1.res ,t1.time_start , t1.time_stop, t2.name, t2.price))) " +
                "from booking as t1 left join price as t2 on t1.price_id = t2.id where t1.club_id={0} and t1.status =-1", clubId).ToList();
        }
        public int? GetIdZone(int clubId, int mapCompId) 
        {
            return _context.Map.Where(m => m.ClubId == clubId && m.IdComp == mapCompId).Select(m => m.Zone).FirstOrDefault();
        }
    }
    public class VerifyFinanceData
    {
        public double? Amount { get; set; }
        public double? Price1 { get; set; }
    }
    public class Statistic
    {
        public int IdZone { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public int? TariffType { get; set; }
        public int? WeekDaay { get; set; }
    }

}
