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
using TicketingOffice.ShowsService.Contracts;
using System.ServiceModel;
using System.Configuration;
using TicketingOffice.UI.Templates;
using System.Collections.ObjectModel;
using TicketingOffice.TicketingService.Contracts;
using System.Transactions;
using TicketingOffice.HallState.Contracts;
using System.ComponentModel;

namespace TicketingOffice.UI
{
    /// <summary>
    /// Interaction logic for OrderTicketWindow.xaml
    /// </summary>
    public partial class OrderTicketWindow : Window
    {
        #region Proxies
        private IShowsService _showsProxy;

        private ITicketingService _ticketingProxy;
        #endregion
        private const int ROWS_IN_HALL = 12;
        private const int SEATS_IN_ROW = 7;
        private ObservableCollection<SeatIndex> _seats = new ObservableCollection<SeatIndex>();

        private enum AsyncType
        {
            NoAsync,
            ClientAsync,
            ServiceAsync
        }

        public IEnumerable<CrmService.Contracts.Customer> Customers
        {
            get
            {
                return Repositories.RepositoryFactory.Default.CreateCustomerRepository().GetCustomers();
            }
        }

        private CrmService.Contracts.Customer SelectedCustomer
        {
            get
            {
                return cmbCustomers.SelectedItem as CrmService.Contracts.Customer;
            }
        }

        private bool IsBridgeEnabled
        {
            get
            {
                return chkUseTicketingBridge.IsChecked.Value;
            }
        }

        private Event SelectedEvent
        {
            get
            {
                return lstEvents.SelectedItem as Event;
            }
        }

        public IEnumerable<Show> Shows
        {
            get
            {
                return _showsProxy.FindShowsByCriteria(new ShowCriteria());
            }
        }

        public ObservableCollection<SeatIndex> WantedSeats
        {
            get
            {
                return _seats;
            }
        }

        public OrderTicketWindow()
        {
            InitializeProxy();
            InitializeComponent();
        }

        private void InitializeProxy()
        {
            _showsProxy = new ChannelFactory<IShowsService>("ShowEP").CreateChannel();

            InitializeTicketingProxy(false);
        }

        private void lstShows_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (Resources["cvs"] as CollectionViewSource).Source = (lstShows.SelectedValue as Show).Events;
        }

        private void ShowAvailableSeats(SeatIndex[] reservedSeats)
        {
            seatsLayout.Children.Clear();
            seatsLayout.RowDefinitions.Clear();
            seatsLayout.ColumnDefinitions.Clear();
            _seats.Clear();
            if (reservedSeats != null)
            {
                for (int i = 0; i < ROWS_IN_HALL; i++)
                {
                    seatsLayout.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    for (int j = 0; j < SEATS_IN_ROW; j++)
                    {
                        seatsLayout.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                        SeatMarker seatMarker = new SeatMarker();
                        if (reservedSeats.FirstOrDefault(s => s.Row == i + 1 && s.Seat == j + 1) != null)
                        {
                            seatMarker.Availability = SeatAvailability.Reserved;
                        }
                        else
                        {
                            seatMarker.Availability = SeatAvailability.Available;
                        }
                        seatsLayout.Children.Add(seatMarker);
                        seatMarker.ReservationChanged += new SeatMarker.ReservationChangedEventHandler(seatMarker_ReservationChanged);

                        Grid.SetRow(seatMarker, i);
                        Grid.SetColumn(seatMarker, j);
                    }
                }
            }
        }

        void seatMarker_ReservationChanged(object sender, ReservationChangedEventArgs e)
        {
            short row = (short)(Grid.GetRow(sender as UIElement) + 1);
            short seat = (short)(Grid.GetColumn(sender as UIElement) + 1);

            if (e.Availability == SeatAvailability.Available)
            {
                // Remove the seat from the selected seats list
                SeatIndex existingSeat = _seats.FirstOrDefault(s => s.Row == row && s.Seat == seat);
                if (existingSeat != null)
                    _seats.Remove(existingSeat);
            }
            else if (e.Availability == SeatAvailability.NewReservation)
            {
                // Add the selected seat to the selected seats list
                _seats.Add(new SeatIndex() { Row = row, Seat = seat });
            }
        }

        void BackgroundTicketOrderWork(object sender, DoWorkEventArgs e)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0,10,0)))
            {
                _ticketingProxy.OrderTicket(e.Argument as Order, _seats.ToArray());

                scope.Complete();
            }
        }

        void BackgroundTicketOrderCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            statusText.Text = string.Empty;

            if (e.Error != null)
            {
                MessageBox.Show("Exception was thrown: " + e.Error.Message);
            }
            else
            {
                MessageBox.Show(Properties.Resources.OrderTicketWindow_OperationCompleted);
            }
            GetHallState(AsyncType.NoAsync);
            btnOrderTicket.IsEnabled = true;
        }


        private void btnOrderTicket_Click(object sender, RoutedEventArgs e)
        {
            if (_seats.Count == 0)
            {
                MessageBox.Show(Properties.Resources.OrderTicketWindow_NoSeatSelected);
            }
            else if (SelectedCustomer == null)
            {
                MessageBox.Show(Properties.Resources.OrderTicketWindow_NoCustomerSelected);
            }
            else
            {
                Order newOrder = new Order();
                // Create an order
                newOrder.ReservationID = Guid.NewGuid();
                newOrder.CustomerInfo = new Customer() { ID = SelectedCustomer.ID };
                newOrder.EventInfo = SelectedEvent;
                btnOrderTicket.IsEnabled = false;
                statusText.Text = "Waiting for the service to complete";
                BackgroundWorker backgroundWorker = new BackgroundWorker();

                backgroundWorker.DoWork += BackgroundTicketOrderWork;
                backgroundWorker.RunWorkerCompleted += BackgroundTicketOrderCompleted;

                backgroundWorker.RunWorkerAsync(newOrder);
            }
        }

        private void chkUseTicketingBridge_Checked(object sender, RoutedEventArgs e)
        {
            InitializeTicketingProxy(chkUseTicketingBridge.IsChecked.Value);
        }

        private void InitializeTicketingProxy(bool useBridge)
        {
            string endpointName;

            if (useBridge)
            {
                endpointName = "TicketingHttpBridgeEP";
            }
            else
            {
                endpointName = "InternalTicketingEP";
            }

            ChannelFactory<ITicketingService> factory = new ChannelFactory<ITicketingService>(endpointName);
            (factory.Endpoint.Binding as WSHttpBindingBase).TransactionFlow = true;
            _ticketingProxy = factory.CreateChannel();
        }

        private void GetHallState(AsyncType asyncType)
        {
            if (SelectedEvent != null)
            {
                SeatIndex[] seats;
                switch (asyncType)
                {
                    case AsyncType.NoAsync:
                        IHallStateService syncProxy = new ChannelFactory<IHallStateService>("HallStateEP").CreateChannel();
                        seats = syncProxy.GetHallState(SelectedEvent.EventID);
                        ShowAvailableSeats(seats);
                        break;
                }
            }
            else
            {
                ShowAvailableSeats(null);
            }
        }

        private void btnGetHallStateSync_Click(object sender, RoutedEventArgs e)
        {
            GetHallState(AsyncType.NoAsync);
        }
    }
}
