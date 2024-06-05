var map;
function initialize_leaflet() {
    if (map_g == null) {
        map = L.map('map').setView([52.133, 19.615], 7);
    }
    else {
        map = L.map('map').setView([map_g.getCenter().lat(), map_g.getCenter().lng()], map_g.getZoom());
    }
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png',
        {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        }).addTo(map);



}

var map_g;
function initialize_google() {
    map_g = new google.maps.Map(document.getElementById("map2"), {
        zoom: map.getZoom(),
        //zoom: 12,
        center: { lat: map.getCenter().lat, lng: map.getCenter().lng },
        //center: { lat: 52.216, lng: 21.018 },
    });
    const trafficLayer = new google.maps.TrafficLayer();

    trafficLayer.setMap(map_g);

    cords.forEach(function (data) {
        var marker = new google.maps.Marker({
            position: { lat: data.lat, lng: data.lng },
            map: map_g,
            title: data.info,
            icon: 'https://i.imgur.com/F2QlQ4v.png'
        });
        marker.setMap(map);
    });
}

var cords = [
    { lat: 52.220833, lng: 20.984733 },
    { lat: 52.220253, lng: 20.916736 },
    { lat: 52.198525, lng: 21.051978 },
    { lat: 52.189025, lng: 20.914143 },
    { lat: 52.231183, lng: 21.127038 },
    { lat: 52.297869, lng: 21.016926 },
    { lat: 52.242522, lng: 20.907058 },
    { lat: 52.241948, lng: 20.994765 },
    { lat: 52.234843, lng: 21.036877 },
    { lat: 52.268504, lng: 20.986100 },
    { lat: 52.235361, lng: 20.972201 },
    { lat: 52.182608, lng: 20.987679 },
    { lat: 52.179677, lng: 21.023081 }
]


function addMarkerToMap_g() {
    var marker = new google.maps.Marker({
        position: { lat: mag_g.getCenter().lat(), lng: mag_g.getCenter().lng() },
        map: map_g,
        title: 'info'
    });
    marker.setMap(map_g);
}

function addMarkers_leaflet(ltd, lng, aqi, className, location) {
    var myIcon = L.divIcon({ className: className, html: '<b>' + aqi + '</b>' });
    function createPopupContent(location) {
        let popupContent = '<table>';
        popupContent += '<tr><td><b>' + location.name + '</b></td></tr>';
        popupContent += '<tr><td>' + location.latitude + ' ' + location.longitude + '</td></tr>';

        location.sensors.forEach(sensor => {
            popupContent += '<tr><td></td></tr>';
            popupContent += '<tr><td>Sensor:</td><td><b>' + sensor.name + '</b></td></tr>';
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
