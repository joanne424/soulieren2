   M S B A M L   `   `   `  ����   DM2.Ent.Client.Views4    .DM2.Ent.Client.Views.Report.ExposureReportView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  $	��600��$	��900��  � WindowStartupLocation$  CenterScreen=�   CommonOnlyCloseWindowStyle#��[  .���  � RowDefinitions �� $	��100 ��� $��10 ��� $��* ��� $��40 �.+�� $��0�����#F8F8F8 �� $��10 ��� $��40 ��� $��40 ��� $��10 � � ColumnDefinitions �� $��2* ��� $��3* ��� $��2* ��� $��3* ��� $��4* �.+���   BusinessUnit#���  $��0,0,10,0q�$��1��$��0���� $��1��$��1�� �� DisplayMemberPath$
 Name�� �� SelectedValuePath$ Id������ *BusinessUnits+ �� SelectedValue �� *SelectedBusinessUnitId+ �� Mode$ TwoWay=���   	AssetType#���  $��0,0,10,0q�$��1��$��2���� $��1��$��3��$��2�� �� Orientation$ 
Horizontal=�$
��Left=�$��Center=�.+���  ! AssetType.HedgingDealPosition#���   d� 	IsChecked �� *IsHedgingDealPositionChecked+$ TwoWay=���  " AssetType.InternalDealPosition#���  {�.  $
��12,0q� �� *IsInternalDealPositionChecked+$ TwoWay=���   AssetType.BankBalance#���   �� *IsBankAccountBalanceChecked+$ TwoWay=���   	ValueDate#���  $��0,0,10,0q�$��2��$��0��(  "System.Windows.Controls.DatePicker    ClearButtonDatePicker#��[ $��2��$��1��	   SelectedDate	 �� *SelectedValueDate+$ TwoWay=���   	ReportCCY#���  $��0,0,10,0q�$��2��$��2���� -   $��	ReportCcy��$��2��$��3��$
 Name��$ Id������ *
Currencies+ �� *SelectedCurrencyId+$ TwoWay=��� $��2��$��4��$��Center=� 
	 Search#��� 	  
 CommonButtonStle#��[
   Caliburn.Micro.Message
   Attach$%
 [Event Click] = [Action Search]��&   System.Windows.Controls.DataGrid  $��2��$:
 4[Event MouseDoubleClick] = [Action OpenDetailedList]������ *CurrencySummaryList+ �� SelectedItem �� *SelectedCurrencySummary+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ 2*   CCY   Header# �  1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *CurrencyName+  $ 3*  
 Amount# �   `�  # FormatAmountByCurrencyConverter `� 	Converter# [ .1���  �� Path$ Amount��� $ 
CurrencyId�   	CellStyle ��   DataGridCellRightStyle �� BasedOn# [  �� 
TargetType*  $System.Windows.Controls.DataGridCell"  .���  �� Property � �� �� Value �� *Amount+ $  DeficitAmountForegroundConverter �� 	Converter# [   $ 3*   
MarketRate# �  # [  `�  " FormatPriceByContractConverter# [ .1��� $ 
MarketRate��� $ 
ContractId�  $ 3*    HeaderTemplate �� .R��� ���� $ SelectedItem.Name� �� ElementName$ 	ReportCcy��$ TwoWay=� `� # [ .1��� $ TransferAmount��� $ 	ReportCcy��$ SelectedValue�$ TwoWay=� �� # [ "  .���  � �� �� *TransferAmount+# [ �� $��3��$��Right=� 	 Close#���  #��[
 $$
 [Event Click] = [Action Close]��