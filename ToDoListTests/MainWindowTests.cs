using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ToDoList.Tests
{
    [TestClass()]
    public class MainWindowTests
    {
        MainWindow t = new MainWindow();
        static Random rn = new Random();
        static int RandomNumber = rn.Next(0, 5000);

        string date = "10.10.2017 0:00:00";

        [TestMethod()]
        public void FileExistenceTest()
        {
            try
            {
                t.FileWrite(RandomNumber.ToString(), "10.10.2017");
            }
            catch (Exception e)
            {
                StringAssert.Contains(e.Message, "file does not exist");
                Assert.Fail("file does not exist");
            }
        }

        [TestMethod()]
        public void StringToDateTest()
        {
            List<Record> tr = new List<Record>();
            t.ReedOfFileInArray(tr);
            string recieved = tr[t.coutRecords - 1].text + "/" + tr[t.coutRecords - 1].date;
            /*MessageBox.Show(date);
            MessageBox.Show(t.StringToDate(recieved).ToString());*/
            if (t.StringToDate(recieved).ToString() != date) Assert.Fail("Error: returned date is wrong");

        }

        [TestMethod()]
        public void ReedOfFileInArrayTest()
        {
            List<Record> tr = new List<Record>();
            t.ReedOfFileInArray(tr);
           /* MessageBox.Show(RandomNumber.ToString());
            MessageBox.Show(tr[t.coutRecords - 1].text.ToString());
            MessageBox.Show(date);
            MessageBox.Show(tr[t.coutRecords - 1].date.ToString());*/
            if (tr[t.coutRecords - 1].text.ToString() != RandomNumber.ToString()) Assert.Fail("Error reading from file: wrong text");
            if (tr[t.coutRecords - 1].date.ToString() != date) Assert.Fail("Error reading from file: wrong date. Expected: " + date + " but recieved: " + tr[t.coutRecords - 1].date.ToString());
        }
    }
}