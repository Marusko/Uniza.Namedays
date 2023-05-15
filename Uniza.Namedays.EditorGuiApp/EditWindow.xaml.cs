using System;
using System.Windows;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public DateTime? NamedayDate { get; set; }
        public string? Nameday { get; set; }
        public EditWindow()
        {
            InitializeComponent();
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            Close();
        }

        private void OkCloseWindow(object sender, EventArgs e)
        {
            if (EditDatePicker.SelectedDate != null)
            {
                NamedayDate = EditDatePicker.SelectedDate.Value.Date;
            }
            Nameday = EditTextBox.Text;
            Close();
        }
    }
}
