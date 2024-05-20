var map;
function initialize() {
    map = L.map('map').setView([52.133, 19.615], 7);
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png',
        {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        }).addTo(map);



}

function addMarkers(ltd, lng, text, className) {
    var myIcon = L.divIcon({ className: className, html:'<b>'+text+'</b>' });
    var marker = L.marker([ltd, lng], { icon: myIcon }).addTo(map);
}
