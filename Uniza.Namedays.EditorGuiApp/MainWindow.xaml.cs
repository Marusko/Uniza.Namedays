using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NamedayCalendar _calendar;
        public MainWindow()
        {
            InitializeComponent();
            _calendar = new NamedayCalendar();
        }

        public void MenuNew(object sender, EventArgs e)
        {
            if (_calendar.GetNamedays().ToList().Count > 0)
            {
                var dialog = MessageBox.Show("Do you realy want to clear the calendar?", "New", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (dialog == MessageBoxResult.Yes)
                {
                    _calendar.Clear();
                }
            }
        }
        public void MenuOpen(object sender, EventArgs e)
        {
            var result = new OpenFileDialog();
            result.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            result.ShowDialog();
            _calendar.Load(new FileInfo(result.FileName));
        }
        public void MenuSave(object sender, EventArgs e)
        {
            var save = new SaveFileDialog();
            save.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            save.ShowDialog();
            _calendar.Save(new FileInfo(save.FileName));
        }
        public void MenuExit(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        public void MenuAbout(object sender, EventArgs e)
        {
            var about = new AboutWindow();
            about.ShowDialog();
        }
    }
}
