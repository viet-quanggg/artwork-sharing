const fileInput = document.getElementById('fileInput');
const imagePreview = document.getElementById('imagePreview');
const MAX_FILES = 5;
var token = localStorage.getItem("token");
fileInput.addEventListener('change', function () {
    imagePreview.innerHTML = '';

    const files = this.files;
    if (files.length > MAX_FILES) {
        alert(`You can only upload up to ${MAX_FILES} files.`);
        this.value = '';
    }
    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        const reader = new FileReader();

        reader.onload = function (e) {
            const image = document.createElement('img');
            image.src = e.target.result;
            image.classList.add('preview-image');
            imagePreview.appendChild(image);
        };

        reader.readAsDataURL(file);
    }
});

const form = document.querySelector('.upload-form');

form.addEventListener('submit', async (event) => {
    event.preventDefault(); // Prevent the default form submission behavior
    
    const formData = new FormData(form); // Create FormData object from the form
    

    try {
        const response = await fetch('https://localhost:7270/user/artist/postartwork', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`
            },
            body: formData
        });

        if (!response.ok) {
            console.log(response);

            throw new Error('Failed to upload artwork');
        }
        console.log('create artwork successfully');
        // Handle success response
        window.location.href = "artist"; // Redirect to artist page after successful upload
    } catch (error) {
        console.error('Error uploading artwork:', error);
        // Handle error (e.g., show error message to the user)
    }
});