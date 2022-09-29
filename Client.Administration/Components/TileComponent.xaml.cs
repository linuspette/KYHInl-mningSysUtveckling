using System.Windows;
using System.Windows.Controls;

namespace Client.Administration.Components
{
    /// <summary>
    /// Interaction logic for Tile.xaml
    /// </summary>
    public partial class TileComponent : UserControl
    {
        public TileComponent()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TileComponent));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(TileComponent));
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly DependencyProperty IconActiveProperty =
            DependencyProperty.Register("IconActive", typeof(string), typeof(TileComponent));
        public string IconActive
        {
            get { return (string)GetValue(IconActiveProperty); }
            set { SetValue(IconActiveProperty, value); }
        }

        public static readonly DependencyProperty IconInActiveProperty =
            DependencyProperty.Register("IconInActive", typeof(string), typeof(TileComponent));
        public string IconInActive
        {
            get { return (string)GetValue(IconInActiveProperty); }
            set { SetValue(IconInActiveProperty, value); }
        }
    }
}
