   M S B A M L   `   `   `  ���� c  _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity  U QBancLogix.ExtendedDataGrid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullkLclr-namespace:BancLogix.ExtendedDataGrid;assembly=BancLogix.ExtendedDataGridBancLogix.ExtendedDataGrid [ WBanclogix.Controls.PagedDataGrid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null}Xclr-namespace:Banclogix.Controls.PagedDataGrid;assembly=Banclogix.Controls.PagedDataGrid Banclogix.Controls.PagedDataGrid  DM2.Ent.Client.Views/   )DM2.Ent.Client.Views.BankCashFlowListView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^	 ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 	 h
 dActiproSoftware.Docking.Wpf351, Version=11.1.545.0, Culture=neutral, PublicKeyToken=36ff2196ab5654b9Cdocking5http://schemas.actiprosoftware.com/winfx/xaml/docking 
 XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity 
 cextendedDataGridLclr-namespace:BancLogix.ExtendedDataGrid;assembly=BancLogix.ExtendedDataGrid 
 lpagedDataGridXclr-namespace:Banclogix.Controls.PagedDataGrid;assembly=Banclogix.Controls.PagedDataGrid 
 -     �Name$  
twDealList��   HedgeDealList � Title# �   $	��990��$	��700��  CommonOnlyCloseWindowStyle#��[ .��� $��-2,0,2,0q� � RowDefinitions �� $
��Auto ��� $��* ��� $��40 �.+��� $��0�����#F8F8F8$
��Auto=�$
��Auto=�$	��450��.���  �� $��50 � � ColumnDefinitions �� $��85 ��� $	��140 ��� $��13 ��� $��60 ��� $	��140 ��� $	��100 ��� $	��140 ��� $��40 ��� $	��130 ��� $	��115 ��� $��* �.+��� $��0��  BankAccount#���  $��0,0,10,0q�$��Center=�$��Right=��� $��1��$	��135��$
��Left=�{�.  ���� *BankAcctModel.AccountNo+�� $��2��$��0��$(��"/Resources/Images/White/search.png��$��12��$��Right=�.   (System.Windows.Interactivity.Interaction   Triggers /   )System.Windows.Interactivity.EventTrigger     	EventName$ MouseLeftButtonDown��.   (System.Windows.Interactivity.TriggerBase   Actions. " 	 Caliburn.Micro.ActionMessage     
MethodName$ FindAcctBnt���� $��0��$��3��  Balance#���  $��0,0,10,0q�$��Center=�$��Right=��� $��4��$	��125��$
��Left=�{�.  .����`�  # FormatAmountByCurrencyConverter `� 	Converter# [ .1��� 	 �� Path$!	 BankAcctModel.LedgerBalance��� $	 CcyId�.���� $��0��$��5��$��Right=�$��Center=�  CreationTimeFrom#���  $
��10 5q�(  "System.Windows.Controls.DatePicker  $��0��$��6��  ClearButtonDatePicker#��[ 
   Text
 �� *	TradeFrom+ �� Mode$ OneWayToSource=�  DateTimeEmptyConverter �� 	Converter# [ �� $��0��$��7��$��Center=�$��Right=�  To#���  $��0,0,10,0q�  $��0��$��8��#��[ 
 �� *	TradeTo+$ OneWayToSource=�# [ �� $��9��$��0��$��24��$
��Left=�$��10,0,0,0q� 
	 Search#��� 	  
 CommonButtonStle#��[
    $ Click��.   $ Search��   
Parameters.  	 Caliburn.Micro.Parameter     Value$ 1��   �� *
PageSize+$ TwoWay=� �� ElementName$ PagedDataGrid���� $��1��$
��Auto��$��0,10,0,0q�$
��Auto=�$��Hidden=�$	��250��.��5  /Banclogix.Controls.PagedDataGrid.PagingDataGrid  -     Name$ PagedDataGrid��$	��500��$
��Auto��&   System.Windows.Controls.DataGrid"   VerticalScrollBarVisibility$
 Auto=�$   HorizontalScrollBarVisibility$
 Auto=�$��1��   CanUserSortColumns . 	 	 Caliburn.Micro.Message 	  Attach$F @[Event PagingChanged] = [Action DateGridPageChanged($eventArgs)]������ *BankCashFlowList+   	PageCount �� *	PageCount+   CommandString �� *PageCommand+   Columns 4
  .System.Windows.Controls.DataGridTemplateColumn
    BusinessUnit,  &System.Windows.Controls.DataGridColumn   Header# �     MinWidth$	 100Q� 
  CellTemplate �� .R��� $��	10,0,10,0q�$��Center=�$
��Left=����� *BuId+  BUIDToNameConverter# [ 
    Counterparty# �  $ 90Q� �� .R��� $��	10,0,10,0q�$��Center=�$
��Left=����� *CpId+ ! CounterpartyIdToNameConverter# [ 
    
Instrument# �  $ 80Q� �� .R��� $��	10,0,10,0q�$��Center=�$
��Left=����� *InstrumentId+  InstrumnetIdToNameConverter# [ 
    CCY# �  $ 50Q� �� .R��� $��	10,0,10,0q�$��Center=�$
��Left=����� *CcyId+  CurrencyIdToNameConverter# [ 
   
 Amount# �  $ 80Q� �� .R���  �� TextAlignment$ Right=�$��	10,0,10,0q����� *Amount+ $  DeficitAmountForegroundConverter# [ ��`� # [ .1��� $	 Amount��� $	 CcyId�
  # �  $ 80Q� �� .R��� $ Right=�$��	10,0,10,0q����� *CurrentLedgerBalance+# [ ��`� # [ .1��� $	 CurrentLedgerBalance��� $	 CcyId�
    	ValueDate# �  $ 80Q� �� .R��� $��	10,0,10,0q�$
 Left=����� *	ValueDate+  FormatDateByBuConverter# [ 
    	RelatedId# �  $ 80Q� �� .R��� $��	10,0,10,0q�$��Center=�$
��Left=����� *	RelatedId+
    CreationTime# �  $	 140Q� �� .R��� $��	10,0,10,0q�$
��Left=����� *CreationTime+  FormatDateTimeByBuConverter# [ �� $��2��$��7��$��0,0,20,0q�$��24��$��Right=� 	 Close#���  #��[
 $$ [Event Click] = [Action Close]��