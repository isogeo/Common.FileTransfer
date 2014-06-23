using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Common.FileTransfer
{
    public abstract class FileTransferClient:
        IFileTransferClient
    {

        private FileTransferClient()
        {
        }

        protected FileTransferClient(Uri baseAddress)
        {
            Debug.Assert(baseAddress!=null);
            if (baseAddress==null)
                throw new ArgumentNullException("baseAddress");

            _BaseAddress=baseAddress;
        }

        public Task<TransferableFile> DownloadAsync(Uri relativePath)
        {
            var args=new ResolvingPathEventArgs(_BaseAddress, relativePath.ToString());
            OnResolvingPath(args);
            return DoDownloadAsync(args.ResolvedPath);
        }

        protected virtual void OnResolvingPath(ResolvingPathEventArgs e)
        {
            if (ResolvingPath!=null)
                ResolvingPath(this, e);
        }

        protected abstract Task<TransferableFile> DoDownloadAsync(Uri path);

        public event EventHandler<ResolvingPathEventArgs> ResolvingPath;

        private Uri _BaseAddress;
    }
}
