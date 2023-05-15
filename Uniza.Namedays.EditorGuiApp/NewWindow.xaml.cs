using System;
using System.Windows;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for NewWindow.xaml
    /// </summary>
    public partial class NewWindow : Window
    {
        public DateTime? NamedayDate { get; set; }
        public string? Nameday { get; set; }
        public NewWindow()
        {
            InitializeComponent();
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            Close();
        }

        private void OkCloseWindow(object sender, EventArgs e)
        {
            if (NewDatePicker.SelectedDate != null)
            {
                NamedayDate = NewDatePicker.SelectedDate.Value.Date;
            }
            Nameday = NewTextBox.Text;
            Close();
        }
    }
}
