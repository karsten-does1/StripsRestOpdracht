   M S B A M L   `   `   `  ����S  OStripsClientWPFStripView, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:StripsClientWPFStripViewStripsClientWPFStripView  5   	   )    #StripsClientWPFStripView.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:StripsClientWPFStripView     5   	     � Title$  
MainWindow��5      $	��400��6#   $	��600��60   .��5	      � $��10q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $	��100 �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0��6   $��Id��6-   $��Center=�6:   �� 5   
   -   5        $��	IdTextBox��5   /   $��0��6   $��1��6   $	��150��6@   $��5q�6L   �� 5   
   -   5        $��GetStripButton��5   .   $��0��6   $��2��6   $��	Get Strip��6D   $	��100��6X   $��5q�6d   6o   �� 5   
   $��1��6   $��0��6   $��Titel��6-   $��Center=�6=   �� 5   
   -   5        $��TitelTextBox��5   /   $��1��6   $��1��6    �� 
IsReadOnly$
 True��6C   $��5q�6U   �� 5"   
   $��2��6   $��0��6   $��Nr��6-   $��Center=�6:   �� 5#   
   -   5        $��	NrTextBox��5#   /   $��2��6   $��1��6   $
 True��6@   $��5q�6R   �� 5&   
   $��3��6   $��0��6   $��Reeks��6-   $��Center=�6=   �� 5'   
   -   5        $��ReeksTextBox��5'   /   $��3��6   $��1��6   $
 True��6C   $��5q�6U   �� 5*   
   $��4��6   $��0��6   $��
Uitgeverij��6-   $��Center=�6B   �� 5+   
   -   5        $��UitgeverijTextBox��5+   /   $��4��6   $��1��6   $
 True��6H   $��5q�6Z   �� 5.   
   $��5��6   $��0��6   $��Auteurs��6-   $	��Top=�6?   �� 5/   
   -   5        $��AuteursListBox��5/   /   $��5��6   $��1��6   $��5q�6E   50      51      