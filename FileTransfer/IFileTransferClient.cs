using System;
using System.IO;
using System.Threading.Tasks;

namespace Common.FileTransfer
{



    ////////////////////////////////////////////////////////////////////////////
    ///
    /// <summary>Interface implemented by a file transfer client.</summary>
    ///
    ////////////////////////////////////////////////////////////////////////////

    public interface IFileTransferClient
    {

        /// <summary>Downloads the file referenced by the specified <paramref name="path" />.</summary>
        /// <param name="path">The URI to the file to be downloaded.</param>
        /// <returns>The file.</returns>
        Task<TransferableFile> DownloadAsync(Uri path);

        /// <summary>Event triggered when a path is being resolved.</summary>
        event EventHandler<ResolvingPathEventArgs> ResolvingPath;
    }
}
