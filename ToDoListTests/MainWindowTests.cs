using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Tests
{
    [TestClass()]
    public class MainWindowTests
    {
        [TestMethod()]
        public void FileExistenceTest()
        {
            MainWindow t = new MainWindow();
            try
            {
                t.FileWrite("test", "10.10.2017");
            }
            catch(Exception e)
            {
                StringAssert.Contains(e.Message, "file does not exist");
                Assert.Fail("file does not exist");
            }
        }
    }
}