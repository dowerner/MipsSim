   M S B A M L   `   `   `  ����B  >MipsSim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:MipsSim.View.ConverterMipsSim.View.Converter  5      *    $MipsSim.View.Controls.RegisterViewer   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :	converter$clr-namespace:MipsSim.View.Converter     5      -   5        $��RegisterView��5        � 	Resources  5
      2   ,MipsSim.View.Converter.BoolDataModeConverter   BoolDataModeConverter��%   &	          5   
   6*   5      .��5      �  � ColumnDefinitions 5   
   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $��3* �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��Center=�6$   $��Index��6A   �� 5   
   $��1��6   $��Center=�6$   $
��Name��6A   �� 5   
   $��2��6   $��Center=�6$   $
��Data��6A   �� 5   
   
�������$��1��6%   $��0��62   $��2��6B   �� 5   
   -   5         ��Name$ Viewer��5   8   $��1��6   $��3��6$   $
��Auto=�6H   .��5      �  � ContextMenu 5      �� 5      .@�5       m� $ ��Save current CPU Registers��6F    m� Command 6#   �� *SaveCpuCommand+6F   m� 5!      $%��Load CPU Registers from file...��6F    6#   �� *LoadCpuCommand+6F   5"       5$      �� 5%      $��40 �6'   �� 5&      $��40 �6'   �� 5'      $��* �6'   .+�5)      �� $��0��6   
�������{�.  $��Right=�5,      $��Disabled=�5-      ��5)   /   �� *	Indices+��5.      �� 5/      .R�50      ��  �� TextAlignment$ Right=�53   (   $��Right=�54   (   ��50   (   �� *Index+��51   (   �� *
Background+��52   (   �� *
Foreground+55      56      58      �� 59      $��1��6   
�������{�.  $��Right=�5<      $��Disabled=�5=      ��59   /   �� *RegisterData+��5>      �� 5?      .R�5@      �� $ Center=�5A   (   $��Center=�5B   (   ��5@   (   �� *RegisterName+5C      5D      5F      �� 5G      $��2��6   $��Stretch=�5H      $��Disabled=�5I      ��5G   /   �� *RegisterData+��5J      �� 5K      .R�5L      �� -   5        $��0,0,5,0q�5L   F   $��Right=�6W   $ Right=�6s   ��5M   "   `� 5N   &    `� 	Converter# [  63   .1�5O   *   ��  �� Path$
 Data�62   	 �� UpdateSourceTrigger$	 PropertyChanged=�6>   �� 5P   *   
 �� ElementName$
 RegisterView��62   $% DataContext.DisplayRegisterMode�6M   5Q   '   5S      5T      5V      5W      5X      � 5Y   
   $��10q�6   $��2��6   $��3��6(    5Z      �� 5[      $��50 �6#   �� 5\      $��* �6#   .+�5^      �� $��0��6   .+�5_      �� $��PC��6   �� 5`      $��lo��6   �� 5a      $��hi��6   5b      �� 5c      $��1��6   .+�5d      �� -   5        $��PcBlock��5d      ��5e      `� 5f      # [  6'   .1�5g   *   �� $ PC.Data�62   �� 5h   *   $ DisplayRegisterMode�62   5i   '   5k      �� 5l      -   5        $��LoBlock��5l      ��5m      `� 5n      # [  6'   .1�5o   *   �� $ Lo.Data�62   �� 5p   *   $ DisplayRegisterMode�62   5q   '   5s      �� 5t      -   5        $��HiBlock��5t      ��5u      `� 5v      # [  6'   .1�5w   *   �� $ Hi.Data�62   �� 5x   *   $ DisplayRegisterMode�62   5y   '   5{      5|      5}      5~      5      