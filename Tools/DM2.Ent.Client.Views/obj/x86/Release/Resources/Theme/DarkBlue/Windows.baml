   M S B A M L   `   `   `  ���� V  RDM2.Ent.Client.Views, Version=1.0.6411.30809, Culture=neutral, PublicKeyToken=null;"clr-namespace:DM2.Ent.Client.ViewsDM2.Ent.Client.Views  M    GDM2.Ent.Client.Views.Resources.Theme.DarkBlue.WindowsResourceDictionaryR NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089^ ZCaliburn.Micro.Platform, Version=3.0.1.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21fc _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35 !  CommonOnlyCloseWindowTemplate  �� 
TargetType � 	Resources �� 
TargetType �� Property �� Value � RowDefinitions �� Padding � ColumnDefinitions � Title	 � Cursor  Caliburn.Micro.Message
   Attach �� Angle $  NormalWindowWithMaximizeTemplate �� RelativeSource �� AncestorLevel �� Mode �� AncestorType �� Path0   *DM2.Ent.Client.Views.ControlAttachProperty,   %IsMinimizeWindowButtonBehaviorEnabled ��Name,   %IsMaximizeWindowButtonBehaviorEnabled ��Name �� Triggers �� Binding �� Value �� Triggers ^� 
Conditions �� Binding �� ElementName �� Value  CommonOnlyCloseWindowStyle ! NormalWindowWithMaximizeStyle " NormalWindowWithMaximizeStyle1  
TitleStyle  MainWindowTemplate �� BitmapScalingMode P� Property P� Value  �� 
TargetName! �Name  File(F)  ChangePassword" m�Name 	 Trade(T) 
 NewHedgeSpotForward  NewHedgeSwapDeal  BankCashTransfer  BankAccountActualCash  	Report(R)  BankCashTransferReport  BankCashFlowList  BankAccountActualCashReport  Tools(S)  BusinessUnit# �� Orientation  New  BusinessUnitListTitle  User  UserListTitle  Counterparty  CounterpartyListTitle  CounterpartyGroup  CounterpartyGroupListTitle  BankAccountManagement  NewBankAccount  BankAccountList$ K� ZIndex% d� 	IsChecked  bool2VisibilityConverter& �� 	Converter' � Orientation   
MenuButton( � ToolTip.  (System.Windows.Interactivity.Interaction)   Triggers/  )System.Windows.Interactivity.EventTrigger*   	EventName.  (System.Windows.Interactivity.TriggerBase+   Actions"  Caliburn.Micro.ActionMessage,   
MethodName-   
Parameters  Caliburn.Micro.Parameter.   Value/ J� SnapsToDevicePixels   F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     YpresentationOptionsAhttp://schemas.microsoft.com/winfx/2006/xaml/presentation/options  (calhttp://www.caliburnproject.org  Bi:http://schemas.microsoft.com/expression/2010/interactivity  ,local"clr-namespace:DM2.Ent.Client.Views  %�)  &	        &	 J    &	 �
    7   &	 �    7  &	 �    7  &	 �    &	 :    7  �� "  � ���� '��       " ��.���  � ��$ Microsoft YaHei)���  � ��$ Right=���  � ��$ Center=���  � ��$ Medium&���  � ��$ 12(��� '��       " ��.���  � ��$ Right=���  � ��$ Center=�.R�� -    �� $��40 ��� $��* �.+��� -   $��0�����#264E72$ 0q�._�� $��40��$��0q� �� $��* � �� $��* ��� $��40 �.+��� #��z $
��Left=�$��Center=�$
��Auto��$��0��$��16,0,0,0q�$	��500"�$��20(�$��Normal$����#FFFFFF$��Microsoft YaHei)��� $��Images/CommonClose.png��$
��None=�$��Center=�$��Right=�$��15��$��15��$
	 Hand��$��1��$��	0,14,20,0q�$&
  [Event MouseDown]=[Action Close]��z�V� .~��� �� �� $	 -45Q�T� �� $��1��$��1q����#BCBCBC���#FFFFFF$ 10,20,10,10q�._��� .u��� �� "  � ���� '��       " ��.���  � ��$ Microsoft YaHei)���  � ��$ Right=���  � ��$ Center=���  � ��$ Medium&���  � ��$ 12(��� '��       " ��.���  � ��$ Right=���  � ��$ Center=�.R�� -    �� $��40 ��� $��* �.+��� -   $��0�����#264E72._�� $
��15,0q�$��Center=� �� �� $��26 ��� $��36 ��� $��26 �.+��� $��0��$
��Left=�$��Center=�$	��500"�$��20(����#FFFFFF$��Microsoft YaHei)�����  �� $ 1��$ FindAncestor=�$ WindowL�$ Title��� $��1��$��Center=�$��Center=�$��25��$��16�� . $7��1local:ControlAttachProperty.MinimizeWindowCommand������  �� *FindAncestor+" ����� .R��� $4��./Resources/Images/DarkBlue/window_minimize.png���� $ MaxNormalBtn��$��2��$��Center=�$��Center=�$��25��$��16�� . $7��1local:ControlAttachProperty.MaximizeWindowCommand������  �� *FindAncestor+" ����� "  ��.R�� .+��� $ maxImg��$4��./Resources/Images/DarkBlue/window_maximize.png������ " ��.���  � y�$ 	Collapsed=� ��  �� *WindowState+ �� *FindAncestor+" � 	Maximized.a���  � y�$ Visible=��� $ 	normalImg��$2��,/Resources/Images/DarkBlue/window_normal.png������ " ��.���  � y�$ Visible=� ��  �� *WindowState+ �� *FindAncestor+" � 	Maximized.a���  � y�$ 	Collapsed=� ^�  ��  �� *	IsPressed+$ MaxNormalBtn�� True��  �� *
Visibility+$ maxImg�� Visible^�  ��  �� *	IsPressed+$ MaxNormalBtn�� True��  �� *
Visibility+$ 	normalImg�� Visible�� $��3��$��Center=�$��23��$��15��$"
 [Event Click]=[Action Close]������ .R��� $1��+/Resources/Images/DarkBlue/window_close.png���� $��1��$��1q����#BCBCBC���#FFFFFF$ 10q�._��� �� " �.���  �@�AllowsTransparency . ��  �@�WindowStyle$
 None=���  � ��
 ���� ��  � ��8   ��  �@�
ResizeMode$ NoResize=���  �@�ShowInTaskbar . ��  �@�Topmost . �� " �.���  �@�AllowsTransparency . ��  �@�WindowStyle$
 None=���  � ��
 ���� ��  � ��8   ��  �@�
ResizeMode$ CanResizeWithGrip=���  �@�ShowInTaskbar . ��  �@�Topmost . ��  �@�WindowState$ Normal=��� " �.���  �@�AllowsTransparency . ��  �@�WindowStyle$
 None=���  � ��
 ���� ��  � ��8   ��  �@�ShowInTaskbar . ��  �@�Topmost . ��  �@�WindowState$ Normal=��� " ��.���  � ��$
 Left=���  � ��$ Medium&���  � ��$	 400"���  � ��$ 16(���  � �� �#3E8ACC�� "  �.R�� -    �� $
��Auto ��� $��20 ��� $
��Auto ��� $��* ��� $
��Auto � �� $
��Auto ��� $��* ��� $
��Auto �.+��� -   $��0��$��2��$��0��$��3�����#EFEFEF$��1,1,1,0q����#797979$ 0q�._�� $��0q� �� $��* � �� $	��130 ��� $	��140 ��� $��* ��� $	��140 �.+��� $��0��$��0��$	��111��$��25��$/��)/Resources/Theme/DarkBlue/Images/logo.png��$ NearestNeighbor=�$
��Left=�$��8,0,0,0q��� $��0��$��1��$��DM2 Ent Client��$
��Left=�$��Center=�$
��Auto��$��0,0,0,0q�$	��500"�$��16(�$��Normal$����#264E72$��Microsoft JhengHei)��� -   $��0��$��3��$��0,0,73,0q�$��Right=�$	��Top=�$��29��$��19������ "  ��.R��� ._��� $��tgImg��$?��9/Resources/Images/White/MainWindowHeader/MinBtnNormal.png�� P�  �@J�IsMouseOver . . ��� $  tgImg�� � ��$> 8/Resources/Images/White/MainWindowHeader/MinBtnHover.png��P�  � �� . . ��� $  tgImg�� � ��$> 8/Resources/Images/White/MainWindowHeader/MinBtnClick.png���� $��	maxResBtn��-   $��0��$��3��$��0,0,47,0q�$��Right=�$	��Top=�$��26��$��19������ "  ��.R�� .+��� $��maxImg��$?��9/Resources/Images/White/MainWindowHeader/MaxBtnNormal.png������ " ��.���  � y�$ Visible=� ��  �� *WindowState+ �� *FindAncestor+" � 	Maximized.a���  � y�$ 	Collapsed=��� $��resImg��$C��=/Resources/Images/White/MainWindowHeader/RestoreBtnNormal.png������ " ��.���  � y�$ 	Collapsed=� ��  �� *WindowState+ �� *FindAncestor+" � 	Maximized.a���  � y�$ Visible=� ^�  ��  �� *	IsPressed+$ 	maxResBtn�� True��  �� *
Visibility+$ maxImg�� Visible0��� $  maxImg�� � ��$> 8/Resources/Images/White/MainWindowHeader/MaxBtnClick.png��^�  ��  �� *IsMouseOver+$ 	maxResBtn�� True��  �� *
Visibility+$ maxImg�� Visible0��� $  maxImg�� � ��$> 8/Resources/Images/White/MainWindowHeader/MaxBtnHover.png��^�  ��  �� *	IsPressed+$ 	maxResBtn�� True��  �� *
Visibility+$ resImg�� Visible0��� $  resImg�� � ��$B </Resources/Images/White/MainWindowHeader/RestoreBtnClick.png��^�  ��  �� *IsMouseOver+$ 	maxResBtn�� True��  �� *
Visibility+$ resImg�� Visible0��� $  resImg�� � ��$B </Resources/Images/White/MainWindowHeader/RestoreBtnHover.png���� $��0��$��3��$$
 [Event Click]=[Action ExitSys]��$��0,0,5,0q�$��Right=�$	��Top=�$��42��$��19������ "  ��.R��� ._��� $��tgImg��$A��;/Resources/Images/White/MainWindowHeader/CloseBtnNormal.png�� P�  �@J�IsMouseOver . . ��� $  tgImg�� � ��$@ :/Resources/Images/White/MainWindowHeader/CloseBtnHover.png��P�  � �� . . ��� $  tgImg�� � ��$@ :/Resources/Images/White/MainWindowHeader/CloseBtnClick.png��� $! WindowHeader��$��2��$��0��$��3��$
��Auto�� �� $��30 ��� $
��Auto �.+�o� $��0��$��Center=�$��30��.@�m� #���  $��5,3,0,0q�$��Center=�$��Center=�.@�m� #���  $/
 )[Event Click]=[Action ShowChangPwdWindow]��y��� *VisibleChangePasswordItem+m� $" LoginLogoutMenuItem��$2
 ,[Event Click]=[Action LoginLogoutMenuAction]������ *LoginLogoutMenuHeader+m� #��� 	 $��0,3,0,0q�.@�m� #��� 
 $<
 6[Event Click] = [Action NewHedgeSpotForwardMenuAction]��y��� *VisibleWhenLogin+m� #���  $5
 /[Event Click] = [Action NewHedgeSwapMenuAction]��y��� *VisibleWhenLogin+m� #���  $9
 3[Event Click] = [Action BankCashTransferMenuAction]��y��� *VisibleWhenLogin+m� #���  $7
 1[Event Click] = [Action NewBankAccountActualCash]��y��� *VisibleWhenLogin+m� #���  $��0,3,0,0q�.@�m� #���  $?
 9[Event Click] = [Action BankCashTransferReportMenuAction]��y��� *VisibleWhenLogin+m� #���  $;
 5[Event Click] = [Action BankCashFlowReportMenuAction]��y��� *VisibleWhenLogin+m� #���  $>
 8[Event Click] = [Action OpenBankAccountActualCashReport]��y��� *VisibleWhenLogin+m� #���  $��0,3,0,0q�$
��Left=�$��Center=�.@�m� #���  y��� *VisibleWhenLogin+.@�m� $.
 ([Event Click]=[Action NewBusinessUnit()]������ .R��� $# 
Horizontal=�.+��� #���  �� #���  m� #���  $3
 -[Event Click]=[Action OpenBusinessUnitList()]��y��� *VisibleWhenLogin+m� #���  y��� *VisibleWhenLogin+.@�m� $&
  [Event Click]=[Action NewUser()]������ .R��� $# 
Horizontal=�.+��� #���  �� #���  m� #���  $+
 %[Event Click]=[Action OpenUserList()]��m� #���  y��� *VisibleWhenLogin+.@�m� $.
 ([Event Click]=[Action NewCounterparty()]������ .R��� $# 
Horizontal=�.+��� #���  �� #���  m� #���  $3
 -[Event Click]=[Action OpenCounterpartyList()]��m� #���  y��� *VisibleWhenLogin+.@�m� $3
 -[Event Click]=[Action NewCounterpartyGroup()]������ .R��� $# 
Horizontal=�.+��� #���  �� #���  m� #���  $8
 2[Event Click]=[Action OpenCounterpartyGroupList()]��m� #���  y��� *VisibleWhenLogin+.@�m� #���  $1
 +[Event Click]=[Action OpenNewBankAcctClick]��m� #���  $2
 ,[Event Click]=[Action OpenBankAcctListClick]��d� $��0��$
	 Hand��$$ 99��$��Center=�$��79��$��8��$��0,0,0,-0q�$��Bottom=�% �� *
IsExpanded+���� "  d�.R��� ._��� $��tgImg��$,��&/Resources/Images/White/ToggleDown.png�� P�  �@d�	IsChecked$
 TrueY�. ��� $  tgImg�� � ��$* $/Resources/Images/White/ToggleUp.png���� $��1�����#797979$��1q����#FFFFFFy��� *
IsExpanded+8&   ._��  �� $��20 ��� $��70 ��� $��10 ��� $��10 ��� .+��� $��1��$��0q����� *LoginoutMenuItems+���� .R�� $' 
Horizontal=����� .R��� $��50��$��5,3,5,0q�$��Center=�#���   y��� *VisibilityFlag+( �� *	ToolTip+)   $* Click��.+   $, MenuItemClick��.-   . �� *
MethodName+�� $��2��$4��./Resources/Theme/DarkBlue/Images/splitline.png���� $��4��$��0q����� *	MenuItems+���� .R�� $' 
Horizontal=����� .R��� $��50��$��5,3,5,0q�$��Center=�#���   y��� *VisibilityFlag+( �� *	ToolTip+)   $* Click��.+   $, MenuItemClick��.-   . �� *
MethodName+�� $��3��$��0��$��3��$��2�����#797979$��1,1,1,2q�/ . ���#FFFFFF._��� #��z���� -	   $��0��$��1��$��5��$	 SizeNS��
������ �� -
   $��4��$��1��$��5��$	 SizeNS��
������ �� -   $��1��$��0��$��3��$��5��$	 SizeWE��
������ �� -   $��1��$��2��$��3��$��5��$	 SizeWE��
������ �� -   $��0��$��0��$��5��$��5��$	 SizeNWSE��
������ �� -   $��0��$��2��$��5��$��5��$	 SizeNESW��
������ �� -   $��4��$��0��$��5��$��5��$	 SizeNESW��
������ �� -   $��4��$��2��$��5��$��5��$	 SizeNWSE��
������ 