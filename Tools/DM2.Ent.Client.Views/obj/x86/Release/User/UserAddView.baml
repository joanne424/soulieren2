   M S B A M L   `   `   `  ����   DM2.Ent.Client.Views+    %DM2.Ent.Client.Views.User.UserAddView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  $	��500��$	��670��  � WindowStartupLocation$  CenterScreen=�   CommonOnlyCloseWindowStyle#��[  .��� $��5q� � RowDefinitions �� $��35 ��� $	��330 ��� �� $��40 �.+���   UserInformation#���    
TitleStyle#��[ $	��Top=��� $��1�� �� Padding$
 0 10q����#F8F8F8$��0q�._�� $��Center=� �� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��10 ��� $	��120 � � ColumnDefinitions �� $	��100 ��� $��10 ��� $	��150 ��� $	��150 ��� $��10 ��� $	��150 �.+���  �� Orientation$ 
Horizontal=�$��0��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   BusinessUnit#���  �� $��0��$��2�� �� DisplayMemberPath$
 Name�� �� SelectedValuePath$ Id������ *BusinessUnits+ �� SelectedValue �� *BuId+ �� ValidatesOnDataErrors . 	 �� Mode$	 TwoWay=���   	UserGroup#���  $��0��$��3���� $��0��$��5��$ Value��$	 Key������ *
UserGroups+ �� *	GroupId+$	 TwoWay=��� $ 
Horizontal=�$��1��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   	LoginName#���  �� $��1��$��2������ *	LoginName+ . $	 TwoWay=��� $ 
Horizontal=�$��1��$��3��.+��� $��*��
���  ��$��0,4,5,0q���   Password#���  �� $��1��$��5��$��Center=�$
��Left=�$��******���� $ 
Horizontal=�$��2��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   	FirstName#���  �� $��2��$��2������ *	FirstName+ . $	 TwoWay=��� $ 
Horizontal=�$��2��$��3��.+��� $��*��
���  ��$��0,4,5,0q���   LastName#���  �� $��2��$��5������ *
LastName+ . $	 TwoWay=��� $ 
Horizontal=�$��3��$��0��.+��� $��*��
���  ��$��0,4,5,0q���  	 UserRole#��� 	 �� $��3��$��2��$ Value��$	 Key������ *	UserRoles+ �� *RoleId+ . $	 TwoWay=���  	
 Email#��� 
 $��3��$��3���� $��3��$��5������ *Email+ . $	 TwoWay=���   CounterpartyGroup#���  $	��Top=�
 �� Padding$	
 0,5q�$��5��$��0��&   System.Windows.Controls.DataGrid  $��5��$��2��$��4��  DataGridRowStyle   RowStyle# [   DataGridColumnHeaderStyle   ColumnHeaderStyle# [ ���� *CounterpartyGroups+$	 TwoWay=�   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ 2*    MinWidth$ 90Q�   HeaderTemplate �� .R���  ��Name$ AllIsChecked��  All#���   d� 	IsChecked �� *DataContext.IsAllChecked+$	 TwoWay=� �� RelativeSource ��  �� AncestorType$ WindowL� �� Mode$ FindAncestor=�.  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ Click��.  (System.Windows.Interactivity.TriggerBase   Actions. "  Caliburn.Micro.ActionMessage     
MethodName$ OnAllChecked��   
Parameters. 	  Caliburn.Micro.Parameter	   	  Value �� *	IsChecked+$	 TwoWay=� �� ElementName$ AllIsChecked��   CellTemplate �� .R��� $��Center=� �� *	IsChecked+   $ Click��.   $ 	OnChecked��. 	   �� *	GroupId+0
  *System.Windows.Controls.DataGridTextColumn
  $ 4*    Header# �    DataGridCellLeftStyle   	CellStyle# [ 1  +System.Windows.Controls.DataGridBoundColumn    Binding  �� *	GroupName+
  $ 4* # �  # [   �� *BusinessUnitName+�� $ 
Horizontal=�$��3��$��Bottom=�.+���   Save#���    Caliburn.Micro.Message!   Attach$(! "[Event Click] = [Action OnSaved()]��  CommonButtonStle#��[ $��Right=��� $��15����  	 Close#���  $$! [Event Click] = [Action Close]��#��[ $��Right=�