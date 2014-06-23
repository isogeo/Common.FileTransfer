using System;
using System.Diagnostics;

namespace Common.FileTransfer
{
    public class ResolvingPathEventArgs:
        EventArgs
    {

        private ResolvingPathEventArgs()
        {
        }

        public ResolvingPathEventArgs(Uri baseAddress, string relativePath)
        {
            Debug.Assert(baseAddress!=null);
            if (baseAddress==null)
                throw new ArgumentNullException("baseAddress");

            BaseAddress=baseAddress;
            RelativePath=relativePath;
            ResolvedPath=new Uri(baseAddress, RelativePath);
        }

        public Uri BaseAddress
        {
            get;
            private set;
        }

        public string RelativePath
        {
            get;
            set;
        }

        public Uri ResolvedPath
        {
            get;
            set;
        }
    }
}
