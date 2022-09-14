using System.Windows;
using System.Windows.Input;

namespace Device.IntelliTemp
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
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }



        private void CloseBtnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
        }

        private void ResizeBtnClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                btnResize.Content = FindResource("Restore");
            }
            else
            {
                this.WindowState = WindowState.Normal;
                btnResize.Content = FindResource("Maximize");
            }
        }

        private void MinimizeBtnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
