window.readFileData = (file) => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();

    reader.onload = () => {
      resolve({
        name: file.name,
        size: file.size,
        type: file.type,
        content: reader.result.split(',')[1] // Base64 encoded content
      });
    };

    reader.onerror = reject;
    reader.readAsDataURL(file); // Read file as base64 string
  });
};

window.handleFileInputChange = (dotNetHelper, inputId) => {
  const inputElement = document.getElementById(inputId);
  const files = inputElement.files;

  if (files.length > 0) {
    const file = files[0];

    window.readFileData(file).then(fileData => {
      dotNetHelper.invokeMethodAsync('ReceiveFileData', fileData);
    }).catch(error => {
      console.error('Error reading file:', error);
    });
  }
};


window.setupDragAndDrop = (dotNetHelper, dropZoneId) => {
  const dropZone = document.getElementById(dropZoneId);

  dropZone.addEventListener('dragover', (event) => {
    event.preventDefault();
    dropZone.classList.add('drag-over'); // Add a class to highlight the drop zone
  });

  dropZone.addEventListener('dragenter', (event) => {
    event.preventDefault();
    dropZone.classList.add('drag-over'); // Add a class to highlight the drop zone
  });

  dropZone.addEventListener('dragleave', (event) => {
    event.preventDefault();
    dropZone.classList.remove('drag-over'); // Remove the highlight class
  });

  dropZone.addEventListener('drop', (event) => {
    event.preventDefault();
    dropZone.classList.remove('drag-over'); // Remove the highlight class

    const files = [];
    const filePromises = [];

    for (let i = 0; i < event.dataTransfer.files.length; i++) {
      const file = event.dataTransfer.files[i];
      const reader = new FileReader();

      const filePromise = new Promise((resolve, reject) => {
        reader.onload = () => {
          files.push({
            name: file.name,
            size: file.size,
            type: file.type,
            content: reader.result.split(',')[1] // Base64 encoded content
          });
          resolve();
        };
        reader.onerror = reject;

        reader.readAsDataURL(file); // Read file as base64 string
      });

      filePromises.push(filePromise);
    }

    Promise.all(filePromises).then(() => {
      // Send the files data back to Blazor
      dotNetHelper.invokeMethodAsync('ProcessDroppedFiles', files);
    }).catch(error => {
      console.error('Error reading files:', error);
    });
  });
};



