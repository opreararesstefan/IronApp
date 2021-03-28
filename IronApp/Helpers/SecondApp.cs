using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Windows;

namespace IronApp.Helpers
{
    public static class SecondApp
    {
        #region Properties

        /// <summary>
        /// ZipFilePath
        /// </summary>
        private static string ZipFilePath { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// DeleteFiles
        /// </summary>
        /// <returns></returns>
        private static MessageBoxResult DeleteFiles()
        {
            foreach (var item in Directory.GetFiles(@"C:\temp\"))
            {
                FileInfo file = new FileInfo(item);
                if (!file.Extension.Contains("zip"))
                    File.Delete(item);
                else
                    ZipFilePath = item;
            }
            foreach(var dir in Directory.GetDirectories(@"C:\temp\"))
            {
                Directory.Delete(dir, true);
            }
            return MessageBox.Show("All Deployment Files were Succesful deleted! Start Extracting!!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// UnzipDatei
        /// </summary>
        /// <returns></returns>
        private static bool UnzipDatei()
        {
            ZipFile.ExtractToDirectory(ZipFilePath, @"c:\temp\");
            if (Directory.GetFiles(@"C:\temp\").Length > 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// ReadTextFile
        /// </summary>
        /// <returns></returns>
        private static string[] ReadTextFile()
        {
            List<string> txtContainer = new List<string>();
            using (StreamReader sr = new StreamReader(Controller.TestFile))
            {
                while (sr.Peek() >= 0)
                {
                    var mess = sr.ReadLine();
                    if (!string.IsNullOrEmpty(mess) && !mess.StartsWith(" "))
                    txtContainer.Add(mess);
                }
            }
            return txtContainer.ToArray();
        }

        /// <summary>
        /// CreateNewEntry
        /// </summary>
        /// <param name="mess"></param>
        private static void CreateNewEntry(string mess)
        {
            string Connection = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\IronDB.mdf;Integrated Security = True; Connect Timeout = 30";
            SqlConnection connection = new SqlConnection(Connection);
            connection.Open();
            if (connection.State == System.Data.ConnectionState.Open)
                Console.WriteLine("DB connected succesfully!!");
            SqlCommand sqlcmd = new SqlCommand("ImgsAdd", connection);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@ImgRoute", mess);
            sqlcmd.ExecuteNonQuery();
            MessageBox.Show("Add " + mess + " to Datenbank!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            connection.Close();
            connection.Dispose();
        }

        /// <summary>
        /// InitializeApp
        /// </summary>
        public static void InitializeApp()
        {
            var del = DeleteFiles();
            if (del == MessageBoxResult.OK && UnzipDatei())
            {
                foreach(var item in ReadTextFile())
                    CreateNewEntry(item);
            }
            else
                MessageBox.Show("Fail", "Error on Extracting!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion
    }
}
