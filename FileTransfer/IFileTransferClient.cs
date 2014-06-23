using System;
using System.IO;
using System.Threading.Tasks;

namespace Common.FileTransfer
{

    public interface IFileTransferClient
    {

        Task<TransferableFile> DownloadAsync(Uri path);

        event EventHandler<ResolvingPathEventArgs> ResolvingPath;
    }
}
