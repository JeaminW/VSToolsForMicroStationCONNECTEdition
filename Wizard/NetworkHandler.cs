using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace innovoCAD.Bentley.CONNECT
{
    public sealed class NetworkHandler
    {
        /// <summary>
        /// Private constructor to prevent compiler from generating one
        /// since this class only holds static methods and properties
        /// </summary>
        NetworkHandler() { }

        /// <summary>
        /// SafeNativeMethods Class that holds save native methods 
        /// while suppressing unmanaged code security
        /// </summary>
        [SuppressUnmanagedCodeSecurityAttribute]
        internal static class SafeNativeMethods
        {
            // Extern Library
            // UnManaged code - be careful.
            [DllImport("wininet.dll", CharSet = CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private extern static bool
                InternetGetConnectedState(out int Description, int ReservedValue);

            /// <summary>
            /// Determines if there is an active connection on this computer
            /// </summary>
            /// <returns></returns>
            public static bool HasActiveConnection()
            {
                int desc;
                return InternetGetConnectedState(out desc, 0);
            }
        }
    }
}
