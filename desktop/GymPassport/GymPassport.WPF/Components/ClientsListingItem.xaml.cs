using System.Windows;
using System.Windows.Controls;

namespace GymPassport.WPF.Components
{
    /// <summary>
    /// Lógica de interacción para ClientsListingItem.xaml
    /// </summary>
    public partial class ClientsListingItem : UserControl
    {
        public ClientsListingItem()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dropdown.IsOpen = false;
        }
    }
}
