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

using System.Windows;
using System.Windows.Input;
using MipsSim.ViewModel;

namespace MipsSim.View
{
    /// <summary>
    /// Interaction logic for SimWindow.xaml
    /// </summary>
    public partial class SimWindow : Window
    {
        private SimWindowViewModel _vm;

        public SimWindow()
        {
            InitializeComponent();
            _vm = new SimWindowViewModel();
            DataContext = _vm;
            KeyDown += SimWindow_KeyDown;   // catch keyboard
        }

        // handle keyboard input
        private void SimWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.S:
                        if (_vm.SaveCommand.CanExecute(null)) _vm.SaveCommand.Execute(null);    // ctrl+s save command
                        break;
                    case Key.O:
                        if (_vm.OpenFileCommand.CanExecute(null)) _vm.OpenFileCommand.Execute(null);    // ctrl+o open command
                        break;
                    case Key.N:
                        if (_vm.NewFileCommand.CanExecute(null)) _vm.NewFileCommand.Execute(null);  // ctrl+n new command
                        break;
                }
            }
            else
            {
                switch (e.Key)
                {
                    case Key.F1:
                        if (_vm.OpenHelpCommand.CanExecute(null)) _vm.OpenHelpCommand.Execute(null);    // show help
                        break;
                    case Key.F5:
                        if (_vm.RunCommand.CanExecute(null)) _vm.RunCommand.Execute(null);  // run command
                        break;
                    case Key.F11:
                        if (_vm.StepCommand.CanExecute(null)) _vm.StepCommand.Execute(null);    // step command
                        break;
                    case Key.F8:
                        if (_vm.StopCommand.CanExecute(null)) _vm.StopCommand.Execute(null);    // stop execution
                        break;
                }
            }
        }
    }
}
