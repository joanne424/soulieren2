   M S B A M L   `   `   `  ���� V  RDM2.Ent.Client.Views, Version=1.0.6411.30809, Culture=neutral, PublicKeyToken=null;"clr-namespace:DM2.Ent.Client.ViewsDM2.Ent.Client.Views  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   NormalBrush  �� 
StartPoint �� EndPoint � Color � Offset  HorizontalNormalBrush  
LightBrush  HorizontalLightBrush  	DarkBrush  PressedBrush  DisabledForegroundBrush �� Color  DisabledBackgroundBrush  WindowBackgroundBrush 	 SelectedBackgroundBrush 
 NormalBorderBrush  HorizontalNormalBorderBrush  DefaultedBorderBrush  PressedBorderBrush  DisabledBorderBrush  SolidBorderBrush  LightBorderBrush  
GlyphBrush  LightColorBrush  TabItemBorder  TabItemSelected  TabItemSelectedForeground  TabItemNormal �� 
TargetType  queuetabctl �� Property �� Value �� 
TargetType-   'System.Windows.Input.KeyboardNavigation	    TabNavigation
 � RowDefinitions K� ZIndex K� IsItemsHost    TabIndex �� CornerRadius    DirectionalNavigation �� Triggers P� Property P� Value �� 
TargetName  TabItemLeft E� IsClosed E� 
StartPoint �� Point �� ElementName �� Path  LeftLowerRightConverter �� 	Converter � Point1 & "LeftUpperRightCurvePoint1Converter � Point2 & "LeftUpperRightCurvePoint2Converter  LeftUpperRightConverter ��Name  TabItemRight  RightLowerRightConverter  RightUpperRightConverter�� F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     ,views"clr-namespace:DM2.Ent.Client.Views  %/  &	        &	 V     &	 �     &	     &	 X    &	 �    &	 8    &	 H    &	 X    &		 h    &	
 x    &	 �    &	 $    &	 z    &	 �    &	 �    &	 �    &	      &	     &	      &	 3    &	 �    &	 �    &	 (    7  7  7  7  &	     7  7  7  7  &	 �	    7  7  �� $	  0,0&�$	 0,1&���� � $
 #FFF��$	 0.0Q�� $
 #CCC��$	 1.0Q��� $	  0,0&�$	 1,0&���� � $
 #FFF��$	 0.0Q�� $
 #CCC��$	 1.0Q��� $	  0,0&�$	 0,1&���� � $
 #FFF��$	 0.0Q�� $
 #EEE��$	 1.0Q��� $	  0,0&�$	 1,0&���� � $
 #FFF��$	 0.0Q�� $
 #EEE��$	 1.0Q��� $	  0,0&�$	 0,1&���� � $
 #FFF��$	 0.0Q�� $
 #AAA��$	 1.0Q��� $	  0,0&�$	 0,1&���� � $
 #BBB��$	 0.0Q�� $
 #EEE��$	 0.1Q�� $
 #EEE��$	 0.9Q�� $
 #FFF��$	 1.0Q��� $
 #888���� $
 #EEE���� $
 #FFF���� $
 #DDD���� $	  0,0&�$	 0,1&���� � $
 #CCC��$	 0.0Q�� $
 #444��$	 1.0Q��� $	  0,0&�$	 1,0&���� � $
 #CCC��$	 0.0Q�� $
 #444��$	 1.0Q��� $	  0,0&�$	 0,1&���� � $
 #777��$	 0.0Q�� $
 #000��$	 1.0Q��� $	  0,0&�$	 0,1&���� � $
 #444��$	 0.0Q�� $
 #888��$	 1.0Q��� $
 #AAA���� $
 #888���� $
 #AAA���� $
 #444���� $
 #DDD���� $ #b5b5b5���� $ 0.5,1&�$  0.5,0&�.��� $ 	#FFE5E5E5��� $ 	#FFFEFEFE��$ 1Q��� $ Black���� $ 0.5,1&�$  0.5,0&�.��� $ 	#FFF9F9F9��� $ 	#FFDDDDDD��$	 0.5Q�� $ 	#FFDDDDDD��$ 1Q�� $ 	#FFF9F9F9��$ 0.492Q��� " ��.���  �@�OverridesDefaultStyle . ��  �@J�SnapsToDevicePixels . ��  � �� �� " ��.R�� $	 Local=�
 �� $��* ��� $
��auto �.+��� $��HeaderPanel��$��1��$ 1��$��0,0,0,0q� . $ 1��
������ �� $��Border��$��0��8��  8�� $��1q�$ 0��$	 Local=�$ 	Contained=�$ 2��._��� $��PART_SelectedContentHost��$��4q�$��SelectedContent�� P�  � {� .  . ��� 8   � ���� $ Border��8   � ���� " ��.���  � ��$ 29����  � ��$ 1,0,0,1q���  � �� �� " ��.R�� .+��� $��Border��._�� .+��� $
��left��.+�F� $��	pathPanel��#���  #���  ��B� .��E�  . $	 0,0&�.���� $
 0,29&���  �� $
 left��$ ActualWidth�8   �  �� $
 left��$ ActualWidth�8   �� $
 left��$ ActualWidth�8  ��  �� $
 left��$ ActualWidth�8  �� $ ContentSite��$��12(����#393939$��Center=�$��Center=�$��Header����.  P�  �@��
IsSelected . . ���  �@K�ZIndex$	 100���� $ ContentSite��# �   � ���� $ 	pathPanel��# �   � ���� " ��.���  � ��$ 29����  � ��$ 0,0,1,1q���  � �� �� " ��.R�� .+��� $��Border��._�� .+��� $��right��.+�F� $��	pathPanel��#���  #���  $��1����B� .��E�  . $
 -8,0&�.��� $
 7,25&�$
 8,26&��� $ 13,29&���  �� $ right��$ ActualWidth�8   ��  �� $ right��$ ActualWidth�8  �� $ ContentSite��$��12(����#393939$��Center=�$��Center=�$��Header����.  P�  �@��
IsSelected . . ���  �@K�ZIndex$	 100���� $ ContentSite��# �   � ���� $ 	pathPanel��# �   � ��