   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   	   B >MipsSim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:MipsSim.View.ControlsMipsSim.View.Controls 5   	   5clr-namespace:MipsSim.ViewModelMipsSim.ViewModel 5   	      MipsSim.View.SimWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   4sys&clr-namespace:System;assembly=mscorlib    5   	   6controls#clr-namespace:MipsSim.View.Controls    5   	   ,vmclr-namespace:MipsSim.ViewModel    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          �Name$  SimView��5   	   $	��848��5   '   $
��1060��64    � Title 6	   �� *WindowTitle+ � 	Resources 5	       S� 
MethodName S� 
ObjectType   System.Enum   DataDisplayMode S� MethodParameters M� TypeName��%}   &	        S� 5
   
   $ 	GetValues��6   "  5       5      M� 5      $ vm:DataDisplayMode��6   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $��32 �6   �� 5      $��32 �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $	��10* �6   �� 5      $��5 �6   �� 5      $��6* �6   �� 5      $��5 �6   �� 5      $��6* �6   �� 5      $��5 �6   �� 5      $	��10* �6   �� 5       $��5 �6   �� 5!      $��* �6   .+�5#   
   o� $��0��6   $��9��6   .@�5$      m� $��3q�6   $
��File��6"   .@�5%      m� $��New (Ctrl+N)��6   	 m� Command	 61   �� *NewFileCommand+m� 5&      $��Open (Ctrl+O)��6   	 62   �� *OpenFileCommand+m� 5'      $��Save (Ctrl+S)��6   	 62   �� *SaveCommand+m� 5(      $��
Save As...��6   	 6/   �� *SaveAsCommand+m� 5)      $��Save Current Memory��6   	 68   �� *SaveMemoryCommand+m� 5*      $��Load memory from file...��6   	 6=   �� *LoadMemoryCommand+m� 5+      $ ��Save Current CPU Registers��6   	 6?   �� *SaveCpuCommand+m� 5,      $%��Load CPU Registers from file...��6   	 6D   �� *LoadCpuCommand+5-      �� 5.      $��20��6   $��20��6!   
 � ToolTip$
 run code (F5)��6-   ��6E   �� *
RunCommand+��5/      � 50       � Points �    z @KL ��� 
��� � �
��� � �6D   52      �� 53      $��20��6   $��20��6!   $��	-10,0,0,0q�54      $
 stop execution (F8)��6(   ��55      �� *StopCommand+��56      �� 57      
���  ��$��10��6+   $��10��66   59      �� 5:      $��20��6   $��20��6!   $��	-10,0,0,0q�5;      $
 continue (F11)��6(   ��5<      �� *StepCommand+��5=      �� 5>       �� Orientation$ 
Horizontal=�6!   .+�5?      �� 
��� � �$��3��61   $��12��6;   � 5@      $��3,1,0,0q�6"    �    z @KL ��� 
��� � �
��� � �6Y   5A      5C      �� 5D      $��data display mode��6   $��	0,0,-15,0q�60   �� 5E      $��68��5G      ��5E      ��  �� Source# [   �� SelectedItem 5F      �� *DisplayMode+5G      m� 5H      $��3q�6   $��Settings��6"   	 64   �� *OpenSettingsCommand+�� 5I      $��Reset Memory On Compile��6B    d� 	IsChecked 6   �� *ResetMemoryOnCompile+6B   �� 5J      $ ��Reset Registers On Compile��6D    6   �� *ResetRegisterOnCompile+6D   m� 5K      $��3q�6   $��?��6"   .@�5L      m� $��	Help (F1)��6   	 6.   �� *OpenHelpCommand+m� 5M      $��About��6   	 6*   �� *AboutCommand+5N      5O      &   MipsSim.View.Controls.CodeEditor  5P   
   $��1��66   $��2��6C    � DataContext 6   �� 6C   �� 5Q   
   $��1��6   $��2��6!   $��3��61   
�������6E   �� 5R   
   $��Center=�6   $��Center=�5S      $��Memory View��5T      $��16(�5U      $��1��5V      $��2��5W      $��3��5X      � 5Y   
   $��2��6   $��2��6    5Z      �� 5[      $��32 �6    �� 5\      $��* �6    .+�5^      �� $��Center=�6   $��Center=�5_      $��Instruction Section��5`      $��14(�5a      (  "MipsSim.View.Controls.MemoryViewer  5b      $��1��6$    61   �� *InstructionViewModel+5c      � 5d   
   $��2��6   $��4��6    5e      �� 5f      $��32 �6    �� 5g      $��* �6    .+�5i      �� $��Center=�6   $��Center=�5j      $��Data Section��5k      $��14(�5l        5m      $��1��6$    61   �� *DataViewModel+5n      �� 5o   
   $��1��6   $��6��6!   
�������61   �� 5p   
   $��Center=�6   $��1��5q      $��6��6!   $��Center=�5r      $��Register View��5s      $��16(�5t      *  $MipsSim.View.Controls.RegisterViewer  5u   
   $��6��6L   $��2��6\    6"   �� *RegisterViewModel+6\   �� 5v   
   $��Right=�6   $��Stretch=�5w      
�������$��1��5x      $��1��6#   $��2��60    �� ResizeBehavior$ PreviousAndNext=�6B   $��5��5y      �� 5z   
   $��Right=�6   $��Stretch=�5{      
�������$��3��5|      $��2��6#   $ PreviousAndNext=�60   $��5��5}      �� 5~   
   $��Right=�6   $��Stretch=�5      
�������$��5��5�      $��1��6#   $��2��60   $ PreviousAndNext=�6B   $��5��5�      �� 5�   
   $��Right=�6   $��Stretch=�5�      
�������$��7��5�      $��1��6#   $��2��60   $ PreviousAndNext=�6B   $��5��5�      5�      5�      