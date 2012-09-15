/* File Created: September 10, 2012 */
function parseLocation(input) {
	var output = new Object();
	input = input.substr(1, input.length - 2);
	output.latitude = input.substr(0,input.indexOf(","));
	output.longitude = input.substr(input.indexOf(",") + 2);
	return output;
}