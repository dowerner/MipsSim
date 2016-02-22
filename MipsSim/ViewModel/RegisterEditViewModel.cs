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
    public class RegisterEditViewModel
    {
        public string Title { get; set; }
        public bool[] Data { get; set; }
        public DataDisplayMode DisplayRegisterMode
        {
            get { return _vm.DisplayRegisterMode; }
            set { _vm.DisplayRegisterMode = value; }
        }

        private RegisterEditView _view;
        private RegisterViewerViewModel _vm;
        private Register _register;

        public RegisterEditViewModel(Register register, RegisterViewerViewModel vm, RegisterEditView view)
        {
            _register = register;
            Data = _register.Data;
            _vm = vm;
            _view = view;
            Title = string.Format("Register Entry: {0}", _register.RegisterName);
            BoolDataModeConverter.BoolConversionFailed += DataModeConverter_ConversionFailed;
        }

        private void DataModeConverter_ConversionFailed()
        {
            MessageBox.Show("The data you entered is either not recognized as allowed format (binary [0001000], decimal [235], hex [0x3A]) or is not in the allowed range (0,..,2^32-1).", "Edit failed.", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void Save()
        {
            _view.SaveButton.Focus();

            if (_register.RegisterName.Contains("$")) Cpu.Instance.Registers[_register.Index].SetRegData(Data);
            else
            {
                if (_register.RegisterName == "PC") Cpu.Instance.PC.SetRegData(Data);
                if (_register.RegisterName == "hi") Cpu.Instance.Hi.SetRegData(Data);
                if (_register.RegisterName == "lo") Cpu.Instance.Lo.SetRegData(Data);
            }
            _vm.AlertRegisterChange(_register);
            BoolDataModeConverter.BoolConversionFailed -= DataModeConverter_ConversionFailed;
        }
    }
}
