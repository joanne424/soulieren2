   M S B A M L   `   `   `  ���� Q  MBanclogix.Controls.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW@clr-namespace:Banclogix.Controls;assembly=Banclogix.Controls.WPFBanclogix.Controls   DM2.Ent.Client.Views<   6DM2.Ent.Client.Views.BankAccount.PreviewActualCashView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  Ocontrols@clr-namespace:Banclogix.Controls;assembly=Banclogix.Controls.WPF  $	��610��$	��700��  � WindowStartupLocation$  CenterScreen=�   CommonOnlyCloseWindowStyle#��[  .��� $��5q� � RowDefinitions �� $��35 ��� $	��440 ��� �� $��40 �.+���  $  BankAccountActualCashInformation#���    
TitleStyle#��[ $	��Top=��� $��1�� �� Padding$
 0 10q����#F8F8F8{�.  $��0q�._�� $��Center=� �� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��10 ��� $��80 � � ColumnDefinitions �� $	��100 ��� $��10 ��� $	��150 ��� $	��150 ��� $��10 ��� $	��150 �.+���  �� Orientation$ 
Horizontal=�$��0��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   BankAccount#���  �� $��0��$��2��$��4������ *BankAccountName+ �� ValidatesOnDataErrors .  �� Mode$ TwoWay=��� $��1��$��0��  BusinessUnit#���  �� $��1��$��2��{�.  ���� *BusinessUnitId+$ TwoWay=�  BUIDToNameConverter �� 	Converter# [ �� $��1��$��3��  FinancialInstitution#���  �� $��1��$��5��{�.  ���� *InstitutionId+$ TwoWay=�   InstitutionIdToNameConverter# [ �� $��2��$��0��  CCY#���  �� $��2��$��2��{�.  ���� *
CurrencyId+$ TwoWay=� 	 CurrencyIdToNameConverter# [	 �� $��2��$��3�� 
 AvailableBalance#��� 
 �� $��2��$��5�� �� TextAlignment$ Right=�{�.  ���� *BankAvailableBalance+ $  DeficitAmountForegroundConverter# [ .����`�  # FormatAmountByCurrencyConverter	 `� 	Converter#	 [ .1��� 
 �� Path$
 BankAvailableBalance��� $
 
CurrencyId�.���� $ 
Horizontal=�$��3��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   TradableInstrument#���  �� $��3��$��2��  Caliburn.Micro.Message   Attach$G A[Event SelectionChanged] = [Action OnTradableInstrumentChanged()]�� �� DisplayMemberPath$ Value�� �� SelectedValuePath$	 Key������ *TradableInstruments+ �� SelectedValue �� *TradableInstrument+$ TwoWay=��� $ 
Horizontal=�$��3��$��3��.+��� $��*��
���  ��$��0,4,5,0q���   
Instrument#���  �� $��3��$��5��$ Value��$	 Key������ *Instruments+ �� *InstrumentId+ . $ TwoWay=��� $ 
Horizontal=�$��4��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   SignType#���  �� $ 
Horizontal=�$��4��$��2��.+�	� $��Center=�  Payment#���  $3 -[Event Checked] = [Action OnPaymentChecked()]�� d� 	IsChecked �� *PaymentChecked+$ TwoWay=��� $��15��	� $��Center=�  Receipt#���  $3 -[Event Checked] = [Action OnReceiptChecked()]�� �� *ReceiptChecked+$ TwoWay=��� $ 
Horizontal=�$��4��$��3��.+��� $��*��
���  ��$��0,4,5,0q���  
 Amount#���  #   Banclogix.Controls.NumericBox  $��4��$��5��$ Right=�   Amount �� *Amount+ . $ TwoWay=�   Decimals �� *
CurrencyId+$ TwoWay=�  CurrencyIdToAmountDecimals# [ �� $��5��$��0��  HedgingDealID#���  �� $��5��$��2������ *HedgeDealId+$ TwoWay=��� $ 
Horizontal=�$��5��$��3��.+��� $��*��
���  ��$��0,4,5,0q���   LocalTradeDate#���  �� $��5��$��5�� �� 
IsReadOnly . ���� *LocalTradeDate+ . $ TwoWay=�  FormatDateByBuConverter# [ �� $��6��$��0�� 
 Status#���  �� $��6��$��2��{�.  ���� *Status+  StatusDisplayConverter# [ $ TwoWay=��� $��6��$��3��  CreationTime#���  �� $��6��$��5��{�.  ���� *CreationTime+$ TwoWay=�  FormatDateTimeByBuConverter# [ �� $��7��$��0��  User#���  �� $��7��$��2��{�.  ���� *UserId+$ TwoWay=�  UserIdToNameConverter# [ �� $��7��$��3�� 	 Staff#���  �� $��7��$��5��{�.  �� $	��Top=�$��9��$��0��  Comment#���  �� $��9��$��2��$	��Top=� �� TextWrapping$
 Wrap=�$��4��$��80������ *	Comment+$ TwoWay=��� $ 
Horizontal=�$��3��$��Bottom=�.+���  	 Close#���  $$ [Event Click] = [Action Close]��   CommonButtonStle#��[  $��Right=�