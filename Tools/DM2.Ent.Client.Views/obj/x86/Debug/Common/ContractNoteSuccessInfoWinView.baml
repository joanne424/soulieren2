   M S B A M L   `   `   `  ����b  ^Microsoft.Windows.Shell, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35bFclr-namespace:Microsoft.Windows.Shell;assembly=Microsoft.Windows.ShellMicrosoft.Windows.Shell  5   	    DM2.Ent.Client.Views6   0DM2.Manager.Views.ContractNoteSuccessInfoWinView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  5   	   RshellFclr-namespace:Microsoft.Windows.Shell;assembly=Microsoft.Windows.Shell  5   	     � Title$  SuccessInfoWin��5   	    � AllowsTransparency .  � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5	   	    � WindowStartupLocation$ CenterScreen=�5
   	      DialogTemplate#��[  5   	   $	��159��5   	   $	��565��5   	   *   $Microsoft.Windows.Shell.WindowChrome   WindowChrome 5        5   
      GlassFrameThickness$ 0q�6      ResizeBorderThickness$ 4q�5         CaptionHeight$ 29Q�5      	   CornerRadius$	 0��5      .��5      � ���#f8f8f8
 � ColumnDefinitions
 5   
   �� 5      $
��100* �6   �� 5      $
��100* �6   �� 5      $
��100* �6   �� 5      $
��100* �6    � RowDefinitions 5   
   �� 5      $
��100* �6   �� 5      $	��50* �6   .+�5!   
   ��  �� Orientation$ 
Horizontal=�6   $��Center=�5"      $	��Top=�5#      $��0,21,0,0q�5$      $��5��5%      .+�5&      �� $��30��6   $��30��5'      $��0,0,11,0q�5(      ��5)      �� 5*       �� ImageSource$) #/Resources/Images/White/correct.png��6!   6U   5,      �� 5-      $
��Bold"�6   $	��450��5.      $
��Wrap=�5/      $
��Left=�50       �� TextAlignment$
 Left=�51      $��Center=�53      ��52      �� *ConfirmInfo+53   5   54      �� 55   
   $��1��6   $��1��6   $��Center=�56      $	��Top=�57       �� 	IsDefault .   Caliburn.Micro.Message   Attach$, &[Event Click] = [Action ConfirmCancel]��59        OK#���  5:      $��6,0,0,0q�5;        	btn_89_25#��[ 5<      5=      �� 5>   
   $��1��6   $��2��6   $��Center=�5?      $	��Top=�5@       . $&  [Event Click] = [Action Preview]��5B        Preview#���  5C      $��6,0,0,0q�5D      #��[ 5E      5F      5P      5Q      