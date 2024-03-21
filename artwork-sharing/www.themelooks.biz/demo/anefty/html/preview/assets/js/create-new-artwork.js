const fileInput = document.getElementById('fileInput');
const imagePreview = document.getElementById('imagePreview');
const MAX_FILES = 5;
fileInput.addEventListener('change', function() {
    imagePreview.innerHTML = ''; 
    
    const files = this.files;
    if (files.length > MAX_FILES) {
        alert(`You can only upload up to ${MAX_FILES} files.`);
        this.value = ''; 
    }
    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        const reader = new FileReader();

        reader.onload = function(e) {
            const image = document.createElement('img');
            image.src = e.target.result;
            image.classList.add('preview-image');
            imagePreview.appendChild(image);
        };

        reader.readAsDataURL(file);
    }
});
