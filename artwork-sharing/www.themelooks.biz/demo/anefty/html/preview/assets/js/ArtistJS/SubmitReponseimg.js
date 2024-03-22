const fileInput = document.getElementById('fileInput');
const imagePreview = document.getElementById('imagePreview');
const MAX_FILES = 5; // Increased to allow uploading up to 5 files

fileInput.addEventListener('change', function () {
    imagePreview.innerHTML = '';

    const files = this.files;
    if (files.length > MAX_FILES) {
        alert(`You can only upload up to ${MAX_FILES} files.`);
        this.value = '';
        return; // Added to prevent further execution if files exceed the limit
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

const form = document.querySelector('#UploadImg');

form.addEventListener('submit', async (event) => {
    event.preventDefault();

    const itemId = localStorage.getItem('ArtworkId');

    for (let i = 0; i < fileInput.files.length; i++) {
        const formData = new FormData();
        formData.append('file', fileInput.files[i]);

        try {
            const response = await fetch('https://localhost:7270/api/ImageUpload/single', {
                method: 'POST',
                body: formData
            });
            const fileUrl = await response.text(); 
            console.log(fileUrl.toString());

            if (!response.ok) {
                throw new Error('Failed to upload artwork');
            }
            else {
                try {
                    const response1 = await fetch('https://localhost:7270/CommitArtworkByArtist/'+itemId, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify({ artworkProduct: fileUrl })
                        });
                } catch (error) {
                    console.error('Error uploading artwork:', error);
                }
            }
            console.log('create artwork successfully');
        } catch (error) {
            console.error('Error uploading artwork:', error);
            // Handle error (e.g., show error message to the user)
        }
    }
});
