using Sortieranlage_View_WPF.Model;
using Sortieranlage_View_WPF.View;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sortieranlage_View_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new StartseiteView();
        }

        private void MenueButtonStartseite_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new StartseiteView();
        }

        private void MenueButtonHandbetrieb_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new HandbetriebView();

        }

        private void MenueButtonPlcInfo_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new PlcInfoView();

        }

        private void MenueButtonDisconnect_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new DisconnectView();

        }
    }
}