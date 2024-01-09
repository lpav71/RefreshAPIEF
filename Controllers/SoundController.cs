using Microsoft.AspNetCore.Mvc;
using RefreshAPIEF.ApiModels;
using RefreshAPIEF.Repository;

namespace RefreshAPIEF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoundController : ControllerBase
    {
        private readonly ClientRepository _clientRepository;

        public SoundController(ClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        /// <summary>
        /// Возвращает все имена файлов звуковых оповещений
        /// </summary>
        /// <param name="apikey"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AllSounds(ApiKeyOne apikey)
        {
            var clubId = _clientRepository.GetClubId(apikey.apikey);
            var club = _clientRepository.GetClubById(clubId);
            var response = new
            {
                club.sound1,
                club.sound2,
                club.sound3,
                club.sound4,
                club.sound5,
                club.sound6,
                club.sound7,
                club.sound8,
                club.sound9,
            };
            return new OkObjectResult(response);
        }            
    }
}
