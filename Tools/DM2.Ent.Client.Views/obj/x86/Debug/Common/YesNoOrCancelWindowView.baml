   M S B A M L   `   `   `  ����b  ^Microsoft.Windows.Shell, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35bFclr-namespace:Microsoft.Windows.Shell;assembly=Microsoft.Windows.ShellMicrosoft.Windows.Shell  5   	    DM2.Ent.Client.Views/   )DM2.Manager.Views.YesNoOrCancelWindowView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   XshellFclr-namespace:Microsoft.Windows.Shell;assembly=Microsoft.Windows.Shell     5   	   ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  5   	     � AllowsTransparency  .  � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5	   	      DialogTemplate#��[  5
   	   $	��159��5   	   $	��565��5   	   *   $Microsoft.Windows.Shell.WindowChrome   WindowChrome 5        5   
      GlassFrameThickness$ 0q�6      ResizeBorderThickness$ 4q�5         CaptionHeight$ 29Q�5         CornerRadius$ 0��5      .��5      � ���#f8f8f8	 � ColumnDefinitions	 5   
   �� 5      $��* �6   �� 5      $
��1.5* �6   �� 5      $
��1.5* �6   �� 5      $
��1.5* �6   �� 5      $��* �6   
 � RowDefinitions
 5   
   �� 5      $
��100* �6   �� 5      $	��50* �6   .+�5    
   ��  �� Orientation$ 
Horizontal=�6   $��Center=�6.   $	��Top=�6K   $��0,21,0,0q�6c   $��6��5!      .+�5"      �� $��30��6   $��30��6    $��0,0,11,0q�6,   ��5#      �� 5$       �� ImageSource$* $/Resources/Images/White/question.png��6!   6V   5&      �� 5'      $
��Bold"�6   $	��450��6*   $
��Wrap=�5(      $
��Left=�5)       �� TextAlignment$
 Left=�5*      $��Center=�5,      ��5+      �� *ConfirmInfo+5,   5   5-      �� 5.   
   $��1��6     	btn_89_25#��[ 6   $��1��5/      $	��Top=�6!     Yes#���  50       �� 	IsDefault .   Caliburn.Micro.Message   Attach$) #[Event Click] = [Action ConfirmYes]��52      53      �� 54   
   $��1��6   $	��Top=�6   $( "[Event Click] = [Action ConfirmNo]��55        No#���  56      #��[ 57      $��2��58      6#   �� 59   
   $��1��6   $	��Top=�6   $, &[Event Click] = [Action ConfirmCancel]��5:       
 Cancel#���  5;      #��[ 5<      $��3��5=      6#   5>      5?      