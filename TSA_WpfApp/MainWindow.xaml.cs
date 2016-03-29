using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;

namespace TSA_WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string path2XL = @"C:\Users\Sakibul.Khan\Documents\SR\Rue 21\1-267334511 @ 2015-10-06\1-267334511 - Errors.xlsx";
        int noLines = -1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnRemoveErrors_Click(object sender, RoutedEventArgs e)
        {
            object misValue = System.Reflection.Missing.Value;

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWB = xlApp.Workbooks.Open(path2XL, ReadOnly: true);
            Excel.Worksheet xlWS = (Excel.Worksheet)xlWB.Worksheets.Item[0];//AC=2
            //Excel.Worksheet xlWS = (Excel.Worksheet)xlWB.Worksheets.Item[Int16.Parse(txtWsNo.Text)];//AC=2

            //MessageBox.Show(xlWS.Range["A1", "A1"].Value2.ToString());
            // *************************************************************
            string[] errorNos = ColumnData2Array(xlWS, 1);

            RemoveLinesContainingTheErrors(errorNos);

            /* //Working code:
            for (int i = 0; i < errorNos.Length; i++)
            {
                MessageBox.Show(errorNos[i]);
            }*/

            // *************************************************************
            xlWB.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWS); releaseObject(xlWB); releaseObject(xlApp);
        }

        private void RemoveLinesContainingTheErrors(string[] errorNos)
        {
            bool foundStr = false;
            string fileTimeStamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");

            // Launch Thread to update the txtLineNo every 5 seconds
            Thread counterThread = new Thread(new ThreadStart(ThreadMethod));
            counterThread.Start();

            try
            {
                if (File.Exists(txtFileName.Text))
                {
                    using (StreamWriter swGood = new StreamWriter(txtFileName.Text + "-gen-" + fileTimeStamp + ".GOOD"))
                    {
                        using (StreamWriter swBaad = new StreamWriter(txtFileName.Text + "-gen-" + fileTimeStamp + ".BAAD"))
                        {

                            String line;
                            // Create an instance of StreamReader to read from a file.
                            // The using statement also closes the StreamReader.
                            using (StreamReader sr = new StreamReader(txtFileName.Text)) // (@"c:\yourfile.txt")
                            {
                                noLines = 0;
                                // Read and display lines until the EOF is reached.
                                while ((line = sr.ReadLine()) != null)
                                {
                                    foundStr = false;
                                    for (int i = 0; i < errorNos.Length; i++)
                                    {
                                        if (line.Contains(errorNos[i])) foundStr = true;
                                    }

                                    if (foundStr)
                                        swBaad.WriteLine(line);
                                    else
                                        swGood.WriteLine(line);
                                    // **************************************************
                                    noLines++;

                                    /*
                                    int parsedIntValue;
                                    if (Int32.TryParse(txtLineNo.Text, out parsedIntValue))
                                        txtLineNo.Text = (parsedIntValue + 1).ToString();
                                    else
                                        txtLineNo.Text = "0";
                                    */
                                    // **************************************************
                                }
                            }//using sr
                        }//using swBaad
                    }//using swGood
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        private void ThreadMethod()
        {
            //while(true)
            //{
            //    txtLineNo.Text = noLines.ToString();
            //}
        }

        private string[] ColumnData2Array(Excel.Worksheet xlWS, int colNo = 1)
        {
            Excel.Range last = xlWS.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            Excel.Range range = xlWS.get_Range("A1", last); // "A1:A1048576"

            int lastUsedRow = last.Row;
            int lastUsedColumn = last.Column;
            //MessageBox.Show(lastUsedRow + ":" + lastUsedColumn);

            //***************SHURU: Return the values in an array
            Excel.Range firstColumn = xlWS.UsedRange.Columns[colNo]; // colNo = 1
            System.Array myvalues = (System.Array)firstColumn.Cells.Value;
            string[] strArray = myvalues.OfType<object>().Select(o => o.ToString()).ToArray();
            return strArray;
            //***************SHESH: Return the values in an array

            /* Working code:
            for (int r = 1; r <= lastUsedRow; r++)
            {
                MessageBox.Show(xlWS.Cells[r, 1].Value);
            }*/
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void btn3rdParty_Click(object sender, RoutedEventArgs e)
        {
            //Utilities.ReadFileLineByLine(txtFileName.Text, txt3rdParty);
            //Utilities.ReadFileLineByLineIntoList(txtFileName.Text);
            Utilities.tempFix_JFS_JoeFresh(txtFileName.Text);
        }

        private void btnReadXPOLLDFiles_Click(object sender, RoutedEventArgs e)
        {
            //txtFileName_MissingTxns
            //tblkFileName_MissingTxns

        }

        private void btnGrabAllTSXml_Click(object sender, RoutedEventArgs e)
        {
            txtFileName.Text
        }
    }
}
