   M S B A M L   `   `   `  ����L  HCybersportTrainer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:CybersportTrainerCybersportTrainer  5   	   "    CybersportTrainer.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:CybersportTrainer     5   	   -   5          � Title$  
MainWindow��5   	    � WindowStyle$
 None=�6    � WindowState$ 	Maximized=�6/   $
��1080��5	   	   $
��1920��6    � 	Resources 5         b_style �� 
TargetType �� Property �� Value �� Triggers P� Property	 P� Value��%  &	        �� 5   
   " ��6    .�5      ��  � �� �#1c1c1c6+   �� 5       � ��
 �����6,   �� 5       � ��$ 2q�60    5      P� 5       �@J�IsMouseOver	 . . �5      ��  �@J�Opacity$	 0.7Q�60   5      5      5      .��5      � .+�5   
   �� -   5        
 ��Name$
 
back_image��5      $1��+pack://application:,,,/res/background_1.jpg��6$   $��Center=�6Y   $��Center=�6v   �� 5   
   -   5         ��Name$ dark_overlay��5      ���#121212 J� Opacity$
 0.95Q�6<    � 5   
   -   5          �Name$ central_frame��5        � NavigationUIVisibility$ Hidden=�6'   �� 5   
   -   5        $ close_b��5      $��60��6"   $��60��6-   $��0,20,11.5,0q�69   #��[  6N   $��Right=�6�   $	��Top=�6�    � Cursor$
 Hand��6�   ._�5      �� $*��$pack://application:,,,/res/cross.png��6   $��60��6B   $��0,0,0,0q�6N    �� BitmapScalingMode$ HighQuality=�6_   5       5!      5"      