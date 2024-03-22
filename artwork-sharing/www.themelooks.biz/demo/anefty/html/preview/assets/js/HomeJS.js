const token = localStorage.getItem('token');
document.addEventListener('DOMContentLoaded', function() {
    // Fetch header content
    fetch('header.html')
    .then(response => response.text())
    .then(html => {
        // Insert header content into headerContainer
        document.getElementById('headerContainer').innerHTML = html;
        if (!token) {
            document.getElementById('is-login').style.display = 'none';
        } else {
            document.getElementById('is-login').style.display = 'list-item';
        }
    
        // Add event listener to the logout link
        const logoutLink = document.getElementById('logout-link');
        if (logoutLink) {
            logoutLink.addEventListener('click', function(event) {
                event.preventDefault();
                logout(); // Call the logout function
            });
        }
    });

    fetch('footer.html')
        .then(response => response.text())
        .then(html => {
            // Insert header content into headerContainer
            document.getElementById('footerContainer').innerHTML = html;
        });
});

async function logout() {
    try {
        
        if (!token) {
            // Token not found, handle the case appropriately (e.g., redirect to login page)
            window.location.href = 'login.html';
            return;
        }

        const response = await fetch('https://localhost:7270/api/Auth/Logout', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}` 
            }
        });

        if (!response.ok) {
            throw new Error('Failed to logout');
        }

        // Clear token from localStorage
        localStorage.removeItem('token');

        // Redirect user to login page after logout
        window.location.href = 'login.html';
    } catch (error) {
        console.error(error.message);
        // Handle error, such as displaying an error message to the user
        //displayErrorMessage('Failed to logout. Please try again later.');
    }
}