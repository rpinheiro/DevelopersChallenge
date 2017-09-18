using Gamer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gamer.Controllers
{
    public class PlayerController : BaseController
    {
        //Search Player page
        public ActionResult Index()
        {
            return View();
        }

        //add or edit a player
        public ActionResult Add(int? playerId)
        {
            PlayerViewModel viewModel = new PlayerViewModel();
            //verifica se é uma edição de um valor já cadastrado
            if (playerId != null)
            {
                PlayerModel playerSaved = null;
                using (PlayersContext db = new PlayersContext())
                {
                    playerSaved = db.Players.Where(p => p.PlayerId == playerId).FirstOrDefault();
                }

                if (playerSaved != null)
                {
                    //se existir o jogador no banco de dados, vai atribuir as informações a viewmodel e exibir na tela
                    viewModel.PlayerId = playerSaved.PlayerId;
                    viewModel.PlayerName = playerSaved.PlayerName;
                }
            }

            return View(viewModel);
        }

        //Validate function if a player was already added
        private bool ValidateExistingPlayer(string playerName)
        {
            using (PlayersContext db = new PlayersContext())
            {
                PlayerModel player = db.Players.Where(p => p.PlayerName.Trim().ToUpper() == playerName.Trim().ToUpper()).FirstOrDefault();
                if (player != null)
                    return true;
            }

            return false;
        }

        //save player
        public ActionResult SavePlayer(PlayerViewModel viewModel)
        {
            try
            {
                //valida se o nome do jogador está vazio
                if (string.IsNullOrEmpty(viewModel.PlayerName.Trim()))
                    return this.CriarResultadoException("Informe o nome do jogador");

                //verifica se o jogador já foi cadastrado anteriormente
                if (ValidateExistingPlayer(viewModel.PlayerName.Trim()))
                    return this.CriarResultadoException("Jogador já cadastrado");

                using (PlayersContext db = new PlayersContext())
                {
                    //verifica se é um novo jogador
                    if (viewModel.PlayerId == 0)
                    {
                        //se for novo jogador, cria a model 
                        PlayerModel newPlayer = new PlayerModel();
                        newPlayer.PlayerName = viewModel.PlayerName;
                        db.Players.Add(newPlayer);
                    }
                    else
                    {
                        //se for edição de jogador, recupera o jogador do banco de dados e atualiza as informações
                        PlayerModel player = db.Players.Where(p => p.PlayerId == viewModel.PlayerId).FirstOrDefault();
                        if (player != null)
                            player.PlayerName = viewModel.PlayerName;
                    }

                    db.SaveChanges();
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

        //get player list
        private List<PlayerRowViewModel> GetPlayerList()
        {
            List<PlayerRowViewModel> lista = new List<PlayerRowViewModel>();
            using (PlayersContext db = new PlayersContext())
            {
                db.Players.OrderBy(p => p.PlayerName).ToList().ForEach(c => lista.Add(new PlayerRowViewModel() { PlayerId = c.PlayerId, PlayerName = c.PlayerName }));
            }

            return lista;
        }

        //search a player by name
        public ActionResult Search(string playerName)
        {
            List<PlayerRowViewModel> listaPlayer = GetPlayerList();
            if (!string.IsNullOrEmpty(playerName))
            {
                //recupera a lista de jogador a partir do nome informado
                listaPlayer = listaPlayer.Where(c => c.PlayerName.Trim().ToUpper() == playerName.Trim().ToUpper()).ToList();
            }

            return PartialView("RowTablePlayer", listaPlayer);
        }

        //delete a player by id
        public ActionResult Delete(int playerId)
        {
            try
            {
                using (PlayersContext db = new PlayersContext())
                {
                    PlayerModel player = db.Players.Where(p => p.PlayerId == playerId).FirstOrDefault();
                    //valida se o identificador informado está no banco
                    if (player == null)
                        return this.CriarResultadoException("Jogador não encontrado.");

                    db.Players.Remove(player);
                    db.SaveChanges();
                }

                return new JsonResult()
                {
                    Data = new
                    {
                        valid = true,
                        validationErrors = string.Empty,
                        message = "Jogador excluído com sucesso.",
                        redirectUrl = ""
                    }
                };
            }
            catch (Exception ex)
            {
                this.LogErrorElmah(ex);
                return this.CriarResultadoException("Erro ao apagar o jogador.");
            }
        }
    }
}
