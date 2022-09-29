using System.Windows;
using System.Windows.Controls;

namespace Client.Administration.MVVM.Views
{
    /// <summary>
    /// Interaction logic for BedRoomView.xaml
    /// </summary>
    public partial class BedRoomView : UserControl
    {
        public BedRoomView()
        {
            InitializeComponent();
        }
        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
