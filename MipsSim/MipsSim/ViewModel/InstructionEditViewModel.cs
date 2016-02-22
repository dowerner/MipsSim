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

using MipsSim.Model.Mips;
using MipsSim.Model.Mips.Hardware;
using MipsSim.View;
using MipsSim.View.Converter;
using System.Windows;

namespace MipsSim.ViewModel
{
    /// <summary>
    /// Viewmodel for instruction edit window
    /// </summary>
    public class InstructionEditViewModel : BaseViewModel
    {
        public string Title { get; set; }   // window title
        public bool[] Data { get; set; }    // instruction data from memory

        public bool[] Op    // Op-Code part
        {
            get { return _op; }
            set {_op = value.GetRange(value.Length - Compiler.OpCodeLength, Compiler.OpCodeLength); }
        }
        private bool[] _op;

        public bool[] Rs    // Rs-Code part
        {
            get { return _rs; }
            set { _rs = value.GetRange(value.Length - Compiler.RsLength, Compiler.RsLength); }
        }
        private bool[] _rs;

        public bool[] Rt    // Rt-Code part
        {
            get { return _rt; }
            set { _rt = value.GetRange(value.Length - Compiler.RtLength, Compiler.RtLength); }
        }
        private bool[] _rt;

        public bool[] Immediate // Immidiate-code part
        {
            get { return _immediate; }
            set { _immediate = value.GetRange(value.Length - Compiler.ImmidiateLength, Compiler.ImmidiateLength); }
        }
        private bool[] _immediate;

        public bool[] Target    // Target-code part
        {
            get { return _target; }
            set { _target = value.GetRange(value.Length - Compiler.TargetLength, Compiler.TargetLength); }
        }
        private bool[] _target;

        public bool[] Rd    // Rd-code part
        {
            get { return _rd; }
            set { _rd = value.GetRange(value.Length - Compiler.RdLength, Compiler.RdLength); }
        }
        private bool[] _rd;

        public bool[] Shamt // Shamt-code part
        {
            get { return _shamt; }
            set { _shamt = value.GetRange(value.Length - Compiler.ShamtLength, Compiler.ShamtLength); }
        }
        private bool[] _shamt;

        public bool[] Funct // funct-code part
        {
            get { return _funct; }
            set { _funct = value.GetRange(value.Length - Compiler.FunctLength, Compiler.FunctLength); }
        }
        private bool[] _funct;

        // Visibility properties to switch mode between different instruction types
        public Visibility RTypeVisibility { get; private set; }
        public Visibility ITypeVisibility { get; private set; }
        public Visibility JTypeVisibility { get; private set; }

        /// <summary>
        /// Defines mode in which data is displayed
        /// </summary>
        public DataDisplayMode DisplayMemoryMode
        {
            get { return _vm.DisplayMemoryMode; }
            set { _vm.DisplayMemoryMode = value; }
        }

        public InstructionEditView View { get; set; }   // handle of instruction edit window

        private InstructionType _type;

        public MemoryViewerViewModel _vm;   // store memory viewer view model (for which this class serves as view model)
        private MemoryWordEntry _word;

        public InstructionEditViewModel(MemoryWordEntry word, MemoryViewerViewModel vm)
        {
            _word = word;   // store data
            Data = _word.Data;
            _vm = vm;
            BoolDataModeConverter.BoolConversionFailed += DataModeConverter_ConversionFailed;   // catch if conversion fails

            Op = new bool[Compiler.OpCodeLength];   // fill instruciton parts
            Rs = new bool[Compiler.RsLength];
            Rt = new bool[Compiler.RtLength];
            Immediate = new bool[Compiler.ImmidiateLength];
            Target = new bool[Compiler.TargetLength];
            Rd = new bool[Compiler.RdLength];
            Shamt = new bool[Compiler.ShamtLength];
            Funct = new bool[Compiler.FunctLength];

            RTypeVisibility = Visibility.Collapsed;
            ITypeVisibility = Visibility.Collapsed;
            JTypeVisibility = Visibility.Collapsed;

            parse();

            Title = string.Format("Instruction: {0}", _word.Index);

            switch (_type)  // decide which title to show
            {
                case InstructionType.RType:
                    Title = "R-Type-" + Title;
                    break;
                case InstructionType.IType:
                    Title = "I-Type-" + Title;
                    break;
                case InstructionType.JType:
                    Title = "J-Type-" + Title;
                    break;
            }
        }

        private void parse()
        {
            Op = Data.GetRange(0, Compiler.OpCodeLength);

            if (Op.ToInt() == 0) _type = InstructionType.RType;
            else if (Op.ToBitString() == AssemblerInstructions.JOpCode
                || Op.ToBitString() == AssemblerInstructions.JalOpCode) _type = InstructionType.JType;
            else _type = InstructionType.IType;

            switch (_type)  // fill view
            {
                case InstructionType.RType:
                    Rs = Data.GetRange(6, Compiler.RsLength);
                    Rt = Data.GetRange(11, Compiler.RtLength);
                    Rd = Data.GetRange(16, Compiler.RdLength);
                    Shamt = Data.GetRange(21, Compiler.ShamtLength);
                    Funct = Data.GetRange(26, Compiler.FunctLength);
                    RTypeVisibility = Visibility.Visible;   // show correct form
                    NotifyPropertyChanged("RTypeVisibility");
                    break;
                case InstructionType.IType:
                    Rs = Data.GetRange(6, Compiler.RsLength);
                    Rt = Data.GetRange(11, Compiler.RtLength);
                    Immediate = Data.GetRange(16, Compiler.ImmidiateLength);
                    ITypeVisibility = Visibility.Visible;   // show correct form
                    NotifyPropertyChanged("ITypeVisibility");
                    break;
                case InstructionType.JType:
                    Target = Data.GetRange(6, Compiler.TargetLength);
                    JTypeVisibility = Visibility.Visible;   // show correct form
                    NotifyPropertyChanged("JTypeVisibility");
                    break;
            }
        }

        // display error message if provided data was flawed
        private void DataModeConverter_ConversionFailed()
        {
            MessageBox.Show("The data you entered is either not recognized as allowed format (binary [0001000], decimal [235], hex [0x3A]).", "Edit failed.", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Saves data back to memory
        /// </summary>
        public void Save()
        {
            View.SaveButton.Focus();

            bool[] instruction = Op;

            switch (_type)
            {
                case InstructionType.RType:
                    instruction = instruction.JoinArray(Rs).JoinArray(Rt).JoinArray(Rd).JoinArray(Shamt).JoinArray(Funct);
                    break;
                case InstructionType.IType:
                    instruction = instruction.JoinArray(Rs).JoinArray(Rt).JoinArray(Immediate);
                    break;
                case InstructionType.JType:
                    instruction = instruction.JoinArray(Target);
                    break;
            }

            byte[] temp = instruction.ToMipsInstructionBytes();

            /*Memory.Instance.Data[_word.Index] = temp[3];
            Memory.Instance.Data[_word.Index + 1] = temp[2];
            Memory.Instance.Data[_word.Index + 2] = temp[1];
            Memory.Instance.Data[_word.Index + 3] = temp[0];*/

            Memory.Instance.Data[_word.Index] = temp[0];
            Memory.Instance.Data[_word.Index + 1] = temp[1];
            Memory.Instance.Data[_word.Index + 2] = temp[2];
            Memory.Instance.Data[_word.Index + 3] = temp[3];

            _vm.Renew(true);

            BoolDataModeConverter.BoolConversionFailed -= DataModeConverter_ConversionFailed;
        }
    }
}
