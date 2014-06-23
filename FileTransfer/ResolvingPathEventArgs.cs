using System;
using System.Diagnostics;

namespace Common.FileTransfer
{



    ////////////////////////////////////////////////////////////////////////////
    ///
    /// <summary>Handles arguments for a path resolving event.</summary>
    ///
    ////////////////////////////////////////////////////////////////////////////

    public class ResolvingPathEventArgs:
        EventArgs
    {

        private ResolvingPathEventArgs()
        {
        }

        /// <summary>Creates a new instance of the <see cref="ResolvingPathEventArgs" /> class.</summary>
        /// <param name="baseAddress">The base address for the resolved path.</param>
        /// <param name="relativePath">The relative path to resolve.</param>
        public ResolvingPathEventArgs(Uri baseAddress, string relativePath)
        {
            Debug.Assert(baseAddress!=null);
            if (baseAddress==null)
                throw new ArgumentNullException("baseAddress");

            BaseAddress=baseAddress;
            RelativePath=relativePath;
            ResolvedPath=new Uri(baseAddress, RelativePath);
        }

        /// <summary>Gets the base address for the resolved path.</summary>
        public Uri BaseAddress
        {
            get;
            private set;
        }

        /// <summary>Gets or sets the relative path to resolve.</summary>
        public string RelativePath
        {
            get;
            set;
        }

        /// <summary>Gets or sets the resolved path.</summary>
        public Uri ResolvedPath
        {
            get;
            set;
        }
    }
}
