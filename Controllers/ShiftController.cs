using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RefreshAPIEF.ApiModels;
using RefreshAPIEF.Repository;

namespace RefreshAPIEF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly ClientRepository _clientRepository;
        private readonly DateTimeProcessing _dateTimeProcessing;
        private readonly CommonRepository _commonRepository;

        public ShiftController (ClientRepository clientRepository, DateTimeProcessing dateTimeProcessing, CommonRepository commonRepository)
        {
            _clientRepository = clientRepository;
            _dateTimeProcessing = dateTimeProcessing;
            _commonRepository = commonRepository;
        }

        /// <summary>
        /// Создаёт смену
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create (ShiftCreate s)
        {
            var dataClub = _clientRepository.GetClubIdAndTimazone( s.ApiKey);
            if (dataClub != null)
            {
                int UTC = dataClub["time_zone"];
                var todayWithUTCDT = _dateTimeProcessing.GetUtcTime(UTC);
                int financeId = _commonRepository.FinanceAdd(s.AdminId,dataClub["id"],s.Shift,s.CashBoxSerial, todayWithUTCDT);
                var response = new
                {
                    id = financeId
                };
                return new OkObjectResult(response);
            }            
            else
            {
                var response = new
                {
                    status = "Ошибка API-Key"
                };
                return new BadRequestObjectResult(response);
            }
        }

        /// <summary>
        /// Закрывает смену
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Close (ShiftClose s)
        {            
            var dataClub = _clientRepository.GetClubIdAndTimazone(s.ApiKey);
            if (dataClub != null)
            {
                int UTC = dataClub["time_zone"];
                var todayWithUTCDT = _dateTimeProcessing.GetUtcTime(UTC);
                var finance = _commonRepository.ShiftClose(s.Shift, s.AdminId, todayWithUTCDT);
                var response = new
                {
                    id = finance.Id
                };
                return new OkObjectResult(response);
            }
            else
            {
                var response = new
                {
                    status = "Ошибка в Finance"
                };
                return new BadRequestObjectResult(response);
            }
            
        }
    }
}
