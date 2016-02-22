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
using MipsSim.Model.Mips;

namespace MipsSim.ViewModel
{
    public class MemoryWordEntry : BaseViewModel
    {
        public bool[] Data
        {
            get
            {
                if (!_isUpToDate)
                {
                    byte[] temp = new byte[4];
                    /*temp[3] = Memory.Instance.Data[Index];
                    temp[2] = Memory.Instance.Data[Index + 1];
                    temp[1] = Memory.Instance.Data[Index + 2];
                    temp[0] = Memory.Instance.Data[Index + 3];*/

                    temp[0] = Memory.Instance.Data[Index];
                    temp[1] = Memory.Instance.Data[Index + 1];
                    temp[2] = Memory.Instance.Data[Index + 2];
                    temp[3] = Memory.Instance.Data[Index + 3];
                
                    _data = temp.ToMipsInstructionBits();
                    _isUpToDate = true;
                }
                return _data;
            }
            set
            {
                byte[] temp = value.ToMipsInstructionBytes();
                /*Memory.Instance.Data[Index] = temp[3];
                Memory.Instance.Data[Index + 1] = temp[2];
                Memory.Instance.Data[Index + 2] = temp[1];
                Memory.Instance.Data[Index + 3] = temp[0];*/

                Memory.Instance.Data[Index] = temp[0];
                Memory.Instance.Data[Index + 1] = temp[1];
                Memory.Instance.Data[Index + 2] = temp[2];
                Memory.Instance.Data[Index + 3] = temp[3];

                _isUpToDate = false;
            }
        }
        private bool[] _data;
        private bool _isUpToDate;

        public uint Index { get; private set; }

        public MemoryWordEntry(uint index)
        {
            Index = index;
            _data = new bool[32];
            _isUpToDate = false;
        }
    }
}
