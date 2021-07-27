$(document).ready(function () {
    GetAll();
});

const check = document.querySelector('#cbx_consulta');
check.addEventListener('click', GetAll);

function GetAll() {
    $.ajax({
        url: "Categoria/GetAll?nome=" + $('#txt_consulta').val() + "&ativa=" + check.checked,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                var check = "Não";
                if (item.Ativa == 1) {
                    check = "Sim";
                }

                html += '<a href = "#" onclick="Update(' + item.Id + ')">';
                html += '<div class="lista">';
                html += '<span class="listaNome">' + item.NomeCategoria + '</span>';
                html += '<span class="listaAtiva">Ativa: ' + check + '</span>';
                html += '</div >';
                html += '</a >';
            });

            $('#lista').html(html);
        },
        error: function (result) {

        }
    });
}

function Update(id) {
    $('#formCreate').hide();
    $('#formUpdate').show();

    $.ajax({
        url: "Categoria/Get/" + id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#nomeCategoria').val(result.NomeCategoria);

            if (result.Ativa == 1) {
                $('#Ativa').checked;
            }
        },
        error: function (result) {

        }
    });
}

function UpdateCategoria() {
    if ($('#nomeCategoria').val() != "") {
        
    }
    else {
        alert('oiiiiiiii');
    }
}