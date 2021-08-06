const categoria = document.querySelector('#Categorias');

categoria.addEventListener('change', SelecionaCategoria);
//Array que armazena as categorias selecionadas
var categoriaSelecionada = [];

//Passa a categoria selecionada para o array
function SelecionaCategoria() {
    if (categoria.value != "") {
        var boo = 1;
        //verifica se a categoria selecionada já está na lista
        for (var i = 0; i < categoriaSelecionada.length; i++) {
            if (categoria.value == categoriaSelecionada[i].Id) {
                boo = 0;
                break;
            }
        }

        //Caso a categoria não esteja na lista, adiciona ela
        if (boo == 1) {
            var item = new Object();
            item.Id = categoria.value;
            item.NomeCategoria = categoria.options[categoria.selectedIndex].text;
            categoriaSelecionada.push(item);
            PreencharDiv();
        }
    }
}

const divCategoria = document.querySelector('#divCategoria');
//Exibe as categorias selecionadas na tela
function PreencharDiv() {
    $('#divCategoria').empty();//Limpa a div
    for (var item of categoriaSelecionada) {
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
    for (var i = 0; i < categoriaSelecionada.length; i++) {
        if (id == categoriaSelecionada[i].Id) {
            categoriaSelecionada.splice(i, 1);
            PreencharDiv();
            break;
        }
    }
}

function Create() {
    if (ValidarCampo()) {
        alert('certo');
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

    if (window.FormData == undefined) {
        $('#spanCapa').text('Esse campo é obrigatório!');
        boo++;
    }
    else {
        $('#spanCapa').text('');
    }

    if (boo == 0) {
        return true;
    }
    else {
        return false;
    }
}