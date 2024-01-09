using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefreshAPIEF.Data;

namespace RefreshAPIEF.Controllers.Web
{
    [Route("api/web/[controller]/[action]")]
    [ApiController]
    public class BookingController : ControllerBase
    {

        private readonly RefreshAPIEFContext _context;

        public BookingController(RefreshAPIEFContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Возвращает все резервации за каждый час
        /// </summary>
        /// <param name="ClubId"></param>
        /// <returns></returns>
        /// /// <remarks>
        /// Пример запроса: (не обращать внимания. Это тест)
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item #1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        [HttpGet]
        public IActionResult getAllBookingsPerDay(int ClubId)
        {
            List<int> outData = new List<int>();
            DateTime now = DateTime.Now.Date;
            for (int i = 0; i < 24; i++)
            {
                DateTime timeStart = now.AddHours(i);
                DateTime timeStop = timeStart.AddMinutes(59);
                var bookings = _context.Booking
                    .Where(b => (b.TimeStart <= timeStart && b.TimeStop >= timeStart && b.ClubId == ClubId) ||
                    (b.TimeStart > timeStart && b.TimeStart < timeStart.AddMinutes(59) && b.TimeStop > timeStart.AddMinutes(59) && b.ClubId == ClubId) ||
                    b.TimeStart <= timeStart && b.TimeStop >= timeStart && b.TimeStop <= timeStart.AddMinutes(59) && b.ClubId == ClubId)
                    .GroupBy(b => b.MapCompId)
                    .Count();

                outData.Add(bookings);
            }
            return new OkObjectResult(outData);
        }
    }
}
