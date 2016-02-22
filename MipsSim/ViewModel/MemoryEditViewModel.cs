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
using MipsSim.View;
using MipsSim.View.Converter;
using System.Windows;

namespace MipsSim.ViewModel
{
    /// <summary>
    /// View model for memory edit view
    /// </summary>
    public class MemoryEditViewModel
    {
        public string Title { get; set; }   // title of the window containing the name of the edited memroy cell
        public int Index { get; set; }  // Index of the edited memory cell
        public byte Data { get; set; }  // Data loaded from the memory
        public DataDisplayMode DisplayMemoryMode    // mode in which data is displayed
        {
            get { return _vm.DisplayMemoryMode; }
            set { _vm.DisplayMemoryMode = value; }
        }

        private MemoryEditView _view;   // handle of view
        private MemoryViewerViewModel _vm;  // get handle of underlying memory view model

        public MemoryEditViewModel(int index, byte data, MemoryViewerViewModel vm, MemoryEditView view)
        {
            Index = index;
            Data = data;
            _vm = vm;
            _view = view;
            Title = string.Format("Memory Entry: {0}", index);
            DataModeConverter.ConversionFailed += DataModeConverter_ConversionFailed;   // catch if data conversion fails
        }

        // Display error message if entered data was flawed
        private void DataModeConverter_ConversionFailed()
        {
            MessageBox.Show("The data you entered is either not recognized as allowed format (binary [0001000], decimal [235], hex [0x3A]) or is not in the allowed range (0,..,255).", "Edit failed.", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Save entered data back to memory
        /// </summary>
        public void Save()
        {
            _view.SaveButton.Focus();
            Memory.Instance.Data[Index] = Data;
            _vm.Renew(true);
            DataModeConverter.ConversionFailed -= DataModeConverter_ConversionFailed;
        }
    }
}
