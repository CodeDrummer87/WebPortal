var clickSound = new Audio("/audio/selectClick.wav");
var modifySound = new Audio("/audio/modifyClick.wav");
var removeSound = new Audio("/audio/removeClick.wav");
var recoverSound = new Audio("/audio/recoverClick.wav");

$(document).ready(function () {

    $('#inpSignIn').focus();

    $(document).on('keypress', function (e) {
        if (e.which == 13) {
            clickSound.play();
            SignIn();
        }
    });

    $('#signInButton').click(function () {
        clickSound.play();
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

function DisplayModal(id, isShow) {
    if (isShow) {
        $('.nap').css('display', 'flex');
        $(id).css('display', 'flex');
    }
    else {
        $('.nap').css('display', 'none');
        $(id).css('display', 'none');
	}
}

function DisplayUpdateButtonForModal(id, success) {

    ClearFieldsForCreatingNewEmployee();
    if (success) {
        $('#headerBasement > h3').text('Редактирование данных сотрудника');
        $(id).css('display', 'block');
        $('#executeCreatingNewAccount').css('display', 'none');
    }
    else {
        $('#headerBasement > h3').text('Данные нового сотрудника');
        $(id).css('display', 'none');
        $('#executeCreatingNewAccount').css('display', 'block');
    }
    $('#inpCreateEmail').focus();
}

function DisplayArchiveControlPanel(on) {
    if (on) {
        $('#recoverEntity').css('display', 'block');
        $('#addNewEntity').css('display', 'none');
        $('#updateEntity').css('display', 'none');
        $('#deleteEntity').css('display', 'none');
    }
    else {
        $('#recoverEntity').css('display', 'none');
        $('#addNewEntity').css('display', 'block');
        $('#updateEntity').css('display', 'block');
        $('#deleteEntity').css('display', 'block');
	}
}
