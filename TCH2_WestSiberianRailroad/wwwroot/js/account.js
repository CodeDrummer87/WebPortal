$(document).ready(function () {
	$('#positions').click(function () {
		GetPositions();
	});

	$('#employees').click(function () {
		GetEmployees();
	});
});

function GetEmployees() {
	$.ajax({
		url: 'https://localhost:44356/content/getemployees',
		method: 'GET',
		success: function (response) {
			let result = JSON.parse(response);
			$('#mainArticle table').remove();
			//.:: Table building :::
			console.log(result);
			$('#mainArticle p').remove();
			let div = document.querySelector('#mainArticle');
			let table = document.createElement('table');
			let caption = document.createElement('caption');
			caption.innerText = 'Список сотрудников ТЧЭ-2 "Омск"';
			table.appendChild(caption);
			let rows = Object.keys(result).length;

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
			let div = document.querySelector('#mainArticle');
			$('#mainArticle p').remove();
			for (let i = 0; i < result.length; i++) {
				let p = document.createElement('p');
				p.innerText = result[i].FullName;
				div.appendChild(p);
			}
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