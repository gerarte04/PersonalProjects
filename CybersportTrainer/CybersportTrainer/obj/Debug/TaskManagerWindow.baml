   M S B A M L   `   `   `  ����L  HCybersportTrainer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:CybersportTrainerCybersportTrainer  5   	   )    #CybersportTrainer.TaskManagerWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:CybersportTrainer     5   	     � Title$  TaskManagerWindow��5   	   $	��720��5	   	   $
��1280��6   
�������$��1q�5
      ���#1c1c1c � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�6    J� Opacity$ 1Q�62    � 	Resources 5       �� 
TargetType   
text_style �� Property �� Value  	sv_circle �� CornerRadius  sv_line  button_style  radio_b	 d� 	IsChecked��%�  &	        &	 r    &	 �    &	 A    &	 �    �� 5   
   " ��6   .�5      ��  � ��
 �����6+   �� 5       � ��$ Hermes)�6+   5      �� 5   
   
�������$��5q�67   $��40��5      $��40��6   $ 20��6(   $
��Left=�5      5      �� 5   
   
�������$��7q�65   $��17��5      $��7��6   $
��Left=�5      6,   �� 5   
   " ��6%   .�5      ��  � �� �#1c1c1c6+   �� 5       � ��
 �����6,   �� 5        � ��$ 1q�60   5!      	� 5#   
   $��Huilo blyat��6&   $	 FalseY�6<   
�������$	��100��5%      $	��100��6"   
�������$��5q�5&   +   5(      
 � Triggers
 5*      :� 5,   
    :� RoutedEvent$ Loaded��6   .[�5-      �� .��5.      ��  g� Duration$ 
00:00:00:3D�6    �� TargetProperty$ Opacity�5/      .��50      W�  W� From$ 0X�6&   	 W� To$ 1X�6/   51      52      53      .��56      ]� .+�57   
   � $	��Top=�6    � RowDefinitions 58      �� 59      $
��Auto �6    �� 5:      $
��Auto �6    �� 5;      $
��Auto �6    �� 5<      $
��Auto �6    .+�5?      �� -   5         ��Name$ title_tb��5?      $<��6Добро пожаловать в Cybersport Trainer!��6*   $��
100,20,0,0q�6X   #��[  6l   $��50(�6�   $��0��6�   �� 5@      -   5        $ subtitle_tb��5@      $����Давайте подберем наиболее подходящие для вас параметры учета и тренировки вашей статистики.��6-   $��	50,10,0,0q�6�   #��[  6�   $��30(�6�   $��1��6�   � 5A      -   5         �Name$ stage_viewer��5A      $��50, 30, 0, 0q�6)   $��2��6?    �� BitmapScalingMode$ HighQuality=�6L   5C       � 5E      -   5          �Name$ stage_frame��5E      $��Stretch=�6)   $��	55,30,0,0q�5F        � NavigationUIVisibility$ Hidden=�5G      $��3��60   5H      �� 5J   
   -   5         ��Name$ back_b��5J      #��[ 6!   $��30��5K      $
��Left=�5L      $��Bottom=�6,   $��3��5M      $��	30,0,0,30q�6   $y�Hidden=�61   $��Bottom=�5N      ._�5O      ��  �� Orientation$ 
Horizontal=�6   .+�5P      �� $/��)pack://application:,,,/res/arrow_left.png��6   �� 5Q      $��
Назад��6   #��[  6)   $��24(�5R      $��0,1,0,0q�6*   $��Center=�5S      �� 5T      $��4��6   5U      5V      �� 5X   
   -   5        $ next_b��5X      #��[ 6!   $��30��5Y      $��Right=�5Z      $��Bottom=�6-   $��3��5[      $��	0,30,30,0q�6   $��Bottom=�5\      ._�5]      �� $ 
Horizontal=�6   .+�5^      �� -   5        $ 	next_b_tb��5a      $��
Далее��5^      #��[  6)   $��24(�5_      $��4,1,0,0q�6*   $��Center=�5`      5a      �� 5b      $/��)pack://application:,,,/res/arrow_left.png��6    � LayoutTransform 5c      �� 5d       �� Angle$	 180Q�6*   5f      5g      5h      5i      5j      