   M S B A M L   `   `   `  ���� [  WBanclogix.Controls.PagedDataGrid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null}Xclr-namespace:Banclogix.Controls.PagedDataGrid;assembly=Banclogix.Controls.PagedDataGrid Banclogix.Controls.PagedDataGrid  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity U QBancLogix.ExtendedDataGrid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullkLclr-namespace:BancLogix.ExtendedDataGrid;assembly=BancLogix.ExtendedDataGridBancLogix.ExtendedDataGrid  DM2.Ent.Client.Views.   (DM2.Ent.Client.Views.BankAccountListView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^	 ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 	 h
 dActiproSoftware.Docking.Wpf351, Version=11.1.545.0, Culture=neutral, PublicKeyToken=36ff2196ab5654b9Cdocking5http://schemas.actiprosoftware.com/winfx/xaml/docking 
 lpagedDataGridXclr-namespace:Banclogix.Controls.PagedDataGrid;assembly=Banclogix.Controls.PagedDataGrid 
 XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity 
 cextendedDataGridLclr-namespace:BancLogix.ExtendedDataGrid;assembly=BancLogix.ExtendedDataGrid 
 -     �Name$  
twDealList��   BankAccountList � Title# �   $
��1100��$	��600��  CommonOnlyCloseWindowStyle#��[  � 	Resources   textCellRight �� 
TargetType �� Property �� Value  textCellLeft�� %�   &	       &	 O     �� " ��.���  �@��TextAlignment$ Right=���  � ��$ 5,0,5,0q��� " ��.���  �@��TextAlignment$
 Left=���  � ��$ 5,0,5,0q�.��� $��-2,0,2,0q� � RowDefinitions �� $
��Auto ��� $��* �.+��� $��0��$
��Auto=�$
��Auto=�$	��250��.���  �� $��30 ��� $��30 � � ColumnDefinitions �� $	��100 ��� $	��125 ��� $	��100 ��� $	��120 ��� $��70 ��� $��90 ��� $	��100 ��� $	��120 ��� $	��110 ��� $	��120 ��� $��* �.+��� $��0��  BusinessUnit#���  $��0,0,10,0q�$��Right=�$��Center=��� $��1��{�.  �� DisplayMemberPath$
 Name��	 �� SelectedIndex$	 0������ *BusinessUnitList+
 �� SelectedValue
 �� *SelectedBusinessUnit+.  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ SelectionChanged��.  (System.Windows.Interactivity.TriggerBase   Actions. 6  0System.Windows.Interactivity.InvokeCommandAction     Command �� *BuSelectionChangedCommand+�� $��2��  FinancialInstitution#���  $��0,0,10,0q�$��Right=�$��Center=��� $��3��{�. $
 Name��$	 0������ *FinancialInstitutions+
 �� *FinancialInstitution+�� $��0��$��4��  CCY#���  $��0,0,10,0q�$��Right=�$��Center=��� $��5��{�. $��0��$
 Name��$	 0������ *CurrencyList+
 �� *SelectedCurrency+�� $��6��$��0��  	AccountNo#���  $��0,0,10,0q�$��Right=�$��Center=��� $��7��$��0������ *	AccountNo+�� $��8��$��0��  AccountName#���  $��0,0,10,0q�$��Right=�$��Center=��� $��9��$��0������ *AccountName+�� $��1��$��7��$��10,0,0,0q�$��24��$��Center=� 	 New#��� 	  
 CommonButtonStle#��[
  	 Caliburn.Micro.Message   Attach$( "[Event Click] = [Action New_Click]���� $��8��$��1��$��24��$��Center=�$��10,0,0,0q� 
 Search#���  #��[
 $# [Event Click]=[Action Search]���� $��9��$��1��$��24��$��Center=�$��10,0,0,0q� 	 Close#���  #��[
 $" [Event Click]=[Action Close]��&   System.Windows.Controls.DataGrid  -     Name$ TB��$��1��$	��450��$��0,10,0,0q�$   HorizontalScrollBarVisibility$
 Auto=�"   VerticalScrollBarVisibility$
 Auto=�$8 2[Event MouseDoubleClick] = [Action ModifyBankAcct]��   CanUserSortColumns . ���� *BankAccountList+ �� SelectedItem �� *
SelectItem+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header# �  1	  +System.Windows.Controls.DataGridBoundColumn 	  ElementStyle# [    MinWidth$	 140Q� 	  Binding �� *BusinessUnitId+  BUIDToNameConverter �� 	Converter# [   # �  # [ $	 100Q� �� *InstitutionId+   InstitutionIdToNameConverter# [   # �  # [ $ 80Q� �� *	AccountNo+  # �  # [ $	 150Q� �� *AccountName+  # �  $ 60Q� �� *
CurrencyId+  CurrencyIdToNameConverter# [     LedgerBalance# �     Width5
  /System.Windows.Controls.DataGridLengthConverter$ *
 $	 100Q� `�  # FormatAmountByCurrencyConverter `� 	Converter# [ .1���  �� Path$ LedgerBalance��� $ 
CurrencyId�    	CellStyle  ��   DataGridCellRightStyle! �� BasedOn#! [ *  $System.Windows.Controls.DataGridCell"  .���  � �� �� *LedgerBalance+ $  DeficitAmountForegroundConverter# [     AvailableBalance# �  $ *
 $	 100Q� `� # [ .1��� $ AvailableBalance��� $ 
CurrencyId�  �� #! [ "  .���  � �� �� *AvailableBalance+# [     BankAccountGroup# �   �� *BankAccountGroup+    CreationTime# �  $	 160Q� �� *CreationTime+  FormatDateTimeByBuConverter# [     LastUpdateTime# �  $	 160Q� �� *LastUpdateTime+# [ 