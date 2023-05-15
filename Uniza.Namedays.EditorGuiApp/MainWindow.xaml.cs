using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace Uniza.Namedays.EditorGuiApp
{
    //TODO regex
    //TODO listitems
    //TODO enable/disable
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
            Calendar.SelectedDatesChanged += DatesChanged;
            MonthComboBox.SelectionChanged += FilterChanged;
            RegexTextBox.PreviewKeyUp += FilterChanged;
            //RegexTextBox.TextChanged += FilterChanged;
            foreach (var month in DateTimeFormatInfo.CurrentInfo.MonthNames)
            {
                MonthComboBox.Items.Add(month);
            }
            MonthComboBox.SelectedIndex = 12;
            CountLabel.Content = $"Count: {FilterNamedaysListBox.Items.Count} / {_calendar.NameCount}";

            FilterNamedaysListBox.SelectionChanged += (sender, e) =>
            {
                EditButton.IsEnabled = true;
                RemoveButton.IsEnabled = true;
                ShowOnButton.IsEnabled = true;
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
                CountLabel.Content = $"Count: {FilterNamedaysListBox.Items.Count} / {_calendar.NameCount}";
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
            CountLabel.Content = $"Count: {FilterNamedaysListBox.Items.Count} / {_calendar.NameCount}";
        }
        private void MenuSave(object sender, EventArgs e)
        {
            var save = new SaveFileDialog();
            save.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            save.ShowDialog();
            if (!string.IsNullOrEmpty(save.FileName))
            {
                _calendar.Save(new FileInfo(save.FileName));
            }
            
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
        private void DatesChanged(object? sender, EventArgs e)
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
        }

        private void ClearFilter(object sender, EventArgs e)
        {
            RegexTextBox.Text = "";
            MonthComboBox.SelectedIndex = 12;
            FilterNamedaysListBox.Items.Clear();
            CountLabel.Content = $"Count: {FilterNamedaysListBox.Items.Count} / {_calendar.NameCount}";
        }
        private void FilterChanged(object? sender, EventArgs e)
        {
            IEnumerable<Nameday>? menaRegex = null;
            if (sender.GetType() == typeof(TextBox) && ((KeyEventArgs)e).Key == Key.Enter)
            {
                menaRegex = _calendar.GetNamedays(RegexTextBox.Text);
            }
            else
            {
                menaRegex = _calendar.GetNamedays();
            }

            /*try
            {
                menaRegex = _calendar.GetNamedays(RegexTextBox.Text);
            }
            catch (Exception exception)
            {
            }*/

            IEnumerable<Nameday> menaMesiac;
            if (MonthComboBox.SelectedIndex == 12)
            {
                menaMesiac = _calendar.GetNamedays();
            }
            else
            {
                menaMesiac = _calendar.GetNamedays(MonthComboBox.SelectedIndex + 1);
            }
            var spojene = menaMesiac.Intersect(menaRegex);
            FilterNamedaysListBox.Items.Clear();
            foreach (var mena in spojene)
            {
                FilterNamedaysListBox.Items.Add(mena);
            }

            CountLabel.Content = $"Count: {FilterNamedaysListBox.Items.Count} / {_calendar.NameCount}";
        }

        private void AddNew(object sender, EventArgs e)
        {
            var newNameday = new NewWindow();
            newNameday.ShowDialog();
            if (newNameday.NamedayDate != null && newNameday.Nameday != null)
            {
                _calendar.Add(newNameday.NamedayDate.Value.Day, 
                newNameday.NamedayDate.Value.Month, newNameday.Nameday);
            }
            CountLabel.Content = $"Count: {FilterNamedaysListBox.Items.Count} / {_calendar.NameCount}";
        }
        private void Edit(object sender, EventArgs e)
        {
            var selectedItem = (Nameday)FilterNamedaysListBox.SelectedItem;
            var editNameday = new EditWindow();
            editNameday.EditDatePicker.SelectedDate = selectedItem.DayMonth.ToDateTime();
            editNameday.EditTextBox.Text = selectedItem.Name;
            editNameday.ShowDialog();
            if (editNameday.NamedayDate != null && editNameday.Nameday != null)
            {
                _calendar.Remove(selectedItem.Name);
                _calendar.Add(new DayMonth(editNameday.NamedayDate.Value.Day, editNameday.NamedayDate.Value.Month),
                    editNameday.Nameday);
            }
            FilterChanged(sender, e);
        }
        private void Remove(object sender, EventArgs e)
        {
            var selectedItem = (Nameday)FilterNamedaysListBox.SelectedItem;
            var remove = MessageBox.Show($"Do you really want to remove selected nameday({selectedItem.Name})?",
                "Remove nameday", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (remove == MessageBoxResult.Yes)
            {
                _calendar.Remove(selectedItem.Name);
            }
            FilterChanged(sender, e);
        }

        private void ShowOnCalendar(object sender, EventArgs e)
        {
            var selectedItem = (Nameday)FilterNamedaysListBox.SelectedItem;
            Calendar.SelectedDate = selectedItem.DayMonth.ToDateTime();
            Calendar.DisplayDate = selectedItem.DayMonth.ToDateTime();
        }
    }
}
