
$(document).ready(function() {   
    
    // Close button on dialog
    $(document).on("click", "#btnCloseMsgLogin", function (e) {
        $("#loginbox").height(220);
    });

    // Login form Login | Senha
    $("#loginform > form").validate({
        errorLabelContainer: $("#validationLoginPassword"),

        rules: {
            UserName: {
                required: true,
                email: true
            },
            Password: {
                required: true
            }
        },
        messages: {
            UserName: {
                required: "Campo Email não pode estar vazio",
                email: "Formato de E-Mail inválido."
            },
            Password: {
                required: "Campo Senha não pode estar vazio",
            }
        },
        showErrors: function (errorMap, errorList) {

            this.defaultShowErrors();

            var heightBox = 265 + (this.numberOfInvalids() * 25);

            $("#loginbox").height(heightBox);

        }
    });

    // Forgot Password
    $("#recoverform").validate({
        errorLabelContainer: $("#validationForgotPassword"),
        //errorLabelContainer: $("span.msgLoginError"),

        rules: {
            Email: {
                required: true,
                email: true
            }
        },
        messages: {
            Email: {
                required: "Campo Email não pode estar vazio",
                email: "Formato de E-Mail inválido."
            }
        },
        showErrors: function (errorMap, errorList) {

            this.defaultShowErrors();

            var heightBox = 187 + (this.numberOfInvalids() * 25);

            $("#loginbox").height(heightBox);

        }
    });

    // Request Token
    $("#tokenform").validate({
        errorLabelContainer: $("#validationToken"),

        rules: {
            Token: {
                required: true
            }
        },
        messages: {
            Token: {
                required: "Campo Token não pode estar vazio"
            }
        },
        showErrors: function (errorMap, errorList) {

            this.defaultShowErrors();

            var heightBox = 195 + (this.numberOfInvalids() * 25);

            $("#loginbox").height(heightBox);

        }
    });

    // Reset Password
    $("#resetpasswordform").validate({
        errorLabelContainer: $("#validationResetPassword"),

        rules: {
            NewPassword: {
                required: true
            },
            ConfirmPassword: {
                equalTo: "#newpassword"
            }
        },
        messages: {
            NewPassword: {
                required: "Campo Nova Senha não pode estar vazio"
            },
            ConfirmPassword: {
                equalTo: "Nova Senha e Confirmação não conferem"
            }

        },
        showErrors: function (errorMap, errorList) {

            this.defaultShowErrors();

            var heightBox = 195 + (this.numberOfInvalids() * 25);

            $("#loginbox").height(heightBox);

        }
    });
});