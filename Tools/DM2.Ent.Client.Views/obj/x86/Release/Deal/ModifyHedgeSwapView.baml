   M S B A M L   `   `   `  ���� Q  MBanclogix.Controls.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW@clr-namespace:Banclogix.Controls;assembly=Banclogix.Controls.WPFBanclogix.Controls   DM2.Ent.Client.Views.   (DM2.Ent.Client.Views.ModifyHedgeSwapView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  Ncontrol@clr-namespace:Banclogix.Controls;assembly=Banclogix.Controls.WPF  $	��760��$	��680��   CommonOnlyCloseWindowStyle#��[    NewHedgeSwapDeal  � Title#  �   � 	Resources   dealtAmountTip  DealtAmount��%R   &	       _� .���� .+��� #���  �� $&�� Mark which Amount is DealtAmount��.���  � RowDefinitions �� $	��195 ��� $��40 ��� $	��180 ��� $��40 ��� $	��180 ��� $��40 �.+��� ���#F8F8F8$��0q�._��  �� $��35 ��� $��35 ��� $��35 ��� $��35 ��� $��40 � � ColumnDefinitions �� $��6* ��� $	��10* ��� $��6* ��� $	��10* �.+���   BusinessUnit#���  $��Center=�$��0,0,10,0q�$��Right=�$��0��$��0���� $	��165��$��10,0,0,0q�$
��Left=� �� TextWrapping$
 Wrap=�$��24��{�.  $��0��$��1������ *NearDeal.BusinessUnitId+  BUIDToNameConverter �� 	Converter# [  �� Mode$ OneWay=���   Counterparty#���  $��Center=�$��0,0,10,0q�$��Right=�$��0��$��2���� $	��165��$��10,0,0,0q�$
��Left=�$
 Wrap=�$��24��{�.  $��0��$��3������ *NearDeal.CounterpartyId+ ! CounterpartyIdToNameConverter# [ $ OneWay=���   HedgingDealID#���  $��Center=�$��0,0,10,0q�$��Right=�$��1��$��0���� $	��165��$��10,0,0,0q�$
��Left=�$
 Wrap=�{�.  $��1��$��1������ *NearDeal.HedgingDealId+ �� ValidatesOnDataErrors . $ OneWay=���  	 LocalTradeDate#��� 	 $��Center=�$��0,0,10,0q�$��1��$��Right=�$��2���� $��24��$	��165��$��10,0,0,0q�$
��Left=�$��1��$��3��{�.  ���� *NearDeal.LocalTradeDate+ 
 FormatDateByBuConverter# [
 $ OneWay=���   User#���  $��0,0,10,0q�$��Center=�$��Right=�$��2��$��0���� $��2��$��1��$��20��$	��165��$��10,0,0,0q�$
��Left=�{�.  ���� *NearDeal.UserId+  UserIdToNameConverter# [ $ OneWay=���  	 Staff#���  $��Right=�$��0,0,10,0q�$��Center=�$��2��$��2���� $	��165��$��10,0,0,0q�$
��Left=�$
 Wrap=�$��24��{�.  $�� ��$��2��$��3����   CreationTime#���  $��Center=�$��0,0,10,0q�$��3��$��Right=�$��0���� $��24��$	��165��$��10,0,0,0q�$
��Left=�$��3��$��1��{�.  ���� *NearDeal.CreationTime+  FormatDateTimeByBuConverter# [ $ OneWay=���   Comment#���  $��Center=�$��	10,0,10,0q�$��4��$��Right=�$��0���� {�.  $��3��$��1��$��39��$��	10,0,30,0q�$��4��$
 Wrap=�$��Center=����� *NearDeal.Comment+$ OneWay=���   NearLegInfo#���  $��1��  
TitleStyle#��[ $��Center=�$��15,0,0,0q��� ���#F8F8F8$��0q�$��2��._��  �� $��35 ��� $��35 ��� $��35 ��� $��35 ��� $��35 � �� $��6* ��� $	��10* ��� $��6* ��� $	��10* �.+���  
 BuyCCY#���  $��Center=�$��0,0,10,0q�$��0��$��Right=�$��0��� $��0��$��1��$	��165��$
��Left=�$��10,0,0,0q� �� $��50 ��� $	��115 �.+��� {�.  $��Center=�$
��Left=�$��50��$��0,0,0,0q����� *NearDeal.BuyCCY+  CurrencyIdToNameConverter# [ $ OneWay=��� $��1��$	��112��$��3,0,0,0q�$
 Wrap=� �� TextAlignment$ Right=�{�.  .����`�  # FormatAmountByCurrencyConverter	 `� 	Converter#	 [ .1��� 
 �� Path$
 NearDeal.BuyAmount�$ OneWay=��� $
 NearDeal.BuyCCY�$ OneWay=�.����   SellCCY#���  $��Center=�$��0,0,10,0q�$��0��$��Right=�$��2��� $��0��$��3��$	��165��$
��Left=�$��10,0,0,0q� �� $��50 ��� $	��115 �.+��� {�.  $��Center=�$
��Left=�$��50��$��0,0,0,0q����� *NearDeal.SellCCY+# [ $ OneWay=��� $��1��$	��112��$��3,0,0,0q�$
 Wrap=�$ Right=�{�.  .����`� #	 [ .1��� $
 NearDeal.SellAmount�$ OneWay=��� $
 NearDeal.SellCCY�$ OneWay=�.����   DealerSpotRate#���  $��Center=�$��0,0,10,0q�$��Right=�$��1��$��0���� -   $��SpotRate��$��1��$��1��$��20��$	��165��$��10,0,0,0q�{�.  $ Right=�$
��Left=����� *NearDeal.DealerSpotRate+$ OneWay=���   ForwardPoint#���  $��Center=�$��0,0,10,0q�$��Right=�$��1��$��2���� -   $��ForwardPoint��$��1��$��3��$��20��$	��165��$��10,0,0,0q�{�.  $ Right=�$
��Left=����� *NearDeal.ForwardPoint+$ OneWay=���   ContractRate#���  $��Center=�$��0,0,10,0q�$��Right=�$��2��$��0���� -   $��ContractRate��$��2��$��1��$��20��$	��165��$��10,0,0,0q�{�.  $ Right=�$
��Left=����� *NearDeal.ContractRate+��   
Instrument#���  $��Center=�$��0,0,10,0q�$��Right=�$��2��$��2���� $	��165��$��10,0,0,0q�$
 Wrap=�$��24��{�.  $
��Left=�$��2��$��3������ *NearDeal.Instrument+  InstrumnetIdToNameConverter# [ $ OneWay=���  
 DealID#���  $��Center=�$��0,0,10,0q�$��Right=�$��3��$��0���� $	��165��$��10,0,0,0q�$
 Wrap=�$
��Left=�{�.  $��3��$��1������ *NearDeal.Id+ . $ OneWay=���   HedgingExecutionID#���  $��Center=�$��0,0,10,0q�$��Right=�$��3��$��2���� $	��165��$��10,0,0,0q�$
 Wrap=�$
��Left=�{�.  $��3��$��3������ *NearDeal.HedgingExecutionId+ . $ OneWay=���  
 Status#���  $��Center=�$��0,0,10,0q�$��4��$��Right=�$��0���� $��24��$	��165��$��10,0,0,0q�$��4��$��1��{�.  $
��Left=����� *NearDeal.Status+  StatusDisplayConverter# [ $ OneWay=���    	ValueDate#���   $��Center=�$��0,0,10,0q�$��4��$��Right=�$��2���� $��24��$	��165��$��10,0,0,0q�$��4��$��3��{�.  $
��Left=����� *NearDeal.ValueDate+# [
 $ OneWay=���  ! FarInfo#��� ! $��3��#��[ $��Center=�$��15,0,0,0q��� ���#F8F8F8$��0q�$��4��._��  �� $��35 ��� $��35 ��� $��35 ��� $��35 ��� $��35 � �� $��6* ��� $	��10* ��� $��6* ��� $	��10* �.+��� #���  $��Center=�$��0,0,10,0q�$��0��$��Right=�$��0��� $��0��$��1��$	��165��$
��Left=�$��10,0,0,0q� �� $��50 ��� $	��115 �.+��� {�.  $��Center=�$
��Left=�$��50��$��0,0,0,0q����� *FarDeal.BuyCCY+# [ $ OneWay=��� $��1��$	��112��$��3,0,0,0q�$
 Wrap=�$ Right=�{�.  .����`� #	 [ .1��� $
 FarDeal.BuyAmount�$ OneWay=��� $
 FarDeal.BuyCCY�$ OneWay=�.���� #���  $��Center=�$��0,0,10,0q�$��0��$��Right=�$��2��� $��0��$��3��$	��165��$
��Left=�$��10,0,0,0q� �� $��50 ��� $	��115 �.+��� {�.  $��Center=�$
��Left=�$��50��$��0,0,0,0q����� *FarDeal.SellCCY+# [ $ OneWay=��� $��1��$	��112��$��3,0,0,0q�$
 Wrap=�$ Right=�{�.  .����`� #	 [ .1��� $
 FarDeal.SellAmount�$ OneWay=��� $
 FarDeal.SellCCY�$ OneWay=�.���� #���  $��Center=�$��0,0,10,0q�$��Right=�$��1��$��0���� $��1��$��1��$��20��$	��165��$��10,0,0,0q�{�.  $ Right=�$
��Left=����� *FarDeal.DealerSpotRate+$ OneWay=��� #���  $��Center=�$��0,0,10,0q�$��Right=�$��1��$��2���� $��1��$��3��$��20��$	��165��$��10,0,0,0q�{�.  $ Right=�$
��Left=����� *FarDeal.ForwardPoint+$ OneWay=��� #���  $��Center=�$��0,0,10,0q�$��Right=�$��2��$��0���� $��2��$��1��$��20��$	��165��$��10,0,0,0q�{�.  $ Right=�$
��Left=����� *FarDeal.ContractRate+�� #���  $��Center=�$��0,0,10,0q�$��Right=�$��2��$��2���� $	��165��$��10,0,0,0q�$
 Wrap=�$��24��{�.  $
��Left=�$��2��$��3������ *FarDeal.Instrument+# [ $ OneWay=��� #���  $��Center=�$��0,0,10,0q�$��Right=�$��3��$��0���� $	��165��$��10,0,0,0q�$
 Wrap=�$
��Left=�{�.  $��3��$��1������ *
FarDeal.Id+ . $ OneWay=��� #���  $��Center=�$��0,0,10,0q�$��Right=�$��3��$��2���� $	��165��$��10,0,0,0q�$
 Wrap=�$
��Left=�{�.  $��3��$��3������ *FarDeal.HedgingExecutionId+ . $ OneWay=��� #���  $��Center=�$��0,0,10,0q�$��4��$��Right=�$��0���� $��24��$	��165��$��10,0,0,0q�$��4��$��1��{�.  $
��Left=����� *FarDeal.Status+# [ $ OneWay=��� #���   $��Center=�$��0,0,10,0q�$��4��$��Right=�$��2���� $��24��$	��165��$��10,0,0,0q�$��4��$��3��{�.  $
��Left=����� *FarDeal.ValueDate+# [
 $ OneWay=���  	" Close#��� "  # CommonButtonStle#��[# $��Right=�$��Center=�$��
 0,10,20,0q�  Caliburn.Micro.Message   Attach$- '[Event Click] = [Action Cancel_Click()]��$��5��