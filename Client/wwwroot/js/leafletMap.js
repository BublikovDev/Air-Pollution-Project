var map;
function initialize() {
    map = L.map('map').setView([52.133, 19.615], 7);
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png',
        {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        }).addTo(map);


var marker = L.marker([52.200577, 20.895988]).addTo(map);
var marker = L.marker([52.192678, 20.838901]).addTo(map);
var marker = L.marker([52.230321, 20.834141]).addTo(map);
}
