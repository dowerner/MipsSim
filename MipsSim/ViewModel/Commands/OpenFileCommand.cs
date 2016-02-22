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
using MipsSim.Model;

namespace MipsSim.ViewModel.Commands
{
    /// <summary>
    /// Command to open file from UI
    /// </summary>
    public class OpenFileCommand : BaseCommand, ICommand
    {
        private SimWindowViewModel _vm;

        public OpenFileCommand(SimWindowViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return _vm != null;
        }

        public void Execute(object parameter)
        {
            string path = SimWindowViewModel.GetPathFromFileOpenDialog(_vm.FolderPath);
            if (path == string.Empty) return;
            _vm.AssemblerCode = FileDirector.Instance.OpenAssemblerFile(path);
            _vm.WindowTitle = string.Format("{0}: {1}", _vm.WindowName, path);
            _vm.FilePath = path;
            _vm.TextEdited = false;
        }
    }
}
