using System;
using System.IO;
using System.Threading.Tasks;

namespace Common.FileTransfer.FileSystem
{
    public class FileSystemTransferClient:
        FileTransferClient
    {

        public FileSystemTransferClient(Uri baseAddress):
            base(baseAddress)
        {
        }

        protected override Task<TransferableFile> DoDownloadAsync(Uri path)
        {
            string localPath=path.LocalPath;
            var fi=new FileInfo(localPath);

            var ret=new TransferableFile(() => fi.OpenRead(), fi.Length);
            return Task.FromResult(ret);
        }
    }
}
