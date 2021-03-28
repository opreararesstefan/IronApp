using IronApp.Helpers;
using System.Windows;

namespace IronApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Methods

        /// <summary>
        /// MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            GenerateZipBtn.IsEnabled = true;
            InsertDataBtn.IsEnabled = false;
        }

        /// <summary>
        /// GenerateZipBtn_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateZipBtn_Click(object sender, RoutedEventArgs e)
        {
            GenerateZipBtn.IsEnabled = false;
            Controller.GenerateZIP();
            GenerateZipBtn.IsEnabled = true;
            InsertDataBtn.IsEnabled = true;
        }

        /// <summary>
        /// InsertDataBtn_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertDataBtn_Click(object sender, RoutedEventArgs e)
        {
            GenerateZipBtn.IsEnabled = false;
            InsertDataBtn.IsEnabled = false;
            SecondApp.InitializeApp();
            GenerateZipBtn.IsEnabled = true;
        }

        #endregion
    }
}
