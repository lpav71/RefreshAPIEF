using Microsoft.AspNetCore.Mvc;
using RefreshAPIEF.Data;
using RefreshAPIEF.Models;
using RefreshAPIEF.ApiModels;
using RefreshAPIEF.Repository;

namespace RefreshAPIEF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MapsController : ControllerBase
    {
        private readonly ClientRepository _clientRepository;
        private readonly MapRepository _mapRepository;

        public MapsController(ClientRepository clientRepository, MapRepository mapRepository)
        {
            _clientRepository = clientRepository;
            _mapRepository = mapRepository;
        }

        /// <summary>
        /// Добавляет новый компьютер
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PcCreate(Map map)
        {
            var clubId = _clientRepository.GetClubId(map.ApiKey);
            Map computer = _mapRepository.GetComp(clubId, map.IdComp);
            var response = new
            {
                id = 0
            };
            if (computer != null)
            {
                _mapRepository.UpdateComp(computer, map, clubId);               
                response = new
                {
                    id = computer.Id
                };
            }
            else
            {
                Map newComputer = _mapRepository.CreateComp(map, clubId);                
                response = new
                {
                    id = newComputer.Id
                };
            }            
            return new OkObjectResult(response);
        }

        [HttpPost]
        public ActionResult PcInfo(MapPcInfo mapPcInfo)
        {
            var clubId = _clientRepository.GetClubId(mapPcInfo.Api_key);            
            var data = _mapRepository.GenerateNewQR(mapPcInfo.Mac, clubId);
            return new OkObjectResult(data);
        }

        [HttpPost]
        public IActionResult GetMap (ApiKey a)
        {
            var clubId = _clientRepository.GetClubIdMobile(a.apikey, a.MobileApiKey);
            if (clubId == 0)
            {
                var error = new
                {
                    status = "Неверный API KEY"
                };
                return new BadRequestObjectResult(error);
            }
            List<StoreAgg>? result = _mapRepository.GetMap(clubId);
            return new NotFoundObjectResult(result);
            
        }

        [HttpPost]
        public IActionResult GetDataForQR(MapQr m)
        {
            var clubId = _clientRepository.GetClubId(m.ApiKey);
            string? result;
            if (clubId != 0)
            {
                result = _mapRepository.GetDataForQR(clubId, m.CompId);
            }
            else
            {
                var error = new
                {
                    status = "Неверный API KEY"
                };
                return new BadRequestObjectResult(error);
            }
            if (result != null)
            {
                var response = new
                {
                    cmd = "qrcode",
                    error = "0",
                    data = result
                };
                return new OkObjectResult(response);
            }
            else
            {
                var response = new
                {
                    error = "1"
                };
                return new BadRequestObjectResult(response);
            }
        }

        [HttpPost]
        public IActionResult CustomClub (ApiKeyOne a)
        {
            var clubId = _clientRepository.GetClubId(a.apikey);
            List<StoreAgg>? result = null;
            if (clubId != 0)
            {
                result = _mapRepository.GetClubConfig(clubId);
            }
            else
            {
                var error = new
                {
                    status = "Неверный API KEY"
                };
                return new BadRequestObjectResult(error);
            }
            if (result != null)
            {
                var response = new
                {
                    error = "0",
                    setting = result[0].product
                };
                return new OkObjectResult(response);
            }
            else
            {
                var response = new
                {
                    error = "1"
                };
                return new BadRequestObjectResult(response);
            }
        }
    }
}
