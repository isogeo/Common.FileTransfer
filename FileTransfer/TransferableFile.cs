using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Common.FileTransfer
{

    public class TransferableFile
    {

        private TransferableFile()
        { }

        public TransferableFile(Func<Stream> content)
        {
            Debug.Assert(content!=null);
            if (content==null)
                throw new ArgumentNullException("content");

            _Content=content;
        }

        public TransferableFile(Func<Stream> content, long length):
            this(content)
        {
            Debug.Assert(length>0);
            if (length<=0)
                throw new ArgumentOutOfRangeException("length", length, "");

            _Length=length;
        }

        public Stream Content
        {
            get
            {
                return _Content();
            }
        }

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
