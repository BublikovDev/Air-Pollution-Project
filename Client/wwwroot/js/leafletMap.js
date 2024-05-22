var map;
function initialize_leaflet() {
    map = L.map('map').setView([52.133, 19.615], 7);
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png',
        {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        }).addTo(map);



}

function addMarkers_leaflet(ltd, lng, aqi, className, location) {
    var myIcon = L.divIcon({ className: className, html: '<b>' + aqi + '</b>' });
    function createPopupContent(location) {
        let popupContent = '<table>';
        popupContent += '<tr><td>' + location.name + '</td></tr>';
        popupContent += '<tr><td>' + location.latitude + ' ' + location.longitude + '</td></tr>';

        location.sensors.forEach(sensor => {
            popupContent += '<tr><td>Sensor:</td><td>' + sensor.name + '</td></tr>';
            popupContent += '<tr><td>Value:</td><td>' + sensor.value.toFixed(2) + '</td></tr>';
            //popupContent += '<tr><td>Min</td><td>' + sensor.minValue.toFixed(2) + '</td></tr>';
            //popupContent += '<tr><td>Max</td><td>' + sensor.maxValue.toFixed(2) + '</td></tr>';
            popupContent += '<tr><td>Avg</td><td>' + sensor.avgValue.toFixed(2) + '</td></tr>';
        });

        popupContent += '</table>';
        return popupContent;
    }
    var marker = L.marker([ltd, lng], { icon: myIcon }).bindPopup(createPopupContent(location)).addTo(map);
}
