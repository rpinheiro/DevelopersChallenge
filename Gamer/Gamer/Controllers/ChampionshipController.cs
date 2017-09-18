using Gamer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gamer.Controllers
{
    public class ChampionshipController : BaseController
    {
        //search page of championship
        public ActionResult Index()
        {
            return View();
        }

        //get players list
        private IList<PlayerViewModel> GetListPlayers()
        {
            IList<PlayerViewModel> playerList = new List<PlayerViewModel>();

            using (PlayersContext db = new PlayersContext())
            {
                db.Players.ToList().ForEach(c => playerList.Add(new PlayerViewModel() { PlayerId = c.PlayerId, PlayerName = c.PlayerName }));
            }

            return playerList;
        }

        //add or edit a championship
        public ActionResult Add(int? championshipId)
        {
            ChampionshipViewModel viewModel = new ChampionshipViewModel();
            viewModel.Players = GetListPlayers();

            if (championshipId != null)
            {
                ChampionshipModel championshipSaved = null;
                using (ChampionshipsContext db = new ChampionshipsContext())
                {
                    championshipSaved = db.Championships.Where(p => p.ChampionshipId == championshipId).FirstOrDefault();
                    if (championshipSaved != null)
                    {
                        viewModel.ChampionshipId = championshipSaved.ChampionshipId;
                        viewModel.ChampionshipName = championshipSaved.ChampionshipName;
                        if (championshipSaved.Players.Count > 0)
                        {
                            championshipSaved.Players.ToList().ForEach(c => viewModel.PlayersAdded.Add(new ChampionshipPlayerRowViewModel() { PlayerId = c.PlayerId, PlayerName = c.PlayerName }));
                        }
                    }
                }
            }

            return View(viewModel);
        }

        //Validate function if a championship was already added
        private bool ValidateExistingChampionship(string championshipName)
        {
            using (ChampionshipsContext db = new ChampionshipsContext())
            {
                ChampionshipModel championship = db.Championships.Where(p => p.ChampionshipName.Trim().ToUpper() == championshipName.Trim().ToUpper()).FirstOrDefault();
                if (championship != null)
                    return true;
            }

            return false;
        }

        //save championship
        [HttpPost]
        public ActionResult SaveChampionship(ChampionshipViewModel viewModel)
        {
            try
            {
                //valida se o nome do campeonato está vazio
                if (string.IsNullOrEmpty(viewModel.ChampionshipName))
                    return this.CriarResultadoException("Informe o nome do campeonato");

                //valida se o campeonato já foi cadastrado anteriormente
                if (ValidateExistingChampionship(viewModel.ChampionshipName) && viewModel.ChampionshipId == 0)
                    return this.CriarResultadoException("Campeonato já cadastrado");
                
                using (ChampionshipsContext dbChampionship = new ChampionshipsContext())
                {
                    if (viewModel.ChampionshipId == 0)
                    {
                        ChampionshipModel newChampionship = new ChampionshipModel();                        
                        newChampionship.ChampionshipName = viewModel.ChampionshipName;

                        viewModel.Players.ToList().ForEach(c => newChampionship.Players.Add(dbChampionship.Players.Where(p => p.PlayerId == c.PlayerId).First()));

                        dbChampionship.Championships.Add(newChampionship);
                    }
                    else
                    {
                        ChampionshipModel championship = dbChampionship.Championships.Where(p => p.ChampionshipId == viewModel.ChampionshipId).FirstOrDefault();
                        if (championship != null)
                        {
                            championship.ChampionshipName = viewModel.ChampionshipName;
                            UpdatePlayerList(championship, viewModel, dbChampionship.Players.ToList());

                        }
                    }

                    dbChampionship.SaveChanges();
                }


                return new JsonResult()
                {
                    Data = new
                    {
                        valid = true,
                        validationErrors = string.Empty,
                        message = "Dado(s) salvo(s) com sucesso",
                        redirectUrl = ""
                    }
                };
            }
            catch (Exception ex)
            {
                this.LogErrorElmah(ex);
                return this.CriarResultadoException("Erro ao salvar o(s) dado(s)");
            }
        }

        //function to update a player list of championship
        private void UpdatePlayerList(ChampionshipModel championship, ChampionshipViewModel viewModel, IList<PlayerModel> playerListBd)
        {
            IList<PlayerModel> deleteList = new List<PlayerModel>();
            //recupera os jogadores que deverão ser excluídos
            foreach (PlayerModel player in championship.Players)
            {
                PlayerViewModel playerViewModel = viewModel.Players.Where(c => c.PlayerId == player.PlayerId).FirstOrDefault();
                if (playerViewModel == null)
                {
                    deleteList.Add(player);
                }
            }

            //apaga os jogadores que devem ser excluídos
            deleteList.ToList().ForEach(c => championship.Players.Remove(c));

            //adiciona novos jogadores
            foreach (PlayerViewModel player in viewModel.Players)
            {
                PlayerModel playerModel = championship.Players.Where(c => c.PlayerId == player.PlayerId).FirstOrDefault();
                if (playerModel == null)
                {
                    championship.Players.Add(playerListBd.Where(p => p.PlayerId == player.PlayerId).First());
                }
            }
        }

        //get championship list
        private List<ChampionshipRowViewModel> GetChampionshipList()
        {
            List<ChampionshipRowViewModel> lista = new List<ChampionshipRowViewModel>();
            using (ChampionshipsContext db = new ChampionshipsContext())
            {
                db.Championships.OrderBy(p => p.ChampionshipName).ToList().ForEach(c => lista.Add(new ChampionshipRowViewModel() { ChampionshipId = c.ChampionshipId, ChampionshipName = c.ChampionshipName, PlayersName = string.Join(", ", c.Players.Select(d => d.PlayerName).ToArray()) }));
            }

            return lista;
        }

        //search championships by name
        public ActionResult Search(string championshipName)
        {
            List<ChampionshipRowViewModel> listChampionship = GetChampionshipList();
            if (!string.IsNullOrEmpty(championshipName))
            {
                listChampionship = listChampionship.Where(c => c.ChampionshipName.Trim().ToUpper() == championshipName.Trim().ToUpper()).ToList();
            }

            return PartialView("RowTableChampionship", listChampionship);
        }

        //adding a new player in championship
        public ActionResult AddChampionshipPlayer(ChampionshipPlayerRowViewModel viewModel)
        {
            IList<ChampionshipPlayerRowViewModel> listChampionship = new List<ChampionshipPlayerRowViewModel>();
            listChampionship.Add(viewModel);

            return PartialView("RowTableChampionshipPlayer", listChampionship);
        }

        //action to delete a championship
        public ActionResult Delete(int championshipId)
        {
            try
            {
                using (ChampionshipsContext db = new ChampionshipsContext())
                {
                    ChampionshipModel championship = db.Championships.Where(p => p.ChampionshipId == championshipId).FirstOrDefault();
                    if (championship == null)
                        return this.CriarResultadoException("Campeonato não encontrado.");

                    db.Championships.Remove(championship);
                    db.SaveChanges();
                }

                return new JsonResult()
                {
                    Data = new
                    {
                        valid = true,
                        validationErrors = string.Empty,
                        message = "Campeonato excluído com sucesso.",
                        redirectUrl = ""
                    }
                };
            }
            catch (Exception ex)
            {
                this.LogErrorElmah(ex);
                return this.CriarResultadoException("Erro ao apagar o campeonato");
            }
        }
    }
}
