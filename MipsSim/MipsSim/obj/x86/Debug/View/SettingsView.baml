   M S B A M L   `   `   `  ����B  >MipsSim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:MipsSim.View.ConverterMipsSim.View.Converter  5   	       MipsSim.View.SettingsView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   :	converter$clr-namespace:MipsSim.View.Converter     5   	     � Title$  Settings��5   	   $	��220��6   $	��300��6'    � WindowStyle$ 
ToolWindow=�5   	    � 	Resources 5      3   -MipsSim.View.Converter.BytesToKBytesConverter   BytesToKBytesConverter��%   &	          5   
   6+   5	      .��5
      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��40 �6   �� 5      $��* �6   �� 5      $��44 �6    � ColumnDefinitions 5   
   �� 5      $��90 �6   �� 5      $��* �6   �� 5      $��90 �6   .+�5   
   �� $��10q�6   $��3��6    $"Configure the Mips Simulator here.5      �� 5   
   $��Memory size:��6    �� TextAlignment$ Right=�6(   $��0��6>   $��10q�6N   $��1��6Z   �� 5   
   $��(in KBytes)��6   $��2��6'   $��10q�67   $��1��6C   �� 5   
   $��5q�6   $3��-Max. memory size 256 MBytes (262'144 KBytes).��6   $
��Wrap=�6T   $��0��6h   $��2��6x   $��3��6�   �� 5   
   $��8q�6�   $��1��6�   $��1��6�   ��6   �� *Bytes+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=� �� 	Converter# [  6�   �� 5   
   $��1��6   $��2��6N   $��Right=�6b   $	��100��6~   $��3��6�   $��8q�6�   $��save and close��6�   ��6!   �� *SettingsSaveCloseCommand+6�   5      5      