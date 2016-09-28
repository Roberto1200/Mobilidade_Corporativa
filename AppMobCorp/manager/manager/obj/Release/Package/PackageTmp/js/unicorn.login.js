/**
 * Unicorn Admin Template
 * Version 2.0.1
 * Diablo9983 -> diablo9983@gmail.com
**/

$(document).ready(function () {

    var login = $('#loginform');                    // Login Area
    var recover = $('#recoverform');                // Forgot Password Area
    var register = $('#registerform');              // Register Form (Unknown)
    var tokenform = $('#tokenform');                // Token request
    var reset_password = $('#resetpasswordform');   // Password reset

    var login_recover = $('#loginform, #recoverform');
    var login_register = $('#loginform, #registerform, #tokenform, #resetpasswordform');
    var recover_register = $('#recoverform, #registerform, #tokenform, #resetpasswordform');

    var login_token = $('#loginform, #recoverform, #resetpasswordform');
    var token_reset_password = $('#loginform, #recoverform, #tokenform');

    var dialog_validation = $('#dialogValidation');

    var loginbox = $('#loginbox');
    var speed = 300;

    var initLogin = function () {
        $('#username').val('');
        $('#password').val('');
        $('#token').val('');
        $('#newpassword').val('');
        $('#confirmpassword').val('');
        $("#btnLogin").prop('disabled', false);
        $("#SalvSenha").prop('disabled', true);
        login.show();
        $('#recoverform, #tokenform, #resetpasswordform').css("display", "none");
    };

    initLogin();

    $('.flip-link.to-recover').click(function () {

        var validator = $("#loginform > form").validate();
        validator.resetForm();

        $("#btnLogin").prop('disabled', true);
        $("#SalvSenha").prop('disabled', true);

        login_register.css('z-index', '100').fadeTo(speed, 0.01, function () {
            loginbox.animate({ 'height': '187px' }, speed, function () {
                recover.fadeTo(speed, 1).css('z-index', '200');
                recover.show();
                login_register.hide();
            });
        });

    });

    $('.flip-link.to-token').click(function () {

        $("#btnLogin").prop('disabled', true);
        $("#SalvSenha").prop('disabled', true);

        recover.validate();

        if (recover.valid()) {

            // AJAX: Send Mail Instructions
            var email = $('#email').val();

            $.ajax({
                url: ManagerUrlSettings.SendPasswordInstructions,
                method: 'get',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: {
                    'email': email
                },
                beforeSend: function (xhr) {
                    $("#validationForgotPassword > label").remove();
                    $('#msgLoading').show();
                    loginbox.height(187);
                },
                success: function (success) {

                    if (success) {
                        login_token.css('z-index', '100').fadeTo(speed, 0.01, function () {
                            loginbox.animate({ 'height': '177px' }, speed, function () {
                                tokenform.fadeTo(speed, 1).css('z-index', '200');
                                tokenform.show();
                                login_token.hide();
                            });
                        });
                    } else {
                        $("#validationForgotPassword > label").remove();
                        $("#validationForgotPassword").append('<label for="email" class="error">E-Mail invalido</label>');
                        $("#validationForgotPassword").show();
                        loginbox.height(220);
                    }

                    $('#msgLoading').hide();

                },
                error: function (req, status, err) {

                    $("#validationForgotPassword").append('<label for="email" class="error">Erro ao solicitar envio de senha</label>');
                    console.log('Erro ao solicitar envio de senha', status, err);

                    $('#msgLoading').hide();

                }
            });
        }

    });

    $('.flip-link.to-reset-password').click(function () {

        $("#btnLogin").prop('disabled', true);
        $("#SalvSenha").prop('disabled', false);

        tokenform.validate();

        if (tokenform.valid()) {
            var email = $('#email').val();
            var token = $('#token').val();

            $.ajax({
                url: ManagerUrlSettings.ValidateToken,
                method: 'get',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: {
                    'email': email,
                    'token': token
                },
                beforeSend: function (xhr) {
                    $("#validationForgotPassword > label").remove();
                    $('#msgLoading').show();
                },
                success: function (success) {

                    if (success) {
                        $('#emailNewPassword').val(email);
                        $('#tokenNewPassword').val(token);

                        token_reset_password.css('z-index', '100').fadeTo(speed, 0.01, function () {
                            loginbox.animate({ 'height': '200px' }, speed, function () {
                                reset_password.fadeTo(speed, 1).css('z-index', '200');
                                reset_password.show();
                                token_reset_password.hide();
                            });
                        });

                    } else {

                        $("#validationToken > label").remove();
                        $("#validationToken").append('<label for="token" class="error">Token invalido</label>');
                        $("#validationToken").show();
                        loginbox.height(220);

                    }

                    $('#msgLoading').hide();

                },
                error: function (req, status, err) {

                    $('#msgLoading').hide();
                    $("#validationToken").append('<label for="token" class="error">Erro ao solicitar envio de senha</label>');
                    console.log('Erro ao solicitar envio de senha', status, err);
                }
            });
        }
    });


    $('.flip-link.to-login').click(function () {

        initLogin();
        
        var validator = $("#loginform > form").validate();
        validator.resetForm();
        

        dialog_validation.remove();

        recover_register.css('z-index', '100').fadeTo(speed, 0.01, function () {
            loginbox.animate({ 'height': '220px' }, speed, function () {
                login.fadeTo(speed, 1).css('z-index', '200');
                initLogin();
            });
        });
    });
    $('.flip-link.to-register').click(function () {
        login_recover.css('z-index', '100').fadeTo(speed, 0.01, function () {
            loginbox.animate({ 'height': '285px' }, speed, function () {
                register.fadeTo(speed, 1).css('z-index', '200');
            });
        });
    });

    if ($.browser.msie == true && $.browser.version.slice(0, 3) < 10) {
        $('input[placeholder]').each(function () {
            var input = $(this);
            $(input).val(input.attr('placeholder'));
            $(input).focus(function () {
                if (input.val() == input.attr('placeholder')) {
                    input.val('');
                }
            });
            $(input).blur(function () {
                if (input.val() == '' || input.val() == input.attr('placeholder')) {
                    input.val(input.attr('placeholder'));
                }
            });
        });
    }
});