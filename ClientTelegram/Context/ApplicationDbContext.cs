using ClientTelegram.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace ClientTelegram.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<SessionEntity> Sessions { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }

        /* Default queries exclude soft-deleted rows. 
         * Use .IgnoreQueryFilters() to include them. */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SessionEntity>().HasQueryFilter(x => !x.Deleted);
            modelBuilder.Entity<MessageEntity>().HasQueryFilter(x => !x.Deleted);
        }
    }
}
