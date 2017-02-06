using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine.Text;
using CommandLine;
using System.Reflection;
namespace odbcman
{
    class CommandOptions
    {
        [Option('x', "remove", Required = false, HelpText = "Removes a DSN by Name", MutuallyExclusiveSet = "RemoveDSNOperation")]
        public bool IsRemoveDSNOperation
        {
            get;
            set;
        }
        [Option('h', "help", Required = false, HelpText = "Print help", MutuallyExclusiveSet = MutuallyExclusiveOptions.Help)]
        public bool IsHelpOperation
        {
            get;
            set;
        }
        [Option('d', "ListDrivers", Required = false, HelpText = "Lists available ODBC Drivers", MutuallyExclusiveSet = MutuallyExclusiveOptions.ListDriver)]
        public bool IsListDriverOperation
        {
            get;
            set;
        }
        [Option('l', "list", Required = false, HelpText = "lists defined DSNs", MetaValue = "true", MutuallyExclusiveSet = MutuallyExclusiveOptions.ListDSN)]
        public bool IsListDSNOperation
        {
            get;
            set;
        }
        [Option('e', "export", Required = false, HelpText = "Specifies export operation", MutuallyExclusiveSet = MutuallyExclusiveOptions.Export)]
        public bool IsExportDSNoperation
        {
            get;
            set;
        }
        [Option('i', "import", Required = false, HelpText = "Specifies Import operation", MutuallyExclusiveSet = MutuallyExclusiveOptions.Export)]
        public bool IsImportDSNOperation
        {
            get;
            set;
        }
        [Option('f', "file", Required = false, HelpText = "Input file to read.")]
        public string File
        {
            get;
            set;
        }
        [Option('n', "name", Required = false, HelpText = "Name of DS to export,remove, create by import.")]
        public string DSNName
        {
            get;
            set;
        }
        [Option('r', "register", Required = false, HelpText = "Registers ODBCMan to .odbc files", MutuallyExclusiveSet = MutuallyExclusiveOptions.Register)]
        public bool IsRegisterOperation { get; internal set; }
        [HelpOption]
        public string GetUsage()
        {
            var usage = new StringBuilder();
            /* // this without using CommandLine.Text
            
             usage.AppendLine(string.Format("odbcman {0}", Assembly.GetExecutingAssembly().GetName().Version));
             usage.AppendLine(string.Format("odbcman {0}", Assembly.GetExecutingAssembly().GetName().Version));

             usage.AppendLine(formatHelpLine('d', "ListDrivers", "lists available ODBC Drivers"));
             usage.AppendLine(formatHelpLine('e', "export", "Specifies export operation"));
             usage.AppendLine(formatHelpLine('f', "file", "Input file to read",true));
             usage.AppendLine(formatHelpLine('i', "import", "Specifies Import operation"));
             usage.AppendLine(formatHelpLine('l', "list", "lists defined DSNs"));
             usage.AppendLine(formatHelpLine('n', "name", "Input file to read"));
             usage.AppendLine(formatHelpLine('r', "register", "Registers ODBCMan to .odbc files"));
             usage.AppendLine(formatHelpLine('x', "remove", "Name of DS to export,remove, create by import."));
             usage.AppendLine(formatHelpLine('h', "help", "Print this help"));
             */
            usage.AppendLine(" odbcman");
            usage.AppendLine("Windows utility which imports and exports Datasources defined by odbcad32.exe");
            usage.AppendLine("");
            usage.AppendLine(" Usage");
            usage.AppendLine(" Register odbcman to extension of export Files");
            usage.AppendLine("   odbcman --register ");
            usage.AppendLine("   odbcman --r ");
            usage.AppendLine("");
            usage.AppendLine(" Exporting a defined DSN to file ");
            usage.AppendLine("   odbcman --export --name [DSN Name] --file [ExportFileName]  ");
            usage.AppendLine("   odbcman -e -n [DSN Name] -f [ExportFileName] ");
            usage.AppendLine("");
            usage.AppendLine(" Importing DSN from file");
            usage.AppendLine("   odbcman --import  --file [ExportFileName] ");
            usage.AppendLine("   odbcman -i  -f [ExportFileName] ");
            usage.AppendLine("");
            usage.AppendLine(" Listing available Drivers");
            usage.AppendLine("   odbcman --ListDrivers ");
            usage.AppendLine("   odbcman -d ");
            usage.AppendLine("");
            usage.AppendLine(" Listing defined DSNs");
            usage.AppendLine("   odbcman --list ");
            usage.AppendLine("   odbcman -l ");
            usage.AppendLine("");
            usage.AppendLine(" Remove DSN");
            usage.AppendLine("   odbcman --remove --name [DSN Name] ");
            usage.AppendLine("   odbcman -x -n [DSN Name] ");
            usage.AppendLine("");
            usage.AppendLine("");
            usage.AppendLine(" Usage Help");
            usage.AppendLine("   odbcman --help ");
            usage.AppendLine("   odbcman -h ");

            return usage.ToString();
        }
        string formatHelpLine(char shortOption, string longOption, string helpText, bool isSuboption=false)
        {
            if (!isSuboption)
                return string.Format("-{0,-5}\t--{1,-10}\t\t\t{2}", shortOption, longOption, helpText);
            else
                return string.Format("\t-{0,-5}\t--{1,-10}\t\t\t{2}", shortOption, longOption, helpText);

        }
 
        public static CommandOptions Parse(ParserSettings settings, string[] args)
        {
            ParserSettings parsersettings = new ParserSettings();
            parsersettings.CaseSensitive = false;
            CommandOptions options = new CommandOptions();
            Parser parser = new Parser(parsersettings);
            parser.ParseArguments(args, options);
            return options;
        }
    }
}