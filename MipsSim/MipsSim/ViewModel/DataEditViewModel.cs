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
using MipsSim.Model.Mips;

namespace MipsSim.ViewModel
{
    /// <summary>
    /// ViewModel for the dataEditView
    /// </summary>
    public class DataEditViewModel
    {
        public string Title { get; set; }   // title of the window
        public bool[] Data { get; set; }    // data of edited memory word
        public DataDisplayMode DisplayMemoryMode
        {
            get { return _vm.DisplayMemoryMode; }
            set { _vm.DisplayMemoryMode = value; }
        }

        private DataEditView _view; // store handle of view
        private MemoryViewerViewModel _vm;  // store viewmodel of memory view
        private MemoryWordEntry _data;  // store data

        public DataEditViewModel(MemoryWordEntry data, MemoryViewerViewModel vm, DataEditView view)
        {
            _data = data;
            Data = _data.Data;
            _vm = vm;
            _view = view;
            Title = string.Format("Memory Word Entry: {0}", _data.Index);   // set title
            BoolDataModeConverter.BoolConversionFailed += DataModeConverter_ConversionFailed;
        }

        // alert on conversion error
        private void DataModeConverter_ConversionFailed()
        {
            MessageBox.Show("The data you entered is either not recognized as allowed format (binary [0001000], decimal [235], hex [0x3A]) or is not in the allowed range (0,..,2^32-1).", "Edit failed.", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Save entered data back to the memory and renew the memory view
        /// </summary>
        public void Save()
        {
            _view.SaveButton.Focus();

            byte[] temp = Data.ToMipsInstructionBytes();
            /*Memory.Instance.Data[_data.Index] = temp[3];
            Memory.Instance.Data[_data.Index + 1] = temp[2];
            Memory.Instance.Data[_data.Index + 2] = temp[1];
            Memory.Instance.Data[_data.Index + 3] = temp[0];*/

            Memory.Instance.Data[_data.Index] = temp[0];
            Memory.Instance.Data[_data.Index + 1] = temp[1];
            Memory.Instance.Data[_data.Index + 2] = temp[2];
            Memory.Instance.Data[_data.Index + 3] = temp[3];
            
            _vm.Renew(true);
            BoolDataModeConverter.BoolConversionFailed -= DataModeConverter_ConversionFailed;
        }
    }
}
