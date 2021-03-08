$(document).ready(function () {

	CheckForName();

	$('#pop-up-submitData').click(function () {
		SaveCurrentUserData(); 
	});

	$('#positions').click(function () {
		GetPositions();
	});

	$('#employees').click(function () {
		GetEmployees();
	});
});

function CheckForName() {
	$.ajax({
		url: 'https://localhost:44356/content/checkforname',
		method: 'GET',
		success: function (response) {
			if (!response) {
				$('.nap').css('display', 'block');
				$('.pop-up-nameSetting').css('display', 'flex');
			}
			else {
				$('.nap').css('display', 'none');
				$('.pop-up-nameSetting').css('display', 'none');
			}
		},
		error: function () {
			DisplayMessage('Не удалось установить связь с сервером', false);
		}
	});
}

function GetEmployees() {
	$.ajax({
		url: 'https://localhost:44356/content/getemployees',
		method: 'GET',
		success: function (response) {
			let result = JSON.parse(response);
			DisplayEmployees(result);
			DisplayMessage("Список сотрудников ТЧЭ-2 'Омск' загружен", true);
		},
		error: function () {
			DisplayMessage("Ошибка выполнения запроса", false);
		}
	});
}

function GetPositions() {
	$.ajax({
		url: 'https://localhost:44356/content/getpositions',
		method: 'GET',
		success: function (response) {
			let result = JSON.parse(response);
			console.log(result);
			DisplayPositions(result);
			DisplayMessage("Список текущих должностей в ТЧЭ-2 'Омск' загружен", true);
		},
		error: function () {
			DisplayMessage("Ошибка выполнения запроса", false);
		}
	});
}

function DisplayMessage(message, success) {
	if (success) {
		$('#mainMessages').css('color', '#00ff21').text(message);
	}
	else {
		$('#mainMessages').css('color', 'red').text(message);
	}

	setTimeout(function () { $('#mainMessages').text(''); }, 3500);
}

function DisplayEmployees(result) {
	$('#mainArticle table').remove();
	let div = document.querySelector('#mainArticle');
	let table = document.createElement('table');
	let caption = document.createElement('caption');
	caption.innerText = 'Список сотрудников ТЧЭ-2 "Омск"';
	table.appendChild(caption);
	let hRow = document.createElement('tr');
	GetThForTable(table, hRow, "Фамилия");
	GetThForTable(table, hRow, "Имя");
	GetThForTable(table, hRow, "Отчество");
	GetThForTable(table, hRow, "Должность");
	GetThForTable(table, hRow, "Электронная почта");
	let rows = result.length;
	for (let i = 0; i < rows; i++) {
		let row = document.createElement('tr');
		if (i % 2 == 0) {
			$(row).css('background-color', '#2e2e2e');
		}
		else {
			$(row).css('background-color', '#3f3c3c');
		}
		GetTdForTable(table, row, result[i].LastName);
		GetTdForTable(table, row, result[i].FirstName);
		GetTdForTable(table, row, result[i].MiddleName);
		GetTdForTable(table, row, result[i].FullName);
		GetTdForTable(table, row, result[i].Email);
	}
	div.appendChild(table);
}

function DisplayPositions(result) {
	$('#mainArticle table').remove();
	let div = document.querySelector('#mainArticle');
	let table = document.createElement('table');
	let caption = document.createElement('caption');
	caption.innerText = 'Список должностей ТЧЭ-2 "Омск"';
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
		GetTdForTable(table, row, i + 1);
		GetTdForTable(table, row, result[i].FullName);
		GetTdForTable(table, row, result[i].Count);
	}
	div.appendChild(table);
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
					$('.nap').css('display', 'none');
					$('.pop-up-nameSetting').css('display', 'none');
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
