var selectedRailroad = 'undefined';
var selectedIndustry = 'undefined';
var selectedUnit = 'undefined';
var currentPage = 0;
var currentCount = 0;

$(document).ready(function () {

	let clickSound = new Audio("/audio/click.wav");

	$('#signIn').click(function () {
		clickSound.play();
		$('#signIn').css('display', 'none');
		$('.nap').css('display', 'grid');
		GetRailroadList();
		GetIndustryList();
	});

	$('.railroads').on('click', 'p', function () {
		clickSound.play();
		if (selectedRailroad === 'undefined') {
			$(this).css('color', '#00ff21');
			selectedRailroad = this;
		}
		else {
			$(selectedRailroad).css('color', '#ffd800');
			$(this).css('color', '#00ff21');
			selectedRailroad = this;
		}
		currentPage = 0;
		GetUnitList();
	});

	$('.industries').on('click', 'li', function () {
		clickSound.play();
		$(selectedIndustry).css('color', '#ffd800');
		$(this).css('color', '#00ff21');
		selectedIndustry = this;
		currentPage = currentCount = 0;
		$('.pageTurner').css('display', 'none');
		$('#arrowLeft').css('pointerEvents', 'none').css('opacity', '0');
		GetUnitList();
	});

	$('.units').on('click', 'p', function () {
		clickSound.play();
		if (selectedUnit === 'undefined') {
			$(this).css('color', '#00ff21');
			selectedUnit = this;
		}
		else {
			$(selectedUnit).css('color', '#ffd800');
			$(this).css('color', '#00ff21');
			selectedUnit = this;
		}
		$('.transition').css('display', 'block');
	});

	$('.pageTurner').on('mouseover', 'img', function () {
		$(this).attr('src', '/images/arrows/right_hover.png');
	}).on('mouseout', 'img', function () {
		$(this).attr('src', '/images/arrows/right.png');
	}).on('click', 'img', function () {
		clickSound.play();
		$(this).attr('src', '/images/arrows/right_active.png');
		if ($(this).attr('id') == 'arrowRight') {
			++currentPage;
			if (currentPage > 0) {
				$('#arrowLeft').css('pointerEvents', 'auto').css('opacity', '1.0');
			}
			if (currentPage >= Math.round(currentCount/12)) {
				$('#arrowRight').css('pointerEvents', 'none').css('opacity', '0');
			}
		}
		else {
			--currentPage;
			if (currentPage == 0) {
				$('#arrowLeft').css('pointerEvents', 'none').css('opacity', '0');
			}
			if (currentPage == Math.round(currentCount/12) - 1) {
				$('#arrowRight').css('pointerEvents', 'auto').css('opacity', '1.0');
			}
		}
		GetUnitList();
	});

	$('#goOver').click(function () {

		clickSound.play();
		let railroadId = $(selectedRailroad).attr('id');
		let industryId = $(selectedIndustry).attr('id');
		let unitId = $(selectedUnit).attr('id');

		$.ajax({
			url: 'https://localhost:44336/transit/transitToUnit?railroadId='+railroadId+"&industryId="+industryId+"&unitId="+unitId,
			method: 'GET',
			success: function (address) {
				if (address !== '') {
					window.location.href = address;
				}
				else {
					DisplayCurrentMessage("Сайт временно недоступен", false);
				}
			},
			error: function () {

			}
		});
	});

});

function GetRailroadList() {
	$.ajax({
		url: "https://localhost:44336/data/getRailroadList",
		method: 'GET',
		success: function (railroadList) {
			DisplayRailroadList(railroadList);		
		},
		error: function () {

		}
	});
}

function DisplayRailroadList(list) {
	for (let i = 0; i < list.length; i++) {
		let p = document.createElement('p');
		p.setAttribute('id', list[i].id);
		p.innerText = list[i].fullTitle;
		$('.railroads').append(p);
	}
}

function GetUnitList() {

	let elems = document.querySelectorAll('.units p');
	for (let i = 0; i < elems.length; i++) {
		elems[i].remove();
	}

	let railroadId = $(selectedRailroad).attr('id');
	let industryId = $(selectedIndustry).attr('id');
	$.ajax({
		url: "https://localhost:44336/data/getUnitsCount?railroadId=" + railroadId + "&industryId=" + industryId,
		method: 'GET',
		success: function (unitsCount) {
			currentCount = unitsCount;
			DisplayUnitList(unitsCount, railroadId, industryId);
		},
		error: function () {

		}
	});
}

function GetIndustryList() {
	$.ajax({
		url: 'https://localhost:44336/data/getIndustryList',
		method: 'GET',
		success: function (industryList) {
			DisplayIndustryList(industryList);
		},
		error: function () {

		}
	});
}

function DisplayIndustryList(list) {
	let div = document.querySelector('.industries');
	for (let i = 0; i < list.length; i++) {
		let li = document.createElement('li');
		li.innerText = list[i].abbreviation;
		li.setAttribute('id', list[i].id);
		div.appendChild(li);
	}

	selectedIndustry = $('.industries li:first');
	$(selectedIndustry).css('color', '#00ff21');
}

function DisplayUnitList(unitsCount, railroadId, industryId) {
	if (unitsCount > 0 && unitsCount <= 12) {
		GetUnitsAndDisplay(railroadId, industryId);
	}
	else if (unitsCount == 0) {
		let p = document.createElement('p');
		p.innerText = "Список пуст";
		$('.units').append(p);
		$('.transition').css('display', 'none');
	}
	else {
		GetUnitsAndDisplay(railroadId, industryId);
		$('.pageTurner').css('display', 'flex');
	}
}

function GetUnitsAndDisplay(railroadId, industryId) {
	$.ajax({
		url: 'https://localhost:44336/data/getUnitList?railroadId=' + railroadId + '&industryId=' + industryId + '&page=' + currentPage,
		method: 'GET',
		success: function (list) {
			for (let i = 0; i < list.length; i++) {
				let p = document.createElement('p');
				p.setAttribute('id', list[i].id);
				p.setAttribute('title', list[i].fullTitle);
				p.innerText = list[i].shortTitle;
				$('.units').append(p);
			}
		},
		error: function () {

		}
	});
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