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
        var artigo = {
            Id: id,
            Categorias: Categorias
        }

        $.ajax({
            url: "/Artigo/UpdateCategoria",
            type: "POST",
            data: JSON.stringify(artigo),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            beforeSend: function (result) {
                result.setRequestHeader("RequestVerificationToken",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (result) {
                alert(result);
            },
            error: function () {
                alert("Erro ao cadastrar as categorias");
            }
        })
    }
}