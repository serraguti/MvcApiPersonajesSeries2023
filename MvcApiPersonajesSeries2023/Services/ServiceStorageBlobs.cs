﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MvcApiPersonajesSeries2023.Services
{
    public class ServiceStorageBlobs
    {
        private BlobContainerClient container;

        public ServiceStorageBlobs(BlobContainerClient container)
        {
            this.container = container;
        }

        //METODO PARA SUBIR EL BLOB AL SERVIDOR
        public async Task<string> UploadBlobAsync
            (string fileName, Stream stream)
        {
            await this.container.UploadBlobAsync(fileName, stream);
            //RECUPERAMOS LA URL DE NUESTRO CONTAINER
            //https://storageeoipaco.blob.core.windows.net/imagenes
            string url = this.container.Uri.AbsoluteUri;
            //CONCATENAMOS LA URI CON NUESTRO FICHERO SUBIDO
            //https://storageeoipaco.blob.core.windows.net/imagenes/5.jpg
            url = url + "/" + fileName;
            return url;
        }
    }
}
