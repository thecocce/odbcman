using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using odbcman;
namespace odbcmantests
{
    [TestClass]
    public class ListOperationTests
    {
       
        [TestMethod]
        public void ShortUnixArgument()
        {
            string[] args = new string[] {
                "-l"
            };
            Program.Main(args);
        }

        [TestMethod]
        public void ShortListArgument()
        {
            string[] args = new string[] {
                "/l"
            };
            Program.Main(args);
        }

        [TestMethod]
        public void LongArgument()
        {
            string[] args = new string[] {
                "--list"
            };
            Program.Main(args);
        }

    }
}
