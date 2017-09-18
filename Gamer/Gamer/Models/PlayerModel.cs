using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Gamer.Models
{
    public class PlayersContext : Context
    {
        public PlayersContext()
            
        {
        }

        public DbSet<PlayerModel> Players { get; set; }
    }

    [Table("Player")]
    public class PlayerModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]        
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }

        public virtual ICollection<ChampionshipModel> Championships { get; set; }

        #region | Construtor |
        public PlayerModel()
        {
            Championships = new HashSet<ChampionshipModel>();
        }
        #endregion
    }
}