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
using MipsSim.Model.Mips.Hardware;

namespace MipsSim.ViewModel.Commands
{
    /// <summary>
    /// Command to execute next instruction during debugging
    /// </summary>
    public class StepCommand : BaseCommand, ICommand
    {
        private SimWindowViewModel _vm;

        public StepCommand(SimWindowViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return Debugger.Instance.Mode == ProgramMode.Running;
        }

        public void Execute(object parameter)
        {
            _vm.ShowActiveLineInEditor(Cpu.Instance.PC.Data);
            if (Debugger.Instance.LineExists(Cpu.Instance.PC.Data)) // select line in editor
            {
                _vm.InstructionViewModel.SetActiveLine(Cpu.Instance.PC.Data.ToUnsignedInt());
            }
            else
            {
                _vm.InstructionViewModel.UnsetLines();
            }
            if(Debugger.Instance.Mode == ProgramMode.Running) Cpu.Instance.ProcessNextInstruction();  // execute next instruction
        }
    }
}
