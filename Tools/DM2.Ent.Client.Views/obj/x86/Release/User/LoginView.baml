   M S B A M L   `   `   `  ���� V  RDM2.Ent.Client.Views, Version=1.0.6407.25883, Culture=neutral, PublicKeyToken=nullS.clr-namespace:DM2.Ent.Client.Views.ExtendClass DM2.Ent.Client.Views.ExtendClass  b ^Microsoft.Windows.Shell, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35bFclr-namespace:Microsoft.Windows.Shell;assembly=Microsoft.Windows.ShellMicrosoft.Windows.Shell Q MBanclogix.Controls.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null_Dclr-namespace:Banclogix.Controls.WPF;assembly=Banclogix.Controls.WPFBanclogix.Controls.WPF c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity )    #DM2.Ent.Client.Views.User.LoginView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Fextendclass.clr-namespace:DM2.Ent.Client.Views.ExtendClass     XshellFclr-namespace:Microsoft.Windows.Shell;assembly=Microsoft.Windows.Shell     ^	 ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org 	 NwpfDclr-namespace:Banclogix.Controls.WPF;assembly=Banclogix.Controls.WPF 	 XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity 	   � AllowsTransparency  .  � WindowStyle$
 None=� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=�   LoginDialogTemplate#���    	 Login � Title# �  ���#333333$	��326��$	��420��*  $Microsoft.Windows.Shell.WindowChrome   WindowChrome      GlassFrameThickness$ 0q�   ResizeBorderThickness$ 4q�   CaptionHeight$ 29Q�	   CornerRadius$	 0��
 � 	Resources
  �� 
TargetType �� Padding �� CornerRadius  WindowBorderBrush J� Effect3  -System.Windows.Media.Effects.DropShadowEffect   Color   
BlurRadius   ShadowDepth � RowDefinitions �Name �� 
StartPoint �� EndPoint � Color � Offset   IsHitTestVisibleInChrome � Cursor 	 Caliburn.Micro.Message   Attach J� RenderTransformOrigin �� TextAlignment��%(  &	        7  �� " �.R��� ._��� ���#e2e2e2$ 2q�$��1q�$ 4��$��10q�8��     $ #6e6e6e��$ 15Q�$ 0Q�._�� .+��  �� $��46 ��� $��* �.+�� $ WindowHeader��-   $��0������ $	 0,0&�$	 0,1&���� � $ 	#FFF6F6F6��$ 0Q�� $ 	#FFD6D6D6��$ 1Q�]� $��0��.+���  . $
 Hand��$# [Event Click] = [Action Exit]��$��
0,13,14,11q�$��15��$��15��$��Right=�$ -0.118,0.333&����� " ��.R��� $L��F/DM2.Ent.Client.Views;component/Resources/Images/White/Login/Close.png���� $
 Left=�$
��26px(�$��28,0,0,0q�$��Center=�$
��Left=�$
��Bold"����� *	LoginLb+��Z� ���� $ 0.5,1&�$ 0.5,0&�.��� $ 	#FF373636��$ 1Q�� $ 	#FFA5A5A5���� $��1��$��0,1,0,0q����	#FF979797._��� #��z��.��� ���#ededed �� $	��188 ��� $��* �.+��� $��0��$��0,0,0,1q�$��2��._��� $��0,0,0,-1q�.+��� $��
45,46,42,0q�
�������$��45��$ 3��$��1q����#fafafa._�� .+��� $��10,0,0,0q�$
��Left=�$��Center=�$K��E/DM2.Ent.Client.Views;component/Resources/Images/White/Login/name.png��$��17��$��17���� ���#999999$��
47,10,2,12q�$��Center=�$��21�� �� SelectedValuePath$ UserId��$8 2[Event DropDownOpened] = [Action HistoryBoxOpened]������ *
UserList+ �� Mode$ OneWay=� �� SelectedItem �� *CurrentUser+����   �� 
TargetType"  ��.��� ! �� Property! � ��" �� Value$" 0q��� ! � ��
" ���� �� ! � ��" Z� ���� .R�� # � ColumnDefinitions# �� $��* ��� $��30 �.+��� ���� *UserId+$ OneWay=��� $��X��
���  ��$��1��$	��5 0q�.  (System.Windows.Interactivity.Interaction$   Triggers$ /  )System.Windows.Interactivity.EventTrigger  %   	EventName$% Click��.  (System.Windows.Interactivity.TriggerBase&   Actions.& " 	 Caliburn.Micro.ActionMessage  '   
MethodName$' OnItemClicked��(   
Parameters.(  	 Caliburn.Micro.Parameter  )   Value) �� *UserId+�� -   * ��Name$* UserId��$2 ,[Event LostFocus] = [Action UserIdLostFocus]��  LoginTextBox#��[ 
������ $��0q����#999999$��
47,10,0,12q�$��16(�$��Center=�$��21��$ 	0.486,1.5&�$
��Left=�$	��238��+	  %Banclogix.Controls.WPF.FocusExtension+ 	  	IsFocused+ �� , �� Path$, IsUserIdFocused�$ TwoWay=����� *UserId+$ TwoWay=�- �� UpdateSourceTrigger$- PropertyChanged=�. �� ValidatesOnDataErrors. . .���� 
�������$��
45,24,42,0q�$��40��$ 3��$��1q����#fafafa._�� $��40������ "  �/ �� Triggers/ �� 0 �� Binding0 �� *IsPasswordFocused+1 �� Value1 True.a��� ! �@+�FocusedElement" �� 2 �� ElementName$2 Password��.+��� $��10,0,0,0q�$
��Left=�$��Center=�$O��I/DM2.Ent.Client.Views;component/Resources/Images/White/Login/password.png��$��17��$��17��G� -   3 G�Name$3 Password��$4 .[Event LostFocus] = [Action PasswordLostFocus]��  LogionPasswaord#��[ 
������ $��0q�8
   2DM2.Ent.Client.Views.ExtendClass.PasswordBoxHelper4 
  Attach4 . ���#999999$��47,0,0,0q�$��16(�$��Center=�+ �� $, IsPasswordFocused�$ TwoWay=�5 
  Password5 �� *
Password+$ TwoWay=�$- PropertyChanged=�. . �� $��45,187,0,52q�$��0��$
��Left=�$��Center=����#f4a88b$��2��  SaveLoginInfo#���  6 d� 	IsChecked6 �� *IsSaveLoginInfo+�� -   7 ��Name$7 Login��8 �� 	IsDefault8 . $��0��$��90��$��26��  btnPage#��[ $
��Left=�$��Center=�$��260,187,0,38q�$S M[Event Click] = [Action TryLogin];[Event LostFocus] = [Action LoginLostFocus]��#���  $��2��$
��Bold"�+ �� $, IsLoginFocused�$ TwoWay=��� $��1��$	��304��$��	45,30,0,0q�$
��Left=�$	��Top=�$
��Wrap=�
���  ��$��33��$
��Bold"����� *
ErrorMsg+$ TwoWay=�