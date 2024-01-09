using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefreshAPIEF.Data;
using RefreshAPIEF.ApiModels;
using RefreshAPIEF.Models;
using RefreshAPIEF.Repository;

namespace RefreshAPIEF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CashboxController : ControllerBase
    {
        private readonly CommonRepository _commonRepository;
        private readonly ClientRepository _clientRepository;
        private readonly DateTimeProcessing _dateTimeProcessing;


        public CashboxController(CommonRepository commonRepository, ClientRepository clientRepository, DateTimeProcessing dateTimeProcessing)
        {
            _commonRepository = commonRepository;
            _clientRepository = clientRepository;
            _dateTimeProcessing = dateTimeProcessing;
        }

        [HttpPost]
        public IActionResult CreateCheck(CashboxCreateCheck c)
        {
            int clubId = _clientRepository.GetClubId(c.ApiKey);
            var dataClub = _clientRepository.GetClubIdAndTimazone(c.ApiKey);
            int UTC = dataClub["time_zone"];
            var todayWithUTC = _dateTimeProcessing.GetUtcTime(UTC);
            int? userId = 0;
            if (c.user_id == null || c.user_id == 0)
            {
                userId = null;
            }
            var cashboxerId = _commonRepository.CreateCashBoxer(clubId, userId, c.amount,c.admin_id, todayWithUTC, c.admin_name, 
                c.shift,c.type_operation, c.check, c.cashbox, c.old_amount, c.old_bonus, c.status_check);
            _commonRepository.SaveFinance(clubId, c.type, c.amount);            
            var response = new
            {
                status = "Ok",
                id = cashboxerId
            };
            return new OkObjectResult(response);
        }
    }
}
