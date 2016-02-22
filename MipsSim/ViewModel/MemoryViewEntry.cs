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

namespace MipsSim.ViewModel
{
    public class MemoryViewEntry : BaseViewModel
    {
        public uint Index { get; set; }
        public byte Data { get; set; }

        public void Update()
        {
            Data = Memory.Instance.Data[Index];
            NotifyPropertyChanged("Data");
        }

        public MemoryViewEntry(uint index, byte data)
        {
            Index = index;
            Data = data;
        }
    }
}
