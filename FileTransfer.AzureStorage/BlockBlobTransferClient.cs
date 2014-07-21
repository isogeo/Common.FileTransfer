using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        ILargeFileTransferClient
    {

        /// <summary>Creates a new instance of the <see cref="BlockBlobTransferClient" /> class.</summary>
        /// <param name="baseAddress"></param>
        public BlockBlobTransferClient(string connectionString, string containerName)
        {
            _ConnectionString=connectionString;
            _ContainerName=containerName;
        }

        /// <summary>Deletes the file referenced by the specified <paramref name="path" />.</summary>
        /// <param name="relativePath">The URI to the file to be deleted, relative to the client base address.</param>
        public async Task DeleteAsync(Uri relativePath)
        {
            if (!await Container.ExistsAsync())
                return;

            var blob=Container.GetBlockBlobReference(relativePath.ToString());
            await blob.DeleteIfExistsAsync();
        }

        /// <summary>Downloads the file referenced by the specified <paramref name="relativePath" />.</summary>
        /// <param name="relativePath">The URI to the file to be downloaded, relative to the client base address.</param>
        /// <returns>The file.</returns>
        public async Task<ITransferableFile> DownloadAsync(Uri relativePath)
        {
            await Container.CreateIfNotExistsAsync();
            var blob=Container.GetBlockBlobReference(relativePath.ToString());
            return new BlockBlobTransferableFile(blob);
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

        /// <summary>Uploads the specified chunk.</summary>
        /// <param name="id">The identifier of the chunk.</param>
        /// <param name="chunk">The chunk.</param>
        public async Task<Uri> UploadChunkAsync(int id, ITransferableFile chunk)
        {
            await Container.CreateIfNotExistsAsync();
            var blob=Container.GetBlockBlobReference(chunk.Name);
            var sid=string.Format(
                CultureInfo.InvariantCulture,
                "{1:X8}",
                id
            );

            await blob.PutBlockAsync(sid, await chunk.GetContentAsync(), null);

            return new Uri(chunk.Name);
        }

        /// <summary>Consolidates all the uploaded chunks.</summary>
        /// <param name="ids">The ordered list of chunk identifiers.</param>
        public async Task CommitUpload(Uri path, int chunks)
        {
            await Container.CreateIfNotExistsAsync();
            var blob=Container.GetBlockBlobReference(path.ToString());

            await blob.PutBlockListAsync(Enumerable.Range(0, chunks).Select( i => i.ToString("{0:X8}", CultureInfo.InvariantCulture)));
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
