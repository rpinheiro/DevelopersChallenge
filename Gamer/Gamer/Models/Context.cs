using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Gamer.Models
{
    public class Context : DbContext
    {
        public Context()
            : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChampionshipModel>()
               .HasMany(p => p.Players)
               .WithMany(r => r.Championships)
               .Map(mc =>
               {
                   mc.MapLeftKey("ChampionshipId");
                   mc.MapRightKey("PlayerId");
                   mc.ToTable("ChampionshipPlayer");
               });
        }
    }
}