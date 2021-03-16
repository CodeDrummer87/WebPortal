var pageNumber = 0;
var currentCount = 0;
var timeoutHandle = 'undefined';

$(document).ready(function () {

	CheckForName();

	$('#pop-up-submitData').click(function () {
		SaveCurrentUserData(); 
	});

	$('#positions').click(function () {
		pageNumber = 0;
		currentEntities = 'positions';
		GetPositions(1);
		DisplayMessage("Список текущих должностей в ТЧЭ-2 'Омск' загружен", true);
	});

	$('#employees').click(function () {
		pageNumber = 0;
		currentEntities = 'employees';
		GetEmployees(1);
		DisplayMessage("Список сотрудников ТЧЭ-2 'Омск' загружен", true);
	});

	$('#roles').click(function () {
		pageNumber = 0;
		currentEntities = 'roles';
		GetRoles(1);
		DisplayMessage("Список ролей для сайта ТЧЭ-2 'Омск' загружен", true);
	});

	$('.paginationPart').on('mouseover', 'img', function () {
		$(this).attr('src', '/images/arrow_hover.png');
	}).on('mouseout', 'img', function () {
		$(this).attr('src', '/images/arrow.png');
	}).on('click', 'img', function () {
		$(this).attr('src', '/images/arrow_active.png');
		if ($(this).attr('id') == 'pageRight') {
			if (pageNumber < Math.round(currentCount / 14)) {
				++pageNumber;
			}
		}
		else {
			if (pageNumber > 0) {
				--pageNumber;
			}
		}

		switch (currentEntities) {
			case 'positions': GetPositions(1); break;
			case 'archPositions': GetPositions(0); break;
			case 'employees': GetEmployees(1); break;
			case 'archEmployees': GetEmployees(0); break;
			case 'roles': GetRoles(1); break;
			case 'archRoles': GetRoles(0); break;
		}
	});

	$('#logout').click(function () {
		$.ajax({
			url: 'https://localhost:44356/account/logout',
			method: 'GET',
			success: function (address) {
				window.location.href = address;
			}
		});
	});
});

function CheckForName() {
	$.ajax({
		url: 'https://localhost:44356/content/checkforname',
		method: 'GET',
		success: function (response) {
			if (!response) {
				DisplayModal('.pop-up-nameSetting', true);
			}
			else {
				DisplayModal('.pop-up-nameSetting', false);
			}
		},
		error: function () {
			DisplayMessage('Не удалось установить связь с сервером', false);
		}
	});
}

function GetEmployees(isActual) {
	$.ajax({
		url: 'https://localhost:44356/content/getemployees?page=' + pageNumber + "&isActual=" + isActual,
		method: 'GET',
		success: function (response) {
			let result = JSON.parse(response);
			DisplayEmployees(result, isActual);
		},
		error: function () {
			DisplayMessage("Ошибка выполнения запроса", false);
		}
	});
}

function GetPositions(isActual) {
	$.ajax({
		url: 'https://localhost:44356/content/getpositions?page=' + pageNumber + "&isActual=" + isActual,
		method: 'GET',
		success: function (response) {
			let result = JSON.parse(response);
			DisplayPositions(result, isActual);
		},
		error: function () {
			DisplayMessage("Ошибка выполнения запроса", false);
		}
	});
}

function GetRoles(isActual) {
	$.ajax({
		url: 'https://localhost:44356/content/getroles?page=' + pageNumber + "&isActual=" + isActual,
		method: 'GET',
		success: function (response) {
			let result = JSON.parse(response);
			DisplayRoles(result, isActual);
		},
		error: function () {
			DisplayMessage("Ошибка выполнения запроса", false);
		}
	});
}

function DisplayMessage(message, success) {

	if (timeoutHandle != 'undefined') {
		clearTimeout(timeoutHandle);
	}

	if (success) {
		$('#mainMessages').css('color', '#00ff21').text(message);
	}
	else {
		$('#mainMessages').css('color', 'red').text(message);
	}

	timeoutHandle = setTimeout(function () { $('#mainMessages').text(''); }, 3500);
}

function DisplayEmployees(result, isActual) {
	$('#infoDisplay table').remove();

	$.ajax({
		url: 'https://localhost:44356/content/getemployeecount?isActual=' + isActual,
		method: 'GET',
		success: function (count) {
			currentCount = count;

			let div = document.querySelector('#infoDisplay');
			let table = document.createElement('table');
			let caption = document.createElement('caption');
			caption.innerText = isActual == 1 ? 'Список сотрудников ТЧЭ-2 "Омск"' : 'Архив сотрудников ТЧЭ-2 "Омск"';
			table.appendChild(caption);
			let hRow = document.createElement('tr');
			GetThForTable(table, hRow, "Фамилия");
			GetThForTable(table, hRow, "Имя");
			GetThForTable(table, hRow, "Отчество");
			GetThForTable(table, hRow, "Должность");
			GetThForTable(table, hRow, "Электронная почта");
			GetThForTable(table, hRow, "Подтв.");

			let rows = result.length;

			for (let i = 0; i < rows; i++) {
				let row = document.createElement('tr');
				if (i % 2 == 0) {
					$(row).css('background-color', '#2e2e2e');
				}
				else {
					$(row).css('background-color', '#3f3c3c');
				}
				$(row).attr('userId', result[i].Id);
				GetTdForTable(table, row, result[i].LastName);
				GetTdForTable(table, row, result[i].FirstName);
				GetTdForTable(table, row, result[i].MiddleName);
				GetTdForTable(table, row, result[i].FullName);
				GetTdForTable(table, row, result[i].Email);
				let confirmedEmail = result[i].ConfirmedEmail == 1 ? 'Да' : 'Нет';
				GetTdForTable(table, row, confirmedEmail);
			}
			div.appendChild(table);
			SetControlPanels(count);
		}
	});
}

function DisplayPositions(result, isActual) {
	$('#infoDisplay table').remove();

	$.ajax({
		url: 'https://localhost:44356/content/getpositioncount?isActual=' +isActual,
		method: 'GET',
		success: function (count) {
			currentCount = count;

			let div = document.querySelector('#infoDisplay');
			let table = document.createElement('table');
			let caption = document.createElement('caption');
			caption.innerText = isActual == 1 ? 'Список должностей ТЧЭ-2 "Омск"' : ' Архив должностей ТЧЭ-2 "Омск"';
			table.appendChild(caption);
			let hRow = document.createElement('tr');
			GetThForTable(table, hRow, "№");
			GetThForTable(table, hRow, "Наименование должности");
			GetThForTable(table, hRow, "Сотрудников в должности");

			let rows = result.length;
			for (let i = 0; i < rows; i++) {
				let row = document.createElement('tr');
				if (i % 2 == 0) {
					$(row).css('background-color', '#2e2e2e');
				}
				else {
					$(row).css('background-color', '#3f3c3c');
				}
				$(row).attr('positionId', result[i].Id);
				GetTdForTable(table, row, i + 1);
				GetTdForTable(table, row, result[i].FullName);
				GetTdForTable(table, row, result[i].Count);
			}
			div.appendChild(table);
			SetControlPanels(count);
		}
	});
}

function DisplayRoles(result, isActual) {
	$('#infoDisplay table').remove();

	$.ajax({
		url: 'https://localhost:44356/content/getrolecount?isActual=' + isActual,
		method: 'GET',
		success: function (count) {
			currentCount = count;

			let div = document.querySelector('#infoDisplay');
			let table = document.createElement('table');
			let caption = document.createElement('caption');
			caption.innerText = isActual == 1 ? 'Список ролей на сайте ТЧЭ-2 "Омск"' : 'Архив ролей на сайте ТЧЭ-2 "Омск"';
			table.appendChild(caption);
			let hRow = document.createElement('tr');
			GetThForTable(table, hRow, "№");
			GetThForTable(table, hRow, "Наименование роли");
			GetThForTable(table, hRow, "Сотрудников с данной ролью");

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
				GetTdForTable(table, row, result[i].RoleName);
				GetTdForTable(table, row, result[i].Count);
			}
			div.appendChild(table);
			SetControlPanels(count);
		}
	});
}

function GetThForTable(table, row, value) {
	let th = document.createElement('th');
	th.innerText = value;
	row.appendChild(th);
	table.appendChild(row);
}

function GetTdForTable(table, row, value) {
	let td = document.createElement('td');
	td.innerText = value;
	row.appendChild(td);
	table.appendChild(row);
}

function SaveCurrentUserData() {
	let userData = {
		firstname: $('#pop-up-firstname').val(),
		lastname: $('#pop-up-lastname').val(),
		middlename: $('#pop-up-middlename').val()
	}

	if (CheckNames(userData)) {
		$.ajax({
			url: 'https://localhost:44356/account/saveuserdata',
			method: 'POST',
			contentType: 'application/json',
			data: JSON.stringify(userData),
			success: function (response) {
				if (response != '') {
					DisplayModal('.pop-up-nameSetting', false);
					window.location.href = response;
				}
				else {
					$('#pop-up-currentMessage').css('color', 'gold').text('Пользователь не найден');
				}
			},
			error: function () {
				$('#pop-up-currentMessage').css('color', 'red').text("Ошибка запроса: данные не сохранены");
			}
		});
	}
	else {
		$('#pop-up-currentMessage').css('color', 'red').text("Укажите все требуемые данные");
	}
}

function CheckNames(data) {
	if (data.firstname !== '' && data.lastname !== '' && data.middlename !== '') {
		return true;
	}

	return false;
}

function SetControlPanels(count) {
	$('#paginationMiddle p').remove();
	if (count > 14) {
		$('#paginationBlock').css('display', 'flex');
		let div = document.querySelector('#paginationMiddle');
		let p = document.createElement('p');
		p.innerText = `Страница ${pageNumber + 1} из ${Math.round(count / 14 + 1)}`;
		div.appendChild(p);

		if (pageNumber < 1) {
			$('#paginationLeftPart').css('pointerEvents', 'none').css('opacity', '0');
		}
		else {
			$('#paginationLeftPart').css('pointerEvents', 'auto').css('opacity', '1');
		}
		if (pageNumber + 1 == Math.round(count / 14 + 1)) {
			$('#paginationRightPart').css('pointerEvents', 'none').css('opacity', '0');
		}
		else {
			$('#paginationRightPart').css('pointerEvents', 'auto').css('opacity', '1');
		}
	}
	else {
		$('#paginationBlock').css('display', 'none');
	}

	$('#controlPanel').css('display', 'block');
}

