function copyToClipboard() {
    var copyText = document.getElementById("bearerInput");
    copyText.select();
    copyText.setSelectionRange(0, 99999); 
    navigator.clipboard.writeText(copyText.value);
}