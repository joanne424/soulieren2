   M S B A M L   `   `   `  ���� R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� 
TargetType �� 
ResourceId �� TypeInTargetAssembly&    System.Windows.Controls.DataGrid �� Property �� Value �� 
TargetType'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard	 �� 
TargetName
 �� TargetProperty8  2System.Windows.Media.Animation.EasingColorKeyFrame �� KeyTime   DG_HeaderTopColor �� Value R� KeyTime  System.Windows.Visibility   	Collapsed R� Value ��Name J� SnapsToDevicePixels �� Color �Name J� Opacity � Points �� Stretch  DG_GlyphColor  ControlBorderColor  	FontColor  AlternatingRowBGBrush  GridLineBrush ��Name �� Padding # DG_DisabledDG_ControlLightColor  DG_ControlColor ��Name � ColumnDefinitions � RowDefinitions    SelectAllCommand7  1System.Windows.Controls.DataGridHeadersVisibility
   All �� ConverterParameter!    HeadersVisibilityConverter  �� 	Converter! �� RelativeSource" �� AncestorTypeG  ASystem.Windows.Controls.Primitives.DataGridColumnHeadersPresenter#  Name$   Column% ��Name& �� CanContentScroll' ��Name( �� Orientation) �� ViewportHeight* �� ViewportSize+ �� ScrollableHeight, � Maximum*- �� #ComputedVerticalScrollBarVisibility. � Value/ �� Mode0 �� ViewportWidth1 �� ScrollableWidth,2 �� %ComputedHorizontalScrollBarVisibility3 �� Triggers4 P� Property5 P� Value  quoteListDataGridRow*  $System.Windows.Controls.DataGridCell 	 QuoteListMouseOverRowBrush)	  #System.Windows.Controls.DataGridRow 
 	NormalRow  SelectedRowColor?
  9System.Windows.Controls.Primitives.SelectiveScrollingGrid?  9System.Windows.Controls.Primitives.DataGridCellsPresenter6 	  
ItemsPanelA  ;System.Windows.Controls.Primitives.DataGridDetailsPresenter7 	  DetailsVisibility$8 
  SelectiveScrollingOrientation;  5System.Windows.Controls.SelectiveScrollingOrientation9   Vertical#:    RowDetailsScrollingConverter:  4System.Windows.Controls.Primitives.DataGridRowHeader
;   Row< �� Triggers= �� 
TargetName  SpecialAlternatingRowBrush ( $QuoteListMouseOverForegroundRowBrush  RowHeaderGripperStyle  SelectedRow> �� Orientation? n�Name  ColumnHeaderGripperStyle=  7System.Windows.Controls.Primitives.DataGridColumnHeader@ �� EndPointA �� 
StartPointB � ColorC � OffsetD �� BitmapScalingMode  
sortHeaderE  NameF J� IsHitTestVisible�� F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml      %�-  (��       $" DataGridSelectAllButtonStyle��"   )'   �    &	 �    '  �    '	  o    &	     '  �    7  7  &	 (    '  �(    7  7  '  �,    �� "  ��.���  � �� �� " ��.R��    CommonStates��.   
Normal��. �� .���� $	 Border��$*
 $(Shape.Fill).(SolidColorBrush.Color)�.g�  $ 0��# �     	MouseOver��. �� .���� $	 Border��$*
 $(Shape.Fill).(SolidColorBrush.Color)�.g�  $ 0��# �     Pressed��. �� .���� $	 Border��$*
 $(Shape.Fill).(SolidColorBrush.Color)�.g�  $ 0��# �     Disabled��. �� .��T� $
 (UIElement.Visibility)�$	 Arrow��..�i� $ 0��# Z .+��� $ Border�� . ���� $ Transparent������ $ Transparent��� $ Arrow��$��Right=�$��8,8,3,3q�$
 0.15Q� �   ��� ��� ��� ��� $ Uniform=�$��Bottom=����� # �  �� "    .���  � ��$ Arial,NSimSun)���  � ��$ 12(���  � ��$ Stretch=���  � ��$ Stretch=���  �@  ColumnHeaderHeight$ 24Q���  �@  	RowHeight$ 21Q���  �@  AutoGenerateColumns .  ��  �@  CanUserAddRows .  ��  �@  CanUserDeleteRows .  ��  �@  
IsReadOnly . ��  �@  HeadersVisibility$ Column=���  � ��# �  ��  � ��$ 0q���  � ��# �  ��   �@  AlternatingRowBackground# �  ��  �@��AlternationCount$ 2����   �@  HorizontalGridLinesBrush# �  ��  �@  VerticalGridLinesBrush# �  ��   �@  RowDetailsVisibilityMode$ VisibleWhenSelected=���  � �� . ��  �@��PanningMode$
 Both=���  �@��IsFlicksEnabled .  ��  � ��$
 Auto=���  � ��$
 Auto=���  �@%�VirtualizationMode$ 	Recycling=���  �@%�IsVirtualizing . ��  � �� �� "   .R��� $ border�� . #��z��#��z��# z������ $ Transparent��   CommonStates��.   Disabled��. �� .���� $	 border��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    
Normal��. �� .���� $	 border��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �  ._��� $ DG_ScrollViewer��|�.  ���� " ��.R��  �� $
��Auto ��� $��* ��� $
��Auto � �� $
��Auto ��� $��* ��� $
��Auto �.+��� |�.  #��Z ��C� *�� $" DataGridSelectAllButtonStyle��"   +y��� *HeadersVisibility+# Z #  Z ! �� ""   ���� *CellsPanelHorizontalOffset+! �� ""     $!# PART_ColumnHeadersPresenter��$��1��y��� *HeadersVisibility+# Z$ #  Z ! �� ""   �� $!% PART_ScrollContentPresenter��$��2��$��1��#& z���� $' PART_VerticalScrollBar��
������ $��2��$��1��$( Vertical=�#* z) #, z+ #y�z- . �� *VerticalOffset+$/ OneWay=�! �� *TemplatedParent+� $��1��$��2�� �� ���� **(NonFrozenColumnsViewportHorizontalOffset+! �� ""   �� $��* �.+��� $' PART_HorizontalScrollBar��
������ $��1��$( 
Horizontal=�#* z0 #, z1 #y�z2 . �� *HorizontalOffset+$/ OneWay=�! �� *TemplatedParent+.���� # z 3 P� 4 �@��
IsGrouping5 . . ���  � �� .  �� "    .���  � y� �� *LocalVisibility+��  � ��$ Arial,NSimSun)���  � ��$ 12(���  � ��$ Stretch=���  � ��$ Stretch=���  �@  ColumnHeaderHeight$ 24Q���  �@  	RowHeight$ 21Q���  �@  AutoGenerateColumns .  ��  �@  CanUserAddRows .  ��  �@  CanUserDeleteRows .  ��  �@  
IsReadOnly . ��  �@  HeadersVisibility$ Column=���  � ��# �  ��  � ��$ 0q���  � ��# �  ��   �@  AlternatingRowBackground# �  ��  �@��AlternationCount$ 2����   �@  HorizontalGridLinesBrush# �  ��  �@  VerticalGridLinesBrush# �  ��   �@  RowDetailsVisibilityMode$ VisibleWhenSelected=���  � �� . ��  �@��PanningMode$
 Both=���  �@��IsFlicksEnabled .  ��  � ��$
 Auto=���  � ��$
 Auto=���  �@%�VirtualizationMode$ 	Recycling=���  �@%�IsVirtualizing . ��  � �� �� "   .R��� $ border�� . #��z��#��z��# z������ $ Transparent��   CommonStates��.   Disabled��. �� .���� $	 border��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    
Normal��. �� .���� $	 border��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �  ._��� $ DG_ScrollViewer��|�.  ���� " ��.R��  �� $
��Auto ��� $��* ��� $
��Auto � �� $
��Auto ��� $��* ��� $
��Auto �.+��� |�.  #��Z ��C� *�� $" DataGridSelectAllButtonStyle��"   +y��� *HeadersVisibility+# Z #  Z ! �� ""   ���� *CellsPanelHorizontalOffset+! �� ""     $!# PART_ColumnHeadersPresenter��$��1��y��� *HeadersVisibility+# Z$ #  Z ! �� ""   �� $!% PART_ScrollContentPresenter��$��2��$��1��#& z���� $' PART_VerticalScrollBar��
������ $��2��$��1��$( Vertical=�#* z) #, z+ #y�z- . �� *VerticalOffset+$/ OneWay=�! �� *TemplatedParent+� $��1��$��2�� �� ���� **(NonFrozenColumnsViewportHorizontalOffset+! �� ""   �� $��* �.+��� $' PART_HorizontalScrollBar��
������ $��1��$( 
Horizontal=�#* z0 #, z1 #y�z2 . �� *HorizontalOffset+$/ OneWay=�! �� *TemplatedParent+.���� # z 3 P� 4 �@��
IsGrouping5 . . ���  � �� .  �� "   .���  � ��# �  ��  � �� �� "  .R��� $ border��
������ $��0q�
������  .    FocusStates��.   	Unfocused��  Focused��  CurrentStates��.   Regular��  Current��._��� $��Center=�# z ��  �@��TextAlignment$ Center=�3 P� 4 �@ 
IsSelected5 . . ���  � ��# � 	 �� "  	 .���  � �� �� $ Transparent����  � ��
 �   ���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  �@=�ErrorTemplate Z� ��  �@	 ValidationErrorTemplate �� .R��� 
���  ��$��2,0,0,0q�$��!��$��Center=���  � �� �� " 	 .R��� $ 
DGR_Border��#��z��#��z�� . ���� # � 
 ._�� .+��� $��	TopBorder��
������ ._��� $��innerTopBorder��$��0q�$��1,0,1,1q�   CommonStates��.   Normal_AlternatingRow��. �� .���� $	 	TopBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    
Normal��  Normal_Selected��. �� .���� $	 	TopBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    Unfocused_Selected��. �� .���� $	 	TopBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    Normal_Editing��  MouseOver_Editing��  MouseOver_Unfocused_Editing��. �� .���� $	 	TopBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    Unfocused_Editing��  	MouseOver��. �� .���� $	 	TopBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    MouseOver_Selected��. �� .���� $	 	TopBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �     MouseOver_Unfocused_Selected��. �� .���� $	 	TopBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �  �� $��hightBorder��}�. $��0q�
������ ._��� $��innerShadow��$��0q�$��1,0,1,1q�
   �� $
��Auto ��� $��* � �� $��* ��� $
��Auto �.+�  $��1��#��z6 # z   $��1��$��1��#y�z7 8 �� *AreRowDetailsFrozen+# Z9 #  Z: ! �� ""     $��2��$8 Vertical=�y��� *HeadersVisibility+# Z; #  Z ! �� ""   < P� 4 �@J�IsMouseOver5 . . ��� $= hightBorder��# � 	  � ��P� 4 �@��AlternationIndex$5 1��. ��� # �  $= 
DGR_Border�� � ��3 P� 4 �@��
IsSelected5 . . ���  � ��# � 	 ��  � ��# �  P� 4 �@J�IsMouseOver5 . . ���  � ��# � 	 ��  � ��# �  �� "  n�.���  � ��$ 8����  � ��
 ���� ��  �@�Cursor$ Arrow����  � �� �� " n�.R��� #��z��# z���� "   .���  �@�OverridesDefaultStyle . ��  � �� �� "  .R��    CommonStates��.   
Normal��. �� .���� $	 rowHeaderBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# � 
   Normal_CurrentRow��. �� .���� $	 rowHeaderBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    Unfocused_EditingRow��  Normal_EditingRow��  	MouseOver��. �� .���� $	 rowHeaderBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    MouseOver_CurrentRow��. �� .���� $	 rowHeaderBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    "MouseOver_Unfocused_EditingRow��  MouseOver_EditingRow��   MouseOver_Unfocused_Selected��. �� .���� $	 rowHeaderBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    MouseOver_Selected��. �� .���� $	 rowHeaderBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    +'MouseOver_Unfocused_CurrentRow_Selected��. �� .���� $	 rowHeaderBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    !MouseOver_CurrentRow_Selected��. �� .���� $	 rowHeaderBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    Unfocused_Selected��. �� .���� $	 rowHeaderBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    !Unfocused_CurrentRow_Selected��. �� .���� $	 rowHeaderBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    Normal_CurrentRow_Selected��. �� .���� $	 rowHeaderBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �    Normal_Selected��. �� .���� $	 rowHeaderBorder��$0
 *(Panel.Background).(SolidColorBrush.Color)�.g�  $ 0��# �  .+��� $ rowHeaderBorder��$��10��#���  #���  $��1q�._��� $> 
Horizontal=�.+��� $��Center=�# z n� $? PART_TopHeaderGripper��8��  $	��Top=�n� $? PART_BottomHeaderGripper��8�� $��Bottom=��� "  n�.���  � ��$ 8����  � ��
 ���� ��  �@�Cursor$ SizeWE����  � �� �� " n�.R��� #��z��# z���� "   .���  � ��$ Center=���  � ��$ Stretch=���  � ��$ Center=���  � �� �� "  .R�� .+��� $ columnHeaderBorder��$��0,0,1,1q�$ 	10,0,10,0q����#d1d1cf���� $@ 0.5,1&�$A 0.5,0&�.��� $B White��$
C 0.58Q�� $B 	#FFEDEDED��$C 0.621Q�� $B 	#FFEDEDED��$C 0.991Q�._��� #��z��# z #��z���� $��upOrDownArrow��$y�	Collapsed=�$��7��$��8��$D NearestNeighbor=�$��Right=�$��0,0,2,0q�n� $? PART_LeftHeaderGripper��$
��Left=�8��  n� $? PART_RightHeaderGripper��$��Right=�8�� < P� 4 �@ SortDirection$5 	AscendingX�. ��� $= columnHeaderBorder��# �   � ���� $= upOrDownArrow�� � y�$ Visible=��� $= upOrDownArrow�� � ��$H B/DM2.Ent.Client.Views;component/Resources/Images/White/UpArrow.png��P� 4 �@ SortDirection$5 
DescendingX�. ��� $= columnHeaderBorder��# �   � ���� $= upOrDownArrow�� � y�$ Visible=��� $= upOrDownArrow�� � ��$J D/DM2.Ent.Client.Views;component/Resources/Images/White/DownArrow.png���� "   .���  � �� �� "  .R�� .+�  $E PART_FillerColumnHeader��F .  �� 