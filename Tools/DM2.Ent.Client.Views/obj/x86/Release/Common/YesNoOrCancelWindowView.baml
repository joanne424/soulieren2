   M S B A M L   `   `   `  ���� b  ^Microsoft.Windows.Shell, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35bFclr-namespace:Microsoft.Windows.Shell;assembly=Microsoft.Windows.ShellMicrosoft.Windows.Shell   DM2.Ent.Client.Views9   3DM2.Ent.Client.Views.Common.YesNoOrCancelWindowView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     XshellFclr-namespace:Microsoft.Windows.Shell;assembly=Microsoft.Windows.Shell     ^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org    � AllowsTransparency  .  � WindowStyle$
 None=� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=�   DialogTemplate#��[  $	��159��$	��565��*   $Microsoft.Windows.Shell.WindowChrome   WindowChrome      GlassFrameThickness$ 0q�   ResizeBorderThickness$ 4q�   CaptionHeight$ 29Q�   CornerRadius$ 0��.��� ���#f8f8f8	 � ColumnDefinitions	 �� $��* ��� $
��1.5* ��� $
��1.5* ��� $
��1.5* ��� $��* �
 � RowDefinitions
 �� $
��100* ��� $	��50* �.+���  �� Orientation$ 
Horizontal=�$��Center=�$	��Top=�$��0,21,0,0q�$��6��.+��� $��30��$��30��$��0,0,11,0q�����  �� ImageSource$* $/Resources/Images/White/question.png���� $
��Bold"�$	��450��$
��Wrap=�$
��Left=� �� TextAlignment$
 Left=�$��Center=����� *ConfirmInfo+�� $��1��  	btn_89_25#��[ $��1��$	��Top=�  Yes#���   �� 	IsDefault .   Caliburn.Micro.Message   Attach$) #[Event Click] = [Action ConfirmYes]���� $��1��$	��Top=�$( "[Event Click] = [Action ConfirmNo]��  No#���  #��[ $��2���� $��1��$	��Top=�$, &[Event Click] = [Action ConfirmCancel]�� 
 Cancel#���  #��[ $��3��