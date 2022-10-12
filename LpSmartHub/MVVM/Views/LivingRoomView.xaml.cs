using System.Windows;
using System.Windows.Controls;

namespace LpSmartHub.MVVM.Views
{
    /// <summary>
    /// Interaction logic for LivingRoomView.xaml
    /// </summary>
    public partial class LivingRoomView : UserControl
    {
        public LivingRoomView()
        {
            InitializeComponent();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
