function ValidarCampo() {

    if ($('#txtSenhaAntiga').val() == "" || $('#txtSenhaNova').val() == "" || $('#txtConfirmaSenha').val() == "") {
        $('#senhaIncorreta').text("Preencha os campos corretamente");
        return false;
    }
    else {
        if ($('#txtSenhaNova').val() == $('#txtConfirmaSenha').val()) {
            if ($('#txtConfirmaSenha').val().length < 8) {
                $('#senhaIncorreta').text("As senha tem que ser maior que 7");
                return false;
            }
            else {
                return true;
            }
        }
        else {
            $('#senhaIncorreta').text("As senha não estão iguais");
            return false;
        }
    }
}

function UpdatePassword() {
    if (ValidarCampo()) {
        var senha = {
            SenhaAntiga: $('#txtSenhaAntiga').val(),
            SenhaNova: $('#txtSenhaNova').val(),
            SenhaConfirma: $('#txtConfirmaSenha').val()
        };

        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        $.ajax({
            url: "/Escritor/UpdatePassword",
            data: {
                __RequestVerificationToken: token,
                senha: senha
            },
            type: "POST",
            contentType: "application/x-www-form-urlencoded;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result == "") {
                    $('#senhaIncorreta').text("");
                    alert("Senha alterada!");

                    $('#txtSenhaAntiga').val('');
                    $('#txtSenhaNova').val('');
                    $('#txtConfirmaSenha').val('');
                }
                else {
                    $('#senhaIncorreta').text(result);
                }
            },
            error: function () {
                alert('Erro ao alterar senha!');
            }
        });
    }
}