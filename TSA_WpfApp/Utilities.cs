using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication_Aptos
{
    public class Utilities
    {

        public static void tempFix_JFS_JoeFresh(string filename) // incomplete !!!
        {
            int NoOfTimesReplaced = 0;
            int lineIdStart = 32710; // 2^15 = 32,768
            int lineIdStop = 61360;
            string lineIdStrOldTab = String.Empty;
            string lineIdStrNewTab = String.Empty;

            using (StreamReader SR = File.OpenText(filename))
            {
                string s = String.Empty;

                using (StreamWriter file = new StreamWriter(filename + ".new"))
                {
                    while ((s = SR.ReadLine()) != null)
                    {
                        for (int lineID = lineIdStart; lineID <= lineIdStop; lineID++)
                        {
                            //Page 35, Section 4.2.4.4, 4.2.4.4	Discount Detail record
                            lineIdStrOldTab = "\t" + lineID.ToString() + "\t";
                            lineIdStrNewTab = "\t" + (lineID / 10).ToString() + "\t";

                            if (s.Contains(lineIdStrOldTab))
                            {
                                s = s.Replace(lineIdStrOldTab, lineIdStrNewTab);
                                NoOfTimesReplaced++;
                            }
                        }//for

                        file.WriteLine(s);
                    }
                }
                MessageBox.Show("NoOfTimesReplaced = " + NoOfTimesReplaced);
            }
        }

        public static void ReadFileLineByLine(string filename, TextBox txtBox)
        {
            // http://stackoverflow.com/questions/2161895/reading-large-text-files-with-streams-in-c-sharp

            using (StreamReader streamReader = File.OpenText(filename))
            {
                string s = String.Empty;

                while ((s = streamReader.ReadLine()) != null)
                {
                    txtBox.AppendText(s + "\n");
                    txtBox.ScrollToEnd();
                }
            }
        }

        public static void ReadFileLineByLineIntoList_JFS_JoeFresh(string filename) // incomplete !!!
        {
            //List<int> list = new List<int>(); list.Add("Hello World !!!");

            //List<List<string>> lstFileContents = new List<List<string>>();
            //lstFileContents[lstFileContents.Count].Add(s);

            List<Txn> list = new List<Txn>();

            using (StreamReader SR = File.OpenText(filename))
            {
                string s = String.Empty;

                while ((s = SR.ReadLine()) != null)
                {
                    if(s[0] == 'H')
                    {
                        //new Txn();
                    }
                }
            }
        }

        public static void ReadLargeFileAsCharacterBlocks(string filename, TextBox txtBox)
        {
            // http://stackoverflow.com/questions/2161895/reading-large-text-files-with-streams-in-c-sharp

            int bufferSize = 16384; // 2^14 = 16384

            StringBuilder stringBuilder = new StringBuilder();
            FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);

            using (StreamReader streamReader = new StreamReader(fileStream))
            {
                char[] fileContents = new char[bufferSize];
                int charsRead = streamReader.Read(fileContents, 0, bufferSize);

                // Can't do much with 0 bytes
                if (charsRead == 0)
                    throw new Exception("File is 0 bytes");

                while (charsRead > 0)
                {
                    stringBuilder.Append(fileContents);
                    charsRead = streamReader.Read(fileContents, 0, bufferSize);
                    // **************************************************
                    txtBox.Text += fileContents.ToString();
                    MessageBox.Show(stringBuilder.ToString());
                }
            }
        }
    }
}
