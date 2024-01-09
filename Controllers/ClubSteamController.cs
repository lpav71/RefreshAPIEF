#nullable disable
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using RefreshAPIEF.ApiModels;
using RefreshAPIEF.Data;
using RefreshAPIEF.Models;
using RefreshAPIEF.Repository;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RefreshAPIEF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClubSteamController : ControllerBase
    {
        private readonly CommonRepository _commonRepository;
        private readonly ClientRepository _clientRepository;

        public ClubSteamController(CommonRepository commonRepository, ClientRepository clientRepository)
        {
            _commonRepository = commonRepository;
            _clientRepository = clientRepository;
        }

        /// <summary>
        /// Добавляет steam аккаунт
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(ClubSteamAdd c)
        {
            int clubId = _clientRepository.GetClubId(c.ApiKey);
            int clubSteamAccountId = _commonRepository.ClubSteamAdd(clubId, c.Game, c.SteamId, c.LoginSteam, c.PassSteam, c.Status);

            var response = new
            {
                id = clubSteamAccountId
            };
            return new OkObjectResult(response);
        }

        /// <summary>
        /// Отображает доступные steam аккаунты
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult FreeSteamAccount(ClubSteamFreeSteamAccount c)
        {
            int clubId = _clientRepository.GetClubId( c.ApiKey);
            if (clubId != 0)
            {
                var calulateTime = DateTime.Now.AddMinutes(-2);
                Dictionary<string, object> okResponse = _commonRepository.ClubSteamAccountViewOrUpdate(c.SteamId, clubId, calulateTime);                
                return new OkObjectResult(okResponse);
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
    }
}
