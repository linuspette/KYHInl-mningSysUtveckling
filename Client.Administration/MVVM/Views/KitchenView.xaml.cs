using System.Windows;
using System.Windows.Controls;

namespace Client.Administration.MVVM.Views
{
    /// <summary>
    /// Interaction logic for KitchenView.xaml
    /// </summary>
    public partial class KitchenView : UserControl
    {
        public KitchenView()
        {
            InitializeComponent();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
