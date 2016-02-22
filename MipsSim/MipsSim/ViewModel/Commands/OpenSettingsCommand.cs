﻿/*
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

using System.Windows.Input;

namespace MipsSim.ViewModel.Commands
{
    /// <summary>
    /// Command to open the program settings
    /// </summary>
    public class OpenSettingsCommand : BaseCommand, ICommand
    {
        private SimWindowViewModel _vm;

        public OpenSettingsCommand(SimWindowViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _vm.OpenSettings();
        }
    }
}
