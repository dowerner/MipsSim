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
using System.Windows.Input;
using MipsSim.ViewModel.Commands;

namespace MipsSim.ViewModel
{
    public class SettingsViewViewModel : BaseViewModel
    {
        public delegate void OnSaveAndCloseHandler();
        public event OnSaveAndCloseHandler OnSaveAndClose;

        public ICommand SettingsSaveCloseCommand { get; private set; }

        /// <summary>
        /// Gets or sets the kilobytes
        /// </summary>
        public uint Bytes
        {
            get { return _bytes; }
            set
            {
                _bytes = value;
                NotifyPropertyChanged("Bytes");
            }
        }
        private uint _bytes;

        public uint InitialBytes { get; private set; }

        public void SaveAndCloseCalled()
        {
            if (OnSaveAndClose != null) OnSaveAndClose();
        }

        public SettingsViewViewModel()
        {
            Bytes = (uint)Memory.Instance.Data.Length;
            InitialBytes = Bytes;
            SettingsSaveCloseCommand = new SettingsSaveCloseCommand(this);
        }
    }
}
