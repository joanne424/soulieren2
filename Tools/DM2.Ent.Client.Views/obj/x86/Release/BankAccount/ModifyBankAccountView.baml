   M S B A M L   `   `   `  ����   DM2.Ent.Client.Views0    *DM2.Ent.Client.Views.ModifyBankAccountView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org     CommonOnlyCloseWindowStyle#��[    ModifyBankAccount  � Title#  [ $	��530��$	��380�� � 	Resources  �� 
TargetType �� Property �� Value��%J   '��       �� " ��.���  � ��$ Center=���  � ��$ Right=�.��� .+��  � RowDefinitions �� $��30 ��� $
��Auto ��� $��50 �.+���   BankAccountInformation#���    
TitleStyle#��[ $��Center=�� $��1�����#F8F8F8 � ColumnDefinitions �� $��15 ��� $��80 ��� $	��220 ��� $��5 � �� $��10 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��1 ��� $��50 �.+��� $��*��$��1��$��1��$��16(�
���  ��$��0,0,42,0q���   BusinessUnit#���  $��1��$��1��$��0,0,5,0q��� {�.  $��2��$��1�� �� DisplayMemberPath$
 Name������ *BusinessUnitList+ �� SelectedValue �� *BusinessUnit+	 �� Mode$	 TwoWay=��� $��*��$��1��$��2��$��16(�
���  ��$��0,0,55,0q���   FinancialInstitution#���  $��2��$��1��$��0,0,5,0q��� {�.  $��2��$��2��$ FullName������ *FinancialInstitutions+ �� *FinancialInstitution+$	 TwoWay=��� $��*��$��1��$��3��$��16(�
���  ��$��	10,0,32,0q���   	AccountNo#���  $��1��$��3��$��0,0,5,0q��� $��2��$��3������ *	AccountNo+$	 TwoWay=�
 �� ValidatesOnDataErrors
 .  �� UpdateSourceTrigger$ PropertyChanged=��� $��*��$��4��$��1��$��16(�
���  ��$��0,0,55,0q���   AccountName#���  $��1��$��4��$��0,0,5,0q��� $��2��$��4������ *AccountName+$	 TwoWay=�
 . ��   BankAccountGroup#���  $��1��$��5��$��0,0,5,0q���  �� 
IsEditable . $��2��$��5���� $��*��$��1��$��6��$��16(�
���  ��$��0,0,32,0q���  	 CCY#��� 	 $��1��$��6��$��0,0,5,0q��� $��2��$��6��{�.  $
 Name������ *CurrencyList+ �� *SelectedCurrency+$	 TwoWay=���  
 CreationTime#��� 
 $��1��$��7��$��0,0,5,0q��� $��2��$��7��{�.  ���� *CreationTime+$	 TwoWay=�  FormatDateTimeByBuConverter �� 	Converter# [ ��   LastUpdateTime#���  $��1��$��8��$��0,0,5,0q��� $��2��$��8��{�.  ���� *LastUpdateTime+$	 TwoWay=�# [ �� $��4��$��9��
����İ�$��2q� �� CornerRadius$ 5����   Enabled#���  $��10��$��2��$
��Left=�$��Center=�$��20�� d� 	IsChecked �� *	Enabled+�� $��2�� �� Orientation$ 
Horizontal=�$��RightToLeft=�.+���  	 Close#���  $��90��$��26��$
��20,0q�  CommonButtonStle#��[   Caliburn.Micro.Message   Attach$" [Event Click]=[Action Close]����   Save#���  $��90��$��26��$
��20,0q�#��[ $! [Event Click]=[Action Save]��