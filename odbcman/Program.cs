using CommandLine;
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
                        if (options.IsListDriverOperation)
                            ListODBCDrivers();
                        else if (options.IsListDSNOperation)
                            ListODBCsources();

                        else
                            options.GetUsage();

                    }
                }


            }
        }

        private string formatODBCDSNForListCommand(string serverName, string driverName)
        {
            return string.Format("{0,-32}\t\t{1,-50}", serverName, driverName);
        }

        private void ListODBCDrivers()
        {
            List<String> drivers = GetSystemDriverList();
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
    }
}
