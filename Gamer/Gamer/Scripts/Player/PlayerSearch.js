//script control page search
$(document).ready(function () {
    PlayerSearchLoad();
});

//seach players
function SearchPlayer() {
    var urlSearch = $("#SearchAction").val();
    var viewModelSearch = { playerName: $("#inputSearchPlayer").val() };
    $.ajax({
        type: "POST",
        data: viewModelSearch,
        url: urlSearch,
        success: function (data, textStatus, jqXHR) {

            $("#tblResult tbody").html(data);

            if (HasItemAddedTable()) {
                $("#divSemResultado").hide();
                $("#divListPlayer").show();
                $("#tblResult").show();
                ResultSearchLoad();
            }
            else {
                $("#divSemResultado").show();
                $("#divListPlayer").hide();

            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("erro");
        }
    });
}

//create a new tab to add a new player
function AddPlayer() {
    var urlAdd = $("#EditAction").val();
    window.open(urlAdd, '_blank');
}

//create a new tab to edit a old player
function EditPlayer() {
    var playerId = $(this).closest("tr").attr("data-id");
    var urlAction = $("#EditAction").val();
    var urlEdit = urlAction + "?playerId=" + playerId;

    window.open(urlEdit, '_blank');
}

//delete player
function DeletePlayer() {

    var answer = confirm("Deseja excluir o jogador?");
    if (answer) {
        var playerId = $(this).closest("tr").attr("data-id");
        var viewModelDelete = { playerId: playerId };
        var urlDeleteAction = $("#DeleteAction").val();

        $.ajax({
            type: "POST",
            data: viewModelDelete,
            url: urlDeleteAction,
            success: function (data, textStatus, jqXHR) {

                if (data.valid == true) {
                    alert(data.message);
                    SearchPlayer();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("erro");
            }
        });
    }

}

//associate events after seach
function ResultSearchLoad() {


    $('a[name="Editar"]').each(function () {
        $(this).unbind('click');
        $(this).bind('click', null, EditPlayer);
    });

    $('a[name="Excluir"]').each(function () {
        $(this).unbind('click');
        $(this).bind('click', null, DeletePlayer);
    });
}

//associate events after prinpal page load
function PlayerSearchLoad() {
    var doc = $(document);

    doc.on("click", "#btnSearchPlayer", SearchPlayer);

    $('a[name="NovoJogador"]').each(function () {
        $(this).unbind('click');
        $(this).bind('click', null, AddPlayer);
    });
}