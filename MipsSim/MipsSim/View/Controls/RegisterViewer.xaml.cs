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

using MipsSim.Model.Mips.Hardware;
using MipsSim.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MipsSim.View.Controls
{
    /// <summary>
    /// Interaction logic for RegisterViewer.xaml
    /// </summary>
    public partial class RegisterViewer : UserControl
    {
        private RegisterViewerViewModel _vm;

        public RegisterViewer()
        {
            InitializeComponent();
            DataContextChanged += RegisterViewer_DataContextChanged;
        }

        private void RegisterViewer_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is RegisterViewerViewModel) _vm = (RegisterViewerViewModel)e.NewValue;
        }

        private void EditRegisterClick(object sender, MouseButtonEventArgs e)
        {
            Register entry = null;

            if (((TextBlock)sender).DataContext is Register)
            {
                entry = (Register)((TextBlock)sender).DataContext;
            }
            else
            {
                if (((TextBlock)sender).Name == "PcBlock") entry = _vm.PC;
                if (((TextBlock)sender).Name == "HiBlock") entry = _vm.Hi;
                if (((TextBlock)sender).Name == "LoBlock") entry = _vm.Lo;
            }

            RegisterEditView editWindow = new RegisterEditView();
            editWindow.DataContext = new RegisterEditViewModel(entry, _vm, editWindow);
            editWindow.ShowDialog();
        }
    }
}
