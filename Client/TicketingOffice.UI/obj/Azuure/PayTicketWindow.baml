   M S B A M L   `   `   `  ���� M  ITicketingOffice.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:TicketingOffice.UI.PropertiesTicketingOffice.UI.Properties  7 clr-namespace:TicketingOffice.UITicketingOffice.UI  (    "TicketingOffice.UI.PayTicketWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     B
properties+clr-namespace:TicketingOffice.UI.Properties     2local clr-namespace:TicketingOffice.UI     -     � Title$  PayTicketWindow��$	��441��$	��353�� � 	Resources )   #TicketingOffice.UI.EnabledConverter   enabledConverter.   (TicketingOffice.UI.EnabledMultiConverter  enabledMultiConverter��%   &	        &	          .��� .+��� -   $��cmbCustomers��$��27��$
��Left=�$��	12,38,0,0q�$	��Top=�$	��169���� -   'TicketingOffice.UI.Properties.Resources*   #PayTicketWindow_SelectCustomerLabel#��Z $��28��$
��Left=�$��	12,12,0,0q�$	��Top=�$	��169���� -   $��	lstOrders��$��79��$��12,104,12,0q�$	��Top=��� '    PayTicketWindow_SelectOrderLabel#��Z $��28��$
��Left=�$��	12,81,0,0q�$	��Top=�$	��305���� )   "PayTicketWindow_SelectPaymentLabel#��Z $��28��$
��Left=�$��
12,201,0,0q�$	��Top=�$	��305��� -   $��grid1��$
��Left=�$��	0,235,0,0q�$	��Top=�$	��331��$	��167�� � ColumnDefinitions �� $��* ��� $��* � � RowDefinitions �� $
��Auto ��� $��* �.+��� -   $��lstPayments��$��0��$��0��$��79��$	��Top=�$��0,0,0,10q��� -   $��	btnRefund��$	��Top=�$��1��$��0��$��Center=�#   PayTicketWindow_RefundButton#��Z $��37��$	��108��{���  �� Path$ SelectedItem�	 �� ElementName$	 lstPayments��
 �� 	Converter#
 [  � $��1��$��0�� �� $
��Auto ��� $
��Auto ��� $	��27* � �� $
��Auto ��� .+��� '    PayTicketWindow_txtPaymentAmount#��Z $��0��$��0��$��5,5,3,0q��� -   $��txtPaymentAmount��$��1��$��0��$��0,5,0,0q��� %   PayTicketWindow_cmbPaymentType#��Z $��0��$��1��$��5,5,3,0q��� -   $��cmbPaymentType��$��1��$��1��$��0,5,0,0q�.@��� 
 � Tag$ 
CreditCard��)   "PayTicketWindow_PaymentType_Credit#��Z �� $
 Cash��'    PayTicketWindow_PaymentType_Cash#��Z �� $ Cheque��)   "PayTicketWindow_PaymentType_Cheque#��Z �� $ Voucher��*   #PayTicketWindow_PaymentType_Voucher#��Z �� &   PayTicketWindow_cmbCreditNumber#��Z $��0��$��2��$��5,5,3,0q��� -	   $��txtCreditCardNo��$��1��$��2���� -
   $��
btnPayment��$	��Top=�$��1��$��1��$��2��$��Center=�$   PayTicketWindow_PaymentButton#��Z $��37��$	��108��{�`�  `� 	Converter# [ .1��� $
 Text�$	 txtPaymentAmount���� $ SelectedValue�$	 cmbPaymentType���� $ SelectedValue�$	 	lstOrders��