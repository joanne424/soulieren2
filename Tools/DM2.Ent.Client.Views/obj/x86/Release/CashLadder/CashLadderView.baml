   M S B A M L   `   `   `  ����   DM2.Ent.Client.Views)    #DM2.Ent.Client.Views.CashLadderView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity     CommonOnlyCloseWindowStyle#��[    � Title$  CashLadderView��$	��550��$
��1000��$
��Left=�$
��Left=�$	��Top=�$	��Top=� � 	Resources  �� 
TargetType �� Property �� Value��%.   '��       �� " ��.���  � ��$	 Top=�.��� $
��Left=�$	��Top=� � RowDefinitions �� $��40 ��� $��* ��� $��40 �.+��� $��0�� �� Orientation$ 
Horizontal=�$
��Left=�$��Center=����#F8F8F8.+�� $	��990�� � ColumnDefinitions �� $��1* ��� $��9* ��� $	��15* ��� $��9* ��� $	��13* ��� $	��12* ��� $	��14* ��� $��4* ��� $	��14* ��� $��12.5* ��� $
��1.5* �.+��� $��1��  BusinessUnit#���  $��Right=�$��Center=�$��0,0,5,0q���  �� DisplayMemberPath$
 Name��	 �� 
IsEditable	 . $��2��
 �� SelectedIndex$
 0������ *BusinessUnitList+ �� SelectedValue �� *BusinessUnit+.  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ SelectionChanged��.  (System.Windows.Interactivity.TriggerBase   Actions. 6  0System.Windows.Interactivity.InvokeCommandAction     Command �� *BuSelectionChangedCommand+�� $��3��  Counterparty#���  $��Right=�$��0,0,5,0q�$��Center=��� $��4��	 . $
 Name��$
 0������ *Counterpartys+ �� *Counterparty+�� $��5��  ValueDateFrom#���  $��Right=�$��0,0,5,0q�$��Center=�(  "System.Windows.Controls.DatePicker  $��6��  ClearButtonDatePicker#��[    Text �� *ValueDateFrom+ �� Mode$ OneWayToSource=�  DateTimeEmptyConverter �� 	Converter# [ �� $��7��  To#���  $��Center=�$��Right=�$��0,0,5,0q�  $��8��#��[  �� *ValueDateTo+$ OneWayToSource=�# [ �� $��9�� 
 Search#���    Caliburn.Micro.Message   Attach$% [Event Click] = [Action Search]��  CommonButtonStle#��[ �� $��1��$
��Auto=�$��Hidden=�$
��Auto��$
��Left=�$	��Top=�$
��Left=�$	��Top=�.���� $	��440��$
��Auto��$
��Left=�$	��Top=�$
��Left=�$	��Top=����� *CashLadderContent+�� $��2�� 		 Close#��� 	 $��Right=�$��	0,10,20,0q�$$ [Event Click] = [Action Close]��#��[ 