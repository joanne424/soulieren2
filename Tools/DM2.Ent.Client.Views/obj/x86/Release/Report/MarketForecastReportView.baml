   M S B A M L   `   `   `  ����   DM2.Ent.Client.Views:    4DM2.Ent.Client.Views.Report.MarketForecastReportView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  $	��600��$
��1024��  � WindowStartupLocation$  CenterScreen=�   CommonOnlyCloseWindowStyle#��[  .���  � RowDefinitions �� $��70 ��� $��* ��� $��30 �.+�� $��0�����#F8F8F8$��0 0 0 15q� � ColumnDefinitions �� $��3* ��� $��3* ��� $��3* ��� $��3* ��� $��3* ��� $��3* ��� $��2* ��� $��4* �.+���   BusinessUnit#���  $��0,0,10,0q�$��0���� $��1�� �� DisplayMemberPath$
 Name�� �� SelectedValuePath$ Id������ *BusinessUnits+ �� SelectedValue �� *SelectedBusinessUnitId+ �� Mode$ TwoWay=���   Contract#���  $��0,0,10,0q�$��2���� $��0��$��3��$
 Name��$ Id������ *	Contracts+ �� *SelectedContractId+$ TwoWay=���   	ReportCCY#���  $��0,0,10,0q�$��4���� -   $��	ReportCcy��$��0��$��5��$
 Name��$ Id������ *
Currencies+ �� *SelectedCurrencyId+$ TwoWay=��� $��0��$��7��$��Center=� 
 Search#���    CommonButtonStle#��[   Caliburn.Micro.Message   Attach$% [Event Click] = [Action Search]��&   System.Windows.Controls.DataGrid  $��1��$6 0[Event MouseDoubleClick] = [Action OpenDealList]������ *ContractSummaryList+ �� SelectedItem �� *SelectedContractSummary+	   Columns	 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn
   MinWidth$	
 100Q�   Header# �  1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *ContractName+  $
 90Q�  
MarketRate# �    DataGridCellRightStyle   	CellStyle# [  `�  " FormatPriceByContractConverter `� 	Converter# [ .1���  �� Path$ 
MarketRate��� $ 
ContractId�  $	
 130Q�   HeaderTemplate �� .R���  �� Orientation$ 
Horizontal=�.+���  	 PL#��� 	 �� $��(���� ���� $ SelectedItem.Name� �� ElementName$ 	ReportCcy���� $��)�� `�  #
 FormatAmountByCurrencyConverter# [
 .1��� $ Pl��� $ 	ReportCcy��$ SelectedValue�$ TwoWay=� ��  �� BasedOn# [  �� 
TargetType*  $System.Windows.Controls.DataGridCell"  .���  �� Property � �� �� Value �� *Pl+ $  DeficitAmountForegroundConverter �� 	Converter# [ 4  .System.Windows.Controls.DataGridTemplateColumn  $
 90Q�  ForecastRate# �     CellTemplate �� .R���  �� TextAlignment$ Right=� �� Padding$	 5,0q�$
��Auto�� � Cursor$
 Hand������ *ForecastRate+$ TwoWay=�.  (System.Windows.Interactivity.Interaction   Triggers /	  )System.Windows.Interactivity.EventTrigger	   	  	EventName$ MouseLeftButtonDown��.
  (System.Windows.Interactivity.TriggerBase 
  Actions. "  Caliburn.Micro.ActionMessage     
MethodName$ ModifyForecastRate��    
Parameters.    Caliburn.Micro.Parameter  !   Value! �� *
ContractId+  $	
 130Q� �� .R��� $ 
Horizontal=�.+���   
ForecastPL#���  �� $��(���� ���� $ SelectedItem.Name�$ 	ReportCcy���� $��)�� `� # [
 .1��� $ 
ForecastPl��� $ 	ReportCcy��$ SelectedValue�$ TwoWay=� �� # [ "  .���  � �� �� *
ForecastPl+# [   "   Width5  /System.Windows.Controls.DataGridLengthConverter$" * $	
 100Q�  	ValueDate# �   �� *	ValueDate+  FormatDateByBuConverter# [   $" * $	
 120Q�  
CCY1Amount# �   `� # [
 .1��� $ 
Ccy1Amount��� $ Ccy1Id� �� # [ "  .���  � �� �� *
Ccy1Amount+# [   $" * $	
 120Q�  
CCY2Amount# �   `� # [
 .1��� $ 
Ccy2Amount��� $ Ccy2Id� �� # [ "  .���  � �� �� *
Ccy2Amount+# [   $
 90Q�  AverageRate# �  # [  `� # [ .1��� $ AverageRate��� $ 
ContractId��� $��2��$ 
Horizontal=�$
��Left=�.+���  	 Total#���  $
��Bold"�$ Center=�$	��100���� $
��Bold"�$ Right=�$	��220������ *	TotalPl+# [ ��`� # [
 .1��� $ TotalPl��� $ SelectedCurrencyId�$ TwoWay=��� $
��Bold"�$ Right=�$	��220������ *TotalForecastPl+# [ ��`� # [
 .1��� $ TotalForecastPl��� $ SelectedCurrencyId�$ TwoWay=��� $��2��$��Right=� 	 Close#���  #��[ $$ [Event Click] = [Action Close]��