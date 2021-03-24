//.::Script for working with data in the mainArticle block :::

$(document).ready(function () {

	$('#addNewEntity').click(function () {
		clickSound.play();
		ClearFieldsForCreatingNewEmployee();
		switch (currentEntities) {
			case 'positions': DisplayMessage('Добавление в систему новой должности', true);
				DisplayUpdateButtonForPositionModal('#updateCurrentPosition', false);
				DisplayModal('.pop-up-actionWithPosition', true);
				break;
			case 'employees': DisplayMessage('Создание аккаунта нового сотрудника', true);
				DisplayUpdateButtonForModal('#executeUpdateEmployeeData', false);
				DisplayModal('.pop-up-createNewEmployee', true);
				GetPositionsForSelect(1, 1);
				GetRolesForSelect('#selectRole', 1, 1);
				break;
			case 'roles': DisplayMessage('Будет добавлена новая роль', true); break;
			case 'siteEmail': DisplayMessage('Актуальный почтовый аккаунт для сайта ТЧЭ-2 \"Омск\" будет изменён', true); break;
			default: DisplayMessage('Кнопка нажата, но контекст не выбран', false);
		}
	});

	$('#deleteEntity').click(function () {
		clickSound.play();
		switch (currentEntities) {
			case 'positions':
				if (selectedRow != 'undefined') {
					DisplayMessage('Должность будет перенесена в архив', true);
					DisplayButtonIntoConfirmModal('#positionRemoveConfirmButton');
					DisplayModal('.pop-up-confirmRemove', true);
				}
				else {
					DisplayMesa('Не выбрана должность', false);
				}
				break;
			case 'employees':
				if (selectedRow != 'undefined') {
					DisplayMessage('Аккаунт сотрудника будет перенесён в архив', true);
					DisplayButtonIntoConfirmModal('#employeeRemoveConfirmButton');
					DisplayModal('.pop-up-confirmRemove', true);
				}
				else {
					DisplayMessage('Не выбран сотрудник', false);
				}
				break;
			case 'roles': DisplayMessage('Выбранная роль будет перенесена в архив', true); break;
			case 'siteEmail': DisplayMessage('Актуальный почтовый аккаунт для сайта ТЧЭ-2 \"Омск\" будет перенесён в архив', true); break;
			default: DisplayMessage('Кнопка нажата, но контекст не выбран', false);
		}
	});

	$('#updateEntity').click(function () {
		clickSound.play();
		switch (currentEntities) {
			case 'positions':
				if (selectedRow != 'undefined') {
					DisplayUpdateButtonForPositionModal('#updateCurrentPosition', true);
					let positionId = $(selectedRow.row).attr('positionid');
					GetCurrentPositionData(positionId);
					DisplayModal('.pop-up-actionWithPosition', true);
				}
				else {
					DisplayMessage('Не выбрана должность', false);
				}
				break;
			case 'employees':
				if (selectedRow != 'undefined') {
					DisplayUpdateButtonForEmployeeModal('#executeUpdateEmployeeData', true);					
					let userId = $(selectedRow.row).attr('userid');
					GetCurrentEmployeeData(userId);
					DisplayModal('.pop-up-createNewEmployee', true);
				}
				else {
					DisplayMessage('Не выбран сотрудник', false);
				}
				break;
			case 'roles': DisplayMessage('Изменить роль', true); break;
			case 'siteEmail': DisplayMessage('Изменить ктуальный почтовый аккаунт для сайта ТЧЭ-2 \"Омск\"', true); break;
			default: DisplayMessage('Кнопка нажата, но контекст не выбран', false);
		}
	});

	$('#recoverEntity').click(function () {
		clickSound.play();
		if (selectedRow != 'undefined') {
			DisplayModal('.pop-up-confirmRecover', true);
		}
		else {
			DisplayMessage('Не выбран сотрудник из архива', false);
		}
	});

	$('#executeCreatingNewAccount').click(function () {
		modifySound.play();
		CreateNewAccount();
	});

	$('#clearCreatingNewAccount').click(function () {
		clickSound.play();
		ClearFieldsForCreatingNewEmployee();
		$('#inpCreateEmail').focus();
	});

	$('#cancelCreatingNewAccount').click(function () {
		clickSound.play();
		DisplayModal('.pop-up-createNewEmployee', false);
	});

	$('#executeUpdateEmployeeData').click(function () {
		modifySound.play();
		UpdateCurrentEmployeeData();
	});

	$('#executeUpdatePositionData').click(function () {
		modifySound.play();
		UpdateCurrentPositionData();
	});

	$('#employeeRemoveConfirmButton').click(function () {
		removeSound.play();
		DisplayModal('.pop-up-confirmRemove', false);
		if (selectedRow != 'undefined') {
			let userId = $(selectedRow.row).attr('userid');
			ArchiveEmployee(userId);
		}
	});

	$('#positionRemoveConfirmButton').click(function () {
		removeSound.play();
		DisplayModal('.pop-up-confirmRemove', false);
		if (selectedRow != 'undefined') {
			let positionId = $(selectedRow.row).attr('positionId');
			ArchivePosition(positionId);
		}
	});

	$('#cancelEmployeeRemoveButton').click(function () {
		clickSound.play();
		DisplayModal('.pop-up-confirmRemove', false);
	});

	$('#recoverConfirmButton').click(function () {
		recoverSound.play();
		DisplayModal('.pop-up-confirmRecover', false);
		if (selectedRow != 'undefined') {
			let userId = $(selectedRow.row).attr('userid');
			RecoverEmployeeFromArchive(userId);
		}
	});

	$('#cancelRecoverButton').click(function () {
		clickSound.play();
		DisplayModal('.pop-up-confirmRecover', false);
	});

	$('#addNewPosition').click(function () {
		modifySound.play();
		DisplayModal('.pop-up-actionWithPosition', false);
		SaveNewPosition();
	});

	$('#closePositionModal').click(function () {
		clickSound.play();
		DisplayModal('.pop-up-actionWithPosition', false);
		ClearFieldsPositionModal();
	});

	$('#archEmployees').click(function () {
		clickSound.play();
		pageNumber = 0;
		currentEntities = 'archEmployees';
		DisplayArchiveControlPanel(true);
		GetEmployees(0);
		DisplayMessage("Архив сотрудников ТЧЭ-2 'Омск' загружается", true);
	});

	$('#archPositions').click(function () {
		clickSound.play();
		pageNumber = 0;
		currentEntities = 'archPositions';
		DisplayArchiveControlPanel(true);
		GetPositions(0);
		DisplayMessage("Архив должностей ТЧЭ-2 'Омск' загружается", true);
	});

	$('#archRoles').click(function () {
		clickSound.play();
		pageNumber = 0;
		currentEntities = 'archRoles';
		DisplayArchiveControlPanel(true);
		GetRoles(0);
		DisplayMessage("Архив ролей сайта ТЧЭ-2 'Омск' загружается", true);
	});

	$('#siteEmail').click(function () {
		clickSound.play();
		pageNumber = 0;
		currentEntities = 'siteEmail';
		DisplayArchiveControlPanel(false);
		GetAppEmailAccounts();
		DisplayMessage("Данные актуального почтового аккаунта сайта ТЧЭ-2 \"Омск\" загружаются", true);
	});
});

function GetPositionsForSelect(isActual, index) {
	$.ajax({
		url: 'https://localhost:44356/content/getpositions?page=' + 0 + "&isActual=" + isActual,
		method: 'GET',
		success: function (response) {
			let result = JSON.parse(response);
			$('#selectPosition option').remove();
			let select = document.querySelector('#selectPosition');
			let elementCount = result.length;
			for (let i = 0; i < elementCount; i++) {
				let option = document.createElement('option');
				$(option).attr('value', result[i].Id);
				if (i == index) {
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

function GetRolesForSelect(id, isActual, index) {
	$.ajax({
		url: 'https://localhost:44356/content/getroles?page=' + 0 + "&isActual=" + isActual,
		method: 'GET',
		success: function (response) {
			let result = JSON.parse(response);
			$(id + ' option').remove();
			let select = document.querySelector(id);
			let elementCount = result.length;
			for (let i = 0; i < elementCount; i++) {
				let option = document.createElement('option');
				$(option).attr('value', result[i].Id);
				if (i == index) {
					$(option).attr('selected', 'selected');
				}
				$(option).text(result[i].RoleName);
				select.appendChild(option);
			}
		},
		error: function () {
			DisplayRequestErrorWarning(id);
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
				GetEmployees(1);
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

function GetAppEmailAccounts() {
	$.ajax({
		url: 'https://localhost:44356/content/GetAppEmailAccount?page=' + pageNumber,
		method: 'GET',
		success: function (response) {
			selectedRow = 'undefined';
			let result = JSON.parse(response);
			DisplayAppEmailAccounts(result);
		},
		error: function () {
			DisplayMessage("Ошибка загрузки данных актуального почтового аккаунта сайта ТЧЭ-2 \"Омск\"", false);
		}
	});
}

function DisplayAppEmailAccounts(result) {
	$('#infoDisplay table').remove();

	$.ajax({
		url: 'https://localhost:44356/content/getemailcount',
		method: 'GET',
		success: function (count) {
			currentCount = count;

			let div = document.querySelector('#infoDisplay');
			let table = document.createElement('table');
			let caption = document.createElement('caption');
			caption.innerText = 'Список почтовых аккаунтов сайта ТЧЭ-2 "Омск"';
			table.appendChild(caption);
			let hRow = document.createElement('tr');
			GetThForTable(table, hRow, "№");
			GetThForTable(table, hRow, "Почтовый адрес");
			GetThForTable(table, hRow, "Действующий почтовый адрес");

			let rows = result.length;
			for (let i = 0; i < rows; i++) {
				let row = document.createElement('tr');
				if (i % 2 == 0) {
					$(row).css('background-color', '#2e2e2e');
				}
				else {
					$(row).css('background-color', '#3f3c3c');
				}
				$(row).attr('emailId', result[i].Id);
				GetTdForTable(table, row, i + 1);
				GetTdForTable(table, row, result[i].Email);
				let isActual = result[i].IsActual == 1 ? 'Да' : 'Нет';
				GetTdForTable(table, row, isActual);
			}
			div.appendChild(table);
			SetControlPanels(count);
		}
	});
}

function GetCurrentEmployeeData(userId) {
	$.ajax({
		url: 'https://localhost:44356/content/GetCurrentUserById?userId=' + userId,
		method: 'GET',
		success: function (response) {
			let user = JSON.parse(response);
			DisplayEmployeeDataInModal(user[0]);
		},
		error: function () {

		}
	});
}

function DisplayEmployeeDataInModal(user) {
	$('#inpCreateEmail').val(user.Email);
	$('#inpCreateLastName').val(user.LastName);
	$('#inpCreateFirstName').val(user.FirstName);
	$('#inpCreateMiddleName').val(user.MiddleName);
	GetRolesForSelect('#selectRole', 1, user.RoleId - 1);
	GetPositionsForSelect(1, user.PositionId - 1);
}

function UpdateCurrentEmployeeData() {

	userId = $(selectedRow.row).attr('userid');
	email = $('#inpCreateEmail').val();
	firstName = $('#inpCreateFirstName').val();
	lastName = $('#inpCreateLastName').val();
	middleName = $('#inpCreateMiddleName').val();
	positionId = +$('#selectPosition').val();
	roleId = +$('#selectRole').val();

	$.ajax({
		url: 'https://localhost:44356/content/updateEmployeeData?userId=' + userId +
			'&email=' + email + '&firstName=' + firstName + '&lastName=' + lastName + '&middleName=' + middleName +
			'&positionId=' + positionId + '&roleId=' + roleId,
		method: 'PUT',
		success: function (response) {
			if (response != 'Сотрудник не найден') {
				GetEmployees(1);
				DisplayModal('.pop-up-createNewEmployee', false);
				DisplayMessage(response, true);
			}
			else {
				DisplayMessage(response, false);
			}
		},
		error: function () {
			DisplayMessage("Ошибка выполнения запроса на изменение данных сотрудника", false);
		}
	});
}

function ArchiveEmployee(userId) {
	$.ajax({
		url: 'https://localhost:44356/content/removeEmployee?userId=' + userId,
		method: 'DELETE',
		success: function (response) {
			GetEmployees(1);
			DisplayMessage(response, true);
		},
		error: function () {
			DisplayMessage('Ошибка выполнения запроса удаления аккаунта сотрудника');
		}
	});
}

function RecoverEmployeeFromArchive(userId) {
	$.ajax({
		url: 'https://localhost:44356/content/recoverEmployeeFromArchive?userId=' + userId,
		method: 'PUT',
		success: function (response) {
			GetEmployees(0);
			DisplayMessage(response, true);
		},
		error: function () {
			DisplayMessage('Ошибка выполнения запроса восстановления сотрудника');
		}
	});
}

function ClearFieldsPositionModal() {
	$('#newPositionName').val('');
	$('#newPositionAbbreviation').val('');
}

function SaveNewPosition() {

	let positionData = {
		positionName: $('#newPositionName').val(),
		abbreviation: $('#newPositionAbbreviation').val(),
	};

	if (positionData.positionName != '') {
		$.ajax({
			url: 'https://localhost:44356/content/saveNewPosition',
			method: 'POST',
			contentType: 'application/json',
			data: JSON.stringify(positionData),
			success: function (response) {
				if (response != null) {
					GetPositions(1);
					DisplayMessage(response, true);
					ClearFieldsPositionModal();
				}
				else {
					DisplayModal('Ошибка сохранения данных');
				}
			},
			error: function () {
				DisplayMessage('Ошибка запроса сохранения новой должности', false);
			}
		});
	}
	else {
		DisplayModal('.pop-up-actionWithPosition', false);
		DisplayMessage('Нельзя сохранить должность без названия', false);
	}
}

function GetCurrentPositionData(positionId) {
	$.ajax({
		url: 'https://localhost:44356/content/getCurrentPositionById?positionId=' + positionId,
		method: 'GET',
		success: function (response) {
			let position = JSON.parse(response);
			DisplayPositionDataInModal(position[0]);
		},
		error: function () {
			DisplayMessage('Ошибка выполнения запроса на получение данных выбранной должности', false);
		}
	});
}

function DisplayPositionDataInModal(position) {
	$('#newPositionAbbreviation').val(position.Abbreviation);
	$('#newPositionName').val(position.FullName);
}

function UpdateCurrentPositionData() {
	let positionId = $(selectedRow.row).attr('positionid');
	let positionName = $('#newPositionName').val();
	let abbreviation = $('#newPositionAbbreviation').val();

	$.ajax({
		url: 'https://localhost:44356/content/updatePositionData?positionId='
			+ positionId + '&positionName=' + positionName + '&abbreviation=' + abbreviation,
		method: 'PUT',
		success: function (response) {
			DisplayModal('.pop-up-actionWithPosition', false);
			if (response != '') {
				GetPositions(1);
				DisplayMessage(response, true);
			}
			else {
				DisplayMessage('Должность не существует', false);
			}
		},
		error: function () {
			DisplayMessage('Ошибка выполнения запроса на редактирование должности', false);
		}
	});
}

function ArchivePosition(positionId) {
	$.ajax({
		url: 'https://localhost:44356/content/removePosition?positionId=' + positionId,
		method: 'DELETE',
		success: function (response) {
			if (response != '') {
				GetPositions(1);
				DisplayMessage(response, true);
			}
			else {
				DisplayMessage('Должность не существует', false);
			}
		},
		error: function () {
			DisplayMessage('Ошибка выполнения запроса сохранения должности в архив', false);
		}
	});
}
