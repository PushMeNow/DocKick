export const combineIdentityServerUrl = (virtualUrl) => `${process.env.REACT_APP_AUTH_SERVER}/${virtualUrl}`;
export const combineCategorizableUrl = (virtualUrl) => `${process.env.REACT_APP_CATEGORIZABLE}/${virtualUrl}`;

export const getBlobsUrl = () => combineCategorizableUrl('blobs');
export const getBlobsUpdateUrl = (blobId) => combineCategorizableUrl(`blobs/${blobId}`); 
export const getBlobsUploadUrl = () => combineCategorizableUrl('blobs/upload');

export const getCategoriesUrl = () => combineCategorizableUrl('categories');