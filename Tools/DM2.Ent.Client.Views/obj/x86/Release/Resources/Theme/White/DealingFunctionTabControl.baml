   M S B A M L   `   `   `  ���� R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   tabControlBorderBrush  �� Color  tabItemHeaderBackground  tabItemNormalHeader j� Stretch �� ImageSource  tabItemSelectedHeader  DFTabControl �� 
TargetType �� Property �� Value �� 
TargetType J� SnapsToDevicePixels-   'System.Windows.Input.KeyboardNavigation    TabNavigation	 � ColumnDefinitions
 ��Name � RowDefinitions ��Name ��Name    TabIndex K� ZIndex ��Name K� IsItemsHost �Name    DirectionalNavigation ��Name �� Triggers P� Property P� Value �� 
TargetName  DFTabControlItem'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState �� CornerRadius �� EndPoint �� 
StartPoint � Color � Offset�� F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml      %�
  &	        &	      &	 &     &	 k     &	 �     &	 �    �� $  #B1B4B7���� $  #6C6C6C���� $
 Fill=�$4 .../../Images/White/ToolWindowTitleNormalBG.png���� $
 Fill=�$6 0../../Images/White/ToolWindowTitleSelectedBG.png���� " ��.���  � ��
 ���� ��  � ��# �   ��  � ��$ 0q���  � ��$ 0q���  � �� �� " ��.R�� }�.  . $ Local=�	 �� $
 ColumnDefinition0���� $
 ColumnDefinition1��$��0 � �� $ RowDefinition0��$
��Auto ��� $ RowDefinition1��$��* �.+��� $ HeaderBorder��$��0��$��0,2,0,0q�$ 1��$ 1��$��0��
�������$��0,1,0,0q�._��� $ HeaderPanel�����#e1e1e1 . � $ ContentPanel��$��0��$ 	Contained=�$��1��$ 2��$ Local=�.+��� $ PART_SelectedContentHost��$��SelectedContent��$��0q�# z  P�  �@��TabStripPlacement$ Bottom=�. ��� $ HeaderBorder�� � ��$ 1���� $ ContentPanel�� � ��$ 0���� $ RowDefinition0�� � ��$ * ��� $ RowDefinition1�� � ��$
 Auto ��� $ HeaderBorder�� � ��$ 0,0,0,1q�P�  �@��TabStripPlacement$
 Left=�. ��� $ HeaderBorder�� � ��$ 0���� $ ContentPanel�� � ��$ 0���� $ HeaderBorder�� � ��$ 0���� $ ContentPanel�� � ��$ 1���� $ ColumnDefinition0�� � ��$
 Auto ��� $ ColumnDefinition1�� � ��$ * ��� $ RowDefinition0�� � ��$ * ��� $ RowDefinition1�� � ��$ 0 ��� $ HeaderBorder�� � ��$ 2,0,0,0q�P�  �@��TabStripPlacement$ Right=�. ��� $ HeaderBorder�� � ��$ 0���� $ ContentPanel�� � ��$ 0���� $ HeaderBorder�� � ��$ 1���� $ ContentPanel�� � ��$ 0���� $ ColumnDefinition0�� � ��$ * ��� $ ColumnDefinition1�� � ��$
 Auto ��� $ RowDefinition0�� � ��$ * ��� $ RowDefinition1�� � ��$ 0 ��� $ HeaderBorder�� � ��$ 0,0,2,0q�P�  � {� .  . ��� # � �� � ���� " ��.���  � ��$	 150����  � �� �� " ��.R�� $
 Root��   SelectionStates��.   
Unselected��  Selected��  CommonStates��.   
Normal��  	MouseOver��  Disabled��.+��� $ Border��$��-3,0,3,0q�$ 0,0,0,0��
�������$��0,0,1,0q����� $ 0.5,1&�$ 0.5,0&�.��� $ 	#FFD4D4D4��� $ 	#FFD6D6D6��$ 1Q�� $ 	#FFA5A5A5��$	 0.5Q�� $ 	#FFC1C1C1��$ 0.499Q�._��� $ ContentSite��$��Center=�$��Center=�$��Header��$��0q���.  P�  �@��
IsSelected . . ���  �@K�ZIndex$	 100���� $ Border�� � �� �� $ 0.5,1&�$ 0.5,0&�.��� $ 	#FFE4E4E4��� $ 	#FFFEFEFE��$ 1Q�