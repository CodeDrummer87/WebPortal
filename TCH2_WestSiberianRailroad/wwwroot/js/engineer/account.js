$(document).ready(function () {

	$('#mainNav').on('click', '#createTelegram', function () {
		if ($('#telegramCreating').css('display') == 'none') {
			clickSound.play();
			$('#telegramCreating').css('display', 'flex');
			$('#engineerInfoDisplay').css('display', 'none');
			$('#telegramSubject').focus();
			DisplayMessage('Загружена форма создания телеграммы', true);
		}
	}).on('click', '#telegramsList', function () {
		if ($('#engineerInfoDisplay').css('display') == 'none') {
			clickSound.play();
			$('#engineerInfoDisplay').css('display', 'flex');
			$('#telegramCreating').css('display', 'none');

			GetTelegramsList();

			DisplayMessage('Загружен список телеграмм', true);
		}
	});

	$('#telegramCreatingButtons').on('click', '#clearTelegram', function () {
		clickSound.play();
		ClearTelegramForm();
		DisplayMessage('Тема и содержимое телеграммы очищено', true);
	}).on('click', '#creatingTelegramButton', function () {
		modifySound.play();
		let telegram = {
			Subject: $('#telegramSubject').val(),
			Content: $('#telegramContent').val()
		};

		CreateTelegram(telegram);
	});
});

function CreateTelegram(telegram) {

	if (CheckTelegramNotEmpty(telegram)) {
		$.ajax({
			url: 'https://localhost:44356/telegramdata/createnewtelegram',
			method: 'POST',
			contentType: 'application/json',
			data: JSON.stringify(telegram),
			success: function (message) {
				if (message != '') {
					DisplayMessage(message, true);
					ClearTelegramForm();
				}
				else {
					DisplayMessage('Ошибка передачи данных', false);
				}
			},
			error: function () {
				DisplayMessage('Ошибка выполнения запроса передачи данных', false);
			}
		});
	}
	else {
		DisplayMessage('Телеграмма должна иметь тему и содержание', false);
	}
}

function ClearTelegramForm() {
	$('#telegramSubject').val('');
	$('#telegramContent').val('');
}

function CheckTelegramNotEmpty(telegram) {

	return (telegram != null && telegram.Subject != '' && telegram.Content != '') ? true : false;

}

function GetTelegramsList() {
	$.ajax({
		url: 'https://localhost:44356/telegramdata/getTelegramsList',
		method: 'GET',
		success: function (response) {
			let list = JSON.parse(response);
			DisplayTelegramsList(list);
		},
		function() {
			DisplayMessage('Ошибка выполнения запроса на получение списка телеграмм', false);
		}
	});
}

function DisplayTelegramsList(list) {
	$('#engineerInfoDisplay table').remove();

	let div = document.querySelector('#engineerInfoDisplay');
	let table = document.createElement('table');
	let caption = document.createElement('caption');
	caption.innerText = 'Все телеграммы';
	table.appendChild(caption);
	let hRow = document.createElement('tr');
	GetThForTable(table, hRow, "№");
	GetThForTable(table, hRow, "Дата");
	GetThForTable(table, hRow, "Тема");
	GetThForTable(table, hRow, "Актуальность");

	let rows = list.length;
	for (let i = 0; i < rows; i++) {
		let row = document.createElement('tr');
		if (i % 2 == 0) {
			$(row).css('background-color', '#2e2e2e');
		}
		else {
			$(row).css('background-color', '#3f3c3c');
		}
		$(row).attr('telegramId', list[i].Id);
		GetTdForTable(table, row, i + 1);
		GetTdForTable(table, row, ShowDate(list[i].Created));
		GetTdForTable(table, row, list[i].Subject);
		GetTdForTable(table, row, list[i].IsActual == 1? 'Актуально' : 'Устарела');
	}
	div.appendChild(table);
}

function ShowDate(date) {
	let created = new Date(date);

	let year = created.getFullYear();
	let month = created.getMonth();
	let day = created.getDay();

	return `${day} ${GetMonthByText(month)} ${year} г.`;
}

function GetMonthByText(month) {
	switch (month) {
		case 0: return 'января';
		case 1: return 'февраля';
		case 2: return 'марта';
		case 3: return 'апреля';
		case 4: return 'мая';
		case 5: return 'июня';
		case 6: return 'июля';
		case 7: return 'августа';
		case 8: return 'сентября';
		case 9: return 'октября';
		case 10: return 'ноября';
		case 11: return 'декабря';
	}
}
