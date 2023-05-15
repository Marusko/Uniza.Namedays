using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private NamedayCalendar _calendar;
        public MainWindow()
        {
            InitializeComponent();
            _calendar = new NamedayCalendar();
            DateLabel.Content = DateTime.Now.ToShortDateString() + " celebrates:";
            Calendar.SelectedDatesChanged += (sender, e) =>
            {
                if (Calendar.SelectedDate != null)
                {
                    DateLabel.Content = $"{Calendar.SelectedDate.Value:dd.MM.yyyy} celebrates:";
                    NamedayListbox.Items.Clear();
                    foreach (var date in _calendar[Calendar.SelectedDate.Value])
                    {
                        NamedayListbox.Items.Add(date);
                    }
                    Mouse.Capture(null);
                }
            };

        }

        private void MenuNew(object sender, EventArgs e)
        {
            if (_calendar.GetNamedays().ToList().Count > 0)
            {
                var dialog = MessageBox.Show("Do you realy want to clear the calendar?", "New", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (dialog == MessageBoxResult.Yes)
                {
                    _calendar.Clear();
                }
                NamedayListbox.Items.Clear();
                NamedayListbox.Items.Add("No data available");
            }
        }
        private void MenuOpen(object sender, EventArgs e)
        {
            var result = new OpenFileDialog();
            result.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            result.ShowDialog();
            _calendar.Load(new FileInfo(result.FileName));
            NamedayListbox.Items.Clear();
            foreach (var date in _calendar[DateTime.Now])
            {
                NamedayListbox.Items.Add(date);
            }
        }
        private void MenuSave(object sender, EventArgs e)
        {
            var save = new SaveFileDialog();
            save.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            save.ShowDialog();
            _calendar.Save(new FileInfo(save.FileName));
        }
        private void MenuExit(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private void MenuAbout(object sender, EventArgs e)
        {
            var about = new AboutWindow();
            about.ShowDialog();
        }

        private void SetToday(object sender, EventArgs e)
        {
            Calendar.DisplayDate = DateTime.Now;
            Calendar.SelectedDate = DateTime.Now;
            DateLabel.Content = Calendar.SelectedDate.Value.ToString("dd.MM.yyyy") + " celebrates:";
            NamedayListbox.Items.Clear();
            foreach (var date in _calendar[Calendar.SelectedDate.Value])
            {
                NamedayListbox.Items.Add(date);
            }
        }
    }
}
