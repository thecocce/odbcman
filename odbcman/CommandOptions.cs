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
        [Option('l', "list", Required = false, HelpText = "lists defined DSNs", MetaValue ="true")]
        public bool IsListOperation
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

        [Option('x', "remove", Required = false, HelpText = "Removes a DSN by Name")]
        public bool IsRemoveDSNOperation
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
            usage.AppendLine("Read user manual for usage instructions...");
            return usage.ToString();
        }
    }
}
