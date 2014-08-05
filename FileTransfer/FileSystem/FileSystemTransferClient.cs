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

        /// <summary>Deletes the file referenced by the specified <paramref name="path" />.</summary>
        /// <param name="path">The URI to the file to be deleted.</param>
        protected override Task DoDeleteAsync(Uri path)
        {
            string localPath=path.LocalPath;
            var fi=new FileInfo(localPath);
            fi.Delete();

            var tcs=new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }

        /// <summary>Downloads the file referenced by the specified <paramref name="path" />.</summary>
        /// <param name="path">The absolute URI to the file to be downloaded.</param>
        /// <returns>The file.</returns>
        protected override Task<ITransferableFile> DoDownloadAsync(Uri path)
        {
            string localPath=path.LocalPath;
            var fi=new FileInfo(localPath);

            var ret=new FileSystemTransferableFile(fi);
            return Task.FromResult(ret as ITransferableFile);
        }

        /// <summary>Uploads the specified file.</summary>
        /// <param name="file">The file to upload.</param>
        /// <returns>The URI that will be used to <see cref="FileTransferClient.DownloadAsync">download</see> the file.</returns>
        protected override async Task<Uri> DoUploadAsync(ITransferableFile file)
        {
            var path=Path.Combine(BaseAddress.LocalPath, file.Name);
            var dir=Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            using (var dfs=File.Create(path, 1024, FileOptions.Asynchronous | FileOptions.WriteThrough))
                using (var sfs=await file.GetContentAsync())
                    await sfs.CopyToAsync(dfs);

            return new Uri(path);
        }
    }
}
