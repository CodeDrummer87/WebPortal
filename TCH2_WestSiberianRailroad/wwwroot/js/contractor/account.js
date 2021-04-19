$(document).ready(function () {

	$('#mainNav').on('click', '#columnsList', function () {
		clickSound.play();
		pageNumber = 0;
		currentEntities = 'columns';
		GetColumnsList(1);
		DisplayMessage('Загрузка списка колонн ТЧЭ-2 Омск', true);
	});

	$('#pop-up-createNewColumn-buttonsBlock').on('click', '#close-pop-up-createColumn', function () {
		removeSound.play();
		DisplayModal('.pop-up-createNewColumn', false);
	}).on('click', '#createNewColumnButton', function () {
		modifySound.play();
		DisplayModal('.pop-up-createNewColumn', false);
		CreateNewColumn();
	});

});

function GetColumnsList(isActual) {
	$.ajax({
		url: 'https://localhost:44356/columnData/getColumnsList?page=' + pageNumber + "&isActual=" + isActual,
		method: 'GET',
		success: function (response) {
			let result = JSON.parse(response);
			DisplayColumnsList(result, isActual);
		},
		error: function () {
			DisplayMessage("Ошибка выполнения запроса на получение списка колонн", false);
		}
	});
}

function DisplayColumnsList(list, isActual) {

	$('#contractorInfoDisplay table').remove();

	$.ajax({
		url: 'https://localhost:44356/columnData/getDriversCount',
		method: 'GET',
		success: function (response) {
			let driversCount = JSON.parse(response);
			$.ajax({
				url: 'https://localhost:44356/columnData/getColumnsCount?isActual=' + isActual,
				method: 'GET',
				success: function (count) {
					currentCount = count;

					let div = document.querySelector('#contractorInfoDisplay');
					let table = document.createElement('table');
					let caption = document.createElement('caption');
					caption.innerText = "Список колонн локомотивного депо ТЧЭ-2 'Омск'";
					table.appendChild(caption);
					let hRow = document.createElement('tr');
					GetThForTable(table, hRow, "Номер");
					GetThForTable(table, hRow, "Специализация колонны");
					GetThForTable(table, hRow, "Машинист-инструктор");
					GetThForTable(table, hRow, "Машинистов");
					GetThForTable(table, hRow, "Помощников");
					GetThForTable(table, hRow, "Всего человек");

					let rows = list.length;

					for (let i = 0; i < rows; i++) {

						let row = document.createElement('tr');
						if (i % 2 == 0) {
							$(row).css('background-color', '#2e2e2e');
						}
						else {
							$(row).css('background-color', '#3f3c3c');
						}
						$(row).attr('columnId', list[i].Id);
						GetTdForTable(table, row, list[i].Id);
						GetTdForTable(table, row, list[i].Specialization);
						GetTdForTable(table, row, list[i].Trainer);
						GetTdForTable(table, row, GetDriversCount(driversCount, list[i].Id)); //машинисты
						GetTdForTable(table, row, list[i].Total == 0 ? 0 : list[i].Total - GetDriversCount(driversCount, list[i].Id)); //помощники
						GetTdForTable(table, row, list[i].Total);
					}
					div.appendChild(table);
					SetControlPanels(count);
				},
				error: function () {
					DisplayMessage("Ошибка выполнения запроса получения количества записей", false);
				}
			});	
		}
	});
}

function GetHeaderForCreateNewColumnModal(isActual) {
	$.ajax({
		url: 'https://localhost:44356/columnData/getColumnsCount?isActual=' + isActual,
		method: 'GET',
		success: function (count) {
			$('#createNewColumn-header').text(`Создание колонны № ${count + 1}`);
		},
		error: function () {
			DisplayMessage("Ошибка выполнения запроса получения заголовка модального окна", false);
		}
	});
}

function GetColumnSpecializationForSelect() {
	$.ajax({
		url: 'https://localhost:44356/columnData/getColumnSpecialization?page=' + pageNumber,
		method: 'GET',
		success: function (response) {
			let result = JSON.parse(response);
			$('#selectSpecializationForNewColumn option').remove();
			let select = document.querySelector('#selectSpecializationForNewColumn');
			let elementCount = result.length;
			for (let i = 0; i < elementCount; i++) {
				let option = document.createElement('option');
				$(option).attr('value', result[i].Id);
				if (i == 0) {
					$(option).attr('selected', 'selected');
				}
				$(option).text(result[i].Name);
				select.appendChild(option);
			}
		},
		error: function () {
			DisplayRequestErrorWarning('#selectSpecializationForNewColumn');
		}
	});
}

function GetTrainerForCreateNewColumn() {
	$.ajax({
		url: 'https://localhost:44356/columnData/getTrainersListForNewColumn',
		method: 'GET',
		success: function (response) {
			let list = JSON.parse(response);
			$('#selectTrainerForNewColumn option').remove();

			let select = document.querySelector('#selectTrainerForNewColumn');
			let elementCount = list.length;
			for (let i = 0; i < elementCount; i++) {
				let option = document.createElement('option');
				$(option).attr('value', list[i].Id);
				if (i == 0) {
					$(option).attr('selected', 'selected');
				}
				$(option).text(`${list[i].LastName} ${list[i].FirstName[0]}.${list[i].MiddleName[0]}.`);
				select.appendChild(option);
			}
		},
		error: function () {
			DisplayRequestErrorWarning('#selectTrainerForNewColumn');
		}
	});
}

function CreateNewColumn() {
	let column = {
		specialization: +$('#selectSpecializationForNewColumn').val(),
		trainer: +$('#selectTrainerForNewColumn').val()
	};

	$.ajax({
		url: 'https://localhost:44356/columnData/createNewColumn',
		method: 'POST',
		contentType: 'application/json',
		data: JSON.stringify(column),
		success: function (message) {
			if (message != '') {
				GetColumnsList(1);
				DisplayMessage(message, true);
			}
			else {
				DisplayMessage("Колонна не сохранена - ошибка сервера", false);
			}
		},
		error: function () {
			DisplayMessage('Ошибка выполнения запроса создания новой колонны', false);
		}
	});
}

function GetDriversCount(drivers, id) {

	for (d of drivers) {
		if (d.Id == id) {
			return d.Count;
		}
	}

	return 0;
}
