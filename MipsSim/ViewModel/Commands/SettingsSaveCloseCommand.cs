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

using System.Windows.Input;
using MipsSim.Model.Mips.Hardware;

namespace MipsSim.ViewModel.Commands
{
    /// <summary>
    /// Command to change memory size and save the settings
    /// </summary>
    public class SettingsSaveCloseCommand : BaseCommand, ICommand
    {
        private SettingsViewViewModel _vm;

        private const uint min = 1024;  // min allowed memory size
        private const uint max = 268435456; // max allowed memory size

        public SettingsSaveCloseCommand(SettingsViewViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return (min < _vm.Bytes && _vm.Bytes < max && _vm.Bytes != _vm.InitialBytes);
        }

        public void Execute(object parameter)
        {
            Memory.Instance.SetNewSizeInBytes(_vm.Bytes);   // set new memory size
            _vm.SaveAndCloseCalled();
        }
    }
}
