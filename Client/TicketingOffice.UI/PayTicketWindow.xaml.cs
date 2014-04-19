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
using System.ServiceModel.Description;
using System.Diagnostics;
using TicketingOffice.PaymentService.Contracts;

namespace TicketingOffice.UI
{
    /// <summary>
    /// Interaction logic for PayTicketWindow.xaml
    /// </summary>
    public partial class PayTicketWindow : Window
    {
        public PayTicketWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Display the customers list
            FillCustomerList();
        }

        private void FillCustomerList()
        {
            Repositories.ICustomersRepository repository = Repositories.RepositoryFactory.Default.CreateCustomerRepository();
            cmbCustomers.ItemsSource = repository.GetCustomers();
        }

        private void cmbCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Display the list of orders for the selected customer
            if (cmbCustomers.SelectedItem != null)
            {
                Repositories.IOrdersRepository repository = Repositories.RepositoryFactory.Default.CreateOrdersRepository();
                lstOrders.ItemsSource = repository.GetCustomersOrders(SelectedCustomer);
            }
        }

        private void btnPayment_Click(object sender, RoutedEventArgs e)
        {
            double amount;

            if (double.TryParse(txtPaymentAmount.Text, out amount))
            {
                if (amount > 0)
                {
                    // Call the payment service
                    PaymentServiceReference.IPaymentService proxy = new
             PaymentServiceReference.PaymentServiceClient();

                    proxy.PayForOrder(
                                SelectedOrder.ID,
                                SelectedCustomer.ID,
                                amount,
                                SelectedPaymentType,
                                txtCreditCardNo.Text);

                    // Reload the payments of the selected order
                    ShowPaymentsForOrder(SelectedOrder);
                }
                else
                {
                    MessageBox.Show(
        Properties.Resources.PayTicketWindow_NewPayment_NotPositive);
                }
            }
            else
            {
                MessageBox.Show(
                Properties.Resources.PayTicketWindow_NewPayment_NotDouble);
            }
        }

        private void btnRefund_Click(object sender, RoutedEventArgs e)
        {
            PaymentServiceReference.IPaymentService proxy = new PaymentServiceReference.PaymentServiceClient();
            proxy.Refund(SelectedPayment.ID, SelectedPayment.CustomerID);
        }

        private CrmService.Contracts.Customer SelectedCustomer
        {
            get
            {
                return cmbCustomers.SelectedItem as CrmService.Contracts.Customer;
            }
        }

        private TicketingOffice.TicketingService.Contracts.Order SelectedOrder
        {
            get
            {
                return lstOrders.SelectedItem as TicketingOffice.TicketingService.Contracts.Order;
            }
        }

        private Payment SelectedPayment
        {
            get
            {
                return lstPayments.SelectedItem as Payment;
            }
        }

        private PaymentType SelectedPaymentType
        {
            get
            {
                return (PaymentType)Enum.Parse(typeof(PaymentType),
                            ((ComboBoxItem)cmbPaymentType.SelectedValue).Tag.ToString());
            }
        }

        private void lstOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowPaymentsForOrder(SelectedOrder);
        }

        private void ShowPaymentsForOrder(TicketingService.Contracts.Order order)
        {
            if (order == null)
                lstPayments.ItemsSource = null;
            else
            {
                PaymentServiceReference.IPaymentService proxy = new PaymentServiceReference.PaymentServiceClient();
                lstPayments.ItemsSource = proxy.FindPayments(
                   new PaymentCriteria() { OrderID = order.ID });
            }

        }
    }
}
