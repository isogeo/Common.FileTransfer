using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Common.FileTransfer
{



    ////////////////////////////////////////////////////////////////////////////
    ///
    /// <summary>Base implementation of a file transfer client.</summary>
    ///
    ////////////////////////////////////////////////////////////////////////////

    public abstract class FileTransferClient:
        IFileTransferClient
    {

        private FileTransferClient()
        {
        }

        /// <summary>Creates a new instance of the <see cref="FileTransferClient" /> class.</summary>
        /// <param name="baseAddress">The base address for this client.</param>
        protected FileTransferClient(Uri baseAddress)
        {
            Debug.Assert(baseAddress!=null);
            if (baseAddress==null)
                throw new ArgumentNullException("baseAddress");

            _BaseAddress=baseAddress;
        }

        /// <summary>Downloads the file referenced by the specified <paramref name="relativePath" />.</summary>
        /// <param name="relativePath">The URI to the file to be downloaded, relative to the client base address.</param>
        /// <returns>The file.</returns>
        public Task<TransferableFile> DownloadAsync(Uri relativePath)
        {
            var args=new ResolvingPathEventArgs(_BaseAddress, relativePath.ToString());
            OnResolvingPath(args);
            return DoDownloadAsync(args.ResolvedPath);
        }

        /// <summary>Uploads the specified file.</summary>
        /// <param name="file">The file to upload.</param>
        /// <returns>The URI that will be used to <see cref="DownloadAsync">download</see> the file.</returns>
        public Task<Uri> UploadAsync(TransferableFile file)
        {
            return DoUploadAsync(file);
        }

        /// <summary>Triggers the <see cref="ResolvingPath" /> event.</summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnResolvingPath(ResolvingPathEventArgs e)
        {
            if (ResolvingPath!=null)
                ResolvingPath(this, e);
        }

        /// <summary>Downloads the file referenced by the specified <paramref name="path" />.</summary>
        /// <param name="path">The absolute URI to the file to be downloaded.</param>
        /// <returns>The file.</returns>
        protected abstract Task<TransferableFile> DoDownloadAsync(Uri path);

        /// <summary>Uploads the specified file.</summary>
        /// <param name="file">The file to upload.</param>
        /// <returns>The URI that will be used to <see cref="DownloadAsync">download</see> the file.</returns>
        protected abstract Task<Uri> DoUploadAsync(TransferableFile file);

        /// <summary>Gets the base address for this client.</summary>
        protected Uri BaseAddress
        {
            get
            {
                return _BaseAddress;
            }
        }

        /// <summary>Event triggered when a path is being resolved.</summary>
        public event EventHandler<ResolvingPathEventArgs> ResolvingPath;

        private Uri _BaseAddress;
    }
}
