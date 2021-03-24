var clickSound = new Audio("/audio/selectClick.wav");
var modifySound = new Audio("/audio/modifyClick.wav");
var removeSound = new Audio("/audio/removeClick.wav");
var recoverSound = new Audio("/audio/recoverClick.wav");

//.:: Variable to store the current context :::
var currentEntities = 'none';
var selectedRow = 'undefined';

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

    $('#infoDisplay').on('click', 'tr', function () {

        let currentEntityAttr;

        switch (currentEntities) {
            case 'employees': currentEntityAttr = 'userid'; break;
            case 'positions': currentEntityAttr = 'positionId'; break;
            case 'roles': currentEntityAttr = 'roleId'; break;
            case 'archEmployees': currentEntityAttr = 'userId'; break;
            case 'archPositions': currentEntityAttr = 'positionId'; break;
            case 'archRoles': currentEntityAttr = 'roleId'; break;
            case 'siteEmail': currentEntityAttr = 'emailId'; break;
            default: currentEntityAttr = 'undefined';
		}

        if ($(this).attr(currentEntityAttr) != null) {
            clickSound.play();
            if (selectedRow != 'undefined') {
                $(selectedRow.row)
                    .css('color', '#04eaed')
                    .css('background-color', selectedRow.defaultBGColor)
                    .css('box-shadow', 'none');
            }

            selectedRow = {
                row: $(this),
                defaultColor: $(this).css('color'),
                defaultBGColor: $(this).css('background-color')
            };

            $(this).css('background-color', 'gold').css('color', 'black').css('box-shadow', '0px 0px 7px 4px orange');
        }
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

function DisplayUpdateButtonForEmployeeModal(id, success) {

    ClearFieldsForCreatingNewEmployee();
    if (success) {
        $('#headerEmplName').text('Редактирование данных сотрудника');
        $(id).css('display', 'block');
        $('#executeCreatingNewAccount').css('display', 'none');
    }
    else {
        $('#headerEmplName').text('Данные нового сотрудника');
        $(id).css('display', 'none');
        $('#executeCreatingNewAccount').css('display', 'block');
    }
    $('#inpCreateEmail').focus();
}

function ClearFieldsForCreatingNewEmployee() {
    $('#inpCreateEmail').val('');
    $('#inpCreateLastName').val('');
    $('#inpCreateFirstName').val('');
    $('#inpCreateMiddleName').val('');
    GetPositionsForSelect(1, 1);
    GetRolesForSelect('#selectRole', 1, 1);
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

function DisplayUpdateButtonForPositionModal(id, success) {

    ClearFieldsForActionWithPositionModal();
    if (success) {
        $('#headerPositionName').text('Редактирование должности');
        $(id).css('display', 'block');
        $('#addNewPosition').css('display', 'none');
    }
    else {
        $('#headerPositionName').text('Должность для ТЧЭ-2 "Омск"');
        $(id).css('display', 'none');
        $('#addNewPosition').css('display', 'block');
    }
    $('#newPositionName').focus();
}

function ClearFieldsForActionWithPositionModal() {
    $('#newPositionAbbreviation').val('');
    $('#newPositionName').val('');
}
