//.::Script for working with data in the mainArticle block :::

//.:: Variable to store the current context :::
var currentEntities = 'none';
var selectedEmployeeRow = 'undefined';

$(document).ready(function () {

	$('#addNewEntity').click(function () {
		clickSound.play();
		ClearFieldsForCreatingNewEmployee();
		switch (currentEntities) {
			case 'positions': DisplayMessage('Добавление в систему новой должности', true);
				break;
			case 'employees': DisplayMessage('Создание аккаунта нового сотрудника', true);
				DisplayModal('.pop-up-createNewEmployee', true);
				$('#inpCreateEmail').focus();
				GetPositionsForSelect(1);
				GetRolesForSelect(1);
				break;
			case 'roles': DisplayMessage('Будет добавлена новая роль', true); break;
			case 'siteEmail': DisplayMessage('Актуальный почтовый аккаунт для сайта ТЧЭ-2 \"Омск\" будет изменён', true); break;
			default: DisplayMessage('Кнопка нажата, но контекст не выбран', false);
		}
	});

	$('#deleteEntity').click(function () {
		clickSound.play();
		switch (currentEntities) {
			case 'positions': DisplayMessage('Должность будет перенесена в архив', true);
				break;
			case 'employees':
				if (selectedEmployeeRow != 'undefined') {
					DisplayMessage('Аккаунт сотрудника будет перенесён в архив', true);
					DisplayModal('.pop-up-confirmEmployeeRemove', true);
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

	$('#employeeRemoveConfirmButton').click(function () {
		removeSound.play();
		DisplayModal('.pop-up-confirmEmployeeRemove', false);
		if (selectedEmployeeRow != 'undefined') {
			let userId = $(selectedEmployeeRow.row).attr('userid');
			ArchiveEmployee(userId);
		}
	});

	$('#cancelEmployeeRemoveButton').click(function () {
		clickSound.play();
		DisplayModal('.pop-up-confirmEmployeeRemove', false);
		DisplayMessage('Отмена операции удаления аккаунта сотрудника', true);
	});

	$('#archEmployees').click(function () {
		clickSound.play();
		pageNumber = 0;
		currentEntities = 'archEmployees';
		GetEmployees(0);
		DisplayMessage("Архив сотрудников ТЧЭ-2 'Омск' загружен", true);
	});

	$('#archPositions').click(function () {
		clickSound.play();
		pageNumber = 0;
		currentEntities = 'archPositions';
		GetPositions(0);
		DisplayMessage("Архив должностей ТЧЭ-2 'Омск' загружен", true);
	});

	$('#archRoles').click(function () {
		clickSound.play();
		pageNumber = 0;
		currentEntities = 'archRoles';
		GetRoles(0);
		DisplayMessage("Архив ролей сайта ТЧЭ-2 'Омск' загружен", true);
	});

	$('#siteEmail').click(function () {
		clickSound.play();
		pageNumber = 0;
		currentEntities = 'siteEmail';
		GetAppEmailAccounts();
		DisplayMessage("Данные актуального почтового аккаунта сайта ТЧЭ-2 \"Омск\" загружены", true);
	});

	$('#infoDisplay').on('click', 'tr', function () {
		clickSound.play();
		if (selectedEmployeeRow != 'undefined') {
			$(selectedEmployeeRow.row)
				.css('color', '#04eaed')
				.css('background-color', selectedEmployeeRow.defaultBGColor)
				.css('box-shadow', 'none');
		}

		selectedEmployeeRow = {
			row: $(this),
			defaultColor: $(this).css('color'), 
			defaultBGColor: $(this).css('background-color') 
		};

		$(this).css('background-color', 'gold').css('color', 'black').css('box-shadow', '0px 0px 7px 4px orange');
	});
});

function GetPositionsForSelect(isActual) {
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

function GetRolesForSelect(isActual) {
	$.ajax({
		url: 'https://localhost:44356/content/getroles?page=' + 0 + "&isActual=" + isActual,
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
				$(row).attr('roleId', result[i].Id);
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
