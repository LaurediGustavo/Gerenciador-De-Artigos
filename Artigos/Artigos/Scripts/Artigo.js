const cate = document.querySelector('#Categorias');

cate.addEventListener('change', SelecionaCategoria);
//Array que armazena as categorias selecionadas
var Categorias = [];

//Passa a categoria selecionada para o array
function SelecionaCategoria() {
    if (cate.value != "") {
        var boo = 1;
        //verifica se a categoria selecionada já está na lista
        for (var i = 0; i < cate.length; i++) {
            if (cate.value == cate[i].Id) {
                boo = 0;
                break;
            }
        }

        //Caso a categoria não esteja na lista, adiciona ela
        if (boo == 1) {
            var Categoria = new Object();
            Categoria.Id = cate.value;
            Categoria.NomeCategoria = cate.options[cate.selectedIndex].text;
            Categorias.push(Categoria);
            PreencharDiv();
        }
    }
}

const divCategoria = document.querySelector('#divCategoria');
//Exibe as categorias selecionadas na tela
function PreencharDiv() {
    $('#divCategoria').empty();//Limpa a div
    for (var item of Categorias) {
        const div = document.createElement('div');
        const span = document.createElement('span');
        const input = document.createElement('input');

        div.setAttribute('class', 'listaCategoriaSelecionada')

        span.innerText = item.NomeCategoria;

        input.setAttribute('type', "submit");
        input.setAttribute('value', 'X');
        input.setAttribute('onclick', 'removerCategoria(' + item.Id + ')')

        div.appendChild(span);
        div.appendChild(input);

        divCategoria.appendChild(div);
    }
}

function removerCategoria(id) {
    for (var i = 0; i < Categorias.length; i++) {
        if (id == categoriaSelecionada[i].Id) {
            categoriaSelecionada.splice(i, 1);
            PreencharDiv();
            break;
        }
    }
}

const checkAtivo = document.querySelector('#txtAtivo');
function Create() {
    if (ValidarCampo() == true) {

        var artigo = new FormData();
        artigo.append("Img", $('#txtCapa')[0].files[0]);
        artigo.append("Titulo", $('#txtTitulo').val());
        artigo.append("Ativa", checkAtivo.checked);

        $.ajax({
            url: "/Artigo/Create",
            type: "POST",
            data: artigo,
            contentType: false,
            processData: false,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("RequestVerificationToken",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (m) {
                alert(m);
            },
            error: function (m) {
                alert('Erro no cadastro!');
            }
        });
    }
}

function ValidarCampo() {
    var boo = 0;

    if ($('#txtTitulo').val() == "") {
        $('#spanTitulo').text('Esse campo é obrigatório!');
        boo++;
    }
    else {
        $('#spanTitulo').text('');
    }

    var fileUpload = $('#txtCapa').get(0);
    var file = fileUpload.files;
    if (file.length == 0) {
        $('#spanCapa').text('Esse campo é obrigatório!');
        boo++;
    }
    else {
        $('#spanCapa').text('');
        boo += ValidarImagem();
    }

    if (boo == 0) {
        return true;
    }
    else {
        return false;
    }
}

function ValidarImagem() {
    var controle = 0;
    var fileInput = $('#txtCapa');
    var extPermitidas = ['jpg', 'png', 'gif'];

    var fileSize = fileInput.get(0).files[0].size;

    if (fileSize > 200000) {
        $('#spanCapa').text('Arquivo excedeu o limite permitido de 2mb');
        controle = 1;
    } else if (ValidaExtensao() == true) {
        $('#spanCapa').text('');
    } else {
        $('#spanCapa').text('As extensões permitidas são: jpg, png, gif');
        controle = 1;
    }

    if (controle == 0) {
        return 0;
    }
    else {
        return 1;
    }
}

function ValidaExtensao() {
    var fileInput = $('#txtCapa');
    var extPermitidas = ['jpg', 'png', 'gif'];

    for (var i = 0; i < extPermitidas.length; i++) {
        if (extPermitidas[i] == fileInput.val().split('.').pop()) {
            return true;
        }
    }

    return false;
}