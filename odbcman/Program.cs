using CommandLine;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace odbcman
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Program intance = new Program();
            intance.HandleOptions(args);
            //Console.ReadKey();
        }

        public void HandleOptions(string[] args)
        {
            CommandOptions options = null;
            ParserSettings parserSettings = new ParserSettings();
            parserSettings.CaseSensitive = false;
            parserSettings.CaseSensitive = false;
            using (StreamWriter sw = new StreamWriter(Console.OpenStandardOutput()))
            {
                using (TextWriter ssw = TextWriter.Synchronized(sw))
                {
                    parserSettings.HelpWriter = ssw;

                    if (args != null && args.Length > 0)
                        options = CommandOptions.Parse(parserSettings, args);

                    if (options == null)
                    {
                        options = new CommandOptions();
                        Console.WriteLine(options.GetUsage());
                    }
                    else
                    {
                        Validate(options);
                    }

                    if (options.IsExportDSNoperation)
                        ExportDSN(options.File);
                    else if (options.IsHelpOperation)
                    {
                        Console.WriteLine(options.GetUsage());
                    }
                    else if (options.IsImportDSNOperation)
                        ImportDSN(options.File);
                    else if (options.IsListDriverOperation)
                        ListODBCDrivers();
                    else if (options.IsListDSNOperation)
                        ListODBCsources();
                    else if (options.IsRemoveDSNOperation)
                        RemoveDSN(options.DSNName);
                    else
                    {
                        Console.WriteLine(options.GetUsage());
                    }
                }
            }
        }

        private void ImportDSN(string file)
        {
            throw new NotImplementedException();
        }

        private void ExportDSN(string file)
        {
            throw new NotImplementedException();
        }

        void Validate(CommandOptions options)
        {

            //There are 7 mutually exclusive operations
            bool[] programOperations =
            {
                options.IsExportDSNoperation,
                options.IsHelpOperation,
                options.IsImportDSNOperation,
                options.IsListDriverOperation,
                options.IsListDSNOperation,
                options.IsRegisterOperation,
                options.IsRemoveDSNOperation
            };

            HammingWeightHelper mutualExclusionTester = new HammingWeightHelper();
            int mutualExclusiveOptions = mutualExclusionTester.popcount64a((ulong)mutualExclusionTester.booleansToInt(programOperations));

            if (mutualExclusiveOptions != 1)
            {
                Console.WriteLine("only one operation allowed at a time");
            }
        }




        ///<summary>
        /// Checks the registry to see if a DSN exists with the specified name
        ///</summary>
        ///<param name="dsnName"></param>
        ///<returns></returns>
        public static bool DSNExists(string dsnName)
        {
            var driversKey = Registry.LocalMachine.CreateSubKey(Constants.ODBCINST_INI_REG_PATH + "ODBC Drivers");
            if (driversKey == null)
                throw new Exception("ODBC Registry key for drivers does not exist");

            return driversKey.GetValue(dsnName) != null;
        }

        /// <summary>
        /// Creates a new DSN entry with the specified values. If the DSN exists, the values are updated.
        /// </summary>
        /// <param name="dsnName">Name of the DSN for use by client applications</param>
        /// <param name="description">Description of the DSN that appears in the ODBC control panel applet</param>
        /// <param name="server">Network name or IP address of database server</param>
        /// <param name="driverName">Name of the driver to use</param>
        /// <param name="trustedConnection">True to use NT authentication, false to require applications to supply username/password in the connection string</param>
        /// <param name="database">Name of the datbase to connect to</param>
        public static void CreateDSN(string dsnName, string description, string server, string driverName, bool trustedConnection, string database)
        {
            // Lookup driver path from driver name
            var driverKey = Registry.LocalMachine.CreateSubKey(Constants.ODBCINST_INI_REG_PATH + driverName);
            if (driverKey == null) throw new Exception(string.Format("ODBC Registry key for driver '{0}' does not exist", driverName));
            string driverPath = driverKey.GetValue("Driver").ToString();

            // Add value to odbc data sources
            var datasourcesKey = Registry.LocalMachine.CreateSubKey(Constants.ODBC_INI_REG_PATH + "ODBC Data Sources");
            if (datasourcesKey == null) throw new Exception("ODBC Registry key for datasources does not exist");
            datasourcesKey.SetValue(dsnName, driverName);

            // Create new key in odbc.ini with dsn name and add values
            var dsnKey = Registry.LocalMachine.CreateSubKey(Constants.ODBC_INI_REG_PATH + dsnName);
            if (dsnKey == null) throw new Exception("ODBC Registry key for DSN was not created");
            dsnKey.SetValue("Database", database);
            dsnKey.SetValue("Description", description);
            dsnKey.SetValue("Driver", driverPath);
            dsnKey.SetValue("LastUser", Environment.UserName);
            dsnKey.SetValue("Server", server);
            dsnKey.SetValue("Database", database);
            dsnKey.SetValue("Trusted_Connection", trustedConnection ? "Yes" : "No");
        }

        private string formatODBCDSNForListCommand(string serverName, string driverName)
        {
            return string.Format("{0,-32}\t\t{1,-50}", serverName, driverName);
        }

        private void ListODBCDrivers()
        {
            string[] drivers = GetInstalledDrivers();
            foreach (string driver in drivers)
            {
                Console.WriteLine(driver);
            }
        }

        public static List<String> GetSystemDriverList()
        {
            List<string> names = new List<string>();
            // get system dsn's
            Microsoft.Win32.RegistryKey reg = (Microsoft.Win32.Registry.LocalMachine).OpenSubKey("Software");
            if (reg != null)
            {
                reg = reg.OpenSubKey("ODBC");
                if (reg != null)
                {
                    reg = reg.OpenSubKey("ODBCINST.INI");
                    if (reg != null)
                    {

                        reg = reg.OpenSubKey("ODBC Drivers");
                        if (reg != null)
                        {
                            // Get all DSN entries defined in DSN_LOC_IN_REGISTRY.
                            foreach (string sName in reg.GetValueNames())
                            {
                                names.Add(sName);
                            }
                        }
                        try
                        {
                            reg.Close();
                        }
                        catch { /* ignore this exception if we couldn't close */ }
                    }
                }
            }

            return names;
        }

        private void ListODBCsources()
        {
            int envHandle = 0;
            const int SQL_FETCH_NEXT = 1;
            const int SQL_FETCH_FIRST_SYSTEM = 32;

            if (NativeMethods.SQLAllocEnv(ref envHandle) != -1)
            {
                int ret;
                StringBuilder serverName = new StringBuilder(1024);
                StringBuilder driverName = new StringBuilder(1024);
                int snLen = 0;
                int driverLen = 0;
                ret = NativeMethods.SQLDataSources(envHandle, SQL_FETCH_FIRST_SYSTEM, serverName, serverName.Capacity, ref snLen,
                            driverName, driverName.Capacity, ref driverLen);
                while (ret == 0)
                {
                    Console.WriteLine(formatODBCDSNForListCommand(serverName.ToString(), driverName.ToString()));
                    ret = NativeMethods.SQLDataSources(envHandle, SQL_FETCH_NEXT, serverName, serverName.Capacity, ref snLen,
                            driverName, driverName.Capacity, ref driverLen);
                }
            }

        }

        /// <summary>
        /// Removes a DSN entry
        /// </summary>
        /// <param name="dsnName">Name of the DSN to remove.</param>
        public static void RemoveDSN(string dsnName)
        {
            // Remove DSN key
            Registry.LocalMachine.DeleteSubKeyTree(Constants.ODBC_INI_REG_PATH + dsnName);

            // Remove DSN name from values list in ODBC Data Sources key
            var datasourcesKey = Registry.LocalMachine.CreateSubKey(Constants.ODBC_INI_REG_PATH + "ODBC Data Sources");
            if (datasourcesKey == null) throw new Exception("ODBC Registry key for datasources does not exist");
            datasourcesKey.DeleteValue(dsnName);
        }


        ///<summary>
        /// Returns an array of driver names installed on the system
        ///</summary>
        ///<returns></returns>
        public static string[] GetInstalledDrivers()
        {
            var driversKey = Registry.LocalMachine.CreateSubKey(Constants.ODBCINST_INI_REG_PATH + "ODBC Drivers");
            if (driversKey == null) throw new Exception("ODBC Registry key for drivers does not exist");

            var driverNames = driversKey.GetValueNames();

            var ret = new List<string>();

            foreach (var driverName in driverNames)
            {
                if (driverName != "(Default)")
                {
                    ret.Add(driverName);
                }
            }

            return ret.ToArray();
        }
    }
}
