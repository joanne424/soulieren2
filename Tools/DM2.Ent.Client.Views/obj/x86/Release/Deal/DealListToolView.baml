   M S B A M L   `   `   `  ����   DM2.Ent.Client.Views+    %DM2.Ent.Client.Views.DealListToolView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org    � 	Resources     textCellRight �� 
TargetType �� Property �� Value  textCellLeft�� %�   &	        &	 O     �� " ��.���  �@��TextAlignment$ Right=���  � ��$ 5,0,5,0q��� " ��.���  �@��TextAlignment$
 Left=���  � ��$ 5,0,5,0q�.��� $
��Left=�$	��Top=�.+�&   System.Windows.Controls.DataGrid  -     Name$ TB��$	��500��  Caliburn.Micro.Message   Attach$@ :[Event MouseDoubleClick] = [Action ModifyDeal_DoubleClick]��   CanUserSortColumns . $��Stretch=�$��Stretch=����� *
DealList+ �� SelectedItem �� *
DealItem+   Columns 0  *System.Windows.Controls.DataGridTextColumn   
 DealID,  &System.Windows.Controls.DataGridColumn	   Header#	 �  
   CanUserSort
 .    Width5  /System.Windows.Controls.DataGridLengthConverter$ 2* 1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [     Binding �� *Id+    Counterparty#	 �  $ 5* # [  �� *CounterpartyId+ ! CounterpartyIdToNameConverter �� 	Converter# [    
 BuyCCY#	 �  $ 2*  �� *BuyCCY+  CurrencyIdToNameConverter# [     	BuyAmount#	 �  $ 3* # [     
Foreground �� *	BuyAmount+ $  DeficitAmountForegroundConverter# [  `�  #	 FormatAmountByCurrencyConverter `� 	Converter# [	 .1���  �� Path$ 	BuyAmount��� $ BuyCCY�   
 SellCCY#	 � 
 $ 2*  �� *	SellCCY+# [     
SellAmount#	 �  $ 3* # [   �� *
SellAmount+# [  `� # [	 .1��� $ 
SellAmount��� $ SellCCY�    ContractRate#	 �  $ 3* # [   �� *ContractRate+