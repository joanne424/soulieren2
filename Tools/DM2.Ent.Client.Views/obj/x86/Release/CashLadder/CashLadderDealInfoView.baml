   M S B A M L   `   `   `  ���� U  QBancLogix.ExtendedDataGrid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullkLclr-namespace:BancLogix.ExtendedDataGrid;assembly=BancLogix.ExtendedDataGridBancLogix.ExtendedDataGrid   DM2.Ent.Client.Views1   +DM2.Ent.Client.Views.CashLadderDealInfoView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     iextendedDataGridLclr-namespace:BancLogix.ExtendedDataGrid;assembly=BancLogix.ExtendedDataGrid     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org     CommonOnlyCloseWindowStyle#��[    � Title$  CashLadderDealInfoView��$	��500��$	��900��.���  � ColumnDefinitions �� $	��48* ��� $
��175* � � RowDefinitions �� $��* ��� $��40 �.+��� $
��Auto=�$
��Auto=�$��2��.��&   System.Windows.Controls.DataGrid  -     Name$ TB��$��0��$   HorizontalScrollBarVisibility$ Hidden=�"   VerticalScrollBarVisibility$ Hidden=�   CanUserSortColumns . ���� *
DealList+   Columns 4  .System.Windows.Controls.DataGridTemplateColumn   
 DealID,  &System.Windows.Controls.DataGridColumn   Header# �  	   MinWidth$	 60Q�
   CellTemplate
 �� .R��� $��Center=�$��Right=�$��5,0,5,0q����� *Id+    BusinessUnit# �  $	 80Q�
 �� .R��� $��Center=�$
��Left=�$��5,0,5,0q����� *BusinessUnitId+  BUIDToNameConverter �� 	Converter# [     Counterparty# �  $	 80Q�
 �� .R��� $��Center=�$
��Left=�$��5,0,5,0q����� *CounterpartyId+ ! CounterpartyIdToNameConverter# [     	ValueDate# �  $		 150Q�
 �� .R��� $��Center=�$��Center=�$��5,0,5,0q����� *	ValueDate+  FormatDateByBuConverter# [    
 BuyCCY# �  
 �� .R��� $��Center=�$��Center=�$��5,0,5,0q����� *BuyCCY+ 	 CurrencyIdToNameConverter# [	    
 	BuyAmount# � 
 $		 100Q�
 �� .R��� $��Center=�$��Right=���`�  # FormatAmountByCurrencyConverter `� 	Converter# [ .1���  �� Path$ 	BuyAmount��� $ BuyCCY�    SellCCY# �  
 �� .R��� $��Center=�$��Center=�$��5,0,5,0q����� *	SellCCY+# [	     
SellAmount# �  $		 100Q�
 �� .R��� $��Center=�$��Right=���`� # [ .1��� $ 
SellAmount��� $ SellCCY�    ContractRate# �  $		 160Q�
 �� .R��� $��Center=�$��Right=�$��5,0,5,0q����� *ContractRate+    DealerSpotRate# �  $		 160Q�
 �� .R��� $��Center=�$��Right=�$��5,0,5,0q����� *DealerSpotRate+    ForwardPoint# �  $		 120Q�
 �� .R��� $
��Auto��$��Center=�$��Right=�$��5,0,5,0q����� *ForwardPoint+ �� Mode$ OneWay=�    
Instrument# �  $		 200Q�
 �� .R��� $��Center=�$
��Left=�$��5,0,5,0q����� *
Instrument+  InstrumnetIdToNameConverter# [    
 Status# �  
 �� .R��� $��Center=�$��Center=�$��5,0,5,0q����� *Status+  StatusDisplayConverter# [     HedgingDealID# �  $	 80Q�
 �� .R��� $��Center=�$��Right=����� *HedgingDealId+    LocalTradeDate# �  
 �� .R��� $��Center=�$��Center=�$��5,0,5,0q����� *LocalTradeDate+# [     CreationTime# �  $		 150Q�
 �� .R��� $��Center=�$��Center=�$��5,0,5,0q����� *CreationTime+  FormatDateTimeByBuConverter# [ �� $��1��$��1��$��0,8,20,8q�$��24��$��Right=� 	 Close#���    CommonButtonStle#��[   Caliburn.Micro.Message   Attach$$ [Event Click] = [Action Close]��