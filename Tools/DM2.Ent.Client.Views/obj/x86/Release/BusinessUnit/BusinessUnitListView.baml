   M S B A M L   `   `   `  ����   DM2.Ent.Client.Views<    6DM2.Ent.Client.Views.BusinessUnit.BusinessUnitListView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org     BusinessUnitListTitle  � Title#  �   $	��650��$
��1000�� � WindowStartupLocation$ CenterScreen=� ! NormalWindowWithMaximizeStyle#��[ .���  � RowDefinitions �� $��50 ��� $��15 ��� $��* ��� $��40 �.+��� $��0�����#F8F8F8$��0q�._�� $��Center=� � ColumnDefinitions �� $	��100 ��� $��10 ��� $	��240 ��� $��* ��� $��20 �.+���   BusinessUnit#���  $��0��$��0���� $��0��$��2�� �� 
IsEditable .  �� DisplayMemberPath$
 Name�� �� SelectedValuePath$ Id������ *BusinessUnits+ �� Mode$ TwoWay=� �� SelectedValue �� *SearchBusinessUnitId+$ TwoWay=��� $��0��$��3��	 �� Orientation$	 
Horizontal=�$��Right=�.+���  
 Search#���    CommonButtonStle#��[   Caliburn.Micro.Message
   Attach$%
 [Event Click] = [Action Search]���� $��15����   New#���  #��[ $"
 [Event Click] = [Action New]��&   System.Windows.Controls.DataGrid  $��2��$   HorizontalScrollBarVisibility$
 Auto=�"   VerticalScrollBarVisibility$
 Auto=�$.
 ([Event MouseDoubleClick] = [Action Edit]������ *BusinessUnitList+ �� SelectedItem �� *SelectedBusinessUnit+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ 3*    Header# �    DataGridCellLeftStyle   	CellStyle# [ 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Name+  $ 2*   Country# �  # [  �� *	CountryId+  CountryIdToNameConverter �� 	Converter# [   $ 2*  	 LocalCCY# � 	  �� *
LocalCcyId+ 
 CurrencyIdToNameConverter# [
   $ 2*   TimeZone# �  # [  �� *
TimeZone+  TimeZoneDisplayConverter# [   $ 3*   BusinessUnitGroup# �  # [  �� *	GroupId+  $ 3*   CreationTime# �   �� *CreationTime+  FormatDateTimeByBuConverter# [   $ 3*   LastUpdateTime# �   �� *LastUpdateTime+# [ �� $��3�� 	 Close#���  $$
 [Event Click] = [Action Close]��#��[ $��Right=�