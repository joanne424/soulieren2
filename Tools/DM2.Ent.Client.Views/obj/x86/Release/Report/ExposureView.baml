   M S B A M L   `   `   `  ����   DM2.Ent.Client.Views.    (DM2.Ent.Client.Views.Report.ExposureView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  $	��200��$	��400��  � 	Resources     	ReportCcy �� Mode��%:   &	        �� ���� *ReportCurrencyId+$ TwoWay=�.��� .+�&   System.Windows.Controls.DataGrid    Caliburn.Micro.Message   Attach$: 4[Event MouseDoubleClick] = [Action OpenDetailedList]������ *CurrencySummaryList+ �� SelectedItem �� *SelectedCurrencySummary+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ 2*    MinWidth$	 100Q�  CCY   Header# �  1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *CurrencyName+  $ 3* $	 100Q� 
 Amount# �   `�  # FormatAmountByCurrencyConverter	 `� 	Converter#	 [ .1��� 
 �� Path$
 Amount��� $
 
CurrencyId�   	CellStyle ��   DataGridCellRightStyle �� BasedOn# [  �� 
TargetType*  $System.Windows.Controls.DataGridCell"  .���  �� Property � �� �� Value �� *Amount+ $  DeficitAmountForegroundConverter �� 	Converter# [   $ 3* $	 100Q�  
MarketRate# �  # [  `�  " FormatPriceByContractConverter#	 [ .1��� $
 
MarketRate��� $
 
ContractId�  $ 3* $	 100Q�  CNY# �   `� #	 [ .1��� $
 TransferAmount���  �� Source# [   �� # [ "  .���  � �� �� *TransferAmount+# [ 