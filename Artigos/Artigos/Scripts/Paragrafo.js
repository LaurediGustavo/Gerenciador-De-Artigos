function GetAll(id) {
    $.ajax({
        url: "/Paragrafo/GetAll/" + id,
        contentType: "application/json; charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (result) {
            var html = '';
            var num = 1;
            $.each(result, function (key, item) {
                html += '<a onclick="Details(' + item.Id + ')">';
                html += '<div class="divLista">';
                html += 'Paragrafo ' + num;
                html += '</div>';
                html += '</a>';
                num++;
            });

            $('#listaParagrafo').html(html);
        },
        error: function () {
            alert("Erro ao buscar os paragrafos!");
        }
    });
}

function Create(id) {
    if ($('#texto').val() != '') {
        $('#erro').text('');

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        var texto = $('#texto').val();

        $.ajax({
            url: "/Paragrafo/Create/" + id,
            contentType: "application/x-www-form-urlencoded;charset=utf-8",
            type: "POST",
            dataType: "json",
            data: {
                __RequestVerificationToken: token,
                texto
            },
            success: function (result) {
                alert("Paragrafo adicionado!");
                $('#texto').val('');
                GetAll(id);
            },
            error: function () {
                alert("Erro ao adicionar!");
            }
        });
    }
    else {
        $('#erro').text('Esse campo é obrigatório!');
    }
}

function Details(id) {
    $.ajax({
        url: "/Paragrafo/Details/" + id,
        contentType: "application/json; charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (result) {

        },
        error: function () {
            alert("Erro ao buscar o paragrafo!");
        }
    });
}

function DetailsImg() {

}