using System;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow
    {
        public EditWindow()
        {
            InitializeComponent();
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            Close();
        }
    }
}
