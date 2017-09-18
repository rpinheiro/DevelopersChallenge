using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace Gamer.Models
{
    public class ManagementViewModel
    {
        #region |Propriedades |
        [Display(Name = "Campeonato")]
        public IList<ChampionshipViewModel> Championships { get; set; }
        #endregion

        #region | Construtor |
        public ManagementViewModel()
        {
            Championships = new List<ChampionshipViewModel>();
        }
        #endregion
    }
}
