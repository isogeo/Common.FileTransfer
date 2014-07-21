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

        /// <summary>Consolidates all the uploaded chunks.</summary>
        /// <param name="path">The path to the uploaded file.</param>
        /// <param name="chunks">the number of chunks composing the file.</param>
        Task CommitUpload(Uri path, int chunks);
    }
}
