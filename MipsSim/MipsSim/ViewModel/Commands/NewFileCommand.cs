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

namespace MipsSim.ViewModel.Commands
{
    /// <summary>
    /// Command to create new file
    /// </summary>
    public class NewFileCommand : BaseCommand, ICommand
    {
        private SimWindowViewModel _vm;

        public NewFileCommand(SimWindowViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_vm.TextEdited) // check if current file is save or not. Prompt user if he wants to save first.
            {
                var result = MessageBox.Show(string.Format("Do you want to save the currently opened file {0} first?", _vm.FilePath), "save current file", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Cancel) return;
                if (result == MessageBoxResult.Yes) _vm.Save(_vm.FilePath);
            }
            _vm.FilePath = string.Empty;
            _vm.AssemblerCode = string.Empty;
            _vm.WindowTitle = _vm.WindowName;
        }
    }
}
