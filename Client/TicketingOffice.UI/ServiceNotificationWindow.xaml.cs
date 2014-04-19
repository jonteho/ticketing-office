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
using System.Windows.Shapes;
using System.ServiceModel;
using System.Net;
using TicketingOffice.ClientNotification.Contract;
using System.ServiceModel.Description;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.ObjectModel;
using System.Configuration;

namespace TicketingOffice.UI
{
    /// <summary>
    /// Interaction logic for ServiceNotificationWindow.xaml
    /// </summary>
    // TODO: Ex5 - Examine the instancing mode of the Client Notification service
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class ServiceNotificationWindow : Window, IClientNotification
    {
        private ObservableCollection<ClientNotification.Contract.TicketingMessage> _messages = 
            new ObservableCollection<ClientNotification.Contract.TicketingMessage>();
        private ServiceHost _host;

        public ObservableCollection<ClientNotification.Contract.TicketingMessage> Notifications
        {
            get
            {
                return _messages;
            }
        }

        public ServiceNotificationWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void btnListen_Click(object sender, RoutedEventArgs e)
        {
            _host = new ServiceHost(this);            
            _host.Open();

            string hostAddress = _host.Description.Endpoints[0].Address.ToString();

            ChannelFactory<INotificationManager> factory =
                new ChannelFactory<INotificationManager>("CrmNotificationEP");                    

            INotificationManager proxy;

            proxy = factory.CreateChannel();
            proxy.Register(hostAddress, NotificationTypes.CustomerNoification);

            btnListen.IsEnabled = false;
        }      

        #region IClientNotification Members

        // TODO: Ex3 - Implement IClientNotification MessageArrived method
        public void MessageArrived(TicketingMessage message)
        {
            _messages.Add(message);            
        }

        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_host != null && _host.State == CommunicationState.Opened)            
                _host.Close();                  
        }        
    }
}
