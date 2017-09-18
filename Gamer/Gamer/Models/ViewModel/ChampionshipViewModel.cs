using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace Gamer.Models
{
    public class ChampionshipViewModel
    {
        #region |Propriedades |
        public int ChampionshipId { get; set; }
        [Display(Name = "Campeonato")]
        public string ChampionshipName { get; set; }
        
        public IList<PlayerViewModel> Players { get; set; }

        public IList<ChampionshipPlayerRowViewModel> PlayersAdded { get; set; }

        [Display(Name = "Jogadores")]
        public string PlayersString { get; set; }
        #endregion

        #region | Construtor |
        public ChampionshipViewModel()
        {
            PlayersAdded = new List<ChampionshipPlayerRowViewModel>();
        }

        #endregion
    }
}
