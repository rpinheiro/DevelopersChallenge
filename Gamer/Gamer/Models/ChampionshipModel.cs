using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Gamer.Models
{
    public class ChampionshipsContext : Context
    {
        public ChampionshipsContext()            
        {
        }

        public DbSet<ChampionshipModel> Championships { get; set; }
        public DbSet<PlayerModel> Players { get; set; }
    }

    [Table("Championship")]
    public class ChampionshipModel
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ChampionshipId { get; set; }
        public string ChampionshipName { get; set; }

        public virtual ICollection<PlayerModel> Players { get; set; }

        #region | Construtor |
        public ChampionshipModel()
        {
            Players = new HashSet<PlayerModel>();
        }
        #endregion
    }
}