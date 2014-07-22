using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.FileTransfer
{



    ////////////////////////////////////////////////////////////////////////////
    ///
    /// <summary>Interface implemented by a file transfer client.</summary>
    ///
    ////////////////////////////////////////////////////////////////////////////

    public interface ILargeFileTransferClient:
        IFileTransferClient
    {

        /// <summary>Uploads the specified chunk.</summary>
        /// <param name="id">The identifier of the chunk.</param>
        /// <param name="chunk">The chunk.</param>
        Task<Uri> UploadChunkAsync(int id, ITransferableFile chunk);

        /// <summary>Downloads part of the file referenced by the specified <paramref name="path" />.</summary>
        /// <param name="path">The URI to the file to be downloaded.</param>
        /// <param name="offset">The offset in the file at which to begin the download.</param>
        /// <param name="length">The length of the data to download from the file.</param>
        /// <returns>The file.</returns>
        Task<ITransferableFile> DownloadRangeAsync(Uri path, long? offset, long? length);

        /// <summary>Consolidates all the uploaded chunks.</summary>
        /// <param name="path">The path to the uploaded file.</param>
        /// <param name="chunks">the number of chunks composing the file.</param>
        Task CommitUpload(Uri path, int chunks);
    }
}
