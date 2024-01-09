using Microsoft.EntityFrameworkCore;
using RefreshAPIEF.ApiModels;
using RefreshAPIEF.Data;
using RefreshAPIEF.Models;

namespace RefreshAPIEF.Repository
{
    public class MapRepository
    {
        private readonly RefreshAPIEFContext _context;
        public MapRepository(RefreshAPIEFContext context)
        {
            _context = context;
        }
        public Map? GetComp(int? clubId, int? idComp)
        {
            return _context.Map.Where(p => p.ClubId == clubId && p.IdComp == idComp).FirstOrDefault();
        }
        public void UpdateComp(Map target, Map source, int clubId)
        {
            target.Zone = source.Zone;
            target.UserId = source.UserId;
            target.IdComp = source.IdComp;
            target.Level = source.Level;
            target.PosX = source.PosX;
            target.PosY = source.PosY;
            target.StatusActive = source.StatusActive;
            target.Ip = source.Ip;
            target.Mac = source.Mac;
            target.Ver = source.Ver;
            target.ClubId = clubId;
            target.mb = source.mb;
            target.cpu = source.cpu;
            target.gpu = source.gpu;
            target.ram = source.ram;
            target.disk = source.disk;
            target.temp = source.temp;
            target.qr_gen = 0;
            _context.Map.Update(target);
            _context.SaveChanges();
        }
        public Map CreateComp(Map map, int clubId)
        {
            Map newComputer = new()
            {
                Zone = map.Zone,
                UserId = map.UserId,
                IdComp = map.IdComp,
                Level = map.Level,
                PosX = map.PosX,
                PosY = map.PosY,
                StatusActive = map.StatusActive,
                Ip = map.Ip,
                Mac = map.Mac,
                Ver = map.Ver,
                ClubId = clubId,
                mb = map.mb,
                cpu = map.cpu,
                gpu = map.gpu,
                ram = map.ram,
                disk = map.disk,
                temp = map.temp,
                qr_gen = 0
            };
            _context.Map.Add(newComputer);
            _context.SaveChanges();
            return newComputer;
        }
        public Map? GenerateNewQR(string mac, int clubId)
        {
            var data = _context.Map.Where(p => p.Mac == mac && p.ClubId == clubId).FirstOrDefault();
            if (data != null)
            {
                data.u_login = "";
                data.u_pass = "";
                data.qr_gen = 1;
                data.qr_to_login = Convert.ToString(Guid.NewGuid()); // генерируем новый идентификатор
                _context.SaveChanges(); // сохраняем изменения
            }
            return data;
        }
        public List<StoreAgg>? GetMap(int clubId)
        {
            return _context.Set<StoreAgg>().FromSqlRaw("select json_agg(row_to_json(row(club_id, zone, pos_x, pos_y, id_comp))) from map where club_id = {0} group by club_id", clubId).ToList();
        }
        public string? GetDataForQR(int clubId, int compId)
        {
           return _context.Map.Where(x => x.qr_gen == 1 && x.ClubId == clubId && x.IdComp == compId).Select(x => x.qr_to_login).FirstOrDefault();
        }
        public List<StoreAgg>? GetClubConfig(int clubId)
        {
            return _context.Set<StoreAgg>().FromSqlRaw("select row_to_json(row(id, color1,color2,color3 ,colortext1, colortext2,login_image, video)) as setting from public.club_setting where club_id={0} and status=true", clubId).ToList();
        }
    }
}
