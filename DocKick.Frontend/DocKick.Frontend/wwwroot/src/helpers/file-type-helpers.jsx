const supportedImageExtensions = [
    '.jpeg',
    '.jpg',
    '.png'
];

function getFileExtension(fileName) {
    if (!fileName) {
        console.error('File name cannot be empty.');

        return null;
    }
    
    let startExtensionIndex = fileName.lastIndexOf('.');
    
    return fileName.substr(startExtensionIndex);
}

export const supportedImageTypes = [
    'image/jpeg',
    'image/png',
];

export const isSupportedImage = (fileName) => {
    if (!fileName) {
        console.error('Image name cannot be empty.');
        
        return false;
    }
    
    let fileExtension = getFileExtension(fileName);
    
    return supportedImageExtensions.includes(fileExtension);
}