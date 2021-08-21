﻿function GetAll(id) {
    $.ajax({
        url: "/Paragrafo/GetAll/" + id,
        contentType: "application/json; charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (result) {
            var html = '';
            var num = 1;
            $.each(result, function (key, item) {
                html += '<a onclick="Details(' + item.Id + ',' + item.ArtigoId + ')">';
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

function Details(idPa, id) {
    $.ajax({
        url: "/Paragrafo/Details?idPa=" + idPa + "&idAr=" + id,
        contentType: "application/json; charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (result) {
            if (result != null) {
                $('#btnCreate').hide();
                $('#btnExcluir').show();
                $('#btnUpdate').show();
                $('#btnSair').show();

                const sair = document.querySelector('#btnSair');
                const btnImg = document.querySelector('#btnCadastroImg');
                const btnExcluir = document.querySelector('#btnExcluir');
                const btnUpdate = document.querySelector('#btnUpdate');

                sair.setAttribute('onclick', 'Sair()');
                btnImg.setAttribute('class', 'btn btn-info btnCadastroImgTrue');
                btnExcluir.setAttribute('onclick', 'Excluir(' + result.Id + ')');
                btnUpdate.setAttribute('onclick', 'Update(' + result.Id + ')');

                $('#texto').val(result.Texto);
                DetailsImg(result.Id)
            }
        },
        error: function () {
            alert("Erro ao buscar o paragrafo!");
        }
    });
}

function DetailsImg(id) {
    $.ajax({
        url: "/Paragrafo/GetImg/" + id,
        contentType: "application/json; charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (result) {
            var html = '';
            var num = 1;
            $.each(result, function (key, item) {
                html += '<a onclick="GetDetailsImg(' + item.Id + ',' + item.ParagrafoId + ')">';
                html += '<div class="divLista">';
                html += 'Imagem ' + num;
                html += '</div>';
                html += '</a>';
                num++;
            });

            $('#listaImagem').html(html);
        },
        error: function () {
            alert("Erro ao buscar as imagens!");
        }
    });
}

function Sair() {
    $('#btnCreate').show();
    $('#btnExcluir').hide();
    $('#btnUpdate').hide();
    $('#btnSair').hide();
    $('#texto').val('');

    const btnImg = document.querySelector('#btnCadastroImg');
    btnImg.setAttribute('class', 'btn btn-info btnCadastroImgFalse');
}