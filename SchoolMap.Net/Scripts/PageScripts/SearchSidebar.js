$(function () {
	$("#overallOfstedRatingSlider").slider({
		value: 4,
		min: 1,
		max: 4,
		step: 1,
		slide: function (event, ui) {
			$("#rating").val(ui.value);
			getSchools();
		}
	});
	$("#rating").val(+$("#overallOfstedRatingSlider").slider("value"));

	$("#establishmentType").chosen();
	$("#establishmentType").change(function () {
		getSchools();
	});
});