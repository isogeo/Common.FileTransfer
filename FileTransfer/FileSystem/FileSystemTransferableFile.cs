using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Common.FileTransfer.FileSystem
{



    ////////////////////////////////////////////////////////////////////////////
    ///
    /// <summary>Class that represents a transferable file.</summary>
    ///
    ////////////////////////////////////////////////////////////////////////////

    public class FileSystemTransferableFile:
        TransferableFile
    {

        /// <summary>Creates a new instance of the <see cref="TransferableFile" /> class.</summary>
        /// <param name="file">A function that can be called to retrieve the content of the transferable file.</param>
        public FileSystemTransferableFile(FileInfo file)
        {
            Debug.Assert(file!=null);
            if (file==null)
                throw new ArgumentNullException("file");

            _File=file;
        }

        /// <summary>Gets a stream to the content of the file.</summary>
        /// <remarks>It is the responsibility of the caller to <see cref="Stream.Dispose()" /> the returned stream.</remarks>
        public override Task<Stream> GetContentAsync()
        {
            return Task.FromResult(_File.OpenRead() as Stream);
        }

        /// <summary>Asynchronously reads the bytes from the current stream and writes them to another stream.</summary>
        /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
        /// <returns>A task that represents the asynchronous copy operation.</returns>
        public override async Task CopyContentToAsync(Stream destination)
        {
            using (var s=_File.OpenRead())
                await s.CopyToAsync(destination);
        }

        /// <summary>Gets the length of the transferable file.</summary>
        public override Task<long?> GetLengthAsync()
        {
            return Task.FromResult((long?)_File.Length);
        }

        /// <summary>Gets the MIME type associated with the transferable file.</summary>
        /// <returns>The MIME type associated with the transferable file.</returns>
        public override async Task<string> GetMimeTypeAsync()
        {
            return await MimeTypeHelper.GetMimeTypeAsync(_File.FullName, null);
        }

        private FileInfo _File;
    }
}
