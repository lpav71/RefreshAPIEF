using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using RefreshAPIEF.ApiModels;
using RefreshAPIEF.Repository;

namespace RefreshAPIEF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly ClientRepository _clientRepository;
        private readonly CommonRepository _commonRepository;

        public StatisticController(ClientRepository clientRepository, CommonRepository commonRepository)
        {
            _clientRepository = clientRepository;
            _commonRepository = commonRepository;
        }

        [HttpPost]
        public IActionResult Game(StatisticGame s)
        {
            var clubId = _clientRepository.GetClubId(s.ApiKey);
            if (clubId == 0)
            {
                var error = new
                {
                    error = "Клуб не найден"
                };
                return new NotFoundObjectResult(error);
            }            
            var gameStatistic = _commonRepository.GetStatistic(s.BookingId, s.GameId, s.ClientId, clubId);
            _commonRepository.AddGameStatistic(s.BookingId, s.GameId, s.ClientId, clubId,gameStatistic);
            var Ok = new
            {
                status = "Выполнено"
            };
            return new OkObjectResult(Ok);
        }
    }
}
