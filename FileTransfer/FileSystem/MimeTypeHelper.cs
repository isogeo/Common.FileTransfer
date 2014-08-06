using System;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Common.FileTransfer.FileSystem
{



    ////////////////////////////////////////////////////////////////////////////
    ///
    /// <summary>MIME type related methods.</summary>
    /// <remarks>Inspired by <see href="http://stackoverflow.com/a/9435701/8696" />.</remarks>
    ///
    ////////////////////////////////////////////////////////////////////////////

    public static class MimeTypeHelper
    {

        /// <summary>Determines the MIME type from the data provided.</summary>
        /// <see href="http://msdn.microsoft.com/en-us/library/ms775107.aspx" />
        /// <see href="http://msdn.microsoft.com/en-us/library/ms775147.aspx" />
        [DllImport(@"urlmon.dll", CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=false)]
        private extern static int FindMimeFromData(
            IntPtr pBC, 
            [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I1, SizeParamIndex=3)] byte[] pBuffer,
            int cbSize,
            [MarshalAs(UnmanagedType.LPWStr)] string pwzMimeProposed,
            int dwMimeFlags,
            out IntPtr ppwzMimeOut,
            int dwReserved
        );

        private static string GetMimeFromRegistry(string filename, string suggestion)
        {
            string ret=string.IsNullOrWhiteSpace(suggestion) ? _DefaultMimeType : suggestion;
            RegistryKey key=Registry.ClassesRoot.OpenSubKey(Path.GetExtension(filename).ToLower());
            if (key!=null)
            {
                object v=key.GetValue("Content Type");
                if (v!=null)
                    ret=v.ToString();
            }
            return ret;
        }

        /// <summary>Gets the MIME type from the specified file name.</summary>
        /// <param name="filename">The name of the file.</param>
        /// <param name="suggestion">A MIME type suggestion, in case no proper value is found.</param>
        /// <returns>The MIME type of the specified file.</returns>
        public static async Task<string> GetMimeTypeAsync(string filename, string suggestion)
        {
            if (!File.Exists(filename))
               return GetMimeFromRegistry(filename, suggestion);

            var file=new FileInfo(filename);
            int length=Math.Min(256, (int)file.Length);
            byte[] buffer=new byte[length];
            using (var fs=file.OpenRead())
                await fs.ReadAsync(buffer, 0, length);

            try
            {
                IntPtr mimeType=IntPtr.Zero;
                int res=FindMimeFromData(IntPtr.Zero, null, buffer, length, string.IsNullOrWhiteSpace(suggestion) ? _DefaultMimeType : suggestion, 0x20, out mimeType, 0);
                if (res==0)
                {
                    string ret=Marshal.PtrToStringUni(mimeType);
                    Marshal.FreeCoTaskMem(mimeType);

                    if (string.IsNullOrWhiteSpace(ret) || (ret=="text/plain") || (ret=="application/octet-stream"))
                        return GetMimeFromRegistry(filename, suggestion);

                    return ret;
                } else
                    return GetMimeFromRegistry(filename, suggestion);
            } catch (Exception)
            {
                return GetMimeFromRegistry(filename, suggestion);
            }
        }

        private const string _DefaultMimeType="application/octet-stream";
    }
}
