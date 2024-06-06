document.addEventListener('DOMContentLoaded', function() {
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
    const downloadLinks = document.getElementById('downloadLinks');
    const detectedTextList = document.getElementById('detectedTextList');
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

    fileInput.addEventListener('change', function() {
        previewFiles(this.files);
    });

    function previewFiles(files) {
        imagePreview.innerHTML = '';
        Array.from(files).forEach(file => {
            if (!file.type.includes('pdf')) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    const img = document.createElement('img');
                    img.src = e.target.result;
                    img.classList.add('img-fluid');
                    imagePreview.appendChild(img);
                };
                reader.readAsDataURL(file);
            }
        });
    }

    form.addEventListener('submit', function(event) {
        event.preventDefault();

        const formData = new FormData(form);
        const xhr = new XMLHttpRequest();
        xhr.open('POST', '/Home/Upload', true);

        xhr.upload.addEventListener('progress', function(e) {
            if (e.lengthComputable) {
                progressContainer.style.display = 'block';
                const percentComplete = (e.loaded / e.total) * 100;
                progressBar.style.width = percentComplete + '%';
                progressBar.textContent = Math.round(percentComplete) + '%';
            }
        });

        xhr.onload = function() {
            progressContainer.style.display = 'none';
            if (xhr.status === 200) {
                try {
                    // Yanıtın JSON olup olmadığını kontrol edin
                    if (xhr.getResponseHeader('content-type').includes('application/json')) {
                        const response = JSON.parse(xhr.responseText);
                        console.log(response); // Yanıtı kontrol etmek için tarayıcı konsolunda görün

                        if (response.processed_image_url) {
                            resultDiv.innerHTML = `<img src="${response.processed_image_url}" alt="Processed Image" class="img-fluid"/>`;
                            textDiv.style.display = 'block';
                            detectedTextList.innerHTML = response.detected_text.map(text => `<li>${text}</li>`).join('');
                            copyText.value = response.detected_text.join('\n');

                            if (response.audio_file_url) {
                                const audioUrl = `${response.audio_file_url}?t=${new Date().getTime()}`;
                                audioPlayer.src = audioUrl;
                                audioContainer.style.display = 'block';
                                audioPlayer.load();

                                downloadLinks.innerHTML = `
                            <a href="${response.processed_image_url}" download class="btn btn-success mt-3">İşlenmiş Görüntüyü İndir</a>
                            <a href="${audioUrl}" download class="btn btn-success mt-3">Ses Dosyasını İndir</a>
                        `;
                            }
                        } else if (response.error) {
                            alert('Hata: ' + response.error);
                        } else {
                            alert('Resim işlenirken hata oluştu.');
                        }
                    } else {
                        console.error("Yanıt JSON formatında değil:", xhr.responseText);
                        alert('Yanıt JSON formatında değil. Lütfen sunucunun doğru JSON yanıtı döndürdüğünden emin olun.');
                    }
                } catch (e) {
                    console.error("Yanıt JSON formatında değil:", e);
                    console.error("Yanıt içeriği:", xhr.responseText);
                    alert('Yanıt JSON formatında değil. Lütfen sunucunun doğru JSON yanıtı döndürdüğünden emin olun.');
                }
            } else {
                alert('Resim yüklenirken hata oluştu.');
            }
        };


        xhr.onerror = function() {
            alert('Bir ağ hatası oluştu. Lütfen bağlantınızı kontrol edin.');
        };

        xhr.onloadend = function() {
            progressContainer.style.display = 'none';
        };

        xhr.send(formData);
    });
});
