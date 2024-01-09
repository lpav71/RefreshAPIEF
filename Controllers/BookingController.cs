#nullable disable
using Microsoft.AspNetCore.Mvc;
using RefreshAPIEF.Models;
using RefreshAPIEF.ApiModels;
using RefreshAPIEF.Repository;

namespace RefreshAPIEF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingRepository _bookingRepository;
        private readonly ClientRepository _clientRepository;
        private readonly DateTimeProcessing _dateTimeProcessing;

        public BookingController(BookingRepository bookingRepository, ClientRepository clientRepository, DateTimeProcessing dateTimeProcessing)
        {
            _bookingRepository = bookingRepository;
            _clientRepository = clientRepository;
            _dateTimeProcessing = dateTimeProcessing;
        }

        [HttpPost]
        public IActionResult GetBooking(BookingGetBooking b)
        {
            var client = _clientRepository.GetClientByLogin(b.Login.ToLower());
            if (client != null)
            {
                bool passwordCorrect = _clientRepository.CheckPassword(b.Password, client.Password);
                if (passwordCorrect)
                {
                    var clubId = _clientRepository.GetClubId(b.ApiKey);
                    var timeStart = _dateTimeProcessing.ParseDateTime(b.TimeStart);
                    var timeStop = _dateTimeProcessing.ParseDateTime(b.TimeStop);
                    List<StoreAgg> result = new();
                    try
                    {
                        result = _bookingRepository.GetBooking(clubId, timeStart, timeStop);
                    }
                    catch (Exception)
                    {
                        return new NotFoundObjectResult(result);
                    }
                    return new OkObjectResult(result[0].product);
                }
            }
            return new UnauthorizedResult();
        }

        /// <summary>
        /// Создаёт резервацию
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult BookingCreate (BookingBookingCreate b)
        {
            var client = _clientRepository.GetClientByLogin(b.Login.ToLower());
            if (client != null)
            {
                bool passwordCorrect = _clientRepository.CheckPassword(b.Password, client.Password);
                if (passwordCorrect)
                {
                    var clubId = _clientRepository.GetClubIdMobile(b.ApiKey, b.MobileApiKey);
                    if (clubId != 0)
                    {
                        var timeStart = _dateTimeProcessing.ParseDateTime(b.DateTimeStart);
                        if (timeStart.Date < DateTime.Today)
                        {
                            var oldDate = new
                            {
                                status = "Старая дата"
                            };
                            return new BadRequestObjectResult(oldDate);
                        }
                        var timeOnlyStart = new TimeOnly(timeStart.Hour, timeStart.Minute, timeStart.Second);
                        int weekday = (int)timeStart.DayOfWeek;

                        // есть ли в данное время в данном клубе и в данной зоне выбранный нами тариф. если возвращено 0 записей то возвращаем ошибку                                             
                        var query = _bookingRepository.VerifyTariff(timeOnlyStart, clubId, weekday, b.MapCompId, b.PriceId);
                        if (!query.Any())
                        {
                            var notTariff = new
                            {
                                status = "Не найден тариф"
                            };
                            return new BadRequestObjectResult(notTariff);
                        }
                        //теперь нам надо проверить есть ли у клиента столько денег
                        var money = _bookingRepository.VerifyFinance(b.PriceId, b.Login);
                        var myMoney = money.ToList()[0];
                        var duration = Convert.ToDouble(query.ToList()[0].Duration);
                        if (myMoney.Amount >= myMoney.Price1)
                        {
                            DateTime newFullDate = new();
                            if (duration > 0)
                            {
                                newFullDate = timeStart.AddMinutes(duration);
                            }
                            else
                            {
                                //Высчтываем разницу времён чтоб узнать попадает ли дата на следующий день
                                var timeFixed = query.ToList()[0].TimeFixed.ToString();
                                var timeFixed2 = TimeSpan.Parse(timeFixed);
                                var currentTime = timeStart.TimeOfDay;                                
                                if (timeFixed2 < currentTime) //Дата переходит на следующий день
                                {
                                    var newData = timeStart.AddDays(1);
                                    newFullDate = new DateTime(newData.Year, newData.Month, newData.Day, timeFixed2.Hours, timeFixed2.Minutes, timeFixed2.Seconds);
                                }
                                else
                                {
                                    newFullDate = new DateTime(timeStart.Year, timeStart.Month, timeStart.Day, timeFixed2.Hours, timeFixed2.Minutes, timeFixed2.Seconds);
                                }
                            }
                            //Проверяем резерв
                            var reserv = _bookingRepository.VerifyReserv(clubId, b.MapCompId, timeStart, newFullDate);                            
                            if(reserv.Count == 0)
                            {
                                //Получаем id_zone
                                var id_zone = _bookingRepository.GetIdZone(clubId, b.MapCompId);
                                var res = newFullDate.Subtract(timeStart);
                                //Добавляем запись в booking
                                if (id_zone != null)
                                {
                                    _bookingRepository.AddToBookingWithAmount(client.Id, clubId, b.MapCompId, b.PriceId, timeStart, newFullDate, id_zone, res);
                                }
                                var Success = new
                                {
                                    status = "Резервация добавлена"
                                };
                                return new OkObjectResult(id_zone);
                            }
                            else
                            {
                                var Success = new
                                {
                                    status = "Резервация уже есть"
                                };
                                return new OkObjectResult(Success);
                            }
                        }                        
                    }
                    else
                    {
                        var error = new
                        {
                            status = "Неверный API KEY"
                        };
                        return new BadRequestObjectResult(error);
                    }                    
                }
                return new UnauthorizedResult();
            }
            return new UnauthorizedResult();
        }

        /// <summary>
        /// Создаёт новую сессию
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateNewSession(BookingCreateNewSession b)
        {  
            int duraion = _clientRepository.GetPriceDuration(b.PriceId);
            var dataClub = _clientRepository.GetClubIdAndTimazone(b.ApiKey);
            int id = dataClub["id"];
            int UTC = dataClub["time_zone"];
            var todayWithUTC = _dateTimeProcessing.GetUtcTime(UTC);
            var todayWithUTCWithDuration = todayWithUTC.AddMinutes(duraion);

            Client client = _clientRepository.GetClientByLogin(b.Login);
            int user_id = client.Id;
            double amount = _clientRepository.GetAmount(b.Login);
            double bonus = _clientRepository.GetBonus(b.Login);
            int TariffType = (int)_clientRepository.GetPriceTariffType(b.PriceId);
            _bookingRepository.AddToBooking(user_id, b.PriceId, id, b.MapCompId, TariffType, bonus, amount, todayWithUTC.ToUniversalTime(), todayWithUTCWithDuration.ToUniversalTime());            
            var response = new
            {
                status = "Ok"
            };
            return new OkObjectResult(response);            
        }

        [HttpPost]
        public IActionResult Statistic (BookingStatistic statistic)
        {
            var timeStart = _dateTimeProcessing.ParseDateTime(statistic.TimeStart);
            var timeStop = _dateTimeProcessing.ParseDateTime(statistic.TimeStop);
            var results = _bookingRepository.GetStatistic(statistic.ClubId, timeStart, timeStop, statistic.IdZone,statistic.TariffType);            
            return new OkObjectResult(results);
        }

        [HttpPost]
        public IActionResult BusyComp(BookingBusyComp b)
        {
            var timeStart = _dateTimeProcessing.ParseDateTime(b.dateTimeStart);
            var timeStop = _dateTimeProcessing.ParseDateTime(b.dateTimeEnd);
            var client = _clientRepository.GetClientByLogin(b.login.ToLower());
            if (client != null)
            {
                bool passwordCorrect = _clientRepository.CheckPassword(b.password, client.Password);
                if (passwordCorrect)
                {
                    int clubId = _clientRepository.GetClubIdMobile(b.apiKey, b.apiKeyMobile);
                    if (clubId != 0)
                    {
                        var busyComp = _bookingRepository.BusyComp(clubId, timeStart, timeStop);
                        return new OkObjectResult(busyComp);
                    }
                    else
                    {
                        var error = new
                        {
                            status = "Неверный API KEY"
                        };
                        return new BadRequestObjectResult(error);
                    }
                }
                return new UnauthorizedResult();
            }
            return new UnauthorizedResult();
        }

        [HttpPost]
        public IActionResult Nextbooking(BookingNextbooking c)
        {
            int clubId = _clientRepository.GetClubIdMobile(c.apikey, c.MobileApiKey);
            if (clubId != 0)
            {
                List<StoreAgg> result = new();
                try
                {
                    if (c.UserId != null)
                    {
                        result = _bookingRepository.NextBookingUser(clubId, c.UserId);
                    }
                    else
                    {
                        result = _bookingRepository.NextBooking(clubId);
                    }
                }
                catch (Exception)
                {
                    return new NotFoundObjectResult(result);
                }
                return new OkObjectResult(result[0].product);
            }
            else
            {
                var error = new
                {
                    error = "Клуб не найден"
                };
                return new NotFoundObjectResult(error);
            }
        }
    }
    public class AllBookings
    {
        public int ClubId { get; set; }
    }
}
