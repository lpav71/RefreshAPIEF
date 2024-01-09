#nullable disable
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RefreshAPIEF.Models;
using RefreshAPIEF.ApiModels;
using RefreshAPIEF.Repository;

namespace Ref2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class StoreController : ControllerBase
    {
        private readonly ClientRepository _clientRepository;
        private readonly CommonRepository _commonRepository;

        public StoreController(ClientRepository clientRepository, CommonRepository commonRepository)
        {
            _clientRepository = clientRepository;
            _commonRepository = commonRepository;
        }

        [HttpPost]
        public IActionResult StoreList(StoreStoreList s)
        {
            int clubId = _clientRepository.GetClubId(s.ApiKey);
            if (clubId == 0)
            {
                var outDataError = new
                {
                    cmd = "storelist",
                    error = "1",
                    data = "Клуб с таким api_key не существует"
                };
                return new BadRequestObjectResult(outDataError);
            }
            List<StoreAgg> store = new();
            try
            {
                store = _commonRepository.GetProduct(clubId);
            }
            catch (Exception)
            {
                return new NotFoundObjectResult(store);
            }

            var product = store[0].product;
            if (product == null)
            {
                var outDataError = new
                {
                    cmd = "storelist",
                    error = "0",
                    data = ""
                };
                return new OkObjectResult(outDataError);
            }
            var storeDecode = JsonConvert.DeserializeObject<List<StoreDecode>>(product);  //раскодировать json в объект

            var outData = new
            {
                cmd = "storelist",
                error = "0",
                data = storeDecode,
            };

            return new OkObjectResult(outData);
        }
    }
}