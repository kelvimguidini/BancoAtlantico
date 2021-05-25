using Microsoft.EntityFrameworkCore;
using Atlantico.Domain;
using Atlantico.Data.Mappings;

namespace Atlantico.Data.Context
{

    public class ContextDB : DbContext
    {
        public ContextDB(DbContextOptions<ContextDB> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<User> User { get; set; }
        public DbSet<ATM> ATM { get; set; }
        public DbSet<ATMBankNote> ATMBankNote { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ATMConfiguration());
            modelBuilder.ApplyConfiguration(new ATMBankNoteConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextDB).Assembly);
        }

    }

}
