   M S B A M L   `   `   `  ���� c  _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity  U QBancLogix.ExtendedDataGrid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullkLclr-namespace:BancLogix.ExtendedDataGrid;assembly=BancLogix.ExtendedDataGridBancLogix.ExtendedDataGrid [ WBanclogix.Controls.PagedDataGrid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null}Xclr-namespace:Banclogix.Controls.PagedDataGrid;assembly=Banclogix.Controls.PagedDataGrid Banclogix.Controls.PagedDataGrid  DM2.Ent.Client.Views6   0DM2.Ent.Client.Views.BankAccountTransferListView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^	 ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 	 h
 dActiproSoftware.Docking.Wpf351, Version=11.1.545.0, Culture=neutral, PublicKeyToken=36ff2196ab5654b9Cdocking5http://schemas.actiprosoftware.com/winfx/xaml/docking 
 XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity 
 cextendedDataGridLclr-namespace:BancLogix.ExtendedDataGrid;assembly=BancLogix.ExtendedDataGrid 
 lpagedDataGridXclr-namespace:Banclogix.Controls.PagedDataGrid;assembly=Banclogix.Controls.PagedDataGrid 
 -     �Name$  
twDealList��   HedgeDealList � Title# �   $
��1060��$	��700��  CommonOnlyCloseWindowStyle#��[ .��� $��-2,0,2,0q� � RowDefinitions �� $
��Auto ��� $��* ��� $��30 �.+��� $��0�����#F8F8F8$
��Auto=�$
��Auto=�$	��250��.���  �� $��30 ��� $��30 ��� $��10 � � ColumnDefinitions �� $��90 ��� $	��120 ��� $��95 ��� $	��140 ��� $��95 ��� $	��120 ��� $	��110 ��� $	��140 ��� $	��110 ��� $��* �.+��� $��0��  FromBusinessUnit#���  $��0,0,10,0q�$��Right=�$��Center=��� $��1�� �� 
IsEditable .  �� DisplayMemberPath$
 Name�� �� SelectedIndex$ 0������ *FromBusinessUnitList+ �� SelectedValue �� *FromBusinessUnit+.   (System.Windows.Interactivity.Interaction   Triggers /   )System.Windows.Interactivity.EventTrigger  	   	EventName$	 SelectionChanged��.   (System.Windows.Interactivity.TriggerBase
   Actions.
 6   0System.Windows.Interactivity.InvokeCommandAction     Command �� *BuSelectionChangedCommand+�� $��2��  FromAccount#���  $��0,0,10,0q�$��Right=�$��Center=��� $��3��$	��125��$
��Left=�{�.  ���� *FromBankAcctModel.AccountNo+�� $6��0/Resources/Images/DarkBlue/common_add_cancel.png�� J� Opacity$	 0.6Q�$��3��$��Right=�$��12��$��	0,12,22,0q� 	 Caliburn.Micro.Message   Attach$C =[Event MouseLeftButtonDown] = [Action ClearFromBankAccount()]��z���  �� Angle$	 -45Q�����  �� 
TargetType" �� �� Triggers ��  �� Binding �� *SearchBankAccountId+ �� Mode$ TwoWay=� �� Value  .a���  �� Property � y� �� Value$ 	Collapsed=��� $��3��$��0��$(��"/Resources/Images/White/search.png��$��12��$��Right=�   $	 MouseLeftButtonDown��.
 " 	 Caliburn.Micro.ActionMessage     
MethodName$ FindFromAcctBnt���� $��4��  ToBusinessUnit#���  $��0,0,10,0q�$��Right=�$��Center=��� $��5�� . $
 Name��$ 0������ *ToBusinessUnitList+ �� *ToBusinessUnit+   $	 SelectionChanged��.
    �� *BuSelectionChangedCommand+�� $��6��  	ToAccount#���  $��0,0,10,0q�$��Right=�$��Center=��� $��7��$	��125��$
��Left=�{�.  ���� *ToBankAcctModel.AccountNo+�� $6��0/Resources/Images/DarkBlue/common_add_cancel.png��$	 0.6Q�$��7��$��Right=�$��12��$��	0,12,22,0q�$A ;[Event MouseLeftButtonDown] = [Action ClearToBankAccount()]��z��� $	 -45Q����� " �� ��  �� *SearchBankAccountId+$ TwoWay=�  .a���  � y�$ 	Collapsed=��� $��7��$��0��$(��"/Resources/Images/White/search.png��$��12��$��Right=�   $	 MouseLeftButtonDown��.
   $ FindToAcctBnt���� $��1��$��0��$��Right=�  TradeDateFrom#���  $
��10 5q�(  "System.Windows.Controls.DatePicker  $��1��$��1��  ClearButtonDatePicker#��[    Text �� *	TradeFrom+$ OneWayToSource=�  DateTimeEmptyConverter �� 	Converter# [ �� $��1��$��2��$��Right=�$��Center=� 	 To#��� 	 $��0,0,10,0q�  $��1��$��3��#��[  �� *	TradeTo+$ OneWayToSource=�# [ �� $��1��$��4�� 

 DealID#��� 
 $��0,0,10,0q�$��Right=�$��Center=��� $��5��$��1������ *DealId+�� $��1��$��6��  HedgingDealID#���  $��0,0,10,0q�$��Right=�$��Center=��� $��1��$��7������ *HedgingDealId+�� $��8��$��0��$��24��$
��Left=�$��10,0,0,0q� 
 Search#���    CommonButtonStle#��[    $	 Click��.
   $ Search��   
Parameters.  	 Caliburn.Micro.Parameter     Value$ 1��   �� *
PageSize+$ TwoWay=� �� ElementName$ PagedDataGrid���� $��1��$
��Auto��$��0,10,0,0q�$
��Auto=�$��Hidden=�$	��250��.��5	  /Banclogix.Controls.PagedDataGrid.PagingDataGrid	  -    	 Name$ PagedDataGrid��$	��500��$
��Auto��&
   System.Windows.Controls.DataGrid" 
  VerticalScrollBarVisibility$
 Auto=�$ 
  HorizontalScrollBarVisibility$
 Auto=� 
  CanUserSortColumns . $v p[Event RowDoubleClick] = [Action DoubleClicked];[Event PagingChanged] = [Action DateGridPageChanged($eventArgs)]������ *BankCashTransferList+  	  	PageCount  �� *	PageCount+! 	  CommandString! �� *PageCommand+" �� SelectedItem" �� *SelectedDeal+# 
  Columns# 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn$   Header#$ � 
 %   MinWidth$% 50Q�&   CellTemplate& �� .R��� $��Center=�$��Right=����� *Id+  #$ �  $	% 100Q�& �� .R��� $��5,0,0,0q�$��Center=�$
��Left=����� *
FromBuName+    FromCounterparty#$ �  $% 90Q�& �� .R��� $��5,0,0,0q�$��Center=�$
��Left=����� *
FromCpName+  #$ �  $% 80Q�& �� .R��� $��5,0,0,0q�$��Center=�$
��Left=����� *FromBankAcctNo+  #$ �  $% 30Q�& �� .R��� $��5,0,0,0q�$��Center=�$
��Left=����� *
ToBuName+    ToCounterparty#$ �  $	% 100Q�& �� .R��� $��5,0,0,0q�$��Center=�$
��Left=����� *
ToCpName+    ToBankAcctNo#$ �  $% 30Q�& �� .R��� $��Center=�$��5,0,0,0q�$
��Left=����� *ToBankAcctNo+    CCY#$ �  $% 50Q�& �� .R��� $��5,0,0,0q�$��Center=�$
��Left=����� *CcyId+  CurrencyIdToNameConverter# [    
 Amount#$ �  $% 80Q�& �� .R��� ' �� TextAlignment$' Right=�$��0,0,5,0q���`�  # FormatAmountByCurrencyConverter( `� 	Converter#( [ .1��� ) �� Path$) Amount��� $) CcyId�    LocalTradeDate#$ �  $% 80Q�& �� .R��� $��5,0,0,0q�$
' Left=����� *LocalTradeDate+  FormatDateByBuConverter# [     User#$ �  $% 70Q�& �� .R��� $
��Auto��$��5,0,0,0q�$��Center=�$
��Left=����� *UserId+$ OneWay=�  UserIdToNameConverter# [    	 Staff#$ �  $% 80Q�& �� .R��� $��Center=�$��5,0,0,0q�$
��Left=����� *	StaffId+    CreationTime#$ �  $	% 140Q�& �� .R��� $��5,0,0,0q�$
��Left=����� *CreationTime+  FormatDateTimeByBuConverter# [   #$ �  $% 80Q�& �� .R��� $��5,0,0,0q�$��Center=�$
��Left=����� *	HedgingId+� $��2��.+��� $��1��$��7��$��0,5,20,0q�$��24��$��Right=� 	 Close#���  #��[ $$ [Event Click] = [Action Close]��