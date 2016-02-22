/*
    Copyright 2016 Dominik Werner

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using MipsSim.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;

namespace MipsSim.View
{
    /// <summary>
    /// Interaction logic for MemoryEditView.xaml
    /// </summary>
    public partial class DataEditView : Window
    {
        public DataEditView()
        {
            InitializeComponent();
            KeyDown += RegisterEditView_KeyDown;    // catch keyboard to be able to close on ESC pressed

            InputBox.Focus();   // focus inputbox at beginning (for userfriendlyness)
            InputBox.Dispatcher.BeginInvoke(
            new Action(delegate
            {
                InputBox.SelectAll();
            }), System.Windows.Threading.DispatcherPriority.Input);
        }

        // handle keyboard input
        private void RegisterEditView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
            if (e.Key == Key.Enter)
            {
                ((DataEditViewModel)DataContext).Save();
                Close();
            }
        }

        // handle save button
        private void Button_SaveAndClose(object sender, RoutedEventArgs e)
        {
            ((DataEditViewModel)DataContext).Save();
            Close();
        }
    }
}
