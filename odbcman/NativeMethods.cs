using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace odbcman
{
    class NativeMethods
    {
        [DllImport("odbc32.dll")]
        public static extern int SQLDataSources(int EnvHandle, int Direction, StringBuilder ServerName, int ServerNameBufferLenIn, ref int ServerNameBufferLenOut, StringBuilder Driver, int DriverBufferLenIn, ref int DriverBufferLenOut);

        [DllImport("odbc32.dll")]
        public static extern int SQLAllocEnv(ref int EnvHandle);

        [DllImport("ODBCCP32.dll")]
        private static extern bool SQLConfigDataSource(IntPtr parent, int request, string driver, string attributes);
    }
}
