   M S B A M L   `   `   `  ����   DM2.Ent.Client.Views@    :DM2.Ent.Client.Views.Counterparty.NewCounterpartyGroupView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  $	��310��$	��420��  � WindowStartupLocation$  CenterScreen=�   CommonOnlyCloseWindowStyle#��[  .��� $��5q� � RowDefinitions �� $��35 ��� $	��140 ��� $��* �.+���    CounterpartyGroupInformation#���    
TitleStyle#��[ $	��Top=�$��0���� $��1�� �� Padding$
 0 10q����#F8F8F8$��0q�._��  �� $��40 ��� $��40 ��� $��40 � � ColumnDefinitions �� $	��120 ��� $��10 ��� $	��220 ��� .+���  �� Orientation$ 
Horizontal=�$��Right=�$��0��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   BusinessUnit#���  �� $��0��$��2�� �� DisplayMemberPath$
 Name�� �� SelectedValuePath$ Id��  Caliburn.Micro.Message   Attach$? 9[Event SelectionChanged]=[Action OnBusinessUnitChanged()]������ *BusinessUnits+ �� SelectedValue �� *BusinessUnitId+	 �� ValidatesOnDataErrors	 . 
 �� Mode$
 TwoWay=��� $ 
Horizontal=�$��Right=�$��1��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   CounterpartyGroup#���  �� $��1��$��2������  �� Path$
 Name�	 . $
 TwoWay=���   ParentGroup#���  $��Right=�$��2��$��0���� $��2��$��2��$
 Name��$ Id������ *	AllGroups+$
 TwoWay=� �� *ParentGroupId+	 . $
 TwoWay=��� $ 
Horizontal=�$��2��$��Right=�$��Bottom=�.+���   Save#���  $( "[Event Click] = [Action OnSaved()]��  CommonButtonStle#��[ $��Right=��� $��15����  	 Close#���  $$ [Event Click] = [Action Close]��#��[ $��Right=�