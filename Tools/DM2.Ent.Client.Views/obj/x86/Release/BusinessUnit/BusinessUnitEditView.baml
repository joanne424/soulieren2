   M S B A M L   `   `   `  ���� V  RDM2.Ent.Client.Views, Version=1.0.6407.25883, Culture=neutral, PublicKeyToken=nullS.clr-namespace:DM2.Ent.Client.Views.ExtendClass DM2.Ent.Client.Views.ExtendClass  <    6DM2.Ent.Client.Views.BusinessUnit.BusinessUnitEditView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     FextendClass.clr-namespace:DM2.Ent.Client.Views.ExtendClass     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  $	��350��$	��700��  � WindowStartupLocation$  CenterScreen=�   CommonOnlyCloseWindowStyle#��[  .��� $
��20,0q� � RowDefinitions �� $	��220 ��� $��* �.+��� $��0�� �� Padding$
 0 10q����#F8F8F8$��0q�._�� $��Center=� �� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 � � ColumnDefinitions �� $	��100 ��� $��10 ��� $	��150 ��� $	��150 ��� $��10 ��� $	��150 �.+���  �� Orientation$ 
Horizontal=�$��0��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   BusinessUnit#���  �� $��0��$��2������ *Name+ �� Mode$ TwoWay=� �� ValidatesOnDataErrors . ��   BusinessUnitGroup#���  $��0��$��3���� $��0��$��5�� �� DisplayMemberPath$
 Name�� �� SelectedValuePath$ Id������ *BusinessUnitGroups+	 �� SelectedValue	 �� *	GroupId+�� $ 
Horizontal=�$��1��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   Country#���  �� $��1��$��2��$ FullName��$ Id������ *	Countries+	 �� *	CountryId+$ TwoWay=� . �� $ 
Horizontal=�$��1��$��3��.+��� $��*��
���  ��$��0,4,5,0q���   LocalCCY#���  �� $��1��$��5��{�.  $
 Name��$ Id������ *
Currencies+	 �� *
LocalCcyId+$ TwoWay=� . �� $ 
Horizontal=�$��0��$��2��.+��� $��*��
���  ��$��0,4,5,0q���   TimeZone#���  �� $��2��$��2��{�.  1   +DM2.Ent.Client.Views.ExtendClass.EnumBinder
   Path$H
 BInfrastructure.Common.dll/Infrastructure.Common.Enums.TimeZoneEnum�� �� SelectedIndex$ 1��	 �� *
TimeZone+$ TwoWay=��� $ 
Horizontal=�$��2��$��3��.+��� $��*��
���  ��$��0,4,5,0q���   
DateFormat#���  �� $��2��$��5��$J
 DInfrastructure.Common.dll/Infrastructure.Common.Enums.DateFormatEnum��$ 1��	 �� *
DateFormat+$ TwoWay=���   CreationTime#���  $��3��$��0���� $��3��$��2�� �� TextAlignment$ Right=�{�.  ���� *CreationTime+$ TwoWay=�  FormatDateTimeByBuConverter �� 	Converter# [ ��  	 LastUpdateTime#��� 	 $��3��$��3���� $��3��$��5��$ Right=�{�.  ���� *LastUpdateTime+$ TwoWay=�# [ ��  
 RegionId#��� 
 $��4��$��0���� $��4��$��2��$
 Name��$ Id���� $ 
Horizontal=�$��1��$��Right=�$��Bottom=�.+���   Save#���    Caliburn.Micro.Message   Attach$( "[Event Click] = [Action OnSaved()]��  CommonButtonStle#��[ $��Right=��� $��15����  	 Close#���  $$ [Event Click] = [Action Close]��#��[ $��Right=�