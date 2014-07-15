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

        /// <summary>Uploads the specified file.</summary>
        /// <param name="file">The file to upload.</param>
        /// <returns>The URI that will be used to <see cref="FileTransferClient.DownloadAsync">download</see> the file.</returns>
        protected override async Task<Uri> DoUploadAsync(TransferableFile file)
        {
            var path=Path.Combine(BaseAddress.LocalPath, file.Name);
            using (var dfs=File.Create(path, 1024, FileOptions.Asynchronous | FileOptions.WriteThrough))
                using (var sfs=file.Content)
                    await sfs.CopyToAsync(dfs);

            return new Uri(path);
        }
    }
}
