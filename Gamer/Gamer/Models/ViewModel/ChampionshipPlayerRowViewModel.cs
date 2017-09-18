using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace Gamer.Models
{
    public class ChampionshipPlayerRowViewModel
    {
        #region |Propriedades |
        public int PlayerId { get; set; }        
        public string PlayerName { get; set; }
        #endregion

        #region | Construtor |
        public ChampionshipPlayerRowViewModel()
        {

        }
        #endregion
    }
}
