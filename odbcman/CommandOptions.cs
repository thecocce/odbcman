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
        [Option('l', "list", Required = false, HelpText = "lists defined DSNs", MetaValue = "true", MutuallyExclusiveSet = "ListDSNOperation")]
        public bool IsListDSNOperation
        {
            get;
            set;
        }

        [Option('e', "export", Required = false, HelpText = "Specifies export operation")]
        public bool IsExportDSNOption
        {
            get;
            set;
        }

        [Option('i', "import", Required = false, HelpText = "Specifies Import operation")]
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

        [Option('x', "remove", Required = false, HelpText = "Removes a DSN by Name", MutuallyExclusiveSet = "RemoveDSNOperation")]
        public bool IsRemoveDSNOperation
        {
            get;
            set;

        }

        [Option('h', "help", Required = false, HelpText = "Print help", MutuallyExclusiveSet = "HelpOperation")]
        public bool IsHelpOperation
        {
            get;
            set;

        }


        [Option('d', "ListDrivers", Required = false, HelpText = "Lists available ODBC Drivers", MutuallyExclusiveSet = "ListDriverOperation")]
        public bool IsListDriverOperation
        {
            get;
            set;

        }

        [HelpOption]
        public string GetUsage()
        {
            // this without using CommandLine.Text
            var usage = new StringBuilder();
            usage.AppendLine(string.Format("odbcman {0}", Assembly.GetExecutingAssembly().GetName().Version));
            usage.AppendLine(formatHelpLine('l', "list", "lists defined DSNs"));
            usage.AppendLine(formatHelpLine('d', "ListDrivers", "lists available ODBC Drivers"));

            usage.AppendLine(formatHelpLine('e', "export", "Specifies export operation"));
            usage.AppendLine(formatHelpLine('i', "import", "Specifies Import operation"));
            usage.AppendLine(formatHelpLine('f', "file", "Input file to read"));
            usage.AppendLine(formatHelpLine('n', "name", "Input file to read"));
            usage.AppendLine(formatHelpLine('x', "remove", "Name of DS to export,remove, create by import."));
            usage.AppendLine(formatHelpLine('?', "help", "Print this help"));

            return usage.ToString();

        }

        string formatHelpLine(char shortOption, string longOption, string helpText)
        {
            return string.Format("-{0,-5}\t--{1,-10}\t\t\t{2}", shortOption, longOption, helpText);
        }

        public static CommandOptions Parse(ParserSettings settings, string[] args)
        {
            CommandOptions options = new CommandOptions();

            Parser parser = new Parser(settings);
            parser.ParseArguments(args, options);

            return options;
        }
    }
}

