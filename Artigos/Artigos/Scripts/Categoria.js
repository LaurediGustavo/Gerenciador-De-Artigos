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

                html += '<a href = "#" onclick="Details(' + item.Id + ')">';
                html += '<div class="lista">';
                html += '<span class="listaNome">' + item.NomeCategoria + '</span>';
                html += '<span class="listaAtiva">Ativa: ' + check + '</span>';
                html += '</div >';
                html += '</a >';
            });

            $('#lista').html(html);
        },
        error: function (result) {
            alert('Erro ao buscar as categorias!');
        }
    });
}

const checkAtivo = document.querySelector('#cbxCategoria');

function Create() {
    if ($('#nomeCategoria').val() != "") {
        var categoria = {
            NomeCategoria: $('#nomeCategoria').val(),
            Ativa: checkAtivo.checked
        };

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        $.ajax({
            url: "/Categoria/Create",
            data: {
                __RequestVerificationToken: token,
                categoria: categoria
            },
            type: "POST",
            contentType: "application/x-www-form-urlencoded;charset=utf-8",
            dataType: "json",
            success: function (result) {
                alert('Cadastro Realizado!');
                $('#erro').text('');
                LimpaCampo();
                GetAll();
            },
            error: function () {
                alert('Erro no cadastro!');
            }
        });
    }
    else {
        $('#erro').text('Esse campo é obrigatório!');
    }
}

function Details(id) {
    $('#btnCriar').hide();
    $('#btnUpdate').show();

    $.ajax({
        url: "Categoria/Get/" + id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#nomeCategoria').val(result.NomeCategoria);

            if (result.Ativa == 1) {
                checkAtivo.checked = true;
            }
            else {
                checkAtivo.checked = false;
            }

            var html = "";
            html += '<div class="text-center">';
            html += '<input type="submit" value="Atualizar" id="btnUp" class="btn btn-info" onclick="Update(' + id + ')" /> |';
            html += '<a href="#" onclick="Sair()">Sair</a>';
            html += '</div>';

            $('#btnUpdate').html(html);
            $('#erro').text('');
        },
        error: function (result) {
            alert('Erro ao buscar a categoria!');
        }
    });
}

function Update(id) {
    if ($('#nomeCategoria').val() != "") {
        var categoria = {
            Id: id,
            NomeCategoria: $('#nomeCategoria').val(),
            Ativa: checkAtivo.checked
        };

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        $.ajax({
            url: "Categoria/Update",
            data: {
                __RequestVerificationToken: token,
                categoria: categoria
            },
            type: "POST",
            contentType: "application/x-www-form-urlencoded;charset=utf-8",
            dataType: "json",
            success: function (result) {
                alert('Atualização Realizada!');
                $('#erro').text('');
                LimpaCampo();
                GetAll();
                Sair();
            },
            error: function () {
                alert('Erro ao atualizar!');
            }
        });
    }
    else {
        $('#erro').text('Esse campo é obrigatório!');
    }
}

function Sair() {
    $('#btnCriar').show();
    $('#btnUpdate').hide();
    LimpaCampo();
    $('#erro').text('');
}

function LimpaCampo() {
    $('#nomeCategoria').val("");
    checkAtivo.checked = false;
    $('#erro').text('');
}