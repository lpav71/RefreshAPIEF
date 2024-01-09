using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RefreshAPIEF.ApiModels;
using RefreshAPIEF.Data;
using RefreshAPIEF.Models;
using RefreshAPIEF.Repository;
using System.Data;

namespace RefreshAPIEF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly CommonRepository _commonRepository;

        public ClubsController(CommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        /// <summary>
        /// Добавляет клуб
        /// </summary>
        [HttpPost]
        public IActionResult ClubCreate(ClubsClubCreate c)
        {
            var clubId = _commonRepository.ClubCreate(c.Name, c.Id_group, c.Address, c.Ip, c.Api_key, c.Local_ip, c.Cashbox, c.Cashbox_port, c.Max_bonus, c.Time_zone);
            var response = new
            {
                id = clubId
            };
            return new OkObjectResult(response);
        }

        [HttpPost]
        public IActionResult GetName()
		{
            var jsonResult = _commonRepository.ClubGetName();

            if (jsonResult != null)
            {
                var result = (dynamic)jsonResult;
                return new OkObjectResult(result.json_agg);
            }                
            else
                return new OkObjectResult(jsonResult);
		}
    }
}
