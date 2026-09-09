/**
 * Geolocation handling for deal_cust.aspx
 * This file provides functions to capture the user's current location
 * using the browser's geolocation API.
 */

// Function to get current location
function getCurrentLocation() {
    if (navigator.geolocation) {
        // Show loading indicator or message
        document.getElementById('locationStatus').innerHTML = '<i class="fa fa-spinner fa-spin"></i> Getting location...';
        
        navigator.geolocation.getCurrentPosition(
            function(position) {
                // Success callback
                var latitude = position.coords.latitude;
                var longitude = position.coords.longitude;
                
                // Update hidden fields with location data
                // Note: These IDs will be set from the page using updateGeolocationTargets function
                var latField = document.getElementById(geolocationLatitudeFieldId);
                var lngField = document.getElementById(geolocationLongitudeFieldId);
                
                if (latField && lngField) {
                    latField.value = latitude;
                    lngField.value = longitude;
                }
                
                // Update status message
                document.getElementById('locationStatus').innerHTML = 
                    '<i class="fa fa-map-marker"></i> Location captured: ' + 
                    latitude.toFixed(6) + ', ' + longitude.toFixed(6);
            },
            function(error) {
                // Error callback
                var errorMessage;
                switch(error.code) {
                    case error.PERMISSION_DENIED:
                        errorMessage = "User denied the request for geolocation.";
                        break;
                    case error.POSITION_UNAVAILABLE:
                        errorMessage = "Location information is unavailable.";
                        break;
                    case error.TIMEOUT:
                        errorMessage = "The request to get user location timed out.";
                        break;
                    case error.UNKNOWN_ERROR:
                        errorMessage = "An unknown error occurred.";
                        break;
                }
                document.getElementById('locationStatus').innerHTML = 
                    '<i class="fa fa-exclamation-triangle"></i> Error: ' + errorMessage;
            },
            {
                enableHighAccuracy: true,
                timeout: 10000,
                maximumAge: 0
            }
        );
    } else {
        document.getElementById('locationStatus').innerHTML = 
            '<i class="fa fa-exclamation-triangle"></i> Geolocation is not supported by this browser.';
    }
}

// Variables to store the IDs of the hidden fields
var geolocationLatitudeFieldId = '';
var geolocationLongitudeFieldId = '';

/**
 * Updates the target field IDs used by the geolocation functions
 * This function should be called from the page to set the correct ClientIDs
 * @param {string} latitudeFieldId - The client ID of the latitude hidden field
 * @param {string} longitudeFieldId - The client ID of the longitude hidden field
 */
function updateGeolocationTargets(latitudeFieldId, longitudeFieldId) {
    geolocationLatitudeFieldId = latitudeFieldId;
    geolocationLongitudeFieldId = longitudeFieldId;
}

/**
 * Shows the current location on a small map (if you want to implement this feature later)
 * @param {number} latitude - The latitude coordinate
 * @param {number} longitude - The longitude coordinate
 * @param {string} containerId - The ID of the container element for the map
 */
function showLocationOnMap(latitude, longitude, containerId) {
    // This is a placeholder for a future map implementation
    // You could use Google Maps, Leaflet, or another mapping library here
    console.log('Map would show location at', latitude, longitude);
}
