using Microsoft.AspNetCore.Mvc;
using RefreshAPIEF.ApiModels;
using RefreshAPIEF.Models;
using RefreshAPIEF.Repository;
using System.Globalization;

namespace RefreshAPIEF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ClientRepository _clientRepository;

        public ClientsController(ClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpPost]
        public IActionResult Finance(ClientFinance finance)
        {
            var client = _clientRepository.GetClientByLogin(finance.Login);
            if (client != null)
            {
                bool passwordCorrect = BCrypt.Net.BCrypt.Verify(finance.Password, client.Password);
                if (passwordCorrect)
                {
                    var result = _clientRepository.GetClientFinance(client.ClubId, client.Id);  
                    return new OkObjectResult(result);
                }
            }           
            return new UnauthorizedResult();
        }

        /// <summary>
        /// Регистрирует нового клиента
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Register(ClientRegister c)
        {
            var client = _clientRepository.GetClientByLogin(c.Login);
            int clubId = _clientRepository.GetClubIdMobile(c.ApiKey, c.MobileApiKey);
            //Если клиент не найден
            Client clientNew = new();
            if (client == null)
            {
                clientNew = _clientRepository.RegisterNewClient(clubId, c.Login, c.Password, c.Phone, c.Email, c.Icon, c.Amount, c.Bonus, c.TotalTime,
                    c.FullName, c.StatusActive, c.TelegramId, c.VkId, c.Bday, c.Verify, c.VerifyDt, c.Name, c.SurName, c.MiddleName);
            }           
            else
            {
                var response = new
                {
                    status = "Пользователь с таким имененем уже существует"
                };
                return new BadRequestObjectResult(response);
            }
            return new OkObjectResult(clientNew);
        }

        /// <summary>
        /// Получает пользователя по id
        /// </summary>
        [HttpPost]
        public IActionResult AccountInfo(ClientAccountInfo c)
        {
            var client = _clientRepository.GetClientById(c.Id);
            return new OkObjectResult(client);
        }

        /// <summary>
        /// Получает пользователя по логину
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UserInfo(ClientUserInfo c)
        {
            var client = _clientRepository.GetClientByLogin(c.Login);
            bool passwordCorrect = BCrypt.Net.BCrypt.Verify(c.Password, client.Password);
            if (passwordCorrect)
                return new OkObjectResult(client);
            else
            {
                var response2 = new
                {
                    status = "Неверный пароль"
                };
                return new BadRequestObjectResult(response2);
            }
        }

        /// <summary>
        /// Получает список всех пользователей
        /// </summary>
        [HttpPost]
        public IActionResult GetAllUsers(ClientGetAllUsers c)
        {
            int clubId = _clientRepository.GetClubId(c.ApiKey);  
            var clients = _clientRepository.GetClient(clubId);
            return new OkObjectResult(clients);
        }

        /// <summary>
        /// Ищет пользователей в рамках клуба
        /// </summary>
        [HttpPost]
        public IActionResult FindUsers(ClientFindUser c)
        {
            var sText = c.SearchText.ToUpper();
            int clubId = _clientRepository.GetClubId(c.ApiKey);
            var clients = _clientRepository.FindClient(sText, clubId);
            return new OkObjectResult(clients);            
        }

        /// <summary>
        /// Удаляет пользователя по id
        /// </summary>
        [HttpPost]
        public IActionResult DeleteUser(ClientDeleteUser c)
        {
            var client = _clientRepository.GetClientById(c.Id);
            _clientRepository.DeleteClient(client);

            var Ok = new
            {
                status = "Ok"
            };
            return new OkObjectResult(Ok);            
        }

        [HttpPost]
        public IActionResult Operation(ClientOparation o)
        {
            var client = _clientRepository.GetClientByLogin(o.UserLogin.ToLower());
            if (client != null)
            {
                bool passwordCorrect = BCrypt.Net.BCrypt.Verify(o.UserPassword, client.Password);
                if (passwordCorrect)
                {
                    var clubId = _clientRepository.GetClubIdMobile(o.Apikey, o.ApikeyMobile);
                    if (clubId != 0)
                    {
                        var dateFrom = DateTime.ParseExact(o.DateFrom, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        var dateTo = DateTime.ParseExact(o.DateTo, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                        List<StoreAgg>? result = new();
                        try
                        {
                            result = _clientRepository.GetOperations(client.Id, clubId, dateFrom, dateTo);
                        }
                        catch (Exception)
                        {
                            return new NotFoundObjectResult(result);
                        }                       

                        return new OkObjectResult(result[0].product);
                    }
                    else
                    {
                        var error = new
                        {
                            status = "Неверный API KEY"
                        };
                        return new BadRequestObjectResult(error);
                    }
                }
                return new UnauthorizedResult();
            }
            return new UnauthorizedResult();
        }

        [HttpPost]
        public IActionResult Purchases(ClientPurchases c)
        {
            if (c.ApiKey == null && c.ApiKeyMobile == null)
            {
                var clubNotFound = new
                {
                    error = "Не указан api_key"
                };
                return new NotFoundObjectResult(clubNotFound);
            }
            int club_id = _clientRepository.GetClubIdMobile(c.ApiKey, c.ApiKeyMobile);
            if (club_id == 0)
            {
                var clubNotFound = new
                {
                    error = "Клуб с таким api_key не существует"
                };
                return new NotFoundObjectResult(clubNotFound);
            }
            if (c.Login == null || c.Password == null)
            {
                var clubNotFound = new
                {
                    error = "Не указан логин или пароль"
                };
                return new NotFoundObjectResult(clubNotFound);
            }
            if (c.DataFrom == null || c.DataTo == null)
            {
                var dateNotFound = new
                {
                    error = "Не указана дата"
                };
                return new NotFoundObjectResult(dateNotFound);
            }
            var timeStart = DateTime.ParseExact(c.DataFrom, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            var timeStop = DateTime.ParseExact(c.DataTo, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            List<Purchase>? result = _clientRepository.GetPurchases(timeStart, timeStop, club_id);
            return new OkObjectResult(result);
        }
    }
}
