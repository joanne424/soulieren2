   M S B A M L   `   `   `  ���� [  WBanclogix.Controls.PagedDataGrid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null}Xclr-namespace:Banclogix.Controls.PagedDataGrid;assembly=Banclogix.Controls.PagedDataGrid Banclogix.Controls.PagedDataGrid   DM2.Ent.Client.Views6   0DM2.Ent.Client.Views.Report.ActualCashReportView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  lpagedDataGridXclr-namespace:Banclogix.Controls.PagedDataGrid;assembly=Banclogix.Controls.PagedDataGrid  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  $	��650��$
��1000��  � WindowStartupLocation$  CenterScreen=� � 
ResizeMode$ 	CanResize=� !  NormalWindowWithMaximizeStyle#��[  .���  � RowDefinitions �� $	��100 ��� $��15 ��� $��* ��� $��40 �.+��� $��0�����#F8F8F8$��0q�._��  �� $��10 ��� $��40 ��� $��40 ��� $��10 � � ColumnDefinitions �� $��3* ��� $��3* ��� $��3* ��� $��3* ��� $��3* ��� $��3* ��� $��3* ��� $��3* ��� $��20 �.+���   BusinessUnit#���  $��0,0,10,0q�$��1��$��0���� $��1��$��1�� �� DisplayMemberPath$
 Name�� �� SelectedValuePath$ Id������ *BusinessUnits+ �� SelectedValue �� *SearchBusinessUnitId+ �� Mode$ TwoWay=���   BankAccount#���  $��0,0,10,0q�$��1��$��2���� $��1��$��3�� �� 
IsReadOnly . ���� *SearchBankAccount+$ TwoWay=��� $��1��$��3��$��Right=�$��	0,12,22,0q�  Caliburn.Micro.Message	   Attach$1	 +[Event Click] = [Action ClearBankAccount()]������ 
 �� 
TargetType"
 ��  CancelButtonStyle �� BasedOn# [  �� Triggers ��  �� Binding �� *SearchBankAccountId+$ TwoWay=� �� Value  .a���  �� Property � y� �� Value$ 	Collapsed=���   FindButtonStyle#��[ $��1��$��3��$��Right=�$��0,0,5,0q�$0	 *[Event Click] = [Action FindBankAccount()]����  
 DealID#���  $��0,0,10,0q�$��1��$��4���� $��1��$��5������ *SearchDealId+��   HedgingDealID#���  $��0,0,10,0q�$��1��$��6���� $��1��$��7������ *SearchHedgeDealId+��   TradeDateFrom#���  $��0,0,10,0q�$��2��$��0��(  "System.Windows.Controls.DatePicker    ClearButtonDatePicker#��[ $��2��$��1��   SelectedDate �� *LocalTradeDateFrom+$ TwoWay=���  	 To#��� 	 $��0,0,10,0q�$��2��$��2��  #��[ $��2��$��3�� �� *LocalTradeDateTo+$ TwoWay=���  
 
Instrument#��� 
 $��0,0,10,0q�$��2��$��4���� $��2��$��5��$ Value��$	 Key������ *Instruments+ �� *SearchInstrumentId+$ TwoWay=��� $��Right=�$��2��$��6��$��2�� 
 Search#���    CommonButtonStle#��[ .  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ Click��.  (System.Windows.Interactivity.TriggerBase   Actions. "  Caliburn.Micro.ActionMessage     
MethodName$ Search��   
Parameters.   Caliburn.Micro.Parameter     Value$ 1��   �� *
PageSize+$ TwoWay=� �� ElementName$ PagedDataGrid��5   /Banclogix.Controls.PagedDataGrid.PagingDataGrid  -     Name$ PagedDataGrid��$��2��&	   System.Windows.Controls.DataGrid" 	  VerticalScrollBarVisibility$
 Auto=�$ 	  HorizontalScrollBarVisibility$
 Auto=�$r	 l[Event PagingChanged] = [Action DateGridPageChanged($eventArgs)];[Event MouseDoubleClick] = [Action Preview]������ *ActualCashList+   	PageCount �� *	PageCount+   CommandString �� *PageCommand+ �� SelectedItem �� *SelectedActualCash+ 	  Columns 0
  *System.Windows.Controls.DataGridTextColumn
  ,  &System.Windows.Controls.DataGridColumn    Header#  �  !   Width5  /System.Windows.Controls.DataGridLengthConverter$! * "   MinWidth$" 90Q�  DataGridCellRightStyle#   	CellStyle## [ 1  +System.Windows.Controls.DataGridBoundColumn$   Binding$ �� *Id+
  #  �  $! * $	" 150Q�  DataGridCellLeftStyle## [ $ �� *BusinessUnitId+  BUIDToNameConverter% �� 	Converter#% [ 
  #  �  $! * $	" 150Q�## [ $ �� *BankAccountId+   BankAccountIdToNameConverter#% [ 
  #  � 
 $! * $	" 120Q�## [ $ �� *InstrumentId+  InstrumnetIdToNameConverter#% [ 
    CCY#  �  $! * $" 80Q�$ �� *
CurrencyId+  CurrencyIdToNameConverter#% [ 
    SignType#  �  $! * $	" 100Q�$ �� *
SignType+  SignTypeDisplayConverter#% [ 
   
 Amount#  �  $! * $	" 100Q�## [ & 
  
Foreground& �� *Amount+ $  DeficitAmountForegroundConverter#% [ $ `�  # FormatAmountByCurrencyConverter' `� 	Converter#' [ .1��� ( �� Path$( Amount��� $( 
CurrencyId�
  $! * $	" 100Q�  LocalTradeDate#  �  $ �� *LocalTradeDate+  FormatDateByBuConverter#% [ 
  $! * $" 80Q� 
 Status#  �  $ �� *Status+  StatusDisplayConverter#% [ 
  $! * $	" 120Q�  Comment#  �  ## [ $ �� *	Comment+
  $! * $" 90Q�  User#  �  ## [ $ �� *UserId+  UserIdToNameConverter#% [ 
  $! * $" 90Q� 	  Staff#  �   ## [ $ �� *	StaffId+#% [ 
  $! * $	" 150Q� ! CreationTime#  � ! $ �� *CreationTime+ " FormatDateTimeByBuConverter#% [" 
  $! * $	" 100Q�#  �  ## [ $ �� *HedgeDealId+�� $��3�� 	# Close#��� # $$	 [Event Click] = [Action Close]��#��[ $��Right=�