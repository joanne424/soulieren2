   M S B A M L   `   `   `  ���� Q  MBanclogix.Controls.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW@clr-namespace:Banclogix.Controls;assembly=Banclogix.Controls.WPFBanclogix.Controls   DM2.Ent.Client.Views.   (DM2.Ent.Client.Views.NewInternalDealView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  Ncontrol@clr-namespace:Banclogix.Controls;assembly=Banclogix.Controls.WPF  X	 TXceed.Wpf.Toolkit, Version=2.1.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit 	 $	��590��$	��680��   CommonOnlyCloseWindowStyle#��[    NewInternalDeal  � Title#  �   � 	Resources   dealtAmountTip  DealtAmount��%R   &	       _� .���� .+��� #���  �� $&�� Mark which Amount is DealtAmount��.���  � RowDefinitions �� $��* ��� $��50 �.+��� ���#F8F8F8$��0q�._��  �� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $	��220 � � ColumnDefinitions �� $��6* ��� $	��10* ��� $��6* ��� $	��10* �.+���   BusinessUnit1#���  $��Center=�$��0,0,10,0q�$��Right=�$��0��$��0����  �� DisplayMemberPath$
 Name�� �� 
IsEditable . $��24��$	��185��$��0,0,20,0q�$��0��$��1������ *CounterPartys+ �� SelectedValue �� *CounterParty+ �� ValidatesOnDataErrors . .  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger  	   	EventName$	 SelectionChanged��.  (System.Windows.Interactivity.TriggerBase
   Actions.
 6  0System.Windows.Interactivity.InvokeCommandAction     Command �� *%#CounterPartySelectionChangedCommand+��   BusinessUnit2#���  $��Center=�$��0,0,10,0q�$��Right=�$��0��$��2���� $	��185��$��0,0,20,0q� �� TextWrapping$
 Wrap=�$��24��$��0��$��3������ *HedgingDealId+ .  �� Mode$ TwoWay=���   BankAccount1#���  $��Center=�$��0,0,10,0q�$��Right=�$��1��$��0���� $
 Name�� . $��24��$	��185��$��0,0,20,0q�$��1��$��1������ *CounterPartys+ �� *CounterParty+ .    $	 SelectionChanged��.
    �� *%#CounterPartySelectionChangedCommand+��   BankAccount2#���  $��Center=�$��0,0,10,0q�$��Right=�$��1��$��2���� $	��185��$��0,0,20,0q�$
 Wrap=�$��24��$��1��$��3������ *HedgingDealId+ . $ TwoWay=���  
 BuyCCY#���  $��Center=�$��0,0,10,0q�$��2��$��Right=�$��0��� $��2��$��1��$	��185��$
��Left=� �� $��60 ��� $	��125 �.+��� {�. $��Center=�$
��Left=�$
 Name�� �� SelectedValuePath$ Id��$��24��$��60��$��0,0,0,0q����� *	CCYList+ �� SelectedIndex �� *BuyCCYIndex+ �� *
CustBuyCCY+ .    $	 SelectionChanged��.
    �� *$"BuyCurrencySelectionChangedCommand+#   Banclogix.Controls.NumericBox  $��1��$��Center=�$
��Left=�$��21��$	��122��$��3,0,0,0q�$
 Wrap=� �� TextAlignment$ Right=�  Caliburn.Micro.Message   Attach$1 +[Event LostFocus]=[Action FocusedBuyAmount]��   Amount �� *CustBuyAmount+$ TwoWay=� . {��� *IsCustBuyAmountEnable+$ OneWay=�   Decimals �� *
CustBuyCCY+ 	 CurrencyIdToAmountDecimals �� 	Converter# [	 �� #���  $��Center=�$��0,0,10,0q�$��2��$��2��$��Right=�� $��2��$��3��$	��185��$
��Left=� �� $��60 ��� $	��125 �.+��� {�.  $��Center=�$
��Left=�$��24��$��60��$��0,0,0,0q�.���� $��1��$	��122��$��3,0,0,0q�$
 Wrap=�$ Right=�{�.  .����`�  #
 FormatAmountByCurrencyConverter `� 	Converter# [
 .1���  �� Path$ 
SellAmount�$ OneWay=��� $ SellCCY�$ OneWay=�.����   SellCCY#���  $��Center=�$��0,0,10,0q�$��3��$��Right=�$��0��� $��3��$��1��$	��185��$
��Left=� �� $��60 ��� $	��125 �.+��� {�. $��Center=�$
��Left=�$
 Name��$ Id��$��24��$��60��$��0,0,0,0q����� *	CCYList+ �� *BuyCCYIndex+ �� *
CustBuyCCY+ .    $	 SelectionChanged��.
    �� *$"BuyCurrencySelectionChangedCommand+  $��1��$��Center=�$
��Left=�$��21��$	��122��$��3,0,0,0q�$
 Wrap=�$ Right=�$1 +[Event LostFocus]=[Action FocusedBuyAmount]�� �� *CustBuyAmount+$ TwoWay=� . {��� *IsCustBuyAmountEnable+$ OneWay=� �� *
CustBuyCCY+# [	 �� #���  $��Center=�$��0,0,10,0q�$��3��$��2��$��Right=�� $��3��$��3��$	��185��$
��Left=� �� $��60 ��� $	��125 �.+��� {�.  $��Center=�$
��Left=�$��24��$��60��$��0,0,0,0q�.���� $��1��$	��122��$��3,0,0,0q�$
 Wrap=�$ Right=�{�.  .����`� # [
 .1��� $ 
SellAmount�$ OneWay=��� $ SellCCY�$ OneWay=�.����   ContractRate#���  $��Center=�$��0,0,10,0q�$��Right=�$��4��$��0��+   %Banclogix.Controls.NotPointNumericBox  -   $��SpotRate��$��4��$��1��$��20��$	��185��$��0,0,20,0q�   IsAllowNegative .  $ Right=�   Text �� *
SpotRate+ . $ TwoWay=��� #���  $��Center=�$��0,0,10,0q�$��Right=�$��5��$��0��  -   $��ContractRate��$��5��$��1��$��20��$	��185��$��0,0,20,0q�{�.  $ Right=� .   �� *
OpenRate+ . $ TwoWay=���   
Instrument#���  $��Center=�$��0,0,10,0q�$��Right=�$��5��$��2���� $��24��{�. $	 Key��$ Value��$	��185��$��0,0,20,0q�$��5��$��3������ *EntInstrument+ �� *
Instrument+ . ��   LocalTradeDate#���  $��Center=�$��0,0,10,0q�$��Right=�$��6��$��0��(  "System.Windows.Controls.DatePicker     SelectedDateFormat$ Short=�$��24��$	��185��$��0,0,20,0q�$��6��$��1��   Text �� *LocalTradeDate+$ TwoWay=� .  �� UpdateSourceTrigger$ PropertyChanged=���   	ValueDate#���  $��Center=�$��0,0,10,0q�$��6��$��Right=�$��2��  $��24��$	��185��$��0,0,20,0q�$��6��$��3��$ Short=� �� *	ValueDate+$ TwoWay=� . $ PropertyChanged=���   Comment#���  $��Center=�$��	10,0,10,0q�$��7��$��Right=�$��0���� $��3��$��1��$	��160��$	��510��$��	0,10,20,0q�$��7��$
 Wrap=�$	��Top=�$
��Left=����� *	Comment+��   Confirm#���    CommonButtonStle#��[ $��1��$. ([Event Click] = [Action Confirm_Click()]��$��Right=�$	��Top=�$�� 0,10,150,0q���  	 Close#���  #��[ $��Right=�$	��Top=�$��
 0,10,20,0q�$- '[Event Click] = [Action Cancel_Click()]��$��1��