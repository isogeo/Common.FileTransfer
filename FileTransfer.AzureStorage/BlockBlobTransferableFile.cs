using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Common.FileTransfer.AzureStorage
{



#pragma warning disable 3001
    ////////////////////////////////////////////////////////////////////////////
    ///
    /// <summary>Class that represents a transferable file.</summary>
    ///
    ////////////////////////////////////////////////////////////////////////////

    public class BlockBlobTransferableFile:
        TransferableFile
    {

        /// <summary>Creates a new instance of the <see cref="TransferableFile" /> class.</summary>
        /// <param name="content">A function that can be called to retrieve the content of the transferable file.</param>
        public BlockBlobTransferableFile(CloudBlockBlob blob)
        {
            Debug.Assert(blob!=null);
            if (blob==null)
                throw new ArgumentNullException("blob");

            _Blob=blob;
        }

        /// <summary>Gets a stream to the content of the file.</summary>
        /// <remarks>It is the responsibility of the caller to <see cref="Stream.Dispose()" /> the returned stream.</remarks>
        public override async Task<Stream> GetContentAsync()
        {
            var ret=new MemoryStream();
            await _Blob.DownloadToStreamAsync(ret);
            return ret;
        }

        /// <summary>Asynchronously reads the bytes from the current stream and writes them to another stream.</summary>
        /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
        /// <returns>A task that represents the asynchronous copy operation.</returns>
        public override async Task CopyContentToAsync(Stream destination)
        {
            await _Blob.DownloadToStreamAsync(destination);
        }

        /// <summary>Gets the length of the transferable file.</summary>
        public override async Task<long?> GetLengthAsync()
        {
            await _Blob.FetchAttributesAsync();
            return _Blob.Properties.Length;
        }

        /// <summary>Gets the MIME type associated with the transferable file.</summary>
        /// <returns>The MIME type associated with the transferable file.</returns>
        public override async Task<string> GetMimeTypeAsync()
        {
            await _Blob.FetchAttributesAsync();
            return _Blob.Properties.ContentType;
        }
        public override string Name
        {
            get
            {
                return _Blob.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        private CloudBlockBlob _Blob;
    }
#pragma warning restore 3001
}
