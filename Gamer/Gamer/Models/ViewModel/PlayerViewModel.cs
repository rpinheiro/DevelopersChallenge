using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace Gamer.Models
{
    public class PlayerViewModel
    {
        #region |Propriedades |
        public int PlayerId { get; set; }
        
        [Display(Name = "Nome")]        
        public string PlayerName { get; set; }
        #endregion

        #region | Construtor |
        public PlayerViewModel()
        {

        }

        #endregion
    }
}
