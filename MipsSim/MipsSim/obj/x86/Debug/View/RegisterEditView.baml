   M S B A M L   `   `   `  ����B  >MipsSim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:MipsSim.View.ConverterMipsSim.View.Converter  5   	   #    MipsSim.View.RegisterEditView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   :	converter$clr-namespace:MipsSim.View.Converter     5   	   -   5        $��RegisterView��5   	   $	��135��5   !   $	��300��6.     � WindowStyle$  
ToolWindow=�5   	    � Title 5   	   �� *Title+ � 	Resources 5      2   ,MipsSim.View.Converter.BoolDataModeConverter   BoolDataModeConverter��%   &	          5	   
   6*   5
      .��5      � $��10q�6    � ColumnDefinitions 5   
   �� 5      $	��150 �6   �� 5      $��20 �6   �� 5      $��80 �6    � RowDefinitions 5   
   �� 5      $��2* �6   �� 5      $��24 �6   �� 5      $��* �6   .+�5   
   �� $��3��6   $
��Wrap=�6(   +)Enter the new data in the current format.6g   �� 5   
   -   5        $��InputBox��5      $��1��6"   $��2��6/    �� TextWrapping$
 Wrap=�6@   .��6T   ��5      `� 5       `� 	Converter# [  6   .1�5      ��  �� Path$
 Data�6   �� 5       �� ElementName$ RegisterView��6   $% DataContext.DisplayRegisterMode�69   5      .��5      5      �� 5   
   -   5        $��
SaveButton��5      $
��save��6?   $��2��6N   $��1��6^   5       5!      