//.::Script for working with data in the mainArticle block :::

//.:: Variable to store the current context :::
var currentEntities = 'none';

$(document).ready(function () {

	$('#addNewEntity').click(function () {
		ClearFieldsForCreatingNewEmployee();
		switch (currentEntities) {
			case 'positions': DisplayMessage('Добавление в систему новой должности', true);
				break;
			case 'employees': DisplayMessage('Будет добавлен новый сотрудник', true);
				DisplayModal('.pop-up-createNewEmployee', true);
				$('#inpCreateEmail').focus();
				GetPositionsForSelect();
				GetRolesForSelect();
				break;
			case 'roles': DisplayMessage('Будет добавлена новая роль', true); break;
			default: DisplayMessage('Кнопка нажата, но контекст не выбран', false);
		}
	});

	$('#executeCreatingNewAccount').click(function () {
		CreateNewAccount();
	});

	$('#clearCreatingNewAccount').click(function () {
		ClearFieldsForCreatingNewEmployee();
		$('#inpCreateEmail').focus();
	});

	$('#cancelCreatingNewAccount').click(function () {
		DisplayModal('.pop-up-createNewEmployee', false);
	});
});

function GetPositionsForSelect() {
	$.ajax({
		url: 'https://localhost:44356/content/getpositions',
		method: 'GET',
		success: function (response) {
			let result = JSON.parse(response);
			$('#selectPosition option').remove();
			let select = document.querySelector('#selectPosition');
			let elementCount = result.length;
			for (let i = 0; i < elementCount; i++) {
				let option = document.createElement('option');
				$(option).attr('value', result[i].Id);
				if (i == 1) {
					$(option).attr('selected', 'selected');
				}
				$(option).text(result[i].FullName);
				select.appendChild(option);
			}
		},
		error: function () {
			DisplayRequestErrorWarning('#selectPosition');
		}
	});
}

function GetRolesForSelect() {
	$.ajax({
		url: 'https://localhost:44356/content/getroles',
		method: 'GET',
		success: function (response) {
			let result = JSON.parse(response);
			$('#selectRole option').remove();
			let select = document.querySelector('#selectRole');
			let elementCount = result.length;
			for (let i = 0; i < elementCount; i++) {
				let option = document.createElement('option');
				$(option).attr('value', result[i].Id);
				if (i == 1) {
					$(option).attr('selected', 'selected');
				}
				$(option).text(result[i].RoleName);
				select.appendChild(option);
			}
		},
		error: function () {
			DisplayRequestErrorWarning('#selectRole');
		}
	});
}

function DisplayRequestErrorWarning(id) {
	let select = document.querySelector(id);
	let option = document.createElement('option');
	$(option).text('База данных недоступна');
	select.appendChild(option);
	$(id).css('color', 'red');
}

function ClearFieldsForCreatingNewEmployee() {
	$('#inpCreateEmail').val('');
	$('#inpCreateLastName').val('');
	$('#inpCreateFirstName').val('');
	$('#inpCreateMiddleName').val('');
}

function CreateNewAccount() {

	let employee = {
		email: $('#inpCreateEmail').val(),
		lastname: $('#inpCreateLastName').val(),
		firstname: $('#inpCreateFirstName').val(),
		middlename: $('#inpCreateMiddleName').val(),
		roleId: +$('#selectRole').val(),
		positionId: +$('#selectPosition').val()
	};

	$.ajax({
		url: 'https://localhost:44356/account/createNewAccount',
		method: 'POST',
		contentType: 'application/json',
		data: JSON.stringify(employee),
		success: function (email) {
			if (email != '') {
				GetEmployees();
				DisplayMessage(`Зарегистрирован аккаунт для ${email}`, true);
				DisplayModal('.pop-up-createNewEmployee', false);
			}
			else {
				DisplayMessage("Укажите email и ФИО сотрудника", false);
				DisplayModal('.pop-up-createNewEmployee', false);
			}
		},
		error: function () {
			DisplayMessage('Ошибка запроса создания нового аккаунта', false);
		}
	});
}