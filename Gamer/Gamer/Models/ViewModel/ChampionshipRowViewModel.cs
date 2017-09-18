using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace Gamer.Models
{
    public class ChampionshipRowViewModel
    {
        #region |Propriedades |
        public int ChampionshipId { get; set; }
        public string ChampionshipName { get; set; }
        public string PlayersName { get; set; }
        #endregion

        #region | Construtor |
        public ChampionshipRowViewModel()
        {

        }
        #endregion
    }
}
