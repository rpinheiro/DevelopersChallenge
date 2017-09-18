using Gamer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gamer.Controllers
{
    public class ManagementController : BaseController
    {
        //principal page of management championship
        public ActionResult Index()
        {
            ManagementViewModel viewModel = new ManagementViewModel();
            using (ChampionshipsContext db = new ChampionshipsContext())
            {
                db.Championships.ToList().ForEach(c => viewModel.Championships.Add(new ChampionshipViewModel() { ChampionshipId = c.ChampionshipId, ChampionshipName = c.ChampionshipName }));
            }

            return View(viewModel);
        }

        //create a championship
        public ActionResult GenerateChampionship(int championshipId)
        {
            try
            {
                IList<string> playerList = new List<string>();
                using (ChampionshipsContext db = new ChampionshipsContext())
                {
                    ChampionshipModel championship = db.Championships.Where(n => n.ChampionshipId == championshipId).FirstOrDefault();
                    if (championship != null)
                    {
                        playerList = championship.Players.Select(p => p.PlayerName).ToList();
                    }
                }

                return new JsonResult()
                {
                    Data = new
                    {
                        valid = true,
                        validationErrors = string.Empty,
                        message = "",
                        players = playerList,
                        redirectUrl = ""
                    }
                };
            }
            catch (Exception ex)
            {
                this.LogErrorElmah(ex);
                return this.CriarResultadoException("Erro ao obter os dados do campeonato", null);
            }

        }
    }
}
