   M S B A M L   `   `   `  ����a  ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes  5      [ WBanclogix.Controls.PagedDataGrid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:Banclogix.Controls.PagedDataGrid Banclogix.Controls.PagedDataGrid 5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   ButtonFocusVisual  �� Property �� Value �� StrokeDashArray J� SnapsToDevicePixels  ImageButtonStyle �� 
TargetType �� 
TargetType �� Triggers P� Property P� Value:   4Banclogix.Controls.PagedDataGrid.VisibilityConverter  visibilityConverter5  /Banclogix.Controls.PagedDataGrid.PagingDataGrid&   System.Windows.Controls.DataGrid	 �� Padding
 � RowDefinitions � ColumnDefinitions   SelectAllCommand �� RelativeSource �� AncestorType �� Path �� TypeInTargetAssembly �� 
ResourceId!   HeadersVisibilityConverter �� 	Converter7  1System.Windows.Controls.DataGridHeadersVisibility
   All �� ConverterParameterG  ASystem.Windows.Controls.Primitives.DataGridColumnHeadersPresenter   Column ��Name �� CanContentScroll �� Orientation �� ScrollableHeight � Maximum �� ViewportHeight �� ViewportSize* �� #ComputedVerticalScrollBarVisibility � Value  �� Mode! �� ScrollableWidth" �� ViewportWidth,# �� %ComputedHorizontalScrollBarVisibility$ ��Name% �� SelectedItem& �� UpdateSourceTrigger' �� Orientation2  ,Banclogix.Controls.PagedDataGrid.ImageButton(  Name)   ImageSource*   EntryImageSource+   GrayImageSource, ��Name- �� Triggers=  7System.Windows.Controls.Primitives.DataGridColumnHeader. J� IsHitTestVisible  ColumnHeaderGripperStyle  RowHeaderGripperStyle)  #System.Windows.Controls.DataGridRow/ ��Name?  9System.Windows.Controls.Primitives.SelectiveScrollingGrid?	  9System.Windows.Controls.Primitives.DataGridCellsPresenter0   
ItemsPanelA
  ;System.Windows.Controls.Primitives.DataGridDetailsPresenter1   DetailsVisibility$2   SelectiveScrollingOrientation#3   RowDetailsScrollingConverter;  5System.Windows.Controls.SelectiveScrollingOrientation4   Vertical:  4System.Windows.Controls.Primitives.DataGridRowHeader
5   Row6 �� Color*  $System.Windows.Controls.DataGridCell7   FocusBorderBrushKey8 �� SelectionBoxItem9 �� SelectionBoxItemTemplate#: �� SelectionBoxItemStringFormat; �� ContentStringFormat3   -Microsoft.Windows.Themes.DataGridHeaderBorder<   SortDirection=   SortDirection> J� IsMouseOver?   	IsHovered@   	IsPressedA   CanUserSortB   IsClickableC   SeparatorVisibilityD   SeparatorVisibilityE   SeparatorBrushF   SeparatorBrushG n�NameH   IsRowSelectedI   
IsSelectedJ   OrientationK   SeparatorVisibilityL   SeparatorBrush  bool2VisibilityConverter�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      TthemeJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero  5      8local.clr-namespace:Banclogix.Controls.PagedDataGrid  5      %�*  &	        &	 �     7   &	 �    '  �    7  '  k    &	 ?    &	 U    (�� k    5�      *31DataGridColumnHeader.ColumnFloatingHeaderStyleKey+)(��     5�      *86DataGridColumnHeader.ColumnHeaderDropSeparatorStyleKey+)'  �    '	  �    (�� n     5B     *DataGrid.FocusBorderBrushKey+)'  �     (�� �"    5a     *20DataGridComboBoxColumn.TextBlockComboBoxStyleKey+)'  �#    7  7  '  H&    7  7  7  �� 5      .�5   
   ��   � �� 5      �� 5	      .R�5
      �� $��0q�6    $��0��5       #��� ��5       $	 1 2R�5        . 5       5      5      5      �� 5      " ��5      .�5   
   ��   �@�FocusVisualStyle8   5      �� 5   
     � �� 5      Z� �� 5   
     � �� 5      Z� �� 5   
     � ��$ 1q�5      �� 5   
     � ��# � ��5      �� 5   
     � ��$ Center=�5       �� 5!   
     � ��$ Center=�5"      �� 5#   
     � ��$ 1q�5$      �� 5%   
     � �� 5&      �� 5'      " ��6"   .R�5(      � .+�5)      �� #��z��6+   ��. # z 6�   #��z��6�   5*       5+      P� 5,       �@J�	IsFocused . 67   P� 5-       �@J�IsMouseOver . 69   P� 5.       � �� . 67   P� 5/       � {� .  67   51      53      54         57      6    �� 58      "  62   .�59   
   ��   �@ 
IsReadOnly . 5:      �� 5;   
     � ��# � ��5<      �� 5=   
     � ��# � ��5>      �� 5?   
     � �� �	#FF688CAF6(   �� 5@   
     � ��$ 1q�6,   �� 5A   
      �@ RowDetailsVisibilityMode$ VisibleWhenSelected=�65   �� 5B   
     � �� . 5C      �� 5D   
     �@��PanningMode$
 Both=�5E      �� 5F   
     �@��IsFlicksEnabled .  5G      �� 5H   
     � �� 5I      �� 5J      "  6"   .R�5K      �� #��z��6   #��z��5L       . #	 z��5N      ._�5O      � 
 5P      �� 5Q   "   $��* �60   �� 5R   "   $
��Auto �60   .+�5T      �� $��DG_ScrollViewer��5V   !   $��0��5T   +   |�.  ��5W   "   �� 5X   &   " ��66   .R�5Y   *   � 
 5Z   .   �� 5[   2   $
��Auto �6@   �� 5\   2   $��* �6@   �� 5]   2   $
��Auto �6@    5_   .   �� 5`   2   $
��Auto �6C   �� 5a   2   $��* �6C   �� 5b   2   $
��Auto �6C   .+�5d   .   �� #��Z 65   |�.  ��5e   1   ��  �� "  $  CellsPanelHorizontalOffset���5f   1   C� *�� "  $" DataGridSelectAllButtonStyle��+y�5h   1   ��  �� "  $ HeadersVisibility�# Z # Z   5i   .   $!��PART_ColumnHeadersPresenter��5j   <   $��1��5i   M   y�5k   <   ��  �� "  $ HeadersVisibility�# Z # Z �� 5m   .   $! PART_ScrollContentPresenter��6E   $��1��6j   $��2��6w   # z��6�   �� 5o   .   $��PART_VerticalScrollBar��6U   $��1��68   $��2��6E   $ Vertical=�5p   2   # z 5q   2   # z 5r   2   #y�z 5t   2    5s   2   �� $ VerticalOffset� �� *TemplatedParent+$  OneWay=�5t   2   � 5v   .   $��2��63   $��1��6@    5w   2   �� 5x   6   ��6G   ��  �� "  $. (NonFrozenColumnsViewportHorizontalOffset��� 5y   6   $��* �6G   .+�5{   2   �� $��PART_HorizontalScrollBar��5|   8   $��1��5{   <   $ 
Horizontal=�5}   8   # z! 5~   8   # z" 5   8   #y�z# 5�   8    5�   8   �� $ HorizontalOffset� �� *TemplatedParent+$  OneWay=�5�   8   5�   /   5�   +   5�   '   .��5�   "   �� # z 61   5�      � 5�      $��1��6#   $��Center=�60   $��0,4,0,4q�6M   y�6^   �� *IsShowPaging+ �� *TemplatedParent+8    5�   "   �� 5�   &   $
��Auto �67   �� 5�   &   .+�5�   "   �� $$ PART_PageSizeList��6W   $��0��6+   $��Center=�6<   $��40��5�   '   $��0,0,5,0q�65   ��5�   '   �� *PageSizeItemsSource+ �� *TemplatedParent+$  TwoWay=�% 5�   '   �� *
PageSize+ �� *TemplatedParent+$  TwoWay=�$& PropertyChanged=��� 5�   "   $��1��6-   $��Center=�6=   $' 
Horizontal=�6X   .+�5�   &     $( PART_FirstPage��68   $M) G/Banclogix.Controls.PagedDataGrid;component/Images/pagination_first.png��5�   )   $S* M/Banclogix.Controls.PagedDataGrid;component/Images/pagination_first_entry.png��5�   )   $R+ L/Banclogix.Controls.PagedDataGrid;component/Images/pagination_first_gray.png��5�   )     5�   &   $( PART_PerviousPage��68   $L) F/Banclogix.Controls.PagedDataGrid;component/Images/pagination_prev.png��5�   )   $R* L/Banclogix.Controls.PagedDataGrid;component/Images/pagination_prev_entry.png��5�   )   $Q+ K/Banclogix.Controls.PagedDataGrid;component/Images/pagination_prev_gray.png��5�   )   �� 5�   &   $��Page ��60   $��Center=�6>   �� 5�   &   $, PART_PageIndex��6�   $��30��6.   ��69   �� *	PageIndex+ �� *TemplatedParent+$  TwoWay=�6�   �� 5�   &   $
�� of ��60   $��Center=�6=   �� 5�   &   $��Center=�6�   ��60   �� *	PageCount+ �� *TemplatedParent+$  OneWay=�6�     5�   &   $( PART_NextPage��68   $L) F/Banclogix.Controls.PagedDataGrid;component/Images/pagination_next.png��5�   )   $R* L/Banclogix.Controls.PagedDataGrid;component/Images/pagination_next_entry.png��5�   )   $Q+ K/Banclogix.Controls.PagedDataGrid;component/Images/pagination_next_gray.png��5�   )     5�   &   $( PART_LastPage��68   $L) F/Banclogix.Controls.PagedDataGrid;component/Images/pagination_last.png��5�   )   $R* L/Banclogix.Controls.PagedDataGrid;component/Images/pagination_last_entry.png��5�   )   $Q+ K/Banclogix.Controls.PagedDataGrid;component/Images/pagination_last_gray.png��5�   )   5�   #   �� 5�   "   $��2��6,   $y�	Collapsed=�6<   $��Center=�6S   $��5,0,5,0q�6n   
Display �� 5�   .   ��68   �� *Start+ �� *TemplatedParent+$  OneWay=� to �� 6�   ��6�   �� *End+ �� *TemplatedParent+$  OneWay=� of �� 6�   ��6�   �� *Total+ �� *TemplatedParent+$  OneWay=� items5�   #   5�      5�      5�      5�      5�      - 5�   
   P� 5�       �@��
IsGrouping . . �5�      ��   � �� .  6B   5�      5�      �� 5�      "  6<   .�5�   
   ��   � �� 5�      �� 5�      "  6"   .R�5�      � .+�5�        $��PART_FillerColumnHeader��6H   . .  �� 5�      5�      5�      5�      5�      �� 5�      " n�6-   .�5�   
   ��   � ��$ 8��6"   �� 5�   
     � ��
 ���� 6'   �� 5�   
     �@�Cursor$ SizeWE��6#   �� 5�   
     � �� 5�      �� 5�      " n�6"   .R�5�      �� #	 z��6   #��z��5�      5�      5�      5�      �� 5�      " n�6*   .�5�   
   ��   � ��$ 8��6#   �� 5�   
     � ��
 ���� 6'   �� 5�   
     �@�Cursor$ SizeNS��6#   �� 5�   
     � �� 5�      �� 5�      " n�6"   .R�5�      �� #	 z��6   #��z��5�      5�      5�      5�      �� 5�      " ��6Q   .�5�   
   ��   �@J�Opacity$	 0.6Q�6$   62   �� 5�   
     � �� �	#FF688CAF6<   �� 5�   
     � ��$ 1q�6,   68   �� 5�   
     � �� 5�      �� 5�      " ��6"   .R�5�      �� #��z��6   #��z��5�      #	 z��5�      #��z��5�      ._�5�      � .+�5�      �� $��PART_VisualBrushCanvas��6%   #��z��6C   5�      5�      5�      5�      5�      �� 5�      " ��6V   .�5�   
   ��   � �� �	#FF0000806'   �� 5�   
     � �� 5�      �� 5�      " ��6"   .R�5�      �� #��z��6   #��z��5      #��z��5      . 5     5     5     5     �� 5	     "  6)   .�5
  
   ��   � ��# � ��6'   �� 5  
     �@J�SnapsToDevicePixels . 60   �� 5  
     �@=�ErrorTemplate 65   Z� �� 5  
     �@ ValidationErrorTemplate 5     �� 5     .R�5     �� $��2,0,0,0q�6    $��Center=�61   
���  ��$��!��6]   5     5     �� 5  
     � �� 5     �� 5     "  6"   .R�5     �� $/ 
DGR_Border��6   #��z��5     #��z��5     #��z��5      . ._�5        5     �� 5  "   $
��Auto �63   �� 5  "   $��* �63   
 5"     �� 5#  "   $��* �60   �� 5$  "   $
��Auto �60   .+�5'     	  $��1��65   #��z0 5(  .   # z 5)  .   
  5+     $��1��5,  1   $��1��6A   #y�z1 5-  1   2 5+  8   ��  �� "  $ AreRowDetailsFrozen�# Z3 # Z4 5-  1     5/     $2 Vertical=�60   $��2��6q   y�50  )   ��  �� "  $ HeadersVisibility�# Z # Z5 51     52     53     55     56     �� 58     " 	 64   .�59  
   ��   � �� 5:     �� 5;     " 	 6"   .R�5<     �� 5=     5?     5@     �� 5B     $6 	#FF000000��5C     �� 5E     "  6*   .�5F  
   ��   � ��
 ���� 6'   �� 5G  
     � ��
 ���� 6(   �� 5H  
     � ��$ 1q�6,   �� 5I  
     � �� 5J     �� 5K     "  6"   .R�5L     �� #��z��6   #��z��5M     #��z��5N      . ._�5P     �� # z 6+   5Q     5R     5T     - 5U  
   P� 5V      �@ 
IsSelected . . �5W     ��   � ��# � ��6/   �� 5X       � ��# � ��6/   �� 5Y       � ��# � ��60   5Z     P� 5[      �@J�IsKeyboardFocusWithin . . �5\     ��   � ��# � 7 60   5]     5_     �� 5a     " ��6P   .�5b  
   ��   � �� 5c     �� 5d     " ��6"   .R�5e     �� $��1,1,1,1q�6'   #��z8 5f  !   #��z9 5g  !   #��z��5h  !   #; z: 5i  !   #��z��5j  !   #��z��5k  !   # z 5l  !   . .  5m  !   5n     5p     5q     �� 5u     "  62   .�5v  
   ��   � ��$ Center=�65   �� 5w  
     � �� 5x     �� 5y     "  6"   .R�5z     � .+�5{       #= z< 65   #? z> 5|  -   #@ z��5}  -   #B zA 5~  -   #��z��5  -   #��z��5�  -   #��z��5�  -   #	 z��5�  -   #D zC 5�  -   #F zE 5�  -   ._�5�     �� # z 6/   #��z��5�  )   #��z��5�  )   5�     n� 5�     $G PART_LeftHeaderGripper��6    $
��Left=�5�  !   8��  5�  !   n� 5�     $G PART_RightHeaderGripper��6    $��Right=�5�  !   8�� 5�  !   5�     5�     5�     5�     �� 5�     "  6/   .�5�  
   ��   � �� 5�     �� 5�     "  6"   .R�5�     � .+�5�       #I zH 65   #? z> 5�  *   #@ z��5�  *   #��z��5�  *   #��z��5�  *   #��z��5�  *   #	 z��5�  *   $J 
Horizontal=�5�  *   #D zK 5�  *   #F zL 5�  *   ._�5�     �� $' 
Horizontal=�6)   .+�5�  "   �� # z 63   $��Center=�6o   �� 5�  "    .  y�5�  *   ��  �� "  $ (Validation.HasError)�8   ��5�  *   ��  �� "  $ ValidationErrorTemplate�5�     5�     n� 5�     $G PART_TopHeaderGripper��6    $	��Top=�5�      8�� 5�      n� 5�     $G PART_BottomHeaderGripper��6    $��Bottom=�5�      8�� 5�      5�     5�     5�     5�     5�     