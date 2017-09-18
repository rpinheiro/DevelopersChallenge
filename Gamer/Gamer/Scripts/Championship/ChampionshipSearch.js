//script control player seach

$(document).ready(function () {
    ChampionshipSearchLoad();
});

//seach championship 
function SearchChampionship() {
    var urlSearch = $("#SearchAction").val();
    var viewModelSearch = { championshipName: $("#inputSearchChampionship").val() };
    $.ajax({
        type: "POST",
        data: viewModelSearch,
        url: urlSearch,
        success: function (data, textStatus, jqXHR) {

            $("#tblResult tbody").html(data);

            if (HasItemAddedTable()) {
                $("#divSemResultado").hide();
                $("#divListChampionship").show();
                $("#tblResult").show();
                ResultSearchLoad();
            }
            else {
                $("#divSemResultado").show();
                $("#divListChampionship").hide();

            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert("erro");
        }
    });
}

//create a tab to add a new championship
function AddChampionship() {
    var urlAdd = $("#EditAction").val();
    window.open(urlAdd, '_blank');
}

//create a tab to edit a old championship
function EditChampionship() {
    var championshipId = $(this).closest("tr").attr("data-id");
    var urlAction = $("#EditAction").val();
    var urlEdit = urlAction + "?championshipId=" + championshipId;

    window.open(urlEdit, '_blank');
}

//delete a championship
function DeleteChampionship() {

    var answer = confirm("Deseja excluir o campeonato?");
    if (answer) {
        var championshipId = $(this).closest("tr").attr("data-id");
        var viewModelDelete = { championshipId: championshipId };
        var urlDeleteAction = $("#DeleteAction").val();

        $.ajax({
            type: "POST",
            data: viewModelDelete,
            url: urlDeleteAction,
            success: function (data, textStatus, jqXHR) {

                if (data.valid == true) {
                    alert(data.message);
                    SearchChampionship();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("erro");
            }
        });
    }

}

//associte events to page control after seach
function ResultSearchLoad() {

    $('a[name="Editar"]').each(function () {
        $(this).unbind('click');
        $(this).bind('click', null, EditChampionship);
    });

    $('a[name="Excluir"]').each(function () {
        $(this).unbind('click');
        $(this).bind('click', null, DeleteChampionship);
    });
}

//associte events to principal page
function ChampionshipSearchLoad() {
    var doc = $(document);

    doc.on("click", "#btnSearchChampionship", SearchChampionship);

    $('a[name="NovoCampeonato"]').each(function () {
        $(this).unbind('click');
        $(this).bind('click', null, AddChampionship);
    });
}