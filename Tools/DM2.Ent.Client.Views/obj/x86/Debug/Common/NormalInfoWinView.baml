   M S B A M L   `   `   `  ����b  ^Microsoft.Windows.Shell, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35bFclr-namespace:Microsoft.Windows.Shell;assembly=Microsoft.Windows.ShellMicrosoft.Windows.Shell  5   	    DM2.Ent.Client.Views)   #DM2.Manager.Views.NormalInfoWinView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  5   	   RshellFclr-namespace:Microsoft.Windows.Shell;assembly=Microsoft.Windows.Shell  5   	     � AllowsTransparency  .  � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5	   	      DialogTemplate#��[  5
   	   $	��159��5   	   $	��565��5   	   *   $Microsoft.Windows.Shell.WindowChrome   WindowChrome 5        5   
      GlassFrameThickness$ 0q�6      ResizeBorderThickness$ 4q�5         CaptionHeight$ 29Q�5         CornerRadius$ 0��5      .��5      � ���#f8f8f8	 � ColumnDefinitions	 5   
   �� 5      $
��100* �6   �� 5      $
��100* �6   
 � RowDefinitions
 5   
   �� 5      $
��100* �6   �� 5      $	��50* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��Center=�5      $	��Top=�5       $��0,21,0,0q�5!      $��2��5"      .+�5#      �� $��30��6   $��30��5$      $��0,0,11,0q�5%      ��5&      �� 5'       �� ImageSource$&  /Resources/Images/White/info.png��6!   6R   5)      �� 5*      $
��Bold"�6   $	��450��5+      $
��Wrap=�5,      $
��Left=�5-       �� TextAlignment$
 Left=�5.      $��Center=�50      ��5/      �� *ConfirmInfo+50   5   51      �� 52   
   $��1��6   $��2��53      $��0��54       �� 	IsDefault . $��Center=�56      $	��Top=�57        Caliburn.Micro.Message   Attach$, &[Event Click] = [Action ConfirmCancel]��58        OK#���  59      $��6,0,0,0q�5:        	btn_89_25#��[ 5;      66   5<      5=      