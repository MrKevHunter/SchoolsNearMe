/* File Created: September 10, 2012 */
/// <reference path="Coordinates/MapHelper.js"/>
/// <reference path="qunit.js"/>
test("parseInput", function () {
	var coord = parseLocation("(51.49890560460843, -0.13744133898921262)");
	equal(coord.latitude, 51.49890560460843);
	equal(coord.longitude, -0.13744133898921262);
})