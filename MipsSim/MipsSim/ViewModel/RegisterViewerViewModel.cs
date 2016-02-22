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

using System.Linq;
using MipsSim.Model.Mips.Hardware;
using System.Collections.ObjectModel;
using MipsSim.View.Controls;
using MipsSim.Model;
using System.IO;
using System.Windows;
using System.Windows.Input;
using MipsSim.ViewModel.Commands;

namespace MipsSim.ViewModel
{
    public class RegisterViewerViewModel : BaseViewModel
    {
        private const string PcName = "PC";
        private const string LoName = "lo";
        private const string HiName = "hi";

        public ICommand SaveCpuCommand { get; private set; }
        public ICommand LoadCpuCommand { get; private set; }

        /// <summary>
        /// Gets or sets the displaymode in the control
        /// </summary>
        public DataDisplayMode DisplayRegisterMode
        {
            get { return _displayRegisterMode; }
            set
            {
                _displayRegisterMode = value;
                NotifyPropertyChanged("DisplayRegisterMode");
            }
        }
        private DataDisplayMode _displayRegisterMode;

        public ObservableCollection<Register> RegisterData
        {
            get { return new ObservableCollection<Register>(Cpu.Instance.Registers.ToList()); }
        }

        public ObservableCollection<LineIndex> Indices { get; private set; }

        public Register PC { get { return Cpu.Instance.PC; } }

        public Register Lo { get { return Cpu.Instance.Lo; } }

        public Register Hi { get { return Cpu.Instance.Hi; } }

        public RegisterViewerViewModel()
        {
            Cpu.Instance.RegisterChanged += Instance_RegisterChanged;
            Indices = new ObservableCollection<LineIndex>();
            for (int i = 0; i < Cpu.Instance.Registers.Count(); i++) Indices.Add(new LineIndex(i));

            SaveCpuCommand = new SaveCpuCommand(this);
            LoadCpuCommand = new LoadCpuCommand(this);
        }

        public void AlertRegisterChange(Register register)
        {
            Instance_RegisterChanged(register);
        }

        private void Instance_RegisterChanged(Register register)
        {
            if (register.RegisterName == PcName) NotifyPropertyChanged("PC");
            else if (register.RegisterName == LoName) NotifyPropertyChanged("Lo");
            else if (register.RegisterName == HiName) NotifyPropertyChanged("Hi");
            else NotifyPropertyChanged("RegisterData");
        }

        public void SaveCurrentCpu()
        {
            string path = SimWindowViewModel.GetPathFromFileSaveDialog(Directory.GetCurrentDirectory(), ".rg", "Register Files (*.rg)|*.rg");
            if (path == string.Empty) return;
            FileDirector.Instance.SaveRegister(path);
        }

        public void LoadCpuFromFile()
        {
            try
            {
                string path = SimWindowViewModel.GetPathFromFileOpenDialog(Directory.GetCurrentDirectory(), ".rg", "Register Files (*.rg)|*.rg");
                FileDirector.Instance.LoadRegisterFromFile(path);

                foreach (Register register in RegisterData) AlertRegisterChange(register);
                AlertRegisterChange(Hi);
                AlertRegisterChange(Lo);
                AlertRegisterChange(PC);
            }
            catch
            {
                MessageBox.Show("Unable to load register file. Make sure you open a proper file.", "Loading error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
