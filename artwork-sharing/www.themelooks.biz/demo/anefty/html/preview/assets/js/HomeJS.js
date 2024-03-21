document.addEventListener('DOMContentLoaded', function() {
    // Fetch header content
    fetch('header.html')
        .then(response => response.text())
        .then(html => {
            // Insert header content into headerContainer
            document.getElementById('headerContainer').innerHTML = html;
        });

    fetch('footer.html')
        .then(response => response.text())
        .then(html => {
            // Insert header content into headerContainer
            document.getElementById('footerContainer').innerHTML = html;
        });
});

document.addEventListener('DOMContentLoaded', function() {
    // Fetch header content
   
});