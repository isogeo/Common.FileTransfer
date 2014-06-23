using System;
using System.IO;
using System.Threading.Tasks;

namespace Common.FileTransfer.FileSystem
{



    ////////////////////////////////////////////////////////////////////////////
    ///
    /// <summary>A file system implementation of a file transfer client.</summary>
    ///
    ////////////////////////////////////////////////////////////////////////////

    public class FileSystemTransferClient:
        FileTransferClient
    {

        /// <summary>Creates a new instance of the <see cref="FileSystemTransferClient" /> class.</summary>
        /// <param name="baseAddress"></param>
        public FileSystemTransferClient(Uri baseAddress):
            base(baseAddress)
        {
        }

        /// <summary>Downloads the file referenced by the specified <paramref name="path" />.</summary>
        /// <param name="path">The absolute URI to the file to be downloaded.</param>
        /// <returns>The file.</returns>
        protected override Task<TransferableFile> DoDownloadAsync(Uri path)
        {
            string localPath=path.LocalPath;
            var fi=new FileInfo(localPath);

            var ret=new TransferableFile(() => fi.OpenRead(), fi.Length);
            return Task.FromResult(ret);
        }
    }
}
