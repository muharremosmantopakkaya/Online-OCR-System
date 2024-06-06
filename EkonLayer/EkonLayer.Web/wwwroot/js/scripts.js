document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('uploadForm');
    const fileInput = document.getElementById('file');
    const dropZone = document.getElementById('drop-zone');
    const imagePreview = document.getElementById('imagePreview');
    const audioPlayer = document.getElementById('audioPlayer');
    const resultDiv = document.getElementById('result');
    const textDiv = document.getElementById('detected-text');
    const progressBar = document.getElementById('progressBar');
    const progressContainer = document.querySelector('.progress');
    const copyText = document.getElementById('copyText');
    const detectedTextList = document.createElement('ul');
    const audioContainer = document.getElementById('audio-container');

    dropZone.addEventListener('click', () => fileInput.click());

    dropZone.addEventListener('dragover', (e) => {
        e.preventDefault();
        dropZone.classList.add('hover');
    });

    dropZone.addEventListener('dragleave', () => dropZone.classList.remove('hover'));

    dropZone.addEventListener('drop', (e) => {
        e.preventDefault();
        dropZone.classList.remove('hover');
        const files = e.dataTransfer.files;
        fileInput.files = files;
        previewFiles(files);
    });

    fileInput.addEventListener('change', function () {
        previewFiles(this.files);
    });

    function previewFiles(files) {
        imagePreview.innerHTML = '';
        Array.from(files).forEach(file => {
            if (!file.type.includes('pdf')) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    const img = document.createElement('img');
                    img.src = e.target.result;
                    img.classList.add('img-fluid');
                    imagePreview.appendChild(img);
                };
                reader.readAsDataURL(file);
            }
        });
    }

    form.addEventListener('submit', function (event) {
        event.preventDefault();

        const formData = new FormData(form);
        const xhr = new XMLHttpRequest();
        xhr.open('POST', 'http://localhost:5000/upload', true);

        xhr.upload.addEventListener('progress', function (e) {
            if (e.lengthComputable) {
                if (progressContainer) {
                    progressContainer.style.display = 'block';
                    const percentComplete = (e.loaded / e.total) * 100;
                    if (progressBar) {
                        progressBar.style.width = percentComplete + '%';
                        progressBar.textContent = Math.round(percentComplete) + '%';
                    }
                }
            }
        });

        xhr.onload = function () {
            if (progressContainer) {
                progressContainer.style.display = 'none';
            }
            if (xhr.status === 200) {
                try {
                    if (xhr.getResponseHeader('content-type').includes('application/json')) {
                        const response = JSON.parse(xhr.responseText);
                        console.log(response);

                        if (response.processed_image_url) {
                            resultDiv.innerHTML = `<img src="${response.processed_image_url}" alt="Processed Image" class="img-fluid"/>`;
                            if (textDiv) {
                                textDiv.style.display = 'block';
                                detectedTextList.innerHTML = response.detected_text.map(text => `<li>${text}</li>`).join('');
                                textDiv.appendChild(detectedTextList);
                            }
                            if (copyText) {
                                copyText.value = response.detected_text.join('\n');
                            }

                            if (response.audio_file_url) {
                                const audioUrl = `${response.audio_file_url}?t=${new Date().getTime()}`;
                                if (audioPlayer) {
                                    audioPlayer.src = audioUrl;
                                    audioPlayer.load();
                                    audioPlayer.style.display = 'block';
                                }
                                if (audioContainer) {
                                    audioContainer.style.display = 'block';
                                }
                            }
                        } else if (response.error) {
                            alert('Hata: ' + response.error);
                        } else {
                            alert('Resim i�lenirken hata olu�tu.');
                        }
                    } else {
                        console.error("Yan�t JSON format�nda de�il:", xhr.responseText);
                        alert('Yan�t JSON format�nda de�il. L�tfen sunucunun do�ru JSON yan�t� d�nd�rd���nden emin olun.');
                    }
                } catch (e) {
                    console.error("Yan�t JSON format�nda de�il:", e);
                    console.error("Yan�t i�eri�i:", xhr.responseText);
                    alert('Yan�t JSON format�nda de�il. L�tfen sunucunun do�ru JSON yan�t� d�nd�rd���nden emin olun.');
                }
            } else {
                alert('Resim y�klenirken hata olu�tu.');
            }
        };

        xhr.onerror = function () {
            alert('Bir a� hatas� olu�tu. L�tfen ba�lant�n�z� kontrol edin.');
        };

        xhr.onloadend = function () {
            if (progressContainer) {
                progressContainer.style.display = 'none';
            }
        };

        xhr.send(formData);
    });
});
