using System;
using System.Windows;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        public void CloseWindow(object sender, EventArgs e)
        {
            Close();
        }
    }
}
