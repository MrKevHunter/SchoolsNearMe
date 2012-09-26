/* File Created: July 23, 2012 */
var markersArray = [];
var map;
var infoWindow;
function displayPosition(position) {
	var centre;
	if (Modernizr.localstorage && localStorage.getItem('lastLocation') != null) {
		var output = parseLocation(localStorage.getItem('lastLocation'));
		centre = new google.maps.LatLng(output.latitude, output.longitude);
	} else {
		centre = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
	}

	var mapOptions = {
		//center: new google.maps.LatLng(51.52269, -0.984406),
		center: centre,
		zoom: 14,
		mapTypeId: google.maps.MapTypeId.ROADMAP
	};
	map = new google.maps.Map(document.getElementById("map_canvas"),
			mapOptions);

	google.maps.event.addListener(map, 'idle', function () {
		getSchools();
		if (Modernizr.localstorage) {
			localStorage.setItem('lastLocation', map.getCenter());	
		}
		
		
	});
}

function displayError(error) {
	var errors = {
		1: 'Permission denied',
		2: 'Position unavailable',
		3: 'Request timeout'
	};
	alert("Error: " + errors[error.code]);
	defaultLocation();
}

function initialize() {
	// this determines if its available or if its defined;
	// http://html5doctor.com/finding-your-position-with-geolocation/
	if (navigator.geolocation) {
		var timeoutVal = 10 * 1000 * 1000;
		navigator.geolocation.getCurrentPosition(
				displayPosition,
				displayError,
				{ enableHighAccuracy: true, timeout: timeoutVal, maximumAge: 0 }
			);
	} else if (confirm("Your location is unable to be determined in your browser, would you like for us to try by network address?")) {
		
		var ajax = $.ajax({
			type: "get",
			url: "/api/geolocation",
			dataType: 'json',
			contentType: 'application/json; charset=utf-8'
		});

		ajax.done(function(result) {
			var defaultCoords = { latitude: result.Latitude, longitude: result.Longitude };
			var defaultPosition = { coords: defaultCoords };
			displayPosition(defaultPosition);   
		});

		ajax.fail(function () {
			alert('Unfortunately your location was unable to be determined; using default location');
			defaultLocation();
		});

	} else {
		defaultLocation();
	}
}

function defaultLocation() {
	var defaultCoords = { latitude: 51.52269, longitude: -0.984406 };
	var defaultPosition = { coords: defaultCoords };
	displayPosition(defaultPosition);    
}

function clearOverlays(markers) {
	if (markers) {
		for (var j = markers.length - 1; j >= 0; j--) {
			for (var markersCount in markersArray) {
				if (markersArray[markersCount].id == markers[j].Id) {
					markersArray[markersCount].setMap(null);
					markersArray.splice(markersCount, 1);
				}
			}
		}
	}
}


function getColour(school) {
	if (school.OfstedRating.OverallEffectiveness == 1) {
		return "Green";
	}
	if (school.OfstedRating.OverallEffectiveness == 2) {
		return "Blue";
	}
	if (school.OfstedRating.OverallEffectiveness == 3) {
		return "Yellow";
	}
	if (school.OfstedRating.OverallEffectiveness == 4) {
		return "red";
	}
}

function getAge(school) {
	if (school.TypeOfEstablishment == 0) {
		return "Nursery";
	}
	if (school.TypeOfEstablishment == 1) {
		return "Primary";
	}
	if (school.TypeOfEstablishment == 2) {
		return "Secondary";
	}
	return "unknown";
}

function getIcon(school) {
	var colour = getColour(school);
	var age = getAge(school);
	return "/Images/" + colour + "-" + age + ".png";
}

function getSchools(ofsted) {
	if (ofsted == undefined) {
		ofsted = $("#overallOfstedRatingSlider").slider("value");
	}

	var boundries = map.getBounds();
	var northEast = boundries.getNorthEast();
	var southWest = boundries.getSouthWest();
	var northEastLat = northEast.lat();
	var northEastLong = northEast.lng();
	var southWestLat = southWest.lat();
	var southWestLong = southWest.lng();
	var items = $("#establishmentType").val();
	$.ajax({
		type: 'post',
		url: '/api/schools',
		dataType: 'json',
		data: JSON.stringify({ northEastLat: northEastLat, northEastLong: northEastLong, southWestLat: southWestLat, southWestLong: southWestLong, ofstedRating: ofsted, schoolTypes: items }),
		contentType: 'application/json; charset=utf-8',
		success: function (result) {
			var markersToDelete = new Array();
			for (var mapCount in map.placeMarkers) {
				var found = false;
				for (var resultCount in result) {
					if (result[resultCount].Id == map.placeMarkers[mapCount].Id) {
						found = true;
					}
				}
				if (!found) {
					markersToDelete.push(map.placeMarkers[mapCount]);
				}
			}
			clearOverlays(markersToDelete);
			map.placeMarkers = $.each(result, function (i, item) {
				var exists = false;
				for (var markerCount in markersArray) {
					if (markersArray[markerCount].id == item.Id) {
						exists = true;
					}
				}
				if (!exists) {
					var lat = item.Location.Latitude;
					var lng = item.Location.Longitude;
					var point = new google.maps.LatLng(parseFloat(lat), parseFloat(lng));
					// extend the bounds to include the new point      
					// add the marker itself      
					var icon = getIcon(item);

					var marker = new google.maps.Marker({
						position: point,
						map: map,
						draggable: false,
						animation: google.maps.Animation.DROP,

						id: item.Id
					});
					if (icon != undefined) {
						marker.setIcon(icon);
					}

					markersArray.push(marker);
					infoWindow = new google.maps.InfoWindow();
					// add a listener to open the tooltip when a user clicks on one of the markers      
					google.maps.event.addListener(marker, 'click', function () {
						$.get('/SchoolDetail/Detail', { id: item.Id }, function (parameters) {

							infoWindow.setContent(parameters);
							infoWindow.open(map, marker);
						});

					});
				}
			});
		},
		error: function (jqXHR, textStatus, errorThrown) {
			alert(jqXHR.responseText);
		}
	});
}

$().ready(function () {
	initialize();
	$("#establishmentType").change(function () {
		getSchools();
	});
});