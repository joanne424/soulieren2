   M S B A M L   `   `   `  ����   DM2.Ent.Client.Views4    .DM2.Ent.Client.Views.Report.MarketForecastView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  $	��150��  � 	Resources     	ReportCcy �� Mode��%<   &	        �� ���� *SelectedCurrencyId+$ TwoWay=�.���  � RowDefinitions �� $��* ��� $��30 �.+�&   System.Windows.Controls.DataGrid  $��0��  Caliburn.Micro.Message   Attach$6 0[Event MouseDoubleClick] = [Action OpenDealList]������ *ContractSummaryList+ �� SelectedItem �� *SelectedContractSummary+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   MinWidth$ 90Q�  Contract   Header# �  1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *ContractName+  $ 90Q�  
MarketRate# �    DataGridCellRightStyle	   	CellStyle#	 [  `�  " FormatPriceByContractConverter
 `� 	Converter#
 [ .1���  �� Path$ 
MarketRate��� $ 
ContractId�  $	 120Q�   HeaderTemplate �� .R���  �� Orientation$ 
Horizontal=�.+���   PL#���  �� $��(����   CNY#���  �� $��)�� `�  # FormatAmountByCurrencyConverter#
 [ .1��� $ Pl���  �� Source# [  $
 Text�	 ��  �� BasedOn# [  �� 
TargetType*  $System.Windows.Controls.DataGridCell"  .���  �� Property � �� �� Value �� *Pl+ $  DeficitAmountForegroundConverter �� 	Converter# [ 4  .System.Windows.Controls.DataGridTemplateColumn  $ 90Q� 	 ForecastRate# � 	    CellTemplate �� .R���  �� TextAlignment$ Right=� �� Padding$	 5,0q�$
��Auto�� � Cursor$
 Hand������ *ForecastRate+$ TwoWay=�.  (System.Windows.Interactivity.Interaction   Triggers /	  )System.Windows.Interactivity.EventTrigger	   	  	EventName$ MouseLeftButtonDown��.
  (System.Windows.Interactivity.TriggerBase 
  Actions. "  Caliburn.Micro.ActionMessage     
MethodName$ ModifyForecastRate��   
Parameters.   Caliburn.Micro.Parameter     Value �� *
ContractId+  $	 120Q� �� .R��� $ 
Horizontal=�.+���  
 
ForecastPL#��� 
 �� $��(���� #���  �� $��)�� `� #
 [ .1��� $ 
ForecastPl��� # [  $
 Text�	 �� # [ "  .���  � �� �� *
ForecastPl+# [      Width5  /System.Windows.Controls.DataGridLengthConverter$ * $	 100Q�  	ValueDate# �   �� *	ValueDate+  FormatDateByBuConverter# [   $ * $	 110Q�  
CCY1Amount# �   `� #
 [ .1��� $ 
Ccy1Amount��� $ Ccy1Id�	 �� # [ "  .���  � �� �� *
Ccy1Amount+# [   $ * $	 110Q�  
CCY2Amount# �   `� #
 [ .1��� $ 
Ccy2Amount��� $ Ccy2Id�	 �� # [ "  .���  � �� �� *
Ccy2Amount+# [   $ 90Q�  AverageRate# �  #	 [  `� #
 [ .1��� $ AverageRate��� $ 
ContractId��� $��1��$ 
Horizontal=�$��Center=�.+���  	 Total#���  $
��Bold"�$ Center=�$��90���� $
��Bold"�$ Right=�$	��210������ *	TotalPl+# [ ��`� #
 [ .1��� $ TotalPl��� $ SelectedCurrencyId��� $
��Bold"�$ Right=�$	��210������ *TotalForecastPl+# [ ��`� #
 [ .1��� $ TotalForecastPl��� $ SelectedCurrencyId�