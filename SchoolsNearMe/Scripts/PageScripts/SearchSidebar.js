var ofstedDescriptions = {};
ofstedDescriptions['4'] = "4 - Poor";
ofstedDescriptions['3'] = "3 - Satisfactory";
ofstedDescriptions['2'] = "2 - Good";
ofstedDescriptions['1'] = "1 - Outstanding";

$(document).ready(function () {
	$("#btnSearchForLocation").click(function () {
		$.ajax({
			type: 'post',
			url: '/api/AddressGeocode',
			dataType: 'json',
			data: JSON.stringify({ AddressSearchText: $("#goToSearchBox").val() }),
			contentType: 'application/json; charset=utf-8',
			success: function (result) {
				map.panTo(new google.maps.LatLng(result.Latitude, result.Longitude));

			},
			error: function (jqXHR, textStatus, errorThrown) {
				alert(jqXHR.responseText);
				alert(textStatus);
				alert(errorThrown);
			}
		});
	});

	$("#overallOfstedRatingSlider").slider({
		value: 4,
		min: 1,
		max: 4,
		step: 1,
		slide: function (event, ui) {
			$("#rating").val(ofstedDescriptions[ui.value]);
			getSchools(ui.value);
		}
	});
	
    var sliderValue = $("#overallOfstedRatingSlider").slider("value");
	$("#rating").val(ofstedDescriptions[sliderValue]);

	$("#establishmentType").chosen();
});
