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
using MipsSim.Model.Mips;

namespace MipsSim.ViewModel.Commands
{
    /// <summary>
    /// Command to stop execution
    /// </summary>
    public class StopCommand : BaseCommand, ICommand
    {
        private SimWindowViewModel _vm; // store view model to access its methods

        public StopCommand(SimWindowViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return Debugger.Instance.Mode == ProgramMode.Running;
        }

        public void Execute(object parameter)
        {
            Debugger.Instance.StopProgramm();   // stop program
            _vm.InstructionViewModel.UnsetLines();  // remove all marks in the editor
            _vm.DataViewModel.UnsetLines(); // remove all marks in the memory viewer
            _vm.ActiveIndex = -1;   // unset active index
        }
    }
}
