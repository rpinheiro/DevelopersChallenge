/*script to control player registration*/

$(document).ready(function () {
    var doc = $(document);

    //associa o evento aos controles da tela
    doc.on("click", "#btnAddPlayer", AddChampionshipPlayer);
    doc.on("click", "#btnSaveChampionship", SaveChampionship);
    doc.on("click", "#btnCancel", CloseWindow);

    $('a[name="Excluir"]').each(function () {
        $(this).unbind('click');
        $(this).bind('click', null, RemovePlayer);
    });

    if (HasItemAddedTable()) {
        $("#tblResult").show();
    }
});

//validate required fields
function ValidateFields() {
    $("#divSucesso").hide();
    $("#divErro").hide();

    var championshipName = $("#ChampionshipName").val();

    if (championshipName == undefined || (championshipName != undefined && championshipName.trim() == "")) {
        $("#divErro strong").html("Informe o nome do jogador");
        $("#divErro").show();
        return false;
    }

    return true;
}

//get player list
function GetPlayersAdded() {
    var playersAdded = [];
    $("#tblResult tbody td[class!='actions']").each(function () {
        var playerId = $(this).closest("tr").attr("data-id");
        var playerName = $(this).text().trim();
        obj = {};
        obj["PlayerId"] = playerId;
        obj["PlayerName"] = playerName;

        playersAdded.push(obj);
    });

    return playersAdded;
}

//save championship in bd
function SaveChampionship() {

    if (ValidateFields()) {
        var urlSave = $("#SaveAction").val();
        var viewModelSearch = { ChampionshipId: $("#ChampionshipId").val(), ChampionshipName: $("#ChampionshipName").val(), Players: GetPlayersAdded() };
        //requisição ao servidor
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(viewModelSearch),
            url: urlSave,
            success: function (data, textStatus, jqXHR) {
                ExibirStatus(data.message, data.valid);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var data = JSON.parse(jqXHR.responseText);
                ExibirStatus(data.message, data.valid);
            }
        });
    }

}

//remove a player from client
function RemovePlayer() {
    $(this).closest("tr").remove();
    if (HasItemAddedTable() == false) {
        $("#tblResult").hide();
    }
}

//validate funtion if a player was already added
function ValidatePlayerAdded(playerId) {
    var ret = false;
    if (HasItemAddedTable()) {

        $("#tblResult tbody tr").each(function () {
            var currentPlayerId = $(this).attr("data-id");
            if (playerId == currentPlayerId) {
                ExibirStatus("Este jogador já foi adicionado ao campeonato.", false);
                ret = true;
                return;
            }
        });
    }

    return ret;
}

//add a player in championship
function AddChampionshipPlayer() {

    var playerId = $("#Players option:selected").val();
    if (playerId > 0 && !ValidatePlayerAdded(playerId)) {
        var playerName = $("#Players option:selected").text();
        var viewModel = { PlayerId: playerId, PlayerName: playerName };
        var urlAddPlayer = $("#AddPlayerAction").val();
        //requisição ao servidor
        $.ajax({
            type: "POST",
            data: viewModel,
            url: urlAddPlayer,
            success: function (data, textStatus, jqXHR) {
                $("#tblResult tbody").append(data);
                var quantidadeResultado = $("#tblResult tbody tr").length;
                if (quantidadeResultado > 0) {
                    $("#tblResult").show();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("erro");
            }
        });
    }


}