using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Common.FileTransfer
{



    ////////////////////////////////////////////////////////////////////////////
    ///
    /// <summary>Class that represents a transferable file.</summary>
    ///
    ////////////////////////////////////////////////////////////////////////////

    public class TransferableFile
    {

        private TransferableFile()
        { }

        /// <summary>Creates a new instance of the <see cref="TransferableFile" /> class.</summary>
        /// <param name="content">A function tahat can be called to retrieve the content of the transferable file.</param>
        public TransferableFile(Func<Stream> content)
        {
            Debug.Assert(content!=null);
            if (content==null)
                throw new ArgumentNullException("content");

            _Content=content;
        }

        /// <summary>Creates a new instance of the <see cref="TransferableFile" /> class.</summary>
        /// <param name="content">A function tahat can be called to retrieve the content of the transferable file.</param>
        /// <param name="length">The length of the file.</param>
        public TransferableFile(Func<Stream> content, long length):
            this(content)
        {
            Debug.Assert(length>0);
            if (length<=0)
                throw new ArgumentOutOfRangeException("length", length, "");

            _Length=length;
        }

        /// <summary>Gets a stream to the content of the file.</summary>
        /// <remarks>It is the responsibility of the caller to <see cref="Stream.Dispose()" /> thje returned stream.</remarks>
        public Stream Content
        {
            get
            {
                return _Content();
            }
        }

        /// <summary>Gets or sets the MIME type of the transferable file.</summary>
        public string MimeType
        {
            get
            {
                return _MimeType;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _MimeType=_DefaultMimetype;
                _MimeType=value;
            }
        }

        /// <summary>Gets the length of the transferable file.</summary>
        public long? Length
        {
            get
            {
                return _Length;
            }
        }

        private string _MimeType=_DefaultMimetype;
        private Func<Stream> _Content;
        private long? _Length;

        private const string _DefaultMimetype="application/octet-stream";
    }
}
