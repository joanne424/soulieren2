   M S B A M L   `   `   `  ���� Q  MBanclogix.Controls.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW@clr-namespace:Banclogix.Controls;assembly=Banclogix.Controls.WPFBanclogix.Controls   DM2.Ent.Client.Views5   /DM2.Ent.Client.Views.ModifyHedgeSpotForwardView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  Ncontrol@clr-namespace:Banclogix.Controls;assembly=Banclogix.Controls.WPF  X	 TXceed.Wpf.Toolkit, Version=2.1.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit 	 $	��570��$	��600��   CommonOnlyCloseWindowStyle#��[    � Title  �� *Title+ � 	Resources  �� 
TargetType �� Property �� Value��%e   '��       �� " ��.���  � ��$ Right=���  � ��$ Center=���  � ��$ 0,0,5,0q�.���  � RowDefinitions �� $��* ��� $��40 �.+��� ���#F8F8F8$��0q�._��  �� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $	��120 � � ColumnDefinitions �� $
��4.5* ��� $	��10* ��� $
��4.5* ��� $	��10* �.+���   BusinessUnit#���  $��0��$��0���� $	��165��$��0,0,30,0q� �� TextWrapping$
 Wrap=�$��24��{�.  $��0��$��1������ *BusinessUnitId+  BUIDToNameConverter �� 	Converter# [ ��   Counterparty#���  $��0��$��2���� $	��165��$��0,0,30,0q�$
 Wrap=�$��24��{�.  $��0��$��3������ *CounterpartyId+ ! CounterpartyIdToNameConverter# [ ��  
 DealID#���  $��1��$��0���� $	��165��$��0,0,30,0q�$
 Wrap=�$��24��{�.  $��1��$��1������ *Id+��   HedgingDealID#���  $��1��$��2���� $	��165��$��0,0,30,0q�$
 Wrap=�$��24��{�.  $��1��$��3������ *HedgingDealId+��  
 BuyCCY#���  $��2��$��0���� $	��165��$��0,0,30,0q�$
 Wrap=�$��24��{�.  $��2��$��1������ *BuyCCY+	 �� Mode$	 OneWay=�  CurrencyIdToNameConverter# [ ��  
	 Amount#��� 	 $��2��$��2���� $��2��$��3��$	��165��$��0,0,30,0q�$
 Wrap=�
 �� TextAlignment$
 Right=�{�.  .����`�  #
 FormatAmountByCurrencyConverter `� 	Converter# [
 .1���  �� Path$ 	BuyAmount�$	 OneWay=��� $ BuyCCY�$	 OneWay=�.����   SellCCY#���  $��3��$��0���� $	��165��$��0,0,30,0q�$
 Wrap=�$��24��{�.  $��3��$��1������ *	SellCCY+$	 OneWay=�# [ �� #��� 	 $��3��$��2���� $��3��$��3��$	��165��$��0,0,30,0q�$
 Wrap=�$
 Right=�{�.  .����`� # [
 .1��� $ 
SellAmount�$	 OneWay=��� $ SellCCY�$	 OneWay=�.����   DealerSpotRate#���  $��4��$��0���� -   $��SpotRate��$��4��$��1��$��20��$	��165��$��0,0,30,0q�{�.  $
 Right=����� *DealerSpotRate+��   ForwardPoint#���  $��4��$��2���� -   $��ForwardPoint��$��4��$��3��$��20��$	��165��$��0,0,30,0q�{�.  $
 Right=����� *ForwardPoint+��   ContractRate#���  $��5��$��0���� -   $��ContractRate��$��5��$��1��$��20��$	��165��$��0,0,30,0q�{�.  $
 Right=����� *ContractRate+��   
Instrument#���  $��5��$��2���� $	��165��$��0,0,30,0q�$
 Wrap=�$��24��{�.  $��5��$��3������ *
Instrument+  InstrumnetIdToNameConverter# [ ��   LocalTradeDate#���  $��6��$��0���� {�.  $��24��$	��165��$��0,0,30,0q�$��6��$��1������ *LocalTradeDate+  FormatDateByBuConverter# [ ��   	ValueDate#���  $��6��$��2���� $��24��$	��165��$��0,0,30,0q�$��6��$��3��{�.  ���� *	ValueDate+# [ ��  
 Status#���  $��7��$��0���� $��7��$��1��$��20��$	��165��$��0,0,30,0q�{�.  ���� *Status+  StatusDisplayConverter# [ ��   CreationTime#���  $��7��$��2���� $	��165��$��0,0,30,0q�$
 Wrap=�$��24��{�.  $��7��$��3������ *CreationTime+  FormatDateTimeByBuConverter# [ ��   User#���  $��8��$��0���� $��8��$��1��$��20��$	��165��$��0,0,30,0q�{�.  ���� *UserId+  UserIdToNameConverter# [ ��  	 Staff#���  $��8��$��2���� $	��165��$��0,0,30,0q�$
 Wrap=�$��24��{�.  $�� ��$��8��$��3����   Comment#���  $	��Top=�$��	10,10,0,0q�$��9��$��0���� $��3��$��1��$��80��$	��500��$��	0,10,30,0q�$��9��$	��Top=�{�.  $
 Wrap=����� *	Comment+��   Confirm#���    btnPage#��[ $��1��$y�	Collapsed=�  Caliburn.Micro.Message   Attach$. ([Event Click] = [Action Confirm_Click()]��$��Right=�$	��Top=�$�� 0,10,150,0q���  	 Close#���    CommonButtonStle#��[ $��Right=�$	��Top=�$��	 0,7,20,0q�$- '[Event Click] = [Action Cancel_Click()]��$��1��