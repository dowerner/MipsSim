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
using MipsSim.Model.Mips;

namespace MipsSim.ViewModel.Commands
{
    /// <summary>
    /// Command to run program
    /// </summary>
    public class RunCommand : BaseCommand, ICommand
    {
        private SimWindowViewModel _vm;

        public RunCommand(SimWindowViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return Debugger.Instance.Mode == ProgramMode.Stopped;
        }

        public void Execute(object parameter)
        {
            _vm.ActiveIndex = -1;   // diable active index
            _vm.ErrorIndex = -1;    // disable error index
            Compiler.Instance.CompileCode(_vm.AssemblerCode);   // run compiler
        }
    }
}
