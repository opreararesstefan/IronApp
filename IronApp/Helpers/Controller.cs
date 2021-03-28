using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Diagnostics;

namespace IronApp.Helpers
{
    public static class Controller
    {
        #region Properties

        private static int ConsecutiveDay { get; set; } = 1;
        private static string ActualMess { get; set; }
        private static string FolderName { get; set; }
        private static string[] FileContainer { get; set; }
        public const string TestFile = @"C:\temp\TestFile.meta";

        #endregion

        #region Methods

        /// <summary>
        /// CreateFolder
        /// </summary>
        private static void CreateFolder()
        {
            CreateFiles();
            FolderName = @"C:\temp\" + DateTime.Now.DayOfWeek.ToString();
            if (Directory.Exists(@"C:\temp\"))
                Directory.Delete(@"C:\temp\", true);
            Directory.CreateDirectory(FolderName);
        }

        /// <summary>
        /// InitializeFiles
        /// </summary>
        private static void InitializeFiles()
        {
            var curent = Environment.CurrentDirectory + @"\Helpers\";
            var container = Directory.GetFiles(curent);
            FileContainer = new string [container.Length];
            int i = 0;
            foreach (var item in container)
            {
                FileInfo file = new FileInfo(item);
                var dest = Path.Combine(FolderName, file.Name);
                if (file.Extension.Contains(("png").ToUpper(new CultureInfo("en-US", false))))
                {
                    File.Copy(item, dest);
                    FileContainer[i++] = dest;
                }
            }
            WriteInfo(@"   ID    |    Creation date    | Image route");
            WriteInfo(" ");
        }

        /// <summary>
        /// CreateFiles
        /// </summary>
        private static void CreateFiles()
        {
            var fi = new FileInfo(TestFile);
            if (fi.Exists)
            {
                fi.Delete();
                fi.Refresh();
                while (fi.Exists)
                {
                    System.Threading.Thread.Sleep(200);
                    fi.Refresh();
                }
            }
        }

        /// <summary>
        /// WriteInfo
        /// </summary>
        /// <param name="strMessage">strMessage</param>
        private static void WriteInfo(string strMessage)
        {
            using (StreamWriter w = File.AppendText(TestFile))
            {
                WriteToFile(strMessage, w);
            }
        }

        /// <summary>
        /// WriteToFile
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="w"></param>
        private static void WriteToFile(string strMessage, TextWriter w)
        {
            w.WriteLine($"{strMessage}");
        }

        /// <summary>
        /// GenerateMess
        /// </summary>
        /// <param name="img"></param>
        private static void GenerateMess(string img)
        {
            var dateToConvert = DateTime.Today;
            ActualMess = string.Format("{0}{1}", dateToConvert.ToString("yy"), dateToConvert.DayOfYear);
            ActualMess += ConsecutiveDay.ToString().PadLeft(4, '0') + @" | ";
            ConsecutiveDay++;
            ActualMess += File.GetCreationTime(img).ToString() + @" | ";
            ActualMess += img;
            WriteInfo(ActualMess);
        }

        /// <summary>
        /// GenerateZIP
        /// </summary>
        /// <returns></returns>
        public static bool GenerateZIP()
        {
            CreateFolder();
            InitializeFiles();
            foreach (var item in FileContainer)
                GenerateMess(item);
            string zipname = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".zip";
            string startPath = @"C:\temp";
            ZipFile.CreateFromDirectory(startPath, zipname, CompressionLevel.Optimal, false);
            System.Threading.Thread.Sleep(1000);
            var curent = Environment.CurrentDirectory;
            File.Copy(Path.Combine(curent, zipname), Path.Combine(startPath, zipname));
            if (File.Exists(Path.Combine(startPath, zipname)))
            {
                MessageBox.Show("Succes", "Zip Succesfull created!", MessageBoxButton.OK, MessageBoxImage.Information);
                Process.Start(@"c:\temp");
                return true;
            }
            else
            {
                MessageBox.Show("Fail", "Zip wasn't created! Please Try again later!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        #endregion
    }
}
