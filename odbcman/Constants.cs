using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace odbcman
{
    public class Constants
    {
        public const string ODBC_INI_REG_PATH = "SOFTWARE\\ODBC\\ODBC.INI\\";
        public const string ODBCINST_INI_REG_PATH = "SOFTWARE\\ODBC\\ODBCINST.INI\\";


    }
    public struct MutuallyExclusiveOptions
    {
        public const string RemoveDSN = "RemoveDSN";
        public const string Help = "Help";
        public const string ListDriver = "ListDriver";
        public const string ListDSN = "ListDSN";
        public const string Import = "Import";
        public const string Export = "Import";
        public const string Register = "register";
    }
}
