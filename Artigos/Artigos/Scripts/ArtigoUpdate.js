$(document).ready(function () {
    PegarCategorias($('#idArtigo').val());
});

const cate = document.querySelector('#Categorias');

cate.addEventListener('change', SelecionaCategoria);
//Array que armazena as categorias selecionadas
var Categorias = [];

//Passa a categoria selecionada para o array
function SelecionaCategoria() {
    if (cate.value != "") {
        var boo = 1;
        //verifica se a categoria selecionada já está na lista
        for (var i = 0; i < Categorias.length; i++) {
            if (cate.value == Categorias[i].Id) {
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
        if (id == Categorias[i].Id) {
            Categorias.splice(i, 1);
            PreencharDiv();
            break;
        }
    }
}

function Create(id) {
    if (Categorias.length != 0) {

        var controle = ValidarCategoria();

        if (controle == false) {

            var form = $('#__AjaxAntiForgeryForm');
            var token = $('input[name="__RequestVerificationToken"]', form).val();

            $.ajax({
                url: "/Artigo/UpdateCategoria",
                type: "POST",
                data: {
                    __RequestVerificationToken: token,
                    id: id,
                    categorias: Categorias
                },
                contentType: "application/x-www-form-urlencoded;charset=utf-8",
                dataType: "json",
                success: function (result) {
                    alert(result);
                },
                error: function () {
                    alert("Erro ao cadastrar as categorias");
                }
            });
        }
    }
}

//Valida as categorias para poder mandar para o servidor
function ValidarCategoria() {
    var controle = true;
    //Ela pode vir vazia do banco de dados
    if (CategoriasBanco.length == 0) {
        controle = false;

        return controle;
    }

    for (var i = 0; i < CategoriasBanco.length; i++) {
        for (var j = 0; j < Categorias.length; j++) {
            if (CategoriasBanco[i].Id == Categorias[j].Id) {
                controle = true;
                break;
            }
            else {
                controle = false;
            }
        }

        if (controle == false) {
            return controle;
        }
    }

    for (var i = 0; i < Categorias.length; i++) {
        for (var j = 0; j < CategoriasBanco.length; j++) {
            if (CategoriasBanco[j].Id == Categorias[i].Id) {
                controle = true;
                break;
            }
            else {
                controle = false;
            }
        }

        if (controle == false) {
            return controle;
        }
    }

    return controle;
}

var CategoriasBanco = [];
function PegarCategorias(id) {
    $.ajax({
        url: "/Artigo/PegarCategorias/" + id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $.each(result, function (key, item) {
                var Categoria = new Object();
                Categoria.Id = item.Id;
                Categoria.NomeCategoria = item.NomeCategoria;
                CategoriasBanco.push(Categoria);
                Categorias.push(Categoria);
            });

            PreencharDiv();
        },
        error: function () {
            alert("Erro ao buscar as categorias");
        }
    });
}