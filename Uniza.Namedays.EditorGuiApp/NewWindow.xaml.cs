using System;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for NewWindow.xaml
    /// </summary>
    public partial class NewWindow
    {
        public NewWindow()
        {
            InitializeComponent();
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            Close();
        }
    }
}
