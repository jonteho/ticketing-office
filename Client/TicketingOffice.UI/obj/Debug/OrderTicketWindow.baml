   M S B A M L   `   `   `  ����M  ITicketingOffice.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:TicketingOffice.UI.PropertiesTicketingOffice.UI.Properties  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   H DShowsContract, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullvKclr-namespace:TicketingOffice.ShowsService.Contracts;assembly=ShowsContract&TicketingOffice.ShowsService.Contracts 5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel 5   	   *    $TicketingOffice.UI.OrderTicketWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   B
properties+clr-namespace:TicketingOffice.UI.Properties     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   ]showsKclr-namespace:TicketingOffice.ShowsService.Contracts;assembly=ShowsContract     5   	   Hscm8clr-namespace:System.ComponentModel;assembly=WindowsBase     5   	     � Title$  OrderTicketWindow��5   	   $	��470��6#   $	��505��60    � DataContext 5	   	   ��  �� RelativeSource ��  �� Mode$
 Self=� � 	Resources 5
         cvs �� SortDescriptions+  %System.ComponentModel.SortDescription   PropertyName   	Direction��%b   &	        �� 5   
    5        5      $
 Date��6&   $ 
Descending=�6:   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   	 � ColumnDefinitions	 5   
   �� 5      �� 5      .+�5   
   �� -   'TicketingOffice.UI.Properties.Resources,
   %OrderTicketWindow_SelectCustomerLabel#��Z
 6   $��28��5      $
��Left=�6   $	��Top=�67   $	��169��6O   �� 5   
   -   5        $��cmbCustomers��5   :   $��27��6   $
��Left=�6   $	��Top=�5      $	��169��6+   $��0,24,0,0q�6Y   ��67   �� *	Customers+6Y   �� 5   
   -   5        $��lstShows��5   0   $��0,90,14,264q�5    ?   ��5      �� *Shows+5!      �� 5#   
   -   5        $��	lstEvents��5#      $��1,168,12,186q�6#   ��69   ��  �� Source# [  � 5%   
   -   5        $��seatsLayout��5%      $��1��6   6   �� 5'   
   $��0,285,10,51q�66   ��6   �� *WantedSeats+66   �� 5(   
   -   5        $��btnOrderTicket��5(   �   '    OrderTicketWindow_btnOrderTicket#��Z 6   $��23��6\   $
��Left=�6h   $��	1,377,0,0q�6�   $	��Top=�6�   $��75��6�   �� 5)   
   -   5        $��chkUseTicketingBridge��5)   �   .   'OrderTicketWindow_chkUseTicketingBridge#��Z 6   $��16��6e   $
��Left=�6q   $��
88,381,0,0q�6�   $	��Top=�6�   $	��144��6�    d� IsThreeState .   d� 	IsChecked$ FalseY�6  60  �� 5*   
   -   5        $��btnGetHallStateSync��5*   �   ,   %OrderTicketWindow_btnGelHallStateSync#��Z 6   $��23��6a   $
��Left=�6m   $��	1,226,0,0q�6�   $	��Top=�6�   $	��180��6�   6�   �� 5+   
   (   !OrderTicketWindow_SelectShowLabel#��Z 6   $��28��6\   $
��Left=�6h   $��0,57,0,0q�6�   $	��Top=�6�   $	��169��6�   �� 5,   
   )   "OrderTicketWindow_SelectEventLabel#��Z 6   $��28��6]   $
��Left=�6i   $��	0,140,0,0q�6�   $	��Top=�6�   $	��169��6�   �� 5-   
   +   $OrderTicketWindow_SelectedSeatsLabel#��Z 6   $��28��6_   $
��Left=�6k   $��	0,256,0,0q�6�   $	��Top=�6�   $	��169��6�   �� 5.   
   $��1��6   $��2��6!   $��25��65   .@�5/      �� -   5        $��
statusText��5/      50      51      52      