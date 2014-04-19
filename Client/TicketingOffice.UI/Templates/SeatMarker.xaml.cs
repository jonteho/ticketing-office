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

namespace TicketingOffice.UI.Templates
{
    /// <summary>
    /// Interaction logic for SeatMarker.xaml
    /// </summary>
    public partial class SeatMarker : UserControl
    {
        public SeatMarker()
        {
            InitializeComponent();
        }

        public SeatAvailability Availability
        {
            get
            {
                if (chkSeat.IsEnabled && chkSeat.IsChecked.Value)
                    return SeatAvailability.NewReservation;
                else if (chkSeat.IsEnabled && !chkSeat.IsChecked.Value)
                    return SeatAvailability.Available;
                else
                    return SeatAvailability.Reserved;
            }
            set
            {
                chkSeat.IsEnabled = (value == SeatAvailability.Available || value == SeatAvailability.NewReservation);
                chkSeat.IsChecked = (value != SeatAvailability.Available);
                RaiseReservationChanged();
            }
        }

        public delegate void ReservationChangedEventHandler(object sender, ReservationChangedEventArgs e);
        public event ReservationChangedEventHandler ReservationChanged;

        protected virtual void RaiseReservationChanged()
        {
            if (ReservationChanged != null)
                ReservationChanged(this, new ReservationChangedEventArgs(Availability));
        }

        private void chkSeat_Checked(object sender, RoutedEventArgs e)
        {
            RaiseReservationChanged();
        }
    }

    public enum SeatAvailability
    {
        Available,
        NewReservation,
        Reserved
    }

    public class ReservationChangedEventArgs : EventArgs
    {
        public SeatAvailability Availability { get; set; }
        public ReservationChangedEventArgs(SeatAvailability availability)
            : base()
        {
            Availability = availability;
        }
    }
}
