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
                btnImg.setAttribute('onclick', 'Img(' + result.Id + ',' + id + ')');

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
        url: "/Imagem/GetImg/" + id,
        contentType: "application/json; charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (result) {
            var html = '';
            var num = 1;
            $.each(result, function (key, item) {
                html += '<a onclick="GetDetailsImg(' + item.Id + ', ' + item.ParagrafoId + ')" data-toggle="modal" data-target="#exampleModal">';
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
        var confi = confirm("Você realmente deseja excluir esse parágrafo?");

        if (confi) {
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
    }
    else {
        $('#erro').text('Esse campo é obrigatório!');
    }
}

var img = $("#fileUpload").get(0);
function Img(idPa, idAr) {
    img.setAttribute('onchange', 'validar(' + idPa + ', ' + idAr + ')');
    $('#fileUpload').click();
}

function validar(idPa, idAr) {
    var nome = "Extensões permitidas: gif, png, jpg. Tamanho de 2Mb";
    img = $("#fileUpload").get(0)

    if (img.files.length > 0) {
        var ex = ["gif", "png", "jpg"];
        var ext = img.files[0].name.split('.').pop().toLowerCase();

        if (ex.lastIndexOf(ext) != -1) {
            CreateImg(idAr, idPa);
        }
        else {
            alert(nome);
        }
    }
}

function CreateImg(idAr, idPa) {
    var img = $("#fileUpload").get(0);
    var files = img.files;

    if (img.files.length > 0) {
        var fileData = new FormData();
        fileData.append(files[0].name, files[0]);

        $.ajax({
            url: "/Imagem/Create?idAr=" + idAr + "&idPa=" + idPa,
            type: "POST",
            data: fileData,
            processData: false,
            contentType: false,
            success: function (result) {
                DetailsImg(idPa);
            },
            error: function () {
                alert("Erro ao cadastrar a imagem!");
            }
        });
    }
    else {
        alert('Selecione uma imagem!');
    }
}

const btnDe = document.querySelector("#btnDe");
const btnUp = document.querySelector("#btnUp");

function GetDetailsImg(idIma, idPa) {
    $.ajax({
        url: "/Imagem/GetDetailsImg/" + idIma,
        contentType: "application/json; charset=utf-8",
        type: "GET",
        dataType: "json",
        success: function (result) {
            const img = document.querySelector('#img');
            img.setAttribute("src", result.Image);

            btnDe.setAttribute('onclick', 'DeleteImg(' + result.Id + ', ' + idPa + ')');
            btnUp.setAttribute('onclick', 'UpdateImg(' + result.Id + ', ' + idPa + ')');
        },
        error: function () {
            alert("Erro ao buscar as imagens!");
        }
    });
}

function DeleteImg(idIma, idPa) {
    if (idIma != null && idIma != 0) {
        var con = confirm("Deseja deletar essa imagem?");
        btnDe.setAttribute('data-dismiss', '');

        if (con) {
            btnDe.setAttribute('data-dismiss', 'modal');

            var form = $('#__AjaxAntiForgeryForm');
            var token = $('input[name="__RequestVerificationToken"]', form).val();

            $.ajax({
                url: "/Imagem/Delete/" + idIma,
                type: "POST",
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                data: { __RequestVerificationToken: token },
                success: function () {
                    DetailsImg(idPa);
                },
                error: function () {
                    alert("Erro ao excluir a imagem!");
                }
            });
        }
    }
}

function UpdateImg(idIma, idPa) {
    if (idIma != null && idIma != 0) {
        var img = $("#fileUpload").get(0);
        var files = img.files;
        btnDe.setAttribute('data-dismiss', '');

        if (img.files.length > 0) {
            var fileData = new FormData();
            fileData.append(files[0].name, files[0]);

            $.ajax({
                url: "/Imagem/Update/" + idIma,
                type: "POST",
                data: fileData,
                processData: false,
                contentType: false,
                success: function (result) {
                    btnDe.setAttribute('data-dismiss', 'modal');
                    DetailsImg(idPa);
                },
                error: function () {
                    alert("Erro ao cadastrar a imagem!");
                }
            });
        }
        else {
            alert('Selecione uma imagem!');
        }
    }
}

function Sair() {
    $('#btnCreate').show();
    $('#btnExcluir').hide();
    $('#btnUpdate').hide();
    $('#btnSair').hide();
    $('#texto').val('');
    $('#listaImagem').html('');

    const btnImg = document.querySelector('#btnCadastroImg');
    btnImg.setAttribute('class', 'btn btn-info btnCadastroImgFalse');
}