#pragma warning disable IDE1006 // Стили именования

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefreshAPIEF.ApiModels;
using RefreshAPIEF.Data;
using RefreshAPIEF.Models;
using RefreshAPIEF.Repository;
using System;
using System.Linq;

namespace RefreshAPIEF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PayController : ControllerBase
    {
        private readonly CommonRepository _commonRepository;
        private readonly ClientRepository _clientRepository;

        public PayController(CommonRepository commonRepository, ClientRepository clientRepository)
        {
            _commonRepository = commonRepository;
            _clientRepository = clientRepository;
        }

        [HttpPost]
        public IActionResult PurchaseGoods(PayAppPay p)
        {
            if (p.apiKey == null && p.apiKeyMobile == null)
            {
                var clubNotFound = new
                {
                    error = "Не указан api_key"
                };
                return new NotFoundObjectResult(clubNotFound);
            }
            int club_id = _clientRepository.GetClubIdMobile(p.apiKey, p.apiKeyMobile);
            if (club_id == 0)
            {
                var clubNotFound = new
                {
                    error = "Клуб с таким api_key не существует"
                };
                return new NotFoundObjectResult(clubNotFound);
            }
            if (p.login == null || p.password == null)
            {
                var clubNotFound = new
                {
                    error = "Не указан логин или пароль"
                };
                return new NotFoundObjectResult(clubNotFound);
            }
            var userId = _clientRepository.VerifyUser(p.login, p.password);
            switch (userId)
            {
                case -1:
                    var clientNotFound = new
                    {
                        error = "Клиент не найден"
                    };
                    return new NotFoundObjectResult(clientNotFound);
                case -2:
                    var passwordIncorrect = new
                    {
                        error = "Пароль неверный"
                    };
                    return new NotFoundObjectResult(passwordIncorrect);
            }
            Finance? financeShifts = _commonRepository.GetFinanceShifts(club_id);
            double? amount = 0; //Сумма необходимая для оплаты
            int nums = 0; //Общее количество приобретаемого товара
            List<Amount> amountList = new(); //Список сумм для покупки
            if (p.data != null)
            {
                foreach (var d in p.data)
                {
                    if (d != null)
                    {
                        var price = _commonRepository.GetPrice(d.id);
                        amount += price * d.num;
                        nums += d.num;
                        Amount a = new()
                        {
                            id = d.id,
                            num = d.num,
                            price = price
                        };
                        amountList.Add(a);
                    }
                }
            }            
            //Проверяем остаток товара
            List<int> ids = new();
            if (p.data == null)
            {
                var dataNotFound = new
                {
                    error = "Поле data не заполнено"
                };
                return new NotFoundObjectResult(dataNotFound);
            }
            foreach (var d in p.data)
            {
                if (d != null)
                {
                    ids.Add(d.id);
                }
            }
            var goods = _commonRepository.GetStores(ids);
            foreach (var item in goods)
            {
                var oneItem = item.id;
                foreach (var i in p.data)
                {
                    if (i.id == item.id)
                    {
                        if (item.num < i.num)
                        {
                            var notGoods = new
                            {
                                error = "Недостаточно товара"
                            };
                            return new NotFoundObjectResult(notGoods);
                        }
                    }
                }
            }
            //Проверяем хватает ли средств клиента на покупку
            var clientFunds = _clientRepository.GetClientById(userId);
            if (p.paytype == 0 && clientFunds?.Amount < amount) //Оплата деньгами
            {
                var clientNotFunds = new
                {
                    error = "Недостаточно средств"
                };
                return new NotFoundObjectResult(clientNotFunds);
            }
            if (p.paytype == 1 & clientFunds?.Bonus < amount) //Оплата бонусами
            {
                var clientNotFunds = new
                {
                    error = "Недостаточно бонусов"
                };
                return new NotFoundObjectResult(clientNotFunds);
            }
            //Производим списание средств с баланса пользователя
            _commonRepository.DebitingFunds(clientFunds,amount,p.paytype);
            //Производим списание товара со склада
            List<int>? storeIds = p.data.Select(d => d.id).ToList();
            List<Store>? stores = _commonRepository.FindStore(storeIds);
            foreach (var d in p.data)
            {
                var store = _commonRepository.GetStoreById(stores,d.id);
                if (store != null)
                {
                    store.num -= d.num;
                }
            }
            _commonRepository.CreateApp(userId, club_id, amount, financeShifts?.AdminId, p.paytype, p.delivery,nums, financeShifts?.Shift,
                p.comp_id,amountList, p.data); 
            var outData = new
            {
                status = "OK"
            };
            return new OkObjectResult(outData);
        }
    }
    public class PayAppPay
    {
        public string? apiKey { get; set; }
        public string? apiKeyMobile { get; set; }
        public int paytype { get; set; }
        public bool? delivery { get; set; }
        public string? login { get; set; }
        public string? password { get; set; }
        public List<Datum>? data { get; set; }
        public int comp_id { get; set; }
    }
    public struct Amount
    {
        public int id;
        public int num;
        public double? price;
    }
}