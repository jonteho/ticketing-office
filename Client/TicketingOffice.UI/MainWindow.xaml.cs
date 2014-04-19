using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TicketingOffice.UI
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

        private void btnListen_Click(object sender, RoutedEventArgs e)
        {
            ServiceNotificationWindow window = new ServiceNotificationWindow();
            window.Show();
        }

        private void btnPrivateCRM_Click(object sender, RoutedEventArgs e)
        {
            PrivateCRMWindow window = new PrivateCRMWindow();
            window.Show();
        }

        private void btnShowOrderTicket_Click(object sender, RoutedEventArgs e)
        {
            OrderTicketWindow window = new OrderTicketWindow();
            window.Show();
        }
    }
}
