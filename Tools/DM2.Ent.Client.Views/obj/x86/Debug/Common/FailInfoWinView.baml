   M S B A M L   `   `   `  ����b  ^Microsoft.Windows.Shell, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35bFclr-namespace:Microsoft.Windows.Shell;assembly=Microsoft.Windows.ShellMicrosoft.Windows.Shell  5   	    DM2.Ent.Client.Views'   !DM2.Manager.Views.FailInfoWinView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  5   	   RshellFclr-namespace:Microsoft.Windows.Shell;assembly=Microsoft.Windows.Shell  5   	     � AllowsTransparency  .  � Topmost .  � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5	   	    � WindowStartupLocation$ CenterScreen=�5
   	      DialogTemplate#��[  5   	   $	��159��5   	   $	��565��5   	   *   $Microsoft.Windows.Shell.WindowChrome   WindowChrome 5        5   
      GlassFrameThickness$ 0q�6      ResizeBorderThickness$ 4q�5         CaptionHeight$ 29Q�5      	   CornerRadius$	 0��5      .��5      � ���#f8f8f8
 � ColumnDefinitions
 5   
   �� 5      $
��100* �6   �� 5      $
��100* �6    � RowDefinitions 5   
   �� 5      $
��100* �6   �� 5      $	��50* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��Center=�5       $	��Top=�5!      $��0,21,0,0q�5"      $��2��5#      .+�5$      �� $��30��6   $��30��5%      $��0,0,11,0q�5&      ��5'      �� 5(       �� ImageSource$' !/Resources/Images/White/wrong.png��6!   6S   5*      �� 5+      $
��Bold"�6   $	��450��5,      $
��Wrap=�5-      $
��Left=�5.       �� TextAlignment$
 Left=�5/      $��Center=�51      ��50      �� *ConfirmInfo+51   5   52      �� 53   
   $��1��6   $��2��54      $��0��55       �� 	IsDefault . $��Center=�57      $	��Top=�58        Caliburn.Micro.Message   Attach$, &[Event Click] = [Action ConfirmCancel]��59        OK#���  5:      $��6,0,0,0q�5;        	btn_89_25#��[ 5<      66   5=      5>      