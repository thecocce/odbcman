using CommandLine;
using System;
using System.Text;

namespace odbcman
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Program intance = new Program();
            intance.HandleOptions(args);
            Console.ReadKey();
        }



        public void HandleOptions(string[] args)
        {
            CommandOptions options = new CommandOptions();

            Parser parser = new Parser();
            parser.ParseArguments(args, options);

            if (options == null)
            {
                options.GetUsage();
            }
            else
            {
                if (options.IsImportDSNOperation)
                    Console.WriteLine("Is ImportDSN Operation");
                else if (options.IsExportDSNOption)
                    Console.WriteLine("Is ExportDSN Operation");
                else if (options.IsListOperation)
                    ListODBCsources();
                else if (options.IsRemoveDSNOperation)
                    Console.WriteLine("Is RemoveDSN Operation");
                else
                    options.GetUsage();
            }
        }

        private string formatODBCDSNForListCommand(string serverName, string driverName)
        {
            return string.Format("{0,32}\t\t{1}", serverName, driverName);
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
