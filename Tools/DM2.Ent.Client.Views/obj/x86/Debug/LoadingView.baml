   M S B A M L   `   `   `  ����F  BGifImageLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:GifImageLib;assembly=GifImageLibGifImageLib  5   	    DM2.Ent.Client.Views#   DM2.Manager.Views.LoadingView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   AgifLib.clr-namespace:GifImageLib;assembly=GifImageLib     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	    � AllowsTransparency .    loadingTemplate#���   5   	    � WindowStyle$
 None=�5	   	    � Title$ Loading��5
   	   $	��332��5   	   $	��392��5   	    � 	Resources 5       �� 
TargetType �� CornerRadius��%�   &	        �� 5   
   " �5      .R�5      �� 
�������$ 5��5      ._�5      �� #��z��6#   6I   5      5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   6,   �� 5      6   �� 5      $��24 �6   6*   .+�5   
   �� $��0��6   $	��110��5      $��24��5      $
��Left=�5      $��	20,20,0,0q�5       ��5!      �� 5"      	 �� 
TargetType"	 ��6   
 �� Triggers
 5#      �� 5$       �� Binding 6&   �� *IsBancLogix+ �� Value True5%   &   .a�5&      ��  �� Property � �� �� Value$I C/DM2.Manager.Views;component/Resources/Images/White/logo_loding.png��5'   %   5(      �� 5)       6&   �� *IsBancLogix+	 False5*   &   .a�5+      ��  � ��$S M/DM2.Manager.Views;component/Resources/Images/White/forexStar_LoadingLogo.png��5,   %   5-      5/      51      �� 52   
   $��1��6   $��Center=�53      $��Center=�54      .+�55         GifImageLib.GifImage     Source$D >/DM2.Manager.Views;component/Resources/Images/White/loding.gif��6   $��64��56      $��64��57      �� 58      $��5q�6   $��15(�59      
�������Loading....5:   7   5;      �� 5<   
   $ 0,0,5,5��6   $��2��5=      ���#e7e7e7._�5?      �� $��Center=�6   $��13(�5@      
��������� 5A   ,   $=�7Copyright © 2010-2016 KVB Kunlun. All rights reserved.��60   6r   5B      5C      5D      