using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Common.FileTransfer
{



    ////////////////////////////////////////////////////////////////////////////
    ///
    /// <summary>Interface implemented by a transferable file.</summary>
    ///
    ////////////////////////////////////////////////////////////////////////////

    public interface ITransferableFile
    {

        /// <summary>Gets a stream to the content of the file.</summary>
        /// <remarks>It is the responsibility of the caller to <see cref="Stream.Dispose()" /> the returned stream.</remarks>
        Task<Stream> GetContentAsync();

        /// <summary>Asynchronously reads the bytes from the current stream and writes them to another stream.</summary>
        /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
        /// <returns>A task that represents the asynchronous copy operation.</returns>
        Task CopyContentToAsync(Stream destination);

        /// <summary>Gets the length of the transferable file.</summary>
        Task<long?> GetLengthAsync();

        /// <summary>Gets the MIME type associated with the transferable file.</summary>
        /// <returns>The MIME type associated with the transferable file.</returns>
        Task<string> GetMimeTypeAsync();

        /// <summary>Gets the name of the transferable file.</summary>
        string Name { get; }
    }
}
