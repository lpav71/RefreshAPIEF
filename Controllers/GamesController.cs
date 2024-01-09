using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefreshAPIEF.ApiModels;
using RefreshAPIEF.Data;
using RefreshAPIEF.Models;
using RefreshAPIEF.Repository;

namespace RefreshAPIEF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly CommonRepository _commonRepository;
        private readonly ClientRepository _clientRepository;

        public GamesController(CommonRepository commonRepository, ClientRepository clientRepository)
        {
            _commonRepository = commonRepository;
            _clientRepository = clientRepository;
        }

        /// <summary>
        /// Отображает список доступных игр
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GameList(GamesGameList g)
        {
            var clubId = _clientRepository.GetClubId(g.ApiKey);
            var games = _commonRepository.GetGameList(clubId, g.mapCompId);
            return new OkObjectResult(games);
        }

        [HttpPost]
        public IActionResult Update(GamesUpdate g)
        {
            var gameId =  _commonRepository.GameUpdate(g.Id, g.Name, g.Description, g.Link, g.Icon, g.Param, g.Type, g.SteamId, g.ClubAccount);
            var response = new
            {
                id = gameId
            };
            return new OkObjectResult(response);
        }
    }
}
