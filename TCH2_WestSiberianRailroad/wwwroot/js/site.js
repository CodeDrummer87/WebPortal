$(document).ready(function () {

    $('#inpSignIn').focus();

    $(document).on('keypress', function (e) {
        if (e.which == 13) {
            SignIn();
        }
    });

    $('#signInButton').click(function () {
        SignIn();
    });

});

function SignIn() {
    let loginModel = {
        email: $('#inpEmail').val(),
        password: $('#inpPassword').val()
    };

    if (loginModel.email === '') {
        DisplayCurrentMessage('Email не указан', false);
    }
    else if (loginModel.password === '') {
        DisplayCurrentMessage('Введите пароль', false);
    }
    else {
        $.ajax({
            url: 'https://localhost:44356/account/signin',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(loginModel),
            success: function (response) {
                ClearForm();
                $('#inpSignIn').focus();
                if (response != null) {
                    window.location.href = response;
                }
                else {
                    DisplayCurrentMessage("Вы не зарегистрированы в системе", false);
                }
            },
            error: function () {
                DisplayCurrentMessage('Ошибка выполнения запроса', false);
            }
        });
    }
}

function DisplayCurrentMessage(message, success) {
    if (success) {
        $('#currentMessage').css('color', 'green').text(message);
    }
    else {
        $('#currentMessage').css('color', 'red').text(message);
    }

    setTimeout(ClearCurrentMessage, 2500);
}

function ClearCurrentMessage() {
    $('#currentMessage').text('');
}

function ClearForm() {
    $('#inpEmail').val('');
    $('#inpPassword').val('');
}