using System.Windows;
using System.Windows.Input;

namespace Client.Administration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void TitleBar_LeftButtonMouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
