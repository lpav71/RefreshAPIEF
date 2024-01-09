using Microsoft.AspNetCore.Mvc;
using RefreshAPIEF.ApiModels;
using RefreshAPIEF.Models;
using RefreshAPIEF.Repository;

namespace RefreshAPIEF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly CommonRepository _commonRepository;
        private readonly ClientRepository _clientRepository;

        public PriceController (CommonRepository commonRepository, ClientRepository clientRepository)
        {
            _commonRepository = commonRepository;
            _clientRepository = clientRepository;
        }
        /// <summary>
        /// Добавляет тариф
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add (PriceAdd p)
        {
            var clubId = _clientRepository.GetClubId(p.ApiKey);
            if (clubId != 0)
            {
                var priceId = _commonRepository.AddPrice(clubId, p.ZoneId,p.pPrice, p.Weekday, p.Duration, p.TariffType,
                    p.StatusActive, p.TimeStart, p.TimeStop, p.TimeFixed, p.Name);
                var response1 = new
                {
                    id = priceId
                };
                return new OkObjectResult(response1);
            }
            var response = new
            {
                status = "Ошибка API KEY"
            };
            return new OkObjectResult(response);
        }

        /// <summary>
        /// Отображает доступные тарифы
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetAvailableTariffs(PriceGetTariffs p)
        {
            var dataClub = _clientRepository.GetClubIdAndTimazone(p.ApiKey);
            int id = dataClub["id"];
            int UTC = dataClub["time_zone"];
            var today = DateTime.Now;
            var todayWithUTC = TimeOnly.FromDateTime(today.AddMinutes(UTC));
            var todayWithUTCDT = today.AddMinutes(UTC);
            int dweek = (int)todayWithUTCDT.DayOfWeek;
            if (dweek == 0)
            {
                dweek = 7;
            }
            var price = _commonRepository.GetAvailableTariffs(todayWithUTC, p.IdZone,  dweek, id);
            return new OkObjectResult(price);
        }
        /// <summary>
        /// Отображает тарифы на определенный день недели
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult WeekDayPrice(PriceWeekDayPrice p)
        {
            var clubId = _clientRepository.GetClubIdMobile( p.ApiKey, p.MobileApiKey);
            if (clubId != 0)
            {
                List<StoreAgg> result = new();
                try
                {
                    result = _commonRepository.GetPriceToWeekDay(clubId, p.WeekDay);
                    return new OkObjectResult(result[0].product);
                }
                catch (Exception)
                {
                    return new NotFoundObjectResult(result);
                }
            }
            var error = new
            {
                status = "Неверный API KEY"
            };
            return new BadRequestObjectResult(error);
        }
    }
}
