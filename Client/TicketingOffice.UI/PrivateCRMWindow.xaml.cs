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
using TicketingOffice.CrmService.Contracts;
using System.ServiceModel;
using TicketingOffice.TicketingService.Contracts;
using System.ComponentModel;
using System.Configuration;

namespace TicketingOffice.UI
{
    /// <summary>
    /// Interaction logic for PrivateCRMWindow.xaml
    /// </summary>
    public partial class PrivateCRMWindow : Window, INotifyPropertyChanged
    {
        private IPrivateCrm _proxy;
        private TicketingOffice.CrmService.Contracts.Customer _customer;

        public TicketingOffice.CrmService.Contracts.Customer SelectedCustomer
        {
            get
            {
                return _customer;
            }
            set
            {
                _customer = value;
                RaisePropertyChanged("SelectedCustomer");
            }
        }

        public PrivateCRMWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void btnFindCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedCustomer = _proxy.GetMyDetails(txtEmail.Text, txtCellNumber.Text);
            }
            catch (FaultException<ExceptionDetail> fe)
            {
                MessageBox.Show(fe.Detail.Message);
                SelectedCustomer = null;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeProxy();
        }

        private void InitializeProxy()
        {
            ChannelFactory<IPrivateCrm> factory = new ChannelFactory<IPrivateCrm>("PrivateCRM");
            _proxy = factory.CreateChannel();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private void btnSaveCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCustomer != null)
            {
                _proxy.ChangeMyDetails(txtEmail.Text, txtCellNumber.Text, SelectedCustomer);
            }
        }
    }
}
