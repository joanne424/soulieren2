   M S B A M L   `   `   `  ���� R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   DealQuotoListRowStyle  �� 
TargetType)   #System.Windows.Controls.DataGridRow �� Property �� Value �� 
TargetType ��Name J� SnapsToDevicePixels'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard	 �� 
TargetName
 �� TargetProperty8  2System.Windows.Media.Animation.EasingColorKeyFrame �� KeyTime  QuoteListSelectedRow �� Value �� Color?  9System.Windows.Controls.Primitives.SelectiveScrollingGrid � ColumnDefinitions � RowDefinitions?  9System.Windows.Controls.Primitives.DataGridCellsPresenter    
ItemsPanelA  ;System.Windows.Controls.Primitives.DataGridDetailsPresenter    DetailsVisibility$   SelectiveScrollingOrientation;  5System.Windows.Controls.SelectiveScrollingOrientation   Vertical �� ConverterParameter&	   System.Windows.Controls.DataGrid# 	  RowDetailsScrollingConverter �� 	Converter �� RelativeSource �� AncestorType:
  4System.Windows.Controls.Primitives.DataGridRowHeader7  1System.Windows.Controls.DataGridHeadersVisibility
   Row! 	  HeadersVisibilityConverter �� Triggers �� Value �� Binding �� Mode �� 
TargetName  QuoteListInDealHLRowBrush  P� Property! P� Value  QuoteListSelectedRowBrush�� F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml      %   &	        �� "    .���  � ��
 ���� ��  � y�$ 	Collapsed=���  � ��
 �   ���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  �@=�ErrorTemplate Z� ��  �@  ValidationErrorTemplate �� .R��� 
���  ��$��2,0,0,0q�$��!��$��Center=���  � �� �� "   .R��� $ 
DGR_Border��#��z��#��z��#��z�� . ._�� .+��� $��	TopBorder��   CommonStates��.   Normal_AlternatingRow��. �� .���� $	 	TopBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    
Normal��  Normal_Selected��. �� .���� $	 	TopBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    Unfocused_Selected��. �� .���� $	 	TopBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    Normal_Editing��  MouseOver_Editing��  MouseOver_Unfocused_Editing��. �� .���� $	 	TopBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    Unfocused_Editing��  	MouseOver��. �� .���� $	 	TopBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    MouseOver_Selected��. �� .���� $	 	TopBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �     MouseOver_Unfocused_Selected��. �� .���� $	 	TopBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �  �� $��hightBorder������ $ Transparent��   �� $
��Auto ��� $��* � �� $��* ��� $
��Auto �.+�  $��1��#��z # z   $��1��$��1��#y�z  �� *AreRowDetailsFrozen+# Z # Z  �� " 	 
  $��2��$ Vertical=�y��� *HeadersVisibility+# Z # Z  �� " 	  �� $
 True�� �� *Value.IsHighLight+$ OneWay=�.a��� $ hightBorder��# �   � ���� $
 True�� �� *Value.IsVisible+$ OneWay=�.a���  � y�$ Visible=�P�   �@J�IsMouseOver! . . ��� $ hightBorder��# �   � ��