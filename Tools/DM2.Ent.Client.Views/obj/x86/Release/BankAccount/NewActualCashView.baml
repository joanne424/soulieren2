   M S B A M L   `   `   `  ���� Q  MBanclogix.Controls.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW@clr-namespace:Banclogix.Controls;assembly=Banclogix.Controls.WPFBanclogix.Controls   DM2.Ent.Client.Views8   2DM2.Ent.Client.Views.BankAccount.NewActualCashView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  Ocontrols@clr-namespace:Banclogix.Controls;assembly=Banclogix.Controls.WPF  $	��530��$	��700��  � WindowStartupLocation$  CenterScreen=�   CommonOnlyCloseWindowStyle#��[  .��� $��5q� � RowDefinitions �� $��35 ��� $	��360 ��� �� $��40 �.+���  $  BankAccountActualCashInformation#���    
TitleStyle#��[ $	��Top=��� $��1�� �� Padding$
 0 10q����#F8F8F8$��0q�._�� $��Center=� �� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��40 ��� $��10 ��� $��80 � � ColumnDefinitions �� $	��100 ��� $��10 ��� $	��150 ��� $	��150 ��� $��10 ��� $	��150 �.+���  �� Orientation$ 
Horizontal=�$��0��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   BankAccount#���  �� $��0��$��2��$��4�� �� 
IsReadOnly . ���� *BankAccountName+ �� ValidatesOnDataErrors .  �� Mode$ TwoWay=���   FindButtonStyle#��[ $��0��$��5��$��Right=�$��0,0,5,0q�  Caliburn.Micro.Message   Attach$0 *[Event Click] = [Action FindBankAccount()]���� $��1��$��0��  BusinessUnit#���  �� $��1��$��2��{�.  ���� *BusinessUnitId+$ TwoWay=�  BUIDToNameConverter	 �� 	Converter#	 [ �� $��1��$��3��  FinancialInstitution#���  �� $��1��$��5��{�.  ���� *InstitutionId+$ TwoWay=�   InstitutionIdToNameConverter#	 [ �� $��2��$��0�� 	 CCY#��� 	 �� $��2��$��2��{�.  ���� *
CurrencyId+$ TwoWay=� 
 CurrencyIdToNameConverter#	 [
 �� $��2��$��3��  AvailableBalance#���  �� $��2��$��5��
 �� TextAlignment$
 Right=�{�.  ���� *BankAvailableBalance+ $  DeficitAmountForegroundConverter#	 [ .����`�  # FormatAmountByCurrencyConverter `� 	Converter# [ .1���  �� Path$ BankAvailableBalance��� $ 
CurrencyId�.���� $ 
Horizontal=�$��3��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   TradableInstrument#���  �� $��3��$��2��$G A[Event SelectionChanged] = [Action OnTradableInstrumentChanged()]�� �� DisplayMemberPath$ Value�� �� SelectedValuePath$	 Key������ *TradableInstruments+ �� SelectedValue �� *TradableInstrument+$ TwoWay=��� $ 
Horizontal=�$��3��$��3��.+��� $��*��
���  ��$��0,4,5,0q���   
Instrument#���  �� $��3��$��5��$ Value��$	 Key������ *Instruments+ �� *InstrumentId+ . $ TwoWay=��� $ 
Horizontal=�$��4��$��0��.+��� $��*��
���  ��$��0,4,5,0q���   SignType#���  �� $ 
Horizontal=�$��4��$��2��.+�	� $��Center=�  Payment#���  $3 -[Event Checked] = [Action OnPaymentChecked()]�� d� 	IsChecked �� *PaymentChecked+$ TwoWay=��� $��15��	� $��Center=�  Receipt#���  $3 -[Event Checked] = [Action OnReceiptChecked()]�� �� *ReceiptChecked+$ TwoWay=��� $ 
Horizontal=�$��4��$��3��.+��� $��*��
���  ��$��0,4,5,0q���  
 Amount#���  #   Banclogix.Controls.NumericBox  $��4��$��5��$
 Right=�   Amount �� *Amount+ . $ TwoWay=�   Decimals �� *
CurrencyId+$ TwoWay=�  CurrencyIdToAmountDecimals#	 [ �� $��5��$��0��  HedgingDealID#���  �� $��5��$��2������ *HedgeDealId+$ TwoWay=��� $ 
Horizontal=�$��5��$��3��.+��� $��*��
���  ��$��0,4,5,0q���   LocalTradeDate#���  (  "System.Windows.Controls.DatePicker  $��5��$��5��   Text �� *LocalTradeDate+ . $ TwoWay=��� $	��Top=�$��7��$��0��  Comment#���  �� $��7��$��2�� �� AcceptsReturn .  �� TextWrapping$
 Wrap=�$��4��$
��Auto������ *	Comment+$ TwoWay=��� $ 
Horizontal=�$��3��$��Bottom=�.+���   Save#���  $( "[Event Click] = [Action OnSaved()]��  CommonButtonStle#��[ $��Right=��� $��15����  	 Close#���  $$ [Event Click] = [Action Close]��#��[ $��Right=�