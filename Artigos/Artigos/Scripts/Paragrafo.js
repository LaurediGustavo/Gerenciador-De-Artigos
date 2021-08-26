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
                btnExcluir.setAttribute('onclick', 'Excluir(' + result.Id + ',' + id + ')');
                btnUpdate.setAttribute('onclick', 'Update(' + result.Id + ',' + id + ')');
                btnImg.setAttribute('onclick', 'Img()');

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

function Update(id, idAr) {
    if ($('#texto').val() != '') {

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        var texto = $('#texto').val();

        $.ajax({
            url: "/Paragrafo/Update",
            contentType: "application/x-www-form-urlencoded;charset=utf-8",
            type: "POST",
            dataType: "json",
            data: {
                __RequestVerificationToken: token,
                idPa: id,
                idAr: idAr,
                texto: texto
            },
            success: function (result) {
                GetAll(idAr);
            },
            error: function () {
                alert("Erro ao atualizar!");
            }
        });
    }
    else {
        $('#erro').text('Esse campo é obrigatório!');
    }
}

function Excluir(id, idAr) {
    if (id != null) {
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        $.ajax({
            url: "/Paragrafo/Delete",
            contentType: "application/x-www-form-urlencoded;charset=utf-8",
            type: "POST",
            dataType: "json",
            data: {
                __RequestVerificationToken: token,
                idPa: id,
                idAr: idAr
            },
            success: function (result) {
                alert("Exclusão realizada!");
                GetAll(idAr);
                Sair();
                $('#listaImagem').html('');
            },
            error: function () {
                alert("Erro ao Excluir!");
            }
        });
    }
    else {
        $('#erro').text('Esse campo é obrigatório!');
    }
}

function Img() {
    $('#fileUpload').click();
}

const img = document.querySelector('#fileUpload');

img.addEventListener('change', function () {
    var nome = "Não há arquivo selecionado. Selecionar arquivo...";
    if (img.files.length > 0) {
        var ex = ["gif", "png", "jpg"];
        var ext = img.files[0].name.split('.').pop().toLowerCase();

        if (ex.lastIndexOf(ext) == -1) {
            nome = "Extensão permitidas: gif, png, jpg";
        }
        else {
            CreateImg(img);
        }
    }

    alert(nome);
});

function CreateImg(img) {
    if (img.files.length > 0) {
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        $.ajax({
            url: "/Imagem/Create",
            contentType: "application/x-www-form-urlencoded;charset=utf-8",
            type: "POST",
            dataType: "json",
            data: {
                __RequestVerificationToken: token,
                idAr: idAr,
                idPa: id,
                img = img
            },
            success: function (result) {
                DetailsImg();
            },
            error: function () {
                alert("Erro ao cadastrar a imagem!");
            }
        });
    }
    else {
        $('#erro').text('Selecione uma imagem!');
    }
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