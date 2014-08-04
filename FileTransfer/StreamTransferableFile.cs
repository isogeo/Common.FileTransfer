using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Common.FileTransfer
{



    ////////////////////////////////////////////////////////////////////////////
    ///
    /// <summary>Class that represents a transferable file based on a generic <see cref="Stream" />.</summary>
    ///
    ////////////////////////////////////////////////////////////////////////////

    public class StreamTransferableFile:
        TransferableFile
    {

        /// <summary>Creates a new instance of the <see cref="StreamTransferableFile" /> class.</summary>
        /// <param name="stream">The stream taht contains the content of the transferable file.</param>
        /// <param name="mimeType">Optional. Sets the MIME type of the data represented by the specified <paramref name="stream" />.</param>
        public StreamTransferableFile(Stream stream, string mimeType)
        {
            Debug.Assert(stream!=null);
            if (stream==null)
                throw new ArgumentNullException("stream");

            _Stream=stream;
            _MimeType=mimeType;
        }

        /// <summary>Gets a stream to the content of the file.</summary>
        /// <remarks>It is the responsibility of the caller to <see cref="Stream.Dispose()" /> the returned stream.</remarks>
        public override Task<Stream> GetContentAsync()
        {
            return Task.FromResult(_Stream);
        }

        /// <summary>Asynchronously reads the bytes from the current stream and writes them to another stream.</summary>
        /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
        /// <returns>A task that represents the asynchronous copy operation.</returns>
        public override async Task CopyContentToAsync(Stream destination)
        {
            await _Stream.CopyToAsync(destination);
        }

        /// <summary>Gets the length of the transferable file.</summary>
        public override Task<long?> GetLengthAsync()
        {
            return Task.FromResult((long?)_Stream.Length);
        }

        /// <summary>Gets the MIME type associated with the transferable file.</summary>
        /// <returns>The MIME type associated with the transferable file.</returns>
        public override Task<string> GetMimeTypeAsync()
        {
            string ret="application/octet-stream";
            if (!string.IsNullOrWhiteSpace(_MimeType))
                ret=_MimeType;
            return Task.FromResult(ret);
        }

        private Stream _Stream;
        private string _MimeType;
    }
}
