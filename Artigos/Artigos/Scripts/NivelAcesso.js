function UpdateNivelAcesso(id) {
    const select = document.getElementById('Id');

    if (select.options[select.selectedIndex].value == "") {
        $('#nivelAcesso').text('Selecione uma opção');
    }
    else {
        $('#nivelAcesso').text('');

        var escritor = {
            Id: id,
            NivelAcesso: select.options[select.selectedIndex].value
        };

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        $.ajax({
            url: "/Escritor/Detalhes",
            data: {
                __RequestVerificationToken: token,
                escritor: escritor
            },
            type: "POST",
            contentType: "application/x-www-form-urlencoded;charset=utf-8",
            dataType: "json",
            success: function (result) {
                alert("Alteração realizada com sucesso!");
                window.location.reload();
            },
            error: function (result) {
                $('#nivelAcesso').text(result.fail);
            }
        });
    }
}