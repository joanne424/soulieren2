   M S B A M L   `   `   `  ���� V  RDM2.Ent.Client.Views, Version=1.0.6407.25883, Culture=neutral, PublicKeyToken=nullS.clr-namespace:DM2.Ent.Client.Views.ExtendClass DM2.Ent.Client.Views.ExtendClass  <    6DM2.Ent.Client.Views.Counterparty.CounterpartyEditView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     FextendClass.clr-namespace:DM2.Ent.Client.Views.ExtendClass     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  $	��500��$	��700��  � WindowStartupLocation$  CenterScreen=�   CommonOnlyCloseWindowStyle#��[  .���  � RowDefinitions �� �� $��40 �.+�� y���  �� Path$ DisplayMainPage� �� Mode$ TwoWay=� �� $��35 ��� $	��340 ��� .+���   CounterpartyInformation#���    
TitleStyle#��[ $	��Top=�$��0���� $��1�� �� Padding$
 0 10q����#F8F8F8$��0q�._�� $��Center=� �� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��10 ���  � ColumnDefinitions �� $	��130 ��� $��10 ��� $	��150 ��� $	��150 ��� $��10 ��� $	��150 �.+���  �� Orientation$ 
Horizontal=�$��0��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   BusinessUnit#���  �� $��0��$��2��{�.   �� DisplayMemberPath$
 Name�� �� SelectedValuePath$ Id��  Caliburn.Micro.Message	   Attach$?	 9[Event SelectionChanged]=[Action OnBusinessUnitChanged()]������ *BusinessUnits+
 �� SelectedValue
 �� *BusinessUnitId+ �� ValidatesOnDataErrors . $ TwoWay=��� $ 
Horizontal=�$��0��$��3��.+��� $��*��
���  ��$��0,4,5,0q���   CounterpartyName#���  �� $��0��$��5������ *Name+ . $ TwoWay=���   FullName#���  $��1��$��0���� $��1��$��2��$��4������ *
FullName+$ TwoWay=��� $ 
Horizontal=�$��2��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   FinancialInstitution#���  �� $��2��$��2��{�.  $
 Name��$ Id������ *FinancialInstitutions+
 �� *InstitutionId+ �� UpdateSourceTrigger$ PropertyChanged=�$ TwoWay=��� $ 
Horizontal=�$��2��$��3��.+��� $��*��
���  ��$��0,4,5,0q���   CounterpartyGroup#���  �� $��2��$��5��$
 Name��$ Id������ *CounterpartyGroups+
 �� *	GroupId+ . $ TwoWay=���   CreationTime#���  $��3��$��0���� $��3��$��2�� �� TextAlignment$ Right=�{�.  ���� *CreationTime+$ TwoWay=� 	 FormatDateTimeByBuConverter �� 	Converter# [	 ��  
 LastUpdateTime#��� 
 $��3��$��3���� $��3��$��5��$ Right=�{�.  ���� *LastUpdateTime+$ TwoWay=�# [	 ��   SelectBankAccount#���  $��4��$��0���� $��4��$��2��$
 Name��$ Id������ *
Currencies+
 �� *SelectedCurrencyId+$ TwoWay=���   FindButtonStyle#��[ $	��5,0q�$��4��$��3��$
��Left=�$.	 ([Event Click]=[Action FindBankAccount()]����   SettlementAccountList#���  $��6��$��0��$	��Top=�&   System.Windows.Controls.DataGrid  $��6��$��2��$��4��  DataGridRowStyle   RowStyle# [   DataGridColumnHeaderStyle   ColumnHeaderStyle# [ ���� *SettlementAccountList+$ TwoWay=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ 2*   CCY   Header# �  1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *CurrencyName+  $ 4*   SettlementAccount# �    DataGridCellLeftStyle   	CellStyle# [  �� *SettlementAccount+  $ 4* # �  # [  �� *FinancialInstitution+� $��0��y��� $ DisplayConnectionPage�$ TwoWay=� �� $��35 ��� $��60 ��� $��40 ��� $	��140 ��� .+��� #���  #��[ $	��Top=�$��0���� $��1��$
 0 10q����#F8F8F8$��0q�._�� $��Center=� �� $	��130 ��� $��10 ��� $	��150 ��� $	��150 ��� $��10 ��� $	��150 �.+��� $ 
Horizontal=�$��0��.+��� $��*��
���  ��$��0,4,5,0q���   ConnectionType#���  �� $��2��$ Value��$	 Key��$A	 ;[Event SelectionChanged]=[Action OnConnectionTypeChanged()]������ *ConnectionTypes+
 �� *ConnectionType+$ PropertyChanged=�$ TwoWay=���   ConnectionParameters#���  #��[ $��2��$��Center=�y��� *" DisplayConnectionParametersPanel+�� $��3��$
 0 10q����#F8F8F8$��0q�y��� *" DisplayConnectionParametersPanel+._�� $��Center=� �� $��40 ��� $��40 ��� $��40 � �� $	��130 ��� $��10 ��� $	��150 ��� $	��150 ��� $��10 ��� $	��150 �.+��� $ 
Horizontal=�$��0��$��0��.+��� $��*��
���  ��$��0,4,5,0q���  	 Email#���  �� $��0��$��2��$��4������ *EmailAddress+ . $ TwoWay=��� $ 
Horizontal=�$��1��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   EmailLoginName#���  �� $��1��$��2������ *EmailLoginName+ . $ TwoWay=��� $ 
Horizontal=�$��1��$��3��.+��� $��*��
���  ��$��0,4,5,0q���   EmailPassword#���  �� $��1��$��5������ *EmailPassword+ . $ TwoWay=��� $ 
Horizontal=�$��2��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   EmailImportTemplate#���  �� $��2��$��2��$ Value��$	 Key������ *EmailTemplates+
 �� *EmailImportTemplate+$ TwoWay=��� $ 
Horizontal=�$��2��$��3��.+��� $��*��
���  ��$��0,4,5,0q���   EmailTimeZone#���  �� $��2��$��5��1   +DM2.Ent.Client.Views.ExtendClass.EnumBinder   Path$H BInfrastructure.Common.dll/Infrastructure.Common.Enums.TimeZoneEnum��
 �� *EmailTimeZone+$ TwoWay=���   PrevPage#���  $��1��$,	 &[Event Click] = [Action OnPrevPaged()]��  CommonButtonStle#��[ $
��Left=�y��� $ DisplayConnectionPage�$ TwoWay=��� $ 
Horizontal=�$��1��$��Right=�.+���   NextPage#���  $,	 &[Event Click] = [Action OnNextPaged()]��#��[ $��Right=�y��� $ DisplayMainPage�$ TwoWay=���   Save#���  $(	 "[Event Click] = [Action OnSaved()]��#��[ $��Right=�y��� $ DisplayConnectionPage�$ TwoWay=��� $��15����  	 Close#���  $$	 [Event Click] = [Action Close]��#��[ $��Right=�