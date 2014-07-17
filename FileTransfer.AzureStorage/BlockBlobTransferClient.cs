using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Common.FileTransfer.AzureStorage
{



#pragma warning disable 3001, 3003
    ////////////////////////////////////////////////////////////////////////////
    ///
    /// <summary>An Azure storage Blob implementation of a file transfer client.</summary>
    ///
    ////////////////////////////////////////////////////////////////////////////

    public class BlockBlobTransferClient:
        IFileTransferClient
    {

        /// <summary>Creates a new instance of the <see cref="BlockBlobTransferClient" /> class.</summary>
        /// <param name="baseAddress"></param>
        public BlockBlobTransferClient(string connectionString, string containerName)
        {
            _ConnectionString=connectionString;
            _ContainerName=containerName;
        }

        /// <summary>Downloads the file referenced by the specified <paramref name="relativePath" />.</summary>
        /// <param name="relativePath">The URI to the file to be downloaded, relative to the client base address.</param>
        /// <returns>The file.</returns>
        public async Task<ITransferableFile> DownloadAsync(Uri relativePath)
        {
            await Container.CreateIfNotExistsAsync();
            var blob=Container.GetBlockBlobReference(relativePath.ToString());
            //blob.
            //var ret=new TransferableFile( () => 
            //);
            //blob.DownloadToStreamAsync()

            throw new NotImplementedException();
        }

        /// <summary>Uploads the specified file.</summary>
        /// <param name="file">The file to upload.</param>
        /// <returns>The URI that will be used to <see cref="DownloadAsync">download</see> the file.</returns>
        public async Task<Uri> UploadAsync(ITransferableFile file)
        {
            await Container.CreateIfNotExistsAsync();
            var blob=Container.GetBlockBlobReference(file.Name);

            blob.Properties.ContentType=await file.GetMimeTypeAsync();
            await blob.SetPropertiesAsync();
            await blob.UploadFromStreamAsync(await file.GetContentAsync());

            return new Uri(file.Name);
        }

        protected CloudBlobContainer Container
        {
            get
            {
                if (_Container==null)
                {
                    _Account=CloudStorageAccount.Parse(_ConnectionString);
                    _Client=_Account.CreateCloudBlobClient();
                    _Container=_Client.GetContainerReference(_ContainerName);
                }
                return _Container;
            }
        }

        /// <summary>Event triggered when a path is being resolved.</summary>
        //public event EventHandler<ResolvingPathEventArgs> ResolvingPath;

        private string _ConnectionString;
        private string _ContainerName;
        private CloudStorageAccount _Account;
        private CloudBlobClient _Client;
        private CloudBlobContainer _Container;
    }
#pragma warning restore 3001, 3003
}
