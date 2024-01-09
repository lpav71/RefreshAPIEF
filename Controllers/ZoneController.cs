using Microsoft.AspNetCore.Mvc;
using RefreshAPIEF.ApiModels;
using RefreshAPIEF.Repository;

namespace RefreshAPIEF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ZoneController : ControllerBase
    {
        private readonly ClientRepository _clientRepository;
        private readonly CommonRepository _commonRepository;

        public ZoneController(ClientRepository clientRepository, CommonRepository commonRepository)
        {
            _clientRepository = clientRepository;
            _commonRepository = commonRepository;
        }

        /// <summary>
        /// Возвращает все игровые зоны
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetAllZone(ApiKey apiKey)
        {
            var clubId = _clientRepository.GetClubIdMobile(apiKey.apikey, apiKey.MobileApiKey);
            if (clubId != 0)
            {
                var zone = _commonRepository.GetZones(clubId);
                return new OkObjectResult(zone);
            }
            else
            {
                var error = new
                {
                    status = "Неверный API key"
                };
                return new OkObjectResult(error);
            }
        }
    }
}
