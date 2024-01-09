using Microsoft.EntityFrameworkCore;
using RefreshAPIEF.Models;

namespace RefreshAPIEF.Data
{
    public class RefreshAPIEFContext : DbContext
    {
        public RefreshAPIEFContext (DbContextOptions<RefreshAPIEFContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=gt;Username=postgres;Password=postgres");
            //optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=gt;Username=postgres;Password=Q12801024q");

            optionsBuilder.UseNpgsql("Host=45.135.165.89;Port=5432;Database=gt;Username=postgres;Password=Q12801024q");
        }

        public DbSet<Map> Map { get; set; } = default!;
        public DbSet<Club> Club { get; set; } = default!;
        public DbSet<Client> Client { get; set; } = default!;
        public DbSet<ClientWallets> ClientWallets { get; set; } = default!;
        public DbSet<Booking> Booking { get; set; } = default!;
        public DbSet<Price> Price { get; set; } = default!;
        public DbSet<ClubSteamAccount> ClubSteamAccount { get; set; } = default!;
        public DbSet<Game> Game { get; set; } = default!;
        public DbSet<Finance> Finance { get; set; } = default!;
        public DbSet<Zone> Zone { get; set; } = default!;
        public DbSet<Store> Store { get; set; } = default!;
        public DbSet<StoreOut> StoreOut { get; set; } = default!;
        public DbSet<StoreAgg> StoreAgg { get; set; } = default!;
        public DbSet<Purchase> Purchase { get; set; } = default!;
        public DbSet<Users> Users { get; set; } = default!;
        public DbSet<Cashbox> Cashbox { get; set; } = default!;
        public DbSet<TypeOperation> TypeOperation { get; set; } = default!;
        public DbSet<Promo> Promo { get; set; } = default!;
        public DbSet<AppPay> AppPay { get; set; } = default!;
        public DbSet<AppPayList> AppPayList { get; set; } = default!;
        public DbSet<GameStatistic> GameStatistic { get; set; } = default!;
    }
}
