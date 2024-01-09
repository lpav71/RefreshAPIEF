using Microsoft.AspNetCore.Mvc;
using RefreshAPIEF.ApiModels;
using RefreshAPIEF.Models;
using RefreshAPIEF.Repository;

namespace RefreshAPIEF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoController : ControllerBase
    {
        private readonly CommonRepository _commonRepository;
        private readonly ClientRepository _clientRepository;

        public PromoController(CommonRepository commonRepository, ClientRepository clientRepository)
        {
            _commonRepository = commonRepository;
            _clientRepository = clientRepository;
        }

        [HttpPost]
        public IActionResult Activation(PromoActivation p)
        {
            var user = _commonRepository.GetUserByEmail(p.EMail);
            if (user != null)
            {
                var passwordCorrect = _clientRepository.CheckPassword(p.Password, user.password);
                if (passwordCorrect)
                {
                    Promo? promo = _commonRepository.GetPromoById(p.Id);
                    _commonRepository.PromoActivations(promo);
                    var responce = new
                    {
                        status = "OK"
                    };
                    return new OkObjectResult(responce);
                }
                else
                {
                    return new UnauthorizedResult();
                }
            }
            return new UnauthorizedResult();
        }
    }
}
