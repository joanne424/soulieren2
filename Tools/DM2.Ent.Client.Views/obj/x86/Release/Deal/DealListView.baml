   M S B A M L   `   `   `  ���� c  _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity  U QBancLogix.ExtendedDataGrid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullkLclr-namespace:BancLogix.ExtendedDataGrid;assembly=BancLogix.ExtendedDataGridBancLogix.ExtendedDataGrid  DM2.Ent.Client.Views'   !DM2.Ent.Client.Views.DealListView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  h	 dActiproSoftware.Docking.Wpf351, Version=11.1.545.0, Culture=neutral, PublicKeyToken=36ff2196ab5654b9Cdocking5http://schemas.actiprosoftware.com/winfx/xaml/docking 	 XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity 	 cextendedDataGridLclr-namespace:BancLogix.ExtendedDataGrid;assembly=BancLogix.ExtendedDataGrid 	 -     �Name$  
twDealList��   HedgeDealList � Title# �   $	��850��$	��700��  CommonOnlyCloseWindowStyle#��[  � 	Resources   textCellRight �� 
TargetType �� Property �� Value  textCellLeft�� %�   &	       &	 O     �� " ��.���  �@��TextAlignment$ Right=���  � ��$ 5,0,5,0q��� " ��.���  �@��TextAlignment$
 Left=���  � ��$ 5,0,5,0q�.��� $��-2,0,2,0q� � RowDefinitions �� $
��Auto ��� $��* �.+��� $��0�����#F8F8F8$
��Auto=�$
��Auto=�$	��250��.���  �� $��30 ��� $��30 ��� $��30 � � ColumnDefinitions �� $	��100 ��� $	��110 ��� $��80 ��� $	��110 ��� $��85 ��� $	��110 ��� $	��100 ��� $	��110 ��� $��* �.+��� $��0��  BusinessUnit#���  $��0,0,10,0q�$��Right=��� $��1��{�.  �� DisplayMemberPath$
 Name��	 �� SelectedIndex$	 0������ *BusinessUnitList+
 �� SelectedValue
 �� *BusinessUnit+.   (System.Windows.Interactivity.Interaction   Triggers /   )System.Windows.Interactivity.EventTrigger     	EventName$ SelectionChanged��.   (System.Windows.Interactivity.TriggerBase   Actions. 6   0System.Windows.Interactivity.InvokeCommandAction     Command �� *BuSelectionChangedCommand+�� $��2��  Counterparty#���  $��0,0,10,0q�$��Right=��� $��3��{�. $
 Name��$	 0������ *CounterPartys+
 �� *CounterParty+�� $��4�� 
 DealID#���  $��0,0,10,0q�$��Right=��� $��5������ *DealId+�� $��6��  HedgingDealID#���  $��0,0,10,0q�$��Right=��� $��7������ *HedgingDealId+�� $��1��$��0��$��Right=�  TradeDateFrom#���  $��0,0,10,0q�(  "System.Windows.Controls.DatePicker  $��1��$��1�� 	 ClearButtonDatePicker#��[	    Text �� *	TradeFrom+ �� Mode$ OneWayToSource=� 
 DateTimeEmptyConverter �� 	Converter# [
 �� $��1��$��2��$��Right=�  To#���  $��0,0,10,0q�  $��1��$��3��#��[	  �� *	TradeTo+$ OneWayToSource=�# [
 �� $��1��$��4��  ContractName#���  $��0,0,10,0q�$��Right=��� $��5��$��1��$
 Name��$	 0������ *ContractList+
 �� *SelectedContract+�� $��2��$��0��$��Right=�  ValueDateFrom#���  $��0,0,10,0q�  $��2��$��1��#��[	  �� *ValueDateFrom+$ OneWayToSource=�# [
 �� $��2��$��2��$��0,0,10,0q�$��Right=�#���    $��2��$��3��#��[	  �� *ValueDateTo+$ OneWayToSource=�# [
 ��   
Instrument#���  $��2��$��Right=�$��4��$��0,0,10,0q��� $��2��$	 0��$ Value�� �� SelectedValuePath$	 Key��$��5������ *InstrumentList+
 �� *
Instrument+�� $��7��$��1��$��24��$
��Left=�$��10,0,0,0q� 
 Search#���    CommonButtonStle#��[   Caliburn.Micro.Message   Attach$% [Event Click] = [Action Search]���� $��2��$��7��$��10,0,0,0q�$��24��$
��Left=� 	 Close#���  #��[ $$ [Event Click] = [Action Close]���� $��1��$
��Auto��$
��Auto=�$��Hidden=�$	��250��$��0,10,0,5q�.��&   System.Windows.Controls.DataGrid  -     Name$ TB��$
��Auto��$@ :[Event MouseDoubleClick] = [Action ModifyDeal_DoubleClick]��$   HorizontalScrollBarVisibility$ Hidden=�"   VerticalScrollBarVisibility$
 Auto=�   CanUserSortColumns . ���� *
DealList+ �� SelectedItem �� *
DealItem+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,	  &System.Windows.Controls.DataGridColumn 	  Header# �   	  CanUserSort .  	  MinWidth$ 60Q�1
  +System.Windows.Controls.DataGridBoundColumn 
  ElementStyle# [  
  Binding �� *Id+  # �  $	 100Q�# [  �� *BusinessUnitId+  BUIDToNameConverter# [   # �  $ 90Q�# [  �� *CounterpartyId+ ! CounterpartyIdToNameConverter# [     	ValueDate# �  $ 80Q� �� *	ValueDate+  FormatDateByBuConverter# [    
 BuyCCY# �  $ 30Q� �� *BuyCCY+  CurrencyIdToNameConverter# [     	BuyAmount# �  $	 100Q�# [    
Foreground �� *	BuyAmount+ $  DeficitAmountForegroundConverter# [  `�  # FormatAmountByCurrencyConverter  `� 	Converter#  [ .1��� ! �� Path$! 	BuyAmount��� $! BuyCCY�    SellCCY# �  $ 30Q� �� *	SellCCY+# [     
SellAmount# �  $	 100Q�# [  �� *
SellAmount+# [  `� #  [ .1��� $! 
SellAmount��� $! SellCCY�    ContractRate# �  $ 80Q�# [  �� *ContractRate+    DealerSpotRate# �  $ 80Q�# [  �� *DealerSpotRate+    ForwardPoint# �  $ 70Q�# [  �� *ForwardPoint+  # �  $	 130Q�# [  �� *
Instrument+   InstrumnetIdToNameConverter# [     
! Status# � ! $	 100Q� �� *Status+ " StatusDisplayConverter# ["   # �   . $ 80Q�# [  �� *HedgingDealId+   # LocalTradeDate# � # $ 80Q� �� *LocalTradeDate+# [    $ CreationTime# � $ $	 140Q� �� *CreationTime+ % FormatDateTimeByBuConverter# [% 