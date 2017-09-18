//script control player registration
$(document).ready(function () {
    PlayerLoad();
});

//associate events to page controls
function PlayerLoad() {
    var doc = $(document);

    doc.on("click", "#btnSavePlayer", SavePlayer);
    doc.on("click", "#btnCancel", CloseWindow);
}

function CloseWindow() {
    window.close();
}


//validate requiered fields
function ValidateFields() {
    $("#divSucesso").hide();
    $("#divErro").hide();

    var playerName = $("#PlayerName").val();

    LimparStatus();
    if (playerName == undefined || (playerName != undefined && playerName.trim() == "")) {
        ExibirStatus("Informe o nome do jogador", false);
        return false;
    }

    return true;
}

//save player
function SavePlayer() {

    if (ValidateFields()) {
        var urlSave = $("#SaveAction").val();
        var viewModelSearch = { PlayerId: $("#PlayerId").val(), PlayerName: $("#PlayerName").val() };

        $.ajax({
            type: "POST",
            data: viewModelSearch,
            url: urlSave,
            success: function (data, textStatus, jqXHR) {

                LimparStatus();
                ExibirStatus(data.message, data.valid);

            },
            error: function (jqXHR, textStatus, errorThrown) {
                var data = JSON.parse(jqXHR.responseText);
                if (data.valid == false) {
                    LimparStatus();
                    ExibirStatus(data.message, false);
                }
            }
        });
    }

}
