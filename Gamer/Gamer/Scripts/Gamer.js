function HasItemAddedTable() {
    var quantidadeResultado = $("#tblResult tbody tr").length;
    if (quantidadeResultado > 0) return true;

    return false;
}

//fecha a aba corrente
function CloseWindow() {
    window.close();
}

function LimparStatus() {
    $("#divStatus").removeClass("alert-success");
    $("#divStatus").removeClass("alert-danger");
    $("#divStatus").hide();
}

function ExibirStatus(message, success) {
    $("#divStatus").removeClass("alert-success");
    $("#divStatus").removeClass("alert-danger");

    $("#divStatus strong").html(message);
    if (success)
        $("#divStatus").addClass("alert-success");
    else
        $("#divStatus").addClass("alert-danger");

    $("#divStatus").show();
}